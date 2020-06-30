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
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFluentUiCallout;
(function (BlazorFluentUiCallout) {
    function registerHandlers(targetElement, calloutRef) {
        var window = targetElement.ownerDocument.defaultView;
        var calloutDivId = Handler.addCallout(targetElement);
        var scrollId = Handler.addListener(window, "scroll", (ev) => { if (checkTarget(ev, targetElement)) {
            calloutRef.invokeMethodAsync("ScrollHandler");
        } ; }, true);
        var resizeId = Handler.addListener(window, "resize", (ev) => { if (checkTarget(ev, targetElement)) {
            calloutRef.invokeMethodAsync("ResizeHandler");
        } ; }, true);
        var focusId = Handler.addListener(document.documentElement, "focus", (ev) => {
            var outsideCallout = true;
            for (let prop in Handler.targetCombinedElements) {
                if (Object.prototype.hasOwnProperty.call(Handler.targetCombinedElements, prop)) {
                    outsideCallout = checkTarget(ev, Handler.targetCombinedElements[prop]);
                    if (outsideCallout == false)
                        break;
                }
            }
            if (outsideCallout)
                calloutRef.invokeMethodAsync("FocusHandler");
        }, true);
        var clickId = Handler.addListener(document.documentElement, "click", (ev) => {
            var outsideCallout = true;
            for (let prop in Handler.targetCombinedElements) {
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
        for (let id of handlerIds) {
            Handler.removeListener(id);
        }
    }
    BlazorFluentUiCallout.unregisterHandlers = unregisterHandlers;
    class Handler {
        static addCallout(element) {
            this.targetCombinedElements[this.i] = element;
            return this.i++;
        }
        static removeCallout(id) {
            if (id in this.targetCombinedElements)
                delete this.targetCombinedElements[id];
        }
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
    Handler.targetCombinedElements = {};
    function clickHandler(ev) {
    }
    function checkTarget(ev, targetElement) {
        const target = ev.target;
        const isEventTargetOutsideCallout = !elementContains(targetElement, target);
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
    function elementContains(parent, child, allowVirtualParents = true) {
        let isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    let nextParent = getParent(child);
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
/// <reference path="../../BlazorFluentUI.BFUBaseComponent/wwwroot/baseComponent.ts" />
var BlazorFluentUiFocusTrapZone;
(function (BlazorFluentUiFocusTrapZone) {
    const IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    const d = 445;
    const IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    const FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    const FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    class FocusTrapZoneInternal {
        constructor(focusTrapZoneProps, dotNetRef) {
            this._hasFocus = true;
            this._onRootFocus = (ev) => {
                //if (this.props.onFocus) {
                //    this.props.onFocus(ev);
                //}
                this._hasFocus = true;
            };
            this._onRootBlur = (ev) => {
                //if (this.props.onBlur) {
                //    this.props.onBlur(ev);
                //}
                let relatedTarget = ev.relatedTarget;
                if (ev.relatedTarget === null) {
                    // In IE11, due to lack of support, event.relatedTarget is always
                    // null making every onBlur call to be "outside" of the ComboBox
                    // even when it's not. Using document.activeElement is another way
                    // for us to be able to get what the relatedTarget without relying
                    // on the event
                    relatedTarget = document.activeElement;
                }
                if (!BlazorFluentUiBaseComponent.elementContains(this._props.rootElement, relatedTarget)) {
                    this._hasFocus = false;
                }
            };
            this._onFirstBumperFocus = () => {
                this._onBumperFocus(true);
            };
            this._onLastBumperFocus = () => {
                this._onBumperFocus(false);
            };
            this._onBumperFocus = (isFirstBumper) => {
                if (this._props.disabled) {
                    return;
                }
                const currentBumper = (isFirstBumper === this._hasFocus ? this._props.lastBumper : this._props.firstBumper);
                if (this._props.rootElement) {
                    const nextFocusable = isFirstBumper === this._hasFocus
                        ? getLastTabbable(this._props.rootElement, currentBumper, true, false)
                        : getFirstTabbable(this._props.rootElement, currentBumper, true, false);
                    if (nextFocusable) {
                        if (this._isBumper(nextFocusable)) {
                            // This can happen when FTZ contains no tabbable elements. focus will take care of finding a focusable element in FTZ.
                            this.focus();
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
        unRegister() {
            this._props.rootElement.removeEventListener("focus", this._onRootFocus, false);
            this._props.rootElement.removeEventListener("blur", this._onRootBlur, false);
            this._props.firstBumper.removeEventListener("focus", this._onFirstBumperFocus, false);
            this._props.lastBumper.removeEventListener("focus", this._onLastBumperFocus, false);
        }
        updateProps(props) {
            this._prevProps = this._props;
            this._props = props;
            //bumpers and root should be the same...
            if ((!this._prevProps.forceFocusInsideTrap && this._props.forceFocusInsideTrap) || (this._prevProps.disabled && !this._props.disabled)) {
                this._bringFocusIntoZone();
            }
            else if ((this._prevProps.forceFocusInsideTrap && !this._props.forceFocusInsideTrap) || (!this._prevProps.disabled && this._props.disabled)) {
                this._returnFocusToInitiator();
            }
        }
        setDisabled(disabled) {
            this._props.disabled = disabled;
        }
        _bringFocusIntoZone() {
            const { elementToFocusOnDismiss, disabled = false, disableFirstFocus = false, rootElement } = this._props;
            if (disabled) {
                return;
            }
            FocusTrapZoneInternal._focusStack.push(this);
            this._previouslyFocusedElementOutsideTrapZone = elementToFocusOnDismiss && elementToFocusOnDismiss.__internalId != null
                ? elementToFocusOnDismiss
                : document.activeElement;
            if (!disableFirstFocus && !BlazorFluentUiBaseComponent.elementContains(rootElement, this._previouslyFocusedElementOutsideTrapZone)) {
                this.focus();
            }
        }
        _returnFocusToInitiator() {
            const { ignoreExternalFocusing, rootElement } = this._props;
            FocusTrapZoneInternal._focusStack = FocusTrapZoneInternal._focusStack.filter((value) => {
                return this !== value;
            });
            const activeElement = document.activeElement;
            if (!ignoreExternalFocusing &&
                this._previouslyFocusedElementOutsideTrapZone &&
                typeof this._previouslyFocusedElementOutsideTrapZone.focus === 'function' &&
                (BlazorFluentUiBaseComponent.elementContains(rootElement, activeElement) || activeElement === document.body)) {
                this._focusAsync(this._previouslyFocusedElementOutsideTrapZone);
            }
        }
        _focusAsync(element) {
            if (!this._isBumper(element)) {
                focusAsync(element);
            }
        }
        _isBumper(element) {
            return element === this._props.firstBumper || element === this._props.lastBumper;
        }
        focus() {
            const { focusPreviouslyFocusedInnerElement, firstFocusableSelector, rootElement } = this._props;
            if (focusPreviouslyFocusedInnerElement &&
                this._previouslyFocusedElementInTrapZone &&
                BlazorFluentUiBaseComponent.elementContains(rootElement, this._previouslyFocusedElementInTrapZone)) {
                // focus on the last item that had focus in the zone before we left the zone
                this._focusAsync(this._previouslyFocusedElementInTrapZone);
                return;
            }
            const focusSelector = firstFocusableSelector;
            let _firstFocusableChild = null;
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
        }
    }
    FocusTrapZoneInternal._focusStack = [];
    var count = 0;
    var focusTrapZones = {};
    function register(props, focusTrapZone) {
        let currentId = count++;
        focusTrapZones[currentId] = new FocusTrapZoneInternal(props, focusTrapZone);
        return currentId;
    }
    BlazorFluentUiFocusTrapZone.register = register;
    function unregister(id) {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.unRegister();
        }
        delete focusTrapZones[id];
    }
    BlazorFluentUiFocusTrapZone.unregister = unregister;
    function updateProps(id, props) {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.updateProps(props);
        }
    }
    BlazorFluentUiFocusTrapZone.updateProps = updateProps;
    function focus(id) {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.focus();
        }
    }
    BlazorFluentUiFocusTrapZone.focus = focus;
    //export function elementContains(parent: HTMLElement, child: HTMLElement, allowVirtualParents: boolean = true): boolean {
    //    let isContained = false;
    //    if (parent && child) {
    //        if (allowVirtualParents) {
    //            isContained = false;
    //            while (child) {
    //                let nextParent: HTMLElement | null = getParent(child);
    //                console.log("NextParent: " + nextParent);
    //                if (nextParent === parent) {
    //                    isContained = true;
    //                    break;
    //                }
    //                child = nextParent;
    //            }
    //        } else if (parent.contains) {
    //            isContained = parent.contains(child);
    //        }
    //    }
    //    return isContained;
    //}
    //export function getParent(child: HTMLElement, allowVirtualParents: boolean = true): HTMLElement | null {
    //    return child && (child.parentNode && (child.parentNode as HTMLElement));
    //}
    let targetToFocusOnNextRepaint = undefined;
    function focusAsync(element) {
        if (element) {
            // An element was already queued to be focused, so replace that one with the new element
            if (targetToFocusOnNextRepaint) {
                targetToFocusOnNextRepaint = element;
                return;
            }
            targetToFocusOnNextRepaint = element;
            // element.focus() is a no-op if the element is no longer in the DOM, meaning this is always safe
            window.requestAnimationFrame(() => {
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
    function isElementVisible(element) {
        // If the element is not valid, return false.
        if (!element || !element.getAttribute) {
            return false;
        }
        const visibilityAttribute = element.getAttribute(IS_VISIBLE_ATTRIBUTE);
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
        let tabIndex = 0;
        let tabIndexAttributeValue = null;
        if (element && element.getAttribute) {
            tabIndexAttributeValue = element.getAttribute('tabIndex');
            if (tabIndexAttributeValue) {
                tabIndex = parseInt(tabIndexAttributeValue, 10);
            }
        }
        let isFocusableAttribute = element.getAttribute ? element.getAttribute(IS_FOCUSABLE_ATTRIBUTE) : null;
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
    function isElementFocusZone(element) {
        return !!(element && element.getAttribute && !!element.getAttribute(FOCUSZONE_ID_ATTRIBUTE));
    }
    function isElementFocusSubZone(element) {
        return !!(element && element.getAttribute && element.getAttribute(FOCUSZONE_SUB_ATTRIBUTE) === 'true');
    }
    function getFirstTabbable(rootElement, currentElement, includeElementsInFocusZones, checkNode = true) {
        return getNextElement(rootElement, currentElement, checkNode, false /*suppressParentTraversal*/, false /*suppressChildTraversal*/, includeElementsInFocusZones, false /*allowFocusRoot*/, true /*tabbable*/);
    }
    function getLastTabbable(rootElement, currentElement, includeElementsInFocusZones, checkNode = true) {
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
    let FocusZoneDirection;
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
    let FocusZoneTabbableElements;
    (function (FocusZoneTabbableElements) {
        /** Tabbing is not allowed */
        FocusZoneTabbableElements[FocusZoneTabbableElements["none"] = 0] = "none";
        /** All tabbing action is allowed */
        FocusZoneTabbableElements[FocusZoneTabbableElements["all"] = 1] = "all";
        /** Tabbing is allowed only on input elements */
        FocusZoneTabbableElements[FocusZoneTabbableElements["inputOnly"] = 2] = "inputOnly";
    })(FocusZoneTabbableElements || (FocusZoneTabbableElements = {}));
    const IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    const IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    const FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    const FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    const IS_ENTER_DISABLED_ATTRIBUTE = 'data-disable-click-on-enter';
    const TABINDEX = 'tabindex';
    const NO_VERTICAL_WRAP = 'data-no-vertical-wrap';
    const NO_HORIZONTAL_WRAP = 'data-no-horizontal-wrap';
    const LARGE_DISTANCE_FROM_CENTER = 999999999;
    const LARGE_NEGATIVE_DISTANCE_FROM_CENTER = -999999999;
    const ALLOWED_INPUT_TYPES = ['text', 'number', 'password', 'email', 'tel', 'url', 'search'];
    const ALLOW_VIRTUAL_ELEMENTS = false; // this is not used in Blazor... concept for React only
    var count = 0;
    var allInstances = {};
    var outerZones = new Set();
    let _disposeGlobalKeyDownListener;
    function register(props, focusZone) {
        let currentId = count++;
        allInstances[currentId] = new FocusZoneInternal(props, focusZone);
        return currentId;
    }
    BlazorFluentUiFocusZone.register = register;
    function unregister(id) {
        let focusZone = allInstances[id];
        if (focusZone) {
            focusZone.unRegister();
        }
        delete allInstances[id];
    }
    BlazorFluentUiFocusZone.unregister = unregister;
    function updateFocusZone(id, props) {
        let focusZone = allInstances[id];
        if (focusZone) {
            focusZone.updateFocusZone(props);
        }
    }
    BlazorFluentUiFocusZone.updateFocusZone = updateFocusZone;
    class FocusZoneInternal {
        constructor(focusZoneProps, dotNetRef) {
            this._disposables = [];
            this._onBlur = () => {
                this._setParkedFocus(false);
            };
            this._onKeyDown = (ev) => {
                if (this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                const { direction, disabled, innerZoneKeystrokeTriggers } = this._focusZoneProps;
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
                if (document.activeElement === this._root && this._isInnerZone) {
                    // If this element has focus, it is being controlled by a parent.
                    // Ignore the keystroke.
                    return;
                }
                if (innerZoneKeystrokeTriggers && (innerZoneKeystrokeTriggers.indexOf(ev.keyCode) != -1) && this._isImmediateDescendantOfZone(ev.target)) {
                    // Try to focus
                    const innerZone = this._getFirstInnerZone();
                    if (innerZone) {
                        if (!innerZone.focus(true)) {
                            return;
                        }
                    }
                    else if (window.BlazorFluentUiBaseComponent.isElementFocusSubZone(ev.target)) {
                        if (!this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(ev.target, ev.target.firstChild, true))) {
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
                            if (this._tryInvokeClickForFocusable(ev.target)) {
                                break;
                            }
                            return;
                        case 37 /* left */:
                            if (direction !== FocusZoneDirection.vertical && this._moveFocusLeft()) {
                                break;
                            }
                            return;
                        case 39 /* right */:
                            if (direction !== FocusZoneDirection.vertical && this._moveFocusRight()) {
                                break;
                            }
                            return;
                        case 38 /* up */:
                            if (direction !== FocusZoneDirection.horizontal && this._moveFocusUp()) {
                                break;
                            }
                            return;
                        case 40 /* down */:
                            if (direction !== FocusZoneDirection.horizontal && this._moveFocusDown()) {
                                break;
                            }
                            return;
                        case 9 /* tab */:
                            if (this._focusZoneProps.handleTabKey === FocusZoneTabbableElements.all ||
                                (this._focusZoneProps.handleTabKey === FocusZoneTabbableElements.inputOnly && this._isElementInput(ev.target))) {
                                let focusChanged = false;
                                this._processingTabKey = true;
                                if (direction === FocusZoneDirection.vertical ||
                                    !this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP)) {
                                    focusChanged = ev.shiftKey ? this._moveFocusUp() : this._moveFocusDown();
                                }
                                else if (direction === FocusZoneDirection.horizontal || direction === FocusZoneDirection.bidirectional) {
                                    const tabWithDirection = window.BlazorFluentUiBaseComponent.getRTL() ? !ev.shiftKey : ev.shiftKey;
                                    focusChanged = tabWithDirection ? this._moveFocusLeft() : this._moveFocusRight();
                                }
                                this._processingTabKey = false;
                                if (focusChanged) {
                                    break;
                                }
                            }
                            return;
                        case 36 /* home */:
                            if (this._isElementInput(ev.target) && !this._shouldInputLoseFocus(ev.target, false)) {
                                return;
                            }
                            const firstChild = this._root && this._root.firstChild;
                            if (this._root && firstChild && this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(this._root, firstChild, true))) {
                                break;
                            }
                            return;
                        case 35 /* end */:
                            if (this._isElementInput(ev.target) && !this._shouldInputLoseFocus(ev.target, true)) {
                                return;
                            }
                            const lastChild = this._root && this._root.lastChild;
                            if (this._root && this.focusElement(window.BlazorFluentUiBaseComponent.getPreviousElement(this._root, lastChild, true, true, true))) {
                                break;
                            }
                            return;
                        case 13 /* enter */:
                            if (this._tryInvokeClickForFocusable(ev.target)) {
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
            this._onFocus = (ev) => {
                if (this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                const { doNotAllowFocusEventToPropagate } = this._focusZoneProps;
                const isImmediateDescendant = this._isImmediateDescendantOfZone(ev.target);
                let newActiveElement;
                this._dotNetRef.invokeMethodAsync("JSOnFocusNotification");
                if (isImmediateDescendant) {
                    newActiveElement = ev.target;
                }
                else {
                    let parentElement = ev.target;
                    while (parentElement && parentElement !== this._root) {
                        if (window.BlazorFluentUiBaseComponent.isElementTabbable(parentElement) && this._isImmediateDescendantOfZone(parentElement)) {
                            newActiveElement = parentElement;
                            break;
                        }
                        parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                    }
                }
                const initialElementFocused = !this._activeElement;
                // If the new active element is a child of this zone and received focus,
                // update alignment an immediate descendant
                if (newActiveElement && newActiveElement !== this._activeElement) {
                    if (isImmediateDescendant || initialElementFocused) {
                        this._setFocusAlignment(newActiveElement, true, true);
                    }
                    this._activeElement = newActiveElement;
                    if (initialElementFocused) {
                        this._updateTabIndexes();
                    }
                }
                this._dotNetRef.invokeMethodAsync("JSOnActiveElementChanged");
                if (doNotAllowFocusEventToPropagate) {
                    ev.stopPropagation();
                }
            };
            this._onKeyDownCapture = (ev) => {
                if (ev.which === 9 /* tab */) {
                    outerZones.forEach(zone => zone._updateTabIndexes());
                }
            };
            this._onMouseDown = (ev) => {
                if (this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                const { disabled } = this._focusZoneProps;
                if (disabled) {
                    return;
                }
                let target = ev.target;
                const path = [];
                while (target && target !== this._root) {
                    path.push(target);
                    target = window.BlazorFluentUiBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
                }
                while (path.length) {
                    target = path.pop();
                    if (target && window.BlazorFluentUiBaseComponent.isElementTabbable(target)) {
                        this._setActiveElement(target, true);
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
        updateFocusZone(props) {
            this._focusZoneProps = props;
            allInstances[props.id] = this;
            if (this._root) {
                const windowElement = window.BlazorFluentUiBaseComponent.getWindow(this._root);
                let parentElement = window.BlazorFluentUiBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
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
        }
        initialized() {
            const windowElement = window.BlazorFluentUiBaseComponent.getWindow(this._root);
            let parentElement = window.BlazorFluentUiBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
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
        }
        /**
       * When focus is in the zone at render time but then all focusable elements are removed,
       * we "park" focus temporarily on the root. Once we update with focusable children, we restore
       * focus to the closest path from previous. If the user tabs away from the parked container,
       * we restore focusability to the pre-parked state.
       */
        _setParkedFocus(isParked) {
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
        }
        unRegister() {
            if (!this._isInnerZone) {
                outerZones.delete(this);
            }
            this._disposables.forEach(d => d());
            if (outerZones.size === 0 && _disposeGlobalKeyDownListener) {
                _disposeGlobalKeyDownListener();
            }
            this._root.removeEventListener("keydown", this._onKeyDown, false);
            this._root.removeEventListener("focus", this._onFocus, false);
            this._root.removeEventListener("mousedown", this._onMouseDown, false);
        }
        focus(forceIntoFirstElement = false) {
            if (this._root) {
                if (!forceIntoFirstElement && this._root.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' && this._isInnerZone) {
                    const ownerZoneElement = this._getOwnerZone(this._root);
                    if (ownerZoneElement !== this._root) {
                        const ownerZone = allInstances[ownerZoneElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
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
                    const firstChild = this._root.firstChild;
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(this._root, firstChild, true));
                }
            }
            return false;
        }
        focusElement(element) {
            const { onBeforeFocusExists } = this._focusZoneProps;
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
        }
        _updateTabIndexes(element) {
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
            const childNodes = element && element.children;
            for (let childIndex = 0; childNodes && childIndex < childNodes.length; childIndex++) {
                const child = childNodes[childIndex];
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
        }
        _getOwnerZone(element) {
            let parentElement = window.BlazorFluentUiBaseComponent.getParent(element, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== this._root && parentElement !== document.body) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                    return parentElement;
                }
                parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            return parentElement;
        }
        _setActiveElement(element, forceAlignment) {
            const previousActiveElement = this._activeElement;
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
        }
        _setFocusAlignment(element, isHorizontal, isVertical) {
            if (this._focusZoneProps.direction === FocusZoneDirection.bidirectional && (!this._focusAlignment || isHorizontal || isVertical)) {
                const rect = element.getBoundingClientRect();
                const left = rect.left + rect.width / 2;
                const top = rect.top + rect.height / 2;
                if (!this._focusAlignment) {
                    this._focusAlignment = {
                        x: left,
                        y: top
                    };
                }
                if (isHorizontal) {
                    this._focusAlignment.x = left;
                }
                if (isVertical) {
                    this._focusAlignment.y = top;
                }
            }
        }
        _isImmediateDescendantOfZone(element) {
            return this._getOwnerZone(element) === this._root;
        }
        /**
   * Traverse to find first child zone.
   */
        _getFirstInnerZone(rootElement) {
            rootElement = rootElement || this._activeElement || this._root;
            if (!rootElement) {
                return null;
            }
            if (window.BlazorFluentUiBaseComponent.isElementFocusZone(rootElement)) {
                return allInstances[rootElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
            }
            let child = rootElement.firstElementChild;
            while (child) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(child)) {
                    return allInstances[child.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
                }
                const match = this._getFirstInnerZone(child);
                if (match) {
                    return match;
                }
                child = child.nextElementSibling;
            }
            return null;
        }
        /**
       * Walk up the dom try to find a focusable element.
       */
        _tryInvokeClickForFocusable(target) {
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
        }
        /**
        * Returns true if the element is a descendant of the FocusZone through a React portal.
        */
        _portalContainsElement(element) {
            // This might break our control when used inside a Layer...
            return false;
            //return element && !!this._root && BlazorFluentUiBaseComponent portalContainsElement(element, this._root.current);
        }
        _isElementInput(element) {
            if (element && element.tagName && (element.tagName.toLowerCase() === 'input' || element.tagName.toLowerCase() === 'textarea')) {
                return true;
            }
            return false;
        }
        _shouldInputLoseFocus(element, isForward) {
            // If a tab was used, we want to focus on the next element.
            if (!this._processingTabKey && element && element.type && ALLOWED_INPUT_TYPES.indexOf(element.type.toLowerCase()) > -1) {
                const selectionStart = element.selectionStart;
                const selectionEnd = element.selectionEnd;
                const isRangeSelected = selectionStart !== selectionEnd;
                const inputValue = element.value;
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
        }
        _shouldWrapFocus(element, noWrapDataAttribute) {
            return !!this._focusZoneProps.checkForNoWrap ? window.BlazorFluentUiBaseComponent.shouldWrapFocus(element, noWrapDataAttribute) : true;
        }
        _moveFocus(isForward, getDistanceFromCenter, ev, useDefaultWrap = true) {
            let element = this._activeElement;
            let candidateDistance = -1;
            let candidateElement = undefined;
            let changedFocus = false;
            const isBidirectional = this._focusZoneProps.direction === FocusZoneDirection.bidirectional;
            if (!element || !this._root) {
                return false;
            }
            if (this._isElementInput(element)) {
                if (!this._shouldInputLoseFocus(element, isForward)) {
                    return false;
                }
            }
            const activeRect = isBidirectional ? element.getBoundingClientRect() : null;
            do {
                element = (isForward ? window.BlazorFluentUiBaseComponent.getNextElement(this._root, element) : window.BlazorFluentUiBaseComponent.getPreviousElement(this._root, element));
                if (isBidirectional) {
                    if (element) {
                        const targetRect = element.getBoundingClientRect();
                        const elementDistance = getDistanceFromCenter(activeRect, targetRect);
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
        }
        _moveFocusDown() {
            let targetTop = -1;
            const leftAlignment = this._focusAlignment.x;
            if (this._moveFocus(true, (activeRect, targetRect) => {
                let distance = -1;
                // ClientRect values can be floats that differ by very small fractions of a decimal.
                // If the difference between top and bottom are within a pixel then we should treat
                // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
                // but without Math.Floor they will be handled incorrectly.
                const targetRectTop = Math.floor(targetRect.top);
                const activeRectBottom = Math.floor(activeRect.bottom);
                if (targetRectTop < activeRectBottom) {
                    if (!this._shouldWrapFocus(this._activeElement, NO_VERTICAL_WRAP)) {
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
        }
        _moveFocusUp() {
            let targetTop = -1;
            const leftAlignment = this._focusAlignment.x;
            if (this._moveFocus(false, (activeRect, targetRect) => {
                let distance = -1;
                // ClientRect values can be floats that differ by very small fractions of a decimal.
                // If the difference between top and bottom are within a pixel then we should treat
                // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
                // but without Math.Floor they will be handled incorrectly.
                const targetRectBottom = Math.floor(targetRect.bottom);
                const targetRectTop = Math.floor(targetRect.top);
                const activeRectTop = Math.floor(activeRect.top);
                if (targetRectBottom > activeRectTop) {
                    if (!this._shouldWrapFocus(this._activeElement, NO_VERTICAL_WRAP)) {
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
        }
        _moveFocusLeft() {
            const shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
            if (this._moveFocus(window.BlazorFluentUiBaseComponent.getRTL(), (activeRect, targetRect) => {
                let distance = -1;
                let topBottomComparison;
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
                if (topBottomComparison && targetRect.right <= activeRect.right && this._focusZoneProps.direction !== FocusZoneDirection.vertical) {
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
        }
        _moveFocusRight() {
            const shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
            if (this._moveFocus(!window.BlazorFluentUiBaseComponent.getRTL(), (activeRect, targetRect) => {
                let distance = -1;
                let topBottomComparison;
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
                if (topBottomComparison && targetRect.left >= activeRect.left && this._focusZoneProps.direction !== FocusZoneDirection.vertical) {
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
        }
    }
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
                let index = item.getAttribute("data-hash");
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
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
/// <reference path="../../BlazorFluentUI.BFUFocusTrapZone/wwwroot/focusTrapZone.ts" />
/// <reference path="../../BlazorFluentUI.BFUBaseComponent/wwwroot/baseComponent.ts" />
var BlazorFluentUiPanel;
(function (BlazorFluentUiPanel) {
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
    BlazorFluentUiPanel.registerSizeHandler = registerSizeHandler;
    function registerMouseDownHandler(panelElement, panelDotNet) {
        var mouseDownId = Handler.addListener(document.body, "mousedown", (ev) => {
            //first get whether click is inside panel
            if (!ev.defaultPrevented) {
                var contains = BlazorFluentUiBaseComponent.elementContains(panelElement, ev.target);
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
    BlazorFluentUiPanel.makeElementScrollAllower = makeElementScrollAllower;
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
})(BlazorFluentUiPanel || (BlazorFluentUiPanel = {}));
window['BlazorFluentUiPanel'] = BlazorFluentUiPanel || {};
//namespace BlazorFluentUiTextField {
//    export function getScrollHeight(element: HTMLElement): number {
//        var paddingTop = window.getComputedStyle(element, null).getPropertyValue('padding-top');
//        var paddingBottom = window.getComputedStyle(element, null).getPropertyValue('padding-bottom');
//        var yPadding = parseInt(paddingTop) + parseInt(paddingBottom);
//        return element.scrollHeight - yPadding;
//    };
//}
//(<any>window)['BlazorFluentUiTextField'] = BlazorFluentUiTextField || {};
// https://github.com/pladaria/requestidlecallback-polyfill
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        define([], factory);
    }
    else if (typeof module === 'object' && module.exports) {
        module.exports = factory();
    }
    else {
        window.idleCallbackShim = factory();
    }
}(function () {
    'use strict';
    var scheduleStart, throttleDelay, lazytimer, lazyraf;
    var root = typeof window != 'undefined' ?
        window :
        typeof global != undefined ?
            global :
            this || {};
    var requestAnimationFrame = root.cancelRequestAnimationFrame && root.requestAnimationFrame || setTimeout;
    var cancelRequestAnimationFrame = root.cancelRequestAnimationFrame || clearTimeout;
    var tasks = [];
    var runAttempts = 0;
    var isRunning = false;
    var remainingTime = 7;
    var minThrottle = 35;
    var throttle = 125;
    var index = 0;
    var taskStart = 0;
    var tasklength = 0;
    var IdleDeadline = {
        get didTimeout() {
            return false;
        },
        timeRemaining: function () {
            var timeRemaining = remainingTime - (Date.now() - taskStart);
            return timeRemaining < 0 ? 0 : timeRemaining;
        },
    };
    var setInactive = debounce(function () {
        remainingTime = 22;
        throttle = 66;
        minThrottle = 0;
    });
    function debounce(fn) {
        var id, timestamp;
        var wait = 99;
        var check = function () {
            var last = (Date.now()) - timestamp;
            if (last < wait) {
                id = setTimeout(check, wait - last);
            }
            else {
                id = null;
                fn();
            }
        };
        return function () {
            timestamp = Date.now();
            if (!id) {
                id = setTimeout(check, wait);
            }
        };
    }
    function abortRunning() {
        if (isRunning) {
            if (lazyraf) {
                cancelRequestAnimationFrame(lazyraf);
            }
            if (lazytimer) {
                clearTimeout(lazytimer);
            }
            isRunning = false;
        }
    }
    function onInputorMutation() {
        if (throttle != 125) {
            remainingTime = 7;
            throttle = 125;
            minThrottle = 35;
            if (isRunning) {
                abortRunning();
                scheduleLazy();
            }
        }
        setInactive();
    }
    function scheduleAfterRaf() {
        lazyraf = null;
        lazytimer = setTimeout(runTasks, 0);
    }
    function scheduleRaf() {
        lazytimer = null;
        requestAnimationFrame(scheduleAfterRaf);
    }
    function scheduleLazy() {
        if (isRunning) {
            return;
        }
        throttleDelay = throttle - (Date.now() - taskStart);
        scheduleStart = Date.now();
        isRunning = true;
        if (minThrottle && throttleDelay < minThrottle) {
            throttleDelay = minThrottle;
        }
        if (throttleDelay > 9) {
            lazytimer = setTimeout(scheduleRaf, throttleDelay);
        }
        else {
            throttleDelay = 0;
            scheduleRaf();
        }
    }
    function runTasks() {
        var task, i, len;
        var timeThreshold = remainingTime > 9 ?
            9 :
            1;
        taskStart = Date.now();
        isRunning = false;
        lazytimer = null;
        if (runAttempts > 2 || taskStart - throttleDelay - 50 < scheduleStart) {
            for (i = 0, len = tasks.length; i < len && IdleDeadline.timeRemaining() > timeThreshold; i++) {
                task = tasks.shift();
                tasklength++;
                if (task) {
                    task(IdleDeadline);
                }
            }
        }
        if (tasks.length) {
            scheduleLazy();
        }
        else {
            runAttempts = 0;
        }
    }
    function requestIdleCallbackShim(task) {
        index++;
        tasks.push(task);
        scheduleLazy();
        return index;
    }
    function cancelIdleCallbackShim(id) {
        var index = id - 1 - tasklength;
        if (tasks[index]) {
            tasks[index] = null;
        }
    }
    if (!root.requestIdleCallback || !root.cancelIdleCallback) {
        root.requestIdleCallback = requestIdleCallbackShim;
        root.cancelIdleCallback = cancelIdleCallbackShim;
        if (root.document && document.addEventListener) {
            root.addEventListener('scroll', onInputorMutation, true);
            root.addEventListener('resize', onInputorMutation);
            document.addEventListener('focus', onInputorMutation, true);
            document.addEventListener('mouseover', onInputorMutation, true);
            ['click', 'keypress', 'touchstart', 'mousedown'].forEach(function (name) {
                document.addEventListener(name, onInputorMutation, { capture: true, passive: true });
            });
            if (root.MutationObserver) {
                new MutationObserver(onInputorMutation).observe(document.documentElement, { childList: true, subtree: true, attributes: true });
            }
        }
    }
    else {
        try {
            root.requestIdleCallback(function () { }, { timeout: 0 });
        }
        catch (e) {
            (function (rIC) {
                var timeRemainingProto, timeRemaining;
                root.requestIdleCallback = function (fn, timeout) {
                    if (timeout && typeof timeout.timeout == 'number') {
                        return rIC(fn, timeout.timeout);
                    }
                    return rIC(fn);
                };
                if (root.IdleCallbackDeadline && (timeRemainingProto = IdleCallbackDeadline.prototype)) {
                    timeRemaining = Object.getOwnPropertyDescriptor(timeRemainingProto, 'timeRemaining');
                    if (!timeRemaining || !timeRemaining.configurable || !timeRemaining.get) {
                        return;
                    }
                    Object.defineProperty(timeRemainingProto, 'timeRemaining', {
                        value: function () {
                            return timeRemaining.get.call(this);
                        },
                        enumerable: true,
                        configurable: true,
                    });
                }
            })(root.requestIdleCallback);
        }
    }
    return {
        request: requestIdleCallbackShim,
        cancel: cancelIdleCallbackShim,
    };
}));
