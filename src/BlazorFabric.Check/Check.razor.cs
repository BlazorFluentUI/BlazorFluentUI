using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class Check : FabricComponentBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; }

        private ICollection<Rule> CheckGlobalRules { get; set; }

        protected override void CreateCss()
        {
            CheckGlobalRules = new List<Rule>();

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
