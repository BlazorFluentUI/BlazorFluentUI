using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Link : FabricComponentBase
    {
        [Parameter]
        public LinkType? Type { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public string Target { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private ICollection<Rule> CreateGlobalCss()
        {
            var linkRules = new HashSet<Rule>();
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.Link};outline:none;font-size:inherit;font-weight:inherit;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link.isButton" }, Properties = new CssString() { Css = "background:none;background-color:transparent;border:none;cursor:pointer;display:inline;margin:0;overflow:inherit;padding:0;text-align:left;text-overflow:inherit;user-select:text;border-bottom:1px solid transparent;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link:not(.isButton)" }, Properties = new CssString() { Css = "text-decoration:none;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link.isDisabled" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.DisabledText};cursor:default;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link.isDisabled:link, .ms-Link.isDisabled:visited" }, Properties = new CssString() { Css = "pointer-events:none;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link.isDisabled:link, .ms-Link.isDisabled:visited" }, Properties = new CssString() { Css = "pointer-events:none;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link:not(.isDisabled):hover:active, .ms-Link:not(.isDisabled):hover, .ms-Link:not(.isDisabled):active" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.LinkHovered};text-decoration:underline;" } });
            linkRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Link:not(.isDisabled):focus" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.Link}" } });
            return linkRules;
        }
    }
}
