namespace Task1.TasksIO {
	public interface ITaskIO {
		string ReadLine();
		void Write(string message);
		void WriteLine(string message);

		bool ReadBlock(string message, out BlockState res);
		bool ReadInteger(string message, out int res);
	}
}
