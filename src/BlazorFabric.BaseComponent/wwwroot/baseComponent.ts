namespace BlazorFabricBaseComponent {
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

    interface IRectangle {
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

    export function getWindow(element: HTMLElement): Window {
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

    interface Map<T> {
        [K: string]: T;
    }

    var eventRegister: Map<(ev: UIEvent) => void> = {};

    var eventElementRegister: Map<[HTMLElement, (ev: UIEvent) => void]> = {};

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
    var _lastFocus: Map<HTMLElement> = {};

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
        return child && (child.parentNode && (child.parentNode as HTMLElement));
    }

    export function elementContains(parent: HTMLElement, child: HTMLElement, allowVirtualParents: boolean = true): boolean {
        let isContained = false;
        if (parent && child) {
            if (allowVirtualParents) {
                isContained = false;
                while (child) {
                    let nextParent: HTMLElement | null = getParent(child);
                    // console.log("NextParent: " + nextParent);
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


    function debounce<T extends Function>(
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


   

}

//declare global {
    interface Window {
        BlazorFabricBaseComponent: typeof BlazorFabricBaseComponent
    }
//}

window.BlazorFabricBaseComponent = BlazorFabricBaseComponent;

//window.BlazorFabricBaseComponent

//(<any>window)['BlazorFabricBaseComponent'] = BlazorFabricBaseComponent || {};

