using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorFluentUI
{
    public partial class TooltipHost : FluentUIComponentBase, IDisposable
    {
        private static TooltipHost? CurrentVisibleTooltip { get; set; }

        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public double CloseDelay { get; set; } = double.NaN;
        [Parameter] public TooltipDelay Delay { get; set; } = TooltipDelay.Medium;
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.TopCenter;
        //[Parameter] public FluentUIComponentBase FabricComponentTarget { get; set; }
        [Parameter] public string? HostClassName { get; set; }
        [Parameter] public EventCallback<bool> OnTooltipToggle { get; set; }
        [Parameter] public TooltipOverflowMode OverflowMode { get; set; } = TooltipOverflowMode.None;
        [Parameter] public FluentUIComponentBase? Parent { get; set; }
        [Parameter] public bool SetAriaDescribedBy { get; set; }
        [Parameter] public RenderFragment? TooltipContent { get; set; }

        protected FluentUIComponentBase? TargetElement;
        protected bool ShowTooltip;

        protected bool IsTooltipVisible = false;
        protected bool IsAriaPlaceholderRendered = false;

        private Timer? openTimer;
        private Timer? dismissTimer;


        protected override void OnInitialized()
        {
            openTimer = new Timer();
            openTimer.Elapsed += openTimer_Elapsed;
            dismissTimer = new Timer();
            dismissTimer.Elapsed += dismissTimer_Elapsed;
            base.OnInitialized();
        }

        private void openTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                openTimer?.Stop();
                ToggleTooltip(true);
            });
        }

        private void dismissTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                dismissTimer?.Stop();
                ToggleTooltip(false);
            });
        }

        public void Show()
        {
            ToggleTooltip(true);
        }

        public void Dismiss()
        {
            ToggleTooltip(false);
        }

        protected override Task OnParametersSetAsync()
        {
            DetermineTargetElement();
            return base.OnParametersSetAsync();
        }

        protected Task OnTooltipMouseEnter(EventArgs args)
        {
            Debug.WriteLine("OnMouseEnter");
            if (CurrentVisibleTooltip != null && CurrentVisibleTooltip != this)
                CurrentVisibleTooltip.Dismiss();

            CurrentVisibleTooltip = this;

            if (OverflowMode != TooltipOverflowMode.None)
            {
                DetermineTargetElement();
                // test for overflow... return and do nothing if there is none
                // for now, let's not show the tooltip until the detection for overflow works
                return Task.CompletedTask;
            }

            // do another test to see if tooltip target is inside a portal relative to the tooltiphost.  Probably won't deal with this for a while.
            // return and do nothing if so...

            dismissTimer?.Stop();
            openTimer?.Stop();

            if (Delay != TooltipDelay.Zero)
            {
                IsAriaPlaceholderRendered = true;
                double delayTime = GetDelayTime(Delay);
                if (openTimer != null)
                {
                    openTimer.Interval = delayTime;
                    openTimer.Start();
                }
            }
            else
            {
                ToggleTooltip(true);
            }

            return Task.CompletedTask;
        }

        protected Task OnTooltipMouseLeave(EventArgs args)
        {
            if (dismissTimer != null)  // component can be disposed already and still return this event
            {
                Debug.WriteLine("OnMouseLeave");
                dismissTimer.Stop();
                openTimer?.Stop();

                if (!double.IsNaN(CloseDelay))
                {
                    dismissTimer.Interval = CloseDelay;
                    dismissTimer.Start();
                }
                else
                {
                    ToggleTooltip(false);
                }

                if (TooltipHost.CurrentVisibleTooltip == this)
                {
                    TooltipHost.CurrentVisibleTooltip = null;
                }
            }
            return Task.CompletedTask;
        }

        protected void HideTooltip()
        {
            ToggleTooltip(false);
        }

        protected Task OnTooltipKeyDown(KeyboardEventArgs args)
        {
            if (args.Code == "Esc")
                HideTooltip();
            return Task.CompletedTask;
        }

        private void ToggleTooltip(bool isOpen)
        {
            Debug.WriteLine($"Toggling tooltip: {isOpen}");

            IsTooltipVisible = isOpen;
            IsAriaPlaceholderRendered = false;
            StateHasChanged();
            Debug.WriteLine($"Toggling statehaschanged");

            OnTooltipToggle.InvokeAsync(isOpen);

        }

        private void DetermineTargetElement()
        {
            if (OverflowMode == TooltipOverflowMode.Parent && Parent != null)
                TargetElement = Parent;
            else
                TargetElement = this;
        }

        private static double GetDelayTime(TooltipDelay delay)
        {
            return delay switch
            {
                TooltipDelay.Medium => 300,
                TooltipDelay.Long => 500,
                _ => 0,
            };
        }

        public void Dispose()
        {
            dismissTimer?.Stop();
            openTimer?.Stop();
            dismissTimer = null;
            openTimer = null;

            if (CurrentVisibleTooltip == this)
                CurrentVisibleTooltip = null;
        }
    }
}
