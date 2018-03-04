using System;
using System.Linq;
using Task1.TasksIO;

namespace Task1.Menu {
    public class FirstTaskMenu : Menu {
        protected RailsInfo _railsInfo;

        public FirstTaskMenu(TaskIO taskIO, RailsInfo railsInfo) : base("FirstMenuTask", taskIO) {
			_taskIO = taskIO;
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
                _taskIO.WriteLine("No blocks found");
                return true;
            }

            if (!_taskIO.ReadInteger("From: ", out int checkFrom)) {
                return false;
            }

            if (checkFrom >= _railsInfo.Blocks.Count || checkFrom < 1) {
				_taskIO.WriteLine($"From must be greater than 0 and less than {_railsInfo.Blocks.Count + 1}");
                return false;
            }

            if (!_taskIO.ReadInteger("To: ", out int checkTo)) {
                return false;
            }

            if (checkTo >= _railsInfo.Blocks.Count || checkTo <= checkFrom) {
				_taskIO.WriteLine($"To must be greater than {checkFrom - 1} and less than {_railsInfo.Blocks.Count + 1}");
                return false;
            }

			_taskIO.Write("Result: ");

            for (int i = checkFrom; i <= checkTo; i++) {
                if (_railsInfo.Blocks[i - 1] == BlockState.Red) {
					_taskIO.WriteLine($"{i} block closed");
                    return true;
                }
            }

			_taskIO.WriteLine("way is open");

            return true;
        }

        public bool ViewState() {
            if (_railsInfo.Blocks.Count == 0) {
				_taskIO.WriteLine("No blocks found");
                return true;
            } else {
				_taskIO.WriteLine($"Rall path state: {String.Join("-", from block in _railsInfo.Blocks select block.ToString()[0])}");
                return true;
            }
        }

        public bool ChangeSignal() {
            if (_railsInfo.Blocks.Count == 0) {
				_taskIO.WriteLine("No blocks found");
                return true;
            }

            if (!_taskIO.ReadInteger("Signal number: ", out int signalNumber)) {
                return false;
            }

            if (signalNumber > _railsInfo.Blocks.Count || signalNumber < 1) {
				_taskIO.WriteLine($"Signal number must be greater than 0 and less than {_railsInfo.Blocks.Count + 1}");
                return false;
            }

            if (!_taskIO.ReadBlock("New block: ", out BlockState newState)) {
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
