using System;

using Task1;
using Task1.Menu;
using Task1.TasksIO;

namespace Task2.Menus {
    public class SecondTaskMenu : FirstTaskMenu {
        private IRailsSerializer _serializer;

        public SecondTaskMenu(ITaskIO taskIO, RailsInfo railsInfo, IRailsSerializer serializer) :
            base(taskIO, railsInfo) {

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
