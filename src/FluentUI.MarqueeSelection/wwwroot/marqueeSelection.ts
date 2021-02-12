/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />


namespace FluentUIMarqueeSelection {

    type IRectangle = FluentUIBaseComponent.IRectangle;
    type EventGroup = FluentUIBaseComponent.EventGroup;
    type EventParams = FluentUIBaseComponent.EventParams;

    interface DotNetReferenceType {
        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
        _id: number;
    }

    interface Point {
        top: number;
        left: number;
    }

    export function getDistanceBetweenPoints(point1: Point, point2: Point): number {
        const left1 = point1.left || 0;
        const top1 = point1.top || 0;
        const left2 = point2.left || 0;
        const top2 = point2.top || 0;

        let distance = Math.sqrt(Math.pow(left1 - left2, 2) + Math.pow(top1 - top2, 2));

        return distance;
    }

   
    

    declare function setTimeout(cb: Function, delay: number): number;

    const SCROLL_ITERATION_DELAY = 16;
    const SCROLL_GUTTER = 100;
    const MAX_SCROLL_VELOCITY = 15;

    export function getRect(element: HTMLElement | Window | null): IRectangle | undefined {
        let rect: IRectangle | undefined;
        if (element) {
            if (element === window) {
                rect = {
                    left: 0,
                    top: 0,
                    width: window.innerWidth,
                    height: window.innerHeight,
                    right: window.innerWidth,
                    bottom: window.innerHeight,
                };
            } else if ((element as HTMLElement).getBoundingClientRect) {
                rect = (element as HTMLElement).getBoundingClientRect();
            }
        }
        return rect;
    }

    export class AutoScroll {
        private _events: EventGroup;
        private _scrollableParent: HTMLElement | null;
        private _scrollRect: IRectangle | undefined;
        private _scrollVelocity: number;
        private _isVerticalScroll: boolean;
        private _timeoutId: number;

        constructor(element: HTMLElement) {
            this._events = new FluentUIBaseComponent.EventGroup(this);
            this._scrollableParent = FluentUIBaseComponent.findScrollableParent(element) as HTMLElement;

            this._incrementScroll = this._incrementScroll.bind(this);
            this._scrollRect = getRect(this._scrollableParent);

            // tslint:disable-next-line:no-any
            if (this._scrollableParent === (window as any)) {
                this._scrollableParent = document.body;
            }

            if (this._scrollableParent) {
                this._events.on(window, 'mousemove', this._onMouseMove, true);
                this._events.on(window, 'touchmove', this._onTouchMove, true);
            }
        }

        public dispose(): void {
            this._events.dispose();
            this._stopScroll();
        }

        private _onMouseMove(ev: MouseEvent): void {
            this._computeScrollVelocity(ev);
        }

        private _onTouchMove(ev: TouchEvent): void {
            if (ev.touches.length > 0) {
                this._computeScrollVelocity(ev);
            }
        }

        private _computeScrollVelocity(ev: MouseEvent | TouchEvent): void {
            if (!this._scrollRect) {
                return;
            }

            let clientX: number;
            let clientY: number;
            if ('clientX' in ev) {
                clientX = ev.clientX;
                clientY = ev.clientY;
            } else {
                clientX = ev.touches[0].clientX;
                clientY = ev.touches[0].clientY;
            }

            let scrollRectTop = this._scrollRect.top;
            let scrollRectLeft = this._scrollRect.left;
            let scrollClientBottom = scrollRectTop + this._scrollRect.height - SCROLL_GUTTER;
            let scrollClientRight = scrollRectLeft + this._scrollRect.width - SCROLL_GUTTER;

            // variables to use for alternating scroll direction
            let scrollRect;
            let clientDirection;
            let scrollClient;

            // if either of these conditions are met we are scrolling vertically else horizontally
            if (clientY < scrollRectTop + SCROLL_GUTTER || clientY > scrollClientBottom) {
                clientDirection = clientY;
                scrollRect = scrollRectTop;
                scrollClient = scrollClientBottom;
                this._isVerticalScroll = true;
            } else {
                clientDirection = clientX;
                scrollRect = scrollRectLeft;
                scrollClient = scrollClientRight;
                this._isVerticalScroll = false;
            }

            // calculate scroll velocity and direction
            if (clientDirection! < scrollRect + SCROLL_GUTTER) {
                this._scrollVelocity = Math.max(
                    -MAX_SCROLL_VELOCITY,
                    -MAX_SCROLL_VELOCITY * ((SCROLL_GUTTER - (clientDirection - scrollRect)) / SCROLL_GUTTER),
                );
            } else if (clientDirection > scrollClient) {
                this._scrollVelocity = Math.min(
                    MAX_SCROLL_VELOCITY,
                    MAX_SCROLL_VELOCITY * ((clientDirection - scrollClient) / SCROLL_GUTTER),
                );
            } else {
                this._scrollVelocity = 0;
            }

            if (this._scrollVelocity) {
                this._startScroll();
            } else {
                this._stopScroll();
            }
        }

