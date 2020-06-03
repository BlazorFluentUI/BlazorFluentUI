var BlazorFluentUiBaseComponent;
(function (BlazorFluentUiBaseComponent) {
    var _a;
    var test = 12333;
    var DATA_IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    var DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';
    var DATA_IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    var FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    var FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    var IsFocusVisibleClassName = 'ms-Fabric--isFocusVisible';
    function initializeFocusRects() {
        if (!window.__hasInitializeFocusRects__) {
            window.__hasInitializeFocusRects__ = true;
            window.addEventListener("mousedown", _onFocusRectMouseDown, true);
            window.addEventListener("keydown", _onFocusRectKeyDown, true);
        }
    }
    BlazorFluentUiBaseComponent.initializeFocusRects = initializeFocusRects;
    function _onFocusRectMouseDown(ev) {
        if (window.document.body.classList.contains(IsFocusVisibleClassName)) {
            window.document.body.classList.remove(IsFocusVisibleClassName);
        }
    }
    function _onFocusRectKeyDown(ev) {
        if (isDirectionalKeyCode(ev.which) && !window.document.body.classList.contains(IsFocusVisibleClassName)) {
            window.document.body.classList.add(IsFocusVisibleClassName);
        }
    }
    var DirectionalKeyCodes = (_a = {},
        _a[38 /* up */] = 1,
        _a[40 /* down */] = 1,
        _a[37 /* left */] = 1,
        _a[39 /* right */] = 1,
        _a[36 /* home */] = 1,
        _a[35 /* end */] = 1,
        _a[9 /* tab */] = 1,
        _a[33 /* pageUp */] = 1,
        _a[34 /* pageDown */] = 1,
        _a);
    function isDirectionalKeyCode(which) {
        return !!DirectionalKeyCodes[which];
    }
    // Disable/enable bodyscroll for overlay
    var _bodyScrollDisabledCount = 0;
    function enableBodyScroll() {
        if (_bodyScrollDisabledCount > 0) {
            if (_bodyScrollDisabledCount === 1) {
                document.body.classList.remove("disabledBodyScroll");
                document.body.removeEventListener('touchmove', _disableIosBodyScroll);
            }
            _bodyScrollDisabledCount--;
        }
    }
    BlazorFluentUiBaseComponent.enableBodyScroll = enableBodyScroll;
    function disableBodyScroll() {
        if (!_bodyScrollDisabledCount) {
            document.body.classList.add("disabledBodyScroll");
            document.body.addEventListener('touchmove', _disableIosBodyScroll, { passive: false, capture: false });
        }
        _bodyScrollDisabledCount++;
    }
    BlazorFluentUiBaseComponent.disableBodyScroll = disableBodyScroll;
    var _disableIosBodyScroll = function (event) {
        event.preventDefault();
    };
    // end
    function getClientHeight(element) {
        if (element == null)
            return 0;
        return element.clientHeight;
    }
    BlazorFluentUiBaseComponent.getClientHeight = getClientHeight;
    function getScrollHeight(element) {
        if (element == null)
            return 0;
        return element.scrollHeight;
    }
    BlazorFluentUiBaseComponent.getScrollHeight = getScrollHeight;
    function findScrollableParent(startingElement) {
        var el = startingElement;
        // First do a quick scan for the scrollable attribute.
        while (el && el !== document.body) {
            if (el.getAttribute(DATA_IS_SCROLLABLE_ATTRIBUTE) === 'true') {
                return el;
            }
            el = el.parentElement;
        }
        // If we haven't found it, then use the slower method: compute styles to evaluate if overflow is set.
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
    BlazorFluentUiBaseComponent.findScrollableParent = findScrollableParent;
    function measureElement(element) {
        var rect = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        };
        return rect;
    }
    BlazorFluentUiBaseComponent.measureElement = measureElement;
    function getNaturalBounds(image) {
        if (image && image !== null) {
            var rect = {
                width: image.naturalWidth,
                height: image.naturalHeight,
                left: 0,
                top: 0
            };
            return rect;
        }
        return null;
    }
    BlazorFluentUiBaseComponent.getNaturalBounds = getNaturalBounds;
    function supportsObjectFit() {
        return window !== undefined && window.navigator.msMaxTouchPoints === undefined;
    }
    BlazorFluentUiBaseComponent.supportsObjectFit = supportsObjectFit;
    function hasOverflow(element) {
        return false;
    }
    BlazorFluentUiBaseComponent.hasOverflow = hasOverflow;
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
    BlazorFluentUiBaseComponent.measureScrollWindow = measureScrollWindow;
    function measureScrollDimensions(element) {
        var dimensions = {
            scrollHeight: element.scrollHeight,
            scrollWidth: element.scrollWidth
        };
        return dimensions;
    }
    BlazorFluentUiBaseComponent.measureScrollDimensions = measureScrollDimensions;
    function measureElementRect(element) {
        if (element !== undefined && element !== null) {
            // EdgeHTML's rectangle can't be serialized for some reason.... serializes to 0 everything.   So break it apart into simple JSON.
            var rect = element.getBoundingClientRect();
            return { height: rect.height, width: rect.width, left: rect.left, right: rect.right, top: rect.top, bottom: rect.bottom };
        }
        else
            return { height: 0, width: 0, left: 0, right: 0, top: 0, bottom: 0 };
    }
    BlazorFluentUiBaseComponent.measureElementRect = measureElementRect;
    function getWindow(element) {
        return element.ownerDocument.defaultView;
    }
    BlazorFluentUiBaseComponent.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    BlazorFluentUiBaseComponent.getWindowRect = getWindowRect;
    function getElementId(element) {
        if (element !== undefined) {
            return element.id;
        }
        return null;
    }
    BlazorFluentUiBaseComponent.getElementId = getElementId;
    var eventRegister = {};
    var eventElementRegister = {};
    /* Function for Dropdown, but could apply to focusing on any element after onkeydown outside of list containing is-element-focusable items */
    function registerKeyEventsForList(element) {
        if (element instanceof HTMLElement) {
            var guid = Guid.newGuid();
            eventElementRegister[guid] = [element, function (ev) {
                    var elementToFocus;
                    var containsExpandCollapseModifier = ev.altKey || ev.metaKey;
                    switch (ev.keyCode) {
                        case 38 /* up */:
                            if (containsExpandCollapseModifier) {
                                //should send a close window or something, maybe let Blazor handle it.
                            }
                            else {
                                elementToFocus = getLastFocusable(element, element.lastChild, true);
                            }
                            break;
                        case 40 /* down */:
                            if (!containsExpandCollapseModifier) {
                                elementToFocus = getFirstFocusable(element, element.firstChild, true);
                            }
                            break;
                        default:
                            return;
                    }
                    if (elementToFocus) {
                        elementToFocus.focus();
                    }
                }];
            element.addEventListener("keydown", eventElementRegister[guid][1]);
            return guid;
        }
        else {
            return null;
        }
    }
    BlazorFluentUiBaseComponent.registerKeyEventsForList = registerKeyEventsForList;
    function deregisterKeyEventsForList(guid) {
        var tuple = eventElementRegister[guid];
        if (tuple) {
            var element = tuple[0];
            var func = tuple[1];
            element.removeEventListener("keydown", func);
            eventElementRegister[guid] = null;
        }
    }
    BlazorFluentUiBaseComponent.deregisterKeyEventsForList = deregisterKeyEventsForList;
    function registerWindowKeyDownEvent(dotnetRef, keyCode, functionName) {
        var guid = Guid.newGuid();
        eventRegister[guid] = function (ev) {
            if (ev.code == keyCode) {
                ev.preventDefault();
                ev.stopPropagation();
                dotnetRef.invokeMethodAsync(functionName, ev.code);
            }
        };
        window.addEventListener("keydown", eventRegister[guid]);
        return guid;
    }
    BlazorFluentUiBaseComponent.registerWindowKeyDownEvent = registerWindowKeyDownEvent;
    function deregisterWindowKeyDownEvent(guid) {
        var func = eventRegister[guid];
        window.removeEventListener("keydown", func);
        eventRegister[guid] = null;
    }
    BlazorFluentUiBaseComponent.deregisterWindowKeyDownEvent = deregisterWindowKeyDownEvent;
    function registerResizeEvent(dotnetRef, functionName) {
        var guid = Guid.newGuid();
        eventRegister[guid] = debounce(function (ev) {
            dotnetRef.invokeMethodAsync(functionName, window.innerWidth, innerHeight);
        }, 100, { leading: true });
        window.addEventListener("resize", eventRegister[guid]);
        return guid;
    }
    BlazorFluentUiBaseComponent.registerResizeEvent = registerResizeEvent;
    function deregisterResizeEvent(guid) {
        var func = eventRegister[guid];
        window.removeEventListener("resize", func);
        eventRegister[guid] = null;
    }
    BlazorFluentUiBaseComponent.deregisterResizeEvent = deregisterResizeEvent;
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
    function findElementRecursive(element, matchFunction) {
        if (!element || element === document.body) {
            return null;
        }
        return matchFunction(element) ? element : findElementRecursive(getParent(element), matchFunction);
    }
    BlazorFluentUiBaseComponent.findElementRecursive = findElementRecursive;
    function elementContainsAttribute(element, attribute) {
        var elementMatch = findElementRecursive(element, function (testElement) { return testElement.hasAttribute(attribute); });
        return elementMatch && elementMatch.getAttribute(attribute);
    }
    BlazorFluentUiBaseComponent.elementContainsAttribute = elementContainsAttribute;
    /* Focus stuff */
    /* Since elements can be stored in Blazor and we don't want to create more js files, this will hold last focused elements for restoring focus later. */
    var _lastFocus = {};
    function storeLastFocusedElement() {
        var element = document.activeElement;
        var htmlElement = element;
        if (htmlElement) {
            var guid = Guid.newGuid();
            _lastFocus[guid] = htmlElement;
            return guid;
        }
        return null;
    }
    BlazorFluentUiBaseComponent.storeLastFocusedElement = storeLastFocusedElement;
    function restoreLastFocus(guid, restoreFocus) {
        if (restoreFocus === void 0) { restoreFocus = true; }
        var htmlElement = _lastFocus[guid];
        if (htmlElement != null) {
            if (restoreFocus) {
                htmlElement.focus();
            }
            delete _lastFocus[guid];
        }
    }
    BlazorFluentUiBaseComponent.restoreLastFocus = restoreLastFocus;
    function getActiveElement() {
        return document.activeElement;
    }
    BlazorFluentUiBaseComponent.getActiveElement = getActiveElement;
    function focusElement(element) {
        element.focus();
    }
    BlazorFluentUiBaseComponent.focusElement = focusElement;
    function focusFirstElementChild(element) {
        var child = this.getFirstFocusable(element, element, true);
        if (child) {
            child.focus();
        }
        else {
            element.focus();
        }
    }
    BlazorFluentUiBaseComponent.focusFirstElementChild = focusFirstElementChild;
    function shouldWrapFocus(element, noWrapDataAttribute) {
        return elementContainsAttribute(element, noWrapDataAttribute) === 'true' ? false : true;
    }
    BlazorFluentUiBaseComponent.shouldWrapFocus = shouldWrapFocus;
    function getFocusableByIndexPath(parent, path) {
        var element = parent;
        for (var _i = 0, path_1 = path; _i < path_1.length; _i++) {
            var index = path_1[_i];
            var nextChild = element.children[Math.min(index, element.children.length - 1)];
            if (!nextChild) {
                break;
            }
            element = nextChild;
        }
        element = isElementTabbable(element) && isElementVisible(element) ? element : getNextElement(parent, element, true) || getPreviousElement(parent, element);
        return { element: element, isNull: !element };
    }
    BlazorFluentUiBaseComponent.getFocusableByIndexPath = getFocusableByIndexPath;
    function getFirstFocusable(rootElement, currentElement, includeElementsInFocusZones) {
        return getNextElement(rootElement, currentElement, true /*checkNode*/, false /*suppressParentTraversal*/, false /*suppressChildTraversal*/, includeElementsInFocusZones);
    }
    BlazorFluentUiBaseComponent.getFirstFocusable = getFirstFocusable;
    function getLastFocusable(rootElement, currentElement, includeElementsInFocusZones) {
        return getPreviousElement(rootElement, currentElement, true /*checkNode*/, false /*suppressParentTraversal*/, true /*suppressChildTraversal*/, includeElementsInFocusZones);
    }
    BlazorFluentUiBaseComponent.getLastFocusable = getLastFocusable;
    function isElementTabbable(element, checkTabIndex) {
        // If this element is null or is disabled, it is not considered tabbable.
        if (!element || element.disabled) {
            return false;
        }
        var tabIndex = 0;
        var tabIndexAttributeValue = null;
        if (element && element.getAttribute) {
            tabIndexAttributeValue = element.getAttribute('tabIndex');
            if (tabIndexAttributeValue) {
                tabIndex = parseInt(tabIndexAttributeValue, 10);
            }
        }
        var isFocusableAttribute = element.getAttribute ? element.getAttribute(DATA_IS_FOCUSABLE_ATTRIBUTE) : null;
        var isTabIndexSet = tabIndexAttributeValue !== null && tabIndex >= 0;
        var result = !!element &&
            isFocusableAttribute !== 'false' &&
            (element.tagName === 'A' ||
                element.tagName === 'BUTTON' ||
                element.tagName === 'INPUT' ||
                element.tagName === 'TEXTAREA' ||
                isFocusableAttribute === 'true' ||
                isTabIndexSet);
        return checkTabIndex ? tabIndex !== -1 && result : result;
    }
    BlazorFluentUiBaseComponent.isElementTabbable = isElementTabbable;
    function isElementVisible(element) {
        // If the element is not valid, return false.
        if (!element || !element.getAttribute) {
            return false;
        }
        var visibilityAttribute = element.getAttribute(DATA_IS_VISIBLE_ATTRIBUTE);
        // If the element is explicitly marked with the visibility attribute, return that value as boolean.
        if (visibilityAttribute !== null && visibilityAttribute !== undefined) {
            return visibilityAttribute === 'true';
        }
        // Fallback to other methods of determining actual visibility.
        return (element.offsetHeight !== 0 ||
            element.offsetParent !== null ||
            // tslint:disable-next-line:no-any
            element.isVisible === true); // used as a workaround for testing.
    }
    BlazorFluentUiBaseComponent.isElementVisible = isElementVisible;
    function focusFirstChild(rootElement) {
        return false;
    }
    BlazorFluentUiBaseComponent.focusFirstChild = focusFirstChild;
    function getParent(child, allowVirtualParents) {
        if (allowVirtualParents === void 0) { allowVirtualParents = true; }
        return child && (child.parentNode && child.parentNode);
    }
    BlazorFluentUiBaseComponent.getParent = getParent;
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
    BlazorFluentUiBaseComponent.elementContains = elementContains;
    function getNextElement(rootElement, currentElement, checkNode, suppressParentTraversal, suppressChildTraversal, includeElementsInFocusZones, allowFocusRoot, tabbable) {
        if (!currentElement || (currentElement === rootElement && suppressChildTraversal && !allowFocusRoot)) {
            return null;
        }
        var isCurrentElementVisible = isElementVisible(currentElement);
        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }
        // Check its children.
        if (!suppressChildTraversal &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))) {
            var childMatch = getNextElement(rootElement, currentElement.firstElementChild, true, true, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
            if (childMatch) {
                return childMatch;
            }
        }
        if (currentElement === rootElement) {
            return null;
        }
        // Check its sibling.
        var siblingMatch = getNextElement(rootElement, currentElement.nextElementSibling, true, true, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
        if (siblingMatch) {
            return siblingMatch;
        }
        if (!suppressParentTraversal) {
            return getNextElement(rootElement, currentElement.parentElement, false, false, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
        }
        return null;
    }
    BlazorFluentUiBaseComponent.getNextElement = getNextElement;
    function getPreviousElement(rootElement, currentElement, checkNode, suppressParentTraversal, traverseChildren, includeElementsInFocusZones, allowFocusRoot, tabbable) {
        if (!currentElement || (!allowFocusRoot && currentElement === rootElement)) {
            return null;
        }
        var isCurrentElementVisible = isElementVisible(currentElement);
        // Check its children.
        if (traverseChildren &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))) {
            var childMatch = getPreviousElement(rootElement, currentElement.lastElementChild, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
            if (childMatch) {
                if ((tabbable && isElementTabbable(childMatch, true)) || !tabbable) {
                    return childMatch;
                }
                var childMatchSiblingMatch = getPreviousElement(rootElement, childMatch.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
                if (childMatchSiblingMatch) {
                    return childMatchSiblingMatch;
                }
                var childMatchParent = childMatch.parentElement;
                // At this point if we have not found any potential matches
                // start looking at the rest of the subtree under the currentParent.
                // NOTE: We do not want to recurse here because doing so could
                // cause elements to get skipped.
                while (childMatchParent && childMatchParent !== currentElement) {
                    var childMatchParentMatch = getPreviousElement(rootElement, childMatchParent.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
                    if (childMatchParentMatch) {
                        return childMatchParentMatch;
                    }
                    childMatchParent = childMatchParent.parentElement;
                }
            }
        }
        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }
        // Check its previous sibling.
        var siblingMatch = getPreviousElement(rootElement, currentElement.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
        if (siblingMatch) {
            return siblingMatch;
        }
        // Check its parent.
        if (!suppressParentTraversal) {
            return getPreviousElement(rootElement, currentElement.parentElement, true, false, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
        }
        return null;
    }
    BlazorFluentUiBaseComponent.getPreviousElement = getPreviousElement;
    /** Raises a click event. */
    function raiseClick(target) {
        var event = createNewEvent('MouseEvents');
        event.initEvent('click', true, true);
        target.dispatchEvent(event);
    }
    BlazorFluentUiBaseComponent.raiseClick = raiseClick;
    function createNewEvent(eventName) {
        var event;
        if (typeof Event === 'function') {
            // Chrome, Opera, Firefox
            event = new Event(eventName);
        }
        else {
            // IE
            event = document.createEvent('Event');
            event.initEvent(eventName, true, true);
        }
        return event;
    }
    function isElementFocusZone(element) {
        return !!(element && element.getAttribute && !!element.getAttribute(FOCUSZONE_ID_ATTRIBUTE));
    }
    BlazorFluentUiBaseComponent.isElementFocusZone = isElementFocusZone;
    function isElementFocusSubZone(element) {
        return !!(element && element.getAttribute && element.getAttribute(FOCUSZONE_SUB_ATTRIBUTE) === 'true');
    }
    BlazorFluentUiBaseComponent.isElementFocusSubZone = isElementFocusSubZone;
    function on(element, eventName, callback, options) {
        element.addEventListener(eventName, callback, options);
        return function () { return element.removeEventListener(eventName, callback, options); };
    }
    BlazorFluentUiBaseComponent.on = on;
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
    BlazorFluentUiBaseComponent.debounce = debounce;
    var RTL_LOCAL_STORAGE_KEY = 'isRTL';
    var _isRTL;
    function getRTL() {
        if (_isRTL === undefined) {
            // Fabric supports persisting the RTL setting between page refreshes via session storage
            var savedRTL = getItem(RTL_LOCAL_STORAGE_KEY);
            if (savedRTL !== null) {
                _isRTL = savedRTL === '1';
                setRTL(_isRTL);
            }
            var doc = document;
            if (_isRTL === undefined && doc) {
                _isRTL = ((doc.body && doc.body.getAttribute('dir')) || doc.documentElement.getAttribute('dir')) === 'rtl';
                //mergeStylesSetRTL(_isRTL);
            }
        }
        return !!_isRTL;
    }
    BlazorFluentUiBaseComponent.getRTL = getRTL;
    function setRTL(isRTL, persistSetting) {
        if (persistSetting === void 0) { persistSetting = false; }
        var doc = document;
        if (doc) {
            doc.documentElement.setAttribute('dir', isRTL ? 'rtl' : 'ltr');
        }
        if (persistSetting) {
            setItem(RTL_LOCAL_STORAGE_KEY, isRTL ? '1' : '0');
        }
        _isRTL = isRTL;
        //mergeStylesSetRTL(_isRTL);
    }
    BlazorFluentUiBaseComponent.setRTL = setRTL;
    function getItem(key) {
        var result = null;
        try {
            result = window.sessionStorage.getItem(key);
        }
        catch (e) {
            /* Eat the exception */
        }
        return result;
    }
    BlazorFluentUiBaseComponent.getItem = getItem;
    function setItem(key, data) {
        try {
            window.sessionStorage.setItem(key, data);
        }
        catch (e) {
            /* Eat the exception */
        }
    }
    BlazorFluentUiBaseComponent.setItem = setItem;
})(BlazorFluentUiBaseComponent || (BlazorFluentUiBaseComponent = {}));
//}
window.BlazorFluentUiBaseComponent = BlazorFluentUiBaseComponent;
//window.BlazorFluentUiBaseComponent
//(<any>window)['BlazorFluentUiBaseComponent'] = BlazorFluentUiBaseComponent || {};
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFluentUiCallout;
(function (BlazorFluentUiCallout) {
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
    BlazorFluentUiCallout.registerHandlers = registerHandlers;
    function unregisterHandlers(ids) {
        Handler.removeCallout(ids[ids.length - 1]);
        var handlerIds = ids.slice(0, ids.length - 1);
        for (var _i = 0, handlerIds_1 = handlerIds; _i < handlerIds_1.length; _i++) {
            var id = handlerIds_1[_i];
            Handler.removeListener(id);
        }
    }
    BlazorFluentUiCallout.unregisterHandlers = unregisterHandlers;
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
    BlazorFluentUiCallout.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    BlazorFluentUiCallout.getWindowRect = getWindowRect;
    ;
})(BlazorFluentUiCallout || (BlazorFluentUiCallout = {}));
window['BlazorFluentUiCallout'] = BlazorFluentUiCallout || {};
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFluentUiFocusTrapZone;
(function (BlazorFluentUiFocusTrapZone) {
    var IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    var d = 445;
    var IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    var FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    var FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    var FocusTrapZoneInternal = /** @class */ (function () {
        function FocusTrapZoneInternal(focusTrapZoneProps, dotNetRef) {
            var _this = this;
            this._hasFocus = true;
            this._onRootFocus = function (ev) {
                //if (this.props.onFocus) {
                //    this.props.onFocus(ev);
                //}
                _this._hasFocus = true;
            };
            this._onRootBlur = function (ev) {
                //if (this.props.onBlur) {
                //    this.props.onBlur(ev);
                //}
                var relatedTarget = ev.relatedTarget;
                if (ev.relatedTarget === null) {
                    // In IE11, due to lack of support, event.relatedTarget is always
                    // null making every onBlur call to be "outside" of the ComboBox
                    // even when it's not. Using document.activeElement is another way
                    // for us to be able to get what the relatedTarget without relying
                    // on the event
                    relatedTarget = document.activeElement;
                }
                if (!elementContains(_this._props.rootElement, relatedTarget)) {
                    _this._hasFocus = false;
                }
            };
            this._onFirstBumperFocus = function () {
                _this._onBumperFocus(true);
            };
            this._onLastBumperFocus = function () {
                _this._onBumperFocus(false);
            };
            this._onBumperFocus = function (isFirstBumper) {
                if (_this._props.disabled) {
                    return;
                }
                var currentBumper = (isFirstBumper === _this._hasFocus ? _this._props.lastBumper : _this._props.firstBumper);
                if (_this._props.rootElement) {
                    var nextFocusable = isFirstBumper === _this._hasFocus
                        ? getLastTabbable(_this._props.rootElement, currentBumper, true, false)
                        : getFirstTabbable(_this._props.rootElement, currentBumper, true, false);
                    if (nextFocusable) {
                        if (_this._isBumper(nextFocusable)) {
                            // This can happen when FTZ contains no tabbable elements. focus will take care of finding a focusable element in FTZ.
                            _this.focus();
                        }
                        else {
                            nextFocusable.focus();
                        }
                    }
                }
            };
            this._props = focusTrapZoneProps;
            this._dotNetRef = dotNetRef;
            this._props.rootElement.addEventListener("focus", this._onRootFocus, false);
            this._props.rootElement.addEventListener("blur", this._onRootBlur, false);
            this._props.firstBumper.addEventListener("focus", this._onFirstBumperFocus, false);
            this._props.lastBumper.addEventListener("focus", this._onLastBumperFocus, false);
            this._bringFocusIntoZone();
        }
        FocusTrapZoneInternal.prototype.unRegister = function () {
            this._props.rootElement.removeEventListener("focus", this._onRootFocus, false);
            this._props.rootElement.removeEventListener("blur", this._onRootBlur, false);
            this._props.firstBumper.removeEventListener("focus", this._onFirstBumperFocus, false);
            this._props.lastBumper.removeEventListener("focus", this._onLastBumperFocus, false);
        };
        FocusTrapZoneInternal.prototype.updateProps = function (props) {
            this._prevProps = this._props;
            this._props = props;
            //bumpers and root should be the same...
            if ((!this._prevProps.forceFocusInsideTrap && this._props.forceFocusInsideTrap) || (this._prevProps.disabled && !this._props.disabled)) {
                this._bringFocusIntoZone();
            }
            else if ((this._prevProps.forceFocusInsideTrap && !this._props.forceFocusInsideTrap) || (!this._prevProps.disabled && this._props.disabled)) {
                this._returnFocusToInitiator();
            }
        };
        FocusTrapZoneInternal.prototype.setDisabled = function (disabled) {
            this._props.disabled = disabled;
        };
        FocusTrapZoneInternal.prototype._bringFocusIntoZone = function () {
            var _a = this._props, elementToFocusOnDismiss = _a.elementToFocusOnDismiss, _b = _a.disabled, disabled = _b === void 0 ? false : _b, _c = _a.disableFirstFocus, disableFirstFocus = _c === void 0 ? false : _c, rootElement = _a.rootElement;
            if (disabled) {
                return;
            }
            FocusTrapZoneInternal._focusStack.push(this);
            this._previouslyFocusedElementOutsideTrapZone = elementToFocusOnDismiss && elementToFocusOnDismiss.__internalId != null
                ? elementToFocusOnDismiss
                : document.activeElement;
            if (!disableFirstFocus && !elementContains(rootElement, this._previouslyFocusedElementOutsideTrapZone)) {
                this.focus();
            }
        };
        FocusTrapZoneInternal.prototype._returnFocusToInitiator = function () {
            var _this = this;
            var _a = this._props, ignoreExternalFocusing = _a.ignoreExternalFocusing, rootElement = _a.rootElement;
            FocusTrapZoneInternal._focusStack = FocusTrapZoneInternal._focusStack.filter(function (value) {
                return _this !== value;
            });
            var activeElement = document.activeElement;
            if (!ignoreExternalFocusing &&
                this._previouslyFocusedElementOutsideTrapZone &&
                typeof this._previouslyFocusedElementOutsideTrapZone.focus === 'function' &&
                (elementContains(rootElement, activeElement) || activeElement === document.body)) {
                this._focusAsync(this._previouslyFocusedElementOutsideTrapZone);
            }
        };
        FocusTrapZoneInternal.prototype._focusAsync = function (element) {
            if (!this._isBumper(element)) {
                focusAsync(element);
            }
        };
        FocusTrapZoneInternal.prototype._isBumper = function (element) {
            return element === this._props.firstBumper || element === this._props.lastBumper;
        };
        FocusTrapZoneInternal.prototype.focus = function () {
            var _a = this._props, focusPreviouslyFocusedInnerElement = _a.focusPreviouslyFocusedInnerElement, firstFocusableSelector = _a.firstFocusableSelector, rootElement = _a.rootElement;
            if (focusPreviouslyFocusedInnerElement &&
                this._previouslyFocusedElementInTrapZone &&
                elementContains(rootElement, this._previouslyFocusedElementInTrapZone)) {
                // focus on the last item that had focus in the zone before we left the zone
                this._focusAsync(this._previouslyFocusedElementInTrapZone);
                return;
            }
            var focusSelector = firstFocusableSelector;
            var _firstFocusableChild = null;
            if (rootElement) {
                if (focusSelector) {
                    _firstFocusableChild = rootElement.querySelector('.' + focusSelector);
                }
                // Fall back to first element if query selector did not match any elements.
                if (!_firstFocusableChild) {
                    _firstFocusableChild = getNextElement(rootElement, rootElement.firstChild, false, false, false, true);
                }
            }
            if (_firstFocusableChild) {
                this._focusAsync(_firstFocusableChild);
            }
        };
        FocusTrapZoneInternal._focusStack = [];
        return FocusTrapZoneInternal;
    }());
    var count = 0;
    var focusTrapZones = {};
    function register(props, focusTrapZone) {
        var currentId = count++;
        focusTrapZones[currentId] = new FocusTrapZoneInternal(props, focusTrapZone);
        return currentId;
    }
    BlazorFluentUiFocusTrapZone.register = register;
    function unregister(id) {
        var focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.unRegister();
        }
        delete focusTrapZones[id];
    }
    BlazorFluentUiFocusTrapZone.unregister = unregister;
    function updateProps(id, props) {
        var focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.updateProps(props);
        }
    }
    BlazorFluentUiFocusTrapZone.updateProps = updateProps;
    function focus(id) {
        var focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.focus();
        }
    }
    BlazorFluentUiFocusTrapZone.focus = focus;
    function elementContains(parent, child, allowVirtualParents) {
        if (allowVirtualParents === void 0) { allowVirtualParents = true; }
        var isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    var nextParent = getParent(child);
                    console.log("NextParent: " + nextParent);
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
    BlazorFluentUiFocusTrapZone.elementContains = elementContains;
    function getParent(child, allowVirtualParents) {
        if (allowVirtualParents === void 0) { allowVirtualParents = true; }
        return child && (child.parentNode && child.parentNode);
    }
    BlazorFluentUiFocusTrapZone.getParent = getParent;
    var targetToFocusOnNextRepaint = undefined;
    function focusAsync(element) {
        if (element) {
            // An element was already queued to be focused, so replace that one with the new element
            if (targetToFocusOnNextRepaint) {
                targetToFocusOnNextRepaint = element;
                return;
            }
            targetToFocusOnNextRepaint = element;
            // element.focus() is a no-op if the element is no longer in the DOM, meaning this is always safe
            window.requestAnimationFrame(function () {
                targetToFocusOnNextRepaint && targetToFocusOnNextRepaint.focus();
                // We are done focusing for this frame, so reset the queued focus element
                targetToFocusOnNextRepaint = undefined;
            });
        }
    }
    function getNextElement(rootElement, currentElement, checkNode, suppressParentTraversal, suppressChildTraversal, includeElementsInFocusZones, allowFocusRoot, tabbable) {
        if (!currentElement || (currentElement === rootElement && suppressChildTraversal && !allowFocusRoot)) {
            return null;
        }
        var isCurrentElementVisible = isElementVisible(currentElement);
        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }
        // Check its children.
        if (!suppressChildTraversal &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))) {
            var childMatch = getNextElement(rootElement, currentElement.firstElementChild, true, true, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
            if (childMatch) {
                return childMatch;
            }
        }
        if (currentElement === rootElement) {
            return null;
        }
        // Check its sibling.
        var siblingMatch = getNextElement(rootElement, currentElement.nextElementSibling, true, true, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
        if (siblingMatch) {
            return siblingMatch;
        }
        if (!suppressParentTraversal) {
            return getNextElement(rootElement, currentElement.parentElement, false, false, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
        }
        return null;
    }
    function getPreviousElement(rootElement, currentElement, checkNode, suppressParentTraversal, traverseChildren, includeElementsInFocusZones, allowFocusRoot, tabbable) {
        if (!currentElement || (!allowFocusRoot && currentElement === rootElement)) {
            return null;
        }
        var isCurrentElementVisible = isElementVisible(currentElement);
        // Check its children.
        if (traverseChildren &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))) {
            var childMatch = getPreviousElement(rootElement, currentElement.lastElementChild, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
            if (childMatch) {
                if ((tabbable && isElementTabbable(childMatch, true)) || !tabbable) {
                    return childMatch;
                }
                var childMatchSiblingMatch = getPreviousElement(rootElement, childMatch.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
                if (childMatchSiblingMatch) {
                    return childMatchSiblingMatch;
                }
                var childMatchParent = childMatch.parentElement;
                // At this point if we have not found any potential matches
                // start looking at the rest of the subtree under the currentParent.
                // NOTE: We do not want to recurse here because doing so could
                // cause elements to get skipped.
                while (childMatchParent && childMatchParent !== currentElement) {
                    var childMatchParentMatch = getPreviousElement(rootElement, childMatchParent.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
                    if (childMatchParentMatch) {
                        return childMatchParentMatch;
                    }
                    childMatchParent = childMatchParent.parentElement;
                }
            }
        }
        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }
        // Check its previous sibling.
        var siblingMatch = getPreviousElement(rootElement, currentElement.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
        if (siblingMatch) {
            return siblingMatch;
        }
        // Check its parent.
        if (!suppressParentTraversal) {
            return getPreviousElement(rootElement, currentElement.parentElement, true, false, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
        }
        return null;
    }
    function isElementVisible(element) {
        // If the element is not valid, return false.
        if (!element || !element.getAttribute) {
            return false;
        }
        var visibilityAttribute = element.getAttribute(IS_VISIBLE_ATTRIBUTE);
        // If the element is explicitly marked with the visibility attribute, return that value as boolean.
        if (visibilityAttribute !== null && visibilityAttribute !== undefined) {
            return visibilityAttribute === 'true';
        }
        // Fallback to other methods of determining actual visibility.
        return (element.offsetHeight !== 0 ||
            element.offsetParent !== null ||
            // tslint:disable-next-line:no-any
            element.isVisible === true); // used as a workaround for testing.
    }
    function isElementTabbable(element, checkTabIndex) {
        // If this element is null or is disabled, it is not considered tabbable.
        if (!element || element.disabled) {
            return false;
        }
        var tabIndex = 0;
        var tabIndexAttributeValue = null;
        if (element && element.getAttribute) {
            tabIndexAttributeValue = element.getAttribute('tabIndex');
            if (tabIndexAttributeValue) {
                tabIndex = parseInt(tabIndexAttributeValue, 10);
            }
        }
        var isFocusableAttribute = element.getAttribute ? element.getAttribute(IS_FOCUSABLE_ATTRIBUTE) : null;
        var isTabIndexSet = tabIndexAttributeValue !== null && tabIndex >= 0;
        var result = !!element &&
            isFocusableAttribute !== 'false' &&
            (element.tagName === 'A' ||
                element.tagName === 'BUTTON' ||
                element.tagName === 'INPUT' ||
                element.tagName === 'TEXTAREA' ||
                isFocusableAttribute === 'true' ||
                isTabIndexSet);
        return checkTabIndex ? tabIndex !== -1 && result : result;
    }
    function isElementFocusZone(element) {
        return !!(element && element.getAttribute && !!element.getAttribute(FOCUSZONE_ID_ATTRIBUTE));
    }
    function isElementFocusSubZone(element) {
        return !!(element && element.getAttribute && element.getAttribute(FOCUSZONE_SUB_ATTRIBUTE) === 'true');
    }
    function getFirstTabbable(rootElement, currentElement, includeElementsInFocusZones, checkNode) {
        if (checkNode === void 0) { checkNode = true; }
        return getNextElement(rootElement, currentElement, checkNode, false /*suppressParentTraversal*/, false /*suppressChildTraversal*/, includeElementsInFocusZones, false /*allowFocusRoot*/, true /*tabbable*/);
    }
    function getLastTabbable(rootElement, currentElement, includeElementsInFocusZones, checkNode) {
        if (checkNode === void 0) { checkNode = true; }
        return getPreviousElement(rootElement, currentElement, checkNode, false /*suppressParentTraversal*/, true /*traverseChildren*/, includeElementsInFocusZones, false /*allowFocusRoot*/, true /*tabbable*/);
    }
    //class Handler {
    //    static i: number = 1;
    //    static listeners: Map<EventParams> = {};
    //    static addListener(element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): number {
    //        element.addEventListener(event, handler, capture);
    //        this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
    //        return this.i++;
    //    }
    //    static removeListener(id: number): void {
    //        if (id in this.listeners) {
    //            var h = this.listeners[id];
    //            h.element.removeEventListener(h.event, h.handler, h.capture);
    //            delete this.listeners[id];
    //        }
    //    }
    //}
})(BlazorFluentUiFocusTrapZone || (BlazorFluentUiFocusTrapZone = {}));
window['BlazorFluentUiFocusTrapZone'] = BlazorFluentUiFocusTrapZone || {};
/// <reference path="../../BlazorFluentUI.BFUBaseComponent/wwwroot/baseComponent.ts" />
var BlazorFluentUiFocusZone;
(function (BlazorFluentUiFocusZone) {
    var FocusZoneDirection;
    (function (FocusZoneDirection) {
        /** Only react to up/down arrows. */
        FocusZoneDirection[FocusZoneDirection["vertical"] = 0] = "vertical";
        /** Only react to left/right arrows. */
        FocusZoneDirection[FocusZoneDirection["horizontal"] = 1] = "horizontal";
        /** React to all arrows. */
        FocusZoneDirection[FocusZoneDirection["bidirectional"] = 2] = "bidirectional";
        /**
         * React to all arrows. Navigate next item in DOM on right/down arrow keys and previous - left/up arrow keys.
         * Right and Left arrow keys are swapped in RTL mode.
         */
        FocusZoneDirection[FocusZoneDirection["domOrder"] = 3] = "domOrder";
    })(FocusZoneDirection || (FocusZoneDirection = {}));
    var FocusZoneTabbableElements;
    (function (FocusZoneTabbableElements) {
        /** Tabbing is not allowed */
        FocusZoneTabbableElements[FocusZoneTabbableElements["none"] = 0] = "none";
        /** All tabbing action is allowed */
        FocusZoneTabbableElements[FocusZoneTabbableElements["all"] = 1] = "all";
        /** Tabbing is allowed only on input elements */
        FocusZoneTabbableElements[FocusZoneTabbableElements["inputOnly"] = 2] = "inputOnly";
    })(FocusZoneTabbableElements || (FocusZoneTabbableElements = {}));
    var IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    var IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    var FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    var FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    var IS_ENTER_DISABLED_ATTRIBUTE = 'data-disable-click-on-enter';
    var TABINDEX = 'tabindex';
    var NO_VERTICAL_WRAP = 'data-no-vertical-wrap';
    var NO_HORIZONTAL_WRAP = 'data-no-horizontal-wrap';
    var LARGE_DISTANCE_FROM_CENTER = 999999999;
    var LARGE_NEGATIVE_DISTANCE_FROM_CENTER = -999999999;
    var ALLOWED_INPUT_TYPES = ['text', 'number', 'password', 'email', 'tel', 'url', 'search'];
    var ALLOW_VIRTUAL_ELEMENTS = false; // this is not used in Blazor... concept for React only
    var count = 0;
    var allInstances = {};
    var outerZones = new Set();
    var _disposeGlobalKeyDownListener;
    function register(props, focusZone) {
        var currentId = count++;
        allInstances[currentId] = new FocusZoneInternal(props, focusZone);
        return currentId;
    }
    BlazorFluentUiFocusZone.register = register;
    function unregister(id) {
        var focusZone = allInstances[id];
        if (focusZone) {
            focusZone.unRegister();
        }
        delete allInstances[id];
    }
    BlazorFluentUiFocusZone.unregister = unregister;
    function updateFocusZone(id, props) {
        var focusZone = allInstances[id];
        if (focusZone) {
            focusZone.updateFocusZone(props);
        }
    }
    BlazorFluentUiFocusZone.updateFocusZone = updateFocusZone;
    var FocusZoneInternal = /** @class */ (function () {
        function FocusZoneInternal(focusZoneProps, dotNetRef) {
            var _this = this;
            this._disposables = [];
            this._onBlur = function () {
                _this._setParkedFocus(false);
            };
            this._onKeyDown = function (ev) {
                if (_this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                var _a = _this._focusZoneProps, direction = _a.direction, disabled = _a.disabled, innerZoneKeystrokeTriggers = _a.innerZoneKeystrokeTriggers;
                if (disabled) {
                    return;
                }
                //if (this.props.onKeyDown) {
                //    this.props.onKeyDown(ev);
                //}
                // If the default has been prevented, do not process keyboard events.
                if (ev.defaultPrevented) {
                    return;
                }
                if (document.activeElement === _this._root && _this._isInnerZone) {
                    // If this element has focus, it is being controlled by a parent.
                    // Ignore the keystroke.
                    return;
                }
                if (innerZoneKeystrokeTriggers && (innerZoneKeystrokeTriggers.indexOf(ev.keyCode) != -1) && _this._isImmediateDescendantOfZone(ev.target)) {
                    // Try to focus
                    var innerZone = _this._getFirstInnerZone();
                    if (innerZone) {
                        if (!innerZone.focus(true)) {
                            return;
                        }
                    }
                    else if (window.BlazorFluentUiBaseComponent.isElementFocusSubZone(ev.target)) {
                        if (!_this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(ev.target, ev.target.firstChild, true))) {
                            return;
                        }
                    }
                    else {
                        return;
                    }
                }
                else if (ev.altKey) {
                    return;
                }
                else {
                    switch (ev.which) {
                        case 32 /* space */:
                            if (_this._tryInvokeClickForFocusable(ev.target)) {
                                break;
                            }
                            return;
                        case 37 /* left */:
                            if (direction !== FocusZoneDirection.vertical && _this._moveFocusLeft()) {
                                break;
                            }
                            return;
                        case 39 /* right */:
                            if (direction !== FocusZoneDirection.vertical && _this._moveFocusRight()) {
                                break;
                            }
                            return;
                        case 38 /* up */:
                            if (direction !== FocusZoneDirection.horizontal && _this._moveFocusUp()) {
                                break;
                            }
                            return;
                        case 40 /* down */:
                            if (direction !== FocusZoneDirection.horizontal && _this._moveFocusDown()) {
                                break;
                            }
                            return;
                        case 9 /* tab */:
                            if (_this._focusZoneProps.handleTabKey === FocusZoneTabbableElements.all ||
                                (_this._focusZoneProps.handleTabKey === FocusZoneTabbableElements.inputOnly && _this._isElementInput(ev.target))) {
                                var focusChanged = false;
                                _this._processingTabKey = true;
                                if (direction === FocusZoneDirection.vertical ||
                                    !_this._shouldWrapFocus(_this._activeElement, NO_HORIZONTAL_WRAP)) {
                                    focusChanged = ev.shiftKey ? _this._moveFocusUp() : _this._moveFocusDown();
                                }
                                else if (direction === FocusZoneDirection.horizontal || direction === FocusZoneDirection.bidirectional) {
                                    var tabWithDirection = window.BlazorFluentUiBaseComponent.getRTL() ? !ev.shiftKey : ev.shiftKey;
                                    focusChanged = tabWithDirection ? _this._moveFocusLeft() : _this._moveFocusRight();
                                }
                                _this._processingTabKey = false;
                                if (focusChanged) {
                                    break;
                                }
                            }
                            return;
                        case 36 /* home */:
                            if (_this._isElementInput(ev.target) && !_this._shouldInputLoseFocus(ev.target, false)) {
                                return;
                            }
                            var firstChild = _this._root && _this._root.firstChild;
                            if (_this._root && firstChild && _this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(_this._root, firstChild, true))) {
                                break;
                            }
                            return;
                        case 35 /* end */:
                            if (_this._isElementInput(ev.target) && !_this._shouldInputLoseFocus(ev.target, true)) {
                                return;
                            }
                            var lastChild = _this._root && _this._root.lastChild;
                            if (_this._root && _this.focusElement(window.BlazorFluentUiBaseComponent.getPreviousElement(_this._root, lastChild, true, true, true))) {
                                break;
                            }
                            return;
                        case 13 /* enter */:
                            if (_this._tryInvokeClickForFocusable(ev.target)) {
                                break;
                            }
                            return;
                        default:
                            return;
                    }
                }
                ev.preventDefault();
                ev.stopPropagation();
            };
            this._onFocus = function (ev) {
                if (_this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                var doNotAllowFocusEventToPropagate = _this._focusZoneProps.doNotAllowFocusEventToPropagate;
                var isImmediateDescendant = _this._isImmediateDescendantOfZone(ev.target);
                var newActiveElement;
                _this._dotNetRef.invokeMethodAsync("JSOnFocusNotification");
                if (isImmediateDescendant) {
                    newActiveElement = ev.target;
                }
                else {
                    var parentElement = ev.target;
                    while (parentElement && parentElement !== _this._root) {
                        if (window.BlazorFluentUiBaseComponent.isElementTabbable(parentElement) && _this._isImmediateDescendantOfZone(parentElement)) {
                            newActiveElement = parentElement;
                            break;
                        }
                        parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                    }
                }
                var initialElementFocused = !_this._activeElement;
                // If the new active element is a child of this zone and received focus,
                // update alignment an immediate descendant
                if (newActiveElement && newActiveElement !== _this._activeElement) {
                    if (isImmediateDescendant || initialElementFocused) {
                        _this._setFocusAlignment(newActiveElement, true, true);
                    }
                    _this._activeElement = newActiveElement;
                    if (initialElementFocused) {
                        _this._updateTabIndexes();
                    }
                }
                _this._dotNetRef.invokeMethodAsync("JSOnActiveElementChanged");
                if (doNotAllowFocusEventToPropagate) {
                    ev.stopPropagation();
                }
            };
            this._onKeyDownCapture = function (ev) {
                if (ev.which === 9 /* tab */) {
                    outerZones.forEach(function (zone) { return zone._updateTabIndexes(); });
                }
            };
            this._onMouseDown = function (ev) {
                if (_this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                var disabled = _this._focusZoneProps.disabled;
                if (disabled) {
                    return;
                }
                var target = ev.target;
                var path = [];
                while (target && target !== _this._root) {
                    path.push(target);
                    target = window.BlazorFluentUiBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
                }
                while (path.length) {
                    target = path.pop();
                    if (target && window.BlazorFluentUiBaseComponent.isElementTabbable(target)) {
                        _this._setActiveElement(target, true);
                    }
                    if (window.BlazorFluentUiBaseComponent.isElementFocusZone(target)) {
                        // Stop here since the focus zone will take care of its own children.
                        break;
                    }
                }
            };
            this._root = focusZoneProps.root;
            this._focusZoneProps = focusZoneProps;
            this._dotNetRef = dotNetRef;
            this._focusAlignment = {
                x: 0,
                y: 0
            };
            this._root.addEventListener("keydown", this._onKeyDown, false);
            this._root.addEventListener("focusin", this._onFocus, false);
            this._root.addEventListener("mousedown", this._onMouseDown, false);
            this.initialized();
        }
        FocusZoneInternal.prototype.updateFocusZone = function (props) {
            this._focusZoneProps = props;
            allInstances[props.id] = this;
            if (this._root) {
                var windowElement = window.BlazorFluentUiBaseComponent.getWindow(this._root);
                var parentElement = window.BlazorFluentUiBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
                while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                    if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                        this._isInnerZone = true;
                        break;
                    }
                    parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                }
                if (!this._isInnerZone) {
                    outerZones.add(this);
                }
                if (windowElement && outerZones.size === 1) {
                    _disposeGlobalKeyDownListener = window.BlazorFluentUiBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
                }
                this._disposables.push(window.BlazorFluentUiBaseComponent.on(this._root, 'blur', this._onBlur, true));
                // Assign initial tab indexes so that we can set initial focus as appropriate.
                this._updateTabIndexes();
                // using a hack to detect whether the passed in HTMLElement is valid (came from a legitimate .NET ElementReference)
                if ((this._focusZoneProps.defaultActiveElement).__internalId !== null) {
                    if (this._activeElement != this._focusZoneProps.defaultActiveElement) {
                        this._activeElement = this._focusZoneProps.defaultActiveElement;
                        this.focus();
                    }
                }
            }
        };
        FocusZoneInternal.prototype.initialized = function () {
            var windowElement = window.BlazorFluentUiBaseComponent.getWindow(this._root);
            var parentElement = window.BlazorFluentUiBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                    this._isInnerZone = true;
                    break;
                }
                parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            if (!this._isInnerZone) {
                outerZones.add(this);
            }
            if (windowElement && outerZones.size === 1) {
                _disposeGlobalKeyDownListener = window.BlazorFluentUiBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
            }
            this._disposables.push(window.BlazorFluentUiBaseComponent.on(this._root, 'blur', this._onBlur, true));
            // Assign initial tab indexes so that we can set initial focus as appropriate.
            this._updateTabIndexes();
            // using a hack to detect whether the passed in HTMLElement is valid (came from a legitimate .NET ElementReference)
            if ((this._focusZoneProps.defaultActiveElement).__internalId !== null) {
                this._activeElement = this._focusZoneProps.defaultActiveElement;
                this.focus();
            }
        };
        /**
       * When focus is in the zone at render time but then all focusable elements are removed,
       * we "park" focus temporarily on the root. Once we update with focusable children, we restore
       * focus to the closest path from previous. If the user tabs away from the parked container,
       * we restore focusability to the pre-parked state.
       */
        FocusZoneInternal.prototype._setParkedFocus = function (isParked) {
            if (this._root && this._isParked !== isParked) {
                this._isParked = isParked;
                if (isParked) {
                    if (!this._focusZoneProps.allowFocusRoot) {
                        this._parkedTabIndex = this._root.getAttribute('tabindex');
                        this._root.setAttribute('tabindex', '-1');
                    }
                    this._root.focus();
                }
                else {
                    if (!this._focusZoneProps.allowFocusRoot) {
                        if (this._parkedTabIndex) {
                            this._root.setAttribute('tabindex', this._parkedTabIndex);
                            this._parkedTabIndex = undefined;
                        }
                        else {
                            this._root.removeAttribute('tabindex');
                        }
                    }
                }
            }
        };
        FocusZoneInternal.prototype.unRegister = function () {
            if (!this._isInnerZone) {
                outerZones["delete"](this);
            }
            this._disposables.forEach(function (d) { return d(); });
            if (outerZones.size === 0 && _disposeGlobalKeyDownListener) {
                _disposeGlobalKeyDownListener();
            }
            this._root.removeEventListener("keydown", this._onKeyDown, false);
            this._root.removeEventListener("focus", this._onFocus, false);
            this._root.removeEventListener("mousedown", this._onMouseDown, false);
        };
        FocusZoneInternal.prototype.focus = function (forceIntoFirstElement) {
            if (forceIntoFirstElement === void 0) { forceIntoFirstElement = false; }
            if (this._root) {
                if (!forceIntoFirstElement && this._root.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' && this._isInnerZone) {
                    var ownerZoneElement = this._getOwnerZone(this._root);
                    if (ownerZoneElement !== this._root) {
                        var ownerZone = allInstances[ownerZoneElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
                        return !!ownerZone && ownerZone.focusElement(this._root);
                    }
                    return false;
                }
                else if (!forceIntoFirstElement &&
                    this._activeElement &&
                    window.BlazorFluentUiBaseComponent.elementContains(this._root, this._activeElement) &&
                    window.BlazorFluentUiBaseComponent.isElementTabbable(this._activeElement)) {
                    this._activeElement.focus();
                    return true;
                }
                else {
                    var firstChild = this._root.firstChild;
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(this._root, firstChild, true));
                }
            }
            return false;
        };
        FocusZoneInternal.prototype.focusElement = function (element) {
            var onBeforeFocusExists = this._focusZoneProps.onBeforeFocusExists;
            if (onBeforeFocusExists && !this._dotNetRef.invokeMethodAsync("JSOnBeforeFocus")) {
                return false;
            }
            if (element) {
                // when we Set focus to a specific child, we should recalculate the alignment depend on its position
                this._setActiveElement(element);
                if (this._activeElement) {
                    this._activeElement.focus();
                }
                return true;
            }
            return false;
        };
        FocusZoneInternal.prototype._updateTabIndexes = function (element) {
            if (!element && this._root) {
                this._defaultFocusElement = null;
                element = this._root;
                if (this._activeElement && !window.BlazorFluentUiBaseComponent.elementContains(element, this._activeElement)) {
                    this._activeElement = null;
                }
            }
            // If active element changes state to disabled, set it to null.
            // Otherwise, we lose keyboard accessibility to other elements in focus zone.
            if (this._activeElement && !window.BlazorFluentUiBaseComponent.isElementTabbable(this._activeElement)) {
                this._activeElement = null;
            }
            var childNodes = element && element.children;
            for (var childIndex = 0; childNodes && childIndex < childNodes.length; childIndex++) {
                var child = childNodes[childIndex];
                if (!window.BlazorFluentUiBaseComponent.isElementFocusZone(child)) {
                    // If the item is explicitly set to not be focusable then TABINDEX needs to be set to -1.
                    if (child.getAttribute && child.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'false') {
                        child.setAttribute(TABINDEX, '-1');
                    }
                    if (window.BlazorFluentUiBaseComponent.isElementTabbable(child)) {
                        if (this._focusZoneProps.disabled) {
                            child.setAttribute(TABINDEX, '-1');
                        }
                        else if (!this._isInnerZone && ((!this._activeElement && !this._defaultFocusElement) || this._activeElement === child)) {
                            this._defaultFocusElement = child;
                            if (child.getAttribute(TABINDEX) !== '0') {
                                child.setAttribute(TABINDEX, '0');
                            }
                        }
                        else if (child.getAttribute(TABINDEX) !== '-1') {
                            child.setAttribute(TABINDEX, '-1');
                        }
                    }
                    else if (child.tagName === 'svg' && child.getAttribute('focusable') !== 'false') {
                        // Disgusting IE hack. Sad face.
                        child.setAttribute('focusable', 'false');
                    }
                }
                else if (child.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true') {
                    if (!this._isInnerZone && ((!this._activeElement && !this._defaultFocusElement) || this._activeElement === child)) {
                        this._defaultFocusElement = child;
                        if (child.getAttribute(TABINDEX) !== '0') {
                            child.setAttribute(TABINDEX, '0');
                        }
                    }
                    else if (child.getAttribute(TABINDEX) !== '-1') {
                        child.setAttribute(TABINDEX, '-1');
                    }
                }
                this._updateTabIndexes(child);
            }
        };
        FocusZoneInternal.prototype._getOwnerZone = function (element) {
            var parentElement = window.BlazorFluentUiBaseComponent.getParent(element, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== this._root && parentElement !== document.body) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                    return parentElement;
                }
                parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            return parentElement;
        };
        FocusZoneInternal.prototype._setActiveElement = function (element, forceAlignment) {
            var previousActiveElement = this._activeElement;
            this._activeElement = element;
            if (previousActiveElement) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(previousActiveElement)) {
                    this._updateTabIndexes(previousActiveElement);
                }
                previousActiveElement.tabIndex = -1;
            }
            if (this._activeElement) {
                if (!this._focusAlignment || forceAlignment) {
                    this._setFocusAlignment(element, true, true);
                }
                this._activeElement.tabIndex = 0;
            }
        };
        FocusZoneInternal.prototype._setFocusAlignment = function (element, isHorizontal, isVertical) {
            if (this._focusZoneProps.direction === FocusZoneDirection.bidirectional && (!this._focusAlignment || isHorizontal || isVertical)) {
                var rect = element.getBoundingClientRect();
                var left = rect.left + rect.width / 2;
                var top_1 = rect.top + rect.height / 2;
                if (!this._focusAlignment) {
                    this._focusAlignment = {
                        x: left,
                        y: top_1
                    };
                }
                if (isHorizontal) {
                    this._focusAlignment.x = left;
                }
                if (isVertical) {
                    this._focusAlignment.y = top_1;
                }
            }
        };
        FocusZoneInternal.prototype._isImmediateDescendantOfZone = function (element) {
            return this._getOwnerZone(element) === this._root;
        };
        /**
   * Traverse to find first child zone.
   */
        FocusZoneInternal.prototype._getFirstInnerZone = function (rootElement) {
            rootElement = rootElement || this._activeElement || this._root;
            if (!rootElement) {
                return null;
            }
            if (window.BlazorFluentUiBaseComponent.isElementFocusZone(rootElement)) {
                return allInstances[rootElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
            }
            var child = rootElement.firstElementChild;
            while (child) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(child)) {
                    return allInstances[child.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
                }
                var match = this._getFirstInnerZone(child);
                if (match) {
                    return match;
                }
                child = child.nextElementSibling;
            }
            return null;
        };
        /**
       * Walk up the dom try to find a focusable element.
       */
        FocusZoneInternal.prototype._tryInvokeClickForFocusable = function (target) {
            if (target === this._root) {
                return false;
            }
            do {
                if (target.tagName === 'BUTTON' || target.tagName === 'A' || target.tagName === 'INPUT' || target.tagName === 'TEXTAREA') {
                    return false;
                }
                if (this._isImmediateDescendantOfZone(target) &&
                    target.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' &&
                    target.getAttribute(IS_ENTER_DISABLED_ATTRIBUTE) !== 'true') {
                    window.BlazorFluentUiBaseComponent.raiseClick(target);
                    return true;
                }
                target = window.BlazorFluentUiBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
            } while (target !== this._root);
            return false;
        };
        /**
        * Returns true if the element is a descendant of the FocusZone through a React portal.
        */
        FocusZoneInternal.prototype._portalContainsElement = function (element) {
            // This might break our control when used inside a Layer...
            return false;
            //return element && !!this._root && BlazorFluentUiBaseComponent portalContainsElement(element, this._root.current);
        };
        FocusZoneInternal.prototype._isElementInput = function (element) {
            if (element && element.tagName && (element.tagName.toLowerCase() === 'input' || element.tagName.toLowerCase() === 'textarea')) {
                return true;
            }
            return false;
        };
        FocusZoneInternal.prototype._shouldInputLoseFocus = function (element, isForward) {
            // If a tab was used, we want to focus on the next element.
            if (!this._processingTabKey && element && element.type && ALLOWED_INPUT_TYPES.indexOf(element.type.toLowerCase()) > -1) {
                var selectionStart = element.selectionStart;
                var selectionEnd = element.selectionEnd;
                var isRangeSelected = selectionStart !== selectionEnd;
                var inputValue = element.value;
                // We shouldn't lose focus in the following cases:
                // 1. There is range selected.
                // 2. When selection start is larger than 0 and it is backward.
                // 3. when selection start is not the end of length and it is forward.
                // 4. We press any of the arrow keys when our handleTabKey isn't none or undefined (only losing focus if we hit tab)
                // and if shouldInputLoseFocusOnArrowKey is defined, if scenario prefers to not loose the focus which is determined by calling the
                // callback shouldInputLoseFocusOnArrowKey
                if (isRangeSelected ||
                    (selectionStart > 0 && !isForward) ||
                    (selectionStart !== inputValue.length && isForward) ||
                    (!!this._focusZoneProps.handleTabKey && !(this._focusZoneProps.shouldInputLoseFocusOnArrowKeyExists && this._dotNetRef.invokeMethodAsync("JSShouldInputLoseFocusOnArrowKey")))) {
                    return false;
                }
            }
            return true;
        };
        FocusZoneInternal.prototype._shouldWrapFocus = function (element, noWrapDataAttribute) {
            return !!this._focusZoneProps.checkForNoWrap ? window.BlazorFluentUiBaseComponent.shouldWrapFocus(element, noWrapDataAttribute) : true;
        };
        FocusZoneInternal.prototype._moveFocus = function (isForward, getDistanceFromCenter, ev, useDefaultWrap) {
            if (useDefaultWrap === void 0) { useDefaultWrap = true; }
            var element = this._activeElement;
            var candidateDistance = -1;
            var candidateElement = undefined;
            var changedFocus = false;
            var isBidirectional = this._focusZoneProps.direction === FocusZoneDirection.bidirectional;
            if (!element || !this._root) {
                return false;
            }
            if (this._isElementInput(element)) {
                if (!this._shouldInputLoseFocus(element, isForward)) {
                    return false;
                }
            }
            var activeRect = isBidirectional ? element.getBoundingClientRect() : null;
            do {
                element = (isForward ? window.BlazorFluentUiBaseComponent.getNextElement(this._root, element) : window.BlazorFluentUiBaseComponent.getPreviousElement(this._root, element));
                if (isBidirectional) {
                    if (element) {
                        var targetRect = element.getBoundingClientRect();
                        var elementDistance = getDistanceFromCenter(activeRect, targetRect);
                        if (elementDistance === -1 && candidateDistance === -1) {
                            candidateElement = element;
                            break;
                        }
                        if (elementDistance > -1 && (candidateDistance === -1 || elementDistance < candidateDistance)) {
                            candidateDistance = elementDistance;
                            candidateElement = element;
                        }
                        if (candidateDistance >= 0 && elementDistance < 0) {
                            break;
                        }
                    }
                }
                else {
                    candidateElement = element;
                    break;
                }
            } while (element);
            // Focus the closest candidate
            if (candidateElement && candidateElement !== this._activeElement) {
                changedFocus = true;
                this.focusElement(candidateElement);
            }
            else if (this._focusZoneProps.isCircularNavigation && useDefaultWrap) {
                if (isForward) {
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(this._root, this._root.firstElementChild, true));
                }
                else {
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getPreviousElement(this._root, this._root.lastElementChild, true, true, true));
                }
            }
            return changedFocus;
        };
        FocusZoneInternal.prototype._moveFocusDown = function () {
            var _this = this;
            var targetTop = -1;
            var leftAlignment = this._focusAlignment.x;
            if (this._moveFocus(true, function (activeRect, targetRect) {
                var distance = -1;
                // ClientRect values can be floats that differ by very small fractions of a decimal.
                // If the difference between top and bottom are within a pixel then we should treat
                // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
                // but without Math.Floor they will be handled incorrectly.
                var targetRectTop = Math.floor(targetRect.top);
                var activeRectBottom = Math.floor(activeRect.bottom);
                if (targetRectTop < activeRectBottom) {
                    if (!_this._shouldWrapFocus(_this._activeElement, NO_VERTICAL_WRAP)) {
                        return LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                    }
                    return LARGE_DISTANCE_FROM_CENTER;
                }
                if ((targetTop === -1 && targetRectTop >= activeRectBottom) || targetRectTop === targetTop) {
                    targetTop = targetRectTop;
                    if (leftAlignment >= targetRect.left && leftAlignment <= targetRect.left + targetRect.width) {
                        distance = 0;
                    }
                    else {
                        distance = Math.abs(targetRect.left + targetRect.width / 2 - leftAlignment);
                    }
                }
                return distance;
            })) {
                this._setFocusAlignment(this._activeElement, false, true);
                return true;
            }
            return false;
        };
        FocusZoneInternal.prototype._moveFocusUp = function () {
            var _this = this;
            var targetTop = -1;
            var leftAlignment = this._focusAlignment.x;
            if (this._moveFocus(false, function (activeRect, targetRect) {
                var distance = -1;
                // ClientRect values can be floats that differ by very small fractions of a decimal.
                // If the difference between top and bottom are within a pixel then we should treat
                // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
                // but without Math.Floor they will be handled incorrectly.
                var targetRectBottom = Math.floor(targetRect.bottom);
                var targetRectTop = Math.floor(targetRect.top);
                var activeRectTop = Math.floor(activeRect.top);
                if (targetRectBottom > activeRectTop) {
                    if (!_this._shouldWrapFocus(_this._activeElement, NO_VERTICAL_WRAP)) {
                        return LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                    }
                    return LARGE_DISTANCE_FROM_CENTER;
                }
                if ((targetTop === -1 && targetRectBottom <= activeRectTop) || targetRectTop === targetTop) {
                    targetTop = targetRectTop;
                    if (leftAlignment >= targetRect.left && leftAlignment <= targetRect.left + targetRect.width) {
                        distance = 0;
                    }
                    else {
                        distance = Math.abs(targetRect.left + targetRect.width / 2 - leftAlignment);
                    }
                }
                return distance;
            })) {
                this._setFocusAlignment(this._activeElement, false, true);
                return true;
            }
            return false;
        };
        FocusZoneInternal.prototype._moveFocusLeft = function () {
            var _this = this;
            var shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
            if (this._moveFocus(window.BlazorFluentUiBaseComponent.getRTL(), function (activeRect, targetRect) {
                var distance = -1;
                var topBottomComparison;
                if (window.BlazorFluentUiBaseComponent.getRTL()) {
                    // When in RTL, this comparison should be the same as the one in _moveFocusRight for LTR.
                    // Going left at a leftmost rectangle will go down a line instead of up a line like in LTR.
                    // This is important, because we want to be comparing the top of the target rect
                    // with the bottom of the active rect.
                    topBottomComparison = parseFloat(targetRect.top.toFixed(3)) < parseFloat(activeRect.bottom.toFixed(3));
                }
                else {
                    topBottomComparison = parseFloat(targetRect.bottom.toFixed(3)) > parseFloat(activeRect.top.toFixed(3));
                }
                if (topBottomComparison && targetRect.right <= activeRect.right && _this._focusZoneProps.direction !== FocusZoneDirection.vertical) {
                    distance = activeRect.right - targetRect.right;
                }
                else {
                    if (!shouldWrap) {
                        distance = LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                    }
                }
                return distance;
            }, undefined /*ev*/, shouldWrap)) {
                this._setFocusAlignment(this._activeElement, true, false);
                return true;
            }
            return false;
        };
        FocusZoneInternal.prototype._moveFocusRight = function () {
            var _this = this;
            var shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
            if (this._moveFocus(!window.BlazorFluentUiBaseComponent.getRTL(), function (activeRect, targetRect) {
                var distance = -1;
                var topBottomComparison;
                if (window.BlazorFluentUiBaseComponent.getRTL()) {
                    // When in RTL, this comparison should be the same as the one in _moveFocusLeft for LTR.
                    // Going right at a rightmost rectangle will go up a line instead of down a line like in LTR.
                    // This is important, because we want to be comparing the bottom of the target rect
                    // with the top of the active rect.
                    topBottomComparison = parseFloat(targetRect.bottom.toFixed(3)) > parseFloat(activeRect.top.toFixed(3));
                }
                else {
                    topBottomComparison = parseFloat(targetRect.top.toFixed(3)) < parseFloat(activeRect.bottom.toFixed(3));
                }
                if (topBottomComparison && targetRect.left >= activeRect.left && _this._focusZoneProps.direction !== FocusZoneDirection.vertical) {
                    distance = targetRect.left - activeRect.left;
                }
                else if (!shouldWrap) {
                    distance = LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                }
                return distance;
            }, undefined /*ev*/, shouldWrap)) {
                this._setFocusAlignment(this._activeElement, true, false);
                return true;
            }
            return false;
        };
        return FocusZoneInternal;
    }());
})(BlazorFluentUiFocusZone || (BlazorFluentUiFocusZone = {}));
//interface Window {
//    BlazorFluentUiFocusZone: typeof BlazorFluentUiFocusZone
//}
//window.BlazorFluentUiFocusZone = BlazorFluentUiFocusZone;
window['BlazorFluentUiFocusZone'] = BlazorFluentUiFocusZone || {};
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
})(BlazorFluentUiList || (BlazorFluentUiList = {}));
window['BlazorFluentUiList'] = BlazorFluentUiList || {};
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
var BlazorFluentUiTextField;
(function (BlazorFluentUiTextField) {
    function getScrollHeight(element) {
        var paddingTop = window.getComputedStyle(element, null).getPropertyValue('padding-top');
        var paddingBottom = window.getComputedStyle(element, null).getPropertyValue('padding-bottom');
        var yPadding = parseInt(paddingTop) + parseInt(paddingBottom);
        return element.scrollHeight - yPadding;
    }
    BlazorFluentUiTextField.getScrollHeight = getScrollHeight;
    ;
})(BlazorFluentUiTextField || (BlazorFluentUiTextField = {}));
window['BlazorFluentUiTextField'] = BlazorFluentUiTextField || {};
