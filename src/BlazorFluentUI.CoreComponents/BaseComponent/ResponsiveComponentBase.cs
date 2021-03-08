using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public class ResponsiveComponentBase : FluentUIComponentBase, IAsyncDisposable
    {
        //STATE
        private bool _jsAvailable;
        private string? _resizeEventToken;
        private ValueTask<string> _resizeEventTokenTask;  // WARNING - can only await this ONCE

        [Inject] IJSRuntime? JSRuntime { get; set; }

        protected ResponsiveMode CurrentMode { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsAvailable = true;
                Rectangle? windowRect = await JSRuntime!.InvokeAsync<Rectangle>("FluentUIBaseComponent.getWindowRect");
                foreach (object? item in Enum.GetValues(typeof(ResponsiveMode)))
                {
                    if (windowRect.width <= ResponsiveModeUtils.RESPONSIVE_MAX_CONSTRAINT[(int)item])
                    {
                        CurrentMode = (ResponsiveMode)item;
                        break;
                    }
                }
                _resizeEventTokenTask = JSRuntime!.InvokeAsync<string>("FluentUIBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "ResizeHappenedAsync");
                StateHasChanged();  // we will never have window size until after first render, so re-render after this to update the component with ResponsiveMode info.
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public virtual Task OnResizedAsync(double windowWidth, double windowHeight)
        {
            ResponsiveMode oldMode = CurrentMode;
            foreach (object? item in Enum.GetValues(typeof(ResponsiveMode)))
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

        public async ValueTask DisposeAsync()
        {
            if (_jsAvailable && _resizeEventTokenTask.IsCompleted)
            {
                _resizeEventToken = await _resizeEventTokenTask;
                await JSRuntime!.InvokeVoidAsync("FluentUIBaseComponent.deregisterResizeEvent", _resizeEventToken);
            }
            GC.SuppressFinalize(this);
        }
    }
}
