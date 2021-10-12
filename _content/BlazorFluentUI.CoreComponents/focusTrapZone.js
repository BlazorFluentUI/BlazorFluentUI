import * as FluentUIBaseComponent from './baseComponent.js';
const HIDDEN_FROM_ACC_TREE = 'data-is-hidden-from-acc-tree';
class FocusTrapZoneInternal {
    constructor(focusTrapZoneProps, dotNetRef) {
        this._hasFocus = true;
        this._onRootFocus = (ev) => {
            //if (this._props.onFocus) {
            //    this._props.onFocus(ev);
            //}
            this._hasFocus = true;
        };
        this._onRootBlur = (ev) => {
            //if (this.props.onBlur) {
            //    this.props.onBlur(ev);
            //}
            let relatedTarget = ev.relatedTarget;
            if (ev.relatedTarget === null) {
                // In IE11, due to lack of support, event.relatedTarget is always
                // null making every onBlur call to be "outside" of the ComboBox
                // even when it's not. Using document.activeElement is another way
                // for us to be able to get what the relatedTarget without relying
                // on the event
                relatedTarget = document.activeElement;
            }
            if (!FluentUIBaseComponent.elementContains(this._props.rootElement, relatedTarget)) {
                this._hasFocus = false;
            }
        };
        this._onFirstBumperFocus = () => {
            this._onBumperFocus(true);
        };
        this._onLastBumperFocus = () => {
            this._onBumperFocus(false);
        };
        this._onBumperFocus = (isFirstBumper) => {
            if (!this._props.rootElement) {
                return;
            }
            const currentBumper = (isFirstBumper === this._hasFocus ? this._props.lastBumper : this._props.firstBumper);
            if (this._props.rootElement) {
                const nextFocusable = isFirstBumper === this._hasFocus
                    ? FluentUIBaseComponent.getLastTabbable(this._props.rootElement, currentBumper, true, false)
                    : FluentUIBaseComponent.getFirstTabbable(this._props.rootElement, currentBumper, true, false);
                if (nextFocusable) {
                    if (this._isBumper(nextFocusable)) {
                        // This can happen when FTZ contains no tabbable elements. focus will take care of finding a focusable element in FTZ.
                        this._findElementAndFocusAsync();
                    }
                    else {
                        nextFocusable.focus();
                    }
                }
            }
        };
        this._enableFocusTrapZone = () => {
            const { disabled = false } = this._props;
            if (disabled) {
                return;
            }
            FocusTrapZoneInternal._focusStack.push(this);
            this._bringFocusIntoZone();
            this._hideContentFromAccessibilityTree();
        };
        this._releaseFocusTrapZone = () => {
            const { ignoreExternalFocusing } = this._props;
            FocusTrapZoneInternal._focusStack = FocusTrapZoneInternal._focusStack.filter((value) => {
                return this !== value;
            });
            // try to focus element which triggered FocusTrapZone - prviously focused element outside trap zone
            //const doc = getDocument(this._props.rootElement);
            // @ts-ignore
            const activeElement = document.activeElement;
            if (!ignoreExternalFocusing &&
                this._previouslyFocusedElementOutsideTrapZone &&
                // @ts-ignore
                (this._props.rootElement.contains(activeElement) || activeElement === document.body)) {
                this._focusAsync(this._previouslyFocusedElementOutsideTrapZone);
            }
            // if last active focus trap zone is going to be released - show previously hidden content in accessibility tree
            const lastActiveFocusTrap = FocusTrapZoneInternal._focusStack.length && FocusTrapZoneInternal._focusStack[FocusTrapZoneInternal._focusStack.length - 1];
            if (!lastActiveFocusTrap) {
                this._showContentInAccessibilityTree();
            }
            else if (lastActiveFocusTrap._props.rootElement &&
                lastActiveFocusTrap._props.rootElement.hasAttribute(HIDDEN_FROM_ACC_TREE)) {
                lastActiveFocusTrap._props.rootElement?.removeAttribute(HIDDEN_FROM_ACC_TREE);
                lastActiveFocusTrap._props.rootElement?.removeAttribute('aria-hidden');
            }
        };
        this._findElementAndFocusAsync = () => {
            if (!this._props.rootElement) {
                return;
            }
            const { focusPreviouslyFocusedInnerElement, firstFocusableSelector } = this._props;
            if (focusPreviouslyFocusedInnerElement &&
                this._previouslyFocusedElementInTrapZone &&
                this._props.rootElement.contains(this._previouslyFocusedElementInTrapZone)) {
                // focus on the last item that had focus in the zone before we left the zone
                this._focusAsync(this._previouslyFocusedElementInTrapZone);
                return;
            }
            const focusSelector = firstFocusableSelector &&
                (typeof firstFocusableSelector === 'string' ? firstFocusableSelector : firstFocusableSelector());
            let firstFocusableChild = null;
            if (focusSelector) {
                firstFocusableChild = this._props.rootElement.querySelector(focusSelector);
            }
            // Fall back to first element if query selector did not match any elements.
            if (!firstFocusableChild) {
                firstFocusableChild = FluentUIBaseComponent.getNextElement(this._props.rootElement, this._props.rootElement.firstChild, false, false, false, true);
            }
            firstFocusableChild && this._focusAsync(firstFocusableChild);
        };
        this._onFocusCapture = (ev) => {
            //this._props.onFocusCapture && this._props.onFocusCapture(ev);
            if (ev.target !== ev.currentTarget && !this._isBumper(ev.target)) {
                // every time focus changes within the trap zone, remember the focused element so that
                // it can be restored if focus leaves the pane and returns via keystroke (i.e. via a call to this.focus(true))
                this._previouslyFocusedElementInTrapZone = ev.target;
            }
        };
        this._forceFocusInTrap = (ev, triggeredElement) => {
            if (FocusTrapZoneInternal._focusStack.length && this === FocusTrapZoneInternal._focusStack[FocusTrapZoneInternal._focusStack.length - 1]) {
                // @ts-ignore
                if (!this._props.rootElement.contains(triggeredElement)) {
                    this._findElementAndFocusAsync();
                    ev.preventDefault();
                    ev.stopPropagation();
                }
            }
        };
        this._handleOutsideFocus = (ev) => {
            // @ts-ignore
            const focusedElement = document.activeElement;
            focusedElement && this._forceFocusInTrap(ev, focusedElement);
        };
        this._handleOutsideClick = (ev) => {
            const clickedElement = ev.target;
            const { isClickableOutsideFocusTrap, focusTriggerOnOutsideClick } = this._props;
            if (!isClickableOutsideFocusTrap) {
                clickedElement && this._forceFocusInTrap(ev, clickedElement);
            }
            else if (!focusTriggerOnOutsideClick) {
                const isOutsideFocusTrapZone = this._props.rootElement && !this._props.rootElement.contains(clickedElement);
                const isOutsideTriggerElement = this._previouslyFocusedElementOutsideTrapZone &&
                    !this._previouslyFocusedElementOutsideTrapZone.contains(clickedElement);
                if (isOutsideFocusTrapZone && isOutsideTriggerElement) {
                    // set it to NULL, so the trigger will not be focused on componentWillUnmount
                    // @ts-ignore
                    this._previouslyFocusedElementOutsideTrapZone = null;
                }
            }
        };
        this._onKeyboardHandler = (ev) => {
            //if (this._props.onKeyDown) {
            //    this._props.onKeyDown(ev);
            //}
            // do not propogate keyboard events outside focus trap zone
            // https://github.com/microsoft/fluent-ui-react/pull/1180
            ev.stopPropagation();
        };
        this._getPreviouslyFocusedElementOutsideTrapZone = () => {
            const { elementToFocusOnDismiss } = this._props;
            let previouslyFocusedElement = this._previouslyFocusedElementOutsideTrapZone;
            if (elementToFocusOnDismiss && previouslyFocusedElement !== elementToFocusOnDismiss) {
                previouslyFocusedElement = elementToFocusOnDismiss;
            }
            else if (!previouslyFocusedElement) {
                // @ts-ignore
                previouslyFocusedElement = document.activeElement;
            }
            return previouslyFocusedElement;
        };
        this._hideContentFromAccessibilityTree = () => {
            const doc = document;
            // @ts-ignore
            const bodyChildren = (doc.body && doc.body.children) || [];
            // @ts-ignore
            if (bodyChildren.length && !doc.body.contains(this._props.rootElement)) {
                // In case popup render options will change
                /* eslint-disable-next-line no-console */
                console.warn('Body element does not contain trap zone element. Please, ensure the trap zone element is placed inside body, so it will work properly.');
            }
            for (let index = 0; index < bodyChildren.length; index++) {
                const currentChild = bodyChildren[index];
                const isOrHasFocusTrapZone = currentChild === this._props.rootElement || currentChild.contains(this._props.rootElement);
                const isAriaLiveRegion = currentChild.hasAttribute('aria-live');
                if (!isOrHasFocusTrapZone && !isAriaLiveRegion && currentChild.getAttribute('aria-hidden') !== 'true') {
                    currentChild.setAttribute('aria-hidden', 'true');
                    currentChild.setAttribute(HIDDEN_FROM_ACC_TREE, 'true');
                }
            }
        };
        this._showContentInAccessibilityTree = () => {
            const doc = document;
            // @ts-ignore
            const hiddenElements = doc.querySelectorAll(`[${HIDDEN_FROM_ACC_TREE}="true"]`);
            for (let index = 0; index < hiddenElements.length; index++) {
                const element = hiddenElements[index];
                element.removeAttribute('aria-hidden');
                element.removeAttribute(HIDDEN_FROM_ACC_TREE);
            }
        };
        this._props = focusTrapZoneProps;
        this._dotNetRef = dotNetRef;
        this._props.rootElement?.addEventListener("focus", this._onRootFocus, false);
        this._props.rootElement?.addEventListener("blur", this._onRootBlur, false);
        this._props.firstBumper?.addEventListener("focus", this._onFirstBumperFocus, false);
        this._props.lastBumper?.addEventListener("focus", this._onLastBumperFocus, false);
        //this._bringFocusIntoZone();
    }
    unRegister() {
        const activeElement = document.activeElement;
        this._props.rootElement?.removeEventListener("focus", this._onRootFocus, false);
        this._props.rootElement?.removeEventListener("blur", this._onRootBlur, false);
        this._props.firstBumper?.removeEventListener("focus", this._onFirstBumperFocus, false);
        this._props.lastBumper?.removeEventListener("focus", this._onLastBumperFocus, false);
        if (!this._props.disabled ||
            this._props.forceFocusInsideTrapOnOutsideFocus ||
            // @ts-ignore
            !FluentUIBaseComponent.elementContains(this._props.rootElement, activeElement)) {
            this._releaseFocusTrapZone();
        }
        // Dispose of element references so the DOM Nodes can be garbage-collected
        delete this._previouslyFocusedElementInTrapZone;
        delete this._previouslyFocusedElementOutsideTrapZone;
    }
    updateProps(prevProps) {
        const { forceFocusInsideTrapOnComponentUpdate, forceFocusInsideTrapOnOutsideFocus, disabled } = this._props;
        // @ts-ignore
        const activeElement = document.activeElement;
        // if after componentDidUpdate focus is not inside the focus trap, bring it back
        if (!disabled &&
            // @ts-ignore
            !FluentUIBaseComponent.elementContains(this._props.rootElement, activeElement) &&
            forceFocusInsideTrapOnComponentUpdate) {
            this._bringFocusIntoZone();
            return;
        }
        const prevForceFocusInsideTrap = prevProps.forceFocusInsideTrapOnOutsideFocus !== undefined ? prevProps.forceFocusInsideTrapOnOutsideFocus : true;
        const newForceFocusInsideTrap = forceFocusInsideTrapOnOutsideFocus !== undefined ? forceFocusInsideTrapOnOutsideFocus : true;
        const prevDisabled = prevProps.disabled !== undefined ? prevProps.disabled : false;
        const newDisabled = disabled !== undefined ? disabled : false;
        if ((!prevForceFocusInsideTrap && newForceFocusInsideTrap) || (prevDisabled && !newDisabled)) {
            // Transition from forceFocusInsideTrap / FTZ disabled to enabled.
            // Emulate what happens when a FocusTrapZone gets mounted.
            this._enableFocusTrapZone();
        }
        else if ((prevForceFocusInsideTrap && !newForceFocusInsideTrap) || (!prevDisabled && newDisabled)) {
            // Transition from forceFocusInsideTrap / FTZ enabled to disabled.
            // Emulate what happens when a FocusTrapZone gets unmounted.
            this._releaseFocusTrapZone();
        }
    }
    setDisabled(disabled) {
        this._props.disabled = disabled;
    }
    _isBumper(element) {
        return element === this._props.firstBumper || element === this._props.lastBumper;
    }
    _focusAsync(element) {
        if (!this._isBumper(element)) {
            FluentUIBaseComponent.focusAsync(element);
        }
    }
    _bringFocusIntoZone() {
        const { disableFirstFocus = false } = this._props;
        this._previouslyFocusedElementOutsideTrapZone = this._getPreviouslyFocusedElementOutsideTrapZone();
        if (
        // @ts-ignore
        FluentUIBaseComponent.elementContains(this._props.rootElement, this._previouslyFocusedElementOutsideTrapZone) &&
            !disableFirstFocus) {
            this._findElementAndFocusAsync();
        }
    }
    _returnFocusToInitiator() {
        const { ignoreExternalFocusing, rootElement } = this._props;
        FocusTrapZoneInternal._focusStack = FocusTrapZoneInternal._focusStack.filter((value) => {
            return this !== value;
        });
        const activeElement = document.activeElement;
        if (!ignoreExternalFocusing &&
            this._previouslyFocusedElementOutsideTrapZone &&
            typeof this._previouslyFocusedElementOutsideTrapZone.focus === 'function' &&
            (FluentUIBaseComponent.elementContains(rootElement, activeElement) || activeElement === document.body)) {
            this._focusAsync(this._previouslyFocusedElementOutsideTrapZone);
        }
    }
}
FocusTrapZoneInternal._focusStack = [];
var count = 0;
var focusTrapZones = {};
export function register(props, focusTrapZone) {
    let currentId = count++;
    focusTrapZones[currentId] = new FocusTrapZoneInternal(props, focusTrapZone);
    return currentId;
}
export function unregister(id) {
    let focusTrapZone = focusTrapZones[id];
    if (focusTrapZone) {
        focusTrapZone.unRegister();
    }
    delete focusTrapZones[id];
}
export function updateProps(id, props) {
    let focusTrapZone = focusTrapZones[id];
    if (focusTrapZone) {
        focusTrapZone.updateProps(props);
    }
}
let targetToFocusOnNextRepaint = undefined;
