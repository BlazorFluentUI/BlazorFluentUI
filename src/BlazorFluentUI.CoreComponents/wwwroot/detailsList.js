var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
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
        this._onRootMouseDown = (ev) => __awaiter(this, void 0, void 0, function* () {
            const columnIndexAttr = ev.target.getAttribute('data-sizer-index');
            const columnIndex = Number(columnIndexAttr);
            if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                // Ignore anything except the primary button.
                return;
            }
            yield this.dotNet.invokeMethodAsync("OnSizerMouseDown", columnIndex, ev.clientX);
            ev.preventDefault();
            ev.stopPropagation();
        });
        this._onRootDblClick = (ev) => __awaiter(this, void 0, void 0, function* () {
            const columnIndexAttr = ev.target.getAttribute('data-sizer-index');
            const columnIndex = Number(columnIndexAttr);
            if (columnIndexAttr === null || ev.button !== MOUSEDOWN_PRIMARY_BUTTON) {
                // Ignore anything except the primary button.
                return;
            }
            yield this.dotNet.invokeMethodAsync("OnDoubleClick", columnIndex);
        });
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
