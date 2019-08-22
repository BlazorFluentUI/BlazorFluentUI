//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }


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



}




window['BlazorFabricBaseComponent'] = BlazorFabricBaseComponent || {};

