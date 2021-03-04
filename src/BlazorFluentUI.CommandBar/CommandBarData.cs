using System.Collections.Generic;

namespace BlazorFluentUI.CommandBarInternal
{
    public class CommandBarData
    {
        public List<ICommandBarItem> PrimaryItems { get; set; }
        public List<ICommandBarItem> OverflowItems { get; set; }
        public List<ICommandBarItem> FarItems { get; set; }
        public int MinimumOverflowItems { get; set; }
        public string CacheKey { get; set; }

    }
}
