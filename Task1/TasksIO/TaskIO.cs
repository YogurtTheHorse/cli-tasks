using System;
using System.Threading;

namespace Task1.TasksIO {
	public abstract class TaskIO {
		public abstract string ReadLine();
		public abstract void Write(string s);

		public void WriteLine(string s) => Write(s + "\n");

		public bool ReadBlock(string message, out BlockState res) {
			Write(message);

			string msg = ReadLine();
			bool parsed = false;
			res = BlockState.Incorrect;

			while (!parsed) {
				parsed = true;
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
						parsed = false;

						WriteLine("Error reading block state. Try again");
						Write(msg);

						msg = ReadLine();
						break;
				}
			}

			return true;
		}

		public virtual void StartListening() { }
		public void StartListeningAsync() {
			Thread listeningThread = new Thread(StartListening);
			listeningThread.Start();
			Thread.Sleep(100);
		}

		public virtual void Stop() { }

		public bool ReadInteger(string message, out int res) {
			Write(message);

			string msg = null;

			do {
				if (msg != null) {
					WriteLine("Can't parse number. Try again");
				}
				msg = ReadLine();
			} while (!int.TryParse(msg, out res));

			return true;
		}
	}
}