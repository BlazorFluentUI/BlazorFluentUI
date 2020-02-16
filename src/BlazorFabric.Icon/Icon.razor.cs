using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Icon : FabricComponentBase
    {
        [Parameter] public string IconName { get; set; }
        [Parameter] public IconType IconType { get; set; }

        private ICollection<Rule> CreateGlobalCss()
        {
            var iconRules = new HashSet<Rule>();
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
