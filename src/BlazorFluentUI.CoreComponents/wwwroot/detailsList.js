import * as FluentUIBaseComponent from './baseComponent.js';
const MOUSEDOWN_PRIMARY_BUTTON = 0; // for mouse down event we are using ev.button property, 0 means left button
const MOUSEMOVE_PRIMARY_BUTTON = 1; // for mouse move event we are using ev.buttons property, 1 means left button
const detailHeaders = new Map();
export function registerDetailsHeader(dotNet, root) {
    let detailHeader = new DetailsHeader(dotNet, root);
    detailHeaders.set(dotNet._id, detailHeader);
}
export function unregisterDetailsHeader(dotNet) {
    let detailHeader = detailHeaders.get(dotNet._id);
    detailHeader.dispose();
    detailHeaders.delete(dotNet._id);
}
class DetailsHeader {
    constructor(dotNet, root) {
        this._onRootMouseDown = async (ev) => {
            const columnIndexAttr = ev.target.getAttribute('data-sizer-index');
            const columnIndex = Number(columnIndexAttr);
            if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                // Ignore anything except the primary button.
                return;
            }
            await this.dotNet.invokeMethodAsync("OnSizerMouseDown", columnIndex, ev.clientX);
            ev.preventDefault();
            ev.stopPropagation();
        };
        this._onRootDblClick = async (ev) => {
            const columnIndexAttr = ev.target.getAttribute('data-sizer-index');
            const columnIndex = Number(columnIndexAttr);
            if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                // Ignore anything except the primary button.
                return;
            }
            await this.dotNet.invokeMethodAsync("OnDoubleClick", columnIndex);
        };
        this.dotNet = dotNet;
        this.root = root;
        this.events = new FluentUIBaseComponent.EventGroup(this);
        this.events.on(root, 'mousedown', this._onRootMouseDown);
        this.events.on(root, 'dblclick', this._onRootDblClick);
    }
    dispose() {
        this.events.dispose();
    }
}
