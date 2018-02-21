using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task1.Readers;

namespace task1.Menu {
    public class FirstTaskMenu : Menu {
        protected IReader<BlockState> _blocksReader;
        protected RailsInfo _railsInfo;

        public FirstTaskMenu(IReader<int> intReader, IReader<BlockState> blocksReader, RailsInfo railsInfo) : base("FirstMenuTask", intReader) {
            _blocksReader = blocksReader;
            _railsInfo = railsInfo;

            InitializeMenuItems();
        }

        public virtual void InitializeMenuItems() {
            AddMenuItem(new MenuItem() {
                Label = "Check",
                Action = Check
            });
            AddMenuItem(new MenuItem() {
                Label = "View state",
                Action = ViewState
            });
            AddMenuItem(new MenuItem() {
                Label = "Change signal",
                Action = ChangeSignal
            });
            AddMenuItem(new MenuItem() {
                Label = "Exit",
                Action = () => { return false; }
            });
        }

        public bool Check() {
            if (_railsInfo.Blocks.Count == 0) {
                Console.WriteLine("No blocks found");
                return true;
            }

            if (!_intReader.Read("From: ", out int checkFrom)) {
                return false;
            }

            if (checkFrom >= _railsInfo.Blocks.Count || checkFrom < 1) {
                Console.WriteLine($"From must be greater than 0 and less than {_railsInfo.Blocks.Count + 1}");
                return false;
            }

            if (!_intReader.Read("To: ", out int checkTo)) {
                return false;
            }

            if (checkTo >= _railsInfo.Blocks.Count || checkTo <= checkFrom) {
                Console.WriteLine($"To must be greater than {checkFrom - 1} and less than {_railsInfo.Blocks.Count + 1}");
                return false;
            }

            Console.Write("Result: ");

            for (int i = checkFrom; i <= checkTo; i++) {
                if (_railsInfo.Blocks[i - 1] == BlockState.Red) {
                    Console.WriteLine($"{i} block closed");
                    return true;
                }
            }

            Console.WriteLine("way is open");

            return true;
        }

        public bool ViewState() {
            if (_railsInfo.Blocks.Count == 0) {
                Console.WriteLine("No blocks found");
                return true;
            } else {
                Console.WriteLine($"Rall path state: {String.Join("-", from block in _railsInfo.Blocks select block.ToString()[0])}");
                return true;
            }
        }

        public bool ChangeSignal() {
            if (_railsInfo.Blocks.Count == 0) {
                Console.WriteLine("No blocks found");
                return true;
            }

            if (!_intReader.Read("Signal number: ", out int signalNumber)) {
                return false;
            }

            if (signalNumber > _railsInfo.Blocks.Count || signalNumber < 1) {
                Console.WriteLine($"Signal number must be greater than 0 and less than {_railsInfo.Blocks.Count + 1}");
                return false;
            }

            if (!_blocksReader.Read("New block: ", out BlockState newState)) {
                return false;
            }

            _railsInfo.Blocks[signalNumber - 1] = newState;
            if (newState == BlockState.Red && signalNumber > 1 && _railsInfo.Blocks[signalNumber - 2] == BlockState.Green) {
                _railsInfo.Blocks[signalNumber - 2] = BlockState.Yellow;
            }

            return true;
        }
    }
}
