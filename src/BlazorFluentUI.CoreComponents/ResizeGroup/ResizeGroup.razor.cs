using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using BlazorFluentUI.Resize;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public partial class ResizeGroup<TObject> : FluentUIComponentBase, IAsyncDisposable
    {
        [Inject] IJSRuntime? JSRuntime { get; set; }

        [Parameter] public bool Vertical { get; set; }
        [Parameter] public TObject Data { get; set; }

        [Parameter] public RenderFragment<TObject> DataTemplate { get; set; }
        [Parameter] public Func<TObject, TObject> OnGrowData { get; set; }
        [Parameter] public Func<TObject, TObject> OnReduceData { get; set; }
        [Parameter] public Func<TObject, string> GetCacheKey { get; set; }

        [Parameter] public EventCallback<TObject> OnDataReduced { get; set; }
        [Parameter] public EventCallback<TObject> OnDataGrown { get; set; }

        protected string hiddenParentStyles = "position:relative;";
        protected string hiddenDivStyles = "position:fixed;visibility:hidden;";

        protected bool _dataNeedsMeasuring = true;
        protected bool _isInitialMeasure = true;

        protected ElementReference initialHiddenDiv;
        protected ElementReference updateHiddenDiv;

        private double _containerDimension = double.NaN;
        private bool _hasRenderedContent = false;
        private Dictionary<string, double> _measurementCache = new();

        //STATE
        private TObject _renderedData;
        private TObject _dataToMeasure;
        private bool _measureContainer;
        private ResizeDirection _resizeDirection = ResizeDirection.None;
        private bool _jsAvailable;
        private string? _resizeEventToken;

        private ValueTask<string> _resizeEventTokenTask;  // WARNING - can only await this ONCE

        private Task<Rectangle> boundsTask;
        private CancellationTokenSource boundsCTS = new();

        private Task<ResizeGroupState<TObject>> nextStateTask;
        private CancellationTokenSource nextStateCTS = new();

        protected override Task OnInitializedAsync()
        {
            //Debug.WriteLine("Initialized");
            ResizeGroupState<TObject>? state = GetInitialState();
            // Debug.WriteLine($"State dataToMeasure: {(state.DataToMeasure== null ? "null" : "not empty")}");
            _dataToMeasure = state.DataToMeasure;
            _measureContainer = state.MeasureContainer;
            _renderedData = state.RenderedData;
            //   Debug.WriteLine($"State renderedData: {(state.RenderedData==null ? "null" : "not empty")}");
            _resizeDirection = state.ResizeDirection;

            return Task.CompletedTask;
        }

        protected override bool ShouldRender()
        {
            if (_dataToMeasure == null || _measurementCache.ContainsKey(GetCacheKey(_dataToMeasure)))
            {
                _dataNeedsMeasuring = false;
            }
            else
            {
                _dataNeedsMeasuring = true;
            }

            _isInitialMeasure = !_hasRenderedContent && _dataNeedsMeasuring;

            return base.ShouldRender();
        }

        //[JSInvokable] public void OnResizedAsync()
        //{
        //    _measureContainer = true;
        //    StateHasChanged();
        //}



        [JSInvokable]
        public void OnResizedAsync()
        {
            _measureContainer = true;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsAvailable = true;
                _resizeEventTokenTask = JSRuntime!.InvokeAsync<string>("FluentUIBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "OnResizedAsync");
            }

            if (_renderedData != null)
                _hasRenderedContent = true;

            try
            {
                double newContainerDimension = double.NaN;
                //Debug.WriteLine($"IsMeasureContainer: {_measureContainer}");
                if (_measureContainer)
                {
                    //if (boundsTask != null && !boundsTask.IsCompleted)
                    //{
                    //    boundsCTS.Cancel();
                    //    boundsCTS.Dispose();
                    //    boundsCTS = new CancellationTokenSource();
                    //}
                    boundsTask = GetBoundsAsync(boundsCTS.Token);
                    Rectangle? bounds = await boundsTask;
                    newContainerDimension = (Vertical ? bounds.Height : bounds.Width);
                }
                //Debug.WriteLine($"Container dimension: {_containerDimension}");


                //if (nextStateTask != null && !nextStateTask.IsCompleted)
                //{
                //    nextStateCTS.Cancel();
                //    nextStateCTS.Dispose();
                //    nextStateCTS = new CancellationTokenSource();
                //}
                nextStateTask = GetNextStateAsync(newContainerDimension, _containerDimension, _dataToMeasure, _renderedData, _resizeDirection, nextStateCTS.Token);
                ResizeGroupState<TObject>? nextState = await nextStateTask;

                if (nextState != null)
                {
                    _dataToMeasure = nextState.DataToMeasure;
                    _measureContainer = nextState.MeasureContainer;
                    _renderedData = nextState.RenderedData;
                    _resizeDirection = nextState.ResizeDirection;
                    _containerDimension = nextState.ContainerDimension;

                    StateHasChanged();
                }
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine("Task was cancelled in ResizeGroup");
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

        private ResizeGroupState<TObject> GetInitialState()
        {
            return new ResizeGroupState<TObject>() { ResizeDirection = ResizeDirection.Grow, DataToMeasure = Data, MeasureContainer = true };
        }

        private async Task<ResizeGroupState<TObject>> GetNextStateAsync(double newContainerDimension, double oldContainerDimension, TObject dataToMeasure, TObject renderedData, ResizeDirection resizeDirection, CancellationToken cancellationToken)
        {
            double replacementContainerDimension = oldContainerDimension;
            if (double.IsNaN(newContainerDimension) && dataToMeasure == null)
                return null;

            if (!double.IsNaN(newContainerDimension))
            {
                // If we know what the last container size was and we rendered data at that width/height, we can do an optimized render
                if (!double.IsNaN(oldContainerDimension) && renderedData != null && dataToMeasure == null)
                {
                    ResizeGroupState<TObject>? state = new(renderedData, resizeDirection, dataToMeasure);
                    ResizeGroupState<TObject>? alteredState = UpdateContainerDimension(oldContainerDimension, newContainerDimension, Data, renderedData);
                    state.ReplaceProperties(alteredState);
                    return state;
                }

                replacementContainerDimension = newContainerDimension;
            }

            ResizeGroupState<TObject>? nextState = new(renderedData, resizeDirection, dataToMeasure);
            nextState.MeasureContainer = false;
            if (replacementContainerDimension != oldContainerDimension)
                nextState.ContainerDimension = replacementContainerDimension;

            if (dataToMeasure != null)
            {
                //get elementDimension here
                double elementDimension = await GetElementDimensionsAsync(cancellationToken);

                if (resizeDirection == ResizeDirection.Grow && OnGrowData != null)
                {
                    ResizeGroupState<TObject>? alteredState = GrowDataUntilItDoesNotFit(dataToMeasure, elementDimension, replacementContainerDimension);
                    nextState.ReplaceProperties(alteredState);
                }
                else
                {
                    ResizeGroupState<TObject>? alteredState = ShrinkContentsUntilTheyFit(dataToMeasure, elementDimension, replacementContainerDimension);
                    nextState.ReplaceProperties(alteredState);
                }
            }


            return nextState;
        }

        private ResizeGroupState<TObject> UpdateContainerDimension(double oldDimension, double newDimension, TObject fullDimensionData, TObject renderedData)
        {
            ResizeGroupState<TObject> nextState;
            if (double.IsNaN(oldDimension))
                throw new Exception("The containerDimension was not supposed to be NaN");
            if (newDimension > oldDimension)
            {
                if (OnGrowData != null)
                {
                    nextState = new ResizeGroupState<TObject>(ResizeDirection.Grow, OnGrowData(renderedData));
                }
                else
                {
                    nextState = new ResizeGroupState<TObject>(ResizeDirection.Shrink, fullDimensionData);
                }
            }
            else
            {
                nextState = new ResizeGroupState<TObject>(ResizeDirection.Shrink, renderedData);
            }

            nextState.ContainerDimension = newDimension;
            nextState.MeasureContainer = false;
            return nextState;
        }

        private ResizeGroupState<TObject> GrowDataUntilItDoesNotFit(TObject dataToMeasure, double elementDimension, double containerDimension)
        {
            while (elementDimension < containerDimension)
            {
                Debug.WriteLine("Loop in GrowUntilNotFit");
                TObject? nextMeasuredData = OnGrowData(dataToMeasure);
                if (nextMeasuredData == null)
                {
                    Debug.WriteLine($"GrowUntilNotFit:  got null nextMeasuredData");
                    return new ResizeGroupState<TObject>() { RenderedData = dataToMeasure, NullifyDataToMeasure = true, ForceNoneResizeDirection = true, ContainerDimension = containerDimension };
                }

                bool found = _measurementCache.TryGetValue(GetCacheKey(nextMeasuredData), out elementDimension);
                if (!found)
                {
                    return new ResizeGroupState<TObject>() { DataToMeasure = nextMeasuredData, ContainerDimension = containerDimension };
                }

                dataToMeasure = nextMeasuredData;
            }
            Debug.WriteLine($"GrowUntilNotFit:  element: {elementDimension}   container: {containerDimension}");

            ResizeGroupState<TObject>? altState = ShrinkContentsUntilTheyFit(dataToMeasure, elementDimension, containerDimension);
            if (altState.ResizeDirection == ResizeDirection.None)
                altState.ResizeDirection = ResizeDirection.Shrink;

            return altState;
        }

        private ResizeGroupState<TObject> ShrinkContentsUntilTheyFit(TObject dataToMeasure, double elementDimension, double containerDimension)
        {
            while (elementDimension > containerDimension)
            {
                Debug.WriteLine("Loop in ShrinkUntilTheyFit");
                TObject? nextMeasuredData = OnReduceData(dataToMeasure);
                if (nextMeasuredData == null)
                {
                    Debug.WriteLine("ShrinkUntilTheyFit:  nextMeasuredData was null");
                    return new ResizeGroupState<TObject>() { RenderedData = dataToMeasure, ForceNoneResizeDirection = true, NullifyDataToMeasure = true, ContainerDimension = containerDimension };

                }


                bool found = _measurementCache.TryGetValue(GetCacheKey(nextMeasuredData), out elementDimension);
                if (!found)
                {
                    return new ResizeGroupState<TObject>() { DataToMeasure = nextMeasuredData, ResizeDirection = ResizeDirection.Shrink, ContainerDimension = containerDimension };
                }

                dataToMeasure = nextMeasuredData;
            }
            Debug.WriteLine($"ShrinkUntilTheyFit:  element: {elementDimension}   container: {containerDimension}");

            return new ResizeGroupState<TObject>() { RenderedData = dataToMeasure, ForceNoneResizeDirection = true, NullifyDataToMeasure = true, ContainerDimension = containerDimension };

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

