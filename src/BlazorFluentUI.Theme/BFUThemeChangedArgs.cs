using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class BFUThemeChangedArgs : EventArgs
    {
        public ITheme Theme { get; }
        public BFUThemeChangedArgs(ITheme theme)
        {
            Theme = theme;
        }
    }
}
