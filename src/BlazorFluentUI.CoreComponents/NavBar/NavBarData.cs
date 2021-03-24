using System.Collections.Generic;

namespace BlazorFluentUI.Routing
{
    public class NavBarData
    {
        public List<INavBarItem> PrimaryItems { get; set; }
        public List<INavBarItem> OverflowItems { get; set; }
        public List<INavBarItem> FarItems { get; set; }
        public int MinimumOverflowItems { get; set; }
        public string CacheKey { get; set; }

    }
}
