using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
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
