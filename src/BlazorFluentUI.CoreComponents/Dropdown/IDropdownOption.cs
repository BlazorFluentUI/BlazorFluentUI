using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public interface IDropdownOption
    {
        bool Disabled { get; set; }
        bool Hidden { get; set; }
        SelectableOptionMenuItemType ItemType { get; set; }
        string Key { get; set; }
        string Text { get; set; }
    }
}
