//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }


namespace FluentUICallout {

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }

    export function registerHandlers(targetElement: HTMLElement, calloutRef: DotNetReferenceType): number[] {
        var window = targetElement.ownerDocument.defaultView;

        var calloutDivId = Handler.addCallout(targetElement);
        
        var scrollId = Handler.addListener(window, "scroll", (ev: Event) => { if (checkTarget(ev, targetElement)) { calloutRef.invokeMethodAsync("ScrollHandler"); }; }, true);
        var resizeId = Handler.addListener(window, "resize", (ev: Event) => { if (checkTarget(ev, targetElement)) { calloutRef.invokeMethodAsync("ResizeHandler"); }; }, true);
        var focusId = Handler.addListener(document.documentElement, "focus", (ev: Event) =>
        {
            var outsideCallout = true;
            for (let prop in Handler.targetCombinedElements) {
                if (Object.prototype.hasOwnProperty.call(Handler.targetCombinedElements, prop)) {
                    outsideCallout = checkTarget(ev, Handler.targetCombinedElements[prop]);
                    if (outsideCallout == false)
                        break;
                }
            }
            if (outsideCallout)
                calloutRef.invokeMethodAsync("FocusHandler");
        }, true);
        var clickId = Handler.addListener(document.documentElement, "click", (ev: Event) =>
        {
            var outsideCallout = true;
            for (let prop in Handler.targetCombinedElements) {
                if (Object.prototype.hasOwnProperty.call(Handler.targetCombinedElements, prop)) {
                    outsideCallout = checkTarget(ev, Handler.targetCombinedElements[prop]);
                    if (outsideCallout == false)
                        break;
                }
            }
            if (outsideCallout)
                calloutRef.invokeMethodAsync("ClickHandler");
        }, true);

        //set focus, too
        
        return [scrollId, resizeId, focusId, clickId, calloutDivId];
    }

    export function unregisterHandlers(ids: number[]): void {

        Handler.removeCallout(ids[ids.length - 1]);

        var handlerIds = ids.slice(0, ids.length - 1);

        for (let id of handlerIds) {
            Handler.removeListener(id);
        }
    }

    interface EventParams {
        element: HTMLElement | Window;
        event: string;
        handler: (ev:Event) => void;
        capture: boolean;
    }
    interface Map<T> {
        [K: number]: T;
    }

    class Handler {       

        static i: number = 1;
        static listeners: Map<EventParams> = {};
        static targetCombinedElements: Map<HTMLElement> = {};

        static addCallout(element: HTMLElement): number {
            this.targetCombinedElements[this.i] = element;
            return this.i++;
        }
        static removeCallout(id:number): void {
            if (id in this.targetCombinedElements)
                delete this.targetCombinedElements[id];
        }

        static addListener(element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): number {
            element.addEventListener(event, handler, capture);
            this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
            return this.i++;
        } 
        static removeListener(id: number): void {
            if (id in this.listeners) {
                var h = this.listeners[id];
                h.element.removeEventListener(h.event, h.handler, h.capture);
                delete this.listeners[id];
            }
        }
    }

    function clickHandler(ev: Event) {
        
    }


    function checkTarget(ev: Event, targetElement: HTMLElement): boolean {
        const target = ev.target as HTMLElement;
        const isEventTargetOutsideCallout = !elementContains(targetElement, target);
        return isEventTargetOutsideCallout;

        //// complicated mess that includes mouse events for right-click menus...
        //if (
        //    (!this._target && isEventTargetOutsideCallout) ||
        //    (ev.target !== this._targetWindow &&
        //        isEventTargetOutsideCallout &&
        //        ((this._target as MouseEvent).stopPropagation ||
        //            (!this._target || (target !== this._target && !elementContains(this._target as HTMLElement, target)))))
        //) {
        //    return true;
        //}
        //return false;
    }    

    function elementContains(parent: HTMLElement | null, child: HTMLElement | null, allowVirtualParents: boolean = true): boolean {
        let isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    let nextParent: HTMLElement | null = getParent(child);
                    // console.log("NextParent: " + nextParent);
                    if (nextParent === parent) {
                        isContained = true;
                        break;
                    }
                    child = nextParent;
                }
            } else if (parent.contains) {
                isContained = parent.contains(child);
            }
        }
        return isContained;
    }

    function getParent(child: HTMLElement): HTMLElement | null {
        return child && (child.parentNode && (child.parentNode as HTMLElement));
    }

  

    export function getWindow(element: HTMLElement): Window {
        return element.ownerDocument.defaultView;
    }

    export function getWindowRect(): IRectangle {
        var rect: IRectangle = {
            width: window.innerWidth,// - scrollbarwidth
            height: window.innerHeight,
            top: 0,
            left: 0
        }
        return rect;
    };



}




(<any>window)['FluentUICallout'] = FluentUICallout || {};

