//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }


namespace BlazorFluentUiList {

    interface DotNetReferenceType {

    invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
    invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }

    interface IElementMeasurements {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
        cwidth: number;
        cheight: number;
        test: string;
    }

  type ICancelable<T> = {
    flush: () => T;
    cancel: () => void;
    pending: () => boolean;
  };

  export function measureElement(element: HTMLElement): IRectangle {
    var rect: IRectangle = {
      width: element.clientWidth,
      height: element.clientHeight,
      left: 0,
      top: 0
    }
    return rect;
  };

  export function measureScrollWindow(element: HTMLElement): IRectangle {
    var rect: IRectangle = {
      width: element.scrollWidth,
      height: element.scrollHeight,
      top: element.scrollTop,
      left: element.scrollLeft,
      bottom: element.scrollTop + element.clientHeight,
        right: element.scrollLeft + element.clientWidth        
    }
    return rect;
  };

    export function measureElementRect(element: HTMLElement): IElementMeasurements {
        var rect = element.getBoundingClientRect();

        var elementMeasurements: IElementMeasurements = { height : rect.height, width: rect.width, left: rect.left, right: rect.right, top: rect.top, bottom: rect.bottom, cheight:element.clientHeight, cwidth:element.clientWidth, test:"Random!"};

        return elementMeasurements;
    };



    var _ticking: boolean;

    export function initialize(component: DotNetReferenceType, scrollElement:HTMLElement, contentElement: HTMLElement): any {
        scrollElement.addEventListener('scroll', e => {
            const lastKnownValues = {
                containerRect: scrollElement.getBoundingClientRect(),
                contentRect: readClientRectWithoutTransform(contentElement),
                averageHeight: 40
            };

            if (!_ticking) {
                (<any>window).requestIdleCallback(() => {
                    component.invokeMethodAsync('OnScroll', lastKnownValues);
                    _ticking = false;
                });

                _ticking = true;
            }
        });

        for (var i = 0; i < contentElement.children.length; i++) {

        }

        return {
            containerRect: scrollElement.getBoundingClientRect(),
            contentRect: readClientRectWithoutTransform(contentElement),
            averageHeight: 40
        };
    }

    function readClientRectWithoutTransform(elem): DOMRect {
        const rect = elem.getBoundingClientRect();
        const translateY = parseFloat(elem.getAttribute('data-translateY'));
        return {
            top: rect.top - translateY, bottom: rect.bottom - translateY, left: rect.left, right: rect.right, height: rect.height, width: rect.width, x: 0, y: 0, toJSON: null };
    }

  

 
  

  function _expandRect(rect: IRectangle, pagesBefore: number, pagesAfter: number): IRectangle {
    const top = rect.top - pagesBefore * rect.height;
    const height = rect.height + (pagesBefore + pagesAfter) * rect.height;

    return {
      top: top,
      bottom: top + height,
      height: height,
      left: rect.left,
      right: rect.right,
      width: rect.width
    };
  }

  function _isContainedWithin(innerRect: IRectangle, outerRect: IRectangle): boolean {
    return (
      innerRect.top >= outerRect.top &&
      innerRect.left >= outerRect.left &&
      innerRect.bottom! <= outerRect.bottom! &&
      innerRect.right! <= outerRect.right!
    );
  }

  function _mergeRect(targetRect: IRectangle, newRect: IRectangle): IRectangle {
    targetRect.top = newRect.top < targetRect.top || targetRect.top === -1 ? newRect.top : targetRect.top;
    targetRect.left = newRect.left < targetRect.left || targetRect.left === -1 ? newRect.left : targetRect.left;
    targetRect.bottom = newRect.bottom! > targetRect.bottom! || targetRect.bottom === -1 ? newRect.bottom : targetRect.bottom;
    targetRect.right = newRect.right! > targetRect.right! || targetRect.right === -1 ? newRect.right : targetRect.right;
    targetRect.width = targetRect.right! - targetRect.left + 1;
    targetRect.height = targetRect.bottom! - targetRect.top + 1;

    return targetRect;
  }

}




(<any>window)['BlazorFluentUiList'] = BlazorFluentUiList || {};

