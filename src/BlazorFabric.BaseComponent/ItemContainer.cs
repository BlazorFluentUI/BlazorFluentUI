using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class ItemContainer<TItem>
    {
        public TItem Item { get; set; }
        public int Index { get; set; }
    }
}
