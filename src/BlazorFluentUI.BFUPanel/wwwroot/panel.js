//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
/// <reference path="../../BlazorFluentUI.BFUFocusTrapZone/wwwroot/focusTrapZone.ts" />
var BlazorFluentUiPanel;
(function (BlazorFluentUiPanel) {
    var Handler = /** @class */ (function () {
        function Handler() {
        }
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
        return Handler;
    }());
    function registerSizeHandler(panel) {
        //var window = targetElement.ownerDocument.defaultView;
        var resizeId = Handler.addListener(window, "resize", function (ev) { panel.invokeMethodAsync("UpdateFooterPositionAsync"); }, false);
        //var blurId = Handler.addListener(targetElement, "blur", (ev: Event) => { ev.preventDefault(); panel.invokeMethodAsync("OnBlur"); }, false);
        return resizeId;
    }
    BlazorFluentUiPanel.registerSizeHandler = registerSizeHandler;
    function registerMouseDownHandler(panelElement, panelDotNet) {
        var mouseDownId = Handler.addListener(document.body, "mousedown", function (ev) {
            //first get whether click is inside panel
            if (!ev.defaultPrevented) {
                var contains = BlazorFluentUiFocusTrapZone.elementContains(panelElement, ev.target);
                //var contains = window["BlazorFluentUiFocusTrapZone"].elementContains(panelElement, ev.target);
                if (!contains) {
                    ev.preventDefault();
                    panelDotNet.invokeMethodAsync("DismissOnOuterClick", contains);
                }
            }
        }, true);
        return mouseDownId;
    }
    BlazorFluentUiPanel.registerMouseDownHandler = registerMouseDownHandler;
    function unregisterHandler(id) {
        Handler.removeListener(id);
    }
    BlazorFluentUiPanel.unregisterHandler = unregisterHandler;
    var DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';
    function makeElementScrollAllower(element) {
        var _previousClientY = 0;
        var _element = null;
        // remember the clientY for future calls of _preventOverscrolling
        var _saveClientY = function (event) {
            if (event.targetTouches.length === 1) {
                _previousClientY = event.targetTouches[0].clientY;
            }
        };
        // prevent the body from scrolling when the user attempts
        // to scroll past the top or bottom of the element
        var _preventOverscrolling = function (event) {
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
            var clientY = event.targetTouches[0].clientY - _previousClientY;
            var scrollableParent = findScrollableParent(event.target);
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
    BlazorFluentUiPanel.makeElementScrollAllower = makeElementScrollAllower;
    function findScrollableParent(startingElement) {
        var el = startingElement;
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
                var computedStyles = getComputedStyle(el);
                var overflowY = computedStyles ? computedStyles.getPropertyValue('overflow-y') : '';
                if (overflowY && (overflowY === 'scroll' || overflowY === 'auto')) {
                    return el;
                }
            }
            el = el.parentElement;
        }
        // Fall back to window scroll.
        if (!el || el === document.body) {
            // tslint:disable-next-line:no-any
            el = window;
        }
        return el;
    }
})(BlazorFluentUiPanel || (BlazorFluentUiPanel = {}));
window['BlazorFluentUiPanel'] = BlazorFluentUiPanel || {};
//# sourceMappingURL=panel.js.map