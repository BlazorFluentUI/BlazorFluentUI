var BlazorFabricBaseComponent;
(function (BlazorFabricBaseComponent) {
    function measureElement(element) {
        var rect = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        };
        return rect;
    }
    BlazorFabricBaseComponent.measureElement = measureElement;
    ;
    function measureScrollWindow(element) {
        var rect = {
            width: element.scrollWidth,
            height: element.scrollHeight,
            top: element.scrollTop,
            left: element.scrollLeft,
            bottom: element.scrollTop + element.clientHeight,
            right: element.scrollLeft + element.clientWidth,
        };
        return rect;
    }
    BlazorFabricBaseComponent.measureScrollWindow = measureScrollWindow;
    ;
    function measureScrollDimensions(element) {
        var dimensions = {
            scrollHeight: element.scrollHeight,
            scrollWidth: element.scrollWidth,
        };
        return dimensions;
    }
    BlazorFabricBaseComponent.measureScrollDimensions = measureScrollDimensions;
    ;
    function measureElementRect(element) {
        return element.getBoundingClientRect();
    }
    BlazorFabricBaseComponent.measureElementRect = measureElementRect;
    ;
    function getWindow(element) {
        return element.ownerDocument.defaultView;
    }
    BlazorFabricBaseComponent.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    BlazorFabricBaseComponent.getWindowRect = getWindowRect;
    ;
    var eventRegister = {};
    function registerResizeEvent(dotnetRef, functionName) {
        var guid = Guid.newGuid();
        eventRegister[guid] = debounce(function (ev) {
            dotnetRef.invokeMethodAsync(functionName);
        }, 16, { leading: true });
        window.addEventListener("resize", eventRegister[guid]);
        return guid;
    }
    BlazorFabricBaseComponent.registerResizeEvent = registerResizeEvent;
    function deregisterResizeEvent(guid) {
        var func = eventRegister[guid];
        window.removeEventListener("resize", func);
        eventRegister[guid] = null;
    }
    BlazorFabricBaseComponent.deregisterResizeEvent = deregisterResizeEvent;
    var Guid = /** @class */ (function () {
        function Guid() {
        }
        Guid.newGuid = function () {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        };
        return Guid;
    }());
    /* Focus stuff */
    function focusFirstChild(rootElement) {
        return false;
    }
    BlazorFabricBaseComponent.focusFirstChild = focusFirstChild;
    function getNextElement(rootElement) {
    }
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
    function debounce(func, wait, options) {
        var _this = this;
        if (this._isDisposed) {
            var noOpFunction = (function () {
                /** Do nothing */
            });
            noOpFunction.cancel = function () {
                return;
            };
            /* tslint:disable:no-any */
            noOpFunction.flush = (function () { return null; });
            /* tslint:enable:no-any */
            noOpFunction.pending = function () { return false; };
            return noOpFunction;
        }
        var waitMS = wait || 0;
        var leading = false;
        var trailing = true;
        var maxWait = null;
        var lastCallTime = 0;
        var lastExecuteTime = new Date().getTime();
        var lastResult;
        // tslint:disable-next-line:no-any
        var lastArgs;
        var timeoutId = null;
        if (options && typeof options.leading === 'boolean') {
            leading = options.leading;
        }
        if (options && typeof options.trailing === 'boolean') {
            trailing = options.trailing;
        }
        if (options && typeof options.maxWait === 'number' && !isNaN(options.maxWait)) {
            maxWait = options.maxWait;
        }
        var markExecuted = function (time) {
            if (timeoutId) {
                _this.clearTimeout(timeoutId);
                timeoutId = null;
            }
            lastExecuteTime = time;
        };
        var invokeFunction = function (time) {
            markExecuted(time);
            lastResult = func.apply(_this._parent, lastArgs);
        };
        var callback = function (userCall) {
            var now = new Date().getTime();
            var executeImmediately = false;
            if (userCall) {
                if (leading && now - lastCallTime >= waitMS) {
                    executeImmediately = true;
                }
                lastCallTime = now;
            }
            var delta = now - lastCallTime;
            var waitLength = waitMS - delta;
            var maxWaitDelta = now - lastExecuteTime;
            var maxWaitExpired = false;
            if (maxWait !== null) {
                // maxWait only matters when there is a pending callback
                if (maxWaitDelta >= maxWait && timeoutId) {
                    maxWaitExpired = true;
                }
                else {
                    waitLength = Math.min(waitLength, maxWait - maxWaitDelta);
                }
            }
            if (delta >= waitMS || maxWaitExpired || executeImmediately) {
                invokeFunction(now);
            }
            else if ((timeoutId === null || !userCall) && trailing) {
                timeoutId = _this.setTimeout(callback, waitLength);
            }
            return lastResult;
        };
        var pending = function () {
            return !!timeoutId;
        };
        var cancel = function () {
            if (pending()) {
                // Mark the debounced function as having executed
                markExecuted(new Date().getTime());
            }
        };
        var flush = function () {
            if (pending()) {
                invokeFunction(new Date().getTime());
            }
            return lastResult;
        };
        // tslint:disable-next-line:no-any
        var resultFunction = (function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            lastArgs = args;
            return callback(true);
        });
        resultFunction.cancel = cancel;
        resultFunction.flush = flush;
        resultFunction.pending = pending;
        return resultFunction;
    }
})(BlazorFabricBaseComponent || (BlazorFabricBaseComponent = {}));
window['BlazorFabricBaseComponent'] = BlazorFabricBaseComponent || {};
//# sourceMappingURL=baseComponent.js.map