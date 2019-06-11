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

        public Rectangle() { }

        public Rectangle(double left, double width, double top, double height)
        {
            this.left = left;
            this.width = width;
            this.top = top;
            this.height = height;
        }
    }

    public class PartialRectangle
    {
        public double? top { get; set; }
        public double? left { get; set; }
        public double? right { get; set; }
        public double? bottom { get; set; }

        public string GetStyle()
        {
            return (top.HasValue ? $"top:{top.Value}px;" : "") + (left.HasValue ? $"left:{left.Value}px;" : "") + (bottom.HasValue ? $"bottom:{bottom.Value}px;" : "") + (right.HasValue ? $"right:{right.Value}px;" : "");
        }
    }
}