        private _startScroll(): void {
            if (!this._timeoutId) {
                this._incrementScroll();
            }
        }

        private _incrementScroll(): void {
            if (this._scrollableParent) {
                if (this._isVerticalScroll) {
                    this._scrollableParent.scrollTop += Math.round(this._scrollVelocity);
                } else {
                    this._scrollableParent.scrollLeft += Math.round(this._scrollVelocity);
                }
            }

            this._timeoutId = setTimeout(this._incrementScroll, SCROLL_ITERATION_DELAY);
        }

        private _stopScroll(): void {
            if (this._timeoutId) {
                clearTimeout(this._timeoutId);
                delete this._timeoutId;
            }
        }
    }

    //class Handler {

    //    static objectListeners: Map<DotNetReferenceType, Map<string, EventParams>> = new Map<DotNetReferenceType, Map<string, EventParams>>();

    //    static addListener(ref: DotNetReferenceType, element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): void {
    //        let listeners: Map<string, EventParams>;
    //        if (this.objectListeners.has(ref)) {
    //            listeners = this.objectListeners.get(ref);
    //        } else {
    //            listeners = new Map<string, EventParams>();
    //            this.objectListeners.set(ref, listeners);
    //        }
    //        element.addEventListener(event, handler, capture);
    //        listeners.set(event, { capture: capture, event: event, handler: handler, element: element });
    //    }
    //    static removeListener(ref: DotNetReferenceType, event: string): void {
    //        if (this.objectListeners.has(ref)) {
    //            let listeners = this.objectListeners.get(ref);
    //            if (listeners.has(event)) {
    //                var handler = listeners.get(event);
    //                handler.element.removeEventListener(handler.event, handler.handler, handler.capture);
    //            }
    //            listeners.delete[event];
    //        }
    //    }
    //}

    const marqueeSelections = new Map<number, MarqueeSelection>();

    export function registerMarqueeSelection(dotNet: DotNetReferenceType, root: HTMLElement, props: IMarqueeSelectionProps) {
        let marqueeSelection = new MarqueeSelection(dotNet, root, props);
        marqueeSelections.set(dotNet._id, marqueeSelection);
    }

    export function updateProps(dotNet: DotNetReferenceType, props: IMarqueeSelectionProps) {
        //assume itemsource may have changed... 

        var marqueeSelection = marqueeSelections.get(dotNet._id);
        if (marqueeSelection !== null) {
            marqueeSelection.props = props;
        }
    }

    export function unregisterMarqueeSelection(dotNet: DotNetReferenceType) {
        let marqueeSelection = marqueeSelections.get(dotNet._id);
        marqueeSelection.dispose();
        marqueeSelections.delete(dotNet._id);
    }



    interface IMarqueeSelectionProps {
        isDraggingConstrainedToRoot: boolean;
        isEnabled: boolean;
    }

    const MIN_DRAG_DISTANCE = 5;

    class MarqueeSelection {

        dotNet: DotNetReferenceType;
        root: HTMLElement;
        scrollableParent: HTMLElement;
        scrollableSurface: HTMLElement;
        isTouch: boolean;

        events: EventGroup;
        autoScroll: AutoScroll;
        _async: FluentUIBaseComponent.Async;

