
namespace BlazorFluentUiTextField {

    export function getScrollHeight(element: HTMLElement): number {
        var paddingTop = window.getComputedStyle(element, null).getPropertyValue('padding-top');
        var paddingBottom = window.getComputedStyle(element, null).getPropertyValue('padding-bottom');
        var yPadding = parseInt(paddingTop) + parseInt(paddingBottom);
        return element.scrollHeight - yPadding;
    };

}

(<any>window)['BlazorFluentUiTextField'] = BlazorFluentUiTextField || {};
