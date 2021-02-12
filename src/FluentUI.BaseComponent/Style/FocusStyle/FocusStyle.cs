using System.Collections.Generic;

namespace FluentUI.Style
{
    public static class FocusStyle
    {
        public static FocusStyleMergeRules GetFocusStyle(FocusStyleProps focusStyleProps, string selectorName)
        {
            var focusStyles = new FocusStyleMergeRules();

            focusStyles.MergeRules = $"outline:transparent;" +
                          $"position:{focusStyleProps.Position};";

            //alternative property for newer extensions (needs string array without semicolons)
            focusStyles.MergeRulesList.Add("outline:transparent");
            focusStyles.MergeRulesList.Add($"position:{focusStyleProps.Position}");

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
                Selector = new CssStringSelector() { SelectorName = $".ms-Fabric--isFocusVisible {selectorName}{(focusStyleProps.IsFocusedOnly ? ":focus" : "")}::after" },
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
                          $"z-index:{focusStyleProps.ZIndex};"
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

        public static ICollection<Rule> FocusClear(string selectorName)
        {
            var focusStyles = new HashSet<Rule>();

            focusStyles.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}::-moz-focus-inner" },
                Properties = new CssString()
                {
                    Css = $"border:0"
                }
            });
            focusStyles.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}" },
                Properties = new CssString()
                {
                    Css = $"outline:transparent;"
                }
            });
            return focusStyles;
        }

        public static FocusStyleMergeRules GetFocusOutlineStyle(FocusStyleProps focusStyleProps, string selectorName)
        {
            var focusStyles = new FocusStyleMergeRules();

            focusStyles.AddRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-Fabric--isFocusVisible {selectorName}::after" },
                Properties = new CssString()
                {
                    Css = $"outline:{focusStyleProps.Width}px solid {focusStyleProps.OutlineColor};" +
                          $"outline-offset:-{focusStyleProps.Inset}px;"
                }
            });
            return focusStyles;
        }

        public static FocusStyleMergeRules GetInputFocusStyle(FocusStyleProps focusStyleProps, string selectorName, bool isBorderBottom = false, double borderPosition = -1)
        {
            var focusStyles = new FocusStyleMergeRules();

            focusStyles.MergeRules = $"border-color:{focusStyleProps.BorderColor};";

            //alternative property for newer extensions (needs string array without semicolons)
            focusStyles.MergeRulesList.Add($"border-color:{focusStyleProps.BorderColor};");
            

            focusStyles.AddRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}::after" },
                Properties = new CssString()
                {
                    Css = $"pointer-events:none;" +
                          $"content:'';" +
                          $"position:absolute;" +
                          $"left:{(isBorderBottom ? 0 : borderPosition)}px;" +
                          $"top:{borderPosition}px;" +
                          $"bottom:{borderPosition}px;" +
                          $"right:{(isBorderBottom ? 0 : borderPosition)}px;" +
                          (isBorderBottom ? $"border-bottom:2px solid {focusStyleProps.BorderColor};" : $"border:2px solid {focusStyleProps.BorderColor};" ) +
                          $"border-radius:{focusStyleProps.BorderRadius};" +
                          (isBorderBottom ? $"width:100%;" : "")
                }
            });
            focusStyles.AddRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $"{selectorName}:after" +
                          "{" +
                          (isBorderBottom ? $"border-bottom-color:Highlight" : "border-color:Highlight") +
                          "}"
                }
            });

            return focusStyles;
        }
    }
}
