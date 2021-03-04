using System.Collections.Generic;

namespace BlazorFluentUI
{
    public interface IBFUNavBarItem : IBFUContextualMenuItem, IBFUCommandBarItem
    {
        //buttonStyles
        string CacheKey { get; set; }
        //commandBarButtonAs
        bool IconOnly { get; set; }

        string Id { get; set; }

        bool IsExpanded { get; set; }

        NavMatchType NavMatchType { get; set; }
        bool RenderedInOverflow { get; set; }
        //tooltipHostProps
        string Url { get; set; }

        IEnumerable<IBFUContextualMenuItem> Items { get; set; }
    }
}
