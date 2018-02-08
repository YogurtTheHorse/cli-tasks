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

            RailsInfo railsInfo = res.Info;

            var mainMenu = new Menu.Menu("Menu", consoleIntParser);
            mainMenu.AddMenuItem(new MenuItem() {
                Label = "Check",
                Action = railsInfo.Check
            });
            mainMenu.AddMenuItem(new MenuItem() {
                Label = "View state",
                Action = railsInfo.ViewState
            });
            mainMenu.AddMenuItem(new MenuItem() {
                Label = "Change signal",
                Action = railsInfo.ChangeSignal
            });
            mainMenu.AddMenuItem(new MenuItem() {
                Label = "Exit",
                Action = () => { return false; }
            });

            mainMenu.Open();
        }
    }
}
