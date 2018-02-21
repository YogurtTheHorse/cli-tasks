using System;

namespace task1.Readers {
    public class ConsoleBlockStateReader : IReader<BlockState> {
        public bool Read(string message, out BlockState res) {
            Console.Write(message);

            string msg = Console.ReadLine();

            switch (msg.ToLower()) {
                case "r":
                    res = BlockState.Red;
                    break;

                case "g":
                    res = BlockState.Green;
                    break;

                case "y":
                    res = BlockState.Yellow;
                    break;

                default:
                    res = BlockState.Incorrect;
                    Console.WriteLine("Error reading block state");
                    return false;
            }

            return true;
        }
    }
}