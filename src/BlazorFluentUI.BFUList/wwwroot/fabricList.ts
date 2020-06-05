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

    var _lastId: number = 0;
    var cachedLists: Map<number, BFUList> = new Map<number, BFUList>();
    
    class BFUList {
        cachedSizes: Map<string, number> = new Map<string, number>();
        averageHeight: number = 40;
        lastDate: number;
        id: number;

        component: DotNetReferenceType;
        scrollElement: HTMLElement;
        spacerBefore: HTMLElement;
        spacerAfter: HTMLElement;

        intersectionObserver: IntersectionObserver;
        mutationObserver: MutationObserver;

        constructor(component: DotNetReferenceType, scrollElement: HTMLElement, spacerBefore: HTMLElement, spacerAfter: HTMLElement) {
            this.id = _lastId++;

            this.component = component;
            this.scrollElement = scrollElement;
            this.spacerBefore = spacerBefore;
            this.spacerAfter = spacerAfter;

            const rootMargin: number = 50;
            this.intersectionObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting && (entry.target as HTMLElement).offsetHeight > 0) {
                        (<any>window).requestIdleCallback(() => {
                            const spacerType = entry.target === this.spacerBefore ? 'before' : 'after';
                            const visibleRect = {
                                top: entry.intersectionRect.top - entry.boundingClientRect.top,
                                left: entry.intersectionRect.left - entry.boundingClientRect.left,
                                width: entry.intersectionRect.width,
                                height: entry.intersectionRect.height,
                                bottom: this.scrollElement.scrollHeight,
                                right: this.scrollElement.scrollWidth
                            };
                            this.component.invokeMethodAsync('OnSpacerVisible', spacerType, visibleRect, this.scrollElement.offsetHeight + 2 * rootMargin, this.spacerBefore.offsetHeight, this.spacerAfter.offsetHeight);
                        });
                    }
                });
            }, {
                root: scrollElement, rootMargin: `${rootMargin}px`
            });
            this.intersectionObserver.observe(this.spacerBefore);
            this.intersectionObserver.observe(this.spacerAfter);

            // After each render, refresh the info about intersections
            this.mutationObserver = new MutationObserver(mutations => {
                this.intersectionObserver.unobserve(this.spacerBefore);
                this.intersectionObserver.unobserve(this.spacerAfter);
                this.intersectionObserver.observe(this.spacerBefore);
                this.intersectionObserver.observe(this.spacerAfter);
            });
            this.mutationObserver.observe(spacerBefore, { attributes: true })
        }

        disconnect(): void {
            this.mutationObserver.disconnect();

            this.intersectionObserver.unobserve(this.spacerBefore);
            this.intersectionObserver.unobserve(this.spacerAfter);
            this.intersectionObserver.disconnect();

        }

        getInitialAverageHeight(): number {
            let calculate: boolean = false;
            let averageHeight: number = 0;
            for (let i = 0; i < this.scrollElement.children.length; i++) {
                let item = this.scrollElement.children.item(i);
                let index = item.getAttribute("data-hash");
                if (index != null && !this.cachedSizes.has(index) && this.cachedSizes.get(index) != item.clientHeight) {
                    this.cachedSizes.set(index, item.clientHeight);
                    calculate = true;
                }
            }
            if (calculate) {
                averageHeight = [...this.cachedSizes.values()].reduce((p, c, i, a) => p + c) / this.cachedSizes.size;
            }

            return averageHeight;
        }

    }

    export function getInitialAverageHeight(id: number) : number {
        let list = cachedLists.get(id);
        if (list == null) {
            return 0;
        } else {
            return list.getInitialAverageHeight();
        }
    }
    
    export function initialize(component: DotNetReferenceType, scrollElement: HTMLElement, spacerBefore: HTMLElement, spacerAfter: HTMLElement, reset: boolean=false): any {

        //if (reset) {
        //    scrollElement.scrollTo(0, 0);
        //}

        let list: BFUList = new BFUList(component, scrollElement, spacerBefore, spacerAfter);
        cachedLists.set(list.id, list);

        const visibleRect = {
            top: 0,
            left: list.id,
            width: scrollElement.clientWidth,
            height: scrollElement.clientHeight,
            bottom: scrollElement.scrollHeight,
            right: scrollElement.scrollWidth
        };
        return visibleRect;
    }

    export function removeList(id: number) {
        let list = cachedLists.get(id);
        list.disconnect();
        cachedLists.delete(id);
    }

    export function getViewport(scrollElement: HTMLElement) :any {
        const visibleRect = {
            top: 0,
            left: 0,
            width: scrollElement.clientWidth,
            height: scrollElement.clientHeight,
            bottom: scrollElement.scrollHeight,
            right: scrollElement.scrollWidth
        };
        return visibleRect;
    }

    function readClientRectWithoutTransform(elem): DOMRect {
        const rect = elem.getBoundingClientRect();
        const translateY = parseFloat(elem.getAttribute('data-translateY'));
        return {
            top: rect.top - translateY, bottom: rect.bottom - translateY, left: rect.left, right: rect.right, height: rect.height, width: rect.width, x: 0, y: 0, toJSON: null
        };
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

