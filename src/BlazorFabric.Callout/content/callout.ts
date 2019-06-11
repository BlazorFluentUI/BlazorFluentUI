//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }


namespace BlazorFabricCallout {

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

    export function registerHandlers(targetElement: HTMLElement, calloutRef: DotNetReferenceType): void {
        var window = targetElement.ownerDocument.defaultView;
        window.onscroll = (ev: Event) => calloutRef.invokeMethodAsync("ScrollHandler");
        
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
    };



}




window['BlazorFabricCallout'] = BlazorFabricCallout || {};

