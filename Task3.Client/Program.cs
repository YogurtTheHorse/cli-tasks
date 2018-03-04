using System;
using Task3.ShareCode;

namespace Task3.Client {
	class Program {
		public static void Main(string[] args) {
			UDPTaskIO io = new UDPTaskIO("127.0.0.1", NetUtils.ClientPort, NetUtils.ServerPort);
			io.OnImcomingTextMessage += (s, e) => Console.Write(e.Message);

			io.StartListeningAsync();

			while (io.IsListening) {
				io.WriteLine(Console.ReadLine());
			}
		}
	}
}
