using BlazorFluentUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI.Demo.Shared.Models
{
    class BFUDropDownViewModel : RibbonItem
    {
        public IEnumerable<IBFUDropdownOption> DropdownOptions { get; set; }
        public IBFUDropdownOption Selected { get; set; }

        public string Width { get; set; } = "200px";
    }
}
