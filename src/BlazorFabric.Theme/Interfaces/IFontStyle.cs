using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IFontStyle
    {
        public IFontSize FontSize { get; set; }
        public IFontWeight FontWeight { get; set; }
        public IIconFontSize IconFontSize { get; set; }
    }
}
