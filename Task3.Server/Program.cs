using System;
using System.Net.Sockets;

using Task1;
using Task1.Menu;
using Task1.TasksIO;
using Task3.ShareCode;

namespace Task3.Server {
	public class Program {
		public static void Main(string[] args) {
			try {
				UDPTaskIO io = new UDPTaskIO(NetUtils.ServerPort);

				io.OnIncomingObject += Io_OnIncomingObject; ;

				io.StartListeningAsync();

				RailsReader railsParser = new RailsReader(io);
				RailsInfoParseResult res = railsParser.ReadRailsInfo();

				if (res.IsSuccess) {
					var mainMenu = new FirstTaskMenu(io, res.Info);
					mainMenu.Open();
				} else {
					io.WriteLine(res.Error);
				}

				io.Stop();
			} catch (SocketException ex) {
				if (ex.SocketErrorCode == SocketError.ConnectionReset) {
					Console.WriteLine("Client disconnected"); 
				} else {
					Console.WriteLine($"Network error: {ex.SocketErrorCode}");
					Console.ReadLine();
				}
			}
		}

		private static void Io_OnIncomingObject(object sender, IncomingObjectEventArgs e) {
			object o = e.ReceivedObject;
			if (o is TestClass testObject) {
				Console.WriteLine(testObject.Test);
			}
		}
	}
}
