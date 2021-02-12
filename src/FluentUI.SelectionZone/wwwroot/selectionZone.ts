/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />


namespace FluentUISelectionZone {

    type IRectangle = FluentUIBaseComponent.IRectangle;
    type EventGroup = FluentUIBaseComponent.EventGroup;
    type EventParams = FluentUIBaseComponent.EventParams;

    interface DotNetReferenceType {
        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
        _id: number;
    }

    const SELECTION_DISABLED_ATTRIBUTE_NAME = 'data-selection-disabled';
    const SELECTION_INDEX_ATTRIBUTE_NAME = 'data-selection-index';
    const SELECTION_TOGGLE_ATTRIBUTE_NAME = 'data-selection-toggle';
    const SELECTION_INVOKE_ATTRIBUTE_NAME = 'data-selection-invoke';
    const SELECTION_INVOKE_TOUCH_ATTRIBUTE_NAME = 'data-selection-touch-invoke';
    const SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME = 'data-selection-all-toggle';
    const SELECTION_SELECT_ATTRIBUTE_NAME = 'data-selection-select';
 
    const selectionZones = new Map<number, SelectionZone>();

    export function registerSelectionZone(dotNet: DotNetReferenceType, root: HTMLElement, props: ISelectionZoneProps) {
        let selectionZone = new SelectionZone(dotNet, root, props);
        selectionZones.set(dotNet._id, selectionZone);
    }

    export function updateProps(dotNet: DotNetReferenceType, props: ISelectionZoneProps) {
        let selectionZone = selectionZones.get(dotNet._id);
        if (selectionZone !== null) {
            selectionZone.props = props;
        }
    }

    export function unregisterSelectionZone(dotNet: DotNetReferenceType) {
        let selectionZone = selectionZones.get(dotNet._id);
        selectionZone.dispose();
        selectionZones.delete(dotNet._id);
    }

    export enum SelectionMode {
        none,
        single,
        multiple
    }

    interface ISelectionZoneProps {
        selectionMode: SelectionMode;
        isModal: boolean;
        disableAutoSelectOnInputElements: boolean;
        enterModalOnTouch: boolean;
        enableTouchInvocationTarget: boolean;
        onItemInvokeSet: boolean;
    }



    class SelectionZone {

        dotNet: DotNetReferenceType;
        root: HTMLElement;
   
        props: ISelectionZoneProps;
        private _events: EventGroup;
        private _async: FluentUIBaseComponent.Async;
        private _isCtrlPressed: boolean;
        private _isShiftPressed: boolean;
        private _isMetaPressed: boolean;
        private _isTabPressed: boolean;
        private _shouldHandleFocus: boolean;
        private _shouldHandleFocusTimeoutId: number | undefined;
        private _isTouch: boolean;
        private _isTouchTimeoutId: number | undefined;

        constructor(dotNet: DotNetReferenceType, root: HTMLElement, props: ISelectionZoneProps) {
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

        public updateProps(props: ISelectionZoneProps): void {
            this.props = props;
            
        }

        public dispose(): void {
            
            this._events.dispose();
            this._async.dispose();
        }

        private _updateModifiers(ev: KeyboardEvent | MouseEvent) {
            this._isShiftPressed = ev.shiftKey;
            this._isCtrlPressed = ev.ctrlKey;
            this._isMetaPressed = ev.metaKey;

            const keyCode = (ev as KeyboardEvent).keyCode;
            this._isTabPressed = keyCode ? keyCode === FluentUIBaseComponent.KeyCodes.tab : false;
            //console.log('updatemodifiers');
        }

        public ignoreNextFocus = (): void => {
            this._handleNextFocus(false);
        };


        private _onMouseDownCapture = (ev: MouseEvent): void => {
            let target = ev.target as HTMLElement;

            if (document.activeElement !== target && !FluentUIBaseComponent.elementContains(document.activeElement as HTMLElement, target)) {
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

                target = FluentUIBaseComponent.getParent(target) as HTMLElement;
            }
        };

        private _onMouseDown = async (ev: MouseEvent): Promise<void> => {
            this._updateModifiers(ev);

            let target = ev.target as HTMLElement;
            const itemRoot = await this._findItemRootAsync(target);

            // No-op if selection is disabled
            if (this._isSelectionDisabled(target)) {
                return;
            }

            while (target !== this.root) {
                if (this._hasAttribute(target, SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME)) {
                    break;
                } else if (itemRoot) {
                    if (this._hasAttribute(target, SELECTION_TOGGLE_ATTRIBUTE_NAME)) {
                        break;
                    } else if (this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)) {
                        break;
                    } else if (
                        (target === itemRoot || this._shouldAutoSelect(target)) &&
                        !this._isShiftPressed &&
                        !this._isCtrlPressed &&
                        !this._isMetaPressed
                    ) {
                        await this._onInvokeMouseDownAsync(ev, this._getItemIndex(itemRoot));
                        break;
                    } else if (
                        this.props.disableAutoSelectOnInputElements &&
                        (target.tagName === 'A' || target.tagName === 'BUTTON' || target.tagName === 'INPUT')
                    ) {
                        return;
                    }
                }

                target = FluentUIBaseComponent.getParent(target) as HTMLElement;
            }
        };

