using BlazorFabric.BaseComponent;
using BlazorFabric.Layer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Callout
{
    public class CalloutBase : FabricComponentBase
    {
        internal CalloutBase() { }

        [Inject] private IComponentContext ComponentContext { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected ElementRef ElementTarget { get; set; }  // not working yet
        [Parameter] protected FabricComponentBase FabricComponentTarget { get; set; }

        [Parameter] protected DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] protected bool DirectionalHintFixed { get; set; }
        [Parameter] protected bool DoNotLayer { get; set; }
        [Parameter] protected bool IsBeakVisible { get; set; } = true;
        [Parameter] protected int GapSpace { get; set; } = 0;
        [Parameter] protected int BeakWidth { get; set; } = 16;
        [Parameter] protected int CalloutWidth { get; set; } = 0;
        [Parameter] protected int CalloutMaxWidth { get; set; } = 0;
        [Parameter] protected string BackgroundColor { get; set; } = null;
        [Parameter] protected Rectangle Bounds { get; set; }
        [Parameter] protected int MinPagePadding { get; set; } = 8;
        [Parameter] protected bool PreventDismissOnScroll { get; set; } = false;
        [Parameter] protected bool PreventDismissOnResize { get; set; } = false;
        [Parameter] protected bool PreventDismissOnLosFocus { get; set; } = false;
        [Parameter] protected bool CoverTarget { get; set; } = false;
        [Parameter] protected bool AlignTargetEdge { get; set; } = false;
        [Parameter] protected string Role { get; set; }
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected string AriaLabelledBy { get; set; }
        [Parameter] protected string AriaDescribedBy { get; set; }
        [Parameter] protected bool Hidden { get; set; } = false;

        [Parameter] protected EventCallback<bool> HiddenChanged { get; set; }

        [Parameter] protected Rectangle Position { get; set; } = new Rectangle();

        [CascadingParameter(Name ="HostedContent")] private LayerHost LayerHost { get; set; }  

        protected double contentMaxHeight = 0;
        protected bool overflowYHidden = false;

        protected bool isMeasured = false;
        protected bool isLayerHostRegistered = false;

        protected ElementRef calloutRef;

        protected override async Task OnAfterRenderAsync()
        {
            if (!isLayerHostRegistered && ComponentContext.IsConnected)
            {
                await JSRuntime.InvokeAsync<object>("BlazorFabricCallout.registerHandlers", this.FabricComponentTarget.RootElementRef, new DotNetObjectRef(this));

                isLayerHostRegistered = true;
            }
            await base.OnAfterRenderAsync();
        }

        [JSInvokable] public void ScrollHandler()
        {
            //Hidden = true;
            if (!Hidden)
            {
                HiddenChanged.InvokeAsync(true);
            }
            //StateHasChanged();
            //return Task.CompletedTask;
        }


        protected override async Task OnParametersSetAsync()
        {
            if (this.FabricComponentTarget != null && !isMeasured)
            {
                Rectangle targetBounds = null;
                //targetBounds = await this.FabricComponentTarget.GetBoundsAsync();
                Rectangle maxBounds = null;
                if (this.Bounds != null)
                    maxBounds = Bounds;
                else
                {
                    //javascript to get screen bounds
                    maxBounds = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.getWindowRect");
                }
                var targetRect = await this.FabricComponentTarget.GetBoundsAsync();

                contentMaxHeight = GetMaxHeight(targetRect, maxBounds);

                var calloutPositioning = await PositionCalloutAsync(targetRect, maxBounds);

                this.Position = calloutPositioning.ElementRectangle;

                isMeasured = true;
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }

        private double GetMaxHeight(Rectangle targetRect, Rectangle maxBounds)
        {
            var gap = GapSpace + BeakWidth + (1/*BORDER_WIDTH*/ * 2);
            return GetMaxHeightFromTargetRectangle(targetRect, this.DirectionalHint , gap, maxBounds);
        }

        private double GetMaxHeightFromTargetRectangle(Rectangle targetRect, DirectionalHint targetEdge, double gapSpace, Rectangle bounds )
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
                        

            var positionedElement = await PositionElementRelativeAsync(targetRect, maxBounds);

            return new CalloutPositionedInfo(await FinalizePositionDataAsync(positionedElement, maxBounds), null);

        }

        private async Task<ElementPosition> FinalizePositionDataAsync(ElementPositionInfo positionedElement, Rectangle bounds)
        {
            var finalizedElement = await FinalizeElementPositionAsync(positionedElement.ElementRectangle,/*hostElement,*/ positionedElement.TargetEdge, bounds, positionedElement.AlignmentEdge);
            return new ElementPosition(finalizedElement, positionedElement.TargetEdge, positionedElement.AlignmentEdge);
        }

        private async Task<Rectangle> FinalizeElementPositionAsync(Rectangle elementRectangle, /* hostElement, */ RectangleEdge targetEdge, Rectangle bounds, RectangleEdge alignmentEdge)
        {
            var hostRectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", RootElementRef);
            var elementEdge = CoverTarget ? targetEdge : (RectangleEdge)((int)targetEdge * -1);
            //elementEdgeString
            var returnEdge = FinalizeReturnEdge(elementRectangle, alignmentEdge != RectangleEdge.None ? alignmentEdge : GetFlankingEdges(targetEdge).positiveEdge, bounds);

            //HOW TO DO THE PARTIAL STUFF?  Might need to set other sides to -1
            var returnValue = new Rectangle();
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
                Math.Abs(GetRelativeEdgeDifference(elementRectangle, bounds, returnEdge)) > (Math.Abs(GetRelativeEdgeDifference(elementRectangle,bounds,(RectangleEdge)((int)returnEdge * -1)))))
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

        private async Task<ElementPositionInfo> PositionElementRelativeAsync(Rectangle targetRect, Rectangle boundingRect)
        {
            
            //previous data... not implemented
            //RTL ... not implemented
            //GetPositionData()
            PositionDirectionalHintData positionData = DirectionalDictionary[DirectionalHint];
            //PositionDirectionalHintData alignmentData = null;
            //GetAlignmentData()
            if (positionData.IsAuto)
            {
                positionData.AlignmentEdge = GetClosestEdge(positionData.TargetEdge, targetRect, boundingRect);
            }
            else
                positionData.AlignTargetEdge = AlignTargetEdge;

            //Now calculate positionedElement
            //GetRectangleFromElement()
            var calloutRectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", calloutRef);

            var positionedElement = PositionElementWithinBounds(calloutRectangle, targetRect, boundingRect, positionData, this.GapSpace);

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
            ElementPosition elementEstimate = new ElementPosition(element, positionData.TargetEdge, positionData.AlignmentEdge);
            if (!DirectionalHintFixed  && !CoverTarget)
            {
                elementEstimate = FlipToFit(element, target, bounding, positionData, gap);
            }
            var outOfBounds = GetOutOfBoundsEdges(element, bounding);
            if(AlignTargetEdge)
            {
                // The edge opposite to the alignment edge might be out of bounds. Flip alignment to see if we can get it within bounds.
                if (outOfBounds.IndexOf((RectangleEdge)((int)elementEstimate.AlignmentEdge * -1)) > -1)
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
            var elementEdge = this.CoverTarget ? positionData.TargetEdge : (RectangleEdge)((int)positionData.TargetEdge * -1);
            Rectangle estimatedElementPosition = null;
            estimatedElementPosition = this.CoverTarget
                ? AlignEdges(elementToPosition, target, positionData.TargetEdge, gap)
                : AlignOppositeEdges(elementToPosition, target, positionData.TargetEdge, gap);
            if (!AlignTargetEdge)
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

        private ElementPosition FlipToFit(Rectangle rect, Rectangle target, Rectangle bounding, PositionDirectionalHintData positionData, double gap=0)
        {
            var currentEstimate = rect;
            var currentEdge = positionData.TargetEdge;
            var currentAlignment = positionData.AlignmentEdge;
            List<RectangleEdge> directions = new List<RectangleEdge> { RectangleEdge.Left, RectangleEdge.Right, RectangleEdge.Bottom, RectangleEdge.Top };
            for (var i=0; i< 4; i++)
            {
                if (IsEdgeInBounds(currentEstimate, bounding, currentEdge))
                {
                    directions.RemoveAt(directions.IndexOf(currentEdge));
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
                    rect.right= value;
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

    }
}
