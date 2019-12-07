using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class ResizeGroup<TObject> : FabricComponentBase
    {
        [Inject] IJSRuntime jSRuntime { get; set; }

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
        private Dictionary<string, double> _measurementCache = new Dictionary<string, double>();

        //STATE
        protected TObject _renderedData;
        protected TObject _dataToMeasure;
        protected bool _measureContainer;
        protected ResizeDirection _resizeDirection = ResizeDirection.None;
        private string _resizeEventToken;

        protected override Task OnInitializedAsync()
        {
            //Debug.WriteLine("Initialized");
            var state = GetInitialState();
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

        //[JSInvokable] public void ResizeHappenedAsync()
        //{
        //    _measureContainer = true;
        //    StateHasChanged();
        //}

        void SetMeasureContainer(bool shouldMeasureContainer)
        {
            _measureContainer = shouldMeasureContainer;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _resizeEventToken = await jSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerResizeEvent", DotNetObjectReference.Create(new ResizeGroupInternal.InteropHelper(SetMeasureContainer)), "ResizeHappenedAsync");
            }

            if (_renderedData != null)
                _hasRenderedContent = true;
            
            double containerDimension = double.NaN;
            //Debug.WriteLine($"IsMeasureContainer: {_measureContainer}");
            if (_measureContainer)
            {
                var bounds = await this.GetBoundsAsync();
                containerDimension = (this.Vertical ? bounds.height : bounds.width);
            }
            //Debug.WriteLine($"Container dimension: {_containerDimension}");


            
            var state = await GetNextStateAsync(containerDimension);

            if (state != null)
            {
                _dataToMeasure = state.DataToMeasure;
                _measureContainer = state.MeasureContainer;
                _renderedData = state.RenderedData;
                _resizeDirection = state.ResizeDirection;
               // Debug.WriteLine($"State changed: {_resizeDirection}");

                StateHasChanged();
            }


            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task<double> GetElementDimensionsAsync()
        {
            // must get this via a funcion because we don't know yet if either of these elements will exist to be measured.
            var refToMeasure = !_hasRenderedContent ? this.initialHiddenDiv : this.updateHiddenDiv;
            var elementBounds = await jSRuntime.InvokeAsync<ScrollDimensions>("BlazorFabricBaseComponent.measureScrollDimensions", refToMeasure);
            var elementDimension = Vertical ? elementBounds.ScrollHeight : elementBounds.ScrollWidth;
            return elementDimension;
        }

        private ResizeGroupState<TObject> GetInitialState()
        {
            return new ResizeGroupState<TObject>() { ResizeDirection = ResizeDirection.Grow, DataToMeasure = this.Data, MeasureContainer = true };
        }

        private async Task<ResizeGroupState<TObject>> GetNextStateAsync(double containerDimension)
        {
            if (double.IsNaN(containerDimension) && _dataToMeasure == null)
                return null;

            if (!double.IsNaN(containerDimension))
            {
                // If we know what the last container size was and we rendered data at that width/height, we can do an optimized render
                if (!double.IsNaN(_containerDimension) && _renderedData != null && _dataToMeasure == null)
                {
                    var state = new ResizeGroupState<TObject>(_renderedData, _resizeDirection, _dataToMeasure);
                    var alteredState = UpdateContainerDimension(containerDimension, this.Data, _renderedData);
                    state.ReplaceProperties(alteredState);
                    return state;
                }

                _containerDimension = containerDimension;
            }

            var nextState = new ResizeGroupState<TObject>(_renderedData, _resizeDirection, _dataToMeasure);
            nextState.MeasureContainer = false;

            if (_dataToMeasure != null)
            {
                //get elementDimension here
                var elementDimension = await GetElementDimensionsAsync();

                if (_resizeDirection == ResizeDirection.Grow && this.OnGrowData != null)
                {
                    var alteredState = GrowDataUntilItDoesNotFit(_dataToMeasure, elementDimension);
                    nextState.ReplaceProperties(alteredState);
                }
                else
                {
                    var alteredState = ShrinkContentsUntilTheyFit(_dataToMeasure, elementDimension);
                    nextState.ReplaceProperties(alteredState);
                }
            }


            return nextState;
        }

        private ResizeGroupState<TObject> UpdateContainerDimension(double newDimension, TObject fullDimensionData, TObject renderedData)
        {
            ResizeGroupState<TObject> nextState; 
            if (double.IsNaN(_containerDimension))
                throw new Exception("The containerDimension was not supposed to be NaN");
            if (newDimension > _containerDimension)
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

            _containerDimension = newDimension;
            nextState.MeasureContainer = false;
            return nextState;
        }

        private ResizeGroupState<TObject> GrowDataUntilItDoesNotFit(TObject dataToMeasure, double elementDimension)
        {
            while (elementDimension < _containerDimension)
            {
                Debug.WriteLine("Loop in GrowUntilNotFit");
                var nextMeasuredData = this.OnGrowData(dataToMeasure);
                if (nextMeasuredData == null)
                {
                    Debug.WriteLine($"GrowUntilNotFit:  got null nextMeasuredData");
                    return new ResizeGroupState<TObject>() { RenderedData = dataToMeasure, NullifyDataToMeasure = true, ForceNoneResizeDirection = true };
                }

                var found = _measurementCache.TryGetValue(this.GetCacheKey(nextMeasuredData), out elementDimension);
                if (!found)
                {
                    return new ResizeGroupState<TObject>() { DataToMeasure = nextMeasuredData };
                }

                dataToMeasure = nextMeasuredData;
            }
            Debug.WriteLine($"GrowUntilNotFit:  element: {elementDimension}   container: {_containerDimension}");

            var altState = ShrinkContentsUntilTheyFit(dataToMeasure, elementDimension);
            if (altState.ResizeDirection == ResizeDirection.None)
                altState.ResizeDirection = ResizeDirection.Shrink;

            return altState;
        }

        private ResizeGroupState<TObject> ShrinkContentsUntilTheyFit(TObject dataToMeasure, double elementDimension)
        {
            while (elementDimension > _containerDimension)
            {
                Debug.WriteLine("Loop in ShrinkUntilTheyFit");
                var nextMeasuredData = this.OnReduceData(dataToMeasure);
                if (nextMeasuredData == null)
                {
                    Debug.WriteLine("ShrinkUntilTheyFit:  nextMeasuredData was null");
                    return new ResizeGroupState<TObject>() { RenderedData = dataToMeasure, ForceNoneResizeDirection=true, NullifyDataToMeasure=true };
                }


                var found = _measurementCache.TryGetValue(this.GetCacheKey(nextMeasuredData), out elementDimension);
                if (!found)
                {
                    return new ResizeGroupState<TObject>() { DataToMeasure = nextMeasuredData, ResizeDirection = ResizeDirection.Shrink };
                }

                dataToMeasure = nextMeasuredData;
            }
            Debug.WriteLine($"ShrinkUntilTheyFit:  element: {elementDimension}   container: {_containerDimension}");

            return new ResizeGroupState<TObject>() { RenderedData = dataToMeasure, ForceNoneResizeDirection = true, NullifyDataToMeasure = true };

        }


    }


    public class ResizeGroupState<TObject> 
    {
        public TObject RenderedData { get; set; }
        public TObject DataToMeasure { get; set; }
        public ResizeDirection ResizeDirection { get; set; } = ResizeDirection.None;
        public bool MeasureContainer { get; set; }

        public bool NullifyRenderedData { get; set; }
        public bool NullifyDataToMeasure { get; set; }
        public bool ForceNoneResizeDirection { get; set; }

        public ResizeGroupState()
        {
        }

        public ResizeGroupState(ResizeDirection resizeDirection, TObject dataToMeasure)
        {
            this.RenderedData = default;
            this.ResizeDirection = resizeDirection;
            this.DataToMeasure = dataToMeasure;
        }

        public ResizeGroupState(TObject renderedData, ResizeDirection resizeDirection, TObject dataToMeasure)
        {
            this.RenderedData = renderedData;
            this.ResizeDirection = resizeDirection;
            this.DataToMeasure = dataToMeasure;
        }

        public void ReplaceProperties(ResizeGroupState<TObject> nextState)
        {
            if (nextState.RenderedData != null)
                this.RenderedData = nextState.RenderedData;
            if (nextState.DataToMeasure != null)
                this.DataToMeasure = nextState.DataToMeasure;
            if (nextState.ResizeDirection != ResizeDirection.None)
                this.ResizeDirection = nextState.ResizeDirection;

            this.MeasureContainer = nextState.MeasureContainer;

            if (nextState.NullifyDataToMeasure)
                this.DataToMeasure = default;
            if (nextState.NullifyRenderedData)
                this.RenderedData = default;
            if (nextState.ForceNoneResizeDirection)
                this.ResizeDirection = ResizeDirection.None;
        }
    }
}

