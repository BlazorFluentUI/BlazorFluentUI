using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class ResponsiveFabricComponentBase : FabricComponentBase, IDisposable
    {
        private string _resizeRegistration;

        [Inject]
        private IJSRuntime jSRuntime { get; set; }

        protected ResponsiveMode CurrentMode { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var windowRect = await jSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.getWindowRect");
                foreach (var item in Enum.GetValues(typeof(ResponsiveMode)))
                {
                    if (windowRect.width <= ResponsiveModeUtils.RESPONSIVE_MAX_CONSTRAINT[(int)item])
                    {
                        CurrentMode = (ResponsiveMode)item;
                        break;
                    }
                }
                Debug.WriteLine($"ResponsiveMode: {CurrentMode}");

                _resizeRegistration = await jSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "OnResizedAsync");
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
                Debug.WriteLine($"ResponsiveMode: {CurrentMode}");
                StateHasChanged();
            }
            return Task.CompletedTask;
        }

        public virtual async void Dispose()
        {
            if (_resizeRegistration != null)
            {
                await jSRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.deregisterResizeEvent", _resizeRegistration);
                Debug.WriteLine($"ResponsiveFabricComponentBase unregistered");
                _resizeRegistration = null;
            }
        }
    }
}
