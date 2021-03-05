/// <reference path="../../BlazorFluentUI.BaseComponent/wwwroot/baseComponent.ts" />
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var BlazorFluentUISelectionZone;
(function (BlazorFluentUISelectionZone) {
    const SELECTION_DISABLED_ATTRIBUTE_NAME = 'data-selection-disabled';
    const SELECTION_INDEX_ATTRIBUTE_NAME = 'data-selection-index';
    const SELECTION_TOGGLE_ATTRIBUTE_NAME = 'data-selection-toggle';
    const SELECTION_INVOKE_ATTRIBUTE_NAME = 'data-selection-invoke';
    const SELECTION_INVOKE_TOUCH_ATTRIBUTE_NAME = 'data-selection-touch-invoke';
    const SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME = 'data-selection-all-toggle';
    const SELECTION_SELECT_ATTRIBUTE_NAME = 'data-selection-select';
    const selectionZones = new Map();
    function registerSelectionZone(dotNet, root, props) {
        let selectionZone = new SelectionZone(dotNet, root, props);
        selectionZones.set(dotNet._id, selectionZone);
    }
    BlazorFluentUISelectionZone.registerSelectionZone = registerSelectionZone;
    function updateProps(dotNet, props) {
        let selectionZone = selectionZones.get(dotNet._id);
        if (selectionZone !== null) {
            selectionZone.props = props;
        }
    }
    BlazorFluentUISelectionZone.updateProps = updateProps;
    function unregisterSelectionZone(dotNet) {
        let selectionZone = selectionZones.get(dotNet._id);
        selectionZone.dispose();
        selectionZones.delete(dotNet._id);
    }
    BlazorFluentUISelectionZone.unregisterSelectionZone = unregisterSelectionZone;
    let SelectionMode;
    (function (SelectionMode) {
        SelectionMode[SelectionMode["none"] = 0] = "none";
        SelectionMode[SelectionMode["single"] = 1] = "single";
        SelectionMode[SelectionMode["multiple"] = 2] = "multiple";
    })(SelectionMode = BlazorFluentUISelectionZone.SelectionMode || (BlazorFluentUISelectionZone.SelectionMode = {}));
    class SelectionZone {
        constructor(dotNet, root, props) {
            this.ignoreNextFocus = () => {
                this._handleNextFocus(false);
            };
            this._onMouseDownCapture = (ev) => {
                let target = ev.target;
                if (document.activeElement !== target && !FluentUIBaseComponent.elementContains(document.activeElement, target)) {
                    this.ignoreNextFocus();
                    return;
                }
                if (!FluentUIBaseComponent.elementContains(target, this.root)) {
                    return;
                }
                while (target !== this.root) {
                    if (this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                        this.ignoreNextFocus();
                        break;
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
            };
            this._onMouseDown = (ev) => __awaiter(this, void 0, void 0, function* () {
                this._updateModifiers(ev);
                let target = ev.target;
                const itemRoot = yield this._findItemRootAsync(target);
                // No-op if selection is disabled
                if (this._isSelectionDisabled(target)) {
                    return;
                }
                while (target !== this.root) {
                    if (this._hasAttribute(target, SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME)) {
                        break;
                    }
                    else if (itemRoot) {
                        if (this._hasAttribute(target, SELECTION_TOGGLE_ATTRIBUTE_NAME)) {
                            break;
                        }
                        else if (this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                            break;
                        }
                        else if ((target === itemRoot || this._shouldAutoSelect(target)) &&
                            !this._isShiftPressed &&
                            !this._isCtrlPressed &&
                            !this._isMetaPressed) {
                            yield this._onInvokeMouseDownAsync(ev, this._getItemIndex(itemRoot));
                            break;
                        }
                        else if (this.props.disableAutoSelectOnInputElements &&
                            (target.tagName === 'A' || target.tagName === 'BUTTON' || target.tagName === 'INPUT')) {
                            return;
                        }
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
            });
            this._onClick = (ev) => __awaiter(this, void 0, void 0, function* () {
                //const { enableTouchInvocationTarget = false } = this.props;
                this._updateModifiers(ev);
                let target = ev.target;
                const itemRoot = yield this._findItemRootAsync(target);
                const isSelectionDisabled = this._isSelectionDisabled(target);
                while (target !== this.root) {
                    if (this._hasAttribute(target, SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME)) {
                        if (!isSelectionDisabled) {
                            yield this._onToggleAllClickAsync(ev);
                        }
                        break;
                    }
                    else if (itemRoot) {
                        const index = this._getItemIndex(itemRoot);
                        if (this._hasAttribute(target, SELECTION_TOGGLE_ATTRIBUTE_NAME)) {
                            if (!isSelectionDisabled) {
                                if (this._isShiftPressed) {
                                    yield this._onItemSurfaceClickAsync(ev, index);
                                }
                                else {
                                    yield this._onToggleClickAsync(ev, index);
                                }
                            }
                            break;
                        }
                        else if ((this._isTouch &&
                            this.props.enableTouchInvocationTarget &&
                            this._hasAttribute(target, SELECTION_INVOKE_TOUCH_ATTRIBUTE_NAME)) ||
                            this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                            // Items should be invokable even if selection is disabled.
                            yield this._onInvokeClickAsync(ev, index);
                            break;
                        }
                        else if (target === itemRoot) {
                            if (!isSelectionDisabled) {
                                yield this._onItemSurfaceClickAsync(ev, index);
                            }
                            else {
                                // if selection is disabled, i.e. SelectionMode is none, then do a plain InvokeItem
                                yield this.dotNet.invokeMethodAsync("InvokeItem", index);
                            }
                            break;
                        }
                        else if (target.tagName === 'A' || target.tagName === 'BUTTON' || target.tagName === 'INPUT') {
                            return;
                        }
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
            });
            this.dotNet = dotNet;
            this.root = root;
            this.props = props;
            const win = FluentUIBaseComponent.getWindow(this.root);
            this._events = new FluentUIBaseComponent.EventGroup(this);
            this._async = new FluentUIBaseComponent.Async(this);
            this._events.on(win, 'keydown', this._updateModifiers);
            //this._events.on(document, 'click', this._findScrollParentAndTryClearOnEmptyClick);
            //this._events.on(document.body, 'touchstart', this._onTouchStartCapture, true);
            //this._events.on(document.body, 'touchend', this._onTouchStartCapture, true);
            //this._events.on(root, 'keydown', this._onKeyDown);
            //this._events.on(root, 'keydown', this._onKeyDownCapture, { capture: true });
            this._events.on(root, 'mousedown', this._onMouseDown);
            this._events.on(root, 'mousedown', this._onMouseDownCapture, { capture: true });
            this._events.on(root, 'click', this._onClick);
            //this._events.on(root, 'focus', this._onFocus, { capture: true });
            //this._events.on(root, 'doubleclick', this._onDoubleClick);
            //this._events.on(root, 'contextmenu', this._onContextMenu);
        }
        updateProps(props) {
            this.props = props;
        }
        dispose() {
            this._events.dispose();
            this._async.dispose();
        }
        _updateModifiers(ev) {
            this._isShiftPressed = ev.shiftKey;
            this._isCtrlPressed = ev.ctrlKey;
            this._isMetaPressed = ev.metaKey;
            const keyCode = ev.keyCode;
            this._isTabPressed = keyCode ? keyCode === 9 /* tab */ : false;
            //console.log('updatemodifiers');
        }
        _onInvokeMouseDownAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                // Only do work if item is not selected.
                var selected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                if (selected) {
                    return;
                }
                yield this._clearAndSelectIndexAsync(index);
            });
        }
        _clearAndSelectIndexAsync(index) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                let isAlreadySingleSelected = false;
                let selectedCount = yield this.dotNet.invokeMethodAsync("GetSelectedCount");
                if (selectedCount) {
                    var indexSelected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                    isAlreadySingleSelected = indexSelected;
                }
                if (!isAlreadySingleSelected) {
                    const isModal = this.props.isModal;
                    //await this.dotNet.invokeMethodAsync("ClearAndSelectIndex", index);
                    //selection.setChangeEvents(false);
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                    //selection.setAllSelected(false);
                    yield this.dotNet.invokeMethodAsync("SetAllSelected", false);
                    //selection.setIndexSelected(index, true, true);
                    yield this.dotNet.invokeMethodAsync("SetIndexSelected", index, true, true);
                    if (isModal || (this.props.enterModalOnTouch && this._isTouch)) {
                        yield this.dotNet.invokeMethodAsync("SetModal", true);
                        if (this._isTouch) {
                            this._setIsTouch(false);
                        }
                    }
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                    //selection.setChangeEvents(true);
                }
            });
        }
        _setIsTouch(isTouch) {
            if (this._isTouchTimeoutId) {
                this._async.clearTimeout(this._isTouchTimeoutId);
                this._isTouchTimeoutId = undefined;
            }
            this._isTouch = true;
            if (isTouch) {
                this._async.setTimeout(() => {
                    this._isTouch = false;
                }, 300);
            }
        }
        _hasAttribute(element, attributeName) {
            let isToggle = false;
            while (!isToggle && element !== this.root) {
                isToggle = element.getAttribute(attributeName) === 'true';
                element = FluentUIBaseComponent.getParent(element);
            }
            return isToggle;
        }
        _handleNextFocus(handleFocus) {
            if (this._shouldHandleFocusTimeoutId) {
                this._async.clearTimeout(this._shouldHandleFocusTimeoutId);
                this._shouldHandleFocusTimeoutId = undefined;
            }
            this._shouldHandleFocus = handleFocus;
            if (handleFocus) {
                this._async.setTimeout(() => {
                    this._shouldHandleFocus = false;
                }, 100);
            }
        }
        _findItemRootAsync(target) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                while (target !== this.root) {
                    const indexValue = target.getAttribute(SELECTION_INDEX_ATTRIBUTE_NAME);
                    const index = Number(indexValue);
                    if (indexValue !== null && index >= 0) {
                        let count = yield this.dotNet.invokeMethodAsync("GetItemsLength");
                        if (index < count) {
                            break;
                        }
                    }
                    target = FluentUIBaseComponent.getParent(target);
                }
                if (target === this.root) {
                    return undefined;
                }
                return target;
            });
        }
        _isSelectionDisabled(target) {
            if (this.props.selectionMode === SelectionMode.none) {
                return true;
            }
            while (target !== this.root) {
                if (this._hasAttribute(target, SELECTION_DISABLED_ATTRIBUTE_NAME)) {
                    return true;
                }
                target = FluentUIBaseComponent.getParent(target);
            }
            return false;
        }
        _shouldAutoSelect(element) {
            return this._hasAttribute(element, SELECTION_SELECT_ATTRIBUTE_NAME);
        }
        _getItemIndex(itemRoot) {
            return Number(itemRoot.getAttribute(SELECTION_INDEX_ATTRIBUTE_NAME));
        }
        _onToggleAllClickAsync(ev) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                const selectionMode = this.props.selectionMode;
                if (selectionMode === SelectionMode.multiple) {
                    //selection.toggleAllSelected();
                    yield this.dotNet.invokeMethodAsync("ToggleAllSelected");
                    ev.stopPropagation();
                    ev.preventDefault();
                }
            });
        }
        _onToggleClickAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection } = this.props;
                const selectionMode = this.props.selectionMode;
                //selection.setChangeEvents(false);
                yield this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                if (this.props.enterModalOnTouch && this._isTouch) { // && !selection.isIndexSelected(index) && selection.setModal) {
                    let isSelected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                    if (!isSelected) {
                        yield this.dotNet.invokeMethodAsync("SetModal", true);
                        this._setIsTouch(false);
                    }
                }
                if (selectionMode === SelectionMode.multiple) {
                    //selection.toggleIndexSelected(index);
                    yield this.dotNet.invokeMethodAsync("ToggleIndexSelected", index);
                }
                else if (selectionMode === SelectionMode.single) {
                    //const isSelected = selection.isIndexSelected(index);
                    let isSelected = yield this.dotNet.invokeMethodAsync("IsIndexSelected", index);
                    const isModal = this.props.isModal; //selection.isModal && selection.isModal();
                    //selection.setAllSelected(false);
                    yield this.dotNet.invokeMethodAsync("SetAllSelected", false);
                    //selection.setIndexSelected(index, !isSelected, true);
                    yield this.dotNet.invokeMethodAsync("SetIndexSelected", index, !isSelected, true);
                    if (isModal) {
                        // Since the above call to setAllSelected(false) clears modal state,
                        // restore it. This occurs because the SelectionMode of the Selection
                        // may differ from the SelectionZone.
                        //selection.setModal(true);
                        yield this.dotNet.invokeMethodAsync("SetModal", true);
                    }
                }
                else {
                    //selection.setChangeEvents(true);
                    yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                    return;
                }
                //selection.setChangeEvents(true);
                yield this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                ev.stopPropagation();
                // NOTE: ev.preventDefault is not called for toggle clicks, because this will kill the browser behavior
                // for checkboxes if you use a checkbox for the toggle.
            });
        }
        _onItemSurfaceClickAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                const isToggleModifierPressed = this._isCtrlPressed || this._isMetaPressed;
                const selectionMode = this.props.selectionMode;
                if (selectionMode === SelectionMode.multiple) {
                    if (this._isShiftPressed && !this._isTabPressed) {
                        //selection.selectToIndex(index, !isToggleModifierPressed);
                        yield this.dotNet.invokeMethodAsync("SelectToIndex", index, !isToggleModifierPressed);
                    }
                    else if (isToggleModifierPressed) {
                        //selection.toggleIndexSelected(index);
                        yield this.dotNet.invokeMethodAsync("ToggleIndexSelected", index);
                    }
                    else {
                        yield this._clearAndSelectIndexAsync(index);
                    }
                }
                else if (selectionMode === SelectionMode.single) {
                    yield this._clearAndSelectIndexAsync(index);
                }
            });
        }
        _onInvokeClickAsync(ev, index) {
            return __awaiter(this, void 0, void 0, function* () {
                //const { selection, onItemInvoked } = this.props;
                if (this.props.onItemInvokeSet) {
                    yield this.dotNet.invokeMethodAsync("InvokeItem", index);
                    //onItemInvoked(selection.getItems()[index], index, ev.nativeEvent);
                    ev.preventDefault();
                    ev.stopPropagation();
                }
            });
        }
    }
})(BlazorFluentUISelectionZone || (BlazorFluentUISelectionZone = {}));
window['BlazorFluentUISelectionZone'] = BlazorFluentUISelectionZone || {};
