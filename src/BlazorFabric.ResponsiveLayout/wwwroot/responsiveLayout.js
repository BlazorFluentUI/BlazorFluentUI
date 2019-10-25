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
            console.log("query triggered: " + ev.media);
            //var mql = window.matchMedia(query);
            if (ev.matches) {
                result = query;
            }
            responsiveLayout.invokeMethodAsync("QueryChanged", result);
        });
        return handler;
    }
    BlazorFabricResponsiveLayout.registerMediaQueryWatcher = registerMediaQueryWatcher;
    function registerMediaQueryWatchers(responsiveLayout, minWidthArray) {
        var mqls = [];
        var handlers = [];
        for (var i = 0; i < minWidthArray.length; i++) {
            var mql = window.matchMedia("(min-width:" + minWidthArray[i] + "px)");
            var handler = Handler.addListener(mql, function (ev) {
                var result = 0;
                for (var i_1 = 0; i_1 < minWidthArray.length; i_1++) {
                    var mql = window.matchMedia("(min-width:" + minWidthArray[i_1] + "px)");
                    if (mql.matches) {
                        result = minWidthArray[i_1];
                    }
                }
                responsiveLayout.invokeMethodAsync("WidthChanged", result);
            });
            mqls.push(mql);
            handlers.push(handler);
        }
        return handlers;
    }
    BlazorFabricResponsiveLayout.registerMediaQueryWatchers = registerMediaQueryWatchers;
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