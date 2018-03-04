using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.TasksIO {
	public class ConsoleTaskIO : TaskIO {
		public override string ReadLine() => Console.ReadLine();

		public override void Write(string s) => Console.Write(s);
	}
}
