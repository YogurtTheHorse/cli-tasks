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
					WriteLine("Error reading block state");
					return false;
			}

			return true;
		}

		public virtual void StartListening() { }
		public void StartListeningAsync() {
			Thread listeningThread = new Thread(StartListening);
			listeningThread.Start();
			Thread.Sleep(100);
		}

		public bool ReadInteger(string message, out int res) {
			Write(message);

			string msg = ReadLine();

			if (!int.TryParse(msg, out res)) {
				WriteLine("Can't parse number");
				return false;
			}

			return true;
		}
	}
}