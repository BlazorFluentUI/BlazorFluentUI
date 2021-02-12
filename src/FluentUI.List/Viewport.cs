namespace FluentUI
{
    public class Viewport
    {
        public double Width { get; set; }
        public double Height { get; set; }
        //public double ScrollWidth { get; set; } // minus scrollbar
        //public double ScrollHeight { get; set; } // minus scrollbar
        //public bool IsScrolling { get; set; }
        //public (double X,double Y) ScrollDistance { get; set; }
        //public (ScrollDirection X, ScrollDirection Y) ScrollDirection { get; set; }


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
