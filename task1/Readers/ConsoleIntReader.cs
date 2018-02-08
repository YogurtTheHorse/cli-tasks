using System;

namespace task1.Readers {
    public class ConsoleIntReader : IReader<int> {
        public ConsoleIntReader() { }

        public bool Read(string message, out int res) {
            Console.Write(message);

            string msg = Console.ReadLine();

            if (!int.TryParse(msg, out res)) {
                Console.WriteLine("Can't parse number");
                return false;
            }

            return true;
        }
    }
}