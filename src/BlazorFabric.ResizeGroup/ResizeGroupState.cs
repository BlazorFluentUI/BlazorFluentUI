using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class ResizeGroupState<TObject>
    {
        public TObject RenderedData { get; set; }
        public TObject DataToMeasure { get; set; }
        public ResizeDirection ResizeDirection { get; set; } = ResizeDirection.None;
        public bool MeasureContainer { get; set; }

        public double ContainerDimension { get; set; }

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
            if (nextState.ContainerDimension != 0)
                this.ContainerDimension = nextState.ContainerDimension;

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
