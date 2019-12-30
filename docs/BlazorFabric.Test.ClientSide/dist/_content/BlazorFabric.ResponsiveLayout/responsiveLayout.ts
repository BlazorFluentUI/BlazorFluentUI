//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }


namespace BlazorFabricResponsiveLayout {

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface Map<T> {
        [K: number]: T;
    }

    export function testQuery(query: string): boolean {
        var mql = window.matchMedia(query);
        return mql.matches;
    }

    export function registerMediaQueryWatcher(responsiveLayout: DotNetReferenceType, query: string): number {
        var mql = window.matchMedia(query);
        var handler = Handler.addListener(mql, (ev : MediaQueryListEvent) => {
            var result: string;
            //var mql = window.matchMedia(query);
            if (ev.matches) {
                console.log("query triggered: " + ev.media);
                result = query;
                responsiveLayout.invokeMethodAsync("QueryChanged", result);
            }

            
        });
        
        return handler;
    }

    //export function registerMediaQueryWatchers(responsiveLayout: DotNetReferenceType, minWidthArray: number[]): number[] {
    //    var mqls: MediaQueryList[] = [];
    //    var handlers: number[] = [];
    //    for (let i = 0; i < minWidthArray.length; i++) {
    //        var mql = window.matchMedia(`(min-width:${minWidthArray[i]}px)`);
    //        var handler = Handler.addListener(mql, (ev) => {
    //            var result: number = 0;
    //            for (let i = 0; i < minWidthArray.length; i++) {
    //                var mql = window.matchMedia(`(min-width:${minWidthArray[i]}px)`);
    //                if (mql.matches) {
    //                    result = minWidthArray[i];
    //                }
    //            }
    //            responsiveLayout.invokeMethodAsync("WidthChanged", result);
    //        });
    //        mqls.push(mql);
    //        handlers.push(handler);
    //    }
               
    //    return handlers;
    //}


    export function unregisterHandlers(ids: number[]): void {
        for (let i = 0; i < ids.length; i++) {
            Handler.removeListener(ids[i]);
        }
    }

    export function unregisterHandler(id: number): void {
        Handler.removeListener(id);
    }

    interface EventParams {
        mql: MediaQueryList;
        handler: (ev: Event) => void;
    }

    class Handler {

        static i: number = 1;
        static listeners: Map<EventParams> = {};

        static addListener(mql: MediaQueryList, handler: (ev: Event) => void): number {
            mql.addListener(handler);
            this.listeners[this.i] = { handler: handler, mql: mql };
            return this.i++;
        }
        static removeListener(id: number): void {
            if (id in this.listeners) {
                var h = this.listeners[id];
                h.mql.removeListener(h.handler);
                delete this.listeners[id];
            }
        }
    }

   

}

(<any>window)['BlazorFabricResponsiveLayout'] = BlazorFabricResponsiveLayout || {};

