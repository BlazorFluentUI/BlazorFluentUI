import * as FluentUIBaseComponent from './baseComponent.js';
var FocusZoneDirection;
(function (FocusZoneDirection) {
    /** Only react to up/down arrows. */
    FocusZoneDirection[FocusZoneDirection["vertical"] = 0] = "vertical";
    /** Only react to left/right arrows. */
    FocusZoneDirection[FocusZoneDirection["horizontal"] = 1] = "horizontal";
    /** React to all arrows. */
    FocusZoneDirection[FocusZoneDirection["bidirectional"] = 2] = "bidirectional";
    /**
     * React to all arrows. Navigate next item in DOM on right/down arrow keys and previous - left/up arrow keys.
     * Right and Left arrow keys are swapped in RTL mode.
     */
    FocusZoneDirection[FocusZoneDirection["domOrder"] = 3] = "domOrder";
})(FocusZoneDirection || (FocusZoneDirection = {}));
var FocusZoneTabbableElements;
(function (FocusZoneTabbableElements) {
    /** Tabbing is not allowed */
    FocusZoneTabbableElements[FocusZoneTabbableElements["none"] = 0] = "none";
    /** All tabbing action is allowed */
    FocusZoneTabbableElements[FocusZoneTabbableElements["all"] = 1] = "all";
    /** Tabbing is allowed only on input elements */
    FocusZoneTabbableElements[FocusZoneTabbableElements["inputOnly"] = 2] = "inputOnly";
})(FocusZoneTabbableElements || (FocusZoneTabbableElements = {}));
const IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
const IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
const FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
const FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
const IS_ENTER_DISABLED_ATTRIBUTE = 'data-disable-click-on-enter';
const TABINDEX = 'tabindex';
const NO_VERTICAL_WRAP = 'data-no-vertical-wrap';
const NO_HORIZONTAL_WRAP = 'data-no-horizontal-wrap';
const LARGE_DISTANCE_FROM_CENTER = 999999999;
const LARGE_NEGATIVE_DISTANCE_FROM_CENTER = -999999999;
const ALLOWED_INPUT_TYPES = ['text', 'number', 'password', 'email', 'tel', 'url', 'search'];
const ALLOW_VIRTUAL_ELEMENTS = false; // this is not used in Blazor... concept for React only
const focusZones = new Map();
//var count = 0;
//var allInstances: Map<FocusZone> = {};
var outerZones = new Set();
let _disposeGlobalKeyDownListener;
export function registerFocusZone(dotNet, root, props) {
    let focusZone = new FocusZone(dotNet, root, props);
    focusZones.set(dotNet._id, focusZone);
}
export function updateFocusZoneProps(dotNet, props) {
    let focusZone = focusZones.get(dotNet._id);
    if (typeof focusZone !== 'undefined' && focusZone !== null) {
        focusZone.updateFocusZone(props);
    }
}
export function unregisterFocusZone(dotNet) {
    let focusZone = focusZones.get(dotNet._id);
    if (typeof focusZone !== 'undefined' && focusZone !== null) {
        focusZone.unRegister();
        //focusZone.dispose();
        //focusZones.delete(dotNet._id);
    }
}
class FocusZone {
    constructor(dotNet, root, props) {
        this._disposables = [];
        this._onBlur = () => {
            this._setParkedFocus(false);
        };
        this._onKeyDown = (ev) => {
            if (this._portalContainsElement(ev.target)) {
                // If the event target is inside a portal do not process the event.
                return;
            }
            const { direction, disabled, innerZoneKeystrokeTriggers } = this.props;
            if (disabled) {
                return;
            }
            //if (this.props.onKeyDown) {
            //    this.props.onKeyDown(ev);
            //}
            // If the default has been prevented, do not process keyboard events.
            if (ev.defaultPrevented) {
                return;
            }
            if (document.activeElement === this.root && this._isInnerZone) {
                // If this element has focus, it is being controlled by a parent.
                // Ignore the keystroke.
                return;
            }
            if (innerZoneKeystrokeTriggers && (innerZoneKeystrokeTriggers.indexOf(ev.keyCode) != -1) && this._isImmediateDescendantOfZone(ev.target)) {
                // Try to focus
                const innerZone = this._getFirstInnerZone();
                if (innerZone) {
                    if (!innerZone.focus(true)) {
                        return;
                    }
                }
                else if (FluentUIBaseComponent.isElementFocusSubZone(ev.target)) {
                    if (!this.focusElement(FluentUIBaseComponent.getNextElement(ev.target, ev.target.firstChild, true))) {
                        return;
                    }
                }
                else {
                    return;
                }
            }
            else if (ev.altKey) {
                return;
            }
            else {
                switch (ev.which) {
                    case 32 /* space */:
                        if (this._tryInvokeClickForFocusable(ev.target)) {
                            break;
                        }
                        return;
                    case 37 /* left */:
                        if (direction !== FocusZoneDirection.vertical && this._moveFocusLeft()) {
                            break;
                        }
                        return;
                    case 39 /* right */:
                        if (direction !== FocusZoneDirection.vertical && this._moveFocusRight()) {
                            break;
                        }
                        return;
                    case 38 /* up */:
                        if (direction !== FocusZoneDirection.horizontal && this._moveFocusUp()) {
                            break;
                        }
                        return;
                    case 40 /* down */:
                        if (direction !== FocusZoneDirection.horizontal && this._moveFocusDown()) {
                            break;
                        }
                        return;
                    case 9 /* tab */:
                        if (this.props?.handleTabKey === FocusZoneTabbableElements.all ||
                            (this.props?.handleTabKey === FocusZoneTabbableElements.inputOnly && this._isElementInput(ev.target))) {
                            let focusChanged = false;
                            this._processingTabKey = true;
                            if (direction === FocusZoneDirection.vertical ||
                                !this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP)) {
                                focusChanged = ev.shiftKey ? this._moveFocusUp() : this._moveFocusDown();
                            }
                            else if (direction === FocusZoneDirection.horizontal || direction === FocusZoneDirection.bidirectional) {
                                const tabWithDirection = FluentUIBaseComponent.getRTL() ? !ev.shiftKey : ev.shiftKey;
                                focusChanged = tabWithDirection ? this._moveFocusLeft() : this._moveFocusRight();
                            }
                            this._processingTabKey = false;
                            if (focusChanged) {
                                break;
                            }
                        }
                        return;
                    case 36 /* home */:
                        if (this._isElementInput(ev.target) && !this._shouldInputLoseFocus(ev.target, false)) {
                            return;
                        }
                        const firstChild = this.root && this.root.firstChild;
                        if (this.root && firstChild && this.focusElement(FluentUIBaseComponent.getNextElement(this.root, firstChild, true))) {
                            break;
                        }
                        return;
                    case 35 /* end */:
                        if (this._isElementInput(ev.target) && !this._shouldInputLoseFocus(ev.target, true)) {
                            return;
                        }
                        const lastChild = this.root && this.root.lastChild;
                        if (this.root && this.focusElement(FluentUIBaseComponent.getPreviousElement(this.root, lastChild, true, true, true))) {
                            break;
                        }
                        return;
                    case 13 /* enter */:
                        if (this._tryInvokeClickForFocusable(ev.target)) {
                            break;
                        }
                        return;
                    default:
                        return;
                }
            }
            ev.preventDefault();
            ev.stopPropagation();
        };
        this._onFocus = (ev) => {
            if (this._portalContainsElement(ev.target)) {
                // If the event target is inside a portal do not process the event.
                return;
            }
            const { doNotAllowFocusEventToPropagate } = this.props;
            const isImmediateDescendant = this._isImmediateDescendantOfZone(ev.target);
            let newActiveElement;
            this.dotNet.invokeMethodAsync("JSOnFocusNotification");
            if (isImmediateDescendant) {
                newActiveElement = ev.target;
            }
            else {
                let parentElement = ev.target;
                while (parentElement && parentElement !== this.root) {
                    if (FluentUIBaseComponent.isElementTabbable(parentElement) && this._isImmediateDescendantOfZone(parentElement)) {
                        newActiveElement = parentElement;
                        break;
                    }
                    parentElement = FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                }
            }
            const initialElementFocused = !this._activeElement;
            // If the new active element is a child of this zone and received focus,
            // update alignment an immediate descendant
            if (newActiveElement && newActiveElement !== this._activeElement) {
                if (isImmediateDescendant || initialElementFocused) {
                    this._setFocusAlignment(newActiveElement, true, true);
                }
                this._activeElement = newActiveElement;
                if (initialElementFocused) {
                    this._updateTabIndexes();
                }
            }
            this.dotNet.invokeMethodAsync("JSOnActiveElementChanged");
            if (doNotAllowFocusEventToPropagate) {
                ev.stopPropagation();
            }
        };
        this._onKeyDownCapture = (ev) => {
            if (ev.which === 9 /* tab */) {
                outerZones.forEach(zone => zone._updateTabIndexes());
            }
        };
        this._onMouseDown = (ev) => {
            if (this._portalContainsElement(ev.target)) {
                // If the event target is inside a portal do not process the event.
                return;
            }
            const { disabled } = this.props;
            if (disabled) {
                return;
            }
            let target = ev.target;
            const path = [];
            while (target && target !== this.root) {
                path.push(target);
                target = FluentUIBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
            }
            while (path.length) {
                target = path.pop();
                if (target && FluentUIBaseComponent.isElementTabbable(target)) {
                    this._setActiveElement(target, true);
                }
                if (FluentUIBaseComponent.isElementFocusZone(target)) {
                    // Stop here since the focus zone will take care of its own children.
                    break;
                }
            }
        };
        this.dotNet = dotNet;
        this.root = root;
        this.props = props;
        this._focusAlignment = {
            x: 0,
            y: 0
        };
        this.root?.addEventListener("keydown", this._onKeyDown, false);
        this.root?.addEventListener("focusin", this._onFocus, false);
        this.root?.addEventListener("mousedown", this._onMouseDown, false);
        this.initialized();
    }
    updateFocusZone(props) {
        this.props = props;
        //allInstances[props.id] = this;
        if (this.root) {
            const windowElement = FluentUIBaseComponent.getWindow(this.root);
            let parentElement = FluentUIBaseComponent.getParent(this.root, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                if (FluentUIBaseComponent.isElementFocusZone(parentElement)) {
                    this._isInnerZone = true;
                    break;
                }
                parentElement = FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            if (!this._isInnerZone) {
                outerZones.add(this);
            }
            if (windowElement && outerZones.size === 1) {
                _disposeGlobalKeyDownListener = FluentUIBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
            }
            this._disposables.push(FluentUIBaseComponent.on(this.root, 'blur', this._onBlur, true));
            // Assign initial tab indexes so that we can set initial focus as appropriate.
            this._updateTabIndexes();
            // using a hack to detect whether the passed in HTMLElement is valid (came from a legitimate .NET ElementReference)
            if ((this.props?.defaultActiveElement)?.__internalId !== null) {
                if (this._activeElement != this.props?.defaultActiveElement) {
                    this._activeElement = this.props?.defaultActiveElement;
                    this.focus();
                }
            }
        }
    }
    initialized() {
        const windowElement = FluentUIBaseComponent.getWindow(this.root);
        let parentElement = FluentUIBaseComponent.getParent(this.root, ALLOW_VIRTUAL_ELEMENTS);
        while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
            if (FluentUIBaseComponent.isElementFocusZone(parentElement)) {
                this._isInnerZone = true;
                break;
            }
            parentElement = FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
        }
        if (!this._isInnerZone) {
            outerZones.add(this);
        }
        if (windowElement && outerZones.size === 1) {
            _disposeGlobalKeyDownListener = FluentUIBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
        }
        this._disposables.push(FluentUIBaseComponent.on(this.root, 'blur', this._onBlur, true));
        // Assign initial tab indexes so that we can set initial focus as appropriate.
        this._updateTabIndexes();
        // using a hack to detect whether the passed in HTMLElement is valid (came from a legitimate .NET ElementReference)
        if ((this.props?.defaultActiveElement)?.__internalId !== null) {
            this._activeElement = this.props?.defaultActiveElement;
            this.focus();
        }
    }
    /**
   * When focus is in the zone at render time but then all focusable elements are removed,
   * we "park" focus temporarily on the root. Once we update with focusable children, we restore
   * focus to the closest path from previous. If the user tabs away from the parked container,
   * we restore focusability to the pre-parked state.
   */
    _setParkedFocus(isParked) {
        if (this.root && this._isParked !== isParked) {
            this._isParked = isParked;
            if (isParked) {
                if (!this.props?.allowFocusRoot) {
                    this._parkedTabIndex = this.root.getAttribute('tabindex');
                    this.root.setAttribute('tabindex', '-1');
                }
                this.root.focus();
            }
            else {
                if (!this.props?.allowFocusRoot) {
                    if (this._parkedTabIndex) {
                        this.root.setAttribute('tabindex', this._parkedTabIndex);
                        this._parkedTabIndex = undefined;
                    }
                    else {
                        this.root.removeAttribute('tabindex');
                    }
                }
            }
        }
    }
    unRegister() {
        if (!this._isInnerZone) {
            outerZones.delete(this);
        }
        this._disposables.forEach(d => d());
        if (outerZones.size === 0 && _disposeGlobalKeyDownListener) {
            _disposeGlobalKeyDownListener();
        }
        this.root.removeEventListener("keydown", this._onKeyDown, false);
        this.root.removeEventListener("focus", this._onFocus, false);
        this.root.removeEventListener("mousedown", this._onMouseDown, false);
    }
    focus(forceIntoFirstElement = false) {
        if (this.root) {
            if (!forceIntoFirstElement && this.root.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' && this._isInnerZone) {
                const ownerZoneElement = this._getOwnerZone(this.root);
                if (ownerZoneElement !== this.root) {
                    const ownerZone = focusZones[ownerZoneElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
                    return !!ownerZone && ownerZone.focusElement(this.root);
                }
                return false;
            }
            else if (!forceIntoFirstElement &&
                this._activeElement &&
                FluentUIBaseComponent.elementContains(this.root, this._activeElement) &&
                FluentUIBaseComponent.isElementTabbable(this._activeElement)) {
                this._activeElement.focus();
                return true;
            }
            else {
                const firstChild = this.root.firstChild;
                return this.focusElement(FluentUIBaseComponent.getNextElement(this.root, firstChild, true));
            }
        }
        return false;
    }
    focusElement(element) {
        const { onBeforeFocusExists } = this.props;
        if (onBeforeFocusExists && !this.dotNet.invokeMethodAsync("JSOnBeforeFocus")) {
            return false;
        }
        if (element) {
            // when we Set focus to a specific child, we should recalculate the alignment depend on its position
            this._setActiveElement(element);
            if (this._activeElement) {
                this._activeElement.focus();
            }
            return true;
        }
        return false;
    }
    _updateTabIndexes(element) {
        if (!element && this.root) {
            this._defaultFocusElement = null;
            element = this.root;
            if (this._activeElement && !FluentUIBaseComponent.elementContains(element, this._activeElement)) {
                this._activeElement = null;
            }
        }
        // If active element changes state to disabled, set it to null.
        // Otherwise, we lose keyboard accessibility to other elements in focus zone.
        if (this._activeElement && !FluentUIBaseComponent.isElementTabbable(this._activeElement)) {
            this._activeElement = null;
        }
        const childNodes = element && element.children;
        for (let childIndex = 0; childNodes && childIndex < childNodes.length; childIndex++) {
            const child = childNodes[childIndex];
            if (!FluentUIBaseComponent.isElementFocusZone(child)) {
                // If the item is explicitly set to not be focusable then TABINDEX needs to be set to -1.
                if (child.getAttribute && child.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'false') {
                    child.setAttribute(TABINDEX, '-1');
                }
                if (FluentUIBaseComponent.isElementTabbable(child)) {
                    if (this.props?.disabled) {
                        child.setAttribute(TABINDEX, '-1');
                    }
                    else if (!this._isInnerZone && ((!this._activeElement && !this._defaultFocusElement) || this._activeElement === child)) {
                        this._defaultFocusElement = child;
                        if (child.getAttribute(TABINDEX) !== '0') {
                            child.setAttribute(TABINDEX, '0');
                        }
                    }
                    else if (child.getAttribute(TABINDEX) !== '-1') {
                        child.setAttribute(TABINDEX, '-1');
                    }
                }
                else if (child.tagName === 'svg' && child.getAttribute('focusable') !== 'false') {
                    // Disgusting IE hack. Sad face.
                    child.setAttribute('focusable', 'false');
                }
            }
            else if (child.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true') {
                if (!this._isInnerZone && ((!this._activeElement && !this._defaultFocusElement) || this._activeElement === child)) {
                    this._defaultFocusElement = child;
                    if (child.getAttribute(TABINDEX) !== '0') {
                        child.setAttribute(TABINDEX, '0');
                    }
                }
                else if (child.getAttribute(TABINDEX) !== '-1') {
                    child.setAttribute(TABINDEX, '-1');
                }
            }
            this._updateTabIndexes(child);
        }
    }
    _getOwnerZone(element) {
        let parentElement = FluentUIBaseComponent.getParent(element, ALLOW_VIRTUAL_ELEMENTS);
        while (parentElement && parentElement !== this.root && parentElement !== document.body) {
            if (FluentUIBaseComponent.isElementFocusZone(parentElement)) {
                return parentElement;
            }
            parentElement = FluentUIBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
        }
        return parentElement;
    }
    _setActiveElement(element, forceAlignment) {
        const previousActiveElement = this._activeElement;
        this._activeElement = element;
        if (previousActiveElement) {
            if (FluentUIBaseComponent.isElementFocusZone(previousActiveElement)) {
                this._updateTabIndexes(previousActiveElement);
            }
            previousActiveElement.tabIndex = -1;
        }
        if (this._activeElement) {
            if (!this._focusAlignment || forceAlignment) {
                this._setFocusAlignment(element, true, true);
            }
            this._activeElement.tabIndex = 0;
        }
    }
    _setFocusAlignment(element, isHorizontal, isVertical) {
        if (this.props?.direction === FocusZoneDirection.bidirectional && (!this._focusAlignment || isHorizontal || isVertical)) {
            const rect = element.getBoundingClientRect();
            const left = rect.left + rect.width / 2;
            const top = rect.top + rect.height / 2;
            if (!this._focusAlignment) {
                this._focusAlignment = {
                    x: left,
                    y: top
                };
            }
            if (isHorizontal) {
                this._focusAlignment.x = left;
            }
            if (isVertical) {
                this._focusAlignment.y = top;
            }
        }
    }
    _isImmediateDescendantOfZone(element) {
        return this._getOwnerZone(element) === this.root;
    }
    /**
* Traverse to find first child zone.
*/
    _getFirstInnerZone(rootElement) {
        rootElement = rootElement || this._activeElement || this.root;
        if (!rootElement) {
            return null;
        }
        if (FluentUIBaseComponent.isElementFocusZone(rootElement)) {
            return focusZones[rootElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
        }
        let child = rootElement.firstElementChild;
        while (child) {
            if (FluentUIBaseComponent.isElementFocusZone(child)) {
                return focusZones[child.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
            }
            const match = this._getFirstInnerZone(child);
            if (match) {
                return match;
            }
            child = child.nextElementSibling;
        }
        return null;
    }
    /**
   * Walk up the dom try to find a focusable element.
   */
    _tryInvokeClickForFocusable(target) {
        if (target === this.root) {
            return false;
        }
        do {
            if (target.tagName === 'BUTTON' || target.tagName === 'A' || target.tagName === 'INPUT' || target.tagName === 'TEXTAREA') {
                return false;
            }
            if (this._isImmediateDescendantOfZone(target) &&
                target.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' &&
                target.getAttribute(IS_ENTER_DISABLED_ATTRIBUTE) !== 'true') {
                FluentUIBaseComponent.raiseClick(target);
                return true;
            }
            target = FluentUIBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
        } while (target !== this.root);
        return false;
    }
    /**
    * Returns true if the element is a descendant of the FocusZone through a React portal.
    */
    _portalContainsElement(element) {
        // This might break our control when used inside a Layer...
        return false;
        //return element && !!this.root && FluentUIBaseComponent portalContainsElement(element, this.root.current);
    }
    _isElementInput(element) {
        if (element && element.tagName && (element.tagName.toLowerCase() === 'input' || element.tagName.toLowerCase() === 'textarea')) {
            return true;
        }
        return false;
    }
    _shouldInputLoseFocus(element, isForward) {
        // If a tab was used, we want to focus on the next element.
        if (!this._processingTabKey && element && element.type && ALLOWED_INPUT_TYPES.indexOf(element.type.toLowerCase()) > -1) {
            const selectionStart = element.selectionStart;
            const selectionEnd = element.selectionEnd;
            const isRangeSelected = selectionStart !== selectionEnd;
            const inputValue = element.value;
            // We shouldn't lose focus in the following cases:
            // 1. There is range selected.
            // 2. When selection start is larger than 0 and it is backward.
            // 3. when selection start is not the end of length and it is forward.
            // 4. We press any of the arrow keys when our handleTabKey isn't none or undefined (only losing focus if we hit tab)
            // and if shouldInputLoseFocusOnArrowKey is defined, if scenario prefers to not loose the focus which is determined by calling the
            // callback shouldInputLoseFocusOnArrowKey
            if (isRangeSelected ||
                (selectionStart > 0 && !isForward) ||
                (selectionStart !== inputValue.length && isForward) ||
                (!!this.props?.handleTabKey && !(this.props?.shouldInputLoseFocusOnArrowKeyExists && this.dotNet.invokeMethodAsync("JSShouldInputLoseFocusOnArrowKey")))) {
                return false;
            }
        }
        return true;
    }
    _shouldWrapFocus(element, noWrapDataAttribute) {
        return !!this.props?.checkForNoWrap ? FluentUIBaseComponent.shouldWrapFocus(element, noWrapDataAttribute) : true;
    }
    _moveFocus(isForward, getDistanceFromCenter, ev, useDefaultWrap = true) {
        let element = this._activeElement;
        let candidateDistance = -1;
        let candidateElement = undefined;
        let changedFocus = false;
        const isBidirectional = this.props?.direction === FocusZoneDirection.bidirectional;
        if (!element || !this.root) {
            return false;
        }
        if (this._isElementInput(element)) {
            if (!this._shouldInputLoseFocus(element, isForward)) {
                return false;
            }
        }
        const activeRect = isBidirectional ? element.getBoundingClientRect() : null;
        do {
            element = (isForward ? FluentUIBaseComponent.getNextElement(this.root, element) : FluentUIBaseComponent.getPreviousElement(this.root, element));
            if (isBidirectional) {
                if (element) {
                    const targetRect = element.getBoundingClientRect();
                    const elementDistance = getDistanceFromCenter(activeRect, targetRect);
                    if (elementDistance === -1 && candidateDistance === -1) {
                        candidateElement = element;
                        break;
                    }
                    if (elementDistance > -1 && (candidateDistance === -1 || elementDistance < candidateDistance)) {
                        candidateDistance = elementDistance;
                        candidateElement = element;
                    }
                    if (candidateDistance >= 0 && elementDistance < 0) {
                        break;
                    }
                }
            }
            else {
                candidateElement = element;
                break;
            }
        } while (element);
        // Focus the closest candidate
        if (candidateElement && candidateElement !== this._activeElement) {
            changedFocus = true;
            this.focusElement(candidateElement);
        }
        else if (this.props?.isCircularNavigation && useDefaultWrap) {
            if (isForward) {
                return this.focusElement(FluentUIBaseComponent.getNextElement(this.root, this.root.firstElementChild, true));
            }
            else {
                return this.focusElement(FluentUIBaseComponent.getPreviousElement(this.root, this.root.lastElementChild, true, true, true));
            }
        }
        return changedFocus;
    }
    _moveFocusDown() {
        let targetTop = -1;
        const leftAlignment = this._focusAlignment.x;
        if (this._moveFocus(true, (activeRect, targetRect) => {
            let distance = -1;
            // ClientRect values can be floats that differ by very small fractions of a decimal.
            // If the difference between top and bottom are within a pixel then we should treat
            // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
            // but without Math.Floor they will be handled incorrectly.
            const targetRectTop = Math.floor(targetRect.top);
            const activeRectBottom = Math.floor(activeRect.bottom);
            if (targetRectTop < activeRectBottom) {
                if (!this._shouldWrapFocus(this._activeElement, NO_VERTICAL_WRAP)) {
                    return LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                }
                return LARGE_DISTANCE_FROM_CENTER;
            }
            if ((targetTop === -1 && targetRectTop >= activeRectBottom) || targetRectTop === targetTop) {
                targetTop = targetRectTop;
                if (leftAlignment >= targetRect.left && leftAlignment <= targetRect.left + targetRect.width) {
                    distance = 0;
                }
                else {
                    distance = Math.abs(targetRect.left + targetRect.width / 2 - leftAlignment);
                }
            }
            return distance;
        })) {
            this._setFocusAlignment(this._activeElement, false, true);
            return true;
        }
        return false;
    }
    _moveFocusUp() {
        let targetTop = -1;
        const leftAlignment = this._focusAlignment.x;
        if (this._moveFocus(false, (activeRect, targetRect) => {
            let distance = -1;
            // ClientRect values can be floats that differ by very small fractions of a decimal.
            // If the difference between top and bottom are within a pixel then we should treat
            // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
            // but without Math.Floor they will be handled incorrectly.
            const targetRectBottom = Math.floor(targetRect.bottom);
            const targetRectTop = Math.floor(targetRect.top);
            const activeRectTop = Math.floor(activeRect.top);
            if (targetRectBottom > activeRectTop) {
                if (!this._shouldWrapFocus(this._activeElement, NO_VERTICAL_WRAP)) {
                    return LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                }
                return LARGE_DISTANCE_FROM_CENTER;
            }
            if ((targetTop === -1 && targetRectBottom <= activeRectTop) || targetRectTop === targetTop) {
                targetTop = targetRectTop;
                if (leftAlignment >= targetRect.left && leftAlignment <= targetRect.left + targetRect.width) {
                    distance = 0;
                }
                else {
                    distance = Math.abs(targetRect.left + targetRect.width / 2 - leftAlignment);
                }
            }
            return distance;
        })) {
            this._setFocusAlignment(this._activeElement, false, true);
            return true;
        }
        return false;
    }
    _moveFocusLeft() {
        const shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
        if (this._moveFocus(FluentUIBaseComponent.getRTL(), (activeRect, targetRect) => {
            let distance = -1;
            let topBottomComparison;
            if (FluentUIBaseComponent.getRTL()) {
                // When in RTL, this comparison should be the same as the one in _moveFocusRight for LTR.
                // Going left at a leftmost rectangle will go down a line instead of up a line like in LTR.
                // This is important, because we want to be comparing the top of the target rect
                // with the bottom of the active rect.
                topBottomComparison = parseFloat(targetRect.top.toFixed(3)) < parseFloat(activeRect.bottom.toFixed(3));
            }
            else {
                topBottomComparison = parseFloat(targetRect.bottom.toFixed(3)) > parseFloat(activeRect.top.toFixed(3));
            }
            if (topBottomComparison && targetRect.right <= activeRect.right && this.props?.direction !== FocusZoneDirection.vertical) {
                distance = activeRect.right - targetRect.right;
            }
            else {
                if (!shouldWrap) {
                    distance = LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
                }
            }
            return distance;
        }, undefined /*ev*/, shouldWrap)) {
            this._setFocusAlignment(this._activeElement, true, false);
            return true;
        }
        return false;
    }
    _moveFocusRight() {
        const shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
        if (this._moveFocus(!FluentUIBaseComponent.getRTL(), (activeRect, targetRect) => {
            let distance = -1;
            let topBottomComparison;
            if (FluentUIBaseComponent.getRTL()) {
                // When in RTL, this comparison should be the same as the one in _moveFocusLeft for LTR.
                // Going right at a rightmost rectangle will go up a line instead of down a line like in LTR.
                // This is important, because we want to be comparing the bottom of the target rect
                // with the top of the active rect.
                topBottomComparison = parseFloat(targetRect.bottom.toFixed(3)) > parseFloat(activeRect.top.toFixed(3));
            }
            else {
                topBottomComparison = parseFloat(targetRect.top.toFixed(3)) < parseFloat(activeRect.bottom.toFixed(3));
            }
            if (topBottomComparison && targetRect.left >= activeRect.left && this.props?.direction !== FocusZoneDirection.vertical) {
                distance = targetRect.left - activeRect.left;
            }
            else if (!shouldWrap) {
                distance = LARGE_NEGATIVE_DISTANCE_FROM_CENTER;
            }
            return distance;
        }, undefined /*ev*/, shouldWrap)) {
            this._setFocusAlignment(this._activeElement, true, false);
            return true;
        }
        return false;
    }
}
