using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorFluentUI
{
    public partial class BFUSlider : BFUComponentBase, IHasPreloadableGlobalStyle, IDisposable
    {
        [Parameter] public Func<double,string>? AriaValueText { get; set; }
        [Parameter] public double? DefaultValue { get; set; }
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public string? Label { get; set; }
        [Parameter] public string? LineContainerClassName { get; set; }
        [Parameter] public double Max { get; set; } = 10.0;
        [Parameter] public double Min { get; set; } = 0.0;
        [Parameter] public bool OriginFromZero { get; set; } = false;
        [Parameter] public bool ShowValue { get; set; } = true;
        [Parameter] public bool SnapToStep { get; set; } = false;
        [Parameter] public double Step { get; set; } = 1.0;
        [Parameter] public string? TitleLabelClassName { get; set; }
        [Parameter] public double? Value { get; set; }
        [Parameter] public EventCallback<double> ValueChanged { get; set; }
        [Parameter] public Func<double,string>? ValueFormat { get; set; }
        [Parameter] public bool Vertical { get; set; }

        [Inject] private IJSRuntime? JSRuntime { get; set; }


        private string id = Guid.NewGuid().ToString();
        private ElementReference slideBox;
        private ElementReference sliderLine;
        private ElementReference thumb;
        private double zeroOffsetPercent;
        private double thumbOffsetPercent;
        private double _renderedValue;
        private bool showTransitions;
        private bool initialValueSet = false;
        private double value;
        private bool jsAvailable;
        private DotNetObjectReference<BFUSlider>? dotNetObjectReference;
        private Timer timer = new Timer();

        private string lengthString => (Vertical? "height" : "width");

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-Slider"},
            {"enabled", "ms-Slider-enabled"},
            {"disabled", "ms-Slider-disabled"},
            {"row", "ms-Slider-row"},
            {"column", "ms-Slider-column"},
            {"container", "ms-Slider-container"},
            {"slideBox", "ms-Slider-slideBox"},
            {"line", "ms-Slider-line"},
            {"thumb", "ms-Slider-thumb"},
            {"activeSection", "ms-Slider-active"},
            {"inactiveSection", "ms-Slider-inactive"},
            {"valueLabel", "ms-Slider-value"},
            {"showValue", "ms-Slider-showValue"},
            {"showTransitions", "ms-Slider-showTransitions"},
            {"zeroTick", "ms-Slider-zeroTick"},
            {"titleLabel", "ms-Slider-titleLabel" },
            {"lineContainer", "ms-Slider-lineContainer" }
        };
        private bool shouldFocus;

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var pressedActiveSectionColor = theme.SemanticColors.InputBackgroundCheckedHovered;
            var hoveredActiveSectionColor = theme.SemanticColors.InputBackgroundChecked;
            var hoveredPressedinactiveSectionColor = theme.SemanticColors.InputPlaceholderBackgroundChecked;
            var restActiveSectionColor = theme.SemanticColors.SmallInputBorder;
            var restInactiveSectionColor = theme.SemanticColors.DisabledBorder;

            var disabledActiveSectionColor = theme.SemanticTextColors.DisabledText;
            var disabledInactiveSectionColor = theme.SemanticColors.DisabledBackground;

            var thumbBackgroundColor = theme.SemanticColors.InputBackground;
            var thumbBorderColor = theme.SemanticColors.SmallInputBorder;
            var thumbDisabledBorderColor = theme.SemanticColors.DisabledBorder;

            var sliderRules = new HashSet<IRule>();

            #region root
            sliderRules.AddCssStringSelector($".{GlobalClassNames["root"]}").AppendCssStyles(
                $"font-size:{theme.FontStyle.FontSize.Medium}",
                $"font-weight:{theme.FontStyle.FontWeight.Regular}",
                "user-select:none"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["root"]}.{GlobalClassNames["column"]}").AppendCssStyles(
                "margin-right:8px"
            );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["titleLabel"]}").AppendCssStyles(
                 "padding:0"
             );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["container"]}").AppendCssStyles(
                 "display:flex",
                 "flex-wrap:nowrap",
                 "align-items:center"
             );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["container"]}").AppendCssStyles(
                 "flex-direction:column",
                 "height:100%",
                 "text-align:center",
                 "margin:8px 0"
             );
            #endregion

            #region slideBox
            var focusStyleProps = new FocusStyleProps(theme); //default everything
            var props = FocusStyle.GetFocusStyle(focusStyleProps, $".{GlobalClassNames["slideBox"]}");

            foreach (var rule in props.AddRules)
            {
                sliderRules.Add(rule);
            }

            sliderRules.AddCssStringSelector($".{GlobalClassNames["slideBox"]}")
                .AppendCssStyles(props.MergeRulesList.ToArray())
                .AppendCssStyles(
                     "background:transparent",
                     "border:none",
                     "flex-grow:1",
                     "line-height:28",
                     "display:flex",
                     "align-items:center"
             );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["activeSection"]}")
                .AppendCssStyles(
                $"background-color:{pressedActiveSectionColor}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["activeSection"]} {{background-color:Highlight;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["activeSection"]}")
                .AppendCssStyles(
                $"background-color:{hoveredActiveSectionColor}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["activeSection"]} {{background-color:Highlight;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["inactiveSection"]}")
                .AppendCssStyles(
                $"background-color:{hoveredPressedinactiveSectionColor}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["inactiveSection"]} {{background-color:Highlight;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["inactiveSection"]}")
                .AppendCssStyles(
                $"background-color:{hoveredPressedinactiveSectionColor}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["inactiveSection"]} {{background-color:Highlight;}}"
                }
            });


            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["thumb"]}")
                .AppendCssStyles(
                $"border:2px solid {pressedActiveSectionColor}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["thumb"]} {{border-color:Highlight;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["thumb"]}")
                .AppendCssStyles(
                $"border:2px solid {pressedActiveSectionColor}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["thumb"]} {{border-color:Highlight;}}"
                }
            });


            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["zeroTick"]}")
                .AppendCssStyles(
                $"background-color:{theme.SemanticColors.InputPlaceholderBackgroundChecked}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:active .{GlobalClassNames["zeroTick"]} {{background-color:Highlight;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["zeroTick"]}")
                .AppendCssStyles(
                $"background-color:{theme.SemanticColors.InputPlaceholderBackgroundChecked}"
                );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["enabled"]} .{GlobalClassNames["slideBox"]}:hover .{GlobalClassNames["zeroTick"]} {{background-color:Highlight;}}"
                }
            });


            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["slideBox"]}")
                .AppendCssStyles(props.MergeRulesList.ToArray())
                .AppendCssStyles(
                     "height:100%",
                     "width:28px",
                     "padding:8px 0"
             );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["row"]} .{GlobalClassNames["slideBox"]}")
               .AppendCssStyles(props.MergeRulesList.ToArray())
               .AppendCssStyles(
                    "height:28px",
                    "width:auto",
                    "padding:0 8px"
            );

