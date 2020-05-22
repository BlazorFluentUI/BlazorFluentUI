var BlazorFluentUiTextField;
(function (BlazorFluentUiTextField) {
    function getScrollHeight(element) {
        var paddingTop = window.getComputedStyle(element, null).getPropertyValue('padding-top');
        var paddingBottom = window.getComputedStyle(element, null).getPropertyValue('padding-bottom');
        var yPadding = parseInt(paddingTop) + parseInt(paddingBottom);
        return element.scrollHeight - yPadding;
    }
    BlazorFluentUiTextField.getScrollHeight = getScrollHeight;
    ;
})(BlazorFluentUiTextField || (BlazorFluentUiTextField = {}));
window['BlazorFluentUiTextField'] = BlazorFluentUiTextField || {};
//# sourceMappingURL=fabricTextField.js.map