using System;
using System.Net.Sockets;

using Task3.ShareCode;

namespace Task3.Client {
	class Program {
		public static void Main(string[] args) {
			try {
				UDPTaskIO io = new UDPTaskIO("127.0.0.1", NetUtils.ClientPort, NetUtils.ServerPort);
				io.OnImcomingTextMessage += (s, e) => Console.Write(e.Message);

				io.StartListeningAsync();

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
			}
		}
	}
}
