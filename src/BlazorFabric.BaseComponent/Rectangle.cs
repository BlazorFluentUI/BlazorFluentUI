using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
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
            return (top.HasValue ? $"top:{top.Value.ToString().Replace(',', '.')}px;" : "") + (left.HasValue ? $"left:{left.Value.ToString().Replace(',', '.')}px;" : "") + (bottom.HasValue ? $"bottom:{bottom.Value.ToString().Replace(',', '.')}px;" : "") + (right.HasValue ? $"right:{right.Value.ToString().Replace(',', '.')}px;" : "");
        }
    }
}
