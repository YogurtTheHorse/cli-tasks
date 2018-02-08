using System;
using System.Collections.Generic;
using task1.Readers;

namespace task1.Menu {
    public class Menu {
        private string menuLabel;
        private List<MenuItem> items;
        private IReader<int> intReader;

        public Menu(string menuLabel, IReader<int> intReader) {
            this.menuLabel = menuLabel;
            this.intReader = intReader; 

            items = new List<MenuItem>();
        }

        public void AddMenuItem(MenuItem item) {
            items.Add(item);
        }

        public void Open() {
            int res;
            do {
                Console.WriteLine(menuLabel);

                for (int i = 0; i < items.Count; ++i) {
                    Console.WriteLine($"{i + 1}. {items[i].Label}");
                }

                if (!intReader.Read("> ", out res)) {
                    return;
                }

                if (res < 0 || res > items.Count) {
                    Console.WriteLine("No such item.");
                    return;
                }

            } while (items[res - 1].Action());
        }
    }
}
