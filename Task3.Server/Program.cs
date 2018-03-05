using System;
using Task1;
using Task1.Menu;
using Task1.TasksIO;
using Task3.ShareCode;

namespace Task3.Server {
	public class Program {
		public static void Main(string[] args) {
			TaskIO io = new UDPTaskIO("127.0.0.1", NetUtils.ServerPort, NetUtils.ClientPort);
			
			io.StartListeningAsync();

			RailsReader railsParser = new RailsReader(io);
			RailsInfoParseResult res = railsParser.ReadRailsInfo();

			if (res.IsSuccess) {
				var mainMenu = new FirstTaskMenu(io, res.Info);
				mainMenu.Open();
			} else {
				io.WriteLine(res.Error);
			}
		}
	}
}
