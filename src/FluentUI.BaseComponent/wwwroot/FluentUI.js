var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var FluentUIBaseComponent;
(function (FluentUIBaseComponent) {
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
    FluentUIBaseComponent.initializeFocusRects = initializeFocusRects;
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
    FluentUIBaseComponent.enableBodyScroll = enableBodyScroll;
    function disableBodyScroll() {
        if (!_bodyScrollDisabledCount) {
            document.body.classList.add("disabledBodyScroll");
            document.body.addEventListener('touchmove', _disableIosBodyScroll, { passive: false, capture: false });
        }
        _bodyScrollDisabledCount++;
    }
    FluentUIBaseComponent.disableBodyScroll = disableBodyScroll;
    const _disableIosBodyScroll = (event) => {
        event.preventDefault();
    };
    // end
    function getClientHeight(element) {
        if (element == null)
            return 0;
        return element.clientHeight;
    }
    FluentUIBaseComponent.getClientHeight = getClientHeight;
    function getScrollHeight(element) {
        if (element == null)
            return 0;
        return element.scrollHeight;
    }
    FluentUIBaseComponent.getScrollHeight = getScrollHeight;
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
    FluentUIBaseComponent.findScrollableParent = findScrollableParent;
    function measureElement(element) {
        var rect = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        };
        return rect;
    }
    FluentUIBaseComponent.measureElement = measureElement;
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
    FluentUIBaseComponent.getNaturalBounds = getNaturalBounds;
    function supportsObjectFit() {
        return window !== undefined && window.navigator.msMaxTouchPoints === undefined;
    }
    FluentUIBaseComponent.supportsObjectFit = supportsObjectFit;
    function hasOverflow(element) {
        return false;
    }
    FluentUIBaseComponent.hasOverflow = hasOverflow;
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
    FluentUIBaseComponent.measureScrollWindow = measureScrollWindow;
    function measureScrollDimensions(element) {
        var dimensions = {
            scrollHeight: element.scrollHeight,
            scrollWidth: element.scrollWidth,
        };
        return dimensions;
    }
    FluentUIBaseComponent.measureScrollDimensions = measureScrollDimensions;
    function measureElementRect(element) {
        if (element !== undefined && element !== null) {
            // EdgeHTML's rectangle can't be serialized for some reason.... serializes to 0 everything.   So break it apart into simple JSON.
            var rect = element.getBoundingClientRect();
            return { height: rect.height, width: rect.width, left: rect.left, right: rect.right, top: rect.top, bottom: rect.bottom };
        }
        else
            return { height: 0, width: 0, left: 0, right: 0, top: 0, bottom: 0 };
    }
    FluentUIBaseComponent.measureElementRect = measureElementRect;
    function getWindow(element) {
        return element.ownerDocument.defaultView;
    }
    FluentUIBaseComponent.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    FluentUIBaseComponent.getWindowRect = getWindowRect;
    function getElementId(element) {
        if (element !== undefined) {
            return element.id;
        }
        return null;
    }
    FluentUIBaseComponent.getElementId = getElementId;
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
    FluentUIBaseComponent.registerKeyEventsForList = registerKeyEventsForList;
    function deregisterKeyEventsForList(guid) {
        var tuple = eventElementRegister[guid];
        if (tuple) {
            var element = tuple[0];
            var func = tuple[1];
            element.removeEventListener("keydown", func);
            eventElementRegister[guid] = null;
        }
    }
    FluentUIBaseComponent.deregisterKeyEventsForList = deregisterKeyEventsForList;
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
    FluentUIBaseComponent.registerWindowKeyDownEvent = registerWindowKeyDownEvent;
    function deregisterWindowKeyDownEvent(guid) {
        var func = eventRegister[guid];
        window.removeEventListener("keydown", func);
        eventRegister[guid] = null;
    }
    FluentUIBaseComponent.deregisterWindowKeyDownEvent = deregisterWindowKeyDownEvent;
    function registerResizeEvent(dotnetRef, functionName) {
        var guid = Guid.newGuid();
        eventRegister[guid] = debounce((ev) => {
            dotnetRef.invokeMethodAsync(functionName, window.innerWidth, innerHeight);
        }, 100, { leading: true });
        window.addEventListener("resize", eventRegister[guid]);
        return guid;
    }
    FluentUIBaseComponent.registerResizeEvent = registerResizeEvent;
    function deregisterResizeEvent(guid) {
        var func = eventRegister[guid];
        window.removeEventListener("resize", func);
        eventRegister[guid] = null;
    }
    FluentUIBaseComponent.deregisterResizeEvent = deregisterResizeEvent;
    class Guid {
        static newGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }
    }
    var _lastId = 0;
    var cachedViewports = new Map();
    class Viewport {
        constructor(component, rootElement, fireInitialViewport = false) {
            this.RESIZE_DELAY = 500;
            this.MAX_RESIZE_ATTEMPTS = 3;
            this.viewport = { width: 0, height: 0 };
            this._onAsyncResizeAsync = () => {
                this._updateViewportAsync();
            };
            this.id = _lastId++;
            this.component = component;
            this.rootElement = rootElement;
            this._onAsyncResizeAsync = debounce(this._onAsyncResizeAsync, this.RESIZE_DELAY, { leading: true });
            this.viewportResizeObserver = new window.ResizeObserver(this._onAsyncResizeAsync);
            this.viewportResizeObserver.observe(this.rootElement);
            if (fireInitialViewport) {
                this._onAsyncResizeAsync();
            }
        }
        disconnect() {
            this.viewportResizeObserver.disconnect();
        }
        _updateViewportAsync(withForceUpdate) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { viewport } = this.state;
                const viewportElement = this.rootElement;
                const scrollElement = findScrollableParent(viewportElement);
                const scrollRect = getRect(scrollElement);
                const clientRect = getRect(viewportElement);
                const updateComponentAsync = () => __awaiter(this, void 0, void 0, function* () {
                    if (withForceUpdate) {
                        yield this.component.invokeMethodAsync("ForceUpdate");
                    }
                });
                const isSizeChanged = (clientRect && clientRect.width) !== this.viewport.width || (scrollRect && scrollRect.height) !== this.viewport.height;
                if (isSizeChanged && this._resizeAttempts < this.MAX_RESIZE_ATTEMPTS && clientRect && scrollRect) {
                    this._resizeAttempts++;
                    this.viewport = {
                        width: clientRect.width,
                        height: scrollRect.height
                    };
                    yield this.component.invokeMethodAsync("ViewportChanged", this.viewport);
                    yield this._updateViewportAsync(withForceUpdate);
                }
                else {
                    this._resizeAttempts = 0;
                    yield updateComponentAsync();
                }
            });
        }
        ;
    }
    function addViewport(component, rootElement, fireInitialViewport = false) {
        let viewport = new Viewport(component, rootElement, fireInitialViewport);
        cachedViewports.set(viewport.id, viewport);
        return viewport.id;
    }
    FluentUIBaseComponent.addViewport = addViewport;
    function removeViewport(id) {
        let viewport = cachedViewports.get(id);
        viewport.disconnect();
        cachedViewports.delete(id);
    }
    FluentUIBaseComponent.removeViewport = removeViewport;
    function getRect(element) {
        let rect;
        if (element) {
            if (element === window) {
                rect = {
                    left: 0,
                    top: 0,
                    width: window.innerWidth,
                    height: window.innerHeight,
                    right: window.innerWidth,
                    bottom: window.innerHeight,
                };
            }
            else if (element.getBoundingClientRect) {
                rect = element.getBoundingClientRect();
            }
        }
        return rect;
    }
    FluentUIBaseComponent.getRect = getRect;
    function findElementRecursive(element, matchFunction) {
        if (!element || element === document.body) {
            return null;
        }
        return matchFunction(element) ? element : findElementRecursive(getParent(element), matchFunction);
    }
    FluentUIBaseComponent.findElementRecursive = findElementRecursive;
    function elementContainsAttribute(element, attribute) {
        let elementMatch = findElementRecursive(element, (testElement) => testElement.hasAttribute(attribute));
        return elementMatch && elementMatch.getAttribute(attribute);
    }
    FluentUIBaseComponent.elementContainsAttribute = elementContainsAttribute;
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
    FluentUIBaseComponent.storeLastFocusedElement = storeLastFocusedElement;
    function restoreLastFocus(guid, restoreFocus = true) {
        var htmlElement = _lastFocus[guid];
        if (htmlElement != null) {
            if (restoreFocus) {
                htmlElement.focus();
            }
            delete _lastFocus[guid];
        }
    }
    FluentUIBaseComponent.restoreLastFocus = restoreLastFocus;
    function getActiveElement() {
        return document.activeElement;
    }
    FluentUIBaseComponent.getActiveElement = getActiveElement;
    function focusElement(element) {
        element.focus();
    }
    FluentUIBaseComponent.focusElement = focusElement;
    function focusFirstElementChild(element) {
        let child = this.getFirstFocusable(element, element, true);
        if (child) {
            child.focus();
        }
        else {
            element.focus();
        }
    }
    FluentUIBaseComponent.focusFirstElementChild = focusFirstElementChild;
    function shouldWrapFocus(element, noWrapDataAttribute) {
        return elementContainsAttribute(element, noWrapDataAttribute) === 'true' ? false : true;
    }
    FluentUIBaseComponent.shouldWrapFocus = shouldWrapFocus;
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
    FluentUIBaseComponent.getFocusableByIndexPath = getFocusableByIndexPath;
    function getFirstFocusable(rootElement, currentElement, includeElementsInFocusZones) {
        return getNextElement(rootElement, currentElement, true /*checkNode*/, false /*suppressParentTraversal*/, false /*suppressChildTraversal*/, includeElementsInFocusZones);
    }
    FluentUIBaseComponent.getFirstFocusable = getFirstFocusable;
    function getLastFocusable(rootElement, currentElement, includeElementsInFocusZones) {
        return getPreviousElement(rootElement, currentElement, true /*checkNode*/, false /*suppressParentTraversal*/, true /*suppressChildTraversal*/, includeElementsInFocusZones);
    }
    FluentUIBaseComponent.getLastFocusable = getLastFocusable;
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
    FluentUIBaseComponent.isElementTabbable = isElementTabbable;
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
    FluentUIBaseComponent.isElementVisible = isElementVisible;
    function focusFirstChild(rootElement) {
        return false;
    }
    FluentUIBaseComponent.focusFirstChild = focusFirstChild;
    function getParent(child, allowVirtualParents = true) {
        return child && ((allowVirtualParents && getVirtualParent(child)) || (child.parentNode && child.parentNode));
    }
    FluentUIBaseComponent.getParent = getParent;
    function addOrUpdateVirtualParent(parent) {
        layerElements[parent.dataset.layerId] = parent;
    }
    FluentUIBaseComponent.addOrUpdateVirtualParent = addOrUpdateVirtualParent;
    function getVirtualParent(child) {
        let parent;
        if (child && child.dataset && child.dataset.parentLayerId) {
            parent = layerElements[child.dataset.parentLayerId];
        }
        return parent;
    }
    FluentUIBaseComponent.getVirtualParent = getVirtualParent;
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
    FluentUIBaseComponent.elementContains = elementContains;
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
    FluentUIBaseComponent.getNextElement = getNextElement;
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
    FluentUIBaseComponent.getPreviousElement = getPreviousElement;
    /** Raises a click event. */
    function raiseClick(target) {
        const event = createNewEvent('MouseEvents');
        event.initEvent('click', true, true);
        target.dispatchEvent(event);
    }
    FluentUIBaseComponent.raiseClick = raiseClick;
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
    FluentUIBaseComponent.isElementFocusZone = isElementFocusZone;
    function isElementFocusSubZone(element) {
        return !!(element && element.getAttribute && element.getAttribute(FOCUSZONE_SUB_ATTRIBUTE) === 'true');
    }
    FluentUIBaseComponent.isElementFocusSubZone = isElementFocusSubZone;
    function on(element, eventName, callback, options) {
        element.addEventListener(eventName, callback, options);
        return () => element.removeEventListener(eventName, callback, options);
    }
    FluentUIBaseComponent.on = on;
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
    FluentUIBaseComponent.debounce = debounce;
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
    FluentUIBaseComponent.getRTL = getRTL;
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
    FluentUIBaseComponent.setRTL = setRTL;
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
    FluentUIBaseComponent.getItem = getItem;
    function setItem(key, data) {
        try {
            window.sessionStorage.setItem(key, data);
        }
        catch (e) {
            /* Eat the exception */
        }
    }
    FluentUIBaseComponent.setItem = setItem;
    class Async {
        // tslint:disable-next-line:no-any
        constructor(parent, onError) {
            this._timeoutIds = null;
            this._immediateIds = null;
            this._intervalIds = null;
            this._animationFrameIds = null;
            this._isDisposed = false;
            this._parent = parent || null;
            this._onErrorHandler = onError;
            this._noop = () => {
                /* do nothing */
            };
        }
        /**
         * Dispose function, clears all async operations.
         */
        dispose() {
            let id;
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
        }
        /**
         * SetTimeout override, which will auto cancel the timeout during dispose.
         * @param callback - Callback to execute.
         * @param duration - Duration in milliseconds.
         * @returns The setTimeout id.
         */
        setTimeout(callback, duration) {
            let timeoutId = 0;
            if (!this._isDisposed) {
                if (!this._timeoutIds) {
                    this._timeoutIds = {};
                }
                /* tslint:disable:ban-native-functions */
                timeoutId = setTimeout(() => {
                    // Time to execute the timeout, enqueue it as a foreground task to be executed.
                    try {
                        // Now delete the record and call the callback.
                        if (this._timeoutIds) {
                            delete this._timeoutIds[timeoutId];
                        }
                        callback.apply(this._parent);
                    }
                    catch (e) {
                        if (this._onErrorHandler) {
                            this._onErrorHandler(e);
                        }
                    }
                }, duration);
                /* tslint:enable:ban-native-functions */
                this._timeoutIds[timeoutId] = true;
            }
            return timeoutId;
        }
        /**
         * Clears the timeout.
         * @param id - Id to cancel.
         */
        clearTimeout(id) {
            if (this._timeoutIds && this._timeoutIds[id]) {
                /* tslint:disable:ban-native-functions */
                clearTimeout(id);
                delete this._timeoutIds[id];
                /* tslint:enable:ban-native-functions */
            }
        }
        /**
         * SetImmediate override, which will auto cancel the immediate during dispose.
         * @param callback - Callback to execute.
         * @param targetElement - Optional target element to use for identifying the correct window.
         * @returns The setTimeout id.
         */
        setImmediate(callback, targetElement) {
            let immediateId = 0;
            const win = getWindow(targetElement);
            if (!this._isDisposed) {
                if (!this._immediateIds) {
                    this._immediateIds = {};
                }
                /* tslint:disable:ban-native-functions */
                let setImmediateCallback = () => {
                    // Time to execute the timeout, enqueue it as a foreground task to be executed.
                    try {
                        // Now delete the record and call the callback.
                        if (this._immediateIds) {
                            delete this._immediateIds[immediateId];
                        }
                        callback.apply(this._parent);
                    }
                    catch (e) {
                        this._logError(e);
                    }
                };
                immediateId = win.setTimeout(setImmediateCallback, 0);
                /* tslint:enable:ban-native-functions */
                this._immediateIds[immediateId] = true;
            }
            return immediateId;
        }
        /**
         * Clears the immediate.
         * @param id - Id to cancel.
         * @param targetElement - Optional target element to use for identifying the correct window.
         */
        clearImmediate(id, targetElement) {
            const win = getWindow(targetElement);
            if (this._immediateIds && this._immediateIds[id]) {
                /* tslint:disable:ban-native-functions */
                win.clearTimeout(id);
                delete this._immediateIds[id];
                /* tslint:enable:ban-native-functions */
            }
        }
        /**
         * SetInterval override, which will auto cancel the timeout during dispose.
         * @param callback - Callback to execute.
         * @param duration - Duration in milliseconds.
         * @returns The setTimeout id.
         */
        setInterval(callback, duration) {
            let intervalId = 0;
            if (!this._isDisposed) {
                if (!this._intervalIds) {
                    this._intervalIds = {};
                }
                /* tslint:disable:ban-native-functions */
                intervalId = setInterval(() => {
                    // Time to execute the interval callback, enqueue it as a foreground task to be executed.
                    try {
                        callback.apply(this._parent);
                    }
                    catch (e) {
                        this._logError(e);
                    }
                }, duration);
                /* tslint:enable:ban-native-functions */
                this._intervalIds[intervalId] = true;
            }
            return intervalId;
        }
        /**
         * Clears the interval.
         * @param id - Id to cancel.
         */
        clearInterval(id) {
            if (this._intervalIds && this._intervalIds[id]) {
                /* tslint:disable:ban-native-functions */
                clearInterval(id);
                delete this._intervalIds[id];
                /* tslint:enable:ban-native-functions */
            }
        }
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
        throttle(func, wait, options) {
            if (this._isDisposed) {
                return this._noop;
            }
            let waitMS = wait || 0;
            let leading = true;
            let trailing = true;
            let lastExecuteTime = 0;
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
            let callback = (userCall) => {
                let now = new Date().getTime();
                let delta = now - lastExecuteTime;
                let waitLength = leading ? waitMS - delta : waitMS;
                if (delta >= waitMS && (!userCall || leading)) {
                    lastExecuteTime = now;
                    if (timeoutId) {
                        this.clearTimeout(timeoutId);
                        timeoutId = null;
                    }
                    lastResult = func.apply(this._parent, lastArgs);
                }
                else if (timeoutId === null && trailing) {
                    timeoutId = this.setTimeout(callback, waitLength);
                }
                return lastResult;
            };
            // tslint:disable-next-line:no-any
            let resultFunction = (...args) => {
                lastArgs = args;
                return callback(true);
            };
            return resultFunction;
        }
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
        debounce(func, wait, options) {
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
        requestAnimationFrame(callback, targetElement) {
            let animationFrameId = 0;
            const win = getWindow(targetElement);
            if (!this._isDisposed) {
                if (!this._animationFrameIds) {
                    this._animationFrameIds = {};
                }
                /* tslint:disable:ban-native-functions */
                let animationFrameCallback = () => {
                    try {
                        // Now delete the record and call the callback.
                        if (this._animationFrameIds) {
                            delete this._animationFrameIds[animationFrameId];
                        }
                        callback.apply(this._parent);
                    }
                    catch (e) {
                        this._logError(e);
                    }
                };
                animationFrameId = win.requestAnimationFrame
                    ? win.requestAnimationFrame(animationFrameCallback)
                    : win.setTimeout(animationFrameCallback, 0);
                /* tslint:enable:ban-native-functions */
                this._animationFrameIds[animationFrameId] = true;
            }
            return animationFrameId;
        }
        cancelAnimationFrame(id, targetElement) {
            const win = getWindow(targetElement);
            if (this._animationFrameIds && this._animationFrameIds[id]) {
                /* tslint:disable:ban-native-functions */
                win.cancelAnimationFrame ? win.cancelAnimationFrame(id) : win.clearTimeout(id);
                /* tslint:enable:ban-native-functions */
                delete this._animationFrameIds[id];
            }
        }
        // tslint:disable-next-line:no-any
        _logError(e) {
            if (this._onErrorHandler) {
                this._onErrorHandler(e);
            }
        }
    }
    FluentUIBaseComponent.Async = Async;
    function assign(target, ...args) {
        return filteredAssign.apply(this, [null, target].concat(args));
    }
    FluentUIBaseComponent.assign = assign;
    function filteredAssign(isAllowed, target, ...args) {
        target = target || {};
        for (let sourceObject of args) {
            if (sourceObject) {
                for (let propName in sourceObject) {
                    if (sourceObject.hasOwnProperty(propName) && (!isAllowed || isAllowed(propName))) {
                        target[propName] = sourceObject[propName];
                    }
                }
            }
        }
        return target;
    }
    FluentUIBaseComponent.filteredAssign = filteredAssign;
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
    class EventGroup {
        /** parent: the context in which events attached to non-HTMLElements are called */
        // tslint:disable-next-line:no-any
        constructor(parent) {
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
        static raise(
        // tslint:disable-next-line:no-any
        target, eventName, 
        // tslint:disable-next-line:no-any
        eventArgs, bubbleEvent) {
            let retVal;
            if (EventGroup._isElement(target)) {
                if (typeof document !== 'undefined' && document.createEvent) {
                    let ev = document.createEvent('HTMLEvents');
                    ev.initEvent(eventName, bubbleEvent || false, true);
                    assign(ev, eventArgs);
                    retVal = target.dispatchEvent(ev);
                    // tslint:disable-next-line:no-any
                }
                else if (typeof document !== 'undefined' && document['createEventObject']) {
                    // IE8
                    // tslint:disable-next-line:no-any
                    let evObj = document['createEventObject'](eventArgs);
                    // cannot set cancelBubble on evObj, fireEvent will overwrite it
                    target.fireEvent('on' + eventName, evObj);
                }
            }
            else {
                while (target && retVal !== false) {
                    let events = target.__events__;
                    let eventRecords = events ? events[eventName] : null;
                    if (eventRecords) {
                        for (let id in eventRecords) {
                            if (eventRecords.hasOwnProperty(id)) {
                                let eventRecordList = eventRecords[id];
                                for (let listIndex = 0; retVal !== false && listIndex < eventRecordList.length; listIndex++) {
                                    let record = eventRecordList[listIndex];
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
        }
        // tslint:disable-next-line:no-any
        static isObserved(target, eventName) {
            let events = target && target.__events__;
            return !!events && !!events[eventName];
        }
        /** Check to see if the target has declared support of the given event. */
        // tslint:disable-next-line:no-any
        static isDeclared(target, eventName) {
            let declaredEvents = target && target.__declaredEvents;
            return !!declaredEvents && !!declaredEvents[eventName];
        }
        // tslint:disable-next-line:no-any
        static stopPropagation(event) {
            if (event.stopPropagation) {
                event.stopPropagation();
            }
            else {
                // IE8
                event.cancelBubble = true;
            }
        }
        static _isElement(target) {
            return (!!target && (!!target.addEventListener || (typeof HTMLElement !== 'undefined' && target instanceof HTMLElement)));
        }
        dispose() {
            if (!this._isDisposed) {
                this._isDisposed = true;
                this.off();
                this._parent = null;
            }
        }
        /** On the target, attach a set of events, where the events object is a name to function mapping. */
        // tslint:disable-next-line:no-any
        onAll(target, events, useCapture) {
            for (let eventName in events) {
                if (events.hasOwnProperty(eventName)) {
                    this.on(target, eventName, events[eventName], useCapture);
                }
            }
        }
        /**
         * On the target, attach an event whose handler will be called in the context of the parent
         * of this instance of EventGroup.
         */
        on(target, // tslint:disable-line:no-any
        eventName, callback, // tslint:disable-line:no-any
        options) {
            if (eventName.indexOf(',') > -1) {
                let events = eventName.split(/[ ,]+/);
                for (let i = 0; i < events.length; i++) {
                    this.on(target, events[i], callback, options);
                }
            }
            else {
                let parent = this._parent;
                let eventRecord = {
                    target: target,
                    eventName: eventName,
                    parent: parent,
                    callback: callback,
                    options,
                };
                // Initialize and wire up the record on the target, so that it can call the callback if the event fires.
                let events = (target.__events__ = target.__events__ || {});
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
                    let processElementEvent = (...args) => {
                        if (this._isDisposed) {
                            return;
                        }
                        let result;
                        try {
                            result = callback.apply(parent, args);
                            if (result === false && args[0]) {
                                let e = args[0];
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
                    let processObjectEvent = (...args) => {
                        if (this._isDisposed) {
                            return;
                        }
                        return callback.apply(parent, args);
                    };
                    eventRecord.objectCallback = processObjectEvent;
                }
                // Remember the record locally, so that it can be removed.
                this._eventRecords.push(eventRecord);
            }
        }
        off(target, // tslint:disable-line:no-any
        eventName, callback, // tslint:disable-line:no-any
        options) {
            for (let i = 0; i < this._eventRecords.length; i++) {
                let eventRecord = this._eventRecords[i];
                if ((!target || target === eventRecord.target) &&
                    (!eventName || eventName === eventRecord.eventName) &&
                    (!callback || callback === eventRecord.callback) &&
                    (typeof options !== 'boolean' || options === eventRecord.options)) {
                    let events = eventRecord.target.__events__;
                    let targetArrayLookup = events[eventRecord.eventName];
                    let targetArray = targetArrayLookup ? targetArrayLookup[this._id] : null;
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
        }
        /** Trigger the given event in the context of this instance of EventGroup. */
        // tslint:disable-next-line:no-any
        raise(eventName, eventArgs, bubbleEvent) {
            return EventGroup.raise(this._parent, eventName, eventArgs, bubbleEvent);
        }
        /** Declare an event as being supported by this instance of EventGroup. */
        declare(event) {
            let declaredEvents = (this._parent.__declaredEvents = this._parent.__declaredEvents || {});
            if (typeof event === 'string') {
                declaredEvents[event] = true;
            }
            else {
                for (let i = 0; i < event.length; i++) {
                    declaredEvents[event[i]] = true;
                }
            }
        }
    }
    EventGroup._uniqueId = 0;
    FluentUIBaseComponent.EventGroup = EventGroup;
})(FluentUIBaseComponent || (FluentUIBaseComponent = {}));
//}
window.FluentUIBaseComponent = FluentUIBaseComponent;
// Workaround to prevent default on keypress until we can do it in Blazor conditionally without javascript
// https://stackoverflow.com/questions/24386354/execute-js-code-after-pressing-the-spacebar
window.addEventListener("load", function () {
    //This will be called when a key is pressed
    var preventDefaultOnSpaceCallback = function (e) {
        if (e.keyCode === 32 || e.key === " ") {
            // console.log("Prevented default.")
            e.preventDefault();
            return false;
        }
    };
    //This will add key event listener on all nodes with the class preventSpace.
    function setupPreventDefaultOnSpaceOnNode(node, add) {
        if (node instanceof HTMLElement) {
            var el = node;
            //Check if main element contains class
            if (el.classList.contains("prevent-default-on-space") && add) {
                // console.log("Adding preventer: " + el.id);
                el.addEventListener('keydown', preventDefaultOnSpaceCallback, false);
            }
            else {
                // console.log("Removing preventer: " + el.id);
                el.removeEventListener('keydown', preventDefaultOnSpaceCallback, false);
            }
        }
    }
    //This will add key event listener on all nodes with the class preventSpace.
    function setupPreventDefaultOnEnterOnElements(nodelist, add) {
        for (var i = 0; i < nodelist.length; i++) {
            var node = nodelist[i];
            if (node instanceof HTMLElement) {
                var el = node;
                //Check if main element contains class
                setupPreventDefaultOnSpaceOnNode(node, add);
                //Check if any child nodes contains class
                var elements = el.getElementsByClassName("prevent-default-on-space");
                for (var i_1 = 0; i_1 < elements.length; i_1++) {
                    setupPreventDefaultOnSpaceOnNode(elements[i_1], add);
                }
            }
        }
    }
    // Create an observer instance linked to the callback function
    // Read more: https://developer.mozilla.org/en-US/docs/Web/API/MutationObserver
    var preventDefaultOnEnterObserver = new MutationObserver(function (mutations) {
        for (var _i = 0, mutations_1 = mutations; _i < mutations_1.length; _i++) {
            var mutation = mutations_1[_i];
            if (mutation.type === 'childList') {
                // A child node has been added or removed.
                setupPreventDefaultOnEnterOnElements(mutation.addedNodes, true);
            }
            else if (mutation.type === 'attributes') {
                if (mutation.attributeName === "class") {
                    //console.log('The ' + mutation.attributeName + ' attribute was modified on' + (mutation.target as any).id);
                    //class was modified on this node. Remove previous event handler (if any).
                    setupPreventDefaultOnSpaceOnNode(mutation.target, false);
                    //And add event handler if class i specified.
                    setupPreventDefaultOnSpaceOnNode(mutation.target, true);
                }
            }
        }
    });
    // Only observe changes in nodes in the whole tree, but do not observe attributes.
    var preventDefaultOnEnterObserverConfig = { subtree: true, childList: true, attributes: true };
    // Start observing the target node for configured mutations
    preventDefaultOnEnterObserver.observe(document, preventDefaultOnEnterObserverConfig);
    //Also check all elements when loaded.
    setupPreventDefaultOnEnterOnElements(document.getElementsByClassName("prevent-default-on-space"), true);
});
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var FluentUICallout;
(function (FluentUICallout) {
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
    FluentUICallout.registerHandlers = registerHandlers;
    function unregisterHandlers(ids) {
        Handler.removeCallout(ids[ids.length - 1]);
        var handlerIds = ids.slice(0, ids.length - 1);
        for (let id of handlerIds) {
            Handler.removeListener(id);
        }
    }
    FluentUICallout.unregisterHandlers = unregisterHandlers;
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
    FluentUICallout.getWindow = getWindow;
    function getWindowRect() {
        var rect = {
            width: window.innerWidth,
            height: window.innerHeight,
            top: 0,
            left: 0
        };
        return rect;
    }
    FluentUICallout.getWindowRect = getWindowRect;
    ;
})(FluentUICallout || (FluentUICallout = {}));
window['FluentUICallout'] = FluentUICallout || {};
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIDocumentCard;
(function (FluentUIDocumentCard) {
    class CardTitleMap {
        stateChanged() {
            this.dotnet.invokeMethodAsync("UpdateTitle", this.state.truncatedTitleFirstPiece, this.state.truncatedTitleSecondPiece);
        }
    }
    class CardTitleState {
        constructor(shouldTruncate) {
            this.needMeasurement = !!shouldTruncate;
        }
    }
    const cardTitles = new Array();
    function getElement(id) {
        for (var i = 0; i < cardTitles.length; i++) {
            if (cardTitles[i].id === id) {
                return cardTitles[i];
            }
        }
        return null;
    }
    FluentUIDocumentCard.getElement = getElement;
    function addElement(id, element, dotnet, shouldTruncate, orgTitle) {
        let title = new CardTitleMap();
        title.state = new CardTitleState(shouldTruncate);
        title.state.originalTitle = orgTitle;
        title.state.previousTitle = orgTitle;
        title.id = id;
        title.element = element;
        title.dotnet = dotnet;
        title.state.watchResize = shouldTruncate;
        title.resizeFunction = e => {
            if (!title.state.watchResize) {
                return;
            }
            title.state.watchResize = false;
            setTimeout(() => {
                console.log('resize');
                title.dotnet.invokeMethodAsync("UpdateneedMeasurement");
                title.state.truncatedTitleFirstPiece = '';
                title.state.truncatedTitleSecondPiece = '';
                truncateTitle(title);
                title.state.watchResize = true;
            }, 500);
        };
        window.addEventListener('resize', title.resizeFunction);
        cardTitles.push(title);
    }
    FluentUIDocumentCard.addElement = addElement;
    function removelement(id) {
        let index = -1;
        for (let i = 0; i < cardTitles.length; i++) {
            if (cardTitles[i].id === id) {
                index = i;
                break;
            }
        }
        if (index >= 0) {
            let title = cardTitles[index];
            window.removeEventListener('resize', title.resizeFunction);
            cardTitles.splice(index, 1);
        }
    }
    FluentUIDocumentCard.removelement = removelement;
    function initInternal(title) {
        if (title.state.needMeasurement) {
            requestAnimationFrame(time => {
                truncateTitle(title);
            });
        }
    }
    function initTitle(id, element, dotnet, shouldTruncate, orgTitle) {
        let title = getElement(id);
        if (title === null) {
            addElement(id, element, dotnet, shouldTruncate, orgTitle);
            title = getElement(id);
            initInternal(title);
        }
    }
    FluentUIDocumentCard.initTitle = initTitle;
    function truncateTitle(cardTitle) {
        if (!cardTitle) {
            return;
        }
        const TRUNCATION_VERTICAL_OVERFLOW_THRESHOLD = 5;
        let el = document.getElementById(cardTitle.id);
        const style = getComputedStyle(el);
        if (style.width && style.lineHeight && style.height) {
            const { clientWidth, scrollWidth } = el;
            const lines = Math.floor((parseInt(style.height, 10) + TRUNCATION_VERTICAL_OVERFLOW_THRESHOLD) / parseInt(style.lineHeight, 10));
            const overFlowRate = scrollWidth / (parseInt(style.width, 10) * lines);
            if (overFlowRate > 1) {
                const truncatedLength = cardTitle.state.originalTitle.length / overFlowRate - 3 /** Saved for separator */;
                cardTitle.state.truncatedTitleFirstPiece = cardTitle.state.originalTitle.slice(0, truncatedLength / 2);
                cardTitle.state.truncatedTitleSecondPiece = cardTitle.state.originalTitle.slice(cardTitle.state.originalTitle.length - truncatedLength / 2);
                cardTitle.stateChanged();
            }
        }
    }
})(FluentUIDocumentCard || (FluentUIDocumentCard = {}));
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIFocusTrapZone;
(function (FluentUIFocusTrapZone) {
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
                if (!FluentUIBaseComponent.elementContains(this._props.rootElement, relatedTarget)) {
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
            if (!disableFirstFocus && !FluentUIBaseComponent.elementContains(rootElement, this._previouslyFocusedElementOutsideTrapZone)) {
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
                (FluentUIBaseComponent.elementContains(rootElement, activeElement) || activeElement === document.body)) {
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
                FluentUIBaseComponent.elementContains(rootElement, this._previouslyFocusedElementInTrapZone)) {
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
    FluentUIFocusTrapZone.register = register;
    function unregister(id) {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.unRegister();
        }
        delete focusTrapZones[id];
    }
    FluentUIFocusTrapZone.unregister = unregister;
    function updateProps(id, props) {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.updateProps(props);
        }
    }
    FluentUIFocusTrapZone.updateProps = updateProps;
    function focus(id) {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.focus();
        }
    }
    FluentUIFocusTrapZone.focus = focus;
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
})(FluentUIFocusTrapZone || (FluentUIFocusTrapZone = {}));
window['FluentUIFocusTrapZone'] = FluentUIFocusTrapZone || {};
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIFocusZone;
(function (FluentUIFocusZone) {
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
    FluentUIFocusZone.register = register;
    function unregister(id) {
        let focusZone = allInstances[id];
        if (focusZone) {
            focusZone.unRegister();
        }
        delete allInstances[id];
    }
    FluentUIFocusZone.unregister = unregister;
    function updateFocusZone(id, props) {
        let focusZone = allInstances[id];
        if (focusZone) {
            focusZone.updateFocusZone(props);
        }
    }
    FluentUIFocusZone.updateFocusZone = updateFocusZone;
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
                    else if (window.FluentUIBaseComponent.isElementFocusSubZone(ev.target)) {
                        if (!this.focusElement(window.FluentUIBaseComponent.getNextElement(ev.target, ev.target.firstChild, true))) {
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
                                    const tabWithDirection = window.FluentUIBaseComponent.getRTL() ? !ev.shiftKey : ev.shiftKey;
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
                            if (this._root && firstChild && this.focusElement(window.FluentUIBaseComponent.getNextElement(this._root, firstChild, true))) {
                                break;
                            }
                            return;
                        case 35 /* end */:
                            if (this._isElementInput(ev.target) && !this._shouldInputLoseFocus(ev.target, true)) {
                                return;
                            }
                            const lastChild = this._root && this._root.lastChild;
                            if (this._root && this.focusElement(window.FluentUIBaseComponent.getPreviousElement(this._root, lastChild, true, true, true))) {
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
                        if (window.FluentUIBaseComponent.isElementTabbable(parentElement) && this._isImmediateDescendantOfZone(parentElement)) {
                            newActiveElement = parentElement;
                            break;
                        }
                        parentElement = window.FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
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
                    target = window.FluentUIBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
                }
                while (path.length) {
                    target = path.pop();
                    if (target && window.FluentUIBaseComponent.isElementTabbable(target)) {
                        this._setActiveElement(target, true);
                    }
                    if (window.FluentUIBaseComponent.isElementFocusZone(target)) {
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
                const windowElement = window.FluentUIBaseComponent.getWindow(this._root);
                let parentElement = window.FluentUIBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
                while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                    if (window.FluentUIBaseComponent.isElementFocusZone(parentElement)) {
                        this._isInnerZone = true;
                        break;
                    }
                    parentElement = window.FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                }
                if (!this._isInnerZone) {
                    outerZones.add(this);
                }
                if (windowElement && outerZones.size === 1) {
                    _disposeGlobalKeyDownListener = window.FluentUIBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
                }
                this._disposables.push(window.FluentUIBaseComponent.on(this._root, 'blur', this._onBlur, true));
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
            const windowElement = window.FluentUIBaseComponent.getWindow(this._root);
            let parentElement = window.FluentUIBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                if (window.FluentUIBaseComponent.isElementFocusZone(parentElement)) {
                    this._isInnerZone = true;
                    break;
                }
                parentElement = window.FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            if (!this._isInnerZone) {
                outerZones.add(this);
            }
            if (windowElement && outerZones.size === 1) {
                _disposeGlobalKeyDownListener = window.FluentUIBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
            }
            this._disposables.push(window.FluentUIBaseComponent.on(this._root, 'blur', this._onBlur, true));
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
                    window.FluentUIBaseComponent.elementContains(this._root, this._activeElement) &&
                    window.FluentUIBaseComponent.isElementTabbable(this._activeElement)) {
                    this._activeElement.focus();
                    return true;
                }
                else {
                    const firstChild = this._root.firstChild;
                    return this.focusElement(window.FluentUIBaseComponent.getNextElement(this._root, firstChild, true));
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
                if (this._activeElement && !window.FluentUIBaseComponent.elementContains(element, this._activeElement)) {
                    this._activeElement = null;
                }
            }
            // If active element changes state to disabled, set it to null.
            // Otherwise, we lose keyboard accessibility to other elements in focus zone.
            if (this._activeElement && !window.FluentUIBaseComponent.isElementTabbable(this._activeElement)) {
                this._activeElement = null;
            }
            const childNodes = element && element.children;
            for (let childIndex = 0; childNodes && childIndex < childNodes.length; childIndex++) {
                const child = childNodes[childIndex];
                if (!window.FluentUIBaseComponent.isElementFocusZone(child)) {
                    // If the item is explicitly set to not be focusable then TABINDEX needs to be set to -1.
                    if (child.getAttribute && child.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'false') {
                        child.setAttribute(TABINDEX, '-1');
                    }
                    if (window.FluentUIBaseComponent.isElementTabbable(child)) {
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
            let parentElement = window.FluentUIBaseComponent.getParent(element, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== this._root && parentElement !== document.body) {
                if (window.FluentUIBaseComponent.isElementFocusZone(parentElement)) {
                    return parentElement;
                }
                parentElement = window.FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            return parentElement;
        }
        _setActiveElement(element, forceAlignment) {
            const previousActiveElement = this._activeElement;
            this._activeElement = element;
            if (previousActiveElement) {
                if (window.FluentUIBaseComponent.isElementFocusZone(previousActiveElement)) {
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
            if (window.FluentUIBaseComponent.isElementFocusZone(rootElement)) {
                return allInstances[rootElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
            }
            let child = rootElement.firstElementChild;
            while (child) {
                if (window.FluentUIBaseComponent.isElementFocusZone(child)) {
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
                    window.FluentUIBaseComponent.raiseClick(target);
                    return true;
                }
                target = window.FluentUIBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
            } while (target !== this._root);
            return false;
        }
        /**
        * Returns true if the element is a descendant of the FocusZone through a React portal.
        */
        _portalContainsElement(element) {
            // This might break our control when used inside a Layer...
            return false;
            //return element && !!this._root && FluentUIBaseComponent portalContainsElement(element, this._root.current);
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
            return !!this._focusZoneProps.checkForNoWrap ? window.FluentUIBaseComponent.shouldWrapFocus(element, noWrapDataAttribute) : true;
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
                element = (isForward ? window.FluentUIBaseComponent.getNextElement(this._root, element) : window.FluentUIBaseComponent.getPreviousElement(this._root, element));
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
                    return this.focusElement(window.FluentUIBaseComponent.getNextElement(this._root, this._root.firstElementChild, true));
                }
                else {
                    return this.focusElement(window.FluentUIBaseComponent.getPreviousElement(this._root, this._root.lastElementChild, true, true, true));
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
            if (this._moveFocus(window.FluentUIBaseComponent.getRTL(), (activeRect, targetRect) => {
                let distance = -1;
                let topBottomComparison;
                if (window.FluentUIBaseComponent.getRTL()) {
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
            if (this._moveFocus(!window.FluentUIBaseComponent.getRTL(), (activeRect, targetRect) => {
                let distance = -1;
                let topBottomComparison;
                if (window.FluentUIBaseComponent.getRTL()) {
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
})(FluentUIFocusZone || (FluentUIFocusZone = {}));
//interface Window {
//    FluentUIFocusZone: typeof FluentUIFocusZone
//}
//window.FluentUIFocusZone = FluentUIFocusZone;
window['FluentUIFocusZone'] = FluentUIFocusZone || {};
//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
// /// <reference path="../../FluentUI.FocusTrapZone/wwwroot/focusTrapZone.ts" />
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIList;
(function (FluentUIList) {
    var _lastId = 0;
    var cachedLists = new Map();
    class List {
        constructor(component, spacerBefore, spacerAfter) {
            this.cachedSizes = new Map();
            this.averageHeight = 40;
            this.id = _lastId++;
            this.component = component;
            //this.surfaceElement = rootElement.children.item(0) as HTMLElement;
            this.scrollElement = FluentUIBaseComponent.findScrollableParent(spacerBefore);
            // get initial width
            this.component.invokeMethodAsync('ResizeHandler', this.scrollElement.clientWidth);
            this.events = new FluentUIBaseComponent.EventGroup(this);
            this.events.on(window, 'resize', this.resize);
            this.rootElement = spacerBefore.parentElement;
            //this.scrollElement = scrollElement;
            this.spacerBefore = spacerBefore;
            this.spacerAfter = spacerAfter;
            const rootMargin = 50;
            this.intersectionObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach((entry) => {
                    var _a;
                    if (!entry.isIntersecting) {
                        return;
                    }
                    const spacerBeforeRect = this.spacerBefore.getBoundingClientRect();
                    const spacerAfterRect = this.spacerAfter.getBoundingClientRect();
                    const spacerSeparation = spacerAfterRect.top - spacerBeforeRect.bottom;
                    const containerSize = (_a = entry.rootBounds) === null || _a === void 0 ? void 0 : _a.height;
                    if (entry.target === this.spacerBefore) {
                        component.invokeMethodAsync('OnBeforeSpacerVisible', entry.intersectionRect.top - entry.boundingClientRect.top, spacerSeparation, containerSize);
                    }
                    else if (entry.target === this.spacerAfter && this.spacerAfter.offsetHeight > 0) {
                        // When we first start up, both the "before" and "after" spacers will be visible, but it's only relevant to raise a
                        // single event to load the initial data. To avoid raising two events, skip the one for the "after" spacer if we know
                        // it's meaningless to talk about any overlap into it.
                        component.invokeMethodAsync('OnAfterSpacerVisible', entry.boundingClientRect.bottom - entry.intersectionRect.bottom, spacerSeparation, containerSize);
                    }
                });
            }, {
                root: this.scrollElement, rootMargin: `${rootMargin}px`
            });
            this.intersectionObserver.observe(this.spacerBefore);
            this.intersectionObserver.observe(this.spacerAfter);
            // After each render, refresh the info about intersections
            this.mutationObserverBefore = new MutationObserver(() => {
                this.intersectionObserver.unobserve(this.spacerBefore);
                this.intersectionObserver.observe(this.spacerBefore);
            });
            this.mutationObserverBefore.observe(this.spacerBefore, { attributes: true });
            this.mutationObserverAfter = new MutationObserver(() => {
                this.intersectionObserver.unobserve(this.spacerAfter);
                this.intersectionObserver.observe(this.spacerAfter);
            });
            this.mutationObserverAfter.observe(this.spacerAfter, { attributes: true });
        }
        resize(ev) {
            this.component.invokeMethodAsync('ResizeHandler', this.scrollElement.clientWidth);
        }
        disconnect() {
            this.events.off(window, 'resize', this.resize);
            this.events.dispose();
            this.mutationObserverBefore.disconnect();
            this.mutationObserverAfter.disconnect();
            this.intersectionObserver.unobserve(this.spacerBefore);
            this.intersectionObserver.unobserve(this.spacerAfter);
            this.intersectionObserver.disconnect();
        }
        getAverageHeight() {
            let calculate = false;
            let averageHeight = 0;
            let newItems = {};
            for (let i = 0; i < this.surfaceElement.children.length; i++) {
                let item = this.surfaceElement.children.item(i);
                let index = item.getAttribute("data-item-index");
                if (index != null && !this.cachedSizes.has(index) && this.cachedSizes.get(index) != item.clientHeight) {
                    this.cachedSizes.set(index, item.clientHeight);
                    newItems[index] = item.clientHeight;
                    calculate = true;
                }
            }
            if (calculate) {
                this.component.invokeMethodAsync("UpdateHeightCache", newItems);
                averageHeight = [...this.cachedSizes.values()].reduce((p, c, i, a) => p + c) / this.cachedSizes.size;
            }
            return averageHeight;
        }
        updateItemHeights() {
        }
    }
    function getInitialAverageHeight(id) {
        let list = cachedLists.get(id);
        if (list == null) {
            return 0;
        }
        else {
            return list.getAverageHeight();
        }
    }
    FluentUIList.getInitialAverageHeight = getInitialAverageHeight;
    function initialize(component, spacerBefore, spacerAfter, reset = false) {
        let list = new List(component, spacerBefore, spacerAfter);
        cachedLists.set(list.id, list);
        return list.id;
    }
    FluentUIList.initialize = initialize;
    function removeList(id) {
        let list = cachedLists.get(id);
        list.disconnect();
        cachedLists.delete(id);
    }
    FluentUIList.removeList = removeList;
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
    FluentUIList.getViewport = getViewport;
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
})(FluentUIList || (FluentUIList = {}));
window['FluentUIList'] = FluentUIList || {};
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIDetailsList;
(function (FluentUIDetailsList) {
    const MOUSEDOWN_PRIMARY_BUTTON = 0; // for mouse down event we are using ev.button property, 0 means left button
    const MOUSEMOVE_PRIMARY_BUTTON = 1; // for mouse move event we are using ev.buttons property, 1 means left button
    const detailHeaders = new Map();
    function registerDetailsHeader(dotNet, root) {
        let detailHeader = new DetailsHeader(dotNet, root);
        detailHeaders.set(dotNet._id, detailHeader);
    }
    FluentUIDetailsList.registerDetailsHeader = registerDetailsHeader;
    function unregisterDetailsHeader(dotNet) {
        let detailHeader = detailHeaders.get(dotNet._id);
        detailHeader.dispose();
        detailHeaders.delete(dotNet._id);
    }
    FluentUIDetailsList.unregisterDetailsHeader = unregisterDetailsHeader;
    class DetailsHeader {
        constructor(dotNet, root) {
            this._onRootMouseDown = (ev) => __awaiter(this, void 0, void 0, function* () {
                const columnIndexAttr = ev.target.getAttribute('data-sizer-index');
                const columnIndex = Number(columnIndexAttr);
                if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                    // Ignore anything except the primary button.
                    return;
                }
                yield this.dotNet.invokeMethodAsync("OnSizerMouseDown", columnIndex, ev.clientX);
                ev.preventDefault();
                ev.stopPropagation();
            });
            this._onRootDblClick = (ev) => __awaiter(this, void 0, void 0, function* () {
                const columnIndexAttr = ev.target.getAttribute('data-sizer-index');
                const columnIndex = Number(columnIndexAttr);
                if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                    // Ignore anything except the primary button.
                    return;
                }
                yield this.dotNet.invokeMethodAsync("OnDoubleClick", columnIndex);
            });
            this.dotNet = dotNet;
            this.root = root;
            this.events = new FluentUIBaseComponent.EventGroup(this);
            this.events.on(root, 'mousedown', this._onRootMouseDown);
            this.events.on(root, 'dblclick', this._onRootDblClick);
        }
        dispose() {
            this.events.dispose();
        }
    }
})(FluentUIDetailsList || (FluentUIDetailsList = {}));
window['FluentUIDetailsList'] = FluentUIDetailsList || {};
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUIMarqueeSelection;
(function (FluentUIMarqueeSelection) {
    function getDistanceBetweenPoints(point1, point2) {
        const left1 = point1.left || 0;
        const top1 = point1.top || 0;
        const left2 = point2.left || 0;
        const top2 = point2.top || 0;
        let distance = Math.sqrt(Math.pow(left1 - left2, 2) + Math.pow(top1 - top2, 2));
        return distance;
    }
    FluentUIMarqueeSelection.getDistanceBetweenPoints = getDistanceBetweenPoints;
    const SCROLL_ITERATION_DELAY = 16;
    const SCROLL_GUTTER = 100;
    const MAX_SCROLL_VELOCITY = 15;
    function getRect(element) {
        let rect;
        if (element) {
            if (element === window) {
                rect = {
                    left: 0,
                    top: 0,
                    width: window.innerWidth,
                    height: window.innerHeight,
                    right: window.innerWidth,
                    bottom: window.innerHeight,
                };
            }
            else if (element.getBoundingClientRect) {
                rect = element.getBoundingClientRect();
            }
        }
        return rect;
    }
    FluentUIMarqueeSelection.getRect = getRect;
    class AutoScroll {
        constructor(element) {
            this._events = new FluentUIBaseComponent.EventGroup(this);
            this._scrollableParent = FluentUIBaseComponent.findScrollableParent(element);
            this._incrementScroll = this._incrementScroll.bind(this);
            this._scrollRect = getRect(this._scrollableParent);
            // tslint:disable-next-line:no-any
            if (this._scrollableParent === window) {
                this._scrollableParent = document.body;
            }
            if (this._scrollableParent) {
                this._events.on(window, 'mousemove', this._onMouseMove, true);
                this._events.on(window, 'touchmove', this._onTouchMove, true);
            }
        }
        dispose() {
            this._events.dispose();
            this._stopScroll();
        }
        _onMouseMove(ev) {
            this._computeScrollVelocity(ev);
        }
        _onTouchMove(ev) {
            if (ev.touches.length > 0) {
                this._computeScrollVelocity(ev);
            }
        }
        _computeScrollVelocity(ev) {
            if (!this._scrollRect) {
                return;
            }
            let clientX;
            let clientY;
            if ('clientX' in ev) {
                clientX = ev.clientX;
                clientY = ev.clientY;
            }
            else {
                clientX = ev.touches[0].clientX;
                clientY = ev.touches[0].clientY;
            }
            let scrollRectTop = this._scrollRect.top;
            let scrollRectLeft = this._scrollRect.left;
            let scrollClientBottom = scrollRectTop + this._scrollRect.height - SCROLL_GUTTER;
            let scrollClientRight = scrollRectLeft + this._scrollRect.width - SCROLL_GUTTER;
            // variables to use for alternating scroll direction
            let scrollRect;
            let clientDirection;
            let scrollClient;
            // if either of these conditions are met we are scrolling vertically else horizontally
            if (clientY < scrollRectTop + SCROLL_GUTTER || clientY > scrollClientBottom) {
                clientDirection = clientY;
                scrollRect = scrollRectTop;
                scrollClient = scrollClientBottom;
                this._isVerticalScroll = true;
            }
            else {
                clientDirection = clientX;
                scrollRect = scrollRectLeft;
                scrollClient = scrollClientRight;
                this._isVerticalScroll = false;
            }
            // calculate scroll velocity and direction
            if (clientDirection < scrollRect + SCROLL_GUTTER) {
                this._scrollVelocity = Math.max(-MAX_SCROLL_VELOCITY, -MAX_SCROLL_VELOCITY * ((SCROLL_GUTTER - (clientDirection - scrollRect)) / SCROLL_GUTTER));
            }
            else if (clientDirection > scrollClient) {
                this._scrollVelocity = Math.min(MAX_SCROLL_VELOCITY, MAX_SCROLL_VELOCITY * ((clientDirection - scrollClient) / SCROLL_GUTTER));
            }
            else {
                this._scrollVelocity = 0;
            }
            if (this._scrollVelocity) {
                this._startScroll();
            }
            else {
                this._stopScroll();
            }
        }
        _startScroll() {
            if (!this._timeoutId) {
                this._incrementScroll();
            }
        }
        _incrementScroll() {
            if (this._scrollableParent) {
                if (this._isVerticalScroll) {
                    this._scrollableParent.scrollTop += Math.round(this._scrollVelocity);
                }
                else {
                    this._scrollableParent.scrollLeft += Math.round(this._scrollVelocity);
                }
            }
            this._timeoutId = setTimeout(this._incrementScroll, SCROLL_ITERATION_DELAY);
        }
        _stopScroll() {
            if (this._timeoutId) {
                clearTimeout(this._timeoutId);
                delete this._timeoutId;
            }
        }
    }
    FluentUIMarqueeSelection.AutoScroll = AutoScroll;
    //class Handler {
    //    static objectListeners: Map<DotNetReferenceType, Map<string, EventParams>> = new Map<DotNetReferenceType, Map<string, EventParams>>();
    //    static addListener(ref: DotNetReferenceType, element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): void {
    //        let listeners: Map<string, EventParams>;
    //        if (this.objectListeners.has(ref)) {
    //            listeners = this.objectListeners.get(ref);
    //        } else {
    //            listeners = new Map<string, EventParams>();
    //            this.objectListeners.set(ref, listeners);
    //        }
    //        element.addEventListener(event, handler, capture);
    //        listeners.set(event, { capture: capture, event: event, handler: handler, element: element });
    //    }
    //    static removeListener(ref: DotNetReferenceType, event: string): void {
    //        if (this.objectListeners.has(ref)) {
    //            let listeners = this.objectListeners.get(ref);
    //            if (listeners.has(event)) {
    //                var handler = listeners.get(event);
    //                handler.element.removeEventListener(handler.event, handler.handler, handler.capture);
    //            }
    //            listeners.delete[event];
    //        }
    //    }
    //}
    const marqueeSelections = new Map();
    function registerMarqueeSelection(dotNet, root, props) {
        let marqueeSelection = new MarqueeSelection(dotNet, root, props);
        marqueeSelections.set(dotNet._id, marqueeSelection);
    }
    FluentUIMarqueeSelection.registerMarqueeSelection = registerMarqueeSelection;
    function updateProps(dotNet, props) {
        //assume itemsource may have changed... 
        var marqueeSelection = marqueeSelections.get(dotNet._id);
        if (marqueeSelection !== null) {
            marqueeSelection.props = props;
        }
    }
    FluentUIMarqueeSelection.updateProps = updateProps;
    function unregisterMarqueeSelection(dotNet) {
        let marqueeSelection = marqueeSelections.get(dotNet._id);
        marqueeSelection.dispose();
        marqueeSelections.delete(dotNet._id);
    }
    FluentUIMarqueeSelection.unregisterMarqueeSelection = unregisterMarqueeSelection;
    const MIN_DRAG_DISTANCE = 5;
    class MarqueeSelection {
        constructor(dotNet, root, props) {
            this.onMouseDown = (ev) => __awaiter(this, void 0, void 0, function* () {
                // Ensure the mousedown is within the boundaries of the target. If not, it may have been a click on a scrollbar.
                if (this._isMouseEventOnScrollbar(ev)) {
                    return;
                }
                if (this._isInSelectionToggle(ev)) {
                    return;
                }
                if (!this.isTouch &&
                    this.props.isEnabled &&
                    !this._isDragStartInSelection(ev)) {
                    let shouldStart = yield this.dotNet.invokeMethodAsync("OnShouldStartSelectionInternal");
                    if (shouldStart) {
                        if (this.scrollableSurface && ev.button === 0 && this.root) {
                            this._selectedIndicies = {};
                            this._preservedIndicies = undefined;
                            this.events.on(window, 'mousemove', this._onAsyncMouseMove, true);
                            this.events.on(this.scrollableParent, 'scroll', this._onAsyncMouseMove);
                            this.events.on(window, 'click', this.onMouseUp, true);
                            this.autoScroll = new AutoScroll(this.root);
                            this._scrollTop = this.scrollableSurface.scrollTop;
                            this._scrollLeft = this.scrollableSurface.scrollLeft;
                            this._rootRect = this.root.getBoundingClientRect();
                            yield this._onMouseMove(ev);
                        }
                    }
                }
            });
            this.dotNet = dotNet;
            this.root = root;
            this.props = props;
            this.events = new FluentUIBaseComponent.EventGroup(this);
            this._async = new FluentUIBaseComponent.Async(this);
            this.scrollableParent = FluentUIBaseComponent.findScrollableParent(root);
            this.scrollableSurface = this.scrollableParent === window ? document.body : this.scrollableParent;
            const hitTarget = props.isDraggingConstrainedToRoot ? this.root : this.scrollableSurface;
            this.events.on(hitTarget, 'mousedown', this.onMouseDown);
            //this.events.on(hitTarget, 'touchstart', this.onTouchStart, true);
            //this.events.on(hitTarget, 'pointerdown', this.onPointerDown, true);
        }
        updateProps(props) {
            this.props = props;
            this._itemRectCache = {};
        }
        dispose() {
            if (this.autoScroll) {
                this.autoScroll.dispose();
            }
            delete this.scrollableParent;
            delete this.scrollableSurface;
            this.events.dispose();
            this._async.dispose();
        }
        _isMouseEventOnScrollbar(ev) {
            const targetElement = ev.target;
            const targetScrollbarWidth = targetElement.offsetWidth - targetElement.clientWidth;
            if (targetScrollbarWidth) {
                const targetRect = targetElement.getBoundingClientRect();
                // Check vertical scroll
                //if (getRTL(this.props.theme)) {
                //    if (ev.clientX < targetRect.left + targetScrollbarWidth) {
                //        return true;
                //    }
                //} else {
                if (ev.clientX > targetRect.left + targetElement.clientWidth) {
                    return true;
                }
                //}
                // Check horizontal scroll
                if (ev.clientY > targetRect.top + targetElement.clientHeight) {
                    return true;
                }
            }
            return false;
        }
        _getRootRect() {
            return {
                left: this._rootRect.left + (this._scrollLeft - this.scrollableSurface.scrollLeft),
                top: this._rootRect.top + (this._scrollTop - this.scrollableSurface.scrollTop),
                width: this._rootRect.width,
                height: this._rootRect.height,
            };
        }
        _onAsyncMouseMove(ev) {
            this.animationFrameRequest = window.requestAnimationFrame(() => __awaiter(this, void 0, void 0, function* () {
                yield this._onMouseMove(ev);
            }));
            ev.stopPropagation();
            ev.preventDefault();
        }
        _onMouseMove(ev) {
            return __awaiter(this, void 0, void 0, function* () {
                if (!this.autoScroll) {
                    return;
                }
                if (ev.clientX !== undefined) {
                    this._lastMouseEvent = ev;
                }
                const rootRect = this._getRootRect();
                const currentPoint = { left: ev.clientX - rootRect.left, top: ev.clientY - rootRect.top };
                if (!this._dragOrigin) {
                    this._dragOrigin = currentPoint;
                }
                if (ev.buttons !== undefined && ev.buttons === 0) {
                    this.onMouseUp(ev);
                }
                else {
                    if (this._mirroredDragRect || getDistanceBetweenPoints(this._dragOrigin, currentPoint) > MIN_DRAG_DISTANCE) {
                        if (!this._mirroredDragRect) {
                            //const { selection } = this.props;
                            if (!ev.shiftKey) {
                                yield this.dotNet.invokeMethodAsync("UnselectAll");
                                //selection.setAllSelected(false);
                            }
                            this._preservedIndicies = yield this.dotNet.invokeMethodAsync("GetSelectedIndicesAsync");
                        }
                        // We need to constrain the current point to the rootRect boundaries.
                        const constrainedPoint = this.props.isDraggingConstrainedToRoot
                            ? {
                                left: Math.max(0, Math.min(rootRect.width, this._lastMouseEvent.clientX - rootRect.left)),
                                top: Math.max(0, Math.min(rootRect.height, this._lastMouseEvent.clientY - rootRect.top)),
                            }
                            : {
                                left: this._lastMouseEvent.clientX - rootRect.left,
                                top: this._lastMouseEvent.clientY - rootRect.top,
                            };
                        this.dragRect = {
                            left: Math.min(this._dragOrigin.left || 0, constrainedPoint.left),
                            top: Math.min(this._dragOrigin.top || 0, constrainedPoint.top),
                            width: Math.abs(constrainedPoint.left - (this._dragOrigin.left || 0)),
                            height: Math.abs(constrainedPoint.top - (this._dragOrigin.top || 0)),
                        };
                        yield this._evaluateSelectionAsync(this.dragRect, rootRect);
                        this._mirroredDragRect = this.dragRect;
                        yield this.dotNet.invokeMethodAsync("SetDragRect", this.dragRect);
                        //this.setState({ dragRect });
                    }
                }
                return false;
            });
        }
        _evaluateSelectionAsync(dragRect, rootRect) {
            return __awaiter(this, void 0, void 0, function* () {
                // Break early if we don't need to evaluate.
                if (!dragRect || !this.root) {
                    return;
                }
                const allElements = this.root.querySelectorAll('[data-selection-index]');
                if (!this._itemRectCache) {
                    this._itemRectCache = {};
                }
                for (let i = 0; i < allElements.length; i++) {
                    const element = allElements[i];
                    const index = element.getAttribute('data-selection-index');
                    // Pull the memoized rectangle for the item, or the get the rect and memoize.
                    let itemRect = this._itemRectCache[index];
                    if (!itemRect) {
                        itemRect = element.getBoundingClientRect();
                        // Normalize the item rect to the dragRect coordinates.
                        itemRect = {
                            left: itemRect.left - rootRect.left,
                            top: itemRect.top - rootRect.top,
                            width: itemRect.width,
                            height: itemRect.height,
                            right: itemRect.left - rootRect.left + itemRect.width,
                            bottom: itemRect.top - rootRect.top + itemRect.height,
                        };
                        if (itemRect.width > 0 && itemRect.height > 0) {
                            this._itemRectCache[index] = itemRect;
                        }
                    }
                    if (itemRect.top < dragRect.top + dragRect.height &&
                        itemRect.bottom > dragRect.top &&
                        itemRect.left < dragRect.left + dragRect.width &&
                        itemRect.right > dragRect.left) {
                        this._selectedIndicies[index] = true;
                    }
                    else {
                        delete this._selectedIndicies[index];
                    }
                }
                // set previousSelectedIndices to be all of the selected indices from last time
                const previousSelectedIndices = this._allSelectedIndices || {};
                this._allSelectedIndices = {};
                // set all indices that are supposed to be selected in _allSelectedIndices
                for (const index in this._selectedIndicies) {
                    if (this._selectedIndicies.hasOwnProperty(index)) {
                        this._allSelectedIndices[index] = true;
                    }
                }
                if (this._preservedIndicies) {
                    for (const index of this._preservedIndicies) {
                        this._allSelectedIndices[index] = true;
                    }
                }
                // check if needs to update selection, only when current _allSelectedIndices
                // is different than previousSelectedIndices
                let needToUpdate = false;
                for (const index in this._allSelectedIndices) {
                    if (this._allSelectedIndices[index] !== previousSelectedIndices[index]) {
                        needToUpdate = true;
                        break;
                    }
                }
                if (!needToUpdate) {
                    for (const index in previousSelectedIndices) {
                        if (this._allSelectedIndices[index] !== previousSelectedIndices[index]) {
                            needToUpdate = true;
                            break;
                        }
                    }
                }
                // only update selection when needed
                if (needToUpdate) {
                    // Stop change events, clear selection to re-populate.
                    //selection.setChangeEvents(false);
                    //selection.setAllSelected(false);
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                    yield this.dotNet.invokeMethodAsync("UnselectAll"); //.then(_ => {
                    const indices = [];
                    for (const index of Object.keys(this._allSelectedIndices)) {
                        indices.push(Number(index));
                        //selection.setIndexSelected(Number(index), true, false);
                    }
                    yield this.dotNet.invokeMethodAsync("SetSelectedIndices", indices);
                    //});
                    //for (const index of Object.keys(this._allSelectedIndices!)) {
                    //    selection.setIndexSelected(Number(index), true, false);
                    //}
                    //selection.setChangeEvents(true);
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                }
            });
        }
        onMouseUp(ev) {
            this.events.off(window);
            this.events.off(this.scrollableParent, 'scroll');
            if (this.autoScroll) {
                this.autoScroll.dispose();
            }
            this.autoScroll = this._dragOrigin = this._lastMouseEvent = undefined;
            this._selectedIndicies = this._itemRectCache = undefined;
            if (this._mirroredDragRect) {
                //this.setState({
                //    dragRect: undefined,
                //});
                this._mirroredDragRect = null;
                this.dotNet.invokeMethodAsync("SetDragRect", null);
                ev.preventDefault();
                ev.stopPropagation();
            }
        }
        _isInSelectionToggle(ev) {
            let element = ev.target;
            while (element && element !== this.root) {
                if (element.getAttribute('data-selection-toggle') === 'true') {
                    return true;
                }
                element = element.parentElement;
            }
            return false;
        }
        /**
   * We do not want to start the marquee if we're trying to marquee
   * from within an existing marquee selection.
   */
        _isDragStartInSelection(ev) {
            const selectedElements = this.root.querySelectorAll('[data-is-selected]');
            for (let i = 0; i < selectedElements.length; i++) {
                const element = selectedElements[i];
                const itemRect = element.getBoundingClientRect();
                if (this._isPointInRectangle(itemRect, { left: ev.clientX, top: ev.clientY })) {
                    return true;
                }
            }
            return false;
        }
        _isPointInRectangle(rectangle, point) {
            return (!!point.top &&
                rectangle.top < point.top &&
                rectangle.bottom > point.top &&
                !!point.left &&
                rectangle.left < point.left &&
                rectangle.right > point.left);
        }
    }
})(FluentUIMarqueeSelection || (FluentUIMarqueeSelection = {}));
window['FluentUIMarqueeSelection'] = FluentUIMarqueeSelection || {};
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
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUISelectionZone;
(function (FluentUISelectionZone) {
    const SELECTION_DISABLED_ATTRIBUTE_NAME = 'data-selection-disabled';
    const SELECTION_INDEX_ATTRIBUTE_NAME = 'data-selection-index';
    const SELECTION_TOGGLE_ATTRIBUTE_NAME = 'data-selection-toggle';
    const SELECTION_INVOKE_ATTRIBUTE_NAME = 'data-selection-invoke';
    const SELECTION_INVOKE_TOUCH_ATTRIBUTE_NAME = 'data-selection-touch-invoke';
    const SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME = 'data-selection-all-toggle';
    const SELECTION_SELECT_ATTRIBUTE_NAME = 'data-selection-select';
    const selectionZones = new Map();
    function registerSelectionZone(dotNet, root, props) {
        let selectionZone = new SelectionZone(dotNet, root, props);
        selectionZones.set(dotNet._id, selectionZone);
    }
    FluentUISelectionZone.registerSelectionZone = registerSelectionZone;
    function updateProps(dotNet, props) {
        let selectionZone = selectionZones.get(dotNet._id);
        if (selectionZone !== null) {
            selectionZone.props = props;
        }
    }
    FluentUISelectionZone.updateProps = updateProps;
    function unregisterSelectionZone(dotNet) {
        let selectionZone = selectionZones.get(dotNet._id);
        selectionZone.dispose();
        selectionZones.delete(dotNet._id);
    }
    FluentUISelectionZone.unregisterSelectionZone = unregisterSelectionZone;
    let SelectionMode;
    (function (SelectionMode) {
        SelectionMode[SelectionMode["none"] = 0] = "none";
        SelectionMode[SelectionMode["single"] = 1] = "single";
        SelectionMode[SelectionMode["multiple"] = 2] = "multiple";
    })(SelectionMode = FluentUISelectionZone.SelectionMode || (FluentUISelectionZone.SelectionMode = {}));
    class SelectionZone {
        constructor(dotNet, root, props) {
            this.ignoreNextFocus = () => {
                this._handleNextFocus(false);
            };
            this._onMouseDownCapture = (ev) => {
                let target = ev.target;
                if (document.activeElement !== target && !FluentUIBaseComponent.elementContains(document.activeElement, target)) {
                    this.ignoreNextFocus();
                    return;
                }
                if (!FluentUIBaseComponent.elementContains(target, this.root)) {
                    return;
                }
                while (target !== this.root) {
                    if (this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                        this.ignoreNextFocus();
                        break;
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
            };
            this._onMouseDown = (ev) => __awaiter(this, void 0, void 0, function* () {
                this._updateModifiers(ev);
                let target = ev.target;
                const itemRoot = yield this._findItemRootAsync(target);
                // No-op if selection is disabled
                if (this._isSelectionDisabled(target)) {
                    return;
                }
                while (target !== this.root) {
                    if (this._hasAttribute(target, SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME)) {
                        break;
                    }
                    else if (itemRoot) {
                        if (this._hasAttribute(target, SELECTION_TOGGLE_ATTRIBUTE_NAME)) {
                            break;
                        }
                        else if (this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                            break;
                        }
                        else if ((target === itemRoot || this._shouldAutoSelect(target)) &&
                            !this._isShiftPressed &&
                            !this._isCtrlPressed &&
                            !this._isMetaPressed) {
                            yield this._onInvokeMouseDownAsync(ev, this._getItemIndex(itemRoot));
                            break;
                        }
                        else if (this.props.disableAutoSelectOnInputElements &&
                            (target.tagName === 'A' || target.tagName === 'BUTTON' || target.tagName === 'INPUT')) {
                            return;
                        }
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
            });
            this._onClick = (ev) => __awaiter(this, void 0, void 0, function* () {
                //const { enableTouchInvocationTarget = false } = this.props;
                this._updateModifiers(ev);
                let target = ev.target;
                const itemRoot = yield this._findItemRootAsync(target);
                const isSelectionDisabled = this._isSelectionDisabled(target);
                while (target !== this.root) {
                    if (this._hasAttribute(target, SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME)) {
                        if (!isSelectionDisabled) {
                            yield this._onToggleAllClickAsync(ev);
                        }
                        break;
                    }
                    else if (itemRoot) {
                        const index = this._getItemIndex(itemRoot);
                        if (this._hasAttribute(target, SELECTION_TOGGLE_ATTRIBUTE_NAME)) {
                            if (!isSelectionDisabled) {
                                if (this._isShiftPressed) {
                                    yield this._onItemSurfaceClickAsync(ev, index);
                                }
                                else {
                                    yield this._onToggleClickAsync(ev, index);
                                }
                            }
                            break;
                        }
                        else if ((this._isTouch &&
                            this.props.enableTouchInvocationTarget &&
                            this._hasAttribute(target, SELECTION_INVOKE_TOUCH_ATTRIBUTE_NAME)) ||
                            this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                            // Items should be invokable even if selection is disabled.
                            yield this._onInvokeClickAsync(ev, index);
                            break;
                        }
                        else if (target === itemRoot) {
                            if (!isSelectionDisabled) {
                                yield this._onItemSurfaceClickAsync(ev, index);
                            }
                            else {
                                // if selection is disabled, i.e. SelectionMode is none, then do a plain InvokeItem
                                yield this.dotNet.invokeMethodAsync("InvokeItem", index);
                            }
                            break;
                        }
                        else if (target.tagName === 'A' || target.tagName === 'BUTTON' || target.tagName === 'INPUT') {
                            return;
                        }
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
            });
            this.dotNet = dotNet;
            this.root = root;
            this.props = props;
            const win = FluentUIBaseComponent.getWindow(this.root);
            this._events = new FluentUIBaseComponent.EventGroup(this);
            this._async = new FluentUIBaseComponent.Async(this);
            this._events.on(win, 'keydown', this._updateModifiers);
            //this._events.on(document, 'click', this._findScrollParentAndTryClearOnEmptyClick);
            //this._events.on(document.body, 'touchstart', this._onTouchStartCapture, true);
            //this._events.on(document.body, 'touchend', this._onTouchStartCapture, true);
            //this._events.on(root, 'keydown', this._onKeyDown);
            //this._events.on(root, 'keydown', this._onKeyDownCapture, { capture: true });
            this._events.on(root, 'mousedown', this._onMouseDown);
            this._events.on(root, 'mousedown', this._onMouseDownCapture, { capture: true });
            this._events.on(root, 'click', this._onClick);
            //this._events.on(root, 'focus', this._onFocus, { capture: true });
            //this._events.on(root, 'doubleclick', this._onDoubleClick);
            //this._events.on(root, 'contextmenu', this._onContextMenu);
        }
        updateProps(props) {
            this.props = props;
        }
        dispose() {
            this._events.dispose();
            this._async.dispose();
        }
        _updateModifiers(ev) {
            this._isShiftPressed = ev.shiftKey;
            this._isCtrlPressed = ev.ctrlKey;
            this._isMetaPressed = ev.metaKey;
            const keyCode = ev.keyCode;
            this._isTabPressed = keyCode ? keyCode === 9 /* tab */ : false;
            //console.log('updatemodifiers');
        }
        _onInvokeMouseDownAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                // Only do work if item is not selected.
                var selected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                if (selected) {
                    return;
                }
                yield this._clearAndSelectIndexAsync(index);
            });
        }
        _clearAndSelectIndexAsync(index) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                let isAlreadySingleSelected = false;
                let selectedCount = yield this.dotNet.invokeMethodAsync("GetSelectedCount");
                if (selectedCount) {
                    var indexSelected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                    isAlreadySingleSelected = indexSelected;
                }
                if (!isAlreadySingleSelected) {
                    const isModal = this.props.isModal;
                    //await this.dotNet.invokeMethodAsync("ClearAndSelectIndex", index);
                    //selection.setChangeEvents(false);
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                    //selection.setAllSelected(false);
                    yield this.dotNet.invokeMethodAsync("SetAllSelected", false);
                    //selection.setIndexSelected(index, true, true);
                    yield this.dotNet.invokeMethodAsync("SetIndexSelected", index, true, true);
                    if (isModal || (this.props.enterModalOnTouch && this._isTouch)) {
                        yield this.dotNet.invokeMethodAsync("SetModal", true);
                        if (this._isTouch) {
                            this._setIsTouch(false);
                        }
                    }
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                    //selection.setChangeEvents(true);
                }
            });
        }
        _setIsTouch(isTouch) {
            if (this._isTouchTimeoutId) {
                this._async.clearTimeout(this._isTouchTimeoutId);
                this._isTouchTimeoutId = undefined;
            }
            this._isTouch = true;
            if (isTouch) {
                this._async.setTimeout(() => {
                    this._isTouch = false;
                }, 300);
            }
        }
        _hasAttribute(element, attributeName) {
            let isToggle = false;
            while (!isToggle && element !== this.root) {
                isToggle = element.getAttribute(attributeName) === 'true';
                element = FluentUIBaseComponent.getParent(element);
            }
            return isToggle;
        }
        _handleNextFocus(handleFocus) {
            if (this._shouldHandleFocusTimeoutId) {
                this._async.clearTimeout(this._shouldHandleFocusTimeoutId);
                this._shouldHandleFocusTimeoutId = undefined;
            }
            this._shouldHandleFocus = handleFocus;
            if (handleFocus) {
                this._async.setTimeout(() => {
                    this._shouldHandleFocus = false;
                }, 100);
            }
        }
        _findItemRootAsync(target) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                while (target !== this.root) {
                    const indexValue = target.getAttribute(SELECTION_INDEX_ATTRIBUTE_NAME);
                    const index = Number(indexValue);
                    if (indexValue !== null && index >= 0) {
                        let count = yield this.dotNet.invokeMethodAsync("GetItemsLength");
                        if (index < count) {
                            break;
                        }
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
                if (target === this.root) {
                    return undefined;
                }
                return target;
            });
        }
        _isSelectionDisabled(target) {
            if (this.props.selectionMode === SelectionMode.none) {
                return true;
            }
            while (target !== this.root) {
                if (this._hasAttribute(target, SELECTION_DISABLED_ATTRIBUTE_NAME)) {
                    return true;
                }
                target = FluentUIBaseComponent.getParent(target);
            }
            return false;
        }
        _shouldAutoSelect(element) {
            return this._hasAttribute(element, SELECTION_SELECT_ATTRIBUTE_NAME);
        }
        _getItemIndex(itemRoot) {
            return Number(itemRoot.getAttribute(SELECTION_INDEX_ATTRIBUTE_NAME));
        }
        _onToggleAllClickAsync(ev) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                const selectionMode = this.props.selectionMode;
                if (selectionMode === SelectionMode.multiple) {
                    //selection.toggleAllSelected();
                    yield this.dotNet.invokeMethodAsync("ToggleAllSelected");
                    ev.stopPropagation();
                    ev.preventDefault();
                }
            });
        }
        _onToggleClickAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                const selectionMode = this.props.selectionMode;
                //selection.setChangeEvents(false);
                yield this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                if (this.props.enterModalOnTouch && this._isTouch) { // && !selection.isIndexSelected(index) && selection.setModal) {
                    let isSelected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                    if (!isSelected) {
                        yield this.dotNet.invokeMethodAsync("SetModal", true);
                        this._setIsTouch(false);
                    }
                }
                if (selectionMode === SelectionMode.multiple) {
                    //selection.toggleIndexSelected(index);
                    yield this.dotNet.invokeMethodAsync("ToggleIndexSelected", index);
                }
                else if (selectionMode === SelectionMode.single) {
                    //const isSelected = selection.isIndexSelected(index);
                    let isSelected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                    const isModal = this.props.isModal; //selection.isModal && selection.isModal();
                    //selection.setAllSelected(false);
                    yield this.dotNet.invokeMethodAsync("SetAllSelected", false);
                    //selection.setIndexSelected(index, !isSelected, true);
                    yield this.dotNet.invokeMethodAsync("SetIndexSelected", index, !isSelected, true);
                    if (isModal) {
                        // Since the above call to setAllSelected(false) clears modal state,
                        // restore it. This occurs because the SelectionMode of the Selection
                        // may differ from the SelectionZone.
                        //selection.setModal(true);
                        yield this.dotNet.invokeMethodAsync("SetModal", true);
                    }
                }
                else {
                    //selection.setChangeEvents(true);
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                    return;
                }
                //selection.setChangeEvents(true);
                yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                ev.stopPropagation();
                // NOTE: ev.preventDefault is not called for toggle clicks, because this will kill the browser behavior
                // for checkboxes if you use a checkbox for the toggle.
            });
        }
        _onItemSurfaceClickAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                const isToggleModifierPressed = this._isCtrlPressed || this._isMetaPressed;
                const selectionMode = this.props.selectionMode;
                if (selectionMode === SelectionMode.multiple) {
                    if (this._isShiftPressed && !this._isTabPressed) {
                        //selection.selectToIndex(index, !isToggleModifierPressed);
                        yield this.dotNet.invokeMethodAsync("SelectToIndex", index, !isToggleModifierPressed);
                    }
                    else if (isToggleModifierPressed) {
                        //selection.toggleIndexSelected(index);
                        yield this.dotNet.invokeMethodAsync("ToggleIndexSelected", index);
                    }
                    else {
                        yield this._clearAndSelectIndexAsync(index);
                    }
                }
                else if (selectionMode === SelectionMode.single) {
                    yield this._clearAndSelectIndexAsync(index);
                }
            });
        }
        _onInvokeClickAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection, onItemInvoked } = this.props;
                if (this.props.onItemInvokeSet) {
                    yield this.dotNet.invokeMethodAsync("InvokeItem", index);
                    //onItemInvoked(selection.getItems()[index], index, ev.nativeEvent);
                    ev.preventDefault();
                    ev.stopPropagation();
                }
            });
        }
    }
})(FluentUISelectionZone || (FluentUISelectionZone = {}));
window['FluentUISelectionZone'] = FluentUISelectionZone || {};
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var FluentUISlider;
(function (FluentUISlider) {
    class Handler {
        static addListener(ref, element, event, handler, capture) {
            let listeners;
            if (this.objectListeners.has(ref)) {
                listeners = this.objectListeners.get(ref);
            }
            else {
                listeners = new Map();
                this.objectListeners.set(ref, listeners);
            }
            element.addEventListener(event, handler, capture);
            listeners.set(event, { capture: capture, event: event, handler: handler, element: element });
        }
        static removeListener(ref, event) {
            if (this.objectListeners.has(ref)) {
                let listeners = this.objectListeners.get(ref);
                if (listeners.has(event)) {
                    var handler = listeners.get(event);
                    handler.element.removeEventListener(handler.event, handler.handler, handler.capture);
                }
                listeners.delete[event];
            }
        }
    }
    Handler.objectListeners = new Map();
    function registerMouseOrTouchStart(slider, slideBox, sliderLine) {
        Handler.addListener(slider, slideBox, "mousedown", (ev) => { onMouseMoveOrTouchStart(slider, sliderLine, ev); }, true);
        Handler.addListener(slider, slideBox, "touchstart", (ev) => { onMouseMoveOrTouchStart(slider, sliderLine, ev); }, true);
        Handler.addListener(slider, slideBox, "keydown", (ev) => { onKeyDown(slider, ev); }, true);
    }
    FluentUISlider.registerMouseOrTouchStart = registerMouseOrTouchStart;
    function onMouseMoveOrTouchStart(slider, sliderLine, event) {
        if (event.type === 'mousedown') {
            Handler.addListener(slider, window, "mousemove", (ev) => { onMouseMoveOrTouchMove(slider, sliderLine, ev); }, true);
            Handler.addListener(slider, window, "mouseup", (ev) => { onMouseUpOrTouchEnd(slider, sliderLine, ev); }, true);
        }
        else if (event.type === 'touchstart') {
            Handler.addListener(slider, window, "touchmove", (ev) => { onMouseMoveOrTouchMove(slider, sliderLine, ev); }, true);
            Handler.addListener(slider, window, "touchend", (ev) => { onMouseUpOrTouchEnd(slider, sliderLine, ev); }, true);
        }
        onMouseMoveOrTouchMove(slider, sliderLine, event, true);
    }
    function onMouseMoveOrTouchMove(slider, sliderLine, event, suppressEventCancelation) {
        let sliderPositionRect = sliderLine.getBoundingClientRect();
        let horizontalPosition = getPosition(event, false);
        let verticalPosition = getPosition(event, true);
        slider.invokeMethodAsync("MouseOrTouchMove", sliderPositionRect, horizontalPosition, verticalPosition);
        if (suppressEventCancelation) {
            event.preventDefault();
            event.stopPropagation();
        }
    }
    function getPosition(event, vertical) {
        let currentPosition;
        switch (event.type) {
            case 'mousedown':
            case 'mousemove':
                currentPosition = !vertical ? event.clientX : event.clientY;
                break;
            case 'touchstart':
            case 'touchmove':
                currentPosition = !vertical
                    ? event.touches[0].clientX
                    : event.touches[0].clientY;
                break;
        }
        return currentPosition;
    }
    function onMouseUpOrTouchEnd(slider, sliderLine, event, suppressEventCancelation) {
        return __awaiter(this, void 0, void 0, function* () {
            yield slider.invokeMethodAsync("MouseOrTouchEnd");
            if (event.type === 'mouseup') {
                Handler.removeListener(slider, "mousemove");
                Handler.removeListener(slider, "mouseup");
            }
            else if (event.type === 'touchend') {
                Handler.removeListener(slider, "touchmove");
                Handler.removeListener(slider, "touchend");
            }
        });
    }
    function onKeyDown(slider, event) {
        let diff;
        let value;
        switch (event.which) {
            case 39 /* right */: //right arrow
            case 38 /* up */: //up arrow
                slider.invokeMethodAsync("OnKeyDown", { step: +1 });
                break;
            case 37 /* left */: //left arrow
            case 40 /* down */: //down arrow
                slider.invokeMethodAsync("OnKeyDown", { step: -1 });
                break;
            case 36 /* home */: //home
                slider.invokeMethodAsync("OnKeyDown", { min: true });
                break;
            case 35 /* end */: //end
                slider.invokeMethodAsync("OnKeyDown", { max: true });
                break;
            default:
                return;
        }
        event.preventDefault();
        event.stopPropagation();
    }
    function unregisterHandlers(ref) {
        Handler.removeListener(ref, "mousedown");
        Handler.removeListener(ref, "touchstart");
        Handler.removeListener(ref, "keydown");
    }
    FluentUISlider.unregisterHandlers = unregisterHandlers;
})(FluentUISlider || (FluentUISlider = {}));
window['FluentUISlider'] = FluentUISlider || {};
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
