//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
var BlazorFabricLayer;
(function (BlazorFabricLayer) {
    function findHostElement(id) {
        var element = document.getElementById(id);
        if (element === undefined) {
            return document.body;
        }
        else {
            return element;
        }
    }
    BlazorFabricLayer.findHostElement = findHostElement;
    function setLayerElement(id, classNames, insertFirst) {
        if (insertFirst === void 0) { insertFirst = false; }
        var host = findHostElement(id);
        var layerElement = document.createElement('div');
        layerElement.className = classNames;
        if (insertFirst) {
            host.insertBefore(layerElement, host.firstChild);
        }
        else {
            host.appendChild(layerElement);
        }
        return layerElement;
    }
    BlazorFabricLayer.setLayerElement = setLayerElement;
})(BlazorFabricLayer || (BlazorFabricLayer = {}));
window['BlazorFabricLayer'] = BlazorFabricLayer || {};
//# sourceMappingURL=fabricLayer.js.map