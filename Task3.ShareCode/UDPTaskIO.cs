using System;
using System.Net.Sockets;

using Task1.TasksIO;
using System.Collections.Concurrent;
using System.Threading;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;

namespace Task3.ShareCode {
	public class UDPTaskIO : TaskIO {
		public bool IsListening { get; private set; }

		public event OnIncomingMesaggeEventHandler OnImcomingStatusMessage;
		public event OnIncomingMesaggeEventHandler OnImcomingTextMessage;

		private AutoResetEvent _messagesQueueUpdated;

		private ConcurrentQueue<string> _recievedMessages;

		private object _readingLock = new object();
		private object _writingLock = new object();

		private Socket _socket;
		private MemoryStream _buffer;

		private EndPoint _receiverEndpoint;
		private int _portToReceive;

		/// <summary>
		/// Stores part of hash if it was delivered in seperated.
		/// </summary>
		private byte[] _hashBuffer;
		/// <summary>
		/// False until first four bytes were recieved.
		/// </summary>
		private bool _firstFourBytesRecieved;

		public UDPTaskIO(string server, int portToReceive, int portToSend) {
			_portToReceive = portToReceive;

			_receiverEndpoint = new IPEndPoint(IPAddress.Parse(server), portToSend);
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


			_buffer = new MemoryStream();

			_messagesQueueUpdated = new AutoResetEvent(false);
			_recievedMessages = new ConcurrentQueue<string>();

			_hashBuffer = new byte[NetUtils.HashBytes.Length];
			_firstFourBytesRecieved = false;
		}

		private void CheckForConnection() {
			if (!IsListening) {
				throw new InvalidOperationException("Didn't connect to server for receiving.");
			}
		}

		public override void StartListening() {
			_socket.Bind(new IPEndPoint(IPAddress.Loopback, _portToReceive));

			IsListening = true;

			while (IsListening) {
				lock (_readingLock) {
					if (_firstFourBytesRecieved) {
						byte[] recievedBuffer = new byte[1], temp = new byte[_hashBuffer.Length];
						int bytesRecieved = ReceiveData(recievedBuffer);

						if (bytesRecieved != 1) {
							continue;
						}

						// Shifting array and setting last element to recieved byte.
						Array.Copy(_hashBuffer, 1, temp, 0, temp.Length - 1);
						temp[temp.Length - 1] = recievedBuffer[0];
						Array.Copy(temp, _hashBuffer, temp.Length);
					} else {
						if (ReceiveData(_hashBuffer) != _hashBuffer.Length) {
							// TODO: Exception.
						}

						_firstFourBytesRecieved = true;
					}

					if (_hashBuffer.SequenceEqual(NetUtils.HashBytes)) {
						RecieveMessage();
					}
				}
			}
		}


		public void Stop() {
			IsListening = false;
		}

		private void RecieveMessage() {
			byte[] convertedMessage,
				convertedMessageType = new byte[1],
				convertedMessageLength = new byte[4];

			ReceiveData(convertedMessageType);
			ReceiveData(convertedMessageLength);

			convertedMessage = new byte[BitConverter.ToInt32(convertedMessageLength, 0)];
			ReceiveData(convertedMessage);

			string message = Encoding.UTF8.GetString(convertedMessage);
			NetworkMessageType messageType = (NetworkMessageType)convertedMessageType[0];

			IncomingMessageEventArgs incomingMessageEventArgs = new IncomingMessageEventArgs(message, messageType);

			switch (messageType) {
				case NetworkMessageType.Status:
					OnImcomingStatusMessage?.Invoke(this, incomingMessageEventArgs);
					break;
				case NetworkMessageType.Text:
					_recievedMessages.Enqueue(message);
					_messagesQueueUpdated.Set();

					OnImcomingTextMessage?.Invoke(this, incomingMessageEventArgs);
					break;
			}
		}

		public override string ReadLine() {
			while (_recievedMessages.IsEmpty) {
				_messagesQueueUpdated.WaitOne();
			}

			_recievedMessages.TryDequeue(out string result);

			return result.TrimEnd('\n');
		}

		public override void Write(string s) {
			Write(s, NetworkMessageType.Text);
		}

		public void Write(string s, NetworkMessageType messageType) {
			byte[]
				convertedMessageType = new byte[] { (byte)messageType },

				convertedMessage = Encoding.UTF8.GetBytes(s),
				convertedMessageLength = BitConverter.GetBytes(convertedMessage.Length);

			int length = NetUtils.HashBytes.Length +
						convertedMessage.Length +
						convertedMessageType.Length +
						convertedMessageLength.Length;

			using (MemoryStream bufferStream = new MemoryStream(length)) {
				bufferStream.Append(NetUtils.HashBytes);
				bufferStream.Append(convertedMessageType);
				bufferStream.Append(convertedMessageLength);
				bufferStream.Append(convertedMessage);

				bufferStream.Position = 0;

				lock (_writingLock) {
					SendData(bufferStream.ToArray());
				}
			}
		}


		protected int ReceiveData(byte[] data) {
			CheckForConnection();


			while (_buffer.Length < data.Length) {
				byte[] buffer = new byte[NetUtils.MaxMessageLength];
				int receivedLength = _socket.ReceiveFrom(buffer, ref _receiverEndpoint);

				byte[] fixedBuffer = new byte[receivedLength];
				Array.Copy(buffer, fixedBuffer, fixedBuffer.Length);

				_buffer.Append(fixedBuffer);
			}

			byte[] allReceived = _buffer.ToArray();
			byte[] newBuffer = new byte[allReceived.Length - data.Length];

			Array.Copy(allReceived, data, data.Length);
			Array.Copy(allReceived, data.Length, newBuffer, 0, newBuffer.Length);

			_buffer = new MemoryStream();
			_buffer.Append(newBuffer);

			return data.Length;

		}

		protected void SendData(byte[] data) {
			CheckForConnection();

			_socket.SendTo(data, _receiverEndpoint);
		}
	}

	public delegate void OnIncomingMesaggeEventHandler(object sender, IncomingMessageEventArgs e);

	public class IncomingMessageEventArgs : EventArgs {
		public string Message { get; }
		public NetworkMessageType MessageType { get; }

		public IncomingMessageEventArgs(string message, NetworkMessageType messageType) {
			Message = message;
			MessageType = messageType;
		}
	}
}
