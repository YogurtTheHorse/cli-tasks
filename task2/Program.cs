using System;
using task1;

using task1.Readers;
using task2.Menus;

namespace task2 {
    public class Program {
        private static RailsInfo _railsInfo;
        private static ConsoleIntReader _consoleIntParser;

        public static void Main(string[] args) {
            IRailsSerializer serializer = new JSONRailsSerializer();
            _consoleIntParser = new ConsoleIntReader();

            Console.Write("Specify rails file (<Enter> to read from keyboard): ");
            string path = Console.ReadLine();
            if (String.IsNullOrEmpty(path)) {
                ReadRailFromConsole();
            } else if (!serializer.TryDeserializeRails(path, out _railsInfo)) {
                Console.WriteLine($"Couldn't read rails info from {path}.");
                return;
            }

            var mainMenu = new SecondTaskMenu(_consoleIntParser, new ConsoleBlockStateReader(), _railsInfo, serializer);
            mainMenu.Open();
        }

        private static void ReadRailFromConsole() {
            RailsReader railsParser = new RailsReader(_consoleIntParser, new ConsoleBlockStateReader());
            RailsInfoParseResult res = railsParser.ReadRailsInfo();

            if (!res.IsSuccess) {
                Console.WriteLine(res.Error);
                Console.ReadLine();
                return;
            }

            _railsInfo = res.Info;
        }
    }
}
