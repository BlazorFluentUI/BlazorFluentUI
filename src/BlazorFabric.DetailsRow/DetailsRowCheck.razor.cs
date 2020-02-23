using BlazorFabric.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class DetailsRowCheck : FabricComponentBase
    {
        [Parameter]
        public bool CanSelect { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool IsHeader { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;

        private ICollection<Rule> DetailsRowCheckGlobalRules { get; set; }

        protected void CreateCss()
        {
            DetailsRowCheckGlobalRules = new List<Rule>();
            var focusProps = new FocusStyleProps(Theme);
            var focusStyles = FocusStyle.GetFocusStyle(focusProps, ".ms-DetailsRowCheck-check");

            DetailsRowCheckGlobalRules.Add(
                new Rule()
                {
                    Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRowCheck-check" },
                    Properties = new CssString()
                    {
                        Css = focusStyles.MergeRules +
                          $"display:flex;" +
                          $"align-items:center;" +
                          $"justify-content:center;" +
                          $"cursor:default;" +
                          $"box-sizing:border-box;" +
                          $"vertical-align:top;" +
                          $"background:none;" +
                          $"background-color:transparent;" +
                          $"border:none;" +
                          $"opacity:0;" +
                          $"height:42px;" +
                          $"width:48px;" +
                          $"padding:0px;" +
                          $"margin:0px;" 
                    }
                });
            
            DetailsRowCheckGlobalRules.Add(
               new Rule()
               {
                   Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-compact .ms-DetailsRowCheck-check" },
                   Properties = new CssString()
                   {
                       Css = "height:32px;"
                   }
               });
            DetailsRowCheckGlobalRules.Add(
               new Rule()
               {
                   Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRowCheck-check.is-checked,.ms-DetailsRowCheck-check.can-select,.ms-DetailsRowCheck-check.is-header" },
                   Properties = new CssString()
                   {
                       Css = "opacity:1;"
                   }
               });
        }

    }
}
