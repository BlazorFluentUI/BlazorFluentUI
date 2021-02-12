//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
/// <reference path="../../FluentUI.FocusTrapZone/wwwroot/focusTrapZone.ts" />
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIPanel;
(function (FluentUIPanel) {
    class Handler {
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
    function registerSizeHandler(panel) {
        //var window = targetElement.ownerDocument.defaultView;
        var resizeId = Handler.addListener(window, "resize", (ev) => { panel.invokeMethodAsync("UpdateFooterPositionAsync"); }, false);
        //var blurId = Handler.addListener(targetElement, "blur", (ev: Event) => { ev.preventDefault(); panel.invokeMethodAsync("OnBlur"); }, false);
        return resizeId;
    }
    FluentUIPanel.registerSizeHandler = registerSizeHandler;
    function registerMouseDownHandler(panelElement, panelDotNet) {
        var mouseDownId = Handler.addListener(document.body, "mousedown", (ev) => {
            //first get whether click is inside panel
            if (!ev.defaultPrevented) {
                var contains = FluentUIBaseComponent.elementContains(panelElement, ev.target);
                //var contains = window["FluentUIFocusTrapZone"].elementContains(panelElement, ev.target);
                if (!contains) {
                    ev.preventDefault();
                    panelDotNet.invokeMethodAsync("DismissOnOuterClick", contains);
                }
            }
        }, true);
        return mouseDownId;
    }
    FluentUIPanel.registerMouseDownHandler = registerMouseDownHandler;
    function unregisterHandler(id) {
        Handler.removeListener(id);
    }
    FluentUIPanel.unregisterHandler = unregisterHandler;
    const DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';
    function makeElementScrollAllower(element) {
        let _previousClientY = 0;
        let _element = null;
        // remember the clientY for future calls of _preventOverscrolling
        const _saveClientY = (event) => {
            if (event.targetTouches.length === 1) {
                _previousClientY = event.targetTouches[0].clientY;
            }
        };
        // prevent the body from scrolling when the user attempts
        // to scroll past the top or bottom of the element
        const _preventOverscrolling = (event) => {
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
            const scrollableParent = findScrollableParent(event.target);
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
    FluentUIPanel.makeElementScrollAllower = makeElementScrollAllower;
    function findScrollableParent(startingElement) {
        let el = startingElement;
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
            el = window;
        }
        return el;
    }
})(FluentUIPanel || (FluentUIPanel = {}));
window['FluentUIPanel'] = FluentUIPanel || {};
//# sourceMappingURL=panel.js.map