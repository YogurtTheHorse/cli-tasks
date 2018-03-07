using System;

namespace Task3.ShareCode {
	[Serializable]
	public class TestClass {
		public string Test { get; }

		public TestClass(string test) {
			Test = test;
		}
	}
}
