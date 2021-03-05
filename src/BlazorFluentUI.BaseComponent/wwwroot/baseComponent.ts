namespace FluentUIBaseComponent {
    const test = 12333;
    const DATA_IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    const DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';
    const DATA_IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    const FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    const FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    const IsFocusVisibleClassName = 'ms-Fabric--isFocusVisible';

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface ElementReferenceResult {
        element: HTMLElement | undefined;
        isNull: boolean;
    }

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
    const layerElements: MapSimple<HTMLElement> = {};  
    //const virtualRelationships: Map<IVirtualRelationship> = {};



    export function initializeFocusRects(): void {
        if (!(<any>window).__hasInitializeFocusRects__) {
            (<any>window).__hasInitializeFocusRects__ = true;
            window.addEventListener("mousedown", _onFocusRectMouseDown, true);
            window.addEventListener("keydown", _onFocusRectKeyDown, true);
        }
    }


    function _onFocusRectMouseDown(ev: MouseEvent) {
        if (window.document.body.classList.contains(IsFocusVisibleClassName)) {
            window.document.body.classList.remove(IsFocusVisibleClassName);
        }
    }
    function _onFocusRectKeyDown(ev: MouseEvent) {
        if (isDirectionalKeyCode(ev.which) && !window.document.body.classList.contains(IsFocusVisibleClassName)) {
            window.document.body.classList.add(IsFocusVisibleClassName);
        }
    }

    const DirectionalKeyCodes: { [key: number]: number } = {
        [KeyCodes.up]: 1,
        [KeyCodes.down]: 1,
        [KeyCodes.left]: 1,
        [KeyCodes.right]: 1,
        [KeyCodes.home]: 1,
        [KeyCodes.end]: 1,
        [KeyCodes.tab]: 1,
        [KeyCodes.pageUp]: 1,
        [KeyCodes.pageDown]: 1
    };

    function isDirectionalKeyCode(which: number): boolean {
        return !!DirectionalKeyCodes[which];
    }

    export interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }

    // Disable/enable bodyscroll for overlay

    var _bodyScrollDisabledCount: number = 0;

    export function enableBodyScroll() : void {
        if (_bodyScrollDisabledCount > 0) {

            if (_bodyScrollDisabledCount === 1) {
                document.body.classList.remove("disabledBodyScroll");
                document.body .removeEventListener('touchmove', _disableIosBodyScroll);
            }

            _bodyScrollDisabledCount--;
        }
    }

    export function disableBodyScroll(): void {
        if (!_bodyScrollDisabledCount) {
            document.body.classList.add("disabledBodyScroll");
            document.body.addEventListener('touchmove', _disableIosBodyScroll, { passive: false, capture: false });
        }

        _bodyScrollDisabledCount++;
    }

    const _disableIosBodyScroll = (event: TouchEvent) => {
        event.preventDefault();
    };

    // end

    export function getClientHeight(element: HTMLElement): number {
        if (element == null)
            return 0;
        return element.clientHeight;
    }

    export function getScrollHeight(element: HTMLElement): number {
        if (element == null)
            return 0;
        return element.scrollHeight;
    }

    export function findScrollableParent(startingElement: HTMLElement | null): HTMLElement | null {
        let el: HTMLElement | null = startingElement;

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
            el = window as any;
        }

        return el;
    }


    export function measureElement(element: HTMLElement): IRectangle {
        var rect: IRectangle = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        }
        return rect;
    }

    export function getNaturalBounds(image: HTMLImageElement): IRectangle {
        if (image && image !== null) {
            var rect: IRectangle = {
                width: image.naturalWidth,
                height: image.naturalHeight,
                left: 0,
                top: 0
            }
            return rect;
        }
        return null;
    }

    export function supportsObjectFit(): boolean {
        return window !== undefined && window.navigator.msMaxTouchPoints === undefined;
    }

    export function hasOverflow(element:HTMLElement): boolean {
        return false;
    }

    export function measureScrollWindow(element: HTMLElement): IRectangle {
        var rect: IRectangle = {
            width: element.scrollWidth,
            height: element.scrollHeight,
            top: element.scrollTop,
            left: element.scrollLeft,
            bottom: element.scrollTop + element.clientHeight,
            right: element.scrollLeft + element.clientWidth,
        }
        return rect;
    }

    interface IScrollDimensions {
        scrollHeight: number;
        scrollWidth: number;
    }

    export function measureScrollDimensions(element: HTMLElement): IScrollDimensions {
        var dimensions: IScrollDimensions = {
            scrollHeight: element.scrollHeight,
            scrollWidth: element.scrollWidth,
        }
        return dimensions;
    }

    export function measureElementRect(element: HTMLElement): IRectangle {
        if (element !== undefined && element !== null) {
            // EdgeHTML's rectangle can't be serialized for some reason.... serializes to 0 everything.   So break it apart into simple JSON.
            var rect = element.getBoundingClientRect();
            return { height: rect.height, width: rect.width, left: rect.left, right: rect.right, top: rect.top, bottom: rect.bottom };
        }
        else
            return { height: 0, width: 0, left: 0, right: 0, top: 0, bottom: 0 };
    }

    export function getWindow(element: Element): Window {
        return element.ownerDocument.defaultView;
    }
       
    export function getWindowRect(): IRectangle {
        var rect: IRectangle = {
            width: window.innerWidth,// - scrollbarwidth
            height: window.innerHeight,
            top: 0,
            left: 0
        }
        return rect;
    }

    export function getElementId(element: HTMLElement): string {
        if (element !== undefined) {
            return element.id;
        }
        return null;
    }

    interface MapSimple<T> {
        [K: string]: T;
    }

    var eventRegister: MapSimple<(ev: UIEvent) => void> = {};

    var eventElementRegister: MapSimple<[HTMLElement, (ev: UIEvent) => void]> = {};

    /* Function for Dropdown, but could apply to focusing on any element after onkeydown outside of list containing is-element-focusable items */
    export function registerKeyEventsForList(element: HTMLElement): string {
        if (element instanceof HTMLElement) {
            var guid = Guid.newGuid();
            eventElementRegister[guid] = [element, (ev: KeyboardEvent) => {
                let elementToFocus: HTMLElement;
                const containsExpandCollapseModifier = ev.altKey || ev.metaKey;
                switch (ev.keyCode) {
                    case KeyCodes.up:
                        if (containsExpandCollapseModifier) {
                            //should send a close window or something, maybe let Blazor handle it.
                        } else {
                            elementToFocus = getLastFocusable(element, element.lastChild as HTMLElement, true);
                        }
                        break;
                    case KeyCodes.down:
                        if (!containsExpandCollapseModifier) {
                            elementToFocus = getFirstFocusable(element, element.firstChild as HTMLElement, true);
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
        } else {
            return null;
        }
    }
    export function deregisterKeyEventsForList(guid: number) {
        var tuple = eventElementRegister[guid];
        if (tuple) {
            var element = tuple[0];
            var func = tuple[1];
            element.removeEventListener("keydown", func);
            eventElementRegister[guid] = null;
        }
    }


    export function registerWindowKeyDownEvent(dotnetRef: DotNetReferenceType, keyCode:string, functionName: string): string {
        var guid = Guid.newGuid();
        eventRegister[guid] = (ev: KeyboardEvent) => {
            if (ev.code == keyCode) {
                ev.preventDefault();
                ev.stopPropagation();
                dotnetRef.invokeMethodAsync(functionName, ev.code);
            }
        };
        window.addEventListener("keydown", eventRegister[guid]);
        return guid;
    }

    export function deregisterWindowKeyDownEvent(guid: number) {
        var func = eventRegister[guid];
        window.removeEventListener("keydown", func);
        eventRegister[guid] = null;
    }

    export function registerResizeEvent(dotnetRef: DotNetReferenceType, functionName: string) : string {
        var guid = Guid.newGuid();
        eventRegister[guid] = debounce((ev: UIEvent) => {
            dotnetRef.invokeMethodAsync(functionName, window.innerWidth, innerHeight);
        }, 100, { leading: true });
        window.addEventListener("resize", eventRegister[guid]);
        return guid;
    }

    export function deregisterResizeEvent(guid: number) {
        var func = eventRegister[guid];
        window.removeEventListener("resize", func);
        eventRegister[guid] = null;
    }

    class Guid {
        static newGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0,
                    v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }
    }


    var _lastId: number = 0;
    var cachedViewports: Map<number, Viewport> = new Map<number, Viewport>();

    class Viewport {
        RESIZE_DELAY = 500;
        MAX_RESIZE_ATTEMPTS = 3;

        id: number;
        component: DotNetReferenceType;
        rootElement: HTMLElement;

        viewportResizeObserver: any;

        viewport: { width: number, height: number } = { width: 0, height: 0 };
        _resizeAttempts: number;

        constructor(component: DotNetReferenceType, rootElement:HTMLElement, fireInitialViewport:boolean=false) {
            this.id = _lastId++;
            this.component = component;
            this.rootElement = rootElement;

            this._onAsyncResizeAsync = debounce(this._onAsyncResizeAsync, this.RESIZE_DELAY, { leading: true });

            this.viewportResizeObserver = new (window as any).ResizeObserver(this._onAsyncResizeAsync);
            this.viewportResizeObserver.observe(this.rootElement);

            if (fireInitialViewport) {
                this._onAsyncResizeAsync();
            }
        }

        public disconnect() {
            this.viewportResizeObserver.disconnect();
        }

        private _onAsyncResizeAsync = (): void => {
            this._updateViewportAsync();
        };

        private async _updateViewportAsync(withForceUpdate?: boolean) {
            //const { viewport } = this.state;

            const viewportElement = this.rootElement;
            const scrollElement = findScrollableParent(viewportElement) as HTMLElement;
            const scrollRect = getRect(scrollElement);
            const clientRect = getRect(viewportElement);
            const updateComponentAsync = async () => {
                if (withForceUpdate) {
                    await this.component.invokeMethodAsync("ForceUpdate");
                }
            };

            const isSizeChanged =
                (clientRect && clientRect.width) !== this.viewport!.width || (scrollRect && scrollRect.height) !== this.viewport!.height;

            if (isSizeChanged && this._resizeAttempts < this.MAX_RESIZE_ATTEMPTS && clientRect && scrollRect) {
                this._resizeAttempts++;
                this.viewport = {
                    width: clientRect.width,
                    height: scrollRect.height
                };
                await this.component.invokeMethodAsync("ViewportChanged", this.viewport);
                await this._updateViewportAsync(withForceUpdate);

            } else {
                this._resizeAttempts = 0;
                await updateComponentAsync();
            }
        };


    }

    export function addViewport(component: DotNetReferenceType, rootElement: HTMLElement, fireInitialViewport: boolean = false): number {

        let viewport: Viewport = new Viewport(component, rootElement, fireInitialViewport);
        cachedViewports.set(viewport.id, viewport);

        return viewport.id;
    }

    export function removeViewport(id: number) {
        let viewport = cachedViewports.get(id);
        viewport.disconnect();
        cachedViewports.delete(id);
    }

    export function getRect(element: HTMLElement | Window | null): IRectangle | undefined {
        let rect: IRectangle | undefined;
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
            } else if ((element as HTMLElement).getBoundingClientRect) {
                rect = (element as HTMLElement).getBoundingClientRect();
            }
        }
        return rect;
    }

    export function findElementRecursive(element: HTMLElement | null, matchFunction: (element: HTMLElement) => boolean): HTMLElement | null {
        if (!element || element === document.body) {
            return null;
        }
        return matchFunction(element) ? element : findElementRecursive(getParent(element), matchFunction);
    }

    export function elementContainsAttribute(element: HTMLElement, attribute: string): string | null {
        let elementMatch = findElementRecursive(element, (testElement: HTMLElement) => testElement.hasAttribute(attribute));
        return elementMatch && elementMatch.getAttribute(attribute);
    }


/* Focus stuff */

    /* Since elements can be stored in Blazor and we don't want to create more js files, this will hold last focused elements for restoring focus later. */
    var _lastFocus: MapSimple<HTMLElement> = {};

    export function storeLastFocusedElement(): string {
        let element = document.activeElement;
        let htmlElement = <HTMLElement>element;
        if (htmlElement) {
            let guid = Guid.newGuid();
            _lastFocus[guid] = htmlElement;
            return guid;
        }
        return null;
    }

    export function restoreLastFocus(guid: string, restoreFocus: boolean = true) {
        var htmlElement = _lastFocus[guid];
        if (htmlElement != null) {
            if (restoreFocus) {
                htmlElement.focus();
            }
            delete _lastFocus[guid];
        }
    }


    export function getActiveElement(): Element {
        return document.activeElement;
    }

    export function focusElement(element: HTMLElement) {
        element.focus();
    }

    export function focusFirstElementChild(element: HTMLElement, ){
        let child = this.getFirstFocusable(element,element, true );
        if (child) {
            child.focus();
        } else {
            element.focus();
        }
    }

    export function shouldWrapFocus(element: HTMLElement, noWrapDataAttribute: 'data-no-vertical-wrap' | 'data-no-horizontal-wrap'): boolean {
        return elementContainsAttribute(element, noWrapDataAttribute) === 'true' ? false : true;
    }

    export function getFocusableByIndexPath(parent: HTMLElement, path: number[]): ElementReferenceResult {
        let element = parent;
        for (const index of path) {
            const nextChild = element.children[Math.min(index, element.children.length - 1)] as HTMLElement;
            if (!nextChild) {
                break;
            }
            element = nextChild;
        }
        element = isElementTabbable(element) && isElementVisible(element) ? element : getNextElement(parent, element, true) || getPreviousElement(parent, element);

        return { element: element as HTMLElement, isNull: !element };
    }

    export function getFirstFocusable(rootElement: HTMLElement, currentElement: HTMLElement, includeElementsInFocusZones?: boolean) {
        return getNextElement(
            rootElement,
            currentElement,
            true /*checkNode*/,
            false /*suppressParentTraversal*/,
            false /*suppressChildTraversal*/,
            includeElementsInFocusZones
        );
    }

    export function getLastFocusable(rootElement: HTMLElement, currentElement: HTMLElement, includeElementsInFocusZones?: boolean) {
        return getPreviousElement(
            rootElement,
            currentElement,
            true /*checkNode*/,
            false /*suppressParentTraversal*/,
            true /*suppressChildTraversal*/,
            includeElementsInFocusZones
        );
    }

    export function isElementTabbable(element: HTMLElement, checkTabIndex?: boolean): boolean {
        // If this element is null or is disabled, it is not considered tabbable.
        if (!element || (element as HTMLButtonElement).disabled) {
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

        const result =
            !!element &&
            isFocusableAttribute !== 'false' &&
            (element.tagName === 'A' ||
                element.tagName === 'BUTTON' ||
                element.tagName === 'INPUT' ||
                element.tagName === 'TEXTAREA' ||
                isFocusableAttribute === 'true' ||
                isTabIndexSet);

        return checkTabIndex ? tabIndex !== -1 && result : result;
    }

    export function isElementVisible(element: HTMLElement | undefined | null): boolean {
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
        return (
            element.offsetHeight !== 0 ||
            element.offsetParent !== null ||
            // tslint:disable-next-line:no-any
            (element as any).isVisible === true
        ); // used as a workaround for testing.
    }

    export function focusFirstChild(rootElement: HTMLElement): boolean {

        return false;
    }

    export function getParent(child: HTMLElement, allowVirtualParents: boolean = true): HTMLElement | null {
        return child && ((allowVirtualParents && getVirtualParent(child)) || (child.parentNode && (child.parentNode as HTMLElement)));
    }

    export function addOrUpdateVirtualParent(parent: HTMLElement) {
        layerElements[parent.dataset.layerId] = parent;
    }

    export function getVirtualParent(child: HTMLElement): HTMLElement | undefined {
        let parent: HTMLElement | undefined;
        if (child && child.dataset && child.dataset.parentLayerId) {
            parent = layerElements[child.dataset.parentLayerId];
        }
        return parent;
    }
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
   


    export function elementContains(parent: HTMLElement, child: HTMLElement, allowVirtualParents: boolean = true): boolean {
        let isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    let nextParent: HTMLElement | null = getParent(child);
                    if (nextParent === parent) {
                        isContained = true;
                        break;
                    }
                    child = nextParent;
                }
            } else if (parent.contains) {
                isContained = parent.contains(child);
            }
        }
        return isContained;
    }

    export function getNextElement(rootElement: HTMLElement, currentElement: HTMLElement|null, checkNode?:boolean, suppressParentTraversal?:boolean, suppressChildTraversal?:boolean, includeElementsInFocusZones?:boolean, allowFocusRoot?:boolean, tabbable?:boolean) : HTMLElement | null {
        if (!currentElement || (currentElement === rootElement && suppressChildTraversal && !allowFocusRoot)) {
            return null;
        }

        let isCurrentElementVisible = isElementVisible(currentElement);

        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }

        // Check its children.
        if (
            !suppressChildTraversal &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))
        ) {
            const childMatch = getNextElement(
                rootElement,
                currentElement.firstElementChild as HTMLElement,
                true,
                true,
                false,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );

            if (childMatch) {
                return childMatch;
            }
        }

        if (currentElement === rootElement) {
            return null;
        }

        // Check its sibling.
        const siblingMatch = getNextElement(
            rootElement,
            currentElement.nextElementSibling as HTMLElement,
            true,
            true,
            false,
            includeElementsInFocusZones,
            allowFocusRoot,
            tabbable
        );

        if (siblingMatch) {
            return siblingMatch;
        }

        if (!suppressParentTraversal) {
            return getNextElement(
                rootElement,
                currentElement.parentElement,
                false,
                false,
                true,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );
        }

        return null;
    }

    export function getPreviousElement(rootElement: HTMLElement, currentElement: HTMLElement | null, checkNode?: boolean, suppressParentTraversal?: boolean, traverseChildren?: boolean, includeElementsInFocusZones?: boolean, allowFocusRoot?: boolean, tabbable?: boolean): HTMLElement | null {
        if (!currentElement || (!allowFocusRoot && currentElement === rootElement)) {
            return null;
        }

        let isCurrentElementVisible = isElementVisible(currentElement);

        // Check its children.
        if (
            traverseChildren &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))
        ) {
            const childMatch = getPreviousElement(
                rootElement,
                currentElement.lastElementChild as HTMLElement,
                true,
                true,
                true,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );

            if (childMatch) {
                if ((tabbable && isElementTabbable(childMatch, true)) || !tabbable) {
                    return childMatch;
                }

                const childMatchSiblingMatch = getPreviousElement(
                    rootElement,
                    childMatch.previousElementSibling as HTMLElement,
                    true,
                    true,
                    true,
                    includeElementsInFocusZones,
                    allowFocusRoot,
                    tabbable
                );
                if (childMatchSiblingMatch) {
                    return childMatchSiblingMatch;
                }

                let childMatchParent = childMatch.parentElement;

                // At this point if we have not found any potential matches
                // start looking at the rest of the subtree under the currentParent.
                // NOTE: We do not want to recurse here because doing so could
                // cause elements to get skipped.
                while (childMatchParent && childMatchParent !== currentElement) {
                    const childMatchParentMatch = getPreviousElement(
                        rootElement,
                        childMatchParent.previousElementSibling as HTMLElement,
                        true,
                        true,
                        true,
                        includeElementsInFocusZones,
                        allowFocusRoot,
                        tabbable
                    );

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
        const siblingMatch = getPreviousElement(
            rootElement,
            currentElement.previousElementSibling as HTMLElement,
            true,
            true,
            true,
            includeElementsInFocusZones,
            allowFocusRoot,
            tabbable
        );

        if (siblingMatch) {
            return siblingMatch;
        }

        // Check its parent.
        if (!suppressParentTraversal) {
            return getPreviousElement(
                rootElement,
                currentElement.parentElement,
                true,
                false,
                false,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );
        }

        return null;
    }

    export interface IPoint {
        x: number;
        y: number;
    }

    /** Raises a click event. */
    export function raiseClick(target: Element): void {
        const event = createNewEvent('MouseEvents');
        event.initEvent('click', true, true);
        target.dispatchEvent(event);
    }

    function createNewEvent(eventName: string): Event {
        let event;
        if (typeof Event === 'function') {
            // Chrome, Opera, Firefox
            event = new Event(eventName);
        } else {
            // IE
            event = document.createEvent('Event');
            event.initEvent(eventName, true, true);
        }
        return event;
    }

    export function isElementFocusZone(element?: HTMLElement): boolean {
        return !!(element && element.getAttribute && !!element.getAttribute(FOCUSZONE_ID_ATTRIBUTE));
    }

    export function isElementFocusSubZone(element?: HTMLElement): boolean {
        return !!(element && element.getAttribute && element.getAttribute(FOCUSZONE_SUB_ATTRIBUTE) === 'true');
    }

    export function on(element: Element | Window, eventName: string, callback: (ev: Event) => void, options?: boolean): () => void {
        element.addEventListener(eventName, callback, options);

        return () => element.removeEventListener(eventName, callback, options);
    }

    function _expandRect(rect: IRectangle, pagesBefore: number, pagesAfter: number): IRectangle {
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

    function _isContainedWithin(innerRect: IRectangle, outerRect: IRectangle): boolean {
        return (
            innerRect.top >= outerRect.top &&
            innerRect.left >= outerRect.left &&
            innerRect.bottom! <= outerRect.bottom! &&
            innerRect.right! <= outerRect.right!
        );

    }

    function _mergeRect(targetRect: IRectangle, newRect: IRectangle): IRectangle {
        targetRect.top = newRect.top < targetRect.top || targetRect.top === -1 ? newRect.top : targetRect.top;
        targetRect.left = newRect.left < targetRect.left || targetRect.left === -1 ? newRect.left : targetRect.left;
        targetRect.bottom = newRect.bottom! > targetRect.bottom! || targetRect.bottom === -1 ? newRect.bottom : targetRect.bottom;
        targetRect.right = newRect.right! > targetRect.right! || targetRect.right === -1 ? newRect.right : targetRect.right;
        targetRect.width = targetRect.right! - targetRect.left + 1;
        targetRect.height = targetRect.bottom! - targetRect.top + 1;

        return targetRect;
    }


    export function debounce<T extends Function>(
        func: T,
        wait?: number,
        options?: {
            leading?: boolean;
            maxWait?: number;
            trailing?: boolean;
        }
    ): ICancelable<T> & (() => void) {
        if (this._isDisposed) {
            let noOpFunction: ICancelable<T> & (() => T) = (() => {
                /** Do nothing */
            }) as ICancelable<T> & (() => T);

            noOpFunction.cancel = () => {
                return;
            };
            /* tslint:disable:no-any */
            noOpFunction.flush = (() => null) as any;
            /* tslint:enable:no-any */
            noOpFunction.pending = () => false;

            return noOpFunction;
        }

        let waitMS = wait || 0;
        let leading = false;
        let trailing = true;
        let maxWait: number | null = null;
        let lastCallTime = 0;
        let lastExecuteTime = new Date().getTime();
        let lastResult: T;
        // tslint:disable-next-line:no-any
        let lastArgs: any[];
        let timeoutId: number | null = null;

        if (options && typeof options.leading === 'boolean') {
            leading = options.leading;
        }

        if (options && typeof options.trailing === 'boolean') {
            trailing = options.trailing;
        }

        if (options && typeof options.maxWait === 'number' && !isNaN(options.maxWait)) {
            maxWait = options.maxWait;
        }

        let markExecuted = (time: number) => {
            if (timeoutId) {
                this.clearTimeout(timeoutId);
                timeoutId = null;
            }
            lastExecuteTime = time;
        };

        let invokeFunction = (time: number) => {
            markExecuted(time);
            lastResult = func.apply(this._parent, lastArgs);
        };

        let callback = (userCall?: boolean) => {
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
                } else {
                    waitLength = Math.min(waitLength, maxWait - maxWaitDelta);
                }
            }

            if (delta >= waitMS || maxWaitExpired || executeImmediately) {
                invokeFunction(now);
            } else if ((timeoutId === null || !userCall) && trailing) {
                timeoutId = this.setTimeout(callback, waitLength);
            }

            return lastResult;
        };

        let pending = (): boolean => {
            return !!timeoutId;
        };

        let cancel = (): void => {
            if (pending()) {
                // Mark the debounced function as having executed
                markExecuted(new Date().getTime());
            }
        };

        let flush = (): T => {
            if (pending()) {
                invokeFunction(new Date().getTime());
            }

            return lastResult;
        };

        // tslint:disable-next-line:no-any
        let resultFunction: ICancelable<T> & (() => T) = ((...args: any[]) => {
            lastArgs = args;
            return callback(true);
        }) as ICancelable<T> & (() => T);

        resultFunction.cancel = cancel;
        resultFunction.flush = flush;
        resultFunction.pending = pending;

        return resultFunction;
    }

    type ICancelable<T> = {
        flush: () => T;
        cancel: () => void;
        pending: () => boolean;
    };

    export const enum KeyCodes {
        backspace = 8,
        tab = 9,
        enter = 13,
        shift = 16,
        ctrl = 17,
        alt = 18,
        pauseBreak = 19,
        capslock = 20,
        escape = 27,
        space = 32,
        pageUp = 33,
        pageDown = 34,
        end = 35,
        home = 36,
        left = 37,
        up = 38,
        right = 39,
        down = 40,
        insert = 45,
        del = 46,
        zero = 48,
        one = 49,
        two = 50,
        three = 51,
        four = 52,
        five = 53,
        six = 54,
        seven = 55,
        eight = 56,
        nine = 57,
        a = 65,
        b = 66,
        c = 67,
        d = 68,
        e = 69,
        f = 70,
        g = 71,
        h = 72,
        i = 73,
        j = 74,
        k = 75,
        l = 76,
        m = 77,
        n = 78,
        o = 79,
        p = 80,
        q = 81,
        r = 82,
        s = 83,
        t = 84,
        u = 85,
        v = 86,
        w = 87,
        x = 88,
        y = 89,
        z = 90,
        leftWindow = 91,
        rightWindow = 92,
        select = 93,
        zero_numpad = 96,
        one_numpad = 97,
        two_numpad = 98,
        three_numpad = 99,
        four_numpad = 100,
        five_numpad = 101,
        six_numpad = 102,
        seven_numpad = 103,
        eight_numpad = 104,
        nine_numpad = 105,
        multiply = 106,
        add = 107,
        subtract = 109,
        decimalPoint = 110,
        divide = 111,
        f1 = 112,
        f2 = 113,
        f3 = 114,
        f4 = 115,
        f5 = 116,
        f6 = 117,
        f7 = 118,
        f8 = 119,
        f9 = 120,
        f10 = 121,
        f11 = 122,
        f12 = 123,
        numlock = 144,
        scrollLock = 145,
        semicolon = 186,
        equalSign = 187,
        comma = 188,
        dash = 189,
        period = 190,
        forwardSlash = 191,
        graveAccent = 192,
        openBracket = 219,
        backSlash = 220,
        closeBracket = 221,
        singleQuote = 222
    }

    const RTL_LOCAL_STORAGE_KEY = 'isRTL';
    let _isRTL: boolean | undefined;

    export function getRTL(): boolean {
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

    export function setRTL(isRTL: boolean, persistSetting: boolean = false): void {
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

    export function getItem(key: string): string | null {
        let result = null;
        try {
            result = window.sessionStorage.getItem(key);
        } catch (e) {
            /* Eat the exception */
        }
        return result;
    }

    export function setItem(key: string, data: string): void {
        try {
            window.sessionStorage.setItem(key, data);
        } catch (e) {
            /* Eat the exception */
        }
    }

    export class Async {
        private _timeoutIds: { [id: number]: boolean } | null = null;
        private _immediateIds: { [id: number]: boolean } | null = null;
        private _intervalIds: { [id: number]: boolean } | null = null;
        private _animationFrameIds: { [id: number]: boolean } | null = null;
        private _isDisposed: boolean;
        private _parent: object | null;
        // tslint:disable-next-line:no-any
        private _onErrorHandler: ((e: any) => void) | undefined;
        private _noop: () => void;
        // tslint:disable-next-line:no-any
        constructor(parent?: object, onError?: (e: any) => void) {
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
        public dispose(): void {
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
        public setTimeout(callback: () => void, duration: number): number {
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
                    } catch (e) {
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
        public clearTimeout(id: number): void {
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
        public setImmediate(callback: () => void, targetElement?: Element | null): number {
            let immediateId = 0;
            const win = getWindow(targetElement)!;

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
                    } catch (e) {
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
        public clearImmediate(id: number, targetElement?: Element | null): void {
            const win = getWindow(targetElement)!;

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
        public setInterval(callback: () => void, duration: number): number {
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
                    } catch (e) {
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
        public clearInterval(id: number): void {
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
        public throttle<T extends Function>(
            func: T,
            wait?: number,
            options?: {
                leading?: boolean;
                trailing?: boolean;
            },
        ): T | (() => void) {
            if (this._isDisposed) {
                return this._noop;
            }

            let waitMS = wait || 0;
            let leading = true;
            let trailing = true;
            let lastExecuteTime = 0;
            let lastResult: T;
            // tslint:disable-next-line:no-any
            let lastArgs: any[];
            let timeoutId: number | null = null;

            if (options && typeof options.leading === 'boolean') {
                leading = options.leading;
            }

            if (options && typeof options.trailing === 'boolean') {
                trailing = options.trailing;
            }

            let callback = (userCall?: boolean) => {
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
                } else if (timeoutId === null && trailing) {
                    timeoutId = this.setTimeout(callback, waitLength);
                }

                return lastResult;
            };

            // tslint:disable-next-line:no-any
            let resultFunction: () => T = (...args: any[]) => {
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
        public debounce<T extends Function>(
            func: T,
            wait?: number,
            options?: {
                leading?: boolean;
                maxWait?: number;
                trailing?: boolean;
            },
        ): ICancelable<T> & (() => void) {
            if (this._isDisposed) {
                let noOpFunction: ICancelable<T> & (() => T) = (() => {
                    /** Do nothing */
                }) as ICancelable<T> & (() => T);

                noOpFunction.cancel = () => {
                    return;
                };
                /* tslint:disable:no-any */
                noOpFunction.flush = (() => null) as any;
                /* tslint:enable:no-any */
                noOpFunction.pending = () => false;

                return noOpFunction;
            }

            let waitMS = wait || 0;
            let leading = false;
            let trailing = true;
            let maxWait: number | null = null;
            let lastCallTime = 0;
            let lastExecuteTime = new Date().getTime();
            let lastResult: T;
            // tslint:disable-next-line:no-any
            let lastArgs: any[];
            let timeoutId: number | null = null;

            if (options && typeof options.leading === 'boolean') {
                leading = options.leading;
            }

            if (options && typeof options.trailing === 'boolean') {
                trailing = options.trailing;
            }

            if (options && typeof options.maxWait === 'number' && !isNaN(options.maxWait)) {
                maxWait = options.maxWait;
            }

            let markExecuted = (time: number) => {
                if (timeoutId) {
                    this.clearTimeout(timeoutId);
                    timeoutId = null;
                }
                lastExecuteTime = time;
            };

            let invokeFunction = (time: number) => {
                markExecuted(time);
                lastResult = func.apply(this._parent, lastArgs);
            };

            let callback = (userCall?: boolean) => {
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
                    } else {
                        waitLength = Math.min(waitLength, maxWait - maxWaitDelta);
                    }
                }

                if (delta >= waitMS || maxWaitExpired || executeImmediately) {
                    invokeFunction(now);
                } else if ((timeoutId === null || !userCall) && trailing) {
                    timeoutId = this.setTimeout(callback, waitLength);
                }

                return lastResult;
            };

            let pending = (): boolean => {
                return !!timeoutId;
            };

            let cancel = (): void => {
                if (pending()) {
                    // Mark the debounced function as having executed
                    markExecuted(new Date().getTime());
                }
            };

            let flush = (): T => {
                if (pending()) {
                    invokeFunction(new Date().getTime());
                }

                return lastResult;
            };

            // tslint:disable-next-line:no-any
            let resultFunction: ICancelable<T> & (() => T) = ((...args: any[]) => {
                lastArgs = args;
                return callback(true);
            }) as ICancelable<T> & (() => T);

            resultFunction.cancel = cancel;
            resultFunction.flush = flush;
            resultFunction.pending = pending;

            return resultFunction;
        }

        public requestAnimationFrame(callback: () => void, targetElement?: Element | null): number {
            let animationFrameId = 0;
            const win = getWindow(targetElement)!;

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
                    } catch (e) {
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

        public cancelAnimationFrame(id: number, targetElement?: Element | null): void {
            const win = getWindow(targetElement)!;

            if (this._animationFrameIds && this._animationFrameIds[id]) {
                /* tslint:disable:ban-native-functions */
                win.cancelAnimationFrame ? win.cancelAnimationFrame(id) : win.clearTimeout(id);
                /* tslint:enable:ban-native-functions */
                delete this._animationFrameIds[id];
            }
        }

        // tslint:disable-next-line:no-any
        protected _logError(e: any): void {
            if (this._onErrorHandler) {
                this._onErrorHandler(e);
            }
        }
    }



    // EventGroup

    export interface IEventRecord {
        target: any;
        eventName: string;
        parent: any;
        callback: (args?: any) => void;
        elementCallback?: (...args: any[]) => void;
        objectCallback?: (args?: any) => void;
        options?: boolean | AddEventListenerOptions;
    }

    export interface IEventRecordsByName {
        [eventName: string]: IEventRecordList;
    }

    export interface IEventRecordList {
        [id: string]: IEventRecord[] | number;
        count: number;
    }

    export interface IDeclaredEventsByName {
        [eventName: string]: boolean;
    }

    export interface EventParams {
        element: HTMLElement | Window;
        event: string;
        handler: (ev: Event) => void;
        capture: boolean;
    }

    export function assign(target: any, ...args: any[]): any {
        return filteredAssign.apply(this, [null, target].concat(args));
    }

    export function filteredAssign(isAllowed: (propName: string) => boolean, target: any, ...args: any[]): any {
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
    export class EventGroup {
        private static _uniqueId: number = 0;
        // tslint:disable-next-line:no-any
        private _parent: any;
        private _eventRecords: IEventRecord[];
        private _id: number = EventGroup._uniqueId++;
        private _isDisposed: boolean;

        /** For IE8, bubbleEvent is ignored here and must be dealt with by the handler.
         *  Events raised here by default have bubbling set to false and cancelable set to true.
         *  This applies also to built-in events being raised manually here on HTMLElements,
         *  which may lead to unexpected behavior if it differs from the defaults.
         *
         */
        public static raise(
            // tslint:disable-next-line:no-any
            target: any,
            eventName: string,
            // tslint:disable-next-line:no-any
            eventArgs?: any,
            bubbleEvent?: boolean,
        ): boolean | undefined {
            let retVal;

            if (EventGroup._isElement(target)) {
                if (typeof document !== 'undefined' && document.createEvent) {
                    let ev = document.createEvent('HTMLEvents');

                    ev.initEvent(eventName, bubbleEvent || false, true);

                    assign(ev, eventArgs);

                    retVal = target.dispatchEvent(ev);
                    // tslint:disable-next-line:no-any
                } else if (typeof document !== 'undefined' && (document as any)['createEventObject']) {
                    // IE8
                    // tslint:disable-next-line:no-any
                    let evObj = (document as any)['createEventObject'](eventArgs);
                    // cannot set cancelBubble on evObj, fireEvent will overwrite it
                    target.fireEvent('on' + eventName, evObj);
                }
            } else {
                while (target && retVal !== false) {
                    let events = <IEventRecordsByName>target.__events__;
                    let eventRecords = events ? events[eventName] : null;

                    if (eventRecords) {
                        for (let id in eventRecords) {
                            if (eventRecords.hasOwnProperty(id)) {
                                let eventRecordList = <IEventRecord[]>eventRecords[id];

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
        public static isObserved(target: any, eventName: string): boolean {
            let events = target && <IEventRecordsByName>target.__events__;

            return !!events && !!events[eventName];
        }

        /** Check to see if the target has declared support of the given event. */
        // tslint:disable-next-line:no-any
        public static isDeclared(target: any, eventName: string): boolean {
            let declaredEvents = target && <IDeclaredEventsByName>target.__declaredEvents;

            return !!declaredEvents && !!declaredEvents[eventName];
        }

        // tslint:disable-next-line:no-any
        public static stopPropagation(event: any): void {
            if (event.stopPropagation) {
                event.stopPropagation();
            } else {
                // IE8
                event.cancelBubble = true;
            }
        }

        private static _isElement(target: HTMLElement): boolean {
            return (
                !!target && (!!target.addEventListener || (typeof HTMLElement !== 'undefined' && target instanceof HTMLElement))
            );
        }

        /** parent: the context in which events attached to non-HTMLElements are called */
        // tslint:disable-next-line:no-any
        public constructor(parent: any) {
            this._parent = parent;
            this._eventRecords = [];
        }

        public dispose(): void {
            if (!this._isDisposed) {
                this._isDisposed = true;

                this.off();
                this._parent = null;
            }
        }

        /** On the target, attach a set of events, where the events object is a name to function mapping. */
        // tslint:disable-next-line:no-any
        public onAll(target: any, events: { [key: string]: (args?: any) => void }, useCapture?: boolean): void {
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
        public on(
            target: any, // tslint:disable-line:no-any
            eventName: string,
            callback: (args?: any) => void, // tslint:disable-line:no-any
            options?: boolean | AddEventListenerOptions,
        ): void {
            if (eventName.indexOf(',') > -1) {
                let events = eventName.split(/[ ,]+/);

                for (let i = 0; i < events.length; i++) {
                    this.on(target, events[i], callback, options);
                }
            } else {
                let parent = this._parent;
                let eventRecord: IEventRecord = {
                    target: target,
                    eventName: eventName,
                    parent: parent,
                    callback: callback,
                    options,
                };

                // Initialize and wire up the record on the target, so that it can call the callback if the event fires.
                let events = <IEventRecordsByName>(target.__events__ = target.__events__ || {});
                events[eventName] =
                    events[eventName] ||
                    <IEventRecordList>{
                        count: 0,
                    };
                events[eventName][this._id] = events[eventName][this._id] || [];
                (<IEventRecord[]>events[eventName][this._id]).push(eventRecord);
                events[eventName].count++;

                if (EventGroup._isElement(target)) {
                    // tslint:disable-next-line:no-any
                    let processElementEvent = (...args: any[]) => {
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
                        } catch (e) {
                            /* ErrorHelper.log(e); */
                        }

                        return result;
                    };

                    eventRecord.elementCallback = processElementEvent;

                    if (target.addEventListener) {
                        /* tslint:disable:ban-native-functions */
                        (<EventTarget>target).addEventListener(eventName, processElementEvent, options);
                        /* tslint:enable:ban-native-functions */
                    } else if (target.attachEvent) {
                        // IE8
                        target.attachEvent('on' + eventName, processElementEvent);
                    }
                } else {
                    // tslint:disable-next-line:no-any
                    let processObjectEvent = (...args: any[]) => {
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

        public off(
            target?: any, // tslint:disable-line:no-any
            eventName?: string,
            callback?: (args?: any) => void, // tslint:disable-line:no-any
            options?: boolean | AddEventListenerOptions,
        ): void {
            for (let i = 0; i < this._eventRecords.length; i++) {
                let eventRecord = this._eventRecords[i];
                if (
                    (!target || target === eventRecord.target) &&
                    (!eventName || eventName === eventRecord.eventName) &&
                    (!callback || callback === eventRecord.callback) &&
                    (typeof options !== 'boolean' || options === eventRecord.options)
                ) {
                    let events = <IEventRecordsByName>eventRecord.target.__events__;
                    let targetArrayLookup = events[eventRecord.eventName];
                    let targetArray = targetArrayLookup ? <IEventRecord[]>targetArrayLookup[this._id] : null;

                    // We may have already target's entries, so check for null.
                    if (targetArray) {
                        if (targetArray.length === 1 || !callback) {
                            targetArrayLookup.count -= targetArray.length;
                            delete events[eventRecord.eventName][this._id];
                        } else {
                            targetArrayLookup.count--;
                            targetArray.splice(targetArray.indexOf(eventRecord), 1);
                        }

                        if (!targetArrayLookup.count) {
                            delete events[eventRecord.eventName];
                        }
                    }

                    if (eventRecord.elementCallback) {
                        if (eventRecord.target.removeEventListener) {
                            eventRecord.target.removeEventListener(
                                eventRecord.eventName,
                                eventRecord.elementCallback,
                                eventRecord.options,
                            );
                        } else if (eventRecord.target.detachEvent) {
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
        public raise(eventName: string, eventArgs?: any, bubbleEvent?: boolean): boolean | undefined {
            return EventGroup.raise(this._parent, eventName, eventArgs, bubbleEvent);
        }

        /** Declare an event as being supported by this instance of EventGroup. */
        public declare(event: string | string[]): void {
            let declaredEvents = (this._parent.__declaredEvents = this._parent.__declaredEvents || {});

            if (typeof event === 'string') {
                declaredEvents[event] = true;
            } else {
                for (let i = 0; i < event.length; i++) {
                    declaredEvents[event[i]] = true;
                }
            }
        }
    }
  




}

//declare global {
    interface Window {
        FluentUIBaseComponent: typeof FluentUIBaseComponent
    }
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

