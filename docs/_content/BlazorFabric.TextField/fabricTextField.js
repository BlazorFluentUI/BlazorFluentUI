var BlazorFabricTextField;
(function (BlazorFabricTextField) {
    function getScrollHeight(element) {
        var paddingTop = window.getComputedStyle(element, null).getPropertyValue('padding-top');
        var paddingBottom = window.getComputedStyle(element, null).getPropertyValue('padding-bottom');
        var yPadding = parseInt(paddingTop) + parseInt(paddingBottom);
        return element.scrollHeight - yPadding;
    }
    BlazorFabricTextField.getScrollHeight = getScrollHeight;
    ;
})(BlazorFabricTextField || (BlazorFabricTextField = {}));
window['BlazorFabricTextField'] = BlazorFabricTextField || {};
//# sourceMappingURL=fabricTextField.js.map