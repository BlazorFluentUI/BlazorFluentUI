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
    var _lastId = 0;
    var cachedLists = new Map();
    class BFUList {
        constructor(component, scrollElement, spacerBefore, spacerAfter) {
            this.cachedSizes = new Map();
            this.averageHeight = 40;
            this.id = _lastId++;
            this.component = component;
            this.scrollElement = scrollElement;
            this.spacerBefore = spacerBefore;
            this.spacerAfter = spacerAfter;
            const rootMargin = 50;
            this.intersectionObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting && entry.target.offsetHeight > 0) {
                        window.requestIdleCallback(() => {
                            const spacerType = entry.target === this.spacerBefore ? 'before' : 'after';
                            const visibleRect = {
                                top: entry.intersectionRect.top - entry.boundingClientRect.top,
                                left: entry.intersectionRect.left - entry.boundingClientRect.left,
                                width: entry.intersectionRect.width,
                                height: entry.intersectionRect.height,
                                bottom: this.scrollElement.scrollHeight,
                                right: this.scrollElement.scrollWidth
                            };
                            this.component.invokeMethodAsync('OnSpacerVisible', spacerType, visibleRect, this.scrollElement.offsetHeight + 2 * rootMargin, this.spacerBefore.offsetHeight, this.spacerAfter.offsetHeight);
                        });
                    }
                });
            }, {
                root: scrollElement, rootMargin: `${rootMargin}px`
            });
            this.intersectionObserver.observe(this.spacerBefore);
            this.intersectionObserver.observe(this.spacerAfter);
            // After each render, refresh the info about intersections
            this.mutationObserver = new MutationObserver(mutations => {
                this.intersectionObserver.unobserve(this.spacerBefore);
                this.intersectionObserver.unobserve(this.spacerAfter);
                this.intersectionObserver.observe(this.spacerBefore);
                this.intersectionObserver.observe(this.spacerAfter);
            });
            this.mutationObserver.observe(spacerBefore, { attributes: true });
        }
        disconnect() {
            this.mutationObserver.disconnect();
            this.intersectionObserver.unobserve(this.spacerBefore);
            this.intersectionObserver.unobserve(this.spacerAfter);
            this.intersectionObserver.disconnect();
        }
        getInitialAverageHeight() {
            let calculate = false;
            let averageHeight = 0;
            for (let i = 0; i < this.scrollElement.children.length; i++) {
                let item = this.scrollElement.children.item(i);
                let index = item.getAttribute("data-index");
                if (index != null && !this.cachedSizes.has(index) && this.cachedSizes.get(index) != item.clientHeight) {
                    this.cachedSizes.set(index, item.clientHeight);
                    calculate = true;
                }
            }
            if (calculate) {
                averageHeight = [...this.cachedSizes.values()].reduce((p, c, i, a) => p + c) / this.cachedSizes.size;
            }
            return averageHeight;
        }
    }
    function getInitialAverageHeight(id) {
        let list = cachedLists.get(id);
        if (list == null) {
            return 0;
        }
        else {
            return list.getInitialAverageHeight();
        }
    }
    BlazorFluentUiList.getInitialAverageHeight = getInitialAverageHeight;
    function initialize(component, scrollElement, spacerBefore, spacerAfter, reset = false) {
        //if (reset) {
        //    scrollElement.scrollTo(0, 0);
        //}
        let list = new BFUList(component, scrollElement, spacerBefore, spacerAfter);
        cachedLists.set(list.id, list);
        const visibleRect = {
            top: 0,
            left: list.id,
            width: scrollElement.clientWidth,
            height: scrollElement.clientHeight,
            bottom: scrollElement.scrollHeight,
            right: scrollElement.scrollWidth
        };
        return visibleRect;
    }
    BlazorFluentUiList.initialize = initialize;
    function removeList(id) {
        let list = cachedLists.get(id);
        list.disconnect();
        cachedLists.delete(id);
    }
    BlazorFluentUiList.removeList = removeList;
    function getViewport(scrollElement) {
        const visibleRect = {
            top: 0,
            left: 0,
            width: scrollElement.clientWidth,
            height: scrollElement.clientHeight,
            bottom: scrollElement.scrollHeight,
            right: scrollElement.scrollWidth
        };
        return visibleRect;
    }
    BlazorFluentUiList.getViewport = getViewport;
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