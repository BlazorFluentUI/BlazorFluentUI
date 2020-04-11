/// <reference path="../../BlazorFluentUI.BFUBaseComponent/wwwroot/baseComponent.ts" />
var BlazorFluentUiFocusZone;
(function (BlazorFluentUiFocusZone) {
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
    var IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    var IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    var FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    var FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';
    var IS_ENTER_DISABLED_ATTRIBUTE = 'data-disable-click-on-enter';
    var TABINDEX = 'tabindex';
    var NO_VERTICAL_WRAP = 'data-no-vertical-wrap';
    var NO_HORIZONTAL_WRAP = 'data-no-horizontal-wrap';
    var LARGE_DISTANCE_FROM_CENTER = 999999999;
    var LARGE_NEGATIVE_DISTANCE_FROM_CENTER = -999999999;
    var ALLOWED_INPUT_TYPES = ['text', 'number', 'password', 'email', 'tel', 'url', 'search'];
    var ALLOW_VIRTUAL_ELEMENTS = false; // this is not used in Blazor... concept for React only
    var count = 0;
    var allInstances = {};
    var outerZones = new Set();
    var _disposeGlobalKeyDownListener;
    function register(props, focusZone) {
        var currentId = count++;
        allInstances[currentId] = new FocusZoneInternal(props, focusZone);
        return currentId;
    }
    BlazorFluentUiFocusZone.register = register;
    function unregister(id) {
        var focusZone = allInstances[id];
        if (focusZone) {
            focusZone.unRegister();
        }
        delete allInstances[id];
    }
    BlazorFluentUiFocusZone.unregister = unregister;
    function updateFocusZone(id, props) {
        var focusZone = allInstances[id];
        if (focusZone) {
            focusZone.updateFocusZone(props);
        }
    }
    BlazorFluentUiFocusZone.updateFocusZone = updateFocusZone;
    var FocusZoneInternal = /** @class */ (function () {
        function FocusZoneInternal(focusZoneProps, dotNetRef) {
            var _this = this;
            this._disposables = [];
            this._onBlur = function () {
                _this._setParkedFocus(false);
            };
            this._onKeyDown = function (ev) {
                if (_this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                var _a = _this._focusZoneProps, direction = _a.direction, disabled = _a.disabled, innerZoneKeystrokeTriggers = _a.innerZoneKeystrokeTriggers;
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
                if (document.activeElement === _this._root && _this._isInnerZone) {
                    // If this element has focus, it is being controlled by a parent.
                    // Ignore the keystroke.
                    return;
                }
                if (innerZoneKeystrokeTriggers && (innerZoneKeystrokeTriggers.indexOf(ev.keyCode) != -1) && _this._isImmediateDescendantOfZone(ev.target)) {
                    // Try to focus
                    var innerZone = _this._getFirstInnerZone();
                    if (innerZone) {
                        if (!innerZone.focus(true)) {
                            return;
                        }
                    }
                    else if (window.BlazorFluentUiBaseComponent.isElementFocusSubZone(ev.target)) {
                        if (!_this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(ev.target, ev.target.firstChild, true))) {
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
                            if (_this._tryInvokeClickForFocusable(ev.target)) {
                                break;
                            }
                            return;
                        case 37 /* left */:
                            if (direction !== FocusZoneDirection.vertical && _this._moveFocusLeft()) {
                                break;
                            }
                            return;
                        case 39 /* right */:
                            if (direction !== FocusZoneDirection.vertical && _this._moveFocusRight()) {
                                break;
                            }
                            return;
                        case 38 /* up */:
                            if (direction !== FocusZoneDirection.horizontal && _this._moveFocusUp()) {
                                break;
                            }
                            return;
                        case 40 /* down */:
                            if (direction !== FocusZoneDirection.horizontal && _this._moveFocusDown()) {
                                break;
                            }
                            return;
                        case 9 /* tab */:
                            if (_this._focusZoneProps.handleTabKey === FocusZoneTabbableElements.all ||
                                (_this._focusZoneProps.handleTabKey === FocusZoneTabbableElements.inputOnly && _this._isElementInput(ev.target))) {
                                var focusChanged = false;
                                _this._processingTabKey = true;
                                if (direction === FocusZoneDirection.vertical ||
                                    !_this._shouldWrapFocus(_this._activeElement, NO_HORIZONTAL_WRAP)) {
                                    focusChanged = ev.shiftKey ? _this._moveFocusUp() : _this._moveFocusDown();
                                }
                                else if (direction === FocusZoneDirection.horizontal || direction === FocusZoneDirection.bidirectional) {
                                    var tabWithDirection = window.BlazorFluentUiBaseComponent.getRTL() ? !ev.shiftKey : ev.shiftKey;
                                    focusChanged = tabWithDirection ? _this._moveFocusLeft() : _this._moveFocusRight();
                                }
                                _this._processingTabKey = false;
                                if (focusChanged) {
                                    break;
                                }
                            }
                            return;
                        case 36 /* home */:
                            if (_this._isElementInput(ev.target) && !_this._shouldInputLoseFocus(ev.target, false)) {
                                return;
                            }
                            var firstChild = _this._root && _this._root.firstChild;
                            if (_this._root && firstChild && _this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(_this._root, firstChild, true))) {
                                break;
                            }
                            return;
                        case 35 /* end */:
                            if (_this._isElementInput(ev.target) && !_this._shouldInputLoseFocus(ev.target, true)) {
                                return;
                            }
                            var lastChild = _this._root && _this._root.lastChild;
                            if (_this._root && _this.focusElement(window.BlazorFluentUiBaseComponent.getPreviousElement(_this._root, lastChild, true, true, true))) {
                                break;
                            }
                            return;
                        case 13 /* enter */:
                            if (_this._tryInvokeClickForFocusable(ev.target)) {
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
            this._onFocus = function (ev) {
                if (_this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                var doNotAllowFocusEventToPropagate = _this._focusZoneProps.doNotAllowFocusEventToPropagate;
                var isImmediateDescendant = _this._isImmediateDescendantOfZone(ev.target);
                var newActiveElement;
                _this._dotNetRef.invokeMethodAsync("JSOnFocusNotification");
                if (isImmediateDescendant) {
                    newActiveElement = ev.target;
                }
                else {
                    var parentElement = ev.target;
                    while (parentElement && parentElement !== _this._root) {
                        if (window.BlazorFluentUiBaseComponent.isElementTabbable(parentElement) && _this._isImmediateDescendantOfZone(parentElement)) {
                            newActiveElement = parentElement;
                            break;
                        }
                        parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                    }
                }
                var initialElementFocused = !_this._activeElement;
                // If the new active element is a child of this zone and received focus,
                // update alignment an immediate descendant
                if (newActiveElement && newActiveElement !== _this._activeElement) {
                    if (isImmediateDescendant || initialElementFocused) {
                        _this._setFocusAlignment(newActiveElement, true, true);
                    }
                    _this._activeElement = newActiveElement;
                    if (initialElementFocused) {
                        _this._updateTabIndexes();
                    }
                }
                _this._dotNetRef.invokeMethodAsync("JSOnActiveElementChanged");
                if (doNotAllowFocusEventToPropagate) {
                    ev.stopPropagation();
                }
            };
            this._onKeyDownCapture = function (ev) {
                if (ev.which === 9 /* tab */) {
                    outerZones.forEach(function (zone) { return zone._updateTabIndexes(); });
                }
            };
            this._onMouseDown = function (ev) {
                if (_this._portalContainsElement(ev.target)) {
                    // If the event target is inside a portal do not process the event.
                    return;
                }
                var disabled = _this._focusZoneProps.disabled;
                if (disabled) {
                    return;
                }
                var target = ev.target;
                var path = [];
                while (target && target !== _this._root) {
                    path.push(target);
                    target = window.BlazorFluentUiBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
                }
                while (path.length) {
                    target = path.pop();
                    if (target && window.BlazorFluentUiBaseComponent.isElementTabbable(target)) {
                        _this._setActiveElement(target, true);
                    }
                    if (window.BlazorFluentUiBaseComponent.isElementFocusZone(target)) {
                        // Stop here since the focus zone will take care of its own children.
                        break;
                    }
                }
            };
            this._root = focusZoneProps.root;
            this._focusZoneProps = focusZoneProps;
            this._dotNetRef = dotNetRef;
            this._focusAlignment = {
                x: 0,
                y: 0
            };
            this._root.addEventListener("keydown", this._onKeyDown, false);
            this._root.addEventListener("focusin", this._onFocus, false);
            this._root.addEventListener("mousedown", this._onMouseDown, false);
            this.initialized();
        }
        FocusZoneInternal.prototype.updateFocusZone = function (props) {
            this._focusZoneProps = props;
            allInstances[props.id] = this;
            if (this._root) {
                var windowElement = window.BlazorFluentUiBaseComponent.getWindow(this._root);
                var parentElement = window.BlazorFluentUiBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
                while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                    if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                        this._isInnerZone = true;
                        break;
                    }
                    parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
                }
                if (!this._isInnerZone) {
                    outerZones.add(this);
                }
                if (windowElement && outerZones.size === 1) {
                    _disposeGlobalKeyDownListener = window.BlazorFluentUiBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
                }
                this._disposables.push(window.BlazorFluentUiBaseComponent.on(this._root, 'blur', this._onBlur, true));
                // Assign initial tab indexes so that we can set initial focus as appropriate.
                this._updateTabIndexes();
                // using a hack to detect whether the passed in HTMLElement is valid (came from a legitimate .NET ElementReference)
                if ((this._focusZoneProps.defaultActiveElement).__internalId !== null) {
                    if (this._activeElement != this._focusZoneProps.defaultActiveElement) {
                        this._activeElement = this._focusZoneProps.defaultActiveElement;
                        this.focus();
                    }
                }
            }
        };
        FocusZoneInternal.prototype.initialized = function () {
            var windowElement = window.BlazorFluentUiBaseComponent.getWindow(this._root);
            var parentElement = window.BlazorFluentUiBaseComponent.getParent(this._root, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== document.body && parentElement.nodeType === 1) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                    this._isInnerZone = true;
                    break;
                }
                parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            if (!this._isInnerZone) {
                outerZones.add(this);
            }
            if (windowElement && outerZones.size === 1) {
                _disposeGlobalKeyDownListener = window.BlazorFluentUiBaseComponent.on(windowElement, 'keydown', this._onKeyDownCapture, true);
            }
            this._disposables.push(window.BlazorFluentUiBaseComponent.on(this._root, 'blur', this._onBlur, true));
            // Assign initial tab indexes so that we can set initial focus as appropriate.
            this._updateTabIndexes();
            // using a hack to detect whether the passed in HTMLElement is valid (came from a legitimate .NET ElementReference)
            if ((this._focusZoneProps.defaultActiveElement).__internalId !== null) {
                this._activeElement = this._focusZoneProps.defaultActiveElement;
                this.focus();
            }
        };
        /**
       * When focus is in the zone at render time but then all focusable elements are removed,
       * we "park" focus temporarily on the root. Once we update with focusable children, we restore
       * focus to the closest path from previous. If the user tabs away from the parked container,
       * we restore focusability to the pre-parked state.
       */
        FocusZoneInternal.prototype._setParkedFocus = function (isParked) {
            if (this._root && this._isParked !== isParked) {
                this._isParked = isParked;
                if (isParked) {
                    if (!this._focusZoneProps.allowFocusRoot) {
                        this._parkedTabIndex = this._root.getAttribute('tabindex');
                        this._root.setAttribute('tabindex', '-1');
                    }
                    this._root.focus();
                }
                else {
                    if (!this._focusZoneProps.allowFocusRoot) {
                        if (this._parkedTabIndex) {
                            this._root.setAttribute('tabindex', this._parkedTabIndex);
                            this._parkedTabIndex = undefined;
                        }
                        else {
                            this._root.removeAttribute('tabindex');
                        }
                    }
                }
            }
        };
        FocusZoneInternal.prototype.unRegister = function () {
            if (!this._isInnerZone) {
                outerZones.delete(this);
            }
            this._disposables.forEach(function (d) { return d(); });
            if (outerZones.size === 0 && _disposeGlobalKeyDownListener) {
                _disposeGlobalKeyDownListener();
            }
            this._root.removeEventListener("keydown", this._onKeyDown, false);
            this._root.removeEventListener("focus", this._onFocus, false);
            this._root.removeEventListener("mousedown", this._onMouseDown, false);
        };
        FocusZoneInternal.prototype.focus = function (forceIntoFirstElement) {
            if (forceIntoFirstElement === void 0) { forceIntoFirstElement = false; }
            if (this._root) {
                if (!forceIntoFirstElement && this._root.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' && this._isInnerZone) {
                    var ownerZoneElement = this._getOwnerZone(this._root);
                    if (ownerZoneElement !== this._root) {
                        var ownerZone = allInstances[ownerZoneElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
                        return !!ownerZone && ownerZone.focusElement(this._root);
                    }
                    return false;
                }
                else if (!forceIntoFirstElement &&
                    this._activeElement &&
                    window.BlazorFluentUiBaseComponent.elementContains(this._root, this._activeElement) &&
                    window.BlazorFluentUiBaseComponent.isElementTabbable(this._activeElement)) {
                    this._activeElement.focus();
                    return true;
                }
                else {
                    var firstChild = this._root.firstChild;
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(this._root, firstChild, true));
                }
            }
            return false;
        };
        FocusZoneInternal.prototype.focusElement = function (element) {
            var onBeforeFocusExists = this._focusZoneProps.onBeforeFocusExists;
            if (onBeforeFocusExists && !this._dotNetRef.invokeMethodAsync("JSOnBeforeFocus")) {
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
        };
        FocusZoneInternal.prototype._updateTabIndexes = function (element) {
            if (!element && this._root) {
                this._defaultFocusElement = null;
                element = this._root;
                if (this._activeElement && !window.BlazorFluentUiBaseComponent.elementContains(element, this._activeElement)) {
                    this._activeElement = null;
                }
            }
            // If active element changes state to disabled, set it to null.
            // Otherwise, we lose keyboard accessibility to other elements in focus zone.
            if (this._activeElement && !window.BlazorFluentUiBaseComponent.isElementTabbable(this._activeElement)) {
                this._activeElement = null;
            }
            var childNodes = element && element.children;
            for (var childIndex = 0; childNodes && childIndex < childNodes.length; childIndex++) {
                var child = childNodes[childIndex];
                if (!window.BlazorFluentUiBaseComponent.isElementFocusZone(child)) {
                    // If the item is explicitly set to not be focusable then TABINDEX needs to be set to -1.
                    if (child.getAttribute && child.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'false') {
                        child.setAttribute(TABINDEX, '-1');
                    }
                    if (window.BlazorFluentUiBaseComponent.isElementTabbable(child)) {
                        if (this._focusZoneProps.disabled) {
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
        };
        FocusZoneInternal.prototype._getOwnerZone = function (element) {
            var parentElement = window.BlazorFluentUiBaseComponent.getParent(element, ALLOW_VIRTUAL_ELEMENTS);
            while (parentElement && parentElement !== this._root && parentElement !== document.body) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(parentElement)) {
                    return parentElement;
                }
                parentElement = window.BlazorFluentUiBaseComponent.getParent(parentElement, ALLOW_VIRTUAL_ELEMENTS);
            }
            return parentElement;
        };
        FocusZoneInternal.prototype._setActiveElement = function (element, forceAlignment) {
            var previousActiveElement = this._activeElement;
            this._activeElement = element;
            if (previousActiveElement) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(previousActiveElement)) {
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
        };
        FocusZoneInternal.prototype._setFocusAlignment = function (element, isHorizontal, isVertical) {
            if (this._focusZoneProps.direction === FocusZoneDirection.bidirectional && (!this._focusAlignment || isHorizontal || isVertical)) {
                var rect = element.getBoundingClientRect();
                var left = rect.left + rect.width / 2;
                var top_1 = rect.top + rect.height / 2;
                if (!this._focusAlignment) {
                    this._focusAlignment = {
                        x: left,
                        y: top_1
                    };
                }
                if (isHorizontal) {
                    this._focusAlignment.x = left;
                }
                if (isVertical) {
                    this._focusAlignment.y = top_1;
                }
            }
        };
        FocusZoneInternal.prototype._isImmediateDescendantOfZone = function (element) {
            return this._getOwnerZone(element) === this._root;
        };
        /**
   * Traverse to find first child zone.
   */
        FocusZoneInternal.prototype._getFirstInnerZone = function (rootElement) {
            rootElement = rootElement || this._activeElement || this._root;
            if (!rootElement) {
                return null;
            }
            if (window.BlazorFluentUiBaseComponent.isElementFocusZone(rootElement)) {
                return allInstances[rootElement.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
            }
            var child = rootElement.firstElementChild;
            while (child) {
                if (window.BlazorFluentUiBaseComponent.isElementFocusZone(child)) {
                    return allInstances[child.getAttribute(FOCUSZONE_ID_ATTRIBUTE)];
                }
                var match = this._getFirstInnerZone(child);
                if (match) {
                    return match;
                }
                child = child.nextElementSibling;
            }
            return null;
        };
        /**
       * Walk up the dom try to find a focusable element.
       */
        FocusZoneInternal.prototype._tryInvokeClickForFocusable = function (target) {
            if (target === this._root) {
                return false;
            }
            do {
                if (target.tagName === 'BUTTON' || target.tagName === 'A' || target.tagName === 'INPUT' || target.tagName === 'TEXTAREA') {
                    return false;
                }
                if (this._isImmediateDescendantOfZone(target) &&
                    target.getAttribute(IS_FOCUSABLE_ATTRIBUTE) === 'true' &&
                    target.getAttribute(IS_ENTER_DISABLED_ATTRIBUTE) !== 'true') {
                    window.BlazorFluentUiBaseComponent.raiseClick(target);
                    return true;
                }
                target = window.BlazorFluentUiBaseComponent.getParent(target, ALLOW_VIRTUAL_ELEMENTS);
            } while (target !== this._root);
            return false;
        };
        /**
        * Returns true if the element is a descendant of the FocusZone through a React portal.
        */
        FocusZoneInternal.prototype._portalContainsElement = function (element) {
            // This might break our control when used inside a Layer...
            return false;
            //return element && !!this._root && BlazorFluentUiBaseComponent portalContainsElement(element, this._root.current);
        };
        FocusZoneInternal.prototype._isElementInput = function (element) {
            if (element && element.tagName && (element.tagName.toLowerCase() === 'input' || element.tagName.toLowerCase() === 'textarea')) {
                return true;
            }
            return false;
        };
        FocusZoneInternal.prototype._shouldInputLoseFocus = function (element, isForward) {
            // If a tab was used, we want to focus on the next element.
            if (!this._processingTabKey && element && element.type && ALLOWED_INPUT_TYPES.indexOf(element.type.toLowerCase()) > -1) {
                var selectionStart = element.selectionStart;
                var selectionEnd = element.selectionEnd;
                var isRangeSelected = selectionStart !== selectionEnd;
                var inputValue = element.value;
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
                    (!!this._focusZoneProps.handleTabKey && !(this._focusZoneProps.shouldInputLoseFocusOnArrowKeyExists && this._dotNetRef.invokeMethodAsync("JSShouldInputLoseFocusOnArrowKey")))) {
                    return false;
                }
            }
            return true;
        };
        FocusZoneInternal.prototype._shouldWrapFocus = function (element, noWrapDataAttribute) {
            return !!this._focusZoneProps.checkForNoWrap ? window.BlazorFluentUiBaseComponent.shouldWrapFocus(element, noWrapDataAttribute) : true;
        };
        FocusZoneInternal.prototype._moveFocus = function (isForward, getDistanceFromCenter, ev, useDefaultWrap) {
            if (useDefaultWrap === void 0) { useDefaultWrap = true; }
            var element = this._activeElement;
            var candidateDistance = -1;
            var candidateElement = undefined;
            var changedFocus = false;
            var isBidirectional = this._focusZoneProps.direction === FocusZoneDirection.bidirectional;
            if (!element || !this._root) {
                return false;
            }
            if (this._isElementInput(element)) {
                if (!this._shouldInputLoseFocus(element, isForward)) {
                    return false;
                }
            }
            var activeRect = isBidirectional ? element.getBoundingClientRect() : null;
            do {
                element = (isForward ? window.BlazorFluentUiBaseComponent.getNextElement(this._root, element) : window.BlazorFluentUiBaseComponent.getPreviousElement(this._root, element));
                if (isBidirectional) {
                    if (element) {
                        var targetRect = element.getBoundingClientRect();
                        var elementDistance = getDistanceFromCenter(activeRect, targetRect);
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
            else if (this._focusZoneProps.isCircularNavigation && useDefaultWrap) {
                if (isForward) {
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getNextElement(this._root, this._root.firstElementChild, true));
                }
                else {
                    return this.focusElement(window.BlazorFluentUiBaseComponent.getPreviousElement(this._root, this._root.lastElementChild, true, true, true));
                }
            }
            return changedFocus;
        };
        FocusZoneInternal.prototype._moveFocusDown = function () {
            var _this = this;
            var targetTop = -1;
            var leftAlignment = this._focusAlignment.x;
            if (this._moveFocus(true, function (activeRect, targetRect) {
                var distance = -1;
                // ClientRect values can be floats that differ by very small fractions of a decimal.
                // If the difference between top and bottom are within a pixel then we should treat
                // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
                // but without Math.Floor they will be handled incorrectly.
                var targetRectTop = Math.floor(targetRect.top);
                var activeRectBottom = Math.floor(activeRect.bottom);
                if (targetRectTop < activeRectBottom) {
                    if (!_this._shouldWrapFocus(_this._activeElement, NO_VERTICAL_WRAP)) {
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
        };
        FocusZoneInternal.prototype._moveFocusUp = function () {
            var _this = this;
            var targetTop = -1;
            var leftAlignment = this._focusAlignment.x;
            if (this._moveFocus(false, function (activeRect, targetRect) {
                var distance = -1;
                // ClientRect values can be floats that differ by very small fractions of a decimal.
                // If the difference between top and bottom are within a pixel then we should treat
                // them as equivalent by using Math.floor. For instance 5.2222 and 5.222221 should be equivalent,
                // but without Math.Floor they will be handled incorrectly.
                var targetRectBottom = Math.floor(targetRect.bottom);
                var targetRectTop = Math.floor(targetRect.top);
                var activeRectTop = Math.floor(activeRect.top);
                if (targetRectBottom > activeRectTop) {
                    if (!_this._shouldWrapFocus(_this._activeElement, NO_VERTICAL_WRAP)) {
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
        };
        FocusZoneInternal.prototype._moveFocusLeft = function () {
            var _this = this;
            var shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
            if (this._moveFocus(window.BlazorFluentUiBaseComponent.getRTL(), function (activeRect, targetRect) {
                var distance = -1;
                var topBottomComparison;
                if (window.BlazorFluentUiBaseComponent.getRTL()) {
                    // When in RTL, this comparison should be the same as the one in _moveFocusRight for LTR.
                    // Going left at a leftmost rectangle will go down a line instead of up a line like in LTR.
                    // This is important, because we want to be comparing the top of the target rect
                    // with the bottom of the active rect.
                    topBottomComparison = parseFloat(targetRect.top.toFixed(3)) < parseFloat(activeRect.bottom.toFixed(3));
                }
                else {
                    topBottomComparison = parseFloat(targetRect.bottom.toFixed(3)) > parseFloat(activeRect.top.toFixed(3));
                }
                if (topBottomComparison && targetRect.right <= activeRect.right && _this._focusZoneProps.direction !== FocusZoneDirection.vertical) {
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
        };
        FocusZoneInternal.prototype._moveFocusRight = function () {
            var _this = this;
            var shouldWrap = this._shouldWrapFocus(this._activeElement, NO_HORIZONTAL_WRAP);
            if (this._moveFocus(!window.BlazorFluentUiBaseComponent.getRTL(), function (activeRect, targetRect) {
                var distance = -1;
                var topBottomComparison;
                if (window.BlazorFluentUiBaseComponent.getRTL()) {
                    // When in RTL, this comparison should be the same as the one in _moveFocusLeft for LTR.
                    // Going right at a rightmost rectangle will go up a line instead of down a line like in LTR.
                    // This is important, because we want to be comparing the bottom of the target rect
                    // with the top of the active rect.
                    topBottomComparison = parseFloat(targetRect.bottom.toFixed(3)) > parseFloat(activeRect.top.toFixed(3));
                }
                else {
                    topBottomComparison = parseFloat(targetRect.top.toFixed(3)) < parseFloat(activeRect.bottom.toFixed(3));
                }
                if (topBottomComparison && targetRect.left >= activeRect.left && _this._focusZoneProps.direction !== FocusZoneDirection.vertical) {
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
        };
        return FocusZoneInternal;
    }());
})(BlazorFluentUiFocusZone || (BlazorFluentUiFocusZone = {}));
//interface Window {
//    BlazorFluentUiFocusZone: typeof BlazorFluentUiFocusZone
//}
//window.BlazorFluentUiFocusZone = BlazorFluentUiFocusZone;
window['BlazorFluentUiFocusZone'] = BlazorFluentUiFocusZone || {};
//# sourceMappingURL=focusZone.js.map