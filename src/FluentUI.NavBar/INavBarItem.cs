using System.Collections.Generic;

namespace FluentUI
{
    public interface INavBarItem : IContextualMenuItem, ICommandBarItem
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

        IEnumerable<IContextualMenuItem> Items { get; set; }
    }
}
