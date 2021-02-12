using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public class ColumnMeasureInfo<TItem>
    {
        public int Index { get; set; }
        public DetailsRowColumn<TItem> Column { get; set; }
        public Action<double> OnMeasureDone { get; set; }
    }
}
