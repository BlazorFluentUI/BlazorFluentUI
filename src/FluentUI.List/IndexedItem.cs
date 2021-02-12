using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public class IndexedItem<TItem>
    {
        public TItem Item { get; set; }
        public int Index { get; set; }

    }
}
