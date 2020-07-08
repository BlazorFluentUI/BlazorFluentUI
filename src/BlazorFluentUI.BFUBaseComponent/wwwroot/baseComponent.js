var BlazorFluentUiBaseComponent;
(function (BlazorFluentUiBaseComponent) {
    const test = 12333;
    const DATA_IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    const DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';
    const DATA_IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    const FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    const FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    const IsFocusVisibleClassName = 'ms-Fabric--isFocusVisible';
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
    const layerElements = {};
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
    const DirectionalKeyCodes = {
        [38 /* up */]: 1,
        [40 /* down */]: 1,
        [37 /* left */]: 1,
        [39 /* right */]: 1,
        [36 /* home */]: 1,
        [35 /* end */]: 1,
        [9 /* tab */]: 1,
        [33 /* pageUp */]: 1,
        [34 /* pageDown */]: 1
    };
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
    const _disableIosBodyScroll = (event) => {
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
        let el = startingElement;
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
            eventElementRegister[guid] = [element, (ev) => {
                    let elementToFocus;
                    const containsExpandCollapseModifier = ev.altKey || ev.metaKey;
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
        eventRegister[guid] = (ev) => {
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
        eventRegister[guid] = debounce((ev) => {
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
    class Guid {
        static newGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }
    }
    function findElementRecursive(element, matchFunction) {
        if (!element || element === document.body) {
            return null;
        }
        return matchFunction(element) ? element : findElementRecursive(getParent(element), matchFunction);
    }
    BlazorFluentUiBaseComponent.findElementRecursive = findElementRecursive;
    function elementContainsAttribute(element, attribute) {
        let elementMatch = findElementRecursive(element, (testElement) => testElement.hasAttribute(attribute));
        return elementMatch && elementMatch.getAttribute(attribute);
    }
    BlazorFluentUiBaseComponent.elementContainsAttribute = elementContainsAttribute;
    /* Focus stuff */
    /* Since elements can be stored in Blazor and we don't want to create more js files, this will hold last focused elements for restoring focus later. */
    var _lastFocus = {};
    function storeLastFocusedElement() {
        let element = document.activeElement;
        let htmlElement = element;
        if (htmlElement) {
            let guid = Guid.newGuid();
            _lastFocus[guid] = htmlElement;
            return guid;
        }
        return null;
    }
    BlazorFluentUiBaseComponent.storeLastFocusedElement = storeLastFocusedElement;
    function restoreLastFocus(guid, restoreFocus = true) {
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
        let child = this.getFirstFocusable(element, element, true);
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
        let element = parent;
        for (const index of path) {
            const nextChild = element.children[Math.min(index, element.children.length - 1)];
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
        let tabIndex = 0;
        let tabIndexAttributeValue = null;
        if (element && element.getAttribute) {
            tabIndexAttributeValue = element.getAttribute('tabIndex');
            if (tabIndexAttributeValue) {
                tabIndex = parseInt(tabIndexAttributeValue, 10);
            }
        }
        let isFocusableAttribute = element.getAttribute ? element.getAttribute(DATA_IS_FOCUSABLE_ATTRIBUTE) : null;
        let isTabIndexSet = tabIndexAttributeValue !== null && tabIndex >= 0;
        const result = !!element &&
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
        const visibilityAttribute = element.getAttribute(DATA_IS_VISIBLE_ATTRIBUTE);
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
    function getParent(child, allowVirtualParents = true) {
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
        let parent;
        if (child && child.dataset && child.dataset.parentLayerId) {
            parent = layerElements[child.dataset.parentLayerId];
        }
        return parent;
    }
    BlazorFluentUiBaseComponent.getVirtualParent = getVirtualParent;
    function elementContains(parent, child, allowVirtualParents = true) {
        let isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    let nextParent = getParent(child);
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
        let isCurrentElementVisible = isElementVisible(currentElement);
        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }
        // Check its children.
        if (!suppressChildTraversal &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))) {
            const childMatch = getNextElement(rootElement, currentElement.firstElementChild, true, true, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
            if (childMatch) {
                return childMatch;
            }
        }
        if (currentElement === rootElement) {
            return null;
        }
        // Check its sibling.
        const siblingMatch = getNextElement(rootElement, currentElement.nextElementSibling, true, true, false, includeElementsInFocusZones, allowFocusRoot, tabbable);
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
        let isCurrentElementVisible = isElementVisible(currentElement);
        // Check its children.
        if (traverseChildren &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))) {
            const childMatch = getPreviousElement(rootElement, currentElement.lastElementChild, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
            if (childMatch) {
                if ((tabbable && isElementTabbable(childMatch, true)) || !tabbable) {
                    return childMatch;
                }
                const childMatchSiblingMatch = getPreviousElement(rootElement, childMatch.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
                if (childMatchSiblingMatch) {
                    return childMatchSiblingMatch;
                }
                let childMatchParent = childMatch.parentElement;
                // At this point if we have not found any potential matches
                // start looking at the rest of the subtree under the currentParent.
                // NOTE: We do not want to recurse here because doing so could
                // cause elements to get skipped.
                while (childMatchParent && childMatchParent !== currentElement) {
                    const childMatchParentMatch = getPreviousElement(rootElement, childMatchParent.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
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
        const siblingMatch = getPreviousElement(rootElement, currentElement.previousElementSibling, true, true, true, includeElementsInFocusZones, allowFocusRoot, tabbable);
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
        const event = createNewEvent('MouseEvents');
        event.initEvent('click', true, true);
        target.dispatchEvent(event);
    }
    BlazorFluentUiBaseComponent.raiseClick = raiseClick;
    function createNewEvent(eventName) {
        let event;
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
        return () => element.removeEventListener(eventName, callback, options);
    }
    BlazorFluentUiBaseComponent.on = on;
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
    function debounce(func, wait, options) {
        if (this._isDisposed) {
            let noOpFunction = (() => {
                /** Do nothing */
            });
            noOpFunction.cancel = () => {
                return;
            };
            /* tslint:disable:no-any */
            noOpFunction.flush = (() => null);
            /* tslint:enable:no-any */
            noOpFunction.pending = () => false;
            return noOpFunction;
        }
        let waitMS = wait || 0;
        let leading = false;
        let trailing = true;
        let maxWait = null;
        let lastCallTime = 0;
        let lastExecuteTime = new Date().getTime();
        let lastResult;
        // tslint:disable-next-line:no-any
        let lastArgs;
        let timeoutId = null;
        if (options && typeof options.leading === 'boolean') {
            leading = options.leading;
        }
        if (options && typeof options.trailing === 'boolean') {
            trailing = options.trailing;
        }
        if (options && typeof options.maxWait === 'number' && !isNaN(options.maxWait)) {
            maxWait = options.maxWait;
        }
        let markExecuted = (time) => {
            if (timeoutId) {
                this.clearTimeout(timeoutId);
                timeoutId = null;
            }
            lastExecuteTime = time;
        };
        let invokeFunction = (time) => {
            markExecuted(time);
            lastResult = func.apply(this._parent, lastArgs);
        };
        let callback = (userCall) => {
            let now = new Date().getTime();
            let executeImmediately = false;
            if (userCall) {
                if (leading && now - lastCallTime >= waitMS) {
                    executeImmediately = true;
                }
                lastCallTime = now;
            }
            let delta = now - lastCallTime;
            let waitLength = waitMS - delta;
            let maxWaitDelta = now - lastExecuteTime;
            let maxWaitExpired = false;
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
                timeoutId = this.setTimeout(callback, waitLength);
            }
            return lastResult;
        };
        let pending = () => {
            return !!timeoutId;
        };
        let cancel = () => {
            if (pending()) {
                // Mark the debounced function as having executed
                markExecuted(new Date().getTime());
            }
        };
        let flush = () => {
            if (pending()) {
                invokeFunction(new Date().getTime());
            }
            return lastResult;
        };
        // tslint:disable-next-line:no-any
        let resultFunction = ((...args) => {
            lastArgs = args;
            return callback(true);
        });
        resultFunction.cancel = cancel;
        resultFunction.flush = flush;
        resultFunction.pending = pending;
        return resultFunction;
    }
    BlazorFluentUiBaseComponent.debounce = debounce;
    const RTL_LOCAL_STORAGE_KEY = 'isRTL';
    let _isRTL;
    function getRTL() {
        if (_isRTL === undefined) {
            // Fabric supports persisting the RTL setting between page refreshes via session storage
            let savedRTL = getItem(RTL_LOCAL_STORAGE_KEY);
            if (savedRTL !== null) {
                _isRTL = savedRTL === '1';
                setRTL(_isRTL);
            }
            let doc = document;
            if (_isRTL === undefined && doc) {
                _isRTL = ((doc.body && doc.body.getAttribute('dir')) || doc.documentElement.getAttribute('dir')) === 'rtl';
                //mergeStylesSetRTL(_isRTL);
            }
        }
        return !!_isRTL;
    }
    BlazorFluentUiBaseComponent.getRTL = getRTL;
    function setRTL(isRTL, persistSetting = false) {
        let doc = document;
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
        let result = null;
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
//# sourceMappingURL=baseComponent.js.map