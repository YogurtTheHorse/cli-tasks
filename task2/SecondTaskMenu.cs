using System;

using task1;
using task1.Menu;
using task1.Readers;

namespace task2.Menus {
    public class SecondTaskMenu : FirstTaskMenu {
        private IRailsSerializer _serializer;

        public SecondTaskMenu(IReader<int> intReader, IReader<BlockState> blocksReader, RailsInfo railsInfo, IRailsSerializer serializer) :
            base(intReader, blocksReader, railsInfo) {

            _menuLabel = "SecondTaskMenu";
            _serializer = serializer;
        }

        public override void InitializeMenuItems() {
            base.InitializeMenuItems();

            _items.Insert(1, new MenuItem() {
                Label = "Save",
                Action = Save
            });
        }

        private bool Save() {
            Console.WriteLine("Specify path to save:");
            string path = Console.ReadLine();

            try {
                _serializer.SerializeRails(path, _railsInfo);
            } catch {
                Console.WriteLine("Couldn't serialize rails");
                return false;
            }

            return true;
        }
    }
}
