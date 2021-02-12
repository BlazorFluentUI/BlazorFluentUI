/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />

namespace FluentUISlider {

    interface DotNetReferenceType {
        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface EventParams {
        element: HTMLElement | Window;
        event: string;
        handler: (ev: Event) => void;
        capture: boolean;
    }

    class Handler {

        static objectListeners: Map<DotNetReferenceType, Map<string, EventParams>> = new Map<DotNetReferenceType, Map<string, EventParams>>();
        
        static addListener(ref: DotNetReferenceType, element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): void {
            let listeners: Map<string, EventParams>;
            if (this.objectListeners.has(ref)) {
                listeners = this.objectListeners.get(ref);
            } else {
                listeners = new Map<string, EventParams>();
                this.objectListeners.set(ref, listeners);
            }
            element.addEventListener(event, handler, capture);
            listeners.set(event, { capture: capture, event: event, handler: handler, element: element });
        }
        static removeListener(ref: DotNetReferenceType,event: string): void {
            if (this.objectListeners.has(ref)) {
                let listeners = this.objectListeners.get(ref);
                if (listeners.has(event)) {
                    var handler = listeners.get(event);
                    handler.element.removeEventListener(handler.event, handler.handler, handler.capture);
                }
                listeners.delete[event];
            }
        }
    }    

    export function registerMouseOrTouchStart(slider: DotNetReferenceType, slideBox:HTMLElement, sliderLine: HTMLElement )  {
        Handler.addListener(slider, slideBox, "mousedown", (ev: MouseEvent) => { onMouseMoveOrTouchStart(slider, sliderLine, ev); }, true);
        Handler.addListener(slider, slideBox, "touchstart", (ev: TouchEvent) => { onMouseMoveOrTouchStart(slider, sliderLine, ev); }, true);
        Handler.addListener(slider, slideBox, "keydown", (ev: KeyboardEvent) => { onKeyDown(slider, ev) }, true);
    }

    function onMouseMoveOrTouchStart(slider: DotNetReferenceType, sliderLine: HTMLElement, event: MouseEvent | TouchEvent): void {
        if (event.type === 'mousedown') {
            Handler.addListener(slider, window, "mousemove", (ev: MouseEvent) => { onMouseMoveOrTouchMove(slider, sliderLine, ev); }, true);
            Handler.addListener(slider, window, "mouseup", (ev: MouseEvent) => { onMouseUpOrTouchEnd(slider, sliderLine, ev); }, true);
        } else if (event.type === 'touchstart') {
            Handler.addListener(slider, window, "touchmove", (ev: TouchEvent) => { onMouseMoveOrTouchMove(slider, sliderLine, ev); }, true);
            Handler.addListener(slider, window, "touchend", (ev: TouchEvent) => { onMouseUpOrTouchEnd(slider, sliderLine, ev); }, true);
        }
        onMouseMoveOrTouchMove(slider, sliderLine, event, true);
    }

    function onMouseMoveOrTouchMove(slider: DotNetReferenceType, sliderLine: HTMLElement, event: MouseEvent | TouchEvent, suppressEventCancelation?:boolean): void {
        let sliderPositionRect : ClientRect = sliderLine.getBoundingClientRect();
        let horizontalPosition = getPosition(event, false);
        let verticalPosition = getPosition(event, true);

        slider.invokeMethodAsync("MouseOrTouchMove", sliderPositionRect, horizontalPosition, verticalPosition);

        if (suppressEventCancelation) {
            event.preventDefault();
            event.stopPropagation();
        }
    }

    function getPosition(event: MouseEvent | TouchEvent, vertical: boolean) {
        let currentPosition: number | undefined;
        switch (event.type) {
            case 'mousedown':
            case 'mousemove':
                currentPosition = !vertical ? (event as MouseEvent).clientX : (event as MouseEvent).clientY;
                break;
            case 'touchstart':
            case 'touchmove':
                currentPosition = !vertical
                    ? (event as TouchEvent).touches[0].clientX
                    : (event as TouchEvent).touches[0].clientY;
                break;
        }
        return currentPosition;
    }

    async function onMouseUpOrTouchEnd(slider: DotNetReferenceType, sliderLine: HTMLElement, event: MouseEvent | TouchEvent, suppressEventCancelation?: boolean): Promise<void> {
        await slider.invokeMethodAsync("MouseOrTouchEnd");
        if (event.type === 'mouseup') {
            Handler.removeListener(slider, "mousemove");
            Handler.removeListener(slider, "mouseup");
        } else if (event.type === 'touchend') {
            Handler.removeListener(slider, "touchmove");
            Handler.removeListener(slider, "touchend");
        }
    }

    function onKeyDown(slider: DotNetReferenceType, event: KeyboardEvent):Promise<void> {
        let diff: number;
        let value: number;
        switch (event.which) {
            case FluentUIBaseComponent.KeyCodes.right: //right arrow
            case FluentUIBaseComponent.KeyCodes.up: //up arrow
                slider.invokeMethodAsync("OnKeyDown", { step: +1 });
                break;
            case FluentUIBaseComponent.KeyCodes.left: //left arrow
            case FluentUIBaseComponent.KeyCodes.down: //down arrow
                slider.invokeMethodAsync("OnKeyDown", { step: -1 });
                break;
            case FluentUIBaseComponent.KeyCodes.home: //home
                slider.invokeMethodAsync("OnKeyDown", { min: true });
                break;
            case FluentUIBaseComponent.KeyCodes.end: //end
                slider.invokeMethodAsync("OnKeyDown", { max: true });
                break;
            default:
                return;
        }

        event.preventDefault();
        event.stopPropagation();
    }
        
    export function unregisterHandlers(ref: DotNetReferenceType): void {
        Handler.removeListener(ref, "mousedown");
        Handler.removeListener(ref, "touchstart");
        Handler.removeListener(ref, "keydown");
    }
}

(<any>window)['FluentUISlider'] = FluentUISlider || {};

