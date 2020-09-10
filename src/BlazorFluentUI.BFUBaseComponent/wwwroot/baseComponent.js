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
    //interface IVirtualElement extends HTMLElement {
    //    _virtual: {
    //        parent?: IVirtualElement;
    //        children: IVirtualElement[];
    //    };
    //}
    //interface IVirtualRelationship {
    //    parent: HTMLElement;
    //    children: HTMLElement[];
    //}
    //Store the element that the layer is started from so we can later match up the layer's children with the original parent.
    var layerElements = {};
    //const virtualRelationships: Map<IVirtualRelationship> = {};
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
            right: element.scrollLeft + element.clientWidth,
        };
        return rect;
    }
    BlazorFluentUiBaseComponent.measureScrollWindow = measureScrollWindow;
    function measureScrollDimensions(element) {
        var dimensions = {
            scrollHeight: element.scrollHeight,
            scrollWidth: element.scrollWidth,
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
        return child && ((allowVirtualParents && getVirtualParent(child)) || (child.parentNode && child.parentNode));
    }
    BlazorFluentUiBaseComponent.getParent = getParent;
    function addOrUpdateVirtualParent(parent) {
        layerElements[parent.dataset.layerId] = parent;
    }
    BlazorFluentUiBaseComponent.addOrUpdateVirtualParent = addOrUpdateVirtualParent;
    //export function setVirtualParent(child: HTMLElement, parent: HTMLElement) {
    //    let virtualChild = <IVirtualElement>child;
    //    let virtualParent = <IVirtualElement>parent;
    //    if (!virtualChild._virtual) {
    //        virtualChild._virtual = {
    //            children: []
    //        };
    //    }
    //    let oldParent = virtualChild._virtual.parent;
    //    if (oldParent && oldParent !== parent) {
    //        // Remove the child from its old parent.
    //        let index = oldParent._virtual.children.indexOf(virtualChild);
    //        if (index > -1) {
    //            oldParent._virtual.children.splice(index, 1);
    //        }
    //    }
    //    virtualChild._virtual.parent = virtualParent || undefined;
    //    if (virtualParent) {
    //        if (!virtualParent._virtual) {
    //            virtualParent._virtual = {
    //                children: []
    //            };
    //        }
    //        virtualParent._virtual.children.push(virtualChild);
    //    }
    //}
    //export function setVirtualParent(child: HTMLElement) {
    //}
    //export function getVirtualParent(child: HTMLElement): HTMLElement | undefined {
    //    let parent: HTMLElement | undefined;
    //    if (child && !!(<IVirtualElement>child)._virtual) {
    //        parent = (child as IVirtualElement)._virtual.parent;
    //    }
    //    return parent;
    //}
    function getVirtualParent(child) {
        var parent;
        if (child && child.dataset && child.dataset.parentLayerId) {
            parent = layerElements[child.dataset.parentLayerId];
        }
        return parent;
    }
    BlazorFluentUiBaseComponent.getVirtualParent = getVirtualParent;
    function elementContains(parent, child, allowVirtualParents) {
        if (allowVirtualParents === void 0) { allowVirtualParents = true; }
        var isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    var nextParent = getParent(child);
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
    var Async = /** @class */ (function () {
        // tslint:disable-next-line:no-any
        function Async(parent, onError) {
            this._timeoutIds = null;
            this._immediateIds = null;
            this._intervalIds = null;
            this._animationFrameIds = null;
            this._isDisposed = false;
            this._parent = parent || null;
            this._onErrorHandler = onError;
            this._noop = function () {
                /* do nothing */
            };
        }
        /**
         * Dispose function, clears all async operations.
         */
        Async.prototype.dispose = function () {
            var id;
            this._isDisposed = true;
            this._parent = null;
            // Clear timeouts.
            if (this._timeoutIds) {
                for (id in this._timeoutIds) {
                    if (this._timeoutIds.hasOwnProperty(id)) {
                        this.clearTimeout(parseInt(id, 10));
                    }
                }
                this._timeoutIds = null;
            }
            // Clear immediates.
            if (this._immediateIds) {
                for (id in this._immediateIds) {
                    if (this._immediateIds.hasOwnProperty(id)) {
                        this.clearImmediate(parseInt(id, 10));
                    }
                }
                this._immediateIds = null;
            }
            // Clear intervals.
            if (this._intervalIds) {
                for (id in this._intervalIds) {
                    if (this._intervalIds.hasOwnProperty(id)) {
                        this.clearInterval(parseInt(id, 10));
                    }
                }
                this._intervalIds = null;
            }
            // Clear animation frames.
            if (this._animationFrameIds) {
                for (id in this._animationFrameIds) {
                    if (this._animationFrameIds.hasOwnProperty(id)) {
                        this.cancelAnimationFrame(parseInt(id, 10));
                    }
                }
                this._animationFrameIds = null;
            }
        };
        /**
         * SetTimeout override, which will auto cancel the timeout during dispose.
         * @param callback - Callback to execute.
         * @param duration - Duration in milliseconds.
         * @returns The setTimeout id.
         */
        Async.prototype.setTimeout = function (callback, duration) {
            var _this = this;
            var timeoutId = 0;
            if (!this._isDisposed) {
                if (!this._timeoutIds) {
                    this._timeoutIds = {};
                }
                /* tslint:disable:ban-native-functions */
                timeoutId = setTimeout(function () {
                    // Time to execute the timeout, enqueue it as a foreground task to be executed.
                    try {
                        // Now delete the record and call the callback.
                        if (_this._timeoutIds) {
                            delete _this._timeoutIds[timeoutId];
                        }
                        callback.apply(_this._parent);
                    }
                    catch (e) {
                        if (_this._onErrorHandler) {
                            _this._onErrorHandler(e);
                        }
                    }
                }, duration);
                /* tslint:enable:ban-native-functions */
                this._timeoutIds[timeoutId] = true;
            }
            return timeoutId;
        };
        /**
         * Clears the timeout.
         * @param id - Id to cancel.
         */
        Async.prototype.clearTimeout = function (id) {
            if (this._timeoutIds && this._timeoutIds[id]) {
                /* tslint:disable:ban-native-functions */
                clearTimeout(id);
                delete this._timeoutIds[id];
                /* tslint:enable:ban-native-functions */
            }
        };
        /**
         * SetImmediate override, which will auto cancel the immediate during dispose.
         * @param callback - Callback to execute.
         * @param targetElement - Optional target element to use for identifying the correct window.
         * @returns The setTimeout id.
         */
        Async.prototype.setImmediate = function (callback, targetElement) {
            var _this = this;
            var immediateId = 0;
            var win = getWindow(targetElement);
            if (!this._isDisposed) {
                if (!this._immediateIds) {
                    this._immediateIds = {};
                }
                /* tslint:disable:ban-native-functions */
                var setImmediateCallback = function () {
                    // Time to execute the timeout, enqueue it as a foreground task to be executed.
                    try {
                        // Now delete the record and call the callback.
                        if (_this._immediateIds) {
                            delete _this._immediateIds[immediateId];
                        }
                        callback.apply(_this._parent);
                    }
                    catch (e) {
                        _this._logError(e);
                    }
                };
                immediateId = win.setTimeout(setImmediateCallback, 0);
                /* tslint:enable:ban-native-functions */
                this._immediateIds[immediateId] = true;
            }
            return immediateId;
        };
        /**
         * Clears the immediate.
         * @param id - Id to cancel.
         * @param targetElement - Optional target element to use for identifying the correct window.
         */
        Async.prototype.clearImmediate = function (id, targetElement) {
            var win = getWindow(targetElement);
            if (this._immediateIds && this._immediateIds[id]) {
                /* tslint:disable:ban-native-functions */
                win.clearTimeout(id);
                delete this._immediateIds[id];
                /* tslint:enable:ban-native-functions */
            }
        };
        /**
         * SetInterval override, which will auto cancel the timeout during dispose.
         * @param callback - Callback to execute.
         * @param duration - Duration in milliseconds.
         * @returns The setTimeout id.
         */
        Async.prototype.setInterval = function (callback, duration) {
            var _this = this;
            var intervalId = 0;
            if (!this._isDisposed) {
                if (!this._intervalIds) {
                    this._intervalIds = {};
                }
                /* tslint:disable:ban-native-functions */
                intervalId = setInterval(function () {
                    // Time to execute the interval callback, enqueue it as a foreground task to be executed.
                    try {
                        callback.apply(_this._parent);
                    }
                    catch (e) {
                        _this._logError(e);
                    }
                }, duration);
                /* tslint:enable:ban-native-functions */
                this._intervalIds[intervalId] = true;
            }
            return intervalId;
        };
        /**
         * Clears the interval.
         * @param id - Id to cancel.
         */
        Async.prototype.clearInterval = function (id) {
            if (this._intervalIds && this._intervalIds[id]) {
                /* tslint:disable:ban-native-functions */
                clearInterval(id);
                delete this._intervalIds[id];
                /* tslint:enable:ban-native-functions */
            }
        };
        /**
         * Creates a function that, when executed, will only call the func function at most once per
         * every wait milliseconds. Provide an options object to indicate that func should be invoked
         * on the leading and/or trailing edge of the wait timeout. Subsequent calls to the throttled
         * function will return the result of the last func call.
         *
         * Note: If leading and trailing options are true func will be called on the trailing edge of
         * the timeout only if the throttled function is invoked more than once during the wait timeout.
         *
         * @param func - The function to throttle.
         * @param wait - The number of milliseconds to throttle executions to. Defaults to 0.
         * @param options - The options object.
         * @returns The new throttled function.
         */
        Async.prototype.throttle = function (func, wait, options) {
            var _this = this;
            if (this._isDisposed) {
                return this._noop;
            }
            var waitMS = wait || 0;
            var leading = true;
            var trailing = true;
            var lastExecuteTime = 0;
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
            var callback = function (userCall) {
                var now = new Date().getTime();
                var delta = now - lastExecuteTime;
                var waitLength = leading ? waitMS - delta : waitMS;
                if (delta >= waitMS && (!userCall || leading)) {
                    lastExecuteTime = now;
                    if (timeoutId) {
                        _this.clearTimeout(timeoutId);
                        timeoutId = null;
                    }
                    lastResult = func.apply(_this._parent, lastArgs);
                }
                else if (timeoutId === null && trailing) {
                    timeoutId = _this.setTimeout(callback, waitLength);
                }
                return lastResult;
            };
            // tslint:disable-next-line:no-any
            var resultFunction = function () {
                var args = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    args[_i] = arguments[_i];
                }
                lastArgs = args;
                return callback(true);
            };
            return resultFunction;
        };
        /**
         * Creates a function that will delay the execution of func until after wait milliseconds have
         * elapsed since the last time it was invoked. Provide an options object to indicate that func
         * should be invoked on the leading and/or trailing edge of the wait timeout. Subsequent calls
         * to the debounced function will return the result of the last func call.
         *
         * Note: If leading and trailing options are true func will be called on the trailing edge of
         * the timeout only if the debounced function is invoked more than once during the wait
         * timeout.
         *
         * @param func - The function to debounce.
         * @param wait - The number of milliseconds to delay.
         * @param options - The options object.
         * @returns The new debounced function.
         */
        Async.prototype.debounce = function (func, wait, options) {
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
        };
        Async.prototype.requestAnimationFrame = function (callback, targetElement) {
            var _this = this;
            var animationFrameId = 0;
            var win = getWindow(targetElement);
            if (!this._isDisposed) {
                if (!this._animationFrameIds) {
                    this._animationFrameIds = {};
                }
                /* tslint:disable:ban-native-functions */
                var animationFrameCallback = function () {
                    try {
                        // Now delete the record and call the callback.
                        if (_this._animationFrameIds) {
                            delete _this._animationFrameIds[animationFrameId];
                        }
                        callback.apply(_this._parent);
                    }
                    catch (e) {
                        _this._logError(e);
                    }
                };
                animationFrameId = win.requestAnimationFrame
                    ? win.requestAnimationFrame(animationFrameCallback)
                    : win.setTimeout(animationFrameCallback, 0);
                /* tslint:enable:ban-native-functions */
                this._animationFrameIds[animationFrameId] = true;
            }
            return animationFrameId;
        };
        Async.prototype.cancelAnimationFrame = function (id, targetElement) {
            var win = getWindow(targetElement);
            if (this._animationFrameIds && this._animationFrameIds[id]) {
                /* tslint:disable:ban-native-functions */
                win.cancelAnimationFrame ? win.cancelAnimationFrame(id) : win.clearTimeout(id);
                /* tslint:enable:ban-native-functions */
                delete this._animationFrameIds[id];
            }
        };
        // tslint:disable-next-line:no-any
        Async.prototype._logError = function (e) {
            if (this._onErrorHandler) {
                this._onErrorHandler(e);
            }
        };
        return Async;
    }());
    BlazorFluentUiBaseComponent.Async = Async;
    function assign(target) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        return filteredAssign.apply(this, [null, target].concat(args));
    }
    BlazorFluentUiBaseComponent.assign = assign;
    function filteredAssign(isAllowed, target) {
        var args = [];
        for (var _i = 2; _i < arguments.length; _i++) {
            args[_i - 2] = arguments[_i];
        }
        target = target || {};
        for (var _a = 0, args_1 = args; _a < args_1.length; _a++) {
            var sourceObject = args_1[_a];
            if (sourceObject) {
                for (var propName in sourceObject) {
                    if (sourceObject.hasOwnProperty(propName) && (!isAllowed || isAllowed(propName))) {
                        target[propName] = sourceObject[propName];
                    }
                }
            }
        }
        return target;
    }
    BlazorFluentUiBaseComponent.filteredAssign = filteredAssign;
    /** An instance of EventGroup allows anything with a handle to it to trigger events on it.
         *  If the target is an HTMLElement, the event will be attached to the element and can be
         *  triggered as usual (like clicking for onClick).
         *  The event can be triggered by calling EventGroup.raise() here. If the target is an
         *  HTMLElement, the event gets raised and is handled by the browser. Otherwise, it gets
         *  handled here in EventGroup, and the handler is called in the context of the parent
         *  (which is passed in in the constructor).
         *
         * @public
         * {@docCategory EventGroup}
         */
    var EventGroup = /** @class */ (function () {
        /** parent: the context in which events attached to non-HTMLElements are called */
        // tslint:disable-next-line:no-any
        function EventGroup(parent) {
            this._id = EventGroup._uniqueId++;
            this._parent = parent;
            this._eventRecords = [];
        }
        /** For IE8, bubbleEvent is ignored here and must be dealt with by the handler.
         *  Events raised here by default have bubbling set to false and cancelable set to true.
         *  This applies also to built-in events being raised manually here on HTMLElements,
         *  which may lead to unexpected behavior if it differs from the defaults.
         *
         */
        EventGroup.raise = function (
        // tslint:disable-next-line:no-any
        target, eventName, 
        // tslint:disable-next-line:no-any
        eventArgs, bubbleEvent) {
            var retVal;
            if (EventGroup._isElement(target)) {
                if (typeof document !== 'undefined' && document.createEvent) {
                    var ev = document.createEvent('HTMLEvents');
                    ev.initEvent(eventName, bubbleEvent || false, true);
                    assign(ev, eventArgs);
                    retVal = target.dispatchEvent(ev);
                    // tslint:disable-next-line:no-any
                }
                else if (typeof document !== 'undefined' && document['createEventObject']) {
                    // IE8
                    // tslint:disable-next-line:no-any
                    var evObj = document['createEventObject'](eventArgs);
                    // cannot set cancelBubble on evObj, fireEvent will overwrite it
                    target.fireEvent('on' + eventName, evObj);
                }
            }
            else {
                while (target && retVal !== false) {
                    var events = target.__events__;
                    var eventRecords = events ? events[eventName] : null;
                    if (eventRecords) {
                        for (var id in eventRecords) {
                            if (eventRecords.hasOwnProperty(id)) {
                                var eventRecordList = eventRecords[id];
                                for (var listIndex = 0; retVal !== false && listIndex < eventRecordList.length; listIndex++) {
                                    var record = eventRecordList[listIndex];
                                    if (record.objectCallback) {
                                        retVal = record.objectCallback.call(record.parent, eventArgs);
                                    }
                                }
                            }
                        }
                    }
                    // If the target has a parent, bubble the event up.
                    target = bubbleEvent ? target.parent : null;
                }
            }
            return retVal;
        };
        // tslint:disable-next-line:no-any
        EventGroup.isObserved = function (target, eventName) {
            var events = target && target.__events__;
            return !!events && !!events[eventName];
        };
        /** Check to see if the target has declared support of the given event. */
        // tslint:disable-next-line:no-any
        EventGroup.isDeclared = function (target, eventName) {
            var declaredEvents = target && target.__declaredEvents;
            return !!declaredEvents && !!declaredEvents[eventName];
        };
        // tslint:disable-next-line:no-any
        EventGroup.stopPropagation = function (event) {
            if (event.stopPropagation) {
                event.stopPropagation();
            }
            else {
                // IE8
                event.cancelBubble = true;
            }
        };
        EventGroup._isElement = function (target) {
            return (!!target && (!!target.addEventListener || (typeof HTMLElement !== 'undefined' && target instanceof HTMLElement)));
        };
        EventGroup.prototype.dispose = function () {
            if (!this._isDisposed) {
                this._isDisposed = true;
                this.off();
                this._parent = null;
            }
        };
        /** On the target, attach a set of events, where the events object is a name to function mapping. */
        // tslint:disable-next-line:no-any
        EventGroup.prototype.onAll = function (target, events, useCapture) {
            for (var eventName in events) {
                if (events.hasOwnProperty(eventName)) {
                    this.on(target, eventName, events[eventName], useCapture);
                }
            }
        };
        /**
         * On the target, attach an event whose handler will be called in the context of the parent
         * of this instance of EventGroup.
         */
        EventGroup.prototype.on = function (target, // tslint:disable-line:no-any
        eventName, callback, // tslint:disable-line:no-any
        options) {
            var _this = this;
            if (eventName.indexOf(',') > -1) {
                var events = eventName.split(/[ ,]+/);
                for (var i = 0; i < events.length; i++) {
                    this.on(target, events[i], callback, options);
                }
            }
            else {
                var parent_1 = this._parent;
                var eventRecord = {
                    target: target,
                    eventName: eventName,
                    parent: parent_1,
                    callback: callback,
                    options: options,
                };
                // Initialize and wire up the record on the target, so that it can call the callback if the event fires.
                var events = (target.__events__ = target.__events__ || {});
                events[eventName] =
                    events[eventName] ||
                        {
                            count: 0,
                        };
                events[eventName][this._id] = events[eventName][this._id] || [];
                events[eventName][this._id].push(eventRecord);
                events[eventName].count++;
                if (EventGroup._isElement(target)) {
                    // tslint:disable-next-line:no-any
                    var processElementEvent = function () {
                        var args = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            args[_i] = arguments[_i];
                        }
                        if (_this._isDisposed) {
                            return;
                        }
                        var result;
                        try {
                            result = callback.apply(parent_1, args);
                            if (result === false && args[0]) {
                                var e = args[0];
                                if (e.preventDefault) {
                                    e.preventDefault();
                                }
                                if (e.stopPropagation) {
                                    e.stopPropagation();
                                }
                                e.cancelBubble = true;
                            }
                        }
                        catch (e) {
                            /* ErrorHelper.log(e); */
                        }
                        return result;
                    };
                    eventRecord.elementCallback = processElementEvent;
                    if (target.addEventListener) {
                        /* tslint:disable:ban-native-functions */
                        target.addEventListener(eventName, processElementEvent, options);
                        /* tslint:enable:ban-native-functions */
                    }
                    else if (target.attachEvent) {
                        // IE8
                        target.attachEvent('on' + eventName, processElementEvent);
                    }
                }
                else {
                    // tslint:disable-next-line:no-any
                    var processObjectEvent = function () {
                        var args = [];
                        for (var _i = 0; _i < arguments.length; _i++) {
                            args[_i] = arguments[_i];
                        }
                        if (_this._isDisposed) {
                            return;
                        }
                        return callback.apply(parent_1, args);
                    };
                    eventRecord.objectCallback = processObjectEvent;
                }
                // Remember the record locally, so that it can be removed.
                this._eventRecords.push(eventRecord);
            }
        };
        EventGroup.prototype.off = function (target, // tslint:disable-line:no-any
        eventName, callback, // tslint:disable-line:no-any
        options) {
            for (var i = 0; i < this._eventRecords.length; i++) {
                var eventRecord = this._eventRecords[i];
                if ((!target || target === eventRecord.target) &&
                    (!eventName || eventName === eventRecord.eventName) &&
                    (!callback || callback === eventRecord.callback) &&
                    (typeof options !== 'boolean' || options === eventRecord.options)) {
                    var events = eventRecord.target.__events__;
                    var targetArrayLookup = events[eventRecord.eventName];
                    var targetArray = targetArrayLookup ? targetArrayLookup[this._id] : null;
                    // We may have already target's entries, so check for null.
                    if (targetArray) {
                        if (targetArray.length === 1 || !callback) {
                            targetArrayLookup.count -= targetArray.length;
                            delete events[eventRecord.eventName][this._id];
                        }
                        else {
                            targetArrayLookup.count--;
                            targetArray.splice(targetArray.indexOf(eventRecord), 1);
                        }
                        if (!targetArrayLookup.count) {
                            delete events[eventRecord.eventName];
                        }
                    }
                    if (eventRecord.elementCallback) {
                        if (eventRecord.target.removeEventListener) {
                            eventRecord.target.removeEventListener(eventRecord.eventName, eventRecord.elementCallback, eventRecord.options);
                        }
                        else if (eventRecord.target.detachEvent) {
                            // IE8
                            eventRecord.target.detachEvent('on' + eventRecord.eventName, eventRecord.elementCallback);
                        }
                    }
                    this._eventRecords.splice(i--, 1);
                }
            }
        };
        /** Trigger the given event in the context of this instance of EventGroup. */
        // tslint:disable-next-line:no-any
        EventGroup.prototype.raise = function (eventName, eventArgs, bubbleEvent) {
            return EventGroup.raise(this._parent, eventName, eventArgs, bubbleEvent);
        };
        /** Declare an event as being supported by this instance of EventGroup. */
        EventGroup.prototype.declare = function (event) {
            var declaredEvents = (this._parent.__declaredEvents = this._parent.__declaredEvents || {});
            if (typeof event === 'string') {
                declaredEvents[event] = true;
            }
            else {
                for (var i = 0; i < event.length; i++) {
                    declaredEvents[event[i]] = true;
                }
            }
        };
        EventGroup._uniqueId = 0;
        return EventGroup;
    }());
    BlazorFluentUiBaseComponent.EventGroup = EventGroup;
})(BlazorFluentUiBaseComponent || (BlazorFluentUiBaseComponent = {}));
//}
window.BlazorFluentUiBaseComponent = BlazorFluentUiBaseComponent;
//window.BlazorFluentUiBaseComponent
//(<any>window)['BlazorFluentUiBaseComponent'] = BlazorFluentUiBaseComponent || {};
//# sourceMappingURL=baseComponent.js.map