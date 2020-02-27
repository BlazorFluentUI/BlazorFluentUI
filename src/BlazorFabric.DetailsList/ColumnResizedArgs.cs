using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class ColumnResizedArgs<TItem>
    {
        public int ColumnIndex { get; set; }
        public double NewWidth { get; set; }
        public DetailsRowColumn<TItem> Column { get; set; }

        public ColumnResizedArgs(DetailsRowColumn<TItem> column, int colIndex, double width)
        {
            Column = column;
            ColumnIndex = colIndex;
            NewWidth = width;
        }
    }
}
