//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
/// <reference path="../../FluentUI.FocusTrapZone/wwwroot/focusTrapZone.ts" />
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />

namespace FluentUIPanel {

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface Map<T> {
        [K: number]: T;
    }

    class Handler {

        static i: number = 1;
        static listeners: Map<EventParams> = {};

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


    export function registerSizeHandler(panel: DotNetReferenceType): number {
        //var window = targetElement.ownerDocument.defaultView;

        var resizeId = Handler.addListener(window, "resize", (ev: Event) => { panel.invokeMethodAsync("UpdateFooterPositionAsync"); }, false);
        //var blurId = Handler.addListener(targetElement, "blur", (ev: Event) => { ev.preventDefault(); panel.invokeMethodAsync("OnBlur"); }, false);
        return resizeId;
    }

    export function registerMouseDownHandler(panelElement: HTMLElement, panelDotNet: DotNetReferenceType): number {
        var mouseDownId = Handler.addListener(document.body, "mousedown", (ev: Event) =>
        {
            //first get whether click is inside panel
            if (!ev.defaultPrevented) {
                var contains = FluentUIBaseComponent.elementContains(panelElement, <HTMLElement>ev.target);
                //var contains = window["FluentUIFocusTrapZone"].elementContains(panelElement, ev.target);
                if (!contains) {
                    ev.preventDefault();
                    panelDotNet.invokeMethodAsync("DismissOnOuterClick", contains);
                }
            }
        }, true);
        return mouseDownId;
    }

    export function unregisterHandler(id: number): void {
        Handler.removeListener(id);
    }

    interface EventParams {
        element: HTMLElement | Window;
        event: string;
        handler: (ev: Event) => void;
        capture: boolean;
    }
    
    
    const DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';

    export function makeElementScrollAllower(element: HTMLElement) : number[] {
        let _previousClientY = 0;
        let _element: Element | null = null;

        // remember the clientY for future calls of _preventOverscrolling
        const _saveClientY = (event: TouchEvent): void => {
            if (event.targetTouches.length === 1) {
                _previousClientY = event.targetTouches[0].clientY;
            }
        };

        // prevent the body from scrolling when the user attempts
        // to scroll past the top or bottom of the element
        const _preventOverscrolling = (event: TouchEvent): void => {
            // only respond to a single-finger touch
            if (event.targetTouches.length !== 1) {
                return;
            }

            // prevent the body touchmove handler from firing
            // so that scrolling is allowed within the element
            event.stopPropagation();

            if (!_element) {
                return;
            }

            const clientY = event.targetTouches[0].clientY - _previousClientY;

            const scrollableParent = findScrollableParent(event.target as HTMLElement);
            if (scrollableParent) {
                _element = scrollableParent;
            }

            // if the element is scrolled to the top,
            // prevent the user from scrolling up
            if (_element.scrollTop === 0 && clientY > 0) {
                event.preventDefault();
            }

            // if the element is scrolled to the bottom,
            // prevent the user from scrolling down
            if (_element.scrollHeight - _element.scrollTop <= _element.clientHeight && clientY < 0) {
                event.preventDefault();
            }
        };

     
        var touchStartId = Handler.addListener(element, "touchstart", _saveClientY, false);
        var touchMoveId = Handler.addListener(element, "touchmove", _preventOverscrolling, false);

        return [touchStartId, touchMoveId];
    }

    function findScrollableParent(startingElement: HTMLElement | null): HTMLElement | null {
        let el: HTMLElement | null = startingElement;

        // First do a quick scan for the scrollable attribute.
        while (el && el !== document.body) {
            if (el.getAttribute(DATA_IS_SCROLLABLE_ATTRIBUTE) === 'true') {
                return el;
            }
            el = el.parentElement;
        }

        // If we haven't found it, the use the slower method: compute styles to evaluate if overflow is set.
        el = startingElement;

        while (el && el !== document.body) {
            if (el.getAttribute(DATA_IS_SCROLLABLE_ATTRIBUTE) !== 'false') {
                const computedStyles = getComputedStyle(el);
                let overflowY = computedStyles ? computedStyles.getPropertyValue('overflow-y') : '';

                if (overflowY && (overflowY === 'scroll' || overflowY === 'auto')) {
                    return el;
                }
            }

            el = el.parentElement;
        }

        // Fall back to window scroll.
        if (!el || el === document.body) {
            // tslint:disable-next-line:no-any
            el = window as any;
        }

        return el;
    }

}

(<any>window)['FluentUIPanel'] = FluentUIPanel || {};

