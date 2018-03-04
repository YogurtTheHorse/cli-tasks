using System;

namespace Task1.Menu {
    public struct MenuItem {
        public string Label;
        public Func<bool> Action;
    }
}