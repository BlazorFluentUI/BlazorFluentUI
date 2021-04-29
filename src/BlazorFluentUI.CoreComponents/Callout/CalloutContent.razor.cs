using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public partial class CalloutContent : FluentUIComponentBase, IAsyncDisposable
    {

        [Inject] private IJSRuntime? JSRuntime { get; set; }
        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        private const string CalloutPath = "./_content/BlazorFluentUI.CoreComponents/callout.js";
        private IJSObjectReference? calloutModule;


        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public ElementReference ElementTarget { get; set; }  // not working yet
        [Parameter] public FluentUIComponentBase? FabricComponentTarget { get; set; }

        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public bool DoNotLayer { get; set; }
        [Parameter] public bool IsBeakVisible { get; set; } = true;
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public int CalloutWidth { get; set; } = 0;
        [Parameter] public int CalloutMaxHeight { get; set; } = 0;
        [Parameter] public int CalloutMaxWidth { get; set; } = 0;
        [Parameter] public string? BackgroundColor { get; set; }
        [Parameter] public Rectangle? Bounds { get; set; }
        [Parameter] public int MinPagePadding { get; set; } = 8;
        [Parameter] public bool PreventDismissOnScroll { get; set; } = false;
        [Parameter] public bool PreventDismissOnResize { get; set; } = false;
        [Parameter] public bool PreventDismissOnLostFocus { get; set; } = false;
        [Parameter] public bool CoverTarget { get; set; } = false;
        [Parameter] public bool AlignTargetEdge { get; set; } = false;
        [Parameter] public string? Role { get; set; }
        [Parameter] public bool HideOverflow { get; set; } = false;

        [Parameter] public bool SetInitialFocus { get; set; }

        // This is no longer available to use publicly.  Need this to hide control when calculating position otherwise it blinks into the top corner.
        protected bool Hidden { get; set; } = true;

        [Parameter] public EventCallback<bool> HiddenChanged { get; set; }

        [Parameter] public EventCallback OnDismiss { get; set; }

        [Parameter] public EventCallback<CalloutPositionedInfo> OnPositioned { get; set; }

        protected Rectangle Position { get; set; } = new Rectangle();

        protected CalloutPositionedInfo CalloutPosition { get; set; } = new CalloutPositionedInfo();

        [CascadingParameter(Name = "PortalId")] private string? PortalId { get; set; }

        protected double contentMaxHeight = -1;
        protected bool overflowYHidden = false;

        protected bool isRenderedOnce = false;
        protected bool isMeasured = false;
        protected bool isEventHandlersRegistered = false;
        private DotNetObjectReference<CalloutContent>? selfReference;

        //protected Layer layerReference;

        protected ElementReference calloutReference;

        private List<int>? eventHandlerIds;

        #region Style
        private ICollection<IRule> CalloutLocalRules { get; set; } = new List<IRule>();

        private Rule CalloutRule = new();
        private Rule CalloutMainRule = new();
        private Rule CalloutBeakRule = new();
        #endregion

        protected override Task OnInitializedAsync()
        {
            Debug.WriteLine("Creating Callout");
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
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            calloutModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", CalloutPath);

            if (!isEventHandlersRegistered)
            {
                isEventHandlersRegistered = true;
                selfReference = DotNetObjectReference.Create(this);
                try
                {
                    try
                    {
                        eventHandlerIds = await calloutModule.InvokeAsync<List<int>>("registerHandlers", cancellationTokenSource.Token, RootElementReference, selfReference);
                    }
                    catch { }
                }
                catch (Exception)
                {

                }



                if (!isMeasured && FabricComponentTarget != null && firstRender && eventHandlerIds != null )
                {
                    await CalculateCalloutPositionAsync(cancellationTokenSource.Token);
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
        public async Task ScrollHandler()
        {
            await OnDismiss.InvokeAsync(null);
            //await HiddenChanged.InvokeAsync(true);
        }

        [JSInvokable]
        public async Task ResizeHandler()
        {
            await OnDismiss.InvokeAsync(null);
            //await HiddenChanged.InvokeAsync(true);
        }

        [JSInvokable]
        public  void FocusHandler()
        {
            //Need way to tie focus handler between all the callouts (linked contextualmenus)  ... only dimiss when ALL of them lost focus.
            System.Diagnostics.Debug.WriteLine($"Callout {PortalId} called dismiss from FocusHandler from {DirectionalHint}");

            //await OnDismiss.InvokeAsync(null);
        }

        [JSInvokable]
        public async Task ClickHandler()
        {
            await OnDismiss.InvokeAsync(null);
            //await HiddenChanged.InvokeAsync(true);
        }

        protected string GetAnimationStyle()
        {
            return CalloutPosition.TargetEdge switch
            {
                RectangleEdge.Bottom => "slideDownIn10",
                RectangleEdge.Left => "slideLeftIn10",
                RectangleEdge.Right => "slideRightIn10",
                RectangleEdge.Top => "slideUpIn10",
                _ => "",
            };
        }

        protected override async Task OnParametersSetAsync()
        {
            if (FabricComponentTarget != null && !isMeasured && isRenderedOnce)
            {
                _finalPositionAnnounced = false;
                //this will never get called initially because the target won't be rendered yet.  Shouldn't be called after due to isMeasured
                await CalculateCalloutPositionAsync(cancellationTokenSource.Token);

            }
            await base.OnParametersSetAsync();
        }




        //public void ShouldRerender()
        //{
        //    //
        //    //layerRef.Rerender();
        //    StateHasChanged();
        //}

        private async Task CalculateCalloutPositionAsync(CancellationToken cancellationToken)
        {
            try
            {
                Rectangle maxBounds;
                if (Bounds != null)
                    maxBounds = Bounds;
                else
                {
                    //javascript to get screen bounds
                    maxBounds = await baseModule!.InvokeAsync<Rectangle>("getWindowRect");
                    maxBounds.Top += MinPagePadding;
                    maxBounds.Left += MinPagePadding;
                    maxBounds.Bottom -= MinPagePadding;
                    maxBounds.Right -= MinPagePadding;
                    maxBounds.Width -= (2 * MinPagePadding);
                    maxBounds.Height -= (2 * MinPagePadding);
                }

                Rectangle? targetRect = await FabricComponentTarget!.GetBoundsAsync();
                Debug.WriteLine($"TargetRect: {targetRect.Left}, {targetRect.Top}, {targetRect.Right}, {targetRect.Bottom}");

                contentMaxHeight = GetMaxHeight(targetRect, maxBounds);
                if (CalloutMaxHeight > 0 && CalloutMaxHeight < contentMaxHeight)
                {
                    contentMaxHeight = CalloutMaxHeight;
                }
                //StateHasChanged();

                CalloutPosition = await PositionCalloutAsync(targetRect, maxBounds, cancellationToken);
                //this.CalloutPosition = calloutPositioning;
                Debug.WriteLine($"CalloutPosition: {CalloutPosition.ElementRectangle.Left}, {CalloutPosition.ElementRectangle.Top}, {CalloutPosition.ElementRectangle.Right}, {CalloutPosition.ElementRectangle.Bottom}");

                //this.Position = this.CalloutPosition.ElementRectangle;


                isMeasured = true;
                Hidden = false;
                StateHasChanged();
            }
            catch (TaskCanceledException canceled)
            {
                Debug.WriteLine("Task was canceled.  Probably due to the component being disposed.");
            }
        }

        private double GetMaxHeight(Rectangle targetRect, Rectangle maxBounds)
        {
            if (DirectionalHintFixed)
            {
                int gap = GapSpace + BeakWidth + (1/*BORDER_WIDTH*/ * 2);
                return GetMaxHeightFromTargetRectangle(targetRect, DirectionalHint, gap, maxBounds);
            }
            else
            {
                return maxBounds.Height;
            }
        }

        private double GetMaxHeightFromTargetRectangle(Rectangle targetRect, DirectionalHint targetEdge, double gapSpace, Rectangle bounds)
        {
            PositionDirectionalHintData? directionalHint = DirectionalDictionary[targetEdge];

            RectangleEdge target = CoverTarget ? (RectangleEdge)((int)directionalHint.TargetEdge * -1) : directionalHint.TargetEdge;

            double maxHeight;
            if (target == RectangleEdge.Top)
            {
                maxHeight = GetEdgeValue(targetRect, directionalHint.TargetEdge) - bounds.Top - gapSpace;
            }
            else if (target == RectangleEdge.Bottom)
            {
                maxHeight = bounds.Bottom - GetEdgeValue(targetRect, directionalHint.TargetEdge) - gapSpace;
            }
            else
            {
                maxHeight = bounds.Bottom - targetRect.Top - gapSpace;
            }
            return maxHeight > 0 ? maxHeight : bounds.Height;
        }

        private async Task<CalloutPositionedInfo> PositionCalloutAsync(Rectangle targetRect, Rectangle maxBounds, CancellationToken cancellationToken)
        {
            int beakWidth = IsBeakVisible ? BeakWidth : 0;
            double gap = Math.Sqrt(beakWidth * beakWidth * 2) / 2 + GapSpace;

            //Debug.WriteLine($"MaxBounds: {maxBounds.left}, {maxBounds.top}, {maxBounds.right}, {maxBounds.bottom}");
            ElementPositionInfo? positionedElement = await PositionElementRelativeAsync(gap, targetRect, maxBounds, cancellationToken);

            Rectangle? beakPositioned = PositionBeak(beakWidth, positionedElement);

            CalloutBeakPositionedInfo? finalizedBeakPosition = FinalizeBeakPosition(positionedElement, beakPositioned, maxBounds);

            (PartialRectangle element, RectangleEdge targetEdge, RectangleEdge alignmentEdge) = await FinalizePositionDataAsync(positionedElement, maxBounds, cancellationToken);

            return new CalloutPositionedInfo(element, targetEdge, alignmentEdge, finalizedBeakPosition);

        }

        private static CalloutBeakPositionedInfo FinalizeBeakPosition(ElementPosition elementPosition, Rectangle positionedBeak, Rectangle bounds)
        {
            RectangleEdge targetEdge = (RectangleEdge)((int)elementPosition.TargetEdge * -1);
            Rectangle? actualElement = new(0, elementPosition.ElementRectangle.Width, 0, elementPosition.ElementRectangle.Height);
            PartialRectangle returnValue = new();
            RectangleEdge returnEdge = FinalizeReturnEdge(
                elementPosition.ElementRectangle,
                elementPosition.AlignmentEdge != RectangleEdge.None ? elementPosition.AlignmentEdge : GetFlankingEdges(targetEdge).positiveEdge,
                bounds);
            switch (targetEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.Bottom = GetEdgeValue(positionedBeak, targetEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.Left = GetEdgeValue(positionedBeak, targetEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.Right = GetEdgeValue(positionedBeak, targetEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.Top = GetEdgeValue(positionedBeak, targetEdge);
                    break;
            }
            switch (returnEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.Bottom = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.Left = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.Right = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.Top = GetRelativeEdgeDifference(positionedBeak, actualElement, returnEdge);
                    break;
            }
            return new CalloutBeakPositionedInfo(
                returnValue,
                GetClosestEdge(elementPosition.TargetEdge, positionedBeak, actualElement),
                targetEdge);
        }

        private static Rectangle PositionBeak(double beakWidth, ElementPositionInfo? elementPosition)
        {
            if (elementPosition == null)
                return new Rectangle(0, 0, 0, 0);
            Rectangle? target = elementPosition.TargetRectangle;
            (RectangleEdge positiveEdge, RectangleEdge negativeEdge) = GetFlankingEdges(elementPosition.TargetEdge);
            double beakTargetPoint = GetCenterValue(target, elementPosition.TargetEdge);
            Rectangle? elementBounds = new(
                beakWidth / 2,
                elementPosition.ElementRectangle.Width - beakWidth / 2,
                beakWidth / 2,
                elementPosition.ElementRectangle.Height - beakWidth / 2
                );
            Rectangle? beakPosition = new(0, beakWidth, 0, beakWidth);
            beakPosition = MoveEdge(beakPosition, (RectangleEdge)((int)elementPosition.TargetEdge * -1), -beakWidth / 2);
            beakPosition = CenterEdgeToPoint(
                beakPosition,
                (RectangleEdge)((int)elementPosition.TargetEdge * -1),
                beakTargetPoint - GetRelativeRectEdgeValue(positiveEdge, elementPosition.ElementRectangle)
                );

            if (!IsEdgeInBounds(beakPosition, elementBounds, positiveEdge))
            {
                beakPosition = AlignEdges(beakPosition, elementBounds, positiveEdge);
            }
            else if (!IsEdgeInBounds(beakPosition, elementBounds, negativeEdge))
            {
                beakPosition = AlignEdges(beakPosition, elementBounds, negativeEdge);
            }

            return beakPosition;
        }


        private async Task<(PartialRectangle element, RectangleEdge targetEdge, RectangleEdge alignmentEdge)> FinalizePositionDataAsync(ElementPositionInfo positionedElement, Rectangle bounds, CancellationToken cancellationToken)
        {
            PartialRectangle? finalizedElement = await FinalizeElementPositionAsync(positionedElement.ElementRectangle, positionedElement.TargetEdge, bounds, positionedElement.AlignmentEdge, cancellationToken);
            return (finalizedElement, positionedElement.TargetEdge, positionedElement.AlignmentEdge);
        }

        private async Task<PartialRectangle> FinalizeElementPositionAsync(Rectangle elementRectangle, RectangleEdge targetEdge, Rectangle bounds, RectangleEdge alignmentEdge, CancellationToken cancellationToken)
        {
            //HOW TO DO THE PARTIAL STUFF?  Might need to set other sides to -1
            PartialRectangle? returnValue = new();

            Rectangle? hostRectangle = await baseModule!.InvokeAsync<Rectangle>("measureElementRect", cancellationToken, RootElementReference);
            //Debug.WriteLine($"HostRect: {hostRectangle.left}, {hostRectangle.top}, {hostRectangle.right}, {hostRectangle.bottom}");


            RectangleEdge elementEdge = CoverTarget ? targetEdge : (RectangleEdge)((int)targetEdge * -1);
            //elementEdgeString
            RectangleEdge returnEdge = FinalizeReturnEdge(elementRectangle, alignmentEdge != RectangleEdge.None ? alignmentEdge : GetFlankingEdges(targetEdge).positiveEdge, bounds);

            switch (elementEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.Bottom = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.Left = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.Right = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.Top = GetRelativeEdgeDifference(elementRectangle, hostRectangle, elementEdge);
                    break;
            }
            switch (returnEdge)
            {
                case RectangleEdge.Bottom:
                    returnValue.Bottom = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
                case RectangleEdge.Left:
                    returnValue.Left = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
                case RectangleEdge.Right:
                    returnValue.Right = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
                case RectangleEdge.Top:
                    returnValue.Top = GetRelativeEdgeDifference(elementRectangle, hostRectangle, returnEdge);
                    break;
            }

            return returnValue;
        }

        private static RectangleEdge FinalizeReturnEdge(Rectangle elementRectangle, RectangleEdge returnEdge, Rectangle bounds)
        {
            if (bounds != null &&
                Math.Abs(GetRelativeEdgeDifference(elementRectangle, bounds, returnEdge)) > (Math.Abs(GetRelativeEdgeDifference(elementRectangle, bounds, (RectangleEdge)((int)returnEdge * -1)))))
            {
                return (RectangleEdge)((int)returnEdge * -1);
            }

            return returnEdge;
        }

        private static double GetRelativeEdgeDifference(Rectangle rect, Rectangle hostRect, RectangleEdge edge)
        {
            double edgeDifference = GetEdgeValue(rect, edge) - GetEdgeValue(hostRect, edge);
            return GetRelativeEdgeValue(edge, edgeDifference);
        }

        private async Task<ElementPositionInfo?> PositionElementRelativeAsync(double gap, Rectangle targetRect, Rectangle boundingRect, CancellationToken cancellationToken)
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
            Rectangle? calloutRectangle = await baseModule!.InvokeAsync<Rectangle>("measureElementRect", cancellationToken, calloutReference);
            //Debug.WriteLine($"Callout: {calloutRectangle.left}, {calloutRectangle.top}, {calloutRectangle.right}, {calloutRectangle.bottom}");

            ElementPosition? positionedElement = PositionElementWithinBounds(calloutRectangle, targetRect, boundingRect, positionData, gap);

            ElementPositionInfo? elementPositionInfo = positionedElement.ToElementPositionInfo(targetRect);
            return elementPositionInfo;

        }

        private ElementPosition PositionElementWithinBounds(Rectangle elementToPosition, Rectangle target, Rectangle bounding, PositionDirectionalHintData positionData, double gap)
        {
            Rectangle? estimatedElementPosition = EstimatePosition(elementToPosition, target, positionData, gap);
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
            RectangleEdge alignmentEdge = positionData.AlignmentEdge;
            bool alignTargetEdge = positionData.AlignTargetEdge;
            ElementPosition elementEstimate = new(element, positionData.TargetEdge, alignmentEdge);
            if (!DirectionalHintFixed && !CoverTarget)
            {
                elementEstimate = FlipToFit(element, target, bounding, positionData, gap);
            }
            List<RectangleEdge>? outOfBounds = GetOutOfBoundsEdges(element, bounding);
            if (alignTargetEdge)
            {
                // The edge opposite to the alignment edge might be out of bounds. Flip alignment to see if we can get it within bounds.
                if (elementEstimate.AlignmentEdge != RectangleEdge.None && outOfBounds.IndexOf((RectangleEdge)((int)elementEstimate.AlignmentEdge * -1)) > -1)
                {
                    ElementPosition? flippedElementEstimate = FlipAlignmentEdge(elementEstimate, target, gap);
                    if (IsRectangleWithinBounds(flippedElementEstimate.ElementRectangle, bounding))
                    {
                        return flippedElementEstimate;
                    }
                }
            }
            else
            {
                foreach (RectangleEdge direction in outOfBounds)
                {
                    elementEstimate.ElementRectangle = AlignEdges(elementEstimate.ElementRectangle, bounding, direction);
                }
            }
            return elementEstimate;
        }


        private Rectangle EstimatePosition(Rectangle elementToPosition, Rectangle target, PositionDirectionalHintData positionData, double gap = 0)
        {
            RectangleEdge elementEdge = CoverTarget ? positionData.TargetEdge : (RectangleEdge)((int)positionData.TargetEdge * -1);
            Rectangle estimatedElementPosition = CoverTarget ? AlignEdges(elementToPosition, target, positionData.TargetEdge, gap)
                                                             : AlignOppositeEdges(elementToPosition, target, positionData.TargetEdge, gap);
            if (positionData.AlignmentEdge == RectangleEdge.None)
            {
                double targetMiddlePoint = GetCenterValue(target, positionData.TargetEdge);
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
            Rectangle? currentEstimate = rect;
            RectangleEdge currentEdge = positionData.TargetEdge;
            RectangleEdge currentAlignment = positionData.AlignmentEdge;
            List<RectangleEdge> directions = new() { RectangleEdge.Left, RectangleEdge.Right, RectangleEdge.Bottom, RectangleEdge.Top };

            //RTL not implemented

            for (int i = 0; i < 4; i++)
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
            RectangleEdge alignmentEdge = elementEstimate.AlignmentEdge;
            RectangleEdge targetEdge = elementEstimate.TargetEdge;
            Rectangle? elementRectangle = elementEstimate.ElementRectangle;
            RectangleEdge oppositeEdge = (RectangleEdge)((int)alignmentEdge * -1);
            Rectangle? newEstimate = EstimatePosition(elementRectangle, target, new PositionDirectionalHintData(targetEdge, oppositeEdge), gap);
            return new ElementPosition(newEstimate, targetEdge, oppositeEdge);
        }

        private static List<RectangleEdge> GetOutOfBoundsEdges(Rectangle rect, Rectangle boundingRect)
        {
            List<RectangleEdge>? outOfBounds = new();
            if (rect.Top < boundingRect.Top)
            {
                outOfBounds.Add(RectangleEdge.Top);
            }
            if (rect.Bottom > boundingRect.Bottom)
            {
                outOfBounds.Add(RectangleEdge.Bottom);
            }
            if (rect.Left < boundingRect.Left)
            {
                outOfBounds.Add(RectangleEdge.Left);
            }
            if (rect.Right > boundingRect.Right)
            {
                outOfBounds.Add(RectangleEdge.Right);
            }
            return outOfBounds;
        }

        private static bool IsEdgeInBounds(Rectangle rect, Rectangle bounds, RectangleEdge edge)
        {
            double adjustedRectValue = GetRelativeRectEdgeValue(edge, rect);
            return adjustedRectValue > GetRelativeRectEdgeValue(edge, bounds);
        }

        private static bool IsRectangleWithinBounds(Rectangle rect, Rectangle boundingRect)
        {
            if (rect.Top < boundingRect.Top)
                return false;
            if (rect.Bottom > boundingRect.Bottom)
                return false;
            if (rect.Left < boundingRect.Left)
                return false;
            if (rect.Right > boundingRect.Right)
                return false;

            return true;
        }

        private static Rectangle CenterEdgeToPoint(Rectangle rect, RectangleEdge edge, double point)
        {
            RectangleEdge positiveEdge = GetFlankingEdges(edge).positiveEdge;
            double elementMiddle = GetCenterValue(rect, edge);
            double distanceToMiddle = elementMiddle - GetEdgeValue(rect, positiveEdge);
            return MoveEdge(rect, positiveEdge, point - distanceToMiddle);
        }

        private static Rectangle AlignEdges(Rectangle rect, Rectangle target, RectangleEdge edge, double gap = 0)
        {
            return MoveEdge(rect, edge, GetEdgeValue(target, edge) + GetRelativeEdgeValue(edge, gap));
        }

        private static Rectangle AlignOppositeEdges(Rectangle rect, Rectangle target, RectangleEdge targetEdge, double gap = 0)
        {
            int oppositeEdge = (int)targetEdge * -1;
            double adjustedGap = GetRelativeEdgeValue((RectangleEdge)oppositeEdge, gap);
            return MoveEdge(rect, (RectangleEdge)((int)targetEdge * -1), GetEdgeValue(target, targetEdge) + adjustedGap);
        }

        private static Rectangle MoveEdge(Rectangle rect, RectangleEdge edge, double newValue)
        {
            double difference = GetEdgeValue(rect, edge) - newValue;
            rect = SetEdgeValue(rect, edge, newValue);
            rect = SetEdgeValue(rect, (RectangleEdge)((int)edge * -1), GetEdgeValue(rect, (RectangleEdge)((int)edge * -1)) - difference);
            return rect;
        }

        private static double GetRelativeRectEdgeValue(RectangleEdge edge, Rectangle rect)
        {
            return GetRelativeEdgeValue(edge, GetEdgeValue(rect, edge));
        }

        private static double GetRelativeEdgeValue(RectangleEdge edge, double value)
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

        private static RectangleEdge GetClosestEdge(RectangleEdge targetEdge, Rectangle targetRect, Rectangle boundingRect)
        {
            double targetCenter = GetCenterValue(targetRect, targetEdge);
            double boundingCenter = GetCenterValue(boundingRect, targetEdge);
            (RectangleEdge positiveEdge, RectangleEdge negativeEdge) = GetFlankingEdges(targetEdge);
            if (targetCenter <= boundingCenter)
                return positiveEdge;
            else
                return negativeEdge;
        }

        private static double GetCenterValue(Rectangle rect, RectangleEdge edge)
        {
            (RectangleEdge positiveEdge, RectangleEdge negativeEdge) = GetFlankingEdges(edge);
            return (GetEdgeValue(rect, positiveEdge) + GetEdgeValue(rect, negativeEdge)) / 2;
        }

        private static double GetEdgeValue(Rectangle rect, RectangleEdge edge)
        {
            return edge switch
            {
                RectangleEdge.Left => rect.Left,
                RectangleEdge.Right => rect.Right,
                RectangleEdge.Top => rect.Top,
                RectangleEdge.Bottom => rect.Bottom,
                _ => 0,
            };
        }
        private static Rectangle SetEdgeValue(Rectangle rect, RectangleEdge edge, double value)
        {
            switch (edge)
            {
                case RectangleEdge.Left:
                    rect.Left = value;
                    break;
                case RectangleEdge.Right:
                    rect.Right = value;
                    break;
                case RectangleEdge.Top:
                    rect.Top = value;
                    break;
                case RectangleEdge.Bottom:
                    rect.Bottom = value;
                    break;
            }
            return rect;
        }

        private static (RectangleEdge positiveEdge, RectangleEdge negativeEdge) GetFlankingEdges(RectangleEdge edge)
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

        Dictionary<DirectionalHint, PositionDirectionalHintData> DirectionalDictionary = new()
        {
            { DirectionalHint.TopLeftEdge, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.Left) },
            { DirectionalHint.TopCenter, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.None) },
            { DirectionalHint.TopRightEdge, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.Right) },
            { DirectionalHint.TopAutoEdge, new PositionDirectionalHintData(RectangleEdge.Top, RectangleEdge.None, true) },
            { DirectionalHint.BottomLeftEdge, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.Left) },
            { DirectionalHint.BottomCenter, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.None) },
            { DirectionalHint.BottomRightEdge, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.Right) },
            { DirectionalHint.BottomAutoEdge, new PositionDirectionalHintData(RectangleEdge.Bottom, RectangleEdge.None, true) },
            { DirectionalHint.LeftTopEdge, new PositionDirectionalHintData(RectangleEdge.Left, RectangleEdge.Top) },
            { DirectionalHint.LeftCenter, new PositionDirectionalHintData(RectangleEdge.Left, RectangleEdge.None) },
            { DirectionalHint.LeftBottomEdge, new PositionDirectionalHintData(RectangleEdge.Left, RectangleEdge.Bottom) },
            { DirectionalHint.RightTopEdge, new PositionDirectionalHintData(RectangleEdge.Right, RectangleEdge.Top) },
            { DirectionalHint.RightCenter, new PositionDirectionalHintData(RectangleEdge.Right, RectangleEdge.None) },
            { DirectionalHint.RightBottomEdge, new PositionDirectionalHintData(RectangleEdge.Right, RectangleEdge.Bottom) },
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
                        $"border-radius:{Theme?.Effects.RoundedCorner2};" +
                        $"box-shadow:{Theme?.Effects.Elevation16};" +
                        $"{(CalloutWidth != 0 ? $"width:{CalloutWidth}px;" : "")}" +
                        $"{(CalloutMaxWidth != 0 ? $"max-width:{CalloutMaxWidth}px;" : "")}" +
                        $"outline:transparent;"
            };
            CalloutMainRule.Properties = new CssString()
            {
                Css = $"background-color:{(BackgroundColor ?? (Theme?.SemanticColors.MenuBackground))};" +
                        $"overflow-x:hidden;" +
                        $"overflow-y:{(overflowYHidden ? "hidden" : "auto")};" +
                        $"position:relative;" +
                        $"border-radius: {Theme?.Effects.RoundedCorner2};"
            };
            CalloutBeakRule.Properties = new CssString()
            {
                Css = $"position:absolute;" +
                        $"background-color:{(BackgroundColor ?? (Theme?.SemanticColors.MenuBackground))};" +
                        $"box-shadow:inherit;" +
                        $"border:inherit;" +
                        $"box-sizing:border-box;" +
                        $"transform:rotate(45deg);" +
                        $"height:{BeakWidth}px;" +
                        $"width:{BeakWidth}px;"
            };
        }

        public override async ValueTask DisposeAsync()
        {
            cancellationTokenSource.Cancel();
            if (calloutModule != null && eventHandlerIds != null)
            {
                isEventHandlersRegistered = false;
                await calloutModule.InvokeAsync<object>("unregisterHandlers", eventHandlerIds);
                await calloutModule.DisposeAsync();
            }
            if (baseModule != null)
            {
                await baseModule.DisposeAsync();
                baseModule = null;
            }
            selfReference?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}