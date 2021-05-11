using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public class ResponsiveComponentBase : FluentUIComponentBase, IAsyncDisposable
    {
        //STATE
        private string? _resizeEventGuid;
        private DotNetObjectReference<ResponsiveComponentBase>? selfReference;

        [Inject] IJSRuntime? JSRuntime { get; set; }
        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        protected ResponsiveMode CurrentMode { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            if (firstRender)
            {
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
                selfReference = DotNetObjectReference.Create(this);
                await baseModule.InvokeVoidAsync("registerResizeEvent", selfReference , "OnResizedAsync", _resizeEventGuid);
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

        public override async ValueTask DisposeAsync()
        {
            try
            {
                if (baseModule != null)
                {
                    if (_resizeEventGuid != null)
                        await baseModule.InvokeVoidAsync("deregisterResizeEvent", _resizeEventGuid);

                    await baseModule.DisposeAsync();
                }
                selfReference?.Dispose();
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
