declare namespace BlazorFabricBaseComponent {
    interface DotNetReferenceType {
        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }
    interface ElementReferenceResult {
        element: HTMLElement | undefined;
        isNull: boolean;
    }
    export function initializeFocusRects(): void;
    interface IRectangle {
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
    export function getWindow(element: HTMLElement): Window;
    export function getWindowRect(): IRectangle;
    export function getElementId(element: HTMLElement): string;
    export function registerKeyEventsForList(element: HTMLElement): string;
    export function deregisterKeyEventsForList(guid: number): void;
    export function registerWindowKeyDownEvent(dotnetRef: DotNetReferenceType, keyCode: string, functionName: string): string;
    export function deregisterWindowKeyDownEvent(guid: number): void;
    export function registerResizeEvent(dotnetRef: DotNetReferenceType, functionName: string): string;
    export function deregisterResizeEvent(guid: number): void;
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
    export {};
}
interface Window {
    BlazorFabricBaseComponent: typeof BlazorFabricBaseComponent;
}
