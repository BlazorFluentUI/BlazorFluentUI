using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.BaseComponent.FocusStyle
{
    public static class FocusStyle
    {
        public static FocusStyleMergeRules GetFocusStyle(FocusStyleProps focusStyleProps, string selectorName)
        {
            var focusStyles = new FocusStyleMergeRules();
            // ROOT
            focusStyles.MergeRules = $"outline:transparent;" +
                          $"position:{focusStyleProps.Position};";

            focusStyles.AddRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}::-moz-focus-inner" },
                Properties = new CssString()
                {
                    Css = $"border:0;"
                }
            });

            focusStyles.AddRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-Fabric--isFocusVisible {selectorName}{(focusStyleProps.IsFocusedOnly ? ":focus" : "")}:after" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                          $"position:absolute;" +
                          $"left:{focusStyleProps.Inset + 1}px;" +
                          $"top:{focusStyleProps.Inset + 1}px;" +
                          $"bottom:{focusStyleProps.Inset + 1}px;" +
                          $"right:{focusStyleProps.Inset + 1}px;" +
                          $"border:{focusStyleProps.Width}px solid {focusStyleProps.BorderColor};" +
                          $"outline:{focusStyleProps.Width}px solid {focusStyleProps.OutlineColor};" +
                          $"z-index:var(--zindex-FocusStyle);"
                }
            });
            focusStyles.AddRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $"{selectorName}{(focusStyleProps.IsFocusedOnly ? ":focus" : "")}:after" +
                          "{" +
                          focusStyleProps.HighContrastStyle +
                          "}"
                }
            });

            return focusStyles;
        }
    }
}
