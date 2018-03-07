using System;
using System.Linq;
using Task1.TasksIO;

namespace Task1.Menu {
	public class FirstTaskMenu : Menu {
		protected RailsInfo _railsInfo;

		public FirstTaskMenu(ITaskIO taskIO, RailsInfo railsInfo) : base("FirstMenuTask", taskIO) {
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

			int? nullableFrom = null, nullableTo = null;

			do {
				if (nullableFrom.HasValue) {
					_taskIO.WriteLine($"From must be greater than 0 and less than {_railsInfo.Blocks.Count + 1}");
				}

				if (!_taskIO.ReadInteger("From: ", out int checkFrom)) {
					return false;
				}
				nullableFrom = checkFrom;
			} while (nullableFrom > _railsInfo.Blocks.Count || nullableFrom < 1);

			do {
				if (nullableTo.HasValue) {
					_taskIO.WriteLine($"To must be greater than {nullableFrom - 1} and less than {_railsInfo.Blocks.Count + 1}");
				}

				if (!_taskIO.ReadInteger("To: ", out int checkTo)) {
					return false;
				}
				nullableTo = checkTo;
			} while (nullableTo > _railsInfo.Blocks.Count || nullableTo < nullableFrom);

			_taskIO.Write("Result: ");

			for (int i = nullableFrom.Value; i <= nullableTo; i++) {
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
			} else {
				_taskIO.WriteLine($"Rall path state: {String.Join("-", from block in _railsInfo.Blocks select block.ToString()[0])}");
			}
			return true;
		}

		public bool ChangeSignal() {
			if (_railsInfo.Blocks.Count == 0) {
				_taskIO.WriteLine("No blocks found");
				return true;
			}

			int? nullableSignalNumber = null;

			do {
				if (nullableSignalNumber.HasValue) {
					_taskIO.WriteLine($"Signal number must be greater than 0 and less than {_railsInfo.Blocks.Count + 1}");
				}

				if (!_taskIO.ReadInteger("Signal number: ", out int signalNumber)) {
					return false;
				}

				nullableSignalNumber = signalNumber;
			} while (nullableSignalNumber > _railsInfo.Blocks.Count || nullableSignalNumber < 1);

			if (!_taskIO.ReadBlock("New block: ", out BlockState newState)) {
				return false;
			}

			_railsInfo.Blocks[nullableSignalNumber.Value - 1] = newState;
			if (newState == BlockState.Red && nullableSignalNumber > 1 && _railsInfo.Blocks[nullableSignalNumber.Value - 2] == BlockState.Green) {
				_railsInfo.Blocks[nullableSignalNumber.Value - 2] = BlockState.Yellow;
			}

			return true;
		}
	}
}
