using System;
using System.Collections.Generic;
using task1.Readers;

namespace task1.Menu {
    public class Menu {
        protected string _menuLabel;
        protected List<MenuItem> _items;
        protected IReader<int> _intReader;

        public Menu(string menuLabel, IReader<int> intReader) {
            _menuLabel = menuLabel;
            _intReader = intReader; 

            _items = new List<MenuItem>();
        }

        public void AddMenuItem(MenuItem item) {
            _items.Add(item);
        }

        public void Open() {
            int res;
            do {
                Console.WriteLine(_menuLabel);

                for (int i = 0; i < _items.Count; ++i) {
                    Console.WriteLine($"{i + 1}. {_items[i].Label}");
                }

                if (!_intReader.Read("> ", out res)) {
                    return;
                }

                if (res < 0 || res > _items.Count) {
                    Console.WriteLine("No such item.");
                    return;
                }

            } while (_items[res - 1].Action());
        }
    }
}
