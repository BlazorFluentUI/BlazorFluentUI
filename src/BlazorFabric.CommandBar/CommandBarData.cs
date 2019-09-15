using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.CommandBarInternal
{
    public class CommandBarData
    {
        public System.Collections.Generic.List<ICommandBarItem> PrimaryItems { get; set; }
        public System.Collections.Generic.List<ICommandBarItem> OverflowItems { get; set; }
        public System.Collections.Generic.List<ICommandBarItem> FarItems { get; set; }
        public int MinimumOverflowItems { get; set; }
        public string CacheKey { get; set; }

    }
}
