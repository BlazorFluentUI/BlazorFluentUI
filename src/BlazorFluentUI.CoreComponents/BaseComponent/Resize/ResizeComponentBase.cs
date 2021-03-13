using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI.Resize
{
    public class ResizeComponentBase : FluentUIComponentBase, IAsyncDisposable
    {
        [Inject] IJSRuntime? JSRuntime { get; set; }
        [Parameter] public bool Vertical { get; set; }
        public Func<bool> OnGrowData { get; set; }
        public Func<bool> OnReduceData { get; set; }
        public Func<string> GetCacheKey { get; set; }


        protected string hiddenParentStyles = "position:relative;";
        protected string hiddenDivStyles = "position:fixed;visibility:hidden;";

        protected bool _dataNeedsMeasuring = true;
        protected bool _isInitialMeasure = true;

        protected ElementReference initialHiddenDiv;
        protected ElementReference updateHiddenDiv;

        private bool _hasRenderedContent = false;
        private Dictionary<string, double> _measurementCache = new();

        //STATE
        private bool _jsAvailable;
        private string? _resizeEventToken;

        private ValueTask<string> _resizeEventTokenTask;  // WARNING - can only await this ONCE

        private Task<Rectangle> boundsTask;
        private CancellationTokenSource boundsCTS = new();

        protected override Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        protected override bool ShouldRender()
        {
            _dataNeedsMeasuring = true;
            _isInitialMeasure = !_hasRenderedContent && _dataNeedsMeasuring;

            return base.ShouldRender();
        }

        [JSInvokable]
        public void OnResizedAsync()
        {
            onceOversized = false;
            StateHasChanged();
        }

        async Task<double> GetVisualElementDimension(ElementReference elementReference)
        {
            double newContainerDimension = double.NaN;
            if (RootElementReference.Id != null)
            {
                boundsTask = GetBoundsAsync(elementReference, boundsCTS.Token);
                Rectangle? bounds = await boundsTask;
                newContainerDimension = (Vertical ? bounds.Height : bounds.Width);
            }
            return newContainerDimension;
        }


        async Task<double> GetContainerDimension()
        {
            return await GetVisualElementDimension(RootElementReference);
        }

        async Task<double> GetElementDimension()
        {
            return await GetVisualElementDimension(updateHiddenDiv);
        }

        bool onceOversized;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsAvailable = true;
                _resizeEventTokenTask = JSRuntime!.InvokeAsync<string>("FluentUIBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "OnResizedAsync");
            }

            double containerDimension = await GetContainerDimension();
            double elementDimension = await GetElementDimension();
            Debug.WriteLine($"ElmentDim: {elementDimension}   ContainerDim: {containerDimension}");
            if (!double.IsNaN(elementDimension) && !double.IsNaN(containerDimension))
            {

                if (elementDimension > containerDimension)
                {
                    if (OnReduceData())
                    {
                        onceOversized = true;
                        StateHasChanged();
                    }
                }
                else
                {
                    if (onceOversized == false)
                    {
                        if (OnGrowData())
                        {
                            StateHasChanged();
                        }
                    }
                    onceOversized = false;
                }

            }
            await base.OnAfterRenderAsync(firstRender);
        }


        private async Task<double> GetElementDimensionsAsync(CancellationToken cancellationToken)
        {
            // must get this via a funcion because we don't know yet if either of these elements will exist to be measured.
            ElementReference refToMeasure = !_hasRenderedContent ? initialHiddenDiv : updateHiddenDiv;
            ScrollDimensions? elementBounds = await JSRuntime.InvokeAsync<ScrollDimensions>("FluentUIBaseComponent.measureScrollDimensions", cancellationToken, refToMeasure);
            double elementDimension = Vertical ? elementBounds.ScrollHeight : elementBounds.ScrollWidth;
            return elementDimension;
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
