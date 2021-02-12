//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var FluentUICallout;
(function (FluentUICallout) {
    function registerHandlers(targetElement, calloutRef) {
        var window = targetElement.ownerDocument.defaultView;
        var calloutDivId = Handler.addCallout(targetElement);
        var scrollId = Handler.addListener(window, "scroll", function (ev) { if (checkTarget(ev, targetElement)) {
            calloutRef.invokeMethodAsync("ScrollHandler");
        } ; }, true);
        var resizeId = Handler.addListener(window, "resize", function (ev) { if (checkTarget(ev, targetElement)) {
            calloutRef.invokeMethodAsync("ResizeHandler");
        } ; }, true);
        var focusId = Handler.addListener(document.documentElement, "focus", function (ev) {
            var outsideCallout = true;
            for (var prop in Handler.targetCombinedElements) {
                if (Object.prototype.hasOwnProperty.call(Handler.targetCombinedElements, prop)) {
                    outsideCallout = checkTarget(ev, Handler.targetCombinedElements[prop]);
                    if (outsideCallout == false)
                        break;
                }
            }
            if (outsideCallout)
                calloutRef.invokeMethodAsync("FocusHandler");
        }, true);
        var clickId = Handler.addListener(document.documentElement, "click", function (ev) {
            var outsideCallout = true;
            for (var prop in Handler.targetCombinedElements) {
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
    FluentUICallout.registerHandlers = registerHandlers;
    function unregisterHandlers(ids) {
        Handler.removeCallout(ids[ids.length - 1]);
        var handlerIds = ids.slice(0, ids.length - 1);
        for (var _i = 0, handlerIds_1 = handlerIds; _i < handlerIds_1.length; _i++) {
            var id = handlerIds_1[_i];
            Handler.removeListener(id);
        }
    }
    FluentUICallout.unregisterHandlers = unregisterHandlers;
    var Handler = /** @class */ (function () {
        function Handler() {
        }
        Handler.addCallout = function (element) {
            this.targetCombinedElements[this.i] = element;
            return this.i++;
        };
        Handler.removeCallout = function (id) {
            if (id in this.targetCombinedElements)
                delete this.targetCombinedElements[id];
        };
        Handler.addListener = function (element, event, handler, capture) {
            element.addEventListener(event, handler, capture);
            this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
            return this.i++;
        };
        Handler.removeListener = function (id) {
            if (id in this.listeners) {
                var h = this.listeners[id];
                h.element.removeEventListener(h.event, h.handler, h.capture);
                delete this.listeners[id];
            }
        };
        Handler.i = 1;
        Handler.listeners = {};
        Handler.targetCombinedElements = {};
        return Handler;
    }());
    function clickHandler(ev) {
    }
    function checkTarget(ev, targetElement) {
        var target = ev.target;
        var isEventTargetOutsideCallout = !elementContains(targetElement, target);
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
    function elementContains(parent, child, allowVirtualParents) {
        if (allowVirtualParents === void 0) { allowVirtualParents = true; }
        var isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    var nextParent = getParent(child);
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
    function getWindow(element) {
        return element.ownerDocument.defaultView;
    }
    FluentUICallout.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    FluentUICallout.getWindowRect = getWindowRect;
    ;
})(FluentUICallout || (FluentUICallout = {}));
window['FluentUICallout'] = FluentUICallout || {};
//# sourceMappingURL=callout.js.map