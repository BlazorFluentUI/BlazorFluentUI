using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Check : FabricComponentBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; }

        private ICollection<Rule> CheckGlobalRules { get; set; } = new List<Rule>();

        protected override Task OnInitializedAsync()
        {
            if (!CStyle.ComponentStyleExist(this))
            {
                CreateCss();
            }
            return base.OnInitializedAsync();
        }
        protected override void OnThemeChanged()
        {
            CreateCss();
            base.OnThemeChanged();
        }

        protected void CreateCss()
        {
            CheckGlobalRules.Clear();

            CheckGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check" },
                Properties = new CssString()
                {
                    Css = $"line-height:1px;" +
                          $"width:18px;" +
                          $"height:18px;" 
                }
            });

        }
    }
}
