using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class FontStyle : IFontStyle
    {
        public IFontSize FontSize { get; set; }
        public IFontWeight FontWeight { get; set; }
        public IIconFontSize IconFontSize { get; set; }
    }
}
