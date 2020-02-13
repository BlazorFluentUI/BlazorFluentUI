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

        private ICollection<Rule> IconRules { get; set; } = new List<Rule>();

        protected override void OnInitialized()
        {
            CreateCss();
            base.OnInitialized();
        }

        protected override void OnThemeChanged()
        {
            CreateCss();
            base.OnThemeChanged();
        }

        private void CreateCss()
        {
            IconRules.Clear();
            IconRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Icon" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;"
                }
            });

            IconRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Icon-placeHolder" },
                Properties = new CssString()
                {
                    Css = $"width:1em;"
                }
            });
            IconRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Icon-imageContainer" },
                Properties = new CssString()
                {
                    Css = $"overflow:hidden;"
                }
            });
        }
    }
}
