using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class Rectangle
    {
        public double top {get;set;}
        public double left { get; set; }
        //public double width { get; set; }
        //public double height { get; set; }
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
            //this.right = left + width;
            //this.bottom = top + height;
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
            return (top.HasValue ? $"top:{top.Value.ToCssValue()}px;" : "") + (left.HasValue ? $"left:{left.Value.ToCssValue()}px;" : "") + (bottom.HasValue ? $"bottom:{bottom.Value.ToCssValue()}px;" : "") + (right.HasValue ? $"right:{right.Value.ToCssValue()}px;" : "");
        }
    }
}
