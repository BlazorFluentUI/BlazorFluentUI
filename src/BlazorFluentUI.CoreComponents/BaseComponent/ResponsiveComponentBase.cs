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
        private string _resizeEventGuid;

        [Inject] IJSRuntime? JSRuntime { get; set; }
        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private static IJSObjectReference? baseModule;

        protected ResponsiveMode CurrentMode { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            if (firstRender)
            {
                _jsAvailable = true;
                Rectangle? windowRect = await baseModule!.InvokeAsync<Rectangle>("getWindowRect");
                foreach (object? item in Enum.GetValues(typeof(ResponsiveMode)))
                {
                    if (windowRect.Width <= ResponsiveModeUtils.RESPONSIVE_MAX_CONSTRAINT[(int)item])
                    {
                        CurrentMode = (ResponsiveMode)item;
                        break;
                    }
                }
                _resizeEventGuid = Guid.NewGuid().ToString().Replace("-", "");
                _resizeEventTokenTask = baseModule!.InvokeAsync<string>("registerResizeEvent", DotNetObjectReference.Create(this), "OnResizedAsync", _resizeEventGuid);
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
            if (_jsAvailable) // && _resizeEventTokenTask.IsCompleted)
            {
                //_resizeEventToken = await _resizeEventTokenTask;
                //await baseModule!.InvokeVoidAsync("deregisterResizeEvent", _resizeEventToken);
                await baseModule!.InvokeVoidAsync("deregisterResizeEvent", _resizeEventGuid);
            }
            GC.SuppressFinalize(this);
        }
    }
}