        private async _onInvokeMouseDownAsync(
            ev: MouseEvent | KeyboardEvent,
            index: number,
        ): Promise<void> {
            
            // Only do work if item is not selected.
            var selected = await this.dotNet.invokeMethodAsync<boolean>("IsIndexSelected", index);
            if (selected) {
                return;
            }

            await this._clearAndSelectIndexAsync(index);
        }

        private async _clearAndSelectIndexAsync(index: number): Promise<void> {
            //const { selection } = this.props;
            let isAlreadySingleSelected: boolean = false;
            let selectedCount = await this.dotNet.invokeMethodAsync("GetSelectedCount");
            if (selectedCount) {
                var indexSelected = await this.dotNet.invokeMethodAsync<boolean>("IsIndexSelected", index);
                isAlreadySingleSelected = indexSelected;
            }


            if (!isAlreadySingleSelected) {
                const isModal = this.props.isModal;
                //await this.dotNet.invokeMethodAsync("ClearAndSelectIndex", index);
                //selection.setChangeEvents(false);
                await this.dotNet.invokeMethodAsync("SetChangeEvents", false);
                //selection.setAllSelected(false);
                await this.dotNet.invokeMethodAsync("SetAllSelected", false);
                //selection.setIndexSelected(index, true, true);
                await this.dotNet.invokeMethodAsync("SetIndexSelected", index, true, true);
                if (isModal || (this.props.enterModalOnTouch && this._isTouch)) {
                    await this.dotNet.invokeMethodAsync("SetModal", true);
                    if (this._isTouch) {
                        this._setIsTouch(false);
                    }
                }
                await this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                //selection.setChangeEvents(true);
            }
        }

        private _onClick = async (ev: MouseEvent): Promise<void> => {
            //const { enableTouchInvocationTarget = false } = this.props;

            this._updateModifiers(ev);

            let target = ev.target as HTMLElement;
            const itemRoot = await this._findItemRootAsync(target);

            const isSelectionDisabled = this._isSelectionDisabled(target);

            while (target !== this.root) {
                if (this._hasAttribute(target, SELECTALL_TOGGLE_ALL_ATTRIBUTE_NAME)) {
                    if (!isSelectionDisabled) {
                        await this._onToggleAllClickAsync(ev);
                    }
                    break;
                } else if (itemRoot) {
                    const index = this._getItemIndex(itemRoot);

                    if (this._hasAttribute(target, SELECTION_TOGGLE_ATTRIBUTE_NAME)) {
                        if (!isSelectionDisabled) {
                            if (this._isShiftPressed) {
                                await this._onItemSurfaceClickAsync(ev, index);
                            } else {
                                await this._onToggleClickAsync(ev, index);
                            }
                        }
                        break;
                    } else if (
                        (this._isTouch &&
                            this.props.enableTouchInvocationTarget &&
                            this._hasAttribute(target, SELECTION_INVOKE_TOUCH_ATTRIBUTE_NAME)) ||
                        this._hasAttribute(target, SELECTION_INVOKE_ATTRIBUTE_NAME)
                    ) {
                        // Items should be invokable even if selection is disabled.
                        await this._onInvokeClickAsync(ev, index);
                        break;
                    } else if (target === itemRoot) {
                        if (!isSelectionDisabled) {
                            await this._onItemSurfaceClickAsync(ev, index);
                        } else {
                            // if selection is disabled, i.e. SelectionMode is none, then do a plain InvokeItem
                            await this.dotNet.invokeMethodAsync("InvokeItem", index);
                        }
                        break;
                    } else if (target.tagName === 'A' || target.tagName === 'BUTTON' || target.tagName === 'INPUT') {
                        return;
                    }
                }

                target = FluentUIBaseComponent.getParent(target) as HTMLElement;
            }
        };

