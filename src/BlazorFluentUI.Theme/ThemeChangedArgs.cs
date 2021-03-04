using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class ThemeChangedArgs : EventArgs
    {
        public ITheme Theme { get; }
        public ThemeChangedArgs(ITheme theme)
        {
            Theme = theme;
        }
    }
}
