using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class StackTokens
    {
        public double[] ChildrenGap { get; set; } = null;
        public double MaxHeight { get; set; } = double.NaN;
        public double MaxWidth { get; set; } = double.NaN;
        public CssValue Padding { get; set; }
    }
}