        private _setIsTouch(isTouch: boolean): void {
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


        private _hasAttribute(element: HTMLElement, attributeName: string): boolean {
            let isToggle = false;

            while (!isToggle && element !== this.root) {
                isToggle = element.getAttribute(attributeName) === 'true';
                element = FluentUIBaseComponent.getParent(element) as HTMLElement;
            }

            return isToggle;
        }

        private _handleNextFocus(handleFocus: boolean): void {
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

        private async _findItemRootAsync(target: HTMLElement): Promise<HTMLElement> | undefined {
            //const { selection } = this.props;

            while (target !== this.root) {
                const indexValue = target.getAttribute(SELECTION_INDEX_ATTRIBUTE_NAME);
                const index = Number(indexValue);



                if (indexValue !== null && index >= 0) {
                    let count = await this.dotNet.invokeMethodAsync<number>("GetItemsLength");
                    if (index < count) {
                        break;
                    }
                }

                target = FluentUIBaseComponent.getParent(target) as HTMLElement;
            }

            if (target === this.root) {
                return undefined;
            }

            return target;
        }

        private _isSelectionDisabled(target: HTMLElement): boolean {
            if (this.props.selectionMode ===  SelectionMode.none) {
                return true;
            }

            while (target !== this.root) {
                if (this._hasAttribute(target, SELECTION_DISABLED_ATTRIBUTE_NAME)) {
                    return true;
                }
                target = FluentUIBaseComponent.getParent(target) as HTMLElement;
            }

            return false;
        }

        private _shouldAutoSelect(element: HTMLElement): boolean {
            return this._hasAttribute(element, SELECTION_SELECT_ATTRIBUTE_NAME);
        }

        private _getItemIndex(itemRoot: HTMLElement): number {
            return Number(itemRoot.getAttribute(SELECTION_INDEX_ATTRIBUTE_NAME));
        }

        private async _onToggleAllClickAsync(ev: MouseEvent): Promise<void> {
            //const { selection } = this.props;

            const selectionMode = this.props.selectionMode;

            if (selectionMode === SelectionMode.multiple) {
                //selection.toggleAllSelected();
                await this.dotNet.invokeMethodAsync("ToggleAllSelected");
                ev.stopPropagation();
                ev.preventDefault();
            }
        }

        private async _onToggleClickAsync(ev: MouseEvent | KeyboardEvent, index: number): Promise<void> {
            //const { selection } = this.props;

            const selectionMode = this.props.selectionMode;

            //selection.setChangeEvents(false);
            await this.dotNet.invokeMethodAsync("SetChangeEvents", false);

            if (this.props.enterModalOnTouch && this._isTouch) { // && !selection.isIndexSelected(index) && selection.setModal) {
                let isSelected = await this.dotNet.invokeMethodAsync<boolean>("IsIndexSelected", index);
                if (!isSelected) {
                    await this.dotNet.invokeMethodAsync("SetModal", true);
                    this._setIsTouch(false);
                }
            }

            if (selectionMode === SelectionMode.multiple) {
                //selection.toggleIndexSelected(index);
                await this.dotNet.invokeMethodAsync("ToggleIndexSelected", index);
            } else if (selectionMode === SelectionMode.single) {
                //const isSelected = selection.isIndexSelected(index);
                let isSelected = await this.dotNet.invokeMethodAsync<boolean>("IsIndexSelected", index);
                const isModal = this.props.isModal; //selection.isModal && selection.isModal();
                //selection.setAllSelected(false);
                await this.dotNet.invokeMethodAsync("SetAllSelected", false);
                //selection.setIndexSelected(index, !isSelected, true);
                await this.dotNet.invokeMethodAsync("SetIndexSelected", index, !isSelected, true);
                if (isModal) {
                    // Since the above call to setAllSelected(false) clears modal state,
                    // restore it. This occurs because the SelectionMode of the Selection
                    // may differ from the SelectionZone.
                    //selection.setModal(true);
                    await this.dotNet.invokeMethodAsync("SetModal", true);
                }
            } else {
                //selection.setChangeEvents(true);
                await this.dotNet.invokeMethodAsync("SetChangeEvents", true);
                return;
            }

            //selection.setChangeEvents(true);
            await this.dotNet.invokeMethodAsync("SetChangeEvents", true);

            ev.stopPropagation();

            // NOTE: ev.preventDefault is not called for toggle clicks, because this will kill the browser behavior
            // for checkboxes if you use a checkbox for the toggle.
        }

        private async _onItemSurfaceClickAsync(ev: Event, index: number): Promise<void> {

            const isToggleModifierPressed = this._isCtrlPressed || this._isMetaPressed;

            const selectionMode = this.props.selectionMode;

            if (selectionMode === SelectionMode.multiple) {
                if (this._isShiftPressed && !this._isTabPressed) {
                    //selection.selectToIndex(index, !isToggleModifierPressed);
                    await this.dotNet.invokeMethodAsync("SelectToIndex", index, !isToggleModifierPressed);
                } else if (isToggleModifierPressed) {
                    //selection.toggleIndexSelected(index);
                    await this.dotNet.invokeMethodAsync("ToggleIndexSelected", index);
                } else {
                    await this._clearAndSelectIndexAsync(index);
                }
            } else if (selectionMode === SelectionMode.single) {
                await this._clearAndSelectIndexAsync(index);
            }
        }

        private async _onInvokeClickAsync(ev: MouseEvent | KeyboardEvent, index: number): Promise<void> {
            //const { selection, onItemInvoked } = this.props;

            if (this.props.onItemInvokeSet) {
                await this.dotNet.invokeMethodAsync("InvokeItem", index);
                //onItemInvoked(selection.getItems()[index], index, ev.nativeEvent);
                ev.preventDefault();
                ev.stopPropagation();
            }
        }



    }


}

(<any>window)['FluentUISelectionZone'] = FluentUISelectionZone || {};

