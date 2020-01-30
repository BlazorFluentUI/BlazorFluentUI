/// <reference path="baseComponent.d.ts" />

namespace BlazorFabricBasicList {

    interface DotNetReferenceType {
        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }
    interface Map<T> {
        [K: number]: T;
    }

    interface IRectangle {
        left: number;
        top: number;
        width: number;
        height: number;
        right?: number;
        bottom?: number;
    }

    var count = 0;
    var allInstances: Map<BasicListInternal> = {};

    export function register(basicList: DotNetReferenceType, rootElement: HTMLElement): number {
        let currentId = count++;
        allInstances[currentId] = new BasicListInternal(basicList, rootElement);
        return currentId;
    }

    export function unregister(id: number): void {
        let basicList = allInstances[id];
        if (basicList) {
            basicList.unRegister();
        }
        delete allInstances[id];
    }

    export function getScrollDimensions(id: number): IRectangle {
        let basicList = <BasicListInternal>allInstances[id];
        if (basicList != null) {
            return basicList.getScrollDimensions();
        }
        return null;
    }

    export function setScrollTop(id: number, top: number): void {
        let basicList = <BasicListInternal>allInstances[id];
        if (basicList != null) {
            basicList.setScrollTop(top);
        }
    }
    

    class BasicListInternal {
        private _dotNetRef: DotNetReferenceType;
        private _rootElement: HTMLElement;
        private _scrollElement: HTMLElement;

        constructor(basicList: DotNetReferenceType, rootElement:HTMLElement) {
            this._dotNetRef = basicList;
            this._rootElement = rootElement;

            this._scrollElement = BlazorFabricBaseComponent.findScrollableParent(this._rootElement);

            if (this._scrollElement) {
                this._scrollElement.addEventListener("scroll", this.debouncedScroll);
            }
        }

        public unRegister(): void {
            if (this._scrollElement) {
                this._scrollElement.removeEventListener("scroll", this.debouncedScroll);
            }
        }

        private debouncedScroll = (ev : UIEvent) => {
            //BlazorFabricBaseComponent.debounce((ev: UIEvent) => {
                this._dotNetRef.invokeMethodAsync("ScrollHandler");
            //}, 100, { leading: true });
        }

        public getScrollDimensions(): IRectangle {
            if (this._scrollElement) {
                return { height: this._scrollElement.scrollHeight, top: this._scrollElement.scrollTop ? this._scrollElement.scrollTop : 0, left:0,width:0 };
            }
            return null;
        }

        public setScrollTop(top: number): void {
            if (this._scrollElement) {
                this._scrollElement.scrollTop = top;
            }
        }

    }
}




(<any>window)['BlazorFabricBasicList'] = BlazorFabricBasicList || {};

