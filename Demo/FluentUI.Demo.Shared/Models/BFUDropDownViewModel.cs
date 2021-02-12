using FluentUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Demo.Shared.Models
{
    class DropDownViewModel : RibbonItem
    {
        public IEnumerable<IDropdownOption> DropdownOptions { get; set; }
        public IDropdownOption Selected { get; set; }

        public string Width { get; set; } = "200px";
    }
}
