namespace BlazorFabricBaseComponent {

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    export function initializeFocusRects(): void {
        if (!(<any>window).__hasInitializeFocusRects__) {
            (<any>window).__hasInitializeFocusRects__ = true;
            window.addEventListener("mousedown", _onFocusRectMouseDown, true);
            window.addEventListener("keydown", _onFocusRectKeyDown, true);
        }
    }

    const DATA_IS_SCROLLABLE_ATTRIBUTE = 'data-is-scrollable';
    const IsFocusVisibleClassName = 'ms-Fabric--isFocusVisible';

    function _onFocusRectMouseDown(ev: MouseEvent) {
        if (window.document.body.classList.contains(IsFocusVisibleClassName)) {
            window.document.body.classList.remove(IsFocusVisibleClassName);
        }
    }
    function _onFocusRectKeyDown(ev: MouseEvent) {
        if (isDirectionalKeyCode(ev.which) && !window.document.body.classList.contains(IsFocusVisibleClassName)) {
            window.document.body.classList.add(IsFocusVisibleClassName);
        }
    };

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
        return element.clientHeight;
    }

    export function getScrollHeight(element: HTMLElement): number {
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
        var rect: IRectangle = {
            width: image.naturalWidth,
            height: image.naturalHeight,
            left: 0,
            top: 0
        }
        return rect;
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
    };

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
    };

    export function measureElementRect(element: HTMLElement): IRectangle {
        return element.getBoundingClientRect();
    };

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
    };

    interface Map<T> {
        [K: string]: T;
    }

    var eventRegister: Map<(ev: UIEvent) => void> = {};

    export function registerResizeEvent(dotnetRef: DotNetReferenceType, functionName: string) : string {
        var guid = Guid.newGuid();
        eventRegister[guid] = debounce((ev: UIEvent) => {
            dotnetRef.invokeMethodAsync(functionName);
        }, 16, { leading: true });
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


/* Focus stuff */

    export function focusFirstChild(rootElement: HTMLElement): boolean {

        return false;
    }

    function getNextElement(rootElement: HTMLElement){

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

    const enum KeyCodes {
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


   

}




(<any>window)['BlazorFabricBaseComponent'] = BlazorFabricBaseComponent || {};

