using FluentUI.Style;
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

namespace FluentUI
{
    public partial class Slider : FluentUIComponentBase, IAsyncDisposable
    {
        [Parameter] public Func<double, string>? AriaValueText { get; set; }
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
        [Parameter] public Func<double, string>? ValueFormat { get; set; }
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
        private DotNetObjectReference<Slider>? dotNetObjectReference;
        private Timer timer = new Timer();

        private string lengthString => (Vertical ? "height" : "width");

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
                    _ = JSRuntime?.InvokeVoidAsync("FluentUISlider.unregisterHandler", dotNetObjectReference);
                    dotNetObjectReference.Dispose();
                    dotNetObjectReference = null;
                }
                else if (!Disabled && dotNetObjectReference == null)
                {
                    dotNetObjectReference = DotNetObjectReference.Create(this);
                    await JSRuntime.InvokeVoidAsync("FluentUISlider.registerMouseOrTouchStart", dotNetObjectReference, slideBox, sliderLine);
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
                    await JSRuntime.InvokeVoidAsync("FluentUISlider.registerMouseOrTouchStart", dotNetObjectReference, slideBox, sliderLine);
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
            await JSRuntime.InvokeVoidAsync("FluentUIBaseComponent.focusElement", slideBox).ConfigureAwait(false);
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

        public async ValueTask DisposeAsync()
        {
            if (dotNetObjectReference != null && JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("FluentUISlider.unregisterHandlers", dotNetObjectReference);
            }
            dotNetObjectReference?.Dispose();
        }

    }
}
