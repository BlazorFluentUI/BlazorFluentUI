using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class BFUDropdownOption : IBFUDropdownOption
    {
        public bool Disabled { get; set; }
        public bool Hidden { get; set; }
        public SelectableOptionMenuItemType ItemType { get; set; } = SelectableOptionMenuItemType.Normal;
        public string Key { get; set; }
        public string Text { get; set; }
    }
}
