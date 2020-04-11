namespace BlazorFluentUI
{
    public class ColumnResizedArgs<TItem>
    {
        public int ColumnIndex { get; set; }
        public double NewWidth { get; set; }
        public BFUDetailsRowColumn<TItem> Column { get; set; }

        public ColumnResizedArgs(BFUDetailsRowColumn<TItem> column, int colIndex, double width)
        {
            Column = column;
            ColumnIndex = colIndex;
            NewWidth = width;
        }
    }
}
