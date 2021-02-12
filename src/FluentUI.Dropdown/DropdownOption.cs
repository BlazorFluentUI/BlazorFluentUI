using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public class DropdownOption : IDropdownOption
    {
        public bool Disabled { get; set; }
        public bool Hidden { get; set; }
        public SelectableOptionMenuItemType ItemType { get; set; } = SelectableOptionMenuItemType.Normal;

        string key;
        public string Key
        {
            get
            {
                if(key == null)
                {
                    key = Guid.NewGuid().ToString();
                }
                return key;
            }
            set
            {
                key= value;
            }
        }
        public string Text { get; set; }
    }
}
