using System.Collections.Generic;

namespace BlazorFluentUI.BFUNavBarInternal
{
    public class BFUNavBarData
    {
        public List<IBFUNavBarItem> PrimaryItems { get; set; }
        public List<IBFUNavBarItem> OverflowItems { get; set; }
        public List<IBFUNavBarItem> FarItems { get; set; }
        public int MinimumOverflowItems { get; set; }
        public string CacheKey { get; set; }

    }
}
