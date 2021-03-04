using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public class RowMountArgs<TItem>
    {
        public DetailsRow<TItem> Row { get; set; }
        public TItem Item { get; set; }
        public int Index { get; set; }
    }
}