        props: IMarqueeSelectionProps;
        dragRect: IRectangle;

        animationFrameRequest: number;

        _lastMouseEvent: MouseEvent | undefined;
        _selectedIndicies: { [key: string]: boolean } | undefined;
        _preservedIndicies: number[] | undefined;
        _scrollTop: number;
        _scrollLeft: number;
        _rootRect: IRectangle;
        _dragOrigin: Point | undefined;
        _itemRectCache: { [key: string]: IRectangle } | undefined;
        _allSelectedIndices: { [key: string]: boolean } | undefined;

        _mirroredDragRect: IRectangle;

        constructor(dotNet: DotNetReferenceType, root: HTMLElement, props: IMarqueeSelectionProps) {
            this.dotNet = dotNet;
            this.root = root;
            this.props = props;

            this.events = new FluentUIBaseComponent.EventGroup(this);
            this._async = new FluentUIBaseComponent.Async(this);

            this.scrollableParent = FluentUIBaseComponent.findScrollableParent(root);
            this.scrollableSurface = this.scrollableParent === (window as any) ? document.body : this.scrollableParent;

            const hitTarget = props.isDraggingConstrainedToRoot ? this.root : this.scrollableSurface;

            this.events.on(hitTarget, 'mousedown', this.onMouseDown);
            //this.events.on(hitTarget, 'touchstart', this.onTouchStart, true);
            //this.events.on(hitTarget, 'pointerdown', this.onPointerDown, true);
        }

        public updateProps(props: IMarqueeSelectionProps): void {
            this.props = props;
            this._itemRectCache = {};
        }

        public dispose(): void {
            if (this.autoScroll) {
                this.autoScroll.dispose();
            }
            delete this.scrollableParent;
            delete this.scrollableSurface;

            this.events.dispose();
            this._async.dispose();
        }

        private _isMouseEventOnScrollbar(ev: MouseEvent): boolean {
            const targetElement = ev.target as HTMLElement;
            const targetScrollbarWidth = targetElement.offsetWidth - targetElement.clientWidth;

            if (targetScrollbarWidth) {
                const targetRect = targetElement.getBoundingClientRect();

                // Check vertical scroll
                //if (getRTL(this.props.theme)) {
                //    if (ev.clientX < targetRect.left + targetScrollbarWidth) {
                //        return true;
                //    }
                //} else {
                if (ev.clientX > targetRect.left + targetElement.clientWidth) {
                    return true;
                }
                //}

                // Check horizontal scroll
                if (ev.clientY > targetRect.top + targetElement.clientHeight) {
                    return true;
                }
            }

            return false;
        }

        private onMouseDown = async (ev: MouseEvent): Promise<void> => {

            // Ensure the mousedown is within the boundaries of the target. If not, it may have been a click on a scrollbar.
            if (this._isMouseEventOnScrollbar(ev)) {
                return;
            }
            if (this._isInSelectionToggle(ev)) {
                return;
            }

            if (
                !this.isTouch &&
                this.props.isEnabled &&
                !this._isDragStartInSelection(ev)) {

                let shouldStart = await this.dotNet.invokeMethodAsync<boolean>("OnShouldStartSelectionInternal");
                if (shouldStart) {

                    if (this.scrollableSurface && ev.button === 0 && this.root) {
                        this._selectedIndicies = {};
                        this._preservedIndicies = undefined;
                        this.events.on(window, 'mousemove', this._onAsyncMouseMove, true);
                        this.events.on(this.scrollableParent, 'scroll', this._onAsyncMouseMove);
                        this.events.on(window, 'click', this.onMouseUp, true);

                        this.autoScroll = new AutoScroll(this.root);
                        this._scrollTop = this.scrollableSurface.scrollTop;
                        this._scrollLeft = this.scrollableSurface.scrollLeft;
                        this._rootRect = this.root.getBoundingClientRect();

                        await this._onMouseMove(ev);
                    }
                }
            }

        }

        private _getRootRect(): IRectangle {
            return {
                left: this._rootRect.left + (this._scrollLeft - this.scrollableSurface.scrollLeft),
                top: this._rootRect.top + (this._scrollTop - this.scrollableSurface.scrollTop),
                width: this._rootRect.width,
                height: this._rootRect.height,
            };
        }

