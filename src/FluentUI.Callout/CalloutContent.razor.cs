using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class CalloutContent : FluentUIComponentBase, IAsyncDisposable
    {

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public ElementReference ElementTarget { get; set; }  // not working yet
        [Parameter] public FluentUIComponentBase FabricComponentTarget { get; set; }

        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public bool DoNotLayer { get; set; }
        [Parameter] public bool IsBeakVisible { get; set; } = true;
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public int CalloutWidth { get; set; } = 0;
        [Parameter] public int CalloutMaxHeight { get; set; } = 0;
        [Parameter] public int CalloutMaxWidth { get; set; } = 0;
        [Parameter] public string BackgroundColor { get; set; } = null;
        [Parameter] public Rectangle Bounds { get; set; }
        [Parameter] public int MinPagePadding { get; set; } = 8;
        [Parameter] public bool PreventDismissOnScroll { get; set; } = false;
        [Parameter] public bool PreventDismissOnResize { get; set; } = false;
        [Parameter] public bool PreventDismissOnLostFocus { get; set; } = false;
        [Parameter] public bool CoverTarget { get; set; } = false;
        [Parameter] public bool AlignTargetEdge { get; set; } = false;
        [Parameter] public string Role { get; set; }
        [Parameter] public bool HideOverflow { get; set; } = false;

        [Parameter] public bool SetInitialFocus { get; set; }

        // This is no longer available to use publicly.  Need this to hide control when calculating position otherwise it blinks into the top corner.
        protected bool Hidden { get; set; } = true;

        [Parameter] public EventCallback<bool> HiddenChanged { get; set; }

        [Parameter] public EventCallback OnDismiss { get; set; }

        [Parameter] public EventCallback<CalloutPositionedInfo> OnPositioned { get; set; }

        protected Rectangle Position { get; set; } = new Rectangle();

        protected CalloutPositionedInfo CalloutPosition { get; set; } = new CalloutPositionedInfo();

        [CascadingParameter(Name = "PortalId")] private string PortalId { get; set; }

        [Inject] private IJSRuntime jSRuntime { get; set; }

        protected double contentMaxHeight = -1;
        protected bool overflowYHidden = false;

        protected bool isRenderedOnce = false;
        protected bool isMeasured = false;
        protected bool isEventHandlersRegistered = false;

        //protected Layer layerReference;

        protected ElementReference calloutReference;

        private List<int> eventHandlerIds;

        #region Style
        private ICollection<IRule> CalloutLocalRules { get; set; } = new List<IRule>();

        private Rule CalloutRule = new Rule();
        private Rule CalloutMainRule = new Rule();
        private Rule CalloutBeakRule = new Rule();
        #endregion
        protected override Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating Callout");
            CreateLocalCss();
            SetStyle();
            return base.OnInitializedAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (!isEventHandlersRegistered) 
            {
                isEventHandlersRegistered = true;

                eventHandlerIds = await JSRuntime.InvokeAsync<List<int>>("FluentUICallout.registerHandlers", RootElementReference, DotNetObjectReference.Create(this));

                

                if (!isMeasured && FabricComponentTarget != null && firstRender)
                {
                    await CalculateCalloutPositionAsync();
                }

            }

            if (!firstRender && isMeasured && !_finalPositionAnnounced)
            {
                _finalPositionAnnounced = true;
                // May have to limit this... 
                await OnPositioned.InvokeAsync(CalloutPosition);
            }

            //isRenderedOnce = true;

            //FocusFirstElement();

            await base.OnAfterRenderAsync(firstRender);
        }

        //private async void FocusFirstElement()
        //{
        //    //await jSRuntime.InvokeVoidAsync("FluentUIBaseComponent.focusFirstElementChild", RootElementReference);
        //}

        [JSInvokable]
        public async void ScrollHandler()
        {
            await OnDismiss.InvokeAsync(null);
            //await HiddenChanged.InvokeAsync(true);
        }

        [JSInvokable]
        public async void ResizeHandler()
        {
            await OnDismiss.InvokeAsync(null);
            //await HiddenChanged.InvokeAsync(true);
        }

        //[JSInvokable]
        //public async void FocusHandler()
        //{
        //    //Need way to tie focus handler between all the callouts (linked contextualmenus)  ... only dimiss when ALL of them lost focus.
        //    System.Diagnostics.Debug.WriteLine($"Callout {PortalId} called dismiss from FocusHandler from {this.DirectionalHint}");

        //    //await OnDismiss.InvokeAsync(null);
        //}

        [JSInvokable]
        public async void ClickHandler()
        {
            await OnDismiss.InvokeAsync(null);
            //await HiddenChanged.InvokeAsync(true);
        }

        protected string GetAnimationStyle()
        {
            switch (CalloutPosition.TargetEdge)
            {
                case RectangleEdge.Bottom:
                    return "slideDownIn10";
                case RectangleEdge.Left:
                    return "slideLeftIn10";
                case RectangleEdge.Right:
                    return "slideRightIn10";
                case RectangleEdge.Top:
                    return "slideUpIn10";
                default:
                    return "";
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (FabricComponentTarget != null && !isMeasured && isRenderedOnce)
            {
                _finalPositionAnnounced = false;
                //this will never get called initially because the target won't be rendered yet.  Shouldn't be called after due to isMeasured  
                await CalculateCalloutPositionAsync();

            }
            await base.OnParametersSetAsync();
        }




        //public void ShouldRerender()
        //{
        //    //
        //    //layerRef.Rerender();
        //    StateHasChanged();
        //}

        private async Task CalculateCalloutPositionAsync()
        {
            Rectangle maxBounds = null;
            if (Bounds != null)
                maxBounds = Bounds;
            else
            {
                //javascript to get screen bounds
                maxBounds = await JSRuntime.InvokeAsync<Rectangle>("FluentUIBaseComponent.getWindowRect");
                maxBounds.top += MinPagePadding;
                maxBounds.left += MinPagePadding;
                maxBounds.bottom -= MinPagePadding;
                maxBounds.right -= MinPagePadding;
                maxBounds.width -= (2 * MinPagePadding);
                maxBounds.height -= (2 * MinPagePadding);
            }
            var targetRect = await FabricComponentTarget.GetBoundsAsync();
            //Debug.WriteLine($"TargetRect: {targetRect.left}, {targetRect.top}, {targetRect.right}, {targetRect.bottom}");

            contentMaxHeight = GetMaxHeight(targetRect, maxBounds);
            if (CalloutMaxHeight > 0 && CalloutMaxHeight < contentMaxHeight)
            {
                contentMaxHeight = CalloutMaxHeight;
            }
            //StateHasChanged();

            CalloutPosition = await PositionCalloutAsync(targetRect, maxBounds);
            //this.CalloutPosition = calloutPositioning;
            //Debug.WriteLine($"CalloutPosition: {CalloutPosition.ElementRectangle.left}, {CalloutPosition.ElementRectangle.top}, {CalloutPosition.ElementRectangle.right}, {CalloutPosition.ElementRectangle.bottom}");

            //this.Position = this.CalloutPosition.ElementRectangle;


            isMeasured = true;
            Hidden = false;
            StateHasChanged();
        }

        private double GetMaxHeight(Rectangle targetRect, Rectangle maxBounds)
        {
            if (DirectionalHintFixed)
            {
                var gap = GapSpace + BeakWidth + (1/*BORDER_WIDTH*/ * 2);
                return GetMaxHeightFromTargetRectangle(targetRect, DirectionalHint, gap, maxBounds);
            }
            else
            {
                return maxBounds.height;
            }
        }

        private double GetMaxHeightFromTargetRectangle(Rectangle targetRect, DirectionalHint targetEdge, double gapSpace, Rectangle bounds)
        {
            double maxHeight = 0;
            var directionalHint = DirectionalDictionary[targetEdge];

            var target = CoverTarget ? (RectangleEdge)((int)directionalHint.TargetEdge * -1) : directionalHint.TargetEdge;

            if (target == RectangleEdge.Top)
            {
                maxHeight = GetEdgeValue(targetRect, directionalHint.TargetEdge) - bounds.top - gapSpace;
            }
            else if (target == RectangleEdge.Bottom)
            {
                maxHeight = bounds.bottom - GetEdgeValue(targetRect, directionalHint.TargetEdge) - gapSpace;
            }
            else
            {
                maxHeight = bounds.bottom - targetRect.top - gapSpace;
            }
            return maxHeight > 0 ? maxHeight : bounds.height;
        }

        private async Task<CalloutPositionedInfo> PositionCalloutAsync(Rectangle targetRect, Rectangle maxBounds)
        {
            var beakWidth = IsBeakVisible ? BeakWidth : 0;
            var gap = Math.Sqrt(beakWidth * beakWidth * 2) / 2 + GapSpace;

            //Debug.WriteLine($"MaxBounds: {maxBounds.left}, {maxBounds.top}, {maxBounds.right}, {maxBounds.bottom}");
            var positionedElement = await PositionElementRelativeAsync(gap, targetRect, maxBounds);

            var beakPositioned = PositionBeak(beakWidth, positionedElement);

            var finalizedBeakPosition = FinalizeBeakPosition(positionedElement, beakPositioned, maxBounds);

            var finalizedElemenentPosition = await FinalizePositionDataAsync(positionedElement, maxBounds);

            return new CalloutPositionedInfo(finalizedElemenentPosition.element, finalizedElemenentPosition.targetEdge, finalizedElemenentPosition.alignmentEdge, finalizedBeakPosition);

        }

        private CalloutBeakPositionedInfo FinalizeBeakPosition(ElementPosition elementPosition, Rectangle positionedBeak, Rectangle bounds)
        {
            var targetEdge = (RectangleEdge)((int)elementPosition.TargetEdge * -1);
            var actualElement = new Rectangle(0, elementPosition.ElementRectangle.width, 0, elementPosition.ElementRectangle.height);
            PartialRectangle returnValue = new PartialRectangle();
            var returnEdge = FinalizeReturnEdge(
                elementPosition.ElementRectangle,
                elementPosition.AlignmentEdge != RectangleEdge.None ? elementPosition.AlignmentEdge : GetFlankingEdges(targetEdge).positiveEdge,
                bounds);
            switch (targetEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.bottom = GetEdgeValue(positionedBeak, targetEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.left = GetEdgeValue(positionedBeak, targetEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.right = GetEdgeValue(positionedBeak, targetEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.top = GetEdgeValue(positionedBeak, targetEdge);
                    break;
            }
            switch (returnEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.bottom = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.left = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.right = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.top = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
            }
            return new CalloutBeakPositionedInfo(
                returnValue,
                GetClosestEdge(elementPosition.TargetEdge, positionedBeak, actualElement),
                targetEdge);
        }

        private Rectangle PositionBeak(double beakWidth, ElementPositionInfo elementPosition)
        {
            var target = elementPosition.TargetRectangle;
            var edges = GetFlankingEdges(elementPosition.TargetEdge);
            var beakTargetPoint = GetCenterValue(target, elementPosition.TargetEdge);
            var elementBounds = new Rectangle(
                beakWidth / 2,
                elementPosition.ElementRectangle.width - beakWidth / 2,
                beakWidth / 2,
                elementPosition.ElementRectangle.height - beakWidth / 2
                );
            var beakPosition = new Rectangle(0, beakWidth, 0, beakWidth);
            beakPosition = MoveEdge(beakPosition, (RectangleEdge)((int)elementPosition.TargetEdge * -1), -beakWidth / 2);
            beakPosition = CenterEdgeToPoint(
                beakPosition,
                (RectangleEdge)((int)elementPosition.TargetEdge * -1),
                beakTargetPoint - GetRelativeRectEdgeValue(edges.positiveEdge, elementPosition.ElementRectangle)
                );

            if (!IsEdgeInBounds(beakPosition, elementBounds, edges.positiveEdge))
            {
                beakPosition = AlignEdges(beakPosition, elementBounds, edges.positiveEdge);
            }
            else if (!IsEdgeInBounds(beakPosition, elementBounds, edges.negativeEdge))
            {
                beakPosition = AlignEdges(beakPosition, elementBounds, edges.negativeEdge);
            }

            return beakPosition;
        }


        private async Task<(PartialRectangle element, RectangleEdge targetEdge, RectangleEdge alignmentEdge)> FinalizePositionDataAsync(ElementPositionInfo positionedElement, Rectangle bounds)
        {
            var finalizedElement = await FinalizeElementPositionAsync(positionedElement.ElementRectangle,/*hostElement,*/ positionedElement.TargetEdge, bounds, positionedElement.AlignmentEdge);
            return (finalizedElement, positionedElement.TargetEdge, positionedElement.AlignmentEdge);
        }

        private async Task<PartialRectangle> FinalizeElementPositionAsync(Rectangle elementRectangle, /* hostElement, */ RectangleEdge targetEdge, Rectangle bounds, RectangleEdge alignmentEdge)
        {
            var hostRectangle = await JSRuntime.InvokeAsync<Rectangle>("FluentUIBaseComponent.measureElementRect", RootElementReference);
            //Debug.WriteLine($"HostRect: {hostRectangle.left}, {hostRectangle.top}, {hostRectangle.right}, {hostRectangle.bottom}");


            var elementEdge = CoverTarget ? targetEdge : (RectangleEdge)((int)targetEdge * -1);
            //elementEdgeString
            var returnEdge = FinalizeReturnEdge(elementRectangle, alignmentEdge != RectangleEdge.None ? alignmentEdge : GetFlankingEdges(targetEdge).positiveEdge, bounds);

            //HOW TO DO THE PARTIAL STUFF?  Might need to set other sides to -1
            var returnValue = new PartialRectangle();
            switch (elementEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.bottom = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.left = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.right = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.top = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
            }
            switch (returnEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.bottom = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.left = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.right = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.top = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
            }
            return returnValue;
        }

        private RectangleEdge FinalizeReturnEdge(Rectangle elementRectangle, RectangleEdge returnEdge, Rectangle bounds)
        {
            if (bounds != null &&
                Math.Abs(GetRelativeEdgeDifference(elementRectangle, bounds, returnEdge)) > (Math.Abs(GetRelativeEdgeDifference(elementRectangle, bounds, (RectangleEdge)((int)returnEdge * -1)))))
            {
                return (RectangleEdge)((int)returnEdge * -1);
            }

            return returnEdge;
        }

        private double GetRelativeEdgeDifference(Rectangle rect, Rectangle hostRect, RectangleEdge edge)
        {
            var edgeDifference = GetEdgeValue(rect, edge) - GetEdgeValue(hostRect, edge);
            return GetRelativeEdgeValue(edge, edgeDifference);
        }

        private async Task<ElementPositionInfo> PositionElementRelativeAsync(double gap, Rectangle targetRect, Rectangle boundingRect)
        {

            //previous data... not implemented
            //RTL ... not implemented
            //GetPositionData()
            PositionDirectionalHintData positionData = DirectionalDictionary[DirectionalHint];
            //PositionDirectionalHintData alignmentData = null;

            // start GetAlignmentData()
            if (positionData.IsAuto)
            {
                positionData.AlignmentEdge = GetClosestEdge(positionData.TargetEdge, targetRect, boundingRect);
            }
            positionData.AlignTargetEdge = AlignTargetEdge;
            // end GetAlignmentData()

            //Now calculate positionedElement
            //GetRectangleFromElement()
            var calloutRectangle = await JSRuntime.InvokeAsync<Rectangle>("FluentUIBaseComponent.measureElementRect", calloutReference);
            //Debug.WriteLine($"Callout: {calloutRectangle.left}, {calloutRectangle.top}, {calloutRectangle.right}, {calloutRectangle.bottom}");

            var positionedElement = PositionElementWithinBounds(calloutRectangle, targetRect, boundingRect, positionData, gap);

            var elementPositionInfo = positionedElement.ToElementPositionInfo(targetRect);
            return elementPositionInfo;

        }

        private ElementPosition PositionElementWithinBounds(Rectangle elementToPosition, Rectangle target, Rectangle bounding, PositionDirectionalHintData positionData, double gap)
        {
            var estimatedElementPosition = EstimatePosition(elementToPosition, target, positionData, gap);
            if (IsRectangleWithinBounds(estimatedElementPosition, bounding))
            {
                return new ElementPosition(estimatedElementPosition, positionData.TargetEdge, positionData.AlignmentEdge);
            }
            else
            {
                return AdjustFitWithinBounds(elementToPosition, target, bounding, positionData, gap);
            }
        }

        private ElementPosition AdjustFitWithinBounds(Rectangle element, Rectangle target, Rectangle bounding, PositionDirectionalHintData positionData, double gap = 0)
        {
            var alignmentEdge = positionData.AlignmentEdge;
            var alignTargetEdge = positionData.AlignTargetEdge;
            ElementPosition elementEstimate = new ElementPosition(element, positionData.TargetEdge, alignmentEdge);
            if (!DirectionalHintFixed && !CoverTarget)
            {
                elementEstimate = FlipToFit(element, target, bounding, positionData, gap);
            }
            var outOfBounds = GetOutOfBoundsEdges(element, bounding);
            if (alignTargetEdge)
            {
                // The edge opposite to the alignment edge might be out of bounds. Flip alignment to see if we can get it within bounds.
                if (elementEstimate.AlignmentEdge != RectangleEdge.None && outOfBounds.IndexOf((RectangleEdge)((int)elementEstimate.AlignmentEdge * -1)) > -1)
                {
                    var flippedElementEstimate = FlipAlignmentEdge(elementEstimate, target, gap);
                    if (IsRectangleWithinBounds(flippedElementEstimate.ElementRectangle, bounding))
                    {
                        return flippedElementEstimate;
                    }
                }
            }
            else
            {
                foreach (var direction in outOfBounds)
                {
                    elementEstimate.ElementRectangle = AlignEdges(elementEstimate.ElementRectangle, bounding, direction);
                }
            }
            return elementEstimate;
        }


        private Rectangle EstimatePosition(Rectangle elementToPosition, Rectangle target, PositionDirectionalHintData positionData, double gap = 0)
        {
            var elementEdge = CoverTarget ? positionData.TargetEdge : (RectangleEdge)((int)positionData.TargetEdge * -1);
            Rectangle estimatedElementPosition = null;
            estimatedElementPosition = CoverTarget
                ? AlignEdges(elementToPosition, target, positionData.TargetEdge, gap)
                : AlignOppositeEdges(elementToPosition, target, positionData.TargetEdge, gap);
            if (positionData.AlignmentEdge == RectangleEdge.None)
            {
                var targetMiddlePoint = GetCenterValue(target, positionData.TargetEdge);
                estimatedElementPosition = CenterEdgeToPoint(estimatedElementPosition, elementEdge, targetMiddlePoint);
            }
            else
            {
                estimatedElementPosition = AlignEdges(estimatedElementPosition, target, positionData.AlignmentEdge);
            }
            return estimatedElementPosition;
        }

        private ElementPosition FlipToFit(Rectangle rect, Rectangle target, Rectangle bounding, PositionDirectionalHintData positionData, double gap = 0)
        {
            var currentEstimate = rect;
            var currentEdge = positionData.TargetEdge;
            var currentAlignment = positionData.AlignmentEdge;
            List<RectangleEdge> directions = new List<RectangleEdge> { RectangleEdge.Left, RectangleEdge.Right, RectangleEdge.Bottom, RectangleEdge.Top };

            //RTL not implemented

            for (var i = 0; i < 4; i++)
            {
                if (!IsEdgeInBounds(currentEstimate, bounding, currentEdge))
                {
                    directions.RemoveAt(directions.IndexOf(currentEdge));
                    if (directions.Count > 0)
                    {
                        if ((int)directions.IndexOf((RectangleEdge)((int)currentEdge * -1)) > -1)
                        {
                            currentEdge = (RectangleEdge)((int)currentEdge * -1);
                        }
                        else
                        {
                            currentAlignment = currentEdge;
                            currentEdge = directions[0];
                        }

                        currentEstimate = EstimatePosition(rect, target, new PositionDirectionalHintData(currentEdge, currentAlignment), gap);
                    }
                }
                else
                {
                    return new ElementPosition(currentEstimate, currentEdge, currentAlignment);
                }
            }
            return new ElementPosition(rect, positionData.TargetEdge, currentAlignment);
        }

        private ElementPosition FlipAlignmentEdge(ElementPosition elementEstimate, Rectangle target, double gap)
        {
            var alignmentEdge = elementEstimate.AlignmentEdge;
            var targetEdge = elementEstimate.TargetEdge;
            var elementRectangle = elementEstimate.ElementRectangle;
            var oppositeEdge = (RectangleEdge)((int)alignmentEdge * -1);
            var newEstimate = EstimatePosition(elementRectangle, target, new PositionDirectionalHintData(targetEdge, oppositeEdge), gap);
            return new ElementPosition(newEstimate, targetEdge, oppositeEdge);
        }

        private List<RectangleEdge> GetOutOfBoundsEdges(Rectangle rect, Rectangle boundingRect)
        {
            var outOfBounds = new List<RectangleEdge>();
            if (rect.top < boundingRect.top)
            {
                outOfBounds.Add(RectangleEdge.Top);
            }
            if (rect.bottom > boundingRect.bottom)
            {
                outOfBounds.Add(RectangleEdge.Bottom);
            }
            if (rect.left < boundingRect.left)
            {
                outOfBounds.Add(RectangleEdge.Left);
            }
            if (rect.right > boundingRect.right)
            {
                outOfBounds.Add(RectangleEdge.Right);
            }
            return outOfBounds;
        }

        private bool IsEdgeInBounds(Rectangle rect, Rectangle bounds, RectangleEdge edge)
        {
            var adjustedRectValue = GetRelativeRectEdgeValue(edge, rect);
            return adjustedRectValue > GetRelativeRectEdgeValue(edge, bounds);
        }

        private bool IsRectangleWithinBounds(Rectangle rect, Rectangle boundingRect)
        {
            if (rect.top < boundingRect.top)
                return false;
            if (rect.bottom > boundingRect.bottom)
                return false;
            if (rect.left < boundingRect.left)
                return false;
            if (rect.right > boundingRect.right)
                return false;

            return true;
        }

        private Rectangle CenterEdgeToPoint(Rectangle rect, RectangleEdge edge, double point)
        {
            var positiveEdge = GetFlankingEdges(edge).positiveEdge;
            var elementMiddle = GetCenterValue(rect, edge);
            var distanceToMiddle = elementMiddle - GetEdgeValue(rect, positiveEdge);
            return MoveEdge(rect, positiveEdge, point - distanceToMiddle);
        }

        private Rectangle AlignEdges(Rectangle rect, Rectangle target, RectangleEdge edge, double gap = 0)
        {
            return MoveEdge(rect, edge, GetEdgeValue(target, edge) + GetRelativeEdgeValue(edge, gap));
        }

        private Rectangle AlignOppositeEdges(Rectangle rect, Rectangle target, RectangleEdge targetEdge, double gap = 0)
        {
            var oppositeEdge = (int)targetEdge * -1;
            var adjustedGap = GetRelativeEdgeValue((RectangleEdge)oppositeEdge, gap);
            return MoveEdge(rect, (RectangleEdge)((int)targetEdge * -1), GetEdgeValue(target, targetEdge) + adjustedGap);
        }

        private Rectangle MoveEdge(Rectangle rect, RectangleEdge edge, double newValue)
        {
            var difference = GetEdgeValue(rect, edge) - newValue;
            rect = SetEdgeValue(rect, edge, newValue);
            rect = SetEdgeValue(rect, (RectangleEdge)((int)edge * -1), GetEdgeValue(rect, (RectangleEdge)((int)edge * -1)) - difference);
            return rect;
        }

        private double GetRelativeRectEdgeValue(RectangleEdge edge, Rectangle rect)
        {
            return GetRelativeEdgeValue(edge, GetEdgeValue(rect, edge));
        }

        private double GetRelativeEdgeValue(RectangleEdge edge, double value)
        {
            if (edge > 0)
            {
                return value;
            }
            else
            {
                return value * -1;
            }
        }

        private RectangleEdge GetClosestEdge(RectangleEdge targetEdge, Rectangle targetRect, Rectangle boundingRect)
        {
            var targetCenter = GetCenterValue(targetRect, targetEdge);
            var boundingCenter = GetCenterValue(boundingRect, targetEdge);
            var flankingEdges = GetFlankingEdges(targetEdge);
            if (targetCenter <= boundingCenter)
                return flankingEdges.positiveEdge;
            else
                return flankingEdges.negativeEdge;
        }

        private double GetCenterValue(Rectangle rect, RectangleEdge edge)
        {
            var edges = GetFlankingEdges(edge);
            return (GetEdgeValue(rect, edges.positiveEdge) + GetEdgeValue(rect, edges.negativeEdge)) / 2;
        }

        private double GetEdgeValue(Rectangle rect, RectangleEdge edge)
        {
            switch (edge)
            {
                case RectangleEdge.Left:
                    return rect.left;
                case RectangleEdge.Right:
                    return rect.right;
                case RectangleEdge.Top:
                    return rect.top;
                case RectangleEdge.Bottom:
                    return rect.bottom;
                default:
                    return 0;
            }
        }
        private Rectangle SetEdgeValue(Rectangle rect, RectangleEdge edge, double value)
        {
            switch (edge)
            {
                case RectangleEdge.Left:
                    rect.left = value;
                    break;
                case RectangleEdge.Right:
                    rect.right = value;
                    break;
                case RectangleEdge.Top:
                    rect.top = value;
                    break;
                case RectangleEdge.Bottom:
                    rect.bottom = value;
                    break;
            }
            return rect;
        }

        private (RectangleEdge positiveEdge, RectangleEdge negativeEdge) GetFlankingEdges(RectangleEdge edge)
        {
            if (edge == RectangleEdge.Top || edge == RectangleEdge.Bottom)
            {
                return (
                  positiveEdge: RectangleEdge.Left,
                  negativeEdge: RectangleEdge.Right
                );
            }
            else
            {
                return (
                  positiveEdge: RectangleEdge.Top,
                  negativeEdge: RectangleEdge.Bottom
                );
            }
        }

        Dictionary<DirectionalHint, PositionDirectionalHintData> DirectionalDictionary = new Dictionary<DirectionalHint, PositionDirectionalHintData>() {
            {DirectionalHint.TopLeftEdge, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.Left) },
            {DirectionalHint.TopCenter, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.None) },
            {DirectionalHint.TopRightEdge, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.Right) },
            {DirectionalHint.TopAutoEdge, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.None, true) },
            {DirectionalHint.BottomLeftEdge, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.Left) },
            {DirectionalHint.BottomCenter, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.None) },
            {DirectionalHint.BottomRightEdge, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.Right) },
            {DirectionalHint.BottomAutoEdge, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.None, true) },
            {DirectionalHint.LeftTopEdge, new PositionDirectionalHintData(RectangleEdge.Left, RectangleEdge.Top) },
            {DirectionalHint.LeftCenter, new PositionDirectionalHintData(RectangleEdge.Left, RectangleEdge.None) },
            {DirectionalHint.LeftBottomEdge, new PositionDirectionalHintData(RectangleEdge.Left, RectangleEdge.Bottom) },
            {DirectionalHint.RightTopEdge, new PositionDirectionalHintData(RectangleEdge.Right, RectangleEdge.Top) },
            {DirectionalHint.RightCenter, new PositionDirectionalHintData(RectangleEdge.Right, RectangleEdge.None) },
            {DirectionalHint.RightBottomEdge, new PositionDirectionalHintData(RectangleEdge.Right, RectangleEdge.Bottom) },
        };
        private bool _finalPositionAnnounced;

        private void CreateLocalCss()
        {
            CalloutRule.Selector = new ClassSelector() { SelectorName = "ms-Callout" };
            CalloutMainRule.Selector = new ClassSelector() { SelectorName = "ms-Callout-main" };
            CalloutBeakRule.Selector = new ClassSelector() { SelectorName = "ms-Callout-beak" };
            CalloutLocalRules.Add(CalloutRule);
            CalloutLocalRules.Add(CalloutMainRule);
            CalloutLocalRules.Add(CalloutBeakRule);
        }

        private void SetStyle()
        {
            CalloutRule.Properties = new CssString()
            {
                Css = $"position:absolute;" +
                        $"box-sizing:border-box;" +
                        $"border-radius:{Theme.Effects.RoundedCorner2};" +
                        $"box-shadow:{Theme.Effects.Elevation16};" +
                        $"{(CalloutWidth != 0 ? $"width:{CalloutWidth}px;" : "")}" +
                        $"{(CalloutMaxWidth != 0 ? $"max-width:{CalloutMaxWidth}px;" : "")}" +
                        $"outline:transparent;"
            };
            CalloutMainRule.Properties = new CssString()
            {
                Css = $"background-color:{(BackgroundColor != null ? BackgroundColor : Theme?.SemanticColors.MenuBackground)};" +
                        $"overflow-x:hidden;" +
                        $"overflow-y:{(overflowYHidden ? "hidden" : "auto")};" +
                        $"position:relative;" +
                        $"border-radius: {Theme.Effects.RoundedCorner2};"
            };
            CalloutBeakRule.Properties = new CssString()
            {
                Css = $"position:absolute;" +
                        $"background-color:{(BackgroundColor != null ? BackgroundColor : Theme?.SemanticColors.MenuBackground)};" +
                        $"box-shadow:inherit;" +
                        $"border:inherit;" +
                        $"box-sizing:border-box;" +
                        $"transform:rotate(45deg);" +
                        $"height:{BeakWidth}px;" +
                        $"width:{BeakWidth}px;"
            };
        }

        public async ValueTask DisposeAsync()
        {
            if (eventHandlerIds != null)
                await JSRuntime.InvokeAsync<object>("FluentUICallout.unregisterHandlers", eventHandlerIds);
        }
    }
}