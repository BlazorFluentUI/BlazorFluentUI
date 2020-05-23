using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUOverflowSet<TItem> : BFUComponentBase
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

        protected BFUFocusZone focusZoneComponent;

        protected override Task OnParametersSetAsync()
        {
            //if (Items != null)
            //{
            //    var e = Items.GetEnumerator();
            //    if (e.MoveNext())
            //    {
            //        if (!(e.Current is IOverflowSetItem))
            //            throw new Exception("Your item class must implement IOverflowSetItem.");
            //    }
            //}

            //There's actually no calculation.  This is not necessary.  ResizeGroup is what does this stuff.
            //calculatedItems = new System.Collections.Generic.List<TItem>(Items);
            //calculatedOverflowItems = new System.Collections.Generic.List<TItem>(OverflowItems);

            return base.OnParametersSetAsync();
        }

        private ICollection<IRule> CreateGlobalCss()
        {
            var overflowSetRules = new HashSet<IRule>();
            overflowSetRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                            $"display:flex;" +
                            $"flex-wrap:nowrap;"
                }
            });
            overflowSetRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet--vertical" },
                Properties = new CssString()
                {
                    Css = $"flex-direction:column;"
                }
            });
            overflowSetRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet-item" },
                Properties = new CssString()
                {
                    Css = $"flex-shrink:0;" +
                            $"display:inherit;"
                }
            });
            overflowSetRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-OverflowSet-overflowButton" },
                Properties = new CssString()
                {
                    Css = $"flex-shrink:0;" +
                            $"display:inherit;"
                }
            });
            return overflowSetRules;
        }
    }
}
