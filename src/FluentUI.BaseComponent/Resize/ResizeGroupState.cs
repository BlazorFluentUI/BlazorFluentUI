namespace FluentUI.Resize
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
            RenderedData = default;
            ResizeDirection = resizeDirection;
            DataToMeasure = dataToMeasure;
        }

        public ResizeGroupState(TObject renderedData, ResizeDirection resizeDirection, TObject dataToMeasure)
        {
            RenderedData = renderedData;
            ResizeDirection = resizeDirection;
            DataToMeasure = dataToMeasure;
        }

        public void ReplaceProperties(ResizeGroupState<TObject> nextState)
        {
            if (nextState.RenderedData != null)
                RenderedData = nextState.RenderedData;
            if (nextState.DataToMeasure != null)
                DataToMeasure = nextState.DataToMeasure;
            if (nextState.ResizeDirection != ResizeDirection.None)
                ResizeDirection = nextState.ResizeDirection;
            if (nextState.ContainerDimension != 0)
                ContainerDimension = nextState.ContainerDimension;

            MeasureContainer = nextState.MeasureContainer;

            if (nextState.NullifyDataToMeasure)
                DataToMeasure = default;
            if (nextState.NullifyRenderedData)
                RenderedData = default;
            if (nextState.ForceNoneResizeDirection)
                ResizeDirection = ResizeDirection.None;
        }
    }
}
