using BlazorFabric.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class TextField : FabricComponentBase, IHasPreloadableGlobalStyle 
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public bool Required { get; set; }
        [Parameter] public bool Multiline { get; set; }
        [Parameter] public InputType InputType { get; set; } = InputType.Text;
        [Parameter] public bool Resizable { get; set; } = true;
        [Parameter] public bool AutoAdjustHeight { get; set; }
        [Parameter] public bool Underlined { get; set; }
        [Parameter] public bool Borderless { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public RenderFragment RenderLabel { get; set; }
        [Parameter] public string Description { get; set; }
        [Parameter] public string Prefix { get; set; }
        [Parameter] public RenderFragment PrefixContent { get; set; }
        [Parameter] public string Suffix { get; set; }
        [Parameter] public RenderFragment SuffixContent { get; set; }
        [Parameter] public string DefaultValue { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public string ErrorMessage { get; set; }
        [Parameter] public bool ValidateOnFocusIn { get; set; }
        [Parameter] public bool ValidateOnFocusOut { get; set; }
        [Parameter] public bool ValidateOnLoad { get; set; } = true;
        [Parameter] public int DeferredValidationTime { get; set; } = 200;
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public AutoComplete AutoComplete { get; set; } = AutoComplete.On;
        //[Parameter] public string Mask { get; set; }
        //[Parameter] public string MaskChar { get; set; }
        //[Parameter] public string MaskFormat { get; set; }
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string IconName { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyPress { get; set; }
        [Parameter]
        public Func<string, string> OnGetErrorMessage { get; set; }
        [Parameter]
        public Action<string, string> OnNotifyValidationResult { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }  // expose click event for Combobox and pickers
        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnChange { get; set; }
        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnInput { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();

        private bool firstRendered = false;
        private int deferredValidationTime;
        private bool defaultErrorMessageIsSet;
        private string latestValidatedValue = "";
        private string currentValue;
        private bool hasIcon;
        private bool hasLabel;
        private Rule TextField_Field_HasIcon = new Rule();

        private ICollection<IRule> TextFieldLocalRules { get; set; } = new List<IRule>();
        private ICollection<Task> DeferredValidationTasks = new List<Task>();

        protected string CurrentValue
        {
            get => currentValue;
            set
            {
                if (value == currentValue)
                    return;
                currentValue = value;
                ChangeHandler(new ChangeEventArgs() { Value = value }).ConfigureAwait(true);
            }
        }

        protected ElementReference textAreaRef;
        protected string inlineTextAreaStyle = "";
        protected bool isFocused = false;

        protected override Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                defaultErrorMessageIsSet = true;
            }

            // to prevent changes after initialisation
            deferredValidationTime = DeferredValidationTime;
            hasIcon = !string.IsNullOrWhiteSpace(IconName);
            hasLabel = !string.IsNullOrWhiteSpace(Label);
            if (hasIcon)
            {
                CreateLocalCss();
            }



            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (DefaultValue != null)
                CurrentValue = DefaultValue;

            if (Value != null)
                CurrentValue = Value;

            if (ValidateOnLoad && ValidateAllChanges())
            {
                Validate(CurrentValue);
            }

            return base.OnParametersSetAsync();
        }

        protected override void OnThemeChanged()
        {
            if (hasIcon)
            {
                SetStyle();
            }
        }

        private void CreateLocalCss()
        {
            if (hasIcon)
            {
                TextField_Field_HasIcon.Selector = new ClassSelector() { SelectorName = "ms-TextField-field" };

                TextFieldLocalRules.Add(TextField_Field_HasIcon);
            }
        }


        protected async Task InputHandler(ChangeEventArgs args)
        {
            if (!defaultErrorMessageIsSet && OnGetErrorMessage != null && !string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = "";
                StateHasChanged();
            }
            if (ValidateAllChanges())
            {
                await DeferredValidation((string)args.Value).ConfigureAwait(false);
            }
            await AdjustInputHeightAsync();
            await OnInput.InvokeAsync((string)args.Value);
            //await InputChanged.InvokeAsync((string)args.Value);
            //if (this.OnInput != null)
            //{
            //    await this.OnInput.Invoke(args);
            //}
        }

        protected async Task ChangeHandler(ChangeEventArgs args)
        {
            await OnChange.InvokeAsync((string)args.Value);
        }

        protected async Task OnFocusInternal(FocusEventArgs args)
        {
            if (OnFocus.HasDelegate)
                await OnFocus.InvokeAsync(args);

            isFocused = true;
            if (ValidateOnFocusIn && !defaultErrorMessageIsSet)
            {
                Validate(CurrentValue);
            }
            //return Task.CompletedTask;
        }

        protected async Task OnBlurInternal(FocusEventArgs args)
        {
            if (OnBlur.HasDelegate)
                await OnBlur.InvokeAsync(args);

            isFocused = false;
            if (ValidateOnFocusOut && !defaultErrorMessageIsSet)
            {
                Validate(CurrentValue);
            }
            //return Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRendered)
            {
                firstRendered = true;
                await AdjustInputHeightAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AdjustInputHeightAsync()
        {
            if (this.AutoAdjustHeight == true && this.Multiline)
            {
                var scrollHeight = await JSRuntime.InvokeAsync<double>("BlazorFabricTextField.getScrollHeight", textAreaRef);
                inlineTextAreaStyle = $"height: {scrollHeight}px";
            }
        }

        protected string GetAutoCompleteString()
        {
            var value = AutoComplete.ToString();
            value = Char.ToLowerInvariant(value[0]) + value.Substring(1);
            string result = "";
            foreach (var c in value.ToCharArray())
            {
                if (Char.IsUpper(c))
                {
                    result += "-";
                    result += Char.ToLowerInvariant(c);
                }
                else
                    result += c;
            }
            return result;
        }

        private void Validate(string value)
        {
            if (value == null || latestValidatedValue == value)
                return;

            latestValidatedValue = value;
            string errorMessage = OnGetErrorMessage?.Invoke(value);
            if (errorMessage != null)
            {
                ErrorMessage = errorMessage;
            }
            OnNotifyValidationResult?.Invoke(errorMessage, value);
        }

        private bool ValidateAllChanges()
        {
            return OnGetErrorMessage != null && !defaultErrorMessageIsSet && !ValidateOnFocusIn && !ValidateOnFocusOut;
        }

        private async Task DeferredValidation(string value)
        {
            DeferredValidationTasks.Add(Task.Run(async () =>
            {
                await Task.Delay(deferredValidationTime);
            }));
            var TaskCount = DeferredValidationTasks.Count();
            await Task.WhenAll(DeferredValidationTasks.ToArray());
            if (TaskCount == DeferredValidationTasks.Count())
            {
                _ = Task.Run(() =>
                  {
                      Validate(value);
                      InvokeAsync(() => StateHasChanged());  //invokeasync required for serverside
                  }).ConfigureAwait(false);
            }
        }

        private void SetStyle()
        {
            if (hasIcon)
            {
                TextField_Field_HasIcon.Properties = new CssString()
                {
                    Css = $"padding-right:{(Multiline ? "40px" : "24px")};"
                };
            }
        }

        public ICollection<Rule> CreateGlobalCss(ITheme theme)
        {
            var MyRules = new List<Rule>();
            #region ms-TextField
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                          //Normalize
                          $"box-shadow:none;" +
                          $"margin:0px;" +
                          $"padding:0px;" +
                          $"box-sizing:border-box;"
                }
            });
            #endregion
            #region ms-TextField-wrapper
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined .ms-TextField-wrapper" },
                Properties = new CssString()
                {
                    Css = $"display: flex;" +
                        $"border-bottom:1px solid {theme.SemanticColors.InputBorder};" +
                        $"width:100%;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined.is-disabled .ms-TextField-wrapper" },
                Properties = new CssString()
                {
                    Css = $"border-bottom-color:{theme.SemanticColors.DisabledBackground};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField--underlined.is-disabled .ms-TextField-wrapper{border-color:gray;}"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined:not(.is-disabled):not(.is-focused):not(.has-error) .ms-TextField-wrapper:hover" },
                Properties = new CssString()
                {
                    Css = $"border-bottom-color:{theme.SemanticColors.InputBorderHovered};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined:not(.is-disabled):not(.is-focused).has-error .ms-TextField-wrapper:hover" },
                Properties = new CssString()
                {
                    Css = $"border-bottom-color:{theme.SemanticTextColors.ErrorText};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField--underlined:not(.is-disabled):not(.is-focused) .ms-TextField-wrapper:hover{border-bottom-color:Highlight;}"
                }
            });
            var WrapperFocusRules = FocusStyle.GetInputFocusStyle(new FocusStyleProps(theme) { BorderColor = theme.SemanticColors.InputFocusBorderAlt, BorderRadius = "0", }, ".ms-TextField--underlined.is-focused .ms-TextField-wrapper", true);
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined.is-focused .ms-TextField-wrapper" },
                Properties = new CssString()
                {
                    Css = "position:relative;" + WrapperFocusRules.MergeRules
                }
            });
            MyRules.AddRange(WrapperFocusRules.AddRules);
            var WrapperFocusErrorRules = FocusStyle.GetInputFocusStyle(new FocusStyleProps(theme) { BorderColor = theme.SemanticTextColors.ErrorText, BorderRadius = "0", }, ".ms-TextField--underlined.is-focused.has-error .ms-TextField-wrapper", true);
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined.is-focused .ms-TextField-wrapper" },
                Properties = new CssString()
                {
                    Css = "position:relative;" + WrapperFocusErrorRules.MergeRules
                }
            });
            MyRules.AddRange(WrapperFocusErrorRules.AddRules);
            #endregion
            #region ms-TextField-fieldGroup
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = //Normalize
                          $"box-shadow:none;" +
                          $"margin:0px;" +
                          $"padding:0px;" +
                          $"box-sizing:border-box;"+

                        $"border:1px solid {theme.SemanticColors.InputBorder};" +
                        $"border-radius:{theme.Effects.RoundedCorner2};" +
                        $"background:{theme.SemanticColors.InputBackground};" +
                        $"cursor:text;" +
                        $"height:32px;" +
                        $"display:flex;" +
                        $"flex-direction: row;" +
                        $"align-items:stretch;" +
                        $"position:relative;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--multiline .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"min-height:60px;" +
                        $"height:auto;" +
                        $"display:flex;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField:not(.is-focused):not(.is-disabled):not(.has-error) .ms-TextField-fieldGroup:hover" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.InputBorderHovered};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField:not(.is-focused):not(.is-disabled):not(.has-error) .ms-TextField-fieldGroup:hover{border-color:Highlight;}"
                }
            });
            var FieldGroupFocusRules = FocusStyle.GetInputFocusStyle(new FocusStyleProps(theme) { BorderColor = theme.SemanticColors.InputFocusBorderAlt, BorderRadius = theme.Effects.RoundedCorner2, }, ".ms-TextField.is-focused:not(.ms-TextField--underlined) .ms-TextField-fieldGroup");
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-focused:not(.ms-TextField--underlined) .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = FieldGroupFocusRules.MergeRules
                }
            });
            MyRules.AddRange(FieldGroupFocusRules.AddRules);
            var FieldGroupErrorFocusRules = FocusStyle.GetInputFocusStyle(new FocusStyleProps(theme) { BorderColor = theme.SemanticTextColors.ErrorText, BorderRadius = theme.Effects.RoundedCorner2, }, ".ms-TextField.is-focused.has-error:not(.ms-TextField--underlined) .ms-TextField-fieldGroup");
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-focused.has-error:not(.ms-TextField--underlined) .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = FieldGroupErrorFocusRules.MergeRules
                }
            });
            MyRules.AddRange(FieldGroupErrorFocusRules.AddRules);
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-disabled .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticColors.DisabledBackground};" +
                        $"cursor:default;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField.is-disabled .ms-TextField-fieldGroup{border-color:gray;}"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--borderless .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"border:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--borderless.is-focused .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"border:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--borderless.is-focused .ms-TextField-fieldGroup:after" },
                Properties = new CssString()
                {
                    Css = $"border:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"flex:1 1 0px;" +
                        $"border:none;" +
                        $"text-align:left;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined.is-disabled .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"background:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.has-error:not(.ms-TextField--underlined) .ms-TextField-fieldGroup" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticTextColors.ErrorText};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.has-error:not(.ms-TextField--underlined) .ms-TextField-fieldGroup:hover" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.SemanticTextColors.ErrorText};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.no-label.is-required .ms-TextField-fieldGroup:before"},
                Properties = new CssString()
                {
                    Css = $"content:'*';" +
                        $"color:{theme.SemanticTextColors.ErrorText};" +
                        $"position:absolute;" +
                        $"top:-5px;" +
                        $"right:-10px;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField.no-label.is-required .ms-TextField-fieldGroup:before{right:-14px;}"
                }
            });
            #endregion
            #region ms-TextField-field
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-field" },
                Properties = new CssString()
                {
                    Css = //Normalize
                          $"box-shadow:none;" +
                          $"margin:0px;" +
                          $"padding:0px;" +
                          $"box-sizing:border-box;" +

                        $"border-radius:0;" +
                        $"border:none;" +
                        $"background:none;" +
                        $"background-color:transparent;" +
                        $"color:{theme.SemanticTextColors.InputText};" +
                        $"padding:0px 8px;" +
                        $"width:100%;" +
                        $"text-overflow:ellipsis;" +
                        $"outline:0;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-field:active,.ms-TextField-field:focus,.ms-TextField-field:hover" },
                Properties = new CssString()
                {
                    Css = $"outline:0;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-field::-ms-clear" },
                Properties = new CssString()
                {
                    Css = $"display:none;"
                }
            });
            MyRules.AddRange(PlaceHolderStyle.GetPlaceholderStyle(".ms-TextField-field", 
                new CssString() 
                { 
                    Css = $"color:{theme.SemanticTextColors.InputPlaceholderText};" +
                        $"opacity:1;" 
                }));
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--multiline.ms-TextField--unresizable .ms-TextField-field" },
                Properties = new CssString()
                {
                    Css = $"resize:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--multiline .ms-TextField-field" },
                Properties = new CssString()
                {
                    Css = $"min-height:inherit;" +
                        $"line-height:17px;" +
                        $"flex-grow:1;" +
                        $"padding-top:6px;" +
                        $"padding-bottom:6px;" +
                        $"overflow:auto;" +
                        $"width:100%;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--multiline.auto-adjust-height .ms-TextField-field" },
                Properties = new CssString()
                {
                    Css = $"overflow:hidden;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-disabled .ms-TextField-field" },
                Properties = new CssString()
                {
                    Css = $"backgroundColor:{theme.SemanticColors.DisabledBackground};" +
                        $"color:{theme.SemanticTextColors.DisabledText};" +
                        $"border-color:{theme.SemanticColors.DisabledBackground};"
                }
            });
            MyRules.AddRange(PlaceHolderStyle.GetPlaceholderStyle(".ms-TextField.is-disabled .ms-TextField-field",
                new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};"
                }));
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--underlined .ms-TextField-field" },
                Properties = new CssString()
                {
                    Css = $"text-align:left;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField.is-focused:not(.ms-TextField--borderless) .ms-TextField-field{padding-left:11px;padding-right:11px;}" +
                        ".ms-TextField.is-focused.ms-TextField--multiline:not(.ms-TextField--borderless) .ms-TextField-field{padding-top:4px;}"
                }
            });
            #endregion
            #region ms-TextField-icon
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--multiline .ms-TextField-icon" },
                Properties = new CssString()
                {
                    Css = $"padding-right:24px;" +
                        $"align-items:flex-end;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-icon" },
                Properties = new CssString()
                {
                    Css = $"pointer-events:none;" +
                        $"position:absolute;" +
                        $"bottom:6px;" +
                        $"right:8px;" +
                        $"top:auto;" +
                        $"font-size:{theme.FontStyle.FontSize.Medium};" +
                        $"line-height:18px;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-disabled .ms-TextField-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};"
                }
            });
            #endregion
            #region ms-TextField-description
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-description" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.BodySubtext};" +
                        $"font-size:{theme.FontStyle.FontSize.XSmall};"
                }
            });
            #endregion
            #region ms-TextField-prefix
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-prefix" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticColors.DisabledBackground};" +
                        $"color:{theme.SemanticTextColors.InputPlaceholderText};" +
                        $"display:flex;" +
                        $"align-items:center;" +
                        $"padding:0px 10px;" +
                        $"line-height:1px;" +
                        $"white-space:nowrap;" +
                        $"flex-shrink:0;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-disabled .ms-TextField-prefix" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};"
                }
            });
            #endregion
            #region ms-TextField-suffix
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField-suffix" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.SemanticColors.DisabledBackground};" +
                        $"color:{theme.SemanticTextColors.InputPlaceholderText};" +
                        $"display:flex;" +
                        $"align-items:center;" +
                        $"padding:0px 10px;" +
                        $"line-height:1px;" +
                        $"white-space:nowrap;" +
                        $"flex-shrink:0;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.is-disabled .ms-TextField-suffix" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};"
                }
            });
            #endregion
            #region ms-TextField--errorMessage
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField--errorMessage" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};"+
                        $"color:{theme.SemanticTextColors.ErrorText};" +
                        $"margin:0;" +
                        $"padding-top:5px;" +
                        $"display:flex;" +
                        $"align-items:center;"
                }
            });
            #endregion
            #region SubComponentStyle_Label
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.ms-TextField--underlined.is-disabled .ms-Label" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralTertiary};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-TextField.ms-TextField--underlined .ms-Label" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.Medium};" +
                        $"margin-right:8px;" +
                        $"padding-left:12px;" +
                        $"padding-right:0px;" +
                        $"line-height:22px;" +
                        $"height:32px;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-TextField.ms-TextField--underlined.is-focused .ms-Label{height:31px;}"
                }
            });
            #endregion

            return MyRules;
        }
    }
}
