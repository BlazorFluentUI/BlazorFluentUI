using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.BaseComponent
{
    public class Rectangle
    {
        public double top {get;set;}
        public double left { get; set; }
        public double width { get { return right - left; } set { right = left + value; } }
        public double height { get { return bottom - top; } set { bottom = top + value; } }
        public double right { get; set; }
        public double bottom { get; set; }
    }
}
