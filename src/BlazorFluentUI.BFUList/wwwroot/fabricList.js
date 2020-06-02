//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFluentUiList;
(function (BlazorFluentUiList) {
    function measureElement(element) {
        var rect = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        };
        return rect;
    }
    BlazorFluentUiList.measureElement = measureElement;
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
    BlazorFluentUiList.measureScrollWindow = measureScrollWindow;
    ;
    function measureElementRect(element) {
        var rect = element.getBoundingClientRect();
        var elementMeasurements = { height: rect.height, width: rect.width, left: rect.left, right: rect.right, top: rect.top, bottom: rect.bottom, cheight: element.clientHeight, cwidth: element.clientWidth, test: "Random!" };
        return elementMeasurements;
    }
    BlazorFluentUiList.measureElementRect = measureElementRect;
    ;
    function initialize(component, scrollElement, contentElement) {
        let _ticking;
        let _cachedSizes = new Map();
        let _averageHeight = 40;
        let _lastDate;
        scrollElement.addEventListener('scroll', e => {
            for (var i = 0; i < contentElement.children.length; i++) {
                let item = contentElement.children.item(i);
                let itemIndex = item.getAttribute("data-index");
                _cachedSizes.set(itemIndex, item.clientHeight);
            }
            if (_cachedSizes.size > 0) {
                _averageHeight = [..._cachedSizes.values()].reduce((p, c, i, a) => p + c) / _cachedSizes.size;
            }
            const lastKnownValues = {
                containerRect: scrollElement.getBoundingClientRect(),
                scrollRect: measureScrollWindow(scrollElement),
                contentRect: readClientRectWithoutTransform(contentElement),
                averageHeight: _averageHeight
            };
            if (!_ticking) {
                window.requestIdleCallback(() => {
                    component.invokeMethodAsync('OnScroll', lastKnownValues);
                    _ticking = false;
                });
                _ticking = true;
            }
        });
        return {
            containerRect: scrollElement.getBoundingClientRect(),
            scrollRect: measureScrollWindow(scrollElement),
            contentRect: readClientRectWithoutTransform(contentElement),
            averageHeight: _averageHeight
        };
    }
    BlazorFluentUiList.initialize = initialize;
    function readClientRectWithoutTransform(elem) {
        const rect = elem.getBoundingClientRect();
        const translateY = parseFloat(elem.getAttribute('data-translateY'));
        return {
            top: rect.top - translateY, bottom: rect.bottom - translateY, left: rect.left, right: rect.right, height: rect.height, width: rect.width, x: 0, y: 0, toJSON: null
        };
    }
    function _expandRect(rect, pagesBefore, pagesAfter) {
        const top = rect.top - pagesBefore * rect.height;
        const height = rect.height + (pagesBefore + pagesAfter) * rect.height;
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
})(BlazorFluentUiList || (BlazorFluentUiList = {}));
window['BlazorFluentUiList'] = BlazorFluentUiList || {};
//# sourceMappingURL=fabricList.js.map