namespace BlazorFluentUI
{
    public class ElementMeasurements
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Cwidth { get; set; }
        public double Cheight { get; set; }
        public string? Test { get; set; }

        public override string ToString()
        {
            return $"left: {Left} top: {Top} right: {Right} bottom: {Bottom} width: {Width} height: {Height}";
        }
    }
}
