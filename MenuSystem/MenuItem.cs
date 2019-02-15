using System;

namespace MenuSystem
{
    public class MenuItem
    {
        public string Description { get; set; }

        public string Shortcut { get; set; }
         
        public Action CommandToExecute { get; set; }

        public bool IsDefaultChoice { get; set; } = false;

        public MenuItemType MenuItemType { get; set; } = MenuItemType.Regular;
        
        public override string ToString()
        {
            return  ") " + Description;
        }
        
    }
}
