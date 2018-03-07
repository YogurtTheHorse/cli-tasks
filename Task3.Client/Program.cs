using System;
using System.Net.Sockets;

using Task3.ShareCode;

namespace Task3.Client {
	class Program {
		public static void Main(string[] args) {
			if (args.Length == 0) {
				Console.WriteLine("Specify server ip in arguments");
				Console.ReadLine();

				return;
			}

			try {
				UDPTaskIO io = new UDPTaskIO(args[0], NetUtils.ClientPort, NetUtils.ServerPort);
				io.OnIncomingTextMessage += (s, e) => Console.Write(e.Message);

				io.StartListeningAsync();
				io.Write("hello", NetworkMessageType.Status);
				io.Send(new TestClass("hi"));

				while (io.IsListening) {
					io.WriteLine(Console.ReadLine());
				}
			} catch (SocketException ex) {
				if (ex.SocketErrorCode == SocketError.ConnectionReset) {
					Console.WriteLine("Server disconnected");
				} else {
					Console.WriteLine($"Network error: {ex.SocketErrorCode}");
					Console.ReadLine();
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}
	}
}
