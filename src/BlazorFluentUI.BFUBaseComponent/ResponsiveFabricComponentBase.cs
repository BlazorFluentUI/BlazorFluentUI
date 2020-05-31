using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public class BFUResponsiveComponentBase : BFUComponentBase, IDisposable
    {
        private string _resizeRegistration;

        [Inject]
        private IJSRuntime jSRuntime { get; set; }

        protected ResponsiveMode CurrentMode { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var windowRect = await jSRuntime.InvokeAsync<Rectangle>("BlazorFluentUiBaseComponent.getWindowRect");
                foreach (var item in Enum.GetValues(typeof(ResponsiveMode)))
                {
                    if (windowRect.width <= ResponsiveModeUtils.RESPONSIVE_MAX_CONSTRAINT[(int)item])
                    {
                        CurrentMode = (ResponsiveMode)item;
                        break;
                    }
                }
                //Debug.WriteLine($"ResponsiveMode: {CurrentMode}");

                _resizeRegistration = await jSRuntime.InvokeAsync<string>("BlazorFluentUiBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "OnResizedAsync");
                StateHasChanged();  // we will never have window size until after first render, so re-render after this to update the component with ResponsiveMode info.
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public virtual Task OnResizedAsync(double windowWidth, double windowHeight)
        {
            var oldMode = CurrentMode;
            foreach (var item in Enum.GetValues(typeof(ResponsiveMode)))
            {
                if (windowWidth <= ResponsiveModeUtils.RESPONSIVE_MAX_CONSTRAINT[(int)item])
                {
                    CurrentMode = (ResponsiveMode)item;
                    break;
                }
            }

            if (oldMode != CurrentMode)
            {
                //Debug.WriteLine($"ResponsiveMode: {CurrentMode}");
                StateHasChanged();
            }
            return Task.CompletedTask;
        }

        public virtual async void Dispose()
        {
            if (_resizeRegistration != null)
            {
                await jSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.deregisterResizeEvent", _resizeRegistration);
                //Debug.WriteLine($"BFUResponsiveComponentBase unregistered");
                _resizeRegistration = null;
            }
        }
    }
}
