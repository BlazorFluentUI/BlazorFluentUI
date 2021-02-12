using FluentUI.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public partial class DetailsRowCheck : FluentUIComponentBase
    {
        [Parameter]
        public bool AnySelected { get; set; }

        [Parameter]
        public bool CanSelect { get; set; }

        [Parameter]
        public RenderFragment DetailsCheckboxTemplate { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public bool IsFocusable { get; set; }

        [Parameter]
        public bool IsHeader { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var detailsRowRules = new HashSet<IRule>();
            //var focusProps = new FocusStyleProps(theme);
            //var focusStyles = FocusStyle.GetFocusStyle(focusProps, ".ms-DetailsRow-check");

            //detailsRowRules.Add(
            //    new Rule()
            //    {
            //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-check" },
            //        Properties = new CssString()
            //        {
            //            Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
            //              $"font-weight:{theme.FontStyle.FontWeight.Regular};" +
            //                focusStyles.MergeRules +
            //              $"display:flex;" +
            //              $"align-items:center;" +
            //              $"justify-content:center;" +
            //              $"cursor:default;" +
            //              $"box-sizing:border-box;" +
            //              $"vertical-align:top;" +
            //              $"background:none;" +
            //              $"background-color:transparent;" +
            //              $"border:none;" +
            //              $"opacity:0;" +
            //              $"height:42px;" +
            //              $"width:48px;" +
            //              $"padding:0px;" +
            //              $"margin:0px;" 
            //        }
            //    });
            
            //detailsRowRules.Add(
            //   new Rule()
            //   {
            //       Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-compact .ms-DetailsRow-check" },
            //       Properties = new CssString()
            //       {
            //           Css = "height:32px;"
            //       }
            //   });
            //detailsRowRules.Add(
            //  new Rule()
            //  {
            //      Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-header .ms-DetailsRow-check" },
            //      Properties = new CssString()
            //      {
            //          Css = "height:42px;"
            //      }
            //  });
            //detailsRowRules.Add(
            //   new Rule()
            //   {
            //       Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-check.is-checked,.ms-DetailsRow-check.is-visible" },
            //       Properties = new CssString()
            //       {
            //           Css = "opacity:1;"
            //       }
            //   });


            return detailsRowRules;
            
        }

    }
}
