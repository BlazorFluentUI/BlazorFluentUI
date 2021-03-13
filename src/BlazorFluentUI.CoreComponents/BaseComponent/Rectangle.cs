namespace BlazorFluentUI
{
    public class ManualRectangle
    {
        public double Top { get; set; }
        public double Left { get; set; }
        virtual public double Width { get; set; }
        virtual public double Height { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }

        public static ManualRectangle EmptyRect()
        {
            ManualRectangle? rect = new() { Left = -1, Right = -1, Top = -1, Bottom = -1, Height = 0, Width = 0 };
            return rect;
        }
    }
    
    public class Rectangle : ManualRectangle
    {
        //public double width { get; set; }
        //public double height { get; set; }
        public override double Width { get { return Right - Left; } set { Right = Left + value; } }
        public override double Height { get { return Bottom - Top; } set { Bottom = Top + value; } }

        public Rectangle() { }

        public Rectangle(double left, double width, double top, double height)
        {
            this.Left = left;
            this.Width = width;
            this.Top = top;
            this.Height = height;
            //this.right = left + width;
            //this.bottom = top + height;
        }

        
    }

    public class PartialRectangle
    {
        public double? Top { get; set; }
        public double? Left { get; set; }
        public double? Right { get; set; }
        public double? Bottom { get; set; }

        public string GetStyle()
        {
            return (Top.HasValue ? $"top:{Top.Value.ToCssValue()}px;" : "") + (Left.HasValue ? $"left:{Left.Value.ToCssValue()}px;" : "") + (Bottom.HasValue ? $"bottom:{Bottom.Value.ToCssValue()}px;" : "") + (Right.HasValue ? $"right:{Right.Value.ToCssValue()}px;" : "");
        }
    }
}
