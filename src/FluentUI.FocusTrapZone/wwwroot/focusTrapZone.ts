//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }
/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />

namespace FluentUIFocusTrapZone { 


    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    const IS_FOCUSABLE_ATTRIBUTE = 'data-is-focusable';
    const d = 445;
    const IS_VISIBLE_ATTRIBUTE = 'data-is-visible';
    const FOCUSZONE_ID_ATTRIBUTE = 'data-focuszone-id';
    const FOCUSZONE_SUB_ATTRIBUTE = 'data-is-sub-focuszone';

    interface IFocusTrapZoneProps {
        rootElement: HTMLElement;
        firstBumper: HTMLElement;
        lastBumper: HTMLElement;
        disabled: boolean;
        disableFirstFocus: boolean;
        elementToFocusOnDismiss: HTMLElement;
        firstFocusableSelector: string;
        focusPreviouslyFocusedInnerElement: boolean;
        forceFocusInsideTrap: boolean;
        ignoreExternalFocusing: boolean;
        isClickableOutsideFocusTrap: boolean;
    }

    class FocusTrapZoneInternal {

        private static _focusStack: FocusTrapZoneInternal[] = [];

        private _prevProps: IFocusTrapZoneProps;
        private _props: IFocusTrapZoneProps;
        private _dotNetRef: DotNetReferenceType;

        private _previouslyFocusedElementInTrapZone?: HTMLElement;
        private _previouslyFocusedElementOutsideTrapZone: HTMLElement;

        private _hasFocus = true;

        constructor(focusTrapZoneProps: IFocusTrapZoneProps, dotNetRef: DotNetReferenceType ) {
            this._props = focusTrapZoneProps;
            this._dotNetRef = dotNetRef;

            this._props.rootElement.addEventListener("focus", this._onRootFocus, false);
            this._props.rootElement.addEventListener("blur", this._onRootBlur, false);

            this._props.firstBumper.addEventListener("focus", this._onFirstBumperFocus, false);
            this._props.lastBumper.addEventListener("focus", this._onLastBumperFocus, false);

            this._bringFocusIntoZone();
        }

        public unRegister(): void {
            this._props.rootElement.removeEventListener("focus", this._onRootFocus, false);
            this._props.rootElement.removeEventListener("blur", this._onRootBlur, false);

            this._props.firstBumper.removeEventListener("focus", this._onFirstBumperFocus, false);
            this._props.lastBumper.removeEventListener("focus", this._onLastBumperFocus, false);
        }

        public updateProps(props: IFocusTrapZoneProps) {
            this._prevProps = this._props;
            this._props = props;
            //bumpers and root should be the same...
            if ((!this._prevProps.forceFocusInsideTrap && this._props.forceFocusInsideTrap) || (this._prevProps.disabled && !this._props.disabled)) {
                this._bringFocusIntoZone();
            } else if ((this._prevProps.forceFocusInsideTrap && !this._props.forceFocusInsideTrap) || (!this._prevProps.disabled && this._props.disabled)) {
                this._returnFocusToInitiator();
            }
        }

        public setDisabled(disabled: boolean) {
            this._props.disabled = disabled;
        }

        private _onRootFocus = (ev: FocusEvent) => {
            //if (this.props.onFocus) {
            //    this.props.onFocus(ev);
            //}
            this._hasFocus = true;
        }

        private _onRootBlur = (ev: FocusEvent) => {
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
                relatedTarget = document.activeElement as Element;
            }

            if (!FluentUIBaseComponent.elementContains(this._props.rootElement, relatedTarget as HTMLElement)) {
                this._hasFocus = false;
            }
        };

        private _onFirstBumperFocus = () => {
            this._onBumperFocus(true);
        };

        private _onLastBumperFocus = () => {
            this._onBumperFocus(false);
        };

        private _onBumperFocus = (isFirstBumper: boolean) => {
            if (this._props.disabled) {
                return;
            }

            const currentBumper = (isFirstBumper === this._hasFocus ? this._props.lastBumper : this._props.firstBumper) as HTMLElement;

            if (this._props.rootElement) {
                const nextFocusable =
                    isFirstBumper === this._hasFocus
                        ? getLastTabbable(this._props.rootElement, currentBumper, true, false)
                        : getFirstTabbable(this._props.rootElement, currentBumper, true, false);

                if (nextFocusable) {
                    if (this._isBumper(nextFocusable)) {
                        // This can happen when FTZ contains no tabbable elements. focus will take care of finding a focusable element in FTZ.
                        this.focus();
                    } else {
                        nextFocusable.focus();
                    }
                }
            }
        };

