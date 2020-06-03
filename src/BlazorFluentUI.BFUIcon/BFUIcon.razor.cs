using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public partial class BFUIcon : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter] public string IconName { get; set; }
        [Parameter] public IconType IconType { get; set; }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var iconRules = new HashSet<IRule>();
            iconRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Icon" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;"
                }
            });

            iconRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Icon-placeHolder" },
                Properties = new CssString()
                {
                    Css = $"width:1em;"
                }
            });

            iconRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Icon-imageContainer" },
                Properties = new CssString()
                {
                    Css = $"overflow:hidden;"
                }
            });

            return iconRules;
        }
    }
}
