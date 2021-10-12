var scrollHandler;
var resizeHandler;
var focusHandler;
var clickHandler;
export function registerHandlers(targetElement, calloutRef) {
    if (targetElement) {
        var window = targetElement.ownerDocument.defaultView;
        function onScroll(ev) {
            if (checkTarget(ev, targetElement)) {
                calloutRef.invokeMethodAsync("ScrollHandler");
            }
            ;
        }
        scrollHandler = onScroll.bind(this);
        window.addEventListener("scroll", scrollHandler);
        function onResize(ev) {
            if (checkTarget(ev, targetElement)) {
                calloutRef.invokeMethodAsync("ResizeHandler");
            }
            ;
        }
        resizeHandler = onResize.bind(this);
        window.addEventListener("resize", resizeHandler);
        function onFocus(ev) {
            var outsideCallout = true;
            outsideCallout = checkTarget(ev, targetElement);
            if (outsideCallout)
                calloutRef.invokeMethodAsync("FocusHandler");
        }
        focusHandler = onFocus.bind(this);
        document.documentElement.addEventListener("focus", focusHandler);
    }
    function onClick(ev) {
        var outsideCallout = true;
        outsideCallout = checkTarget(ev, targetElement);
        if (outsideCallout)
            calloutRef.invokeMethodAsync("ClickHandler");
    }
    clickHandler = onClick.bind(this);
    document.documentElement.addEventListener("click", clickHandler);
}
export function unregisterHandlers() {
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
function checkTarget(ev, targetElement) {
    const target = ev.target;
    const isEventTargetOutsideCallout = !elementContains(targetElement, target);
    return isEventTargetOutsideCallout;
}
function elementContains(parent, child, allowVirtualParents = true) {
    let isContained = false;
    if (parent && child) {
        if (allowVirtualParents) {
            isContained = false;
            while (child) {
                let nextParent = getParent(child);
                // console.log("NextParent: " + nextParent);
                if (nextParent === parent) {
                    isContained = true;
                    break;
                }
                child = nextParent;
            }
        }
        else if (parent.contains) {
            isContained = parent.contains(child);
        }
    }
    return isContained;
}
function getParent(child) {
    return child && (child.parentNode && child.parentNode);
}
export function getWindow(element) {
    return element.ownerDocument.defaultView;
}
export function getWindowRect() {
    var rect = {
        width: window.innerWidth,
        height: window.innerHeight,
        top: 0,
        left: 0
    };
    return rect;
}
;
