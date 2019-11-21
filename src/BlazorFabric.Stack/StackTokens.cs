using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class StackTokens
    {
        public double[] ChildrenGap { get; set; } = null;
        public CssValue MaxHeight { get; set; }
        public CssValue MaxWidth { get; set; }
        public CssValue Padding { get; set; }
    }
}
