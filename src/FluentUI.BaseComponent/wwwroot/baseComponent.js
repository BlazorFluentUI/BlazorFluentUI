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
//# sourceMappingURL=baseComponent.js.map