        private _bringFocusIntoZone(): void {
            const { elementToFocusOnDismiss, disabled = false, disableFirstFocus = false, rootElement } = this._props;

            if (disabled) {
                return;
            }

            FocusTrapZoneInternal._focusStack.push(this);

            this._previouslyFocusedElementOutsideTrapZone = elementToFocusOnDismiss && (<any>elementToFocusOnDismiss).__internalId != null
                ? elementToFocusOnDismiss
                : (document.activeElement as HTMLElement);
            if (!disableFirstFocus && !FluentUIBaseComponent.elementContains(rootElement, this._previouslyFocusedElementOutsideTrapZone)) {
                this.focus();
            }
        }

        private _returnFocusToInitiator(): void {
            const { ignoreExternalFocusing, rootElement } = this._props;

            FocusTrapZoneInternal._focusStack = FocusTrapZoneInternal._focusStack.filter((value: FocusTrapZoneInternal) => {
                return this !== value;
            });

            const activeElement = document.activeElement as HTMLElement;
            if (
                !ignoreExternalFocusing &&
                this._previouslyFocusedElementOutsideTrapZone &&
                typeof this._previouslyFocusedElementOutsideTrapZone.focus === 'function' &&
                (FluentUIBaseComponent.elementContains(rootElement, activeElement) || activeElement === document.body)
            ) {
                this._focusAsync(this._previouslyFocusedElementOutsideTrapZone);
            }
        }


        private _focusAsync(element: HTMLElement): void {
            if (!this._isBumper(element)) {
                focusAsync(element);
            }
        }

        private _isBumper(element: HTMLElement): boolean {
            return element === this._props.firstBumper || element === this._props.lastBumper;
        }

