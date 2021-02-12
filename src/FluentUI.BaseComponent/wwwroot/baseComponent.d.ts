declare namespace FluentUIBaseComponent {
    interface DotNetReferenceType {
        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }
    interface ElementReferenceResult {
        element: HTMLElement | undefined;
        isNull: boolean;
    }
    export function initializeFocusRects(): void;
    export interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }
    export function enableBodyScroll(): void;
    export function disableBodyScroll(): void;
    export function getClientHeight(element: HTMLElement): number;
    export function getScrollHeight(element: HTMLElement): number;
    export function findScrollableParent(startingElement: HTMLElement | null): HTMLElement | null;
    export function measureElement(element: HTMLElement): IRectangle;
    export function getNaturalBounds(image: HTMLImageElement): IRectangle;
    export function supportsObjectFit(): boolean;
    export function hasOverflow(element: HTMLElement): boolean;
    export function measureScrollWindow(element: HTMLElement): IRectangle;
    interface IScrollDimensions {
        scrollHeight: number;
        scrollWidth: number;
    }
    export function measureScrollDimensions(element: HTMLElement): IScrollDimensions;
    export function measureElementRect(element: HTMLElement): IRectangle;
    export function getWindow(element: Element): Window;
    export function getWindowRect(): IRectangle;
    export function getElementId(element: HTMLElement): string;
    export function registerKeyEventsForList(element: HTMLElement): string;
    export function deregisterKeyEventsForList(guid: number): void;
    export function registerWindowKeyDownEvent(dotnetRef: DotNetReferenceType, keyCode: string, functionName: string): string;
    export function deregisterWindowKeyDownEvent(guid: number): void;
    export function registerResizeEvent(dotnetRef: DotNetReferenceType, functionName: string): string;
    export function deregisterResizeEvent(guid: number): void;
    export function addViewport(component: DotNetReferenceType, rootElement: HTMLElement, fireInitialViewport?: boolean): number;
    export function removeViewport(id: number): void;
    export function getRect(element: HTMLElement | Window | null): IRectangle | undefined;
    export function findElementRecursive(element: HTMLElement | null, matchFunction: (element: HTMLElement) => boolean): HTMLElement | null;
    export function elementContainsAttribute(element: HTMLElement, attribute: string): string | null;
    export function storeLastFocusedElement(): string;
    export function restoreLastFocus(guid: string, restoreFocus?: boolean): void;
    export function getActiveElement(): Element;
    export function focusElement(element: HTMLElement): void;
    export function focusFirstElementChild(element: HTMLElement): void;
    export function shouldWrapFocus(element: HTMLElement, noWrapDataAttribute: 'data-no-vertical-wrap' | 'data-no-horizontal-wrap'): boolean;
    export function getFocusableByIndexPath(parent: HTMLElement, path: number[]): ElementReferenceResult;
    export function getFirstFocusable(rootElement: HTMLElement, currentElement: HTMLElement, includeElementsInFocusZones?: boolean): HTMLElement;
    export function getLastFocusable(rootElement: HTMLElement, currentElement: HTMLElement, includeElementsInFocusZones?: boolean): HTMLElement;
    export function isElementTabbable(element: HTMLElement, checkTabIndex?: boolean): boolean;
    export function isElementVisible(element: HTMLElement | undefined | null): boolean;
    export function focusFirstChild(rootElement: HTMLElement): boolean;
    export function getParent(child: HTMLElement, allowVirtualParents?: boolean): HTMLElement | null;
    export function addOrUpdateVirtualParent(parent: HTMLElement): void;
    export function getVirtualParent(child: HTMLElement): HTMLElement | undefined;
    export function elementContains(parent: HTMLElement, child: HTMLElement, allowVirtualParents?: boolean): boolean;
    export function getNextElement(rootElement: HTMLElement, currentElement: HTMLElement | null, checkNode?: boolean, suppressParentTraversal?: boolean, suppressChildTraversal?: boolean, includeElementsInFocusZones?: boolean, allowFocusRoot?: boolean, tabbable?: boolean): HTMLElement | null;
    export function getPreviousElement(rootElement: HTMLElement, currentElement: HTMLElement | null, checkNode?: boolean, suppressParentTraversal?: boolean, traverseChildren?: boolean, includeElementsInFocusZones?: boolean, allowFocusRoot?: boolean, tabbable?: boolean): HTMLElement | null;
    export interface IPoint {
        x: number;
        y: number;
    }
    /** Raises a click event. */
    export function raiseClick(target: Element): void;
    export function isElementFocusZone(element?: HTMLElement): boolean;
    export function isElementFocusSubZone(element?: HTMLElement): boolean;
    export function on(element: Element | Window, eventName: string, callback: (ev: Event) => void, options?: boolean): () => void;
    export function debounce<T extends Function>(func: T, wait?: number, options?: {
        leading?: boolean;
        maxWait?: number;
        trailing?: boolean;
    }): ICancelable<T> & (() => void);
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
    export function getRTL(): boolean;
    export function setRTL(isRTL: boolean, persistSetting?: boolean): void;
    export function getItem(key: string): string | null;
    export function setItem(key: string, data: string): void;
    export class Async {
        private _timeoutIds;
        private _immediateIds;
        private _intervalIds;
        private _animationFrameIds;
        private _isDisposed;
        private _parent;
        private _onErrorHandler;
        private _noop;
        constructor(parent?: object, onError?: (e: any) => void);
        /**
         * Dispose function, clears all async operations.
         */
        dispose(): void;
        /**
         * SetTimeout override, which will auto cancel the timeout during dispose.
         * @param callback - Callback to execute.
         * @param duration - Duration in milliseconds.
         * @returns The setTimeout id.
         */
        setTimeout(callback: () => void, duration: number): number;
        /**
         * Clears the timeout.
         * @param id - Id to cancel.
         */
        clearTimeout(id: number): void;
        /**
         * SetImmediate override, which will auto cancel the immediate during dispose.
         * @param callback - Callback to execute.
         * @param targetElement - Optional target element to use for identifying the correct window.
         * @returns The setTimeout id.
         */
        setImmediate(callback: () => void, targetElement?: Element | null): number;
        /**
         * Clears the immediate.
         * @param id - Id to cancel.
         * @param targetElement - Optional target element to use for identifying the correct window.
         */
        clearImmediate(id: number, targetElement?: Element | null): void;
        /**
         * SetInterval override, which will auto cancel the timeout during dispose.
         * @param callback - Callback to execute.
         * @param duration - Duration in milliseconds.
         * @returns The setTimeout id.
         */
        setInterval(callback: () => void, duration: number): number;
        /**
         * Clears the interval.
         * @param id - Id to cancel.
         */
        clearInterval(id: number): void;
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
        throttle<T extends Function>(func: T, wait?: number, options?: {
            leading?: boolean;
            trailing?: boolean;
        }): T | (() => void);
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
        debounce<T extends Function>(func: T, wait?: number, options?: {
            leading?: boolean;
            maxWait?: number;
            trailing?: boolean;
        }): ICancelable<T> & (() => void);
        requestAnimationFrame(callback: () => void, targetElement?: Element | null): number;
        cancelAnimationFrame(id: number, targetElement?: Element | null): void;
        protected _logError(e: any): void;
    }
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
    export function assign(target: any, ...args: any[]): any;
    export function filteredAssign(isAllowed: (propName: string) => boolean, target: any, ...args: any[]): any;
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
        private static _uniqueId;
        private _parent;
        private _eventRecords;
        private _id;
        private _isDisposed;
        /** For IE8, bubbleEvent is ignored here and must be dealt with by the handler.
         *  Events raised here by default have bubbling set to false and cancelable set to true.
         *  This applies also to built-in events being raised manually here on HTMLElements,
         *  which may lead to unexpected behavior if it differs from the defaults.
         *
         */
        static raise(target: any, eventName: string, eventArgs?: any, bubbleEvent?: boolean): boolean | undefined;
        static isObserved(target: any, eventName: string): boolean;
        /** Check to see if the target has declared support of the given event. */
        static isDeclared(target: any, eventName: string): boolean;
        static stopPropagation(event: any): void;
        private static _isElement;
        /** parent: the context in which events attached to non-HTMLElements are called */
        constructor(parent: any);
        dispose(): void;
        /** On the target, attach a set of events, where the events object is a name to function mapping. */
        onAll(target: any, events: {
            [key: string]: (args?: any) => void;
        }, useCapture?: boolean): void;
        /**
         * On the target, attach an event whose handler will be called in the context of the parent
         * of this instance of EventGroup.
         */
        on(target: any, // tslint:disable-line:no-any
        eventName: string, callback: (args?: any) => void, // tslint:disable-line:no-any
        options?: boolean | AddEventListenerOptions): void;
        off(target?: any, // tslint:disable-line:no-any
        eventName?: string, callback?: (args?: any) => void, // tslint:disable-line:no-any
        options?: boolean | AddEventListenerOptions): void;
        /** Trigger the given event in the context of this instance of EventGroup. */
        raise(eventName: string, eventArgs?: any, bubbleEvent?: boolean): boolean | undefined;
        /** Declare an event as being supported by this instance of EventGroup. */
        declare(event: string | string[]): void;
    }
    export {};
}
interface Window {
    FluentUIBaseComponent: typeof FluentUIBaseComponent;
}
