using BlazorFluentUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BlazorFluentUI.Demo.Shared.Models
{
    class BFUButtonViewModel: RibbonItem
    {
        
        public string IconName { get; set; }

        public string IconSrc { get; set; }
        public bool Toggle { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

    
    }
}
