namespace FluentUI
{
    public class ManualRectangle
    {
        public double top { get; set; }
        public double left { get; set; }
        virtual public double width { get; set; }
        virtual public double height { get; set; }
        public double right { get; set; }
        public double bottom { get; set; }

        public static ManualRectangle EmptyRect()
        {
            var rect = new ManualRectangle { left = -1, right = -1, top = -1, bottom = -1, height = 0, width = 0 };
            return rect;
        }
    }
    
    public class Rectangle : ManualRectangle
    {
        //public double width { get; set; }
        //public double height { get; set; }
        public override double width { get { return right - left; } set { right = left + value; } }
        public override double height { get { return bottom - top; } set { bottom = top + value; } }

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
