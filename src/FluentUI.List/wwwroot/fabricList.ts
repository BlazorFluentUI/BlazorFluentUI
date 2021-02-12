//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
// /// <reference path="../../FluentUI.FocusTrapZone/wwwroot/focusTrapZone.ts" />
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />

namespace FluentUIList {

    type EventGroup = FluentUIBaseComponent.EventGroup;
    interface DotNetReferenceType {

    invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
    invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    interface SerializableDictionary<T> {
        [K: string]: T;
    }

    interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }

    var _lastId: number = 0;
    var cachedLists: Map<number, List> = new Map<number, List>();
    
    class List {
        events: EventGroup;
        cachedSizes: Map<string, number> = new Map<string, number>();
        averageHeight: number = 40;
        lastDate: number;
        id: number;

        component: DotNetReferenceType;
        rootElement: HTMLElement;
        scrollElement: HTMLElement;
        surfaceElement: HTMLElement;
        spacerBefore: HTMLElement;
        spacerAfter: HTMLElement;

        intersectionObserver: IntersectionObserver;
        mutationObserverBefore: MutationObserver;
        mutationObserverAfter: MutationObserver;

        constructor(component: DotNetReferenceType, spacerBefore: HTMLElement, spacerAfter: HTMLElement) {
            this.id = _lastId++;

            this.component = component;

            //this.surfaceElement = rootElement.children.item(0) as HTMLElement;

            this.scrollElement = FluentUIBaseComponent.findScrollableParent(spacerBefore);
            // get initial width
            this.component.invokeMethodAsync('ResizeHandler', this.scrollElement.clientWidth);

            this.events = new FluentUIBaseComponent.EventGroup(this);
            this.events.on(window, 'resize', this.resize);

            this.rootElement = spacerBefore.parentElement;
            //this.scrollElement = scrollElement;
            this.spacerBefore = spacerBefore;
            this.spacerAfter = spacerAfter;

            const rootMargin: number = 50;
            this.intersectionObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach((entry): void => {
                    if (!entry.isIntersecting) {
                        return;
                    }

                    const spacerBeforeRect = this.spacerBefore.getBoundingClientRect();
                    const spacerAfterRect = this.spacerAfter.getBoundingClientRect();
                    const spacerSeparation = spacerAfterRect.top - spacerBeforeRect.bottom;
                    const containerSize = entry.rootBounds?.height;

                    if (entry.target === this.spacerBefore) {
                        component.invokeMethodAsync('OnBeforeSpacerVisible', entry.intersectionRect.top - entry.boundingClientRect.top, spacerSeparation, containerSize);
                    } else if (entry.target === this.spacerAfter && this.spacerAfter.offsetHeight > 0) {
                        // When we first start up, both the "before" and "after" spacers will be visible, but it's only relevant to raise a
                        // single event to load the initial data. To avoid raising two events, skip the one for the "after" spacer if we know
                        // it's meaningless to talk about any overlap into it.
                        component.invokeMethodAsync('OnAfterSpacerVisible', entry.boundingClientRect.bottom - entry.intersectionRect.bottom, spacerSeparation, containerSize);
                    }
                });
            }, {
                root: this.scrollElement, rootMargin: `${rootMargin}px`
            });
            this.intersectionObserver.observe(this.spacerBefore);
            this.intersectionObserver.observe(this.spacerAfter);

            // After each render, refresh the info about intersections
            this.mutationObserverBefore = new MutationObserver((): void => {
                this.intersectionObserver.unobserve(this.spacerBefore);                
                this.intersectionObserver.observe(this.spacerBefore);
            });
            this.mutationObserverBefore.observe(this.spacerBefore, { attributes: true });

            this.mutationObserverAfter = new MutationObserver((): void => {
                this.intersectionObserver.unobserve(this.spacerAfter);
                this.intersectionObserver.observe(this.spacerAfter);
            });
            this.mutationObserverAfter.observe(this.spacerAfter, { attributes: true })

            

        }

        resize(ev: Event): void {
            this.component.invokeMethodAsync('ResizeHandler', this.scrollElement.clientWidth);
        }

        disconnect(): void {

            this.events.off(window, 'resize', this.resize);
            this.events.dispose();
            
            this.mutationObserverBefore.disconnect();
            this.mutationObserverAfter.disconnect();

            this.intersectionObserver.unobserve(this.spacerBefore);
            this.intersectionObserver.unobserve(this.spacerAfter);
            this.intersectionObserver.disconnect();

        }



        getAverageHeight(): number {
            let calculate: boolean = false;
            let averageHeight: number = 0;
            let newItems: SerializableDictionary<number> = {};
            for (let i = 0; i < this.surfaceElement.children.length; i++) {
                let item = this.surfaceElement.children.item(i);
                let index = item.getAttribute("data-item-index");
                if (index != null && !this.cachedSizes.has(index) && this.cachedSizes.get(index) != item.clientHeight) {
                    this.cachedSizes.set(index, item.clientHeight);
                    newItems[index] = item.clientHeight;
                    calculate = true;
                }
            }
            if (calculate) {
                this.component.invokeMethodAsync("UpdateHeightCache", newItems);
                averageHeight = [...this.cachedSizes.values()].reduce((p, c, i, a) => p + c) / this.cachedSizes.size;
            }

            return averageHeight;
        }

        updateItemHeights(): void {



        }

    }

    export function getInitialAverageHeight(id: number) : number {
        let list = cachedLists.get(id);
        if (list == null) {
            return 0;
        } else {
            return list.getAverageHeight();
        }
    }
    
    export function initialize(component: DotNetReferenceType, spacerBefore: HTMLElement, spacerAfter: HTMLElement, reset: boolean=false): any {

        let list: List = new List(component, spacerBefore, spacerAfter);
        cachedLists.set(list.id, list);

        return list.id;
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

(<any>window)['FluentUIList'] = FluentUIList || {};


