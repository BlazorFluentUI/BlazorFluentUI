using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class JSRect
    {
        public double left { get; set; } 
        public double top { get; set; }
        public double right { get; set; }
        public double bottom { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }

        public override string ToString()
        {
            return $"left: {left} top: {top} right: {right} bottom: {bottom} width: {width} height: {height}";
        }
    }
}
