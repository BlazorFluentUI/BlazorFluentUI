export function registerHandlers(targetElement, calloutRef) {
    var window = targetElement.ownerDocument.defaultView;
    var calloutDivId = Handler.addCallout(targetElement);
    var scrollId = Handler.addListener(window, "scroll", (ev) => { if (checkTarget(ev, targetElement)) {
        calloutRef.invokeMethodAsync("ScrollHandler");
    } ; }, true);
    var resizeId = Handler.addListener(window, "resize", (ev) => { if (checkTarget(ev, targetElement)) {
        calloutRef.invokeMethodAsync("ResizeHandler");
    } ; }, true);
    var focusId = Handler.addListener(document.documentElement, "focus", (ev) => {
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
    var clickId = Handler.addListener(document.documentElement, "click", (ev) => {
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
export function unregisterHandlers(ids) {
    Handler.removeCallout(ids[ids.length - 1]);
    var handlerIds = ids.slice(0, ids.length - 1);
    for (let id of handlerIds) {
        Handler.removeListener(id);
    }
}
class Handler {
    static addCallout(element) {
        this.targetCombinedElements[this.i] = element;
        return this.i++;
    }
    static removeCallout(id) {
        if (id in this.targetCombinedElements)
            delete this.targetCombinedElements[id];
    }
    static addListener(element, event, handler, capture) {
        element.addEventListener(event, handler, capture);
        this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
        return this.i++;
    }
    static removeListener(id) {
        if (id in this.listeners) {
            var h = this.listeners[id];
            h.element.removeEventListener(h.event, h.handler, h.capture);
            delete this.listeners[id];
        }
    }
}
Handler.i = 1;
Handler.listeners = {};
Handler.targetCombinedElements = {};
function clickHandler(ev) {
}
function checkTarget(ev, targetElement) {
    const target = ev.target;
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
function elementContains(parent, child, allowVirtualParents = true) {
    let isContained = false;
    if (parent && child) {
        if (allowVirtualParents) {
            isContained = false;
            while (child) {
                let nextParent = getParent(child);
                // console.log("NextParent: " + nextParent);
                if (nextParent === parent) {
                    isContained = true;
                    break;
                }
                child = nextParent;
            }
        }
        else if (parent.contains) {
            isContained = parent.contains(child);
        }
    }
    return isContained;
}
function getParent(child) {
    return child && (child.parentNode && child.parentNode);
}
export function getWindow(element) {
    return element.ownerDocument.defaultView;
}
export function getWindowRect() {
    var rect = {
        width: window.innerWidth,
        height: window.innerHeight,
        top: 0,
        left: 0
    };
    return rect;
}
;
