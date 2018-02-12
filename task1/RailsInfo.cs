
using System;
using System.Collections.Generic;
using System.Linq;
using task1.Readers;

namespace task1 {
    public class RailsInfo {
        private List<BlockState> blocks;
        private IReader<int> intReader;
        private IReader<BlockState> blocksReader;

        public int BlocksCount {
            get {
                return blocks.Count;
            }
        }

        public RailsInfo(IReader<int> intReader, IReader<BlockState> blocksReader) {
            this.intReader = intReader;
            this.blocksReader = blocksReader;

            blocks = new List<BlockState>();
        }

        public void PushBlockState(BlockState blockstate) {
            if (blockstate == BlockState.Red && BlocksCount > 0 && blocks[blocks.Count - 1] == BlockState.Green) {
                blocks[blocks.Count - 1] = BlockState.Yellow;
            }

            if (blockstate == BlockState.Incorrect) {
                throw new InvalidOperationException();
            }

            blocks.Add(blockstate);
        }

        public bool Check() {
            if (blocks.Count == 0) {
                Console.WriteLine("No blocks found");
                return true;
            }

            if (!intReader.Read("From: ", out int checkFrom)) {
                return false;
            }

            if (checkFrom >= blocks.Count || checkFrom < 1) {
                Console.WriteLine($"From must be greater than 0 and less than {blocks.Count + 1}");
                return false;
            }

            if (!intReader.Read("To: ", out int checkTo)) {
                return false;
            }

            if (checkTo >= blocks.Count || checkTo <= checkFrom) {
                Console.WriteLine($"To must be greater than {checkFrom - 1} and less than {blocks.Count + 1}");
                return false;
            }

            Console.Write("Result: ");

            for (int i = checkFrom; i <= checkTo; i++) {
                if (blocks[i - 1] == BlockState.Red) {
                    Console.WriteLine($"{i} block closed");
                    return true;
                }
            }

            Console.WriteLine("way is open");

            return true;
        }

        public bool ViewState() {
            if (blocks.Count == 0) {
                Console.WriteLine("No blocks found");
                return true;
            } else {
                Console.WriteLine($"Rall path state: {String.Join("-", from block in blocks select block.ToString()[0])}");
                return true;
            }
        }

        public bool ChangeSignal() {
            if (blocks.Count == 0) {
                Console.WriteLine("No blocks found");
                return true;
            }

            if (!intReader.Read("Signal number: ", out int signalNumber)) {
                return false;
            }

            if (signalNumber > blocks.Count || signalNumber < 1) {
                Console.WriteLine($"Signal number must be greater than 0 and less than {blocks.Count + 1}");
                return false;
            }

            if (!blocksReader.Read("New block: ", out BlockState newState)) {
                return false;
            }

            blocks[signalNumber - 1] = newState;
            if (newState == BlockState.Red && signalNumber > 1 && blocks[signalNumber - 2] == BlockState.Green) {
                blocks[signalNumber - 2] = BlockState.Yellow;
            }

            return true;
        }
    }
}
