using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class OverflowSet<TItem> : FluentUIComponentBase
    {
        [Parameter] public bool Vertical { get; set; }
        [Parameter] public IEnumerable<TItem> Items { get; set; }
        [Parameter] public IEnumerable<TItem> OverflowItems { get; set; }

        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter] public RenderFragment<IEnumerable<TItem>> OverflowTemplate { get; set; }

        [Parameter] public bool DoNotContainWithinFocusZone { get; set; }

        //[Parameter] public RenderFragment<RenderFragment> OverflowMenuButtonTemplate { get; set; }

        [Parameter] public Func<TItem, string> GetKey { get; set; }

        //protected System.Collections.Generic.List<TItem> calculatedItems;
        //protected System.Collections.Generic.List<TItem> calculatedOverflowItems;

        protected FocusZone focusZoneComponent;

        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
        //{
        //    var overflowSetRules = new HashSet<IRule>();
        //    overflowSetRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet" },
        //        Properties = new CssString()
        //        {
        //            Css = $"position:relative;" +
        //                    $"display:flex;" +
        //                    $"flex-wrap:nowrap;"
        //        }
        //    });
        //    overflowSetRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet--vertical" },
        //        Properties = new CssString()
        //        {
        //            Css = $"flex-direction:column;"
        //        }
        //    });
        //    overflowSetRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet-item" },
        //        Properties = new CssString()
        //        {
        //            Css = $"flex-shrink:0;" +
        //                    $"display:inherit;"
        //        }
        //    });
        //    overflowSetRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet-overflowButton" },
        //        Properties = new CssString()
        //        {
        //            Css = $"flex-shrink:0;" +
        //                    $"display:inherit;"
        //        }
        //    });
        //    return overflowSetRules;
        //}
    }
}
