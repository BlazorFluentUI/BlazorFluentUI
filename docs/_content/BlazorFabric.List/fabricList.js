//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFabricList;
(function (BlazorFabricList) {
    function measureElement(element) {
        var rect = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        };
        return rect;
    }
    BlazorFabricList.measureElement = measureElement;
    ;
    function measureScrollWindow(element) {
        var rect = {
            width: element.scrollWidth,
            height: element.scrollHeight,
            top: element.scrollTop,
            left: element.scrollLeft,
            bottom: element.scrollTop + element.clientHeight,
            right: element.scrollLeft + element.clientWidth
        };
        return rect;
    }
    BlazorFabricList.measureScrollWindow = measureScrollWindow;
    ;
    function measureElementRect(element) {
        var rect = element.getBoundingClientRect();
        var elementMeasurements = { height: rect.height, width: rect.width, left: rect.left, right: rect.right, top: rect.top, bottom: rect.bottom, cheight: element.clientHeight, cwidth: element.clientWidth, test: "Random!" };
        return elementMeasurements;
    }
    BlazorFabricList.measureElementRect = measureElementRect;
    ;
    var FabricList = /** @class */ (function () {
        function FabricList(scrollElement, rootElement, surfaceElement) {
            this._scrollElement = scrollElement;
            this._root = rootElement;
            this._surface = surfaceElement;
            //rootElement.addEventListener('focus', this._onFocus, false);
            scrollElement.addEventListener('scroll', this._onScroll, false);
            //scrollElement.addEventListener('scroll', this._onAsyncScroll, false);
        }
        FabricList.prototype._onScroll = function (ev) {
        };
        return FabricList;
    }());
    function _expandRect(rect, pagesBefore, pagesAfter) {
        var top = rect.top - pagesBefore * rect.height;
        var height = rect.height + (pagesBefore + pagesAfter) * rect.height;
        return {
            top: top,
            bottom: top + height,
            height: height,
            left: rect.left,
            right: rect.right,
            width: rect.width
        };
    }
    function _isContainedWithin(innerRect, outerRect) {
        return (innerRect.top >= outerRect.top &&
            innerRect.left >= outerRect.left &&
            innerRect.bottom <= outerRect.bottom &&
            innerRect.right <= outerRect.right);
    }
    function _mergeRect(targetRect, newRect) {
        targetRect.top = newRect.top < targetRect.top || targetRect.top === -1 ? newRect.top : targetRect.top;
        targetRect.left = newRect.left < targetRect.left || targetRect.left === -1 ? newRect.left : targetRect.left;
        targetRect.bottom = newRect.bottom > targetRect.bottom || targetRect.bottom === -1 ? newRect.bottom : targetRect.bottom;
        targetRect.right = newRect.right > targetRect.right || targetRect.right === -1 ? newRect.right : targetRect.right;
        targetRect.width = targetRect.right - targetRect.left + 1;
        targetRect.height = targetRect.bottom - targetRect.top + 1;
        return targetRect;
    }
})(BlazorFabricList || (BlazorFabricList = {}));
window['BlazorFabricList'] = BlazorFabricList || {};
//# sourceMappingURL=fabricList.js.map