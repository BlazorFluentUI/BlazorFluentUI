
namespace BlazorFabricTextField {

  export function getScrollHeight(element: HTMLElement): number {
    return element.scrollHeight;
  };
 
}

(<any>window)['BlazorFabricTextField'] = BlazorFabricTextField || {};

