///// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
//namespace FluentUIContextualMenu {
//    interface DotNetReferenceType {
//        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
//        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
//    }
//    interface IRectangle {
//        left: number;
//        top: number;
//        width: number;
//        height: number;
//        right?: number;
//        bottom?: number;
//    }
//    export function registerHandlers(targetElement: HTMLElement, contextualMenuItem: DotNetReferenceType): number[] {
//        var window = targetElement.ownerDocument.defaultView;
//        var mouseClickId = Handler.addListener(targetElement, "click", (ev: Event) => { ev.preventDefault(); contextualMenuItem.invokeMethodAsync("ClickHandler"); }, false);
//        //var keyDownId = Handler.addListener(targetElement, "keydown", (ev: KeyboardEvent) => {
//        //    if (ev.keyCode === FluentUIBaseComponent.KeyCodes.right) {
//        //        ev.preventDefault();
//        //        contextualMenuItem.invokeMethodAsync("KeyDownHandler", true);
//        //    } else if (ev.keyCode === FluentUIBaseComponent.KeyCodes.left) {
//        //        ev.preventDefault();
//        //        contextualMenuItem.invokeMethodAsync("KeyDownHandler", false);
//        //    }
//        //}, false);
//        var mouseEnterId = Handler.addListener(targetElement, "mouseenter", (ev: Event) => { ev.preventDefault(); contextualMenuItem.invokeMethodAsync("MouseEnterHandler"); }, false);
//        return [mouseClickId, mouseEnterId];
//    }
//    export function unregisterHandlers(ids: number[]): void {
//        for (let id of ids) {
//            Handler.removeListener(id);
//        }
//    }
//    interface EventParams {
//        element: HTMLElement | Window;
//        event: string;
//        handler: (ev: Event) => void;
//        capture: boolean;
//    }
//    interface Map<T> {
//        [K: number]: T;
//    }
//    class Handler {
//        static i: number = 1;
//        static listeners: Map<EventParams> = {};
//        static addListener(element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): number {
//            element.addEventListener(event, handler, capture);
//            this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
//            return this.i++;
//        }
//        static removeListener(id: number): void {
//            if (id in this.listeners) {
//                var h = this.listeners[id];
//                h.element.removeEventListener(h.event, h.handler, h.capture);
//                delete this.listeners[id];
//            }
//        }
//    }
//}
//(<any>window)['FluentUIContextualMenu'] = FluentUIContextualMenu || {};
//# sourceMappingURL=contextualMenu.js.map