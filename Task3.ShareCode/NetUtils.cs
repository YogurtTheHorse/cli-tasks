using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Task3.ShareCode {
	public static class NetUtils {
		// Some coded bytes that will be send before any message
		public static readonly byte[] HashBytes = new byte[] { 100, 200, 15, 75 };

		public const int ClientPort = 25500;
		public const int ServerPort = ClientPort + 1;

		public static readonly int MaxHeaderLength = HashBytes.Length + 1 + 4;
		public static readonly int MaxTextLength = 256;
		public static readonly int MaxMessageLength = MaxMessageLength + MaxTextLength * 4;

		public const string DisconnectStatusMessage = "DISCONNECT";

		public static Socket ConnectToServer(string server, int port) {
			IPHostEntry hostEntry = Dns.GetHostEntry(server);

			foreach (IPAddress address in hostEntry.AddressList) {
				IPEndPoint ipe = new IPEndPoint(address, port);
				Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

				tempSocket.Connect(ipe);

				if (tempSocket.Connected) {
					return tempSocket;
				}
			}

			return null;
		}

		public static void Append(this MemoryStream stream, byte[] bytes) {
			stream.Write(bytes, 0, bytes.Length);
		}
	}
}
