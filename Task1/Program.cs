using System;
using Task1.Menu;
using Task1.TasksIO;

namespace Task1 {
    public class Program {
        public static void Main(string[] args) {
            TaskIO io = new ConsoleTaskIO();
            RailsReader railsParser = new RailsReader(io);
            RailsInfoParseResult res = railsParser.ReadRailsInfo();

            if (!res.IsSuccess) {
                io.WriteLine(res.Error);
                io.ReadLine();
                return;
            }

            var mainMenu = new FirstTaskMenu(io, res.Info);
            mainMenu.Open();
            Console.ReadLine();
        }
    }
}
