namespace BlazorFluentUI.Lists
{
    public class ColumnResizedArgs<TItem>
    {
        public int ColumnIndex { get; set; }
        public double NewWidth { get; set; }
        public IDetailsRowColumn<TItem> Column { get; set; }

        public ColumnResizedArgs(IDetailsRowColumn<TItem> column, int colIndex, double width)
        {
            Column = column;
            ColumnIndex = colIndex;
            NewWidth = width;
        }
    }
}
