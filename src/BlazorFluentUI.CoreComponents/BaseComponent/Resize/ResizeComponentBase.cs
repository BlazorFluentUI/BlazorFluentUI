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
        public Func<bool>? OnGrowData { get; set; }
        public Func<bool>? OnReduceData { get; set; }
        public Func<string>? GetCacheKey { get; set; }


        protected string hiddenParentStyles = "position:relative;";
        protected string hiddenDivStyles = "position:fixed;visibility:hidden;";

        protected bool _dataNeedsMeasuring = true;
        protected bool _isInitialMeasure = true;

        protected ElementReference initialHiddenDiv;
        protected ElementReference updateHiddenDiv;

        private bool _hasRenderedContent = false;
        private Dictionary<string, double> _measurementCache = new();

        //STATE
        private string? _resizeEventGuid;
        private DotNetObjectReference<ResizeComponentBase>? selfReference;
        private Task<Rectangle>? boundsTask;
        private CancellationTokenSource boundsCTS = new();

        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

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
            if (baseModule == null)
                baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);

            if (firstRender)
            {
                _resizeEventGuid = $"id_{Guid.NewGuid().ToString().Replace("-", "")}";
                selfReference = DotNetObjectReference.Create(this);
                await baseModule.InvokeVoidAsync("registerResizeEvent", selfReference, "OnResizedAsync", _resizeEventGuid);
            }

            double containerDimension = await GetContainerDimension();
            double elementDimension = await GetElementDimension();
            Debug.WriteLine($"ElmentDim: {elementDimension}   ContainerDim: {containerDimension}");
            if (!double.IsNaN(elementDimension) && !double.IsNaN(containerDimension))
            {

                if (elementDimension > containerDimension)
                {
                    if (OnReduceData!())
                    {
                        onceOversized = true;
                        StateHasChanged();
                    }
                }
                else
                {
                    if (onceOversized == false)
                    {
                        if (OnGrowData!())
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
            ScrollDimensions? elementBounds = await baseModule!.InvokeAsync<ScrollDimensions>("measureScrollDimensions", cancellationToken, refToMeasure);
            double elementDimension = Vertical ? elementBounds.ScrollHeight : elementBounds.ScrollWidth;
            return elementDimension;
        }

        public override async ValueTask DisposeAsync()
        {
            if (baseModule != null)
            {
                await baseModule.InvokeVoidAsync("deregisterResizeEvent", _resizeEventGuid);
                await baseModule.DisposeAsync();
            }
            selfReference?.Dispose();
        }
    }
}