        private _onAsyncMouseMove(ev: MouseEvent): void {
            this.animationFrameRequest = window.requestAnimationFrame(async () => {
                await this._onMouseMove(ev);
            });

            ev.stopPropagation();
            ev.preventDefault();
        }

        private async _onMouseMove(ev: MouseEvent): Promise<boolean | undefined> {
            if (!this.autoScroll) {
                return;
            }

            if (ev.clientX !== undefined) {
                this._lastMouseEvent = ev;
            }

            const rootRect = this._getRootRect();
            const currentPoint = { left: ev.clientX - rootRect.left, top: ev.clientY - rootRect.top };

            if (!this._dragOrigin) {
                this._dragOrigin = currentPoint;
            }

            if (ev.buttons !== undefined && ev.buttons === 0) {
                this.onMouseUp(ev);
            } else {
                if (this._mirroredDragRect || getDistanceBetweenPoints(this._dragOrigin, currentPoint) > MIN_DRAG_DISTANCE) {
                    if (!this._mirroredDragRect) {
                        //const { selection } = this.props;

                        if (!ev.shiftKey) {
                            await this.dotNet.invokeMethodAsync("UnselectAll");
                            //selection.setAllSelected(false);
                        }

                        this._preservedIndicies = await this.dotNet.invokeMethodAsync<number[]>("GetSelectedIndicesAsync");
                    }

                    // We need to constrain the current point to the rootRect boundaries.
                    const constrainedPoint = this.props.isDraggingConstrainedToRoot
                        ? {
                            left: Math.max(0, Math.min(rootRect.width, this._lastMouseEvent!.clientX - rootRect.left)),
                            top: Math.max(0, Math.min(rootRect.height, this._lastMouseEvent!.clientY - rootRect.top)),
                        }
                        : {
                            left: this._lastMouseEvent!.clientX - rootRect.left,
                            top: this._lastMouseEvent!.clientY - rootRect.top,
                        };

                    this.dragRect = {
                        left: Math.min(this._dragOrigin.left || 0, constrainedPoint.left),
                        top: Math.min(this._dragOrigin.top || 0, constrainedPoint.top),
                        width: Math.abs(constrainedPoint.left - (this._dragOrigin.left || 0)),
                        height: Math.abs(constrainedPoint.top - (this._dragOrigin.top || 0)),
                    };

                    await this._evaluateSelectionAsync(this.dragRect, rootRect);

                    this._mirroredDragRect = this.dragRect;
                    await this.dotNet.invokeMethodAsync("SetDragRect", this.dragRect);
                    //this.setState({ dragRect });
                }
            }

            return false;
        }

