namespace BlazorFabricBaseComponent {

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }

    export function measureElement(element: HTMLElement): IRectangle {
        var rect: IRectangle = {
            width: element.clientWidth,
            height: element.clientHeight,
            left: 0,
            top: 0
        }
        return rect;
    };

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

}




(<any>window)['BlazorFabricBaseComponent'] = BlazorFabricBaseComponent || {};