#endregion

            #region thumb
            sliderRules.AddCssStringSelector($".{GlobalClassNames["thumb"]}")
               .AppendCssStyles(
                    "border-width:2px",
                    "border-style:solid",
                    $"border-color:{thumbBorderColor}",
                    "border-radius:10px",
                    "box-sizing:border-box",
                    $"background:{thumbBackgroundColor}",
                    "display:block",
                    "width:16px",
                    "height:16px",
                    "position:absolute"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["thumb"]}")
               .AppendCssStyles(
                    "left:-6px",
                    "margin:0 auto",
                    "transform:translateY(8px)"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["row"]} .{GlobalClassNames["thumb"]}")
               .AppendCssStyles(
                    "top:-6px",
                    "transform:translateX(-50%)" //skip RTL
            );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["showTransitions"]} .{GlobalClassNames["thumb"]}")
               .AppendCssStyles(
                    $"transition:left {theme.Animation.Duration3} {theme.Animation.EasingFunction1}"
            );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["disabled"]} .{GlobalClassNames["thumb"]}")
               .AppendCssStyles(
                    $"border-color:{thumbDisabledBorderColor}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["disabled"]} .{GlobalClassNames["thumb"]} {{border-color:GrayText;}}"
                }
            });

            #endregion

            #region line
            sliderRules.AddCssStringSelector($".{GlobalClassNames["line"]}")
               .AppendCssStyles(
                    "display:flex",
                    "position:relative"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["line"]}")
               .AppendCssStyles(
                    "height:100%",
                    "width:4px",
                    "margin:0 auto",
                    "flex-direction:column-reverse"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["row"]} .{GlobalClassNames["line"]}")
               .AppendCssStyles(
                    "width:100%"
            );

            #endregion

            #region lineContainer
            sliderRules.AddCssStringSelector($".{GlobalClassNames["lineContainer"]}")
               .AppendCssStyles(
                    "border-radius:4px",
                    "box-sizing:border-box"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["lineContainer"]}")
               .AppendCssStyles(
                    "height:100%",
                    "width:4px"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["row"]} .{GlobalClassNames["lineContainer"]}")
               .AppendCssStyles(
                    "width:100%",
                    "height:4px"
            );

            #endregion

            #region activeSection
            sliderRules.AddCssStringSelector($".{GlobalClassNames["activeSection"]}")
               .AppendCssStyles(
                    $"background:{restActiveSectionColor}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["activeSection"]} {{background-color:WindowText;}}"
                }
            });


            sliderRules.AddCssStringSelector($".{GlobalClassNames["showTransitions"]} .{GlobalClassNames["activeSection"]}")
               .AppendCssStyles(
                    $"transition:width {theme.Animation.Duration3} {theme.Animation.EasingFunction1}"
            );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["disabled"]} .{GlobalClassNames["activeSection"]}")
               .AppendCssStyles(
                    $"background:{disabledActiveSectionColor}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["disabled"]} .{GlobalClassNames["activeSection"]} {{background-color:GrayText;border-color:GrayText;}}"
                }
            });

            #endregion

            #region inactiveSection
            sliderRules.AddCssStringSelector($".{GlobalClassNames["inactiveSection"]}")
               .AppendCssStyles(
                    $"background:{restInactiveSectionColor}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["inactiveSection"]} {{border:1px solid WindowText;}}"
                }
            });


            sliderRules.AddCssStringSelector($".{GlobalClassNames["showTransitions"]} .{GlobalClassNames["inactiveSection"]}")
               .AppendCssStyles(
                    $"transition:width {theme.Animation.Duration3} {theme.Animation.EasingFunction1}"
            );

            sliderRules.AddCssStringSelector($".{GlobalClassNames["disabled"]} .{GlobalClassNames["inactiveSection"]}")
               .AppendCssStyles(
                    $"background:{disabledInactiveSectionColor}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["disabled"]} .{GlobalClassNames["inactiveSection"]} {{border-color:GrayText;}}"
                }
            });

            #endregion

            #region zeroTick
            sliderRules.AddCssStringSelector($".{GlobalClassNames["zeroTick"]}")
               .AppendCssStyles(
                    "position:absolute",
                    $"background:{theme.SemanticColors.DisabledBorder}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["zeroTick"]} {{background-color:WindowText;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["disabled"]} .{GlobalClassNames["zeroTick"]}")
               .AppendCssStyles(
                    $"background:{theme.SemanticColors.DisabledBackground}"
            );
            sliderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".{GlobalClassNames["disabled"]} .{GlobalClassNames["zeroTick"]} {{background-color:GrayText;}}"
                }
            });

            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["zeroTick"]}")
              .AppendCssStyles(
                   "width:16px",
                   "height:1px",
                   "transform:translateX(-6px)" //skip RTL
           );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["row"]} .{GlobalClassNames["zeroTick"]}")
               .AppendCssStyles(
                    "width:1px",
                    "height:16px",
                   "transform:translateY(-6px)"
            );

            #endregion

            #region valueLabel
            sliderRules.AddCssStringSelector($".{GlobalClassNames["valueLabel"]}")
               .AppendCssStyles(
                    "flex-shrink:1",
                    "width:30px",
                    "line-height:'1'" //string meaning relative to size of font
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["column"]} .{GlobalClassNames["valueLabel"]}")
               .AppendCssStyles(
                    "margin:0 auto",
                    "white-space:nowrap",
                    "width:40px"
            );
            sliderRules.AddCssStringSelector($".{GlobalClassNames["row"]} .{GlobalClassNames["valueLabel"]}")
              .AppendCssStyles(
                   "margin:0 8px",
                   "white-space:nowrap",
                   "width:40px"
           );

            #endregion



            return sliderRules;
        }

        private void UpdateState()
        {
            thumbOffsetPercent = Min == Max ? 0 : ((_renderedValue - Min) / (Max - Min)) * 100.0;
            
            zeroOffsetPercent = Min >= 0 ? 0 : (-Min / (Max - Min)) * 100;
            //lengthString = (set as property)
            showTransitions = _renderedValue == value;
            StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            timer.Interval = 1000;
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            return base.OnInitializedAsync();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _ = InvokeAsync(() => _ = ValueChanged.InvokeAsync(value));
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Value.HasValue)
            {
                value = Value.Value;
                _renderedValue = value;
            }
            else if (DefaultValue.HasValue && !initialValueSet)
            {
                initialValueSet = true;
                value = DefaultValue.Value;
                _renderedValue = value;
            }

            if (jsAvailable)
            {
                if (Disabled && dotNetObjectReference != null)
                {
                    _ = JSRuntime?.InvokeVoidAsync("BlazorFluentUiSlider.unregisterHandler", dotNetObjectReference);
                    dotNetObjectReference.Dispose();
                    dotNetObjectReference = null;
                }
                else if (!Disabled && dotNetObjectReference == null)
                {
                    dotNetObjectReference = DotNetObjectReference.Create(this);
                    await JSRuntime.InvokeVoidAsync("BlazorFluentUiSlider.registerMouseOrTouchStart", dotNetObjectReference, slideBox, sliderLine);
                }
            }

            UpdateState();
            
            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender) 
            {
                jsAvailable = true;
                if (JSRuntime != null && !Disabled)
                {
                    dotNetObjectReference = DotNetObjectReference.Create(this);
                    await JSRuntime.InvokeVoidAsync("BlazorFluentUiSlider.registerMouseOrTouchStart", dotNetObjectReference, slideBox, sliderLine);
                }
            }

            if (shouldFocus)
            {
                shouldFocus = false;
                await Focus();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public void OnKeyDown(KeydownSliderArgs args)
        {
            double diff = 0;
            var current = value;
            if (args.Min)
            {
                current = Min;
            }
            else if (args.Max)
            {
                current = Max;
            }
            else
            {
                diff = args.Step;
            }

            var newValue = Math.Min(Max, Math.Max(Min, current + diff));
            UpdateValue(newValue, newValue);

            ClearOnKeyDownTimer();
            SetOnKeyDownTimer();
        }

        public async Task Focus()
        {
            await JSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.focusElement", slideBox).ConfigureAwait(false);
        }

        private void ClearOnKeyDownTimer()
        {
            timer.Stop();
        }

        private void SetOnKeyDownTimer()
        {
            timer.Start();
        }

        private string GetAriaValueText(double value)
        {
            if (AriaValueText != null)
                return AriaValueText(value);
            else
                return value.ToString();
        }

        [JSInvokable]
        public void MouseOrTouchMove(ManualRectangle rect, double horizontalPosition, double verticalPosition)
        {
            //Debug.WriteLine($"rect:{rect.left} {rect.right}  horiz: {horizontalPosition}");
            var steps = (Max - Min) / Step;
            //Debug.WriteLine($"steps: {steps}");
            var sliderLength = Vertical ? rect.height : rect.width;
            //Debug.WriteLine($"sliderLength: {sliderLength}");
            var stepLength = sliderLength / steps;
            //Debug.WriteLine($"stepLength: {stepLength}");

            double distance;
            double currentSteps;

            if (!Vertical)
            {
                distance = horizontalPosition - rect.left;
            }
            else
            {
                distance = rect.bottom - verticalPosition;
            }
            //Debug.WriteLine($"distance: {distance}");
            currentSteps = distance / stepLength;
            //Debug.WriteLine($"currentSteps: {currentSteps}");

            double currentValue;
            double renderedValue;

            if (currentSteps > Math.Floor(steps))
            {
                renderedValue = currentValue = Max;
            }
            else if (currentSteps < 0)
            {
                renderedValue = currentValue = Min;
            }
            else
            {
                renderedValue = Min + Step * currentSteps;
                currentValue = Min + Step * Math.Round(currentSteps);
            }
            
            UpdateValue(currentValue, renderedValue);

        }

        [JSInvokable]
        public void MouseOrTouchEnd()
        {
            _renderedValue = value;  // _renderedValue is only different during mouse move... falls back to Value when done.
            UpdateState();
            _ = ValueChanged.InvokeAsync(value);

            shouldFocus = true;
        }

        public void UpdateValue(double value, double renderedValue)
        {
            int numDec = 0;
            if (!double.IsInfinity(Step!))
            {
                while (Math.Round(Step * Math.Pow(10, numDec)) / Math.Pow(10, numDec) != Step)
                {
                    numDec++;
                }
            }


            // Make sure value has correct number of decimal places based on number of decimals in step
            var roundedValue = double.Parse(value.ToString($"F{numDec}"));
            var valueChanged = roundedValue != value;

            if (SnapToStep)
            {
                renderedValue = roundedValue;
            }

            this.value = roundedValue;
            _renderedValue = renderedValue;
            UpdateState();
            if (valueChanged)
            {
                _ = ValueChanged.InvokeAsync(value);
            }
        }

    private string GetStyleUsingOffsetPercent(bool vertical, double thumbOffsetPercent)
        {
            var direction = vertical ? "bottom" : "left";  // skipping RTL
            return $"{direction}:{thumbOffsetPercent}%;";
        }

        public void Dispose()
        {
            if (dotNetObjectReference != null)
            {
                _ = JSRuntime?.InvokeVoidAsync("BlazorFluentUiSlider.unregisterHandler", dotNetObjectReference);
            }
            dotNetObjectReference?.Dispose();
            
        }
    }
}
