//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFabricCallout;
(function (BlazorFabricCallout) {
    function registerHandlers(targetElement, calloutRef) {
        var window = targetElement.ownerDocument.defaultView;
        window.onscroll = function (ev) { return calloutRef.invokeMethodAsync("ScrollHandler"); };
        window.onresize = function (ev) { return calloutRef.invokeMethodAsync("ResizeHandler"); };
    }
    BlazorFabricCallout.registerHandlers = registerHandlers;
    function unregisterHandlers(targetElement, calloutRef) {
        var window = targetElement.ownerDocument.defaultView;
        window.onscroll = null;
        window.onresize = null;
    }
    BlazorFabricCallout.unregisterHandlers = unregisterHandlers;
    function getWindow(element) {
        return element.ownerDocument.defaultView;
    }
    BlazorFabricCallout.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    BlazorFabricCallout.getWindowRect = getWindowRect;
    ;
})(BlazorFabricCallout || (BlazorFabricCallout = {}));
window['BlazorFabricCallout'] = BlazorFabricCallout || {};
//# sourceMappingURL=callout.js.map