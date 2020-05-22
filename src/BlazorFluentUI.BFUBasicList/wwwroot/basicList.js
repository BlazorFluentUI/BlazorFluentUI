/// <reference path="baseComponent.d.ts" />
var BlazorFabricBasicList;
(function (BlazorFabricBasicList) {
    var count = 0;
    var allInstances = {};
    function register(basicList, rootElement) {
        var currentId = count++;
        allInstances[currentId] = new BasicListInternal(basicList, rootElement);
        return currentId;
    }
    BlazorFabricBasicList.register = register;
    function unregister(id) {
        var basicList = allInstances[id];
        if (basicList) {
            basicList.unRegister();
        }
        delete allInstances[id];
    }
    BlazorFabricBasicList.unregister = unregister;
    function getScrollDimensions(id) {
        var basicList = allInstances[id];
        if (basicList != null) {
            return basicList.getScrollDimensions();
        }
        return null;
    }
    BlazorFabricBasicList.getScrollDimensions = getScrollDimensions;
    function setScrollTop(id, top) {
        var basicList = allInstances[id];
        if (basicList != null) {
            basicList.setScrollTop(top);
        }
    }
    BlazorFabricBasicList.setScrollTop = setScrollTop;
    var BasicListInternal = /** @class */ (function () {
        function BasicListInternal(basicList, rootElement) {
            var _this = this;
            this.debouncedScroll = function (ev) {
                //BlazorFabricBaseComponent.debounce((ev: UIEvent) => {
                _this._dotNetRef.invokeMethodAsync("ScrollHandler");
                //}, 100, { leading: true });
            };
            this._dotNetRef = basicList;
            this._rootElement = rootElement;
            this._scrollElement = BlazorFabricBaseComponent.findScrollableParent(this._rootElement);
            if (this._scrollElement) {
                this._scrollElement.addEventListener("scroll", this.debouncedScroll);
            }
        }
        BasicListInternal.prototype.unRegister = function () {
            if (this._scrollElement) {
                this._scrollElement.removeEventListener("scroll", this.debouncedScroll);
            }
        };
        BasicListInternal.prototype.getScrollDimensions = function () {
            if (this._scrollElement) {
                return { height: this._scrollElement.scrollHeight, top: this._scrollElement.scrollTop ? this._scrollElement.scrollTop : 0, left: 0, width: 0 };
            }
            return null;
        };
        BasicListInternal.prototype.setScrollTop = function (top) {
            if (this._scrollElement) {
                this._scrollElement.scrollTop = top;
            }
        };
        return BasicListInternal;
    }());
})(BlazorFabricBasicList || (BlazorFabricBasicList = {}));
window['BlazorFabricBasicList'] = BlazorFabricBasicList || {};
//# sourceMappingURL=basicList.js.map