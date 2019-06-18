
namespace BlazorFabricTextField {

  export function getScrollHeight(element: HTMLElement): number {
    return element.scrollHeight;
  };
 
}

window['BlazorFabricTextField'] = BlazorFabricTextField || {};

