﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class FontStyle : IFontStyle
    {
        public IFontSize FontSize { get; set; }
        public IFontWeight FontWeight { get; set; }
    }
}
