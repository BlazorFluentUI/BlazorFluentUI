/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />

namespace FluentUIDetailsList {

    type EventGroup = FluentUIBaseComponent.EventGroup;

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
        _id: number;
    }

    const MOUSEDOWN_PRIMARY_BUTTON = 0; // for mouse down event we are using ev.button property, 0 means left button
    const MOUSEMOVE_PRIMARY_BUTTON = 1; // for mouse move event we are using ev.buttons property, 1 means left button

    const detailHeaders = new Map<number, DetailsHeader>();

    export function registerDetailsHeader(dotNet: DotNetReferenceType, root: HTMLElement) {
        let detailHeader = new DetailsHeader(dotNet, root);
        detailHeaders.set(dotNet._id, detailHeader);
    }

    export function unregisterDetailsHeader(dotNet: DotNetReferenceType) {
        let detailHeader = detailHeaders.get(dotNet._id);
        detailHeader.dispose();
        detailHeaders.delete(dotNet._id);
    }

    class DetailsHeader {
        dotNet: DotNetReferenceType;
        root: HTMLElement;

        events: EventGroup;

        constructor(dotNet: DotNetReferenceType, root: HTMLElement) {
            this.dotNet = dotNet;
            this.root = root;            

            this.events = new FluentUIBaseComponent.EventGroup(this);
            
            this.events.on(root, 'mousedown', this._onRootMouseDown);
            this.events.on(root, 'dblclick', this._onRootDblClick);
        }

        public dispose(): void {
            this.events.dispose();
        }

        private _onRootMouseDown = async (ev: MouseEvent): Promise<void> => {
            const columnIndexAttr = (ev.target as HTMLElement).getAttribute('data-sizer-index');
            const columnIndex = Number(columnIndexAttr);

            if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                // Ignore anything except the primary button.
                return;
            }

            await this.dotNet.invokeMethodAsync<void>("OnSizerMouseDown", columnIndex, ev.clientX);

            ev.preventDefault();
            ev.stopPropagation();
        };

        private _onRootDblClick = async (ev: MouseEvent): Promise<void> => {
            const columnIndexAttr = (ev.target as HTMLElement).getAttribute('data-sizer-index');
            const columnIndex = Number(columnIndexAttr);

            if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                // Ignore anything except the primary button.
                return;
            }

            await this.dotNet.invokeMethodAsync<void>("OnDoubleClick", columnIndex);

           
        };

    }

}

(<any>window)['FluentUIDetailsList'] = FluentUIDetailsList || {};