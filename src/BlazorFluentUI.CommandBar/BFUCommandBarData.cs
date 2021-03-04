using System.Collections.Generic;

namespace BlazorFluentUI.BFUCommandBarInternal
{
    public class BFUCommandBarData
    {
        public List<IBFUCommandBarItem> PrimaryItems { get; set; }
        public List<IBFUCommandBarItem> OverflowItems { get; set; }
        public List<IBFUCommandBarItem> FarItems { get; set; }
        public int MinimumOverflowItems { get; set; }
        public string CacheKey { get; set; }

    }
}
