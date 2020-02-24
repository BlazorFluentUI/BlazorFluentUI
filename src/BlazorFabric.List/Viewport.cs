using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class Viewport
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public bool IsScrolling { get; set; }
        public (double X,double Y) ScrollDistance { get; set; }
        public (ScrollDirection X, ScrollDirection Y) ScrollDirection { get; set; }


    }

    public enum ScrollDirection
    {
        None,
        Backward,
        Forward
    }

    public enum Axis
    {
        X = 0,
        Y = 1
    }

}
