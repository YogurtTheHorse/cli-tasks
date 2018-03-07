using System;
using System.Collections.Generic;
using Task1.TasksIO;

namespace Task1.Menu {
	public class Menu {
		protected string _menuLabel;
		protected List<MenuItem> _items;
		protected TaskIO _taskIO;

		public Menu(string menuLabel, TaskIO taskIO) {
			_menuLabel = menuLabel;
			_taskIO = taskIO;

			_items = new List<MenuItem>();
		}

		public void AddMenuItem(MenuItem item) {
			_items.Add(item);
		}

		public void Open() {
			bool runNext = true;


			do {
				_taskIO.WriteLine(_menuLabel);

				for (int i = 0; i < _items.Count; ++i) {
					_taskIO.WriteLine($"{i + 1}. {_items[i].Label}");
				}

				if (!_taskIO.ReadInteger("> ", out int res)) {
					continue;
				}

				if (res < 0 || res > _items.Count) {
					_taskIO.WriteLine("No such item.");
					continue;
				}

				if (!_items[res - 1].Action()) {
					runNext = false;
				}
			} while (runNext);
		}
	}
}