        public focus() {
            const { focusPreviouslyFocusedInnerElement, firstFocusableSelector, rootElement } = this._props;

            if (
                focusPreviouslyFocusedInnerElement &&
                this._previouslyFocusedElementInTrapZone &&
                FluentUIBaseComponent.elementContains(rootElement, this._previouslyFocusedElementInTrapZone)
            ) {
                // focus on the last item that had focus in the zone before we left the zone
                this._focusAsync(this._previouslyFocusedElementInTrapZone);
                return;
            }

            const focusSelector = firstFocusableSelector;

            let _firstFocusableChild: HTMLElement | null = null;

            if (rootElement) {
                if (focusSelector) {
                    _firstFocusableChild = rootElement.querySelector('.' + focusSelector);
                }

                // Fall back to first element if query selector did not match any elements.
                if (!_firstFocusableChild) {
                    _firstFocusableChild = getNextElement(rootElement, rootElement.firstChild as HTMLElement, false, false, false, true);
                }
            }
            if (_firstFocusableChild) {
                this._focusAsync(_firstFocusableChild);
            }
        }

    }

    var count = 0;
    var focusTrapZones: Map<FocusTrapZoneInternal> = {};

    export function register(props: IFocusTrapZoneProps, focusTrapZone: DotNetReferenceType) { //rootElement: HTMLElement, firstBumper: HTMLElement, lastBumper: HTMLElement, disabled: boolean, focusTrapZone: DotNetReferenceType) : number {
        let currentId = count++;
        
        focusTrapZones[currentId] = new FocusTrapZoneInternal(props, focusTrapZone);
               
        return currentId;
    }

    export function unregister(id: number): void {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.unRegister();
        }
        delete focusTrapZones[id];
    }

    export function updateProps(id: number, props: IFocusTrapZoneProps): void {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.updateProps(props);
        }
    }

    export function focus(id: number): void {
        let focusTrapZone = focusTrapZones[id];
        if (focusTrapZone) {
            focusTrapZone.focus();
        } 
    }

   

    //export function elementContains(parent: HTMLElement, child: HTMLElement, allowVirtualParents: boolean = true): boolean {
    //    let isContained = false;
    //    if (parent && child) {
    //        if (allowVirtualParents) {
    //            isContained = false;
    //            while (child) {
    //                let nextParent: HTMLElement | null = getParent(child);
    //                console.log("NextParent: " + nextParent);
    //                if (nextParent === parent) {
    //                    isContained = true;
    //                    break;
    //                }
    //                child = nextParent;
    //            }
    //        } else if (parent.contains) {
    //            isContained = parent.contains(child);
    //        }
    //    }
    //    return isContained;
    //}

    //export function getParent(child: HTMLElement, allowVirtualParents: boolean = true): HTMLElement | null {
    //    return child && (child.parentNode && (child.parentNode as HTMLElement));
    //}

    let targetToFocusOnNextRepaint: HTMLElement | { focus: () => void } | null | undefined = undefined;

    function focusAsync(element: HTMLElement | { focus: () => void } | undefined | null): void {
        if (element) {
            // An element was already queued to be focused, so replace that one with the new element
            if (targetToFocusOnNextRepaint) {
                targetToFocusOnNextRepaint = element;
                return;
            }

            targetToFocusOnNextRepaint = element;
            
            // element.focus() is a no-op if the element is no longer in the DOM, meaning this is always safe
            window.requestAnimationFrame(() => {
                targetToFocusOnNextRepaint && targetToFocusOnNextRepaint.focus();

                // We are done focusing for this frame, so reset the queued focus element
                targetToFocusOnNextRepaint = undefined;
            });

        }
    }

    function getNextElement(
        rootElement: HTMLElement,
        currentElement: HTMLElement | null,
        checkNode?: boolean,
        suppressParentTraversal?: boolean,
        suppressChildTraversal?: boolean,
        includeElementsInFocusZones?: boolean,
        allowFocusRoot?: boolean,
        tabbable?: boolean
    ): HTMLElement | null {
        if (!currentElement || (currentElement === rootElement && suppressChildTraversal && !allowFocusRoot)) {
            return null;
        }

        let isCurrentElementVisible = isElementVisible(currentElement);

        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }

        // Check its children.
        if (
            !suppressChildTraversal &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))
        ) {
            const childMatch = getNextElement(
                rootElement,
                currentElement.firstElementChild as HTMLElement,
                true,
                true,
                false,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );

            if (childMatch) {
                return childMatch;
            }
        }

        if (currentElement === rootElement) {
            return null;
        }

        // Check its sibling.
        const siblingMatch = getNextElement(
            rootElement,
            currentElement.nextElementSibling as HTMLElement,
            true,
            true,
            false,
            includeElementsInFocusZones,
            allowFocusRoot,
            tabbable
        );

        if (siblingMatch) {
            return siblingMatch;
        }

        if (!suppressParentTraversal) {
            return getNextElement(
                rootElement,
                currentElement.parentElement,
                false,
                false,
                true,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );
        }

        return null;
    }

    function getPreviousElement(
        rootElement: HTMLElement,
        currentElement: HTMLElement | null,
        checkNode ?: boolean,
        suppressParentTraversal ?: boolean,
        traverseChildren ?: boolean,
        includeElementsInFocusZones ?: boolean,
        allowFocusRoot ?: boolean,
        tabbable ?: boolean
    ): HTMLElement | null {
        if (!currentElement || (!allowFocusRoot && currentElement === rootElement)) {
            return null;
        }

        let isCurrentElementVisible = isElementVisible(currentElement);

        // Check its children.
        if (
            traverseChildren &&
            isCurrentElementVisible &&
            (includeElementsInFocusZones || !(isElementFocusZone(currentElement) || isElementFocusSubZone(currentElement)))
        ) {
            const childMatch = getPreviousElement(
                rootElement,
                currentElement.lastElementChild as HTMLElement,
                true,
                true,
                true,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );

            if (childMatch) {
                if ((tabbable && isElementTabbable(childMatch, true)) || !tabbable) {
                    return childMatch;
                }

                const childMatchSiblingMatch = getPreviousElement(
                    rootElement,
                    childMatch.previousElementSibling as HTMLElement,
                    true,
                    true,
                    true,
                    includeElementsInFocusZones,
                    allowFocusRoot,
                    tabbable
                );
                if (childMatchSiblingMatch) {
                    return childMatchSiblingMatch;
                }

                let childMatchParent = childMatch.parentElement;

                // At this point if we have not found any potential matches
                // start looking at the rest of the subtree under the currentParent.
                // NOTE: We do not want to recurse here because doing so could
                // cause elements to get skipped.
                while (childMatchParent && childMatchParent !== currentElement) {
                    const childMatchParentMatch = getPreviousElement(
                        rootElement,
                        childMatchParent.previousElementSibling as HTMLElement,
                        true,
                        true,
                        true,
                        includeElementsInFocusZones,
                        allowFocusRoot,
                        tabbable
                    );

                    if (childMatchParentMatch) {
                        return childMatchParentMatch;
                    }

                    childMatchParent = childMatchParent.parentElement;
                }
            }
        }

        // Check the current node, if it's not the first traversal.
        if (checkNode && isCurrentElementVisible && isElementTabbable(currentElement, tabbable)) {
            return currentElement;
        }

        // Check its previous sibling.
        const siblingMatch = getPreviousElement(
            rootElement,
            currentElement.previousElementSibling as HTMLElement,
            true,
            true,
            true,
            includeElementsInFocusZones,
            allowFocusRoot,
            tabbable
        );

        if (siblingMatch) {
            return siblingMatch;
        }

        // Check its parent.
        if (!suppressParentTraversal) {
            return getPreviousElement(
                rootElement,
                currentElement.parentElement,
                true,
                false,
                false,
                includeElementsInFocusZones,
                allowFocusRoot,
                tabbable
            );
        }

        return null;
    }

    function isElementVisible(element: HTMLElement | undefined | null): boolean {
        // If the element is not valid, return false.
        if (!element || !element.getAttribute) {
            return false;
        }

        const visibilityAttribute = element.getAttribute(IS_VISIBLE_ATTRIBUTE);

        // If the element is explicitly marked with the visibility attribute, return that value as boolean.
        if (visibilityAttribute !== null && visibilityAttribute !== undefined) {
            return visibilityAttribute === 'true';
        }

        // Fallback to other methods of determining actual visibility.
        return (
            element.offsetHeight !== 0 ||
            element.offsetParent !== null ||
            // tslint:disable-next-line:no-any
            (element as any).isVisible === true
        ); // used as a workaround for testing.
    }

    function isElementTabbable(element: HTMLElement, checkTabIndex?: boolean): boolean {
        // If this element is null or is disabled, it is not considered tabbable.
        if (!element || (element as HTMLButtonElement).disabled) {
            return false;
        }

        let tabIndex = 0;
        let tabIndexAttributeValue = null;

        if (element && element.getAttribute) {
            tabIndexAttributeValue = element.getAttribute('tabIndex');

            if (tabIndexAttributeValue) {
                tabIndex = parseInt(tabIndexAttributeValue, 10);
            }
        }

        let isFocusableAttribute = element.getAttribute ? element.getAttribute(IS_FOCUSABLE_ATTRIBUTE) : null;
        let isTabIndexSet = tabIndexAttributeValue !== null && tabIndex >= 0;

        const result =
            !!element &&
            isFocusableAttribute !== 'false' &&
            (element.tagName === 'A' ||
                element.tagName === 'BUTTON' ||
                element.tagName === 'INPUT' ||
                element.tagName === 'TEXTAREA' ||
                isFocusableAttribute === 'true' ||
                isTabIndexSet);

        return checkTabIndex ? tabIndex !== -1 && result : result;
    }

    function isElementFocusZone(element?: HTMLElement): boolean {
        return !!(element && element.getAttribute && !!element.getAttribute(FOCUSZONE_ID_ATTRIBUTE));
    }

    function isElementFocusSubZone(element?: HTMLElement): boolean {
        return !!(element && element.getAttribute && element.getAttribute(FOCUSZONE_SUB_ATTRIBUTE) === 'true');
    }

    function getFirstTabbable(
        rootElement: HTMLElement,
        currentElement: HTMLElement,
        includeElementsInFocusZones?: boolean,
        checkNode: boolean = true
    ): HTMLElement | null {
        return getNextElement(
            rootElement,
            currentElement,
            checkNode,
            false /*suppressParentTraversal*/,
            false /*suppressChildTraversal*/,
            includeElementsInFocusZones,
            false /*allowFocusRoot*/,
            true /*tabbable*/
        );
    }

    function getLastTabbable(
        rootElement: HTMLElement,
        currentElement: HTMLElement,
        includeElementsInFocusZones?: boolean,
        checkNode: boolean = true
    ): HTMLElement | null {
        return getPreviousElement(
            rootElement,
            currentElement,
            checkNode,
            false /*suppressParentTraversal*/,
            true /*traverseChildren*/,
            includeElementsInFocusZones,
            false /*allowFocusRoot*/,
            true /*tabbable*/
        );
    }
  
    //export function registerHandlers(targetElement: HTMLElement, focusTrapZone: DotNetReferenceType): number[] {
    //    var window = targetElement.ownerDocument.defaultView;

    //    var focusId = Handler.addListener(targetElement, "focus", (ev: Event) => { ev.preventDefault(); focusTrapZone.invokeMethodAsync("OnFocus"); }, false);
    //    var blurId = Handler.addListener(targetElement, "blur", (ev: Event) => { ev.preventDefault(); focusTrapZone.invokeMethodAsync("OnBlur"); }, false);
    //    return [focusId, blurId];
    //}

    //export function unregisterHandlers(ids: number[]): void {

    //    for (let id of ids) {
    //        Handler.removeListener(id);
    //    }
    //}

    //interface EventParams {
    //    element: HTMLElement | Window;
    //    event: string;
    //    handler: (ev: Event) => void;
    //    capture: boolean;
    //}

    interface Map<T> {
        [K: number]: T;
    }

    //class Handler {

    //    static i: number = 1;
    //    static listeners: Map<EventParams> = {};

    //    static addListener(element: HTMLElement | Window, event: string, handler: (ev: Event) => void, capture: boolean): number {
    //        element.addEventListener(event, handler, capture);
    //        this.listeners[this.i] = { capture: capture, event: event, handler: handler, element: element };
    //        return this.i++;
    //    }
    //    static removeListener(id: number): void {
    //        if (id in this.listeners) {
    //            var h = this.listeners[id];
    //            h.element.removeEventListener(h.event, h.handler, h.capture);
    //            delete this.listeners[id];
    //        }
    //    }
    //}
}

(<any>window)['FluentUIFocusTrapZone'] = FluentUIFocusTrapZone || {};

