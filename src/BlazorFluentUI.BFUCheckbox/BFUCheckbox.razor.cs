using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUCheckbox : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter]
        public int? AriaPositionInSet { get; set; }

        [Parameter]
        public int? AriaSetSize { get; set; }

        /// <summary>
        /// Allows you to set the checkbox to be at the before (start) or after (end) the label.
        /// </summary>
        [Parameter]
        public BoxSide BoxSide { get; set; }
        /// <summary>
        /// Checked state. Mutually exclusive to "defaultChecked". Use this if you control the checked state at a higher 
        /// level and plan to pass in the correct value based on handling onChange events and re-rendering.
        /// </summary>
        [Parameter]
        public bool? Checked { get; set; }

        /// <summary>
        /// Default checked state. Mutually exclusive to "checked". Use this if you want an uncontrolled component, and
        /// want the Checkbox instance to maintain its own state.
        /// </summary>
        [Parameter]
        public bool DefaultChecked { get; set; }

        /// <summary>
        /// Optional uncontrolled indeterminate visual state for checkbox. Setting indeterminate state takes visual 
        /// precedence over checked or defaultChecked props given but does not affect checked state. 
        /// This is not a toggleable state. On load the checkbox will receive indeterminate visual state and after 
        /// the user's first click it will be removed exposing the true state of the checkbox.
        /// </summary>
        [Parameter]
        public bool DefaultIndeterminate { get; set; }

        /// <summary>
        /// Disabled state of the checkbox.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Optional controlled indeterminate visual state for checkbox. Setting indeterminate state takes visual 
        /// precedence over checked or defaultChecked props given but does not affect checked state. 
        /// This should not be a toggleable state. On load the checkbox will receive indeterminate visual state and after 
        /// the first user click it should be removed by your supplied 
        /// onChange callback function exposing the true state of the checkbox.
        /// </summary>
        [Parameter]
        public bool? Indeterminate { get; set; }

        /// <summary>
        /// Label to display next to the checkbox.
        /// </summary>
        [Parameter]
        public string Label
        { get; set; }


        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter]
        public EventCallback<bool> IndeterminateChanged { get; set; }

        private string Id = Guid.NewGuid().ToString();
        private bool _isChecked;
        private bool _reversed;
        private bool _indeterminate;
        private bool _checkedUncontrolled;
        private bool _indeterminateUncontrolled;
        private bool _indeterminateChanged;

        protected override void OnInitialized()
        {
            if (!Checked.HasValue)
            {
                _isChecked = DefaultChecked;
                _checkedUncontrolled = true;
            }
            if (!Indeterminate.HasValue)
            {
                _indeterminate = DefaultIndeterminate;
                _indeterminateUncontrolled = true;
            }
            else
            {
                _indeterminate = Indeterminate.Value;
            }
            _reversed = BoxSide == BoxSide.End;
            base.OnInitialized();
        }

        protected override Task OnParametersSetAsync()
        {
            if (!_checkedUncontrolled)
            {
                _isChecked = Checked.Value;
            }
            return base.OnParametersSetAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_indeterminateChanged)
            {
                if (!_checkedUncontrolled)
                {
                    Checked = !Checked;
                }
                else
                {
                    _isChecked = !_isChecked;
                }
                _indeterminateChanged = false;
                StateHasChanged();
            }
            base.OnAfterRender(firstRender);
        }

        protected async Task InternalOnChange(ChangeEventArgs args)
        {
            if (_indeterminate)
            {
                _indeterminate = false;
                _indeterminateChanged = true;

                if (!_indeterminateUncontrolled)
                {
                    Indeterminate = false;
                    await IndeterminateChanged.InvokeAsync(false);
                }
            }

            if (!_checkedUncontrolled)
            {
                Checked = (bool)args.Value;
            }
            else
            { 
                _isChecked = (bool)args.Value;
            }

            await CheckedChanged.InvokeAsync((bool)args.Value);

        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var checkboxRules = new HashSet<IRule>();
            // ROOT
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                            $"position:relative;"
                }
            });

            // INPUT
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-input" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                            $"background:none;" +
                            $"opacity:0;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Checkbox-input:focus+.ms-Checkbox-label::before" },
                Properties = new CssString()
                {
                    Css = $"outline:1px solid {theme.Palette.NeutralSecondary };" +
                            $"outline-offset:2px;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Fabric--isFocusVisible .ms-Checkbox-input:focus+.ms-Checkbox-label::before {outline: 1px solid ActiveBorder;}"
                }
            });

            // LABEL
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                            $"align-items:flex-start;" +
                            $"cursor:pointer;" +
                            $"position:relative;" +
                            $"user-select:none;" +
                            $"text-align:left;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-reversed" },
                Properties = new CssString()
                {
                    Css = $"flex-direction:row-reverse;" +
                            $"justify-content:flex-end;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label::before" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                            $"left:0;" +
                            $"right:0;" +
                            $"top:0;" +
                            $"bottom:0;" +
                            $"content:'';" +
                            $"pointer-events:none;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-disabled" },
                Properties = new CssString()
                {
                    Css = $"cursor:default;"
                }
            });

            //Label enable && unchecked
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-unchecked:hover .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBorderHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-unchecked:focus .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBorderHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-unchecked:hover .ms-Checkbox-checkmark" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralSecondary};" +
                            $"opacity:1;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Checkbox-label-enabled.ms-Checkbox-label-unchecked:hover .ms-Checkbox-checkbox {border-color:Highlight;}" +
                            ".ms-Checkbox-label-enabled.ms-Checkbox-label-unchecked:hover .ms-Checkbox-checkmark {color:Highlight;}"
                }
            });

            //Label enable && checked && !indeterminate
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:hover .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticColors.InputBackgroundCheckedHovered};" +
                            $"border-color:{theme.SemanticColors.InputBackgroundCheckedHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:focus .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticColors.InputBackgroundCheckedHovered};" +
                            $"border-color:{theme.SemanticColors.InputBackgroundCheckedHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticColors.InputBackgroundChecked};" +
                            $"border-color:{theme.SemanticColors.InputBackgroundChecked};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:hover .ms-Checkbox-checkbox {background:Window;border-color:Highlight}" +
                            ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:focus .ms-Checkbox-checkbox {background:Highlight;}" +
                            ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:focus:hover .ms-Checkbox-checkbox {background:Highlight;}" +
                            ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:focus:hover .ms-Checkbox-checkmark {color:Window;}" +
                            ".ms-Checkbox-label-enabled:not(.ms-Checkbox-label-indeterminate).ms-Checkbox-label-checked:hover .ms-Checkbox-checkmark {color:Highlight;}"
                }
            });

            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-indeterminate.ms-Checkbox-label-checked:hover .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBackgroundCheckedHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-indeterminate.ms-Checkbox-label-checked:hover .ms-Checkbox-checkbox:after" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBackgroundCheckedHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-indeterminate.ms-Checkbox-label-checked:focus .ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBackgroundCheckedHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-indeterminate.ms-Checkbox-label-checked:hover .ms-Checkbox-checkmark" },
                Properties = new CssString()
                {
                    Css = $"opacity: '0';"
                }
            });

            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-checked:hover .ms-Checkbox-text" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.InputTextHovered};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-label-enabled.ms-Checkbox-label-checked:focus .ms-Checkbox-text" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.InputTextHovered};"
                }
            });

            // CHECKBOX
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                            $"display:flex;" +
                            $"flex-shrink:0;" +
                            $"align-items:center;" +
                            $"justify-content:center;" +
                            $"height:20px;" +
                            $"width:20px;" +
                            $"border:1px solid {theme.Palette.NeutralPrimary};" +
                            $"border-radius:{theme.Effects.RoundedCorner2};" +
                            $"box-sizing:border-box;" +
                            $"transition-property: background, border, border-color;" +
                            $"transition-duration:200ms;" +
                            $"transition-timing-function:cubic-bezier(.4, 0, .23, 1);" +
                            $"overflow:hidden;" +
                            $"margin-left:4px;" +
                            $"margin-right:0"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox-reversed" },
                Properties = new CssString()
                {
                    Css = $"margin-right:4px;" +
                            $"margin-left:0;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox-checked:not(.ms-Checkbox-checkbox-indeterminate).ms-Checkbox-checkbox-enabled" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticColors.InputBackgroundChecked};" +
                            $"border-color:{theme.SemanticColors.InputBackgroundChecked};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox-indeterminate" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBackgroundChecked};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox-disabled" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticTextColors.DisabledBodySubtext};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox-disabled.ms-Checkbox-checkbox-checked" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticTextColors.DisabledBodySubtext};" +
                            $"border-color:{theme.SemanticTextColors.DisabledBodySubtext};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox.ms-Checkbox-checkbox-indeterminate::after" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                            $"border-radius:{theme.Effects.RoundedCorner2};" +
                            $"position:absolute;" +
                            $"height:10px;" +
                            $"width:10px;" +
                            $"top:4px;" +
                            $"left:4px;" +
                            $"box-sizing:border-box;" +
                            $"border:5px solid {theme.SemanticColors.InputBackgroundChecked};" +
                            $"transition-property: border-width, border, border-color;" +
                            $"transition-duration:200ms;" +
                            $"transition-timing-function:cubic-bezier(.4, 0, .23, 1);"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkbox.ms-Checkbox-checkbox-disabled.ms-Checkbox-checkbox-indeterminate::after" },
                Properties = new CssString()
                {
                    Css = $"border:5px solid {theme.SemanticTextColors.DisabledBodySubtext};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Checkbox-checkbox-checked:not(.ms-Checkbox-checkbox-indeterminate).ms-Checkbox-checkbox-enabled {background:Highlight;border-color:Highlight;}" +
                            ".ms-Checkbox-checkbox-disabled {border-color:InactiveBorder;}"
                }
            });

            // CHECKMARK
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkmark" },
                Properties = new CssString()
                {
                    Css = $"opacity:0;" +
                            $"color:{theme.SemanticColors.InputForegroundChecked};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-checkmark.ms-Checkbox-checkmark-checked" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Checkbox-checkmark {color:Window;-ms-high-contrast-adjust:none;}" +
                            ".ms-Checkbox-checkmark-disabled {color:InactiveBorder;}"
                }
            });

            // TEXT
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-text" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.BodyText};" +
                            $"font-size:{theme.FontStyle.FontSize.Medium};" +
                            $"line-height:20px;" +
                            $"margin-left:4px;" +
                            $"margin-right:0;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-text-reversed" },
                Properties = new CssString()
                {
                    Css = $"margin-right:4px;" +
                            $"margin-left:0;"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Checkbox-text-disabled" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};"
                }
            });
            checkboxRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Checkbox-text-disabled {color: InactiveBorder;}"
                }
            });

            return checkboxRules;
        }
    }
}
