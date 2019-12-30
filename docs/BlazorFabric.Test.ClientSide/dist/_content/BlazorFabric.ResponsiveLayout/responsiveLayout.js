//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFabricResponsiveLayout;
(function (BlazorFabricResponsiveLayout) {
    function testQuery(query) {
        var mql = window.matchMedia(query);
        return mql.matches;
    }
    BlazorFabricResponsiveLayout.testQuery = testQuery;
    function registerMediaQueryWatcher(responsiveLayout, query) {
        var mql = window.matchMedia(query);
        var handler = Handler.addListener(mql, function (ev) {
            var result;
            //var mql = window.matchMedia(query);
            if (ev.matches) {
                console.log("query triggered: " + ev.media);
                result = query;
                responsiveLayout.invokeMethodAsync("QueryChanged", result);
            }
        });
        return handler;
    }
    BlazorFabricResponsiveLayout.registerMediaQueryWatcher = registerMediaQueryWatcher;
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
    function unregisterHandlers(ids) {
        for (var i = 0; i < ids.length; i++) {
            Handler.removeListener(ids[i]);
        }
    }
    BlazorFabricResponsiveLayout.unregisterHandlers = unregisterHandlers;
    function unregisterHandler(id) {
        Handler.removeListener(id);
    }
    BlazorFabricResponsiveLayout.unregisterHandler = unregisterHandler;
    var Handler = /** @class */ (function () {
        function Handler() {
        }
        Handler.addListener = function (mql, handler) {
            mql.addListener(handler);
            this.listeners[this.i] = { handler: handler, mql: mql };
            return this.i++;
        };
        Handler.removeListener = function (id) {
            if (id in this.listeners) {
                var h = this.listeners[id];
                h.mql.removeListener(h.handler);
                delete this.listeners[id];
            }
        };
        Handler.i = 1;
        Handler.listeners = {};
        return Handler;
    }());
})(BlazorFabricResponsiveLayout || (BlazorFabricResponsiveLayout = {}));
window['BlazorFabricResponsiveLayout'] = BlazorFabricResponsiveLayout || {};
//# sourceMappingURL=responsiveLayout.js.map