using System;
using task1.Menu;
using task1.Readers;

namespace task1 {
    public class Program {
        public static void Main(string[] args) {
            ConsoleIntReader consoleIntParser = new ConsoleIntReader();
            RailsReader railsParser = new RailsReader(consoleIntParser, new ConsoleBlockStateReader());
            RailsInfoParseResult res = railsParser.ReadRailsInfo();

            if (!res.IsSuccess) {
                Console.WriteLine(res.Error);
                Console.ReadLine();
                return;
            }

            var mainMenu = new FirstTaskMenu(consoleIntParser, new ConsoleBlockStateReader(), res.Info);
            mainMenu.Open();
        }
    }
}