        private async _evaluateSelectionAsync(dragRect: IRectangle, rootRect: IRectangle): Promise<void> {
            // Break early if we don't need to evaluate.
            if (!dragRect || !this.root) {
                return;
            }

            const allElements = this.root.querySelectorAll('[data-selection-index]');

            if (!this._itemRectCache) {
                this._itemRectCache = {};
            }

            for (let i = 0; i < allElements.length; i++) {
                const element = allElements[i];
                const index = element.getAttribute('data-selection-index') as string;

                // Pull the memoized rectangle for the item, or the get the rect and memoize.
                let itemRect = this._itemRectCache[index];

                if (!itemRect) {
                    itemRect = element.getBoundingClientRect();

                    // Normalize the item rect to the dragRect coordinates.
                    itemRect = {
                        left: itemRect.left - rootRect.left,
                        top: itemRect.top - rootRect.top,
                        width: itemRect.width,
                        height: itemRect.height,
                        right: itemRect.left - rootRect.left + itemRect.width,
                        bottom: itemRect.top - rootRect.top + itemRect.height,
                    };

                    if (itemRect.width > 0 && itemRect.height > 0) {
                        this._itemRectCache[index] = itemRect;
                    }
                }

                if (
                    itemRect.top < dragRect.top + dragRect.height &&
                    itemRect.bottom! > dragRect.top &&
                    itemRect.left < dragRect.left + dragRect.width &&
                    itemRect.right! > dragRect.left
                ) {
                    this._selectedIndicies![index] = true;
                } else {
                    delete this._selectedIndicies![index];
                }
            }

            // set previousSelectedIndices to be all of the selected indices from last time
            const previousSelectedIndices = this._allSelectedIndices || {};
            this._allSelectedIndices = {};

            // set all indices that are supposed to be selected in _allSelectedIndices
            for (const index in this._selectedIndicies!) {
                if (this._selectedIndicies!.hasOwnProperty(index)) {
                    this._allSelectedIndices![index] = true;
                }
            }

            if (this._preservedIndicies) {
                for (const index of this._preservedIndicies!) {
                    this._allSelectedIndices![index] = true;
                }
            }

            // check if needs to update selection, only when current _allSelectedIndices
            // is different than previousSelectedIndices
            let needToUpdate = false;
            for (const index in this._allSelectedIndices!) {
                if (this._allSelectedIndices![index] !== previousSelectedIndices![index]) {
                    needToUpdate = true;
                    break;
                }
            }

            if (!needToUpdate) {
                for (const index in previousSelectedIndices!) {
                    if (this._allSelectedIndices![index] !== previousSelectedIndices![index]) {
                        needToUpdate = true;
                        break;
                    }
                }
            }

            // only update selection when needed
            if (needToUpdate) {
                // Stop change events, clear selection to re-populate.
                //selection.setChangeEvents(false);
                //selection.setAllSelected(false);
                await this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                await this.dotNet.invokeMethodAsync("UnselectAll");//.then(_ => {
                const indices: number[] = [];
                for (const index of Object.keys(this._allSelectedIndices!)) {
                    indices.push(Number(index));
                    //selection.setIndexSelected(Number(index), true, false);
                }
                await this.dotNet.invokeMethodAsync("SetSelectedIndices", indices);
                //});
                //for (const index of Object.keys(this._allSelectedIndices!)) {
                //    selection.setIndexSelected(Number(index), true, false);
                //}

                //selection.setChangeEvents(true);
                await this.dotNet.invokeMethodAsync("SetChangeEvents", true);
            }
        }

        private onMouseUp(ev: MouseEvent): void {
            this.events.off(window);
            this.events.off(this.scrollableParent, 'scroll');

            if (this.autoScroll) {
                this.autoScroll.dispose();
            }

            this.autoScroll = this._dragOrigin = this._lastMouseEvent = undefined;
            this._selectedIndicies = this._itemRectCache = undefined;

            if (this._mirroredDragRect) {
                //this.setState({
                //    dragRect: undefined,
                //});
                this._mirroredDragRect = null;
                this.dotNet.invokeMethodAsync("SetDragRect", null);

                ev.preventDefault();
                ev.stopPropagation();
            }
        }

        private _isInSelectionToggle(ev: MouseEvent): boolean {
            let element: HTMLElement | null = ev.target as HTMLElement;

            while (element && element !== this.root) {
                if (element.getAttribute('data-selection-toggle') === 'true') {
                    return true;
                }

                element = element.parentElement;
            }

            return false;
        }

        /**
   * We do not want to start the marquee if we're trying to marquee
   * from within an existing marquee selection.
   */
        private _isDragStartInSelection(ev: MouseEvent): boolean {

            const selectedElements = this.root.querySelectorAll('[data-is-selected]');
            for (let i = 0; i < selectedElements.length; i++) {
                const element = selectedElements[i];
                const itemRect = element.getBoundingClientRect();
                if (this._isPointInRectangle(itemRect, { left: ev.clientX, top: ev.clientY })) {
                    return true;
                }
            }

            return false;
        }

        private _isPointInRectangle(rectangle: IRectangle, point: Point): boolean {
            return (
                !!point.top &&
                rectangle.top < point.top &&
                rectangle.bottom! > point.top &&
                !!point.left &&
                rectangle.left < point.left &&
                rectangle.right! > point.left
            );
        }


    }


}

(<any>window)['FluentUIMarqueeSelection'] = FluentUIMarqueeSelection || {};

