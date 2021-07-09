import * as FluentUIBaseComponent from './baseComponent.js'

type DotNetReferenceType = FluentUIBaseComponent.DotNetReferenceType;
type IRectangle = FluentUIBaseComponent.IRectangle;

var scrollHandler: (ev: Event) => void;
var resizeHandler: (ev: Event) => void;
var focusHandler: (ev: Event) => void;
var clickHandler: (ev: Event) => void;


export function registerHandlers(targetElement: HTMLElement, calloutRef: DotNetReferenceType): void {
    if (targetElement) {
        var window = targetElement.ownerDocument.defaultView;

        function onScroll(ev: Event) {
            if (checkTarget(ev, targetElement)) { calloutRef.invokeMethodAsync("ScrollHandler"); };
        }
        scrollHandler = onScroll.bind(this);
        window.addEventListener("scroll", scrollHandler);

        function onResize(ev: Event) {
            if (checkTarget(ev, targetElement)) { calloutRef.invokeMethodAsync("ResizeHandler"); };
        }
        resizeHandler = onResize.bind(this);
        window.addEventListener("resize", resizeHandler);

        function onFocus(ev: Event) {
            var outsideCallout = true;

            outsideCallout = checkTarget(ev, targetElement);

            if (outsideCallout)
                calloutRef.invokeMethodAsync("FocusHandler");
        }
        focusHandler = onFocus.bind(this);
        document.documentElement.addEventListener("focus", focusHandler);
    }

    function onClick(ev: Event) {
        var outsideCallout = true;
        
        outsideCallout = checkTarget(ev, targetElement);
        
        if (outsideCallout)
            calloutRef.invokeMethodAsync("ClickHandler");
    }
    clickHandler = onClick.bind(this);
    document.documentElement.addEventListener("click", clickHandler);
}

export function unregisterHandlers(): void {

    document.documentElement.removeEventListener("click", clickHandler);
    if (focusHandler !== undefined) {
        document.documentElement.removeEventListener("focus", focusHandler);
    }
    if (resizeHandler !== undefined) {
        window.removeEventListener("resize", resizeHandler);
    }
    if (scrollHandler !== undefined) {
        window.removeEventListener("scroll", scrollHandler);
    }
}

function checkTarget(ev: Event, targetElement: HTMLElement): boolean {
    const target = ev.target as HTMLElement;
    const isEventTargetOutsideCallout = !elementContains(targetElement, target);
    return isEventTargetOutsideCallout;
}

function elementContains(parent: HTMLElement | null, child: HTMLElement | null, allowVirtualParents: boolean = true): boolean {
    let isContained = false;
    if (parent && child) {
        if (allowVirtualParents) {
            isContained = false;
            while (child) {
                let nextParent: HTMLElement | null = getParent(child);
                // console.log("NextParent: " + nextParent);
                if (nextParent === parent) {
                    isContained = true;
                    break;
                }
                child = nextParent;
            }
        } else if (parent.contains) {
            isContained = parent.contains(child);
        }
    }
    return isContained;
}

function getParent(child: HTMLElement): HTMLElement | null {
    return child && (child.parentNode && (child.parentNode as HTMLElement));
}

export function getWindow(element: HTMLElement): Window {
    return element.ownerDocument.defaultView;
}

export function getWindowRect(): IRectangle {
    var rect: IRectangle = {
        width: window.innerWidth,// - scrollbarwidth
        height: window.innerHeight,
        top: 0,
        left: 0
    }
    return rect;
};