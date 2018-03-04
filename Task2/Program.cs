using System;
using Task1;
using Task1.TasksIO;
using Task2.Menus;

namespace Task2 {
    public class Program {
        private static RailsInfo _railsInfo;
		private static TaskIO _taskIO;
        
        public static void Main(string[] args) {
            IRailsSerializer serializer = new JSONRailsSerializer();

            Console.Write("Specify rails file (<Enter> to read from keyboard): ");
            string path = Console.ReadLine();
            if (String.IsNullOrEmpty(path)) {
                ReadRailFromConsole();
            } else if (!serializer.TryDeserializeRails(path, out _railsInfo)) {
                Console.WriteLine($"Couldn't read rails info from {path}.");
                return;
            }

			_taskIO = new ConsoleTaskIO();

			var mainMenu = new SecondTaskMenu(_taskIO, _railsInfo, serializer);
            mainMenu.Open();
        }

        private static void ReadRailFromConsole() {
            RailsReader railsParser = new RailsReader(_taskIO);
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
