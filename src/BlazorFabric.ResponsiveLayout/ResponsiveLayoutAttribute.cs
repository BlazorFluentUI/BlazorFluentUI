using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct,
                       AllowMultiple = true)]
    public class ResponsiveLayoutAttribute : Attribute
    {
        public double MinWidth { get; set; }
        public double MaxWidth { get; set; }
        public double MinHeight { get; set; }
        public double MaxHeight { get; set; }
    }
}
