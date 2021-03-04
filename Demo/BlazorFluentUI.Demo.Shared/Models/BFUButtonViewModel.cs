using BlazorFluentUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BlazorFluentUI.Demo.Shared.Models
{
    class ButtonViewModel: RibbonItem
    {
        
        public string IconName { get; set; }

        public string IconSrc { get; set; }
        public bool Toggle { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public bool IsRadioButton { get; set; }
        public string GroupName { get; set; }


    }
}
