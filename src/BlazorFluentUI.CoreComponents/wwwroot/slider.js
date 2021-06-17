class Handler {
    static addListener(ref, element, event, handler, capture) {
        let listeners;
        if (this.objectListeners.has(ref)) {
            listeners = this.objectListeners.get(ref);
        }
        else {
            listeners = new Map();
            this.objectListeners.set(ref, listeners);
        }
        element.addEventListener(event, handler, capture);
        listeners.set(event, { capture: capture, event: event, handler: handler, element: element });
    }
    static removeListener(ref, event) {
        if (this.objectListeners.has(ref)) {
            let listeners = this.objectListeners.get(ref);
            if (listeners.has(event)) {
                var handler = listeners.get(event);
                handler.element.removeEventListener(handler.event, handler.handler, handler.capture);
            }
            listeners.delete[event];
        }
    }
}
Handler.objectListeners = new Map();
export function registerMouseOrTouchStart(slider, slideBox, sliderLine) {
    Handler.addListener(slider, slideBox, "mousedown", (ev) => { onMouseMoveOrTouchStart(slider, sliderLine, ev); }, true);
    Handler.addListener(slider, slideBox, "touchstart", (ev) => { onMouseMoveOrTouchStart(slider, sliderLine, ev); }, true);
    Handler.addListener(slider, slideBox, "keydown", (ev) => { onKeyDown(slider, ev); }, true);
}
function onMouseMoveOrTouchStart(slider, sliderLine, event) {
    if (event.type === 'mousedown') {
        Handler.addListener(slider, window, "mousemove", (ev) => { onMouseMoveOrTouchMove(slider, sliderLine, ev); }, true);
        Handler.addListener(slider, window, "mouseup", (ev) => { onMouseUpOrTouchEnd(slider, sliderLine, ev); }, true);
    }
    else if (event.type === 'touchstart') {
        Handler.addListener(slider, window, "touchmove", (ev) => { onMouseMoveOrTouchMove(slider, sliderLine, ev); }, true);
        Handler.addListener(slider, window, "touchend", (ev) => { onMouseUpOrTouchEnd(slider, sliderLine, ev); }, true);
    }
    onMouseMoveOrTouchMove(slider, sliderLine, event, true);
}
function onMouseMoveOrTouchMove(slider, sliderLine, event, suppressEventCancelation) {
    let sliderPositionRect = sliderLine.getBoundingClientRect();
    let horizontalPosition = getPosition(event, false);
    let verticalPosition = getPosition(event, true);
    slider.invokeMethodAsync("MouseOrTouchMove", sliderPositionRect, horizontalPosition, verticalPosition);
    if (suppressEventCancelation) {
        event.preventDefault();
        event.stopPropagation();
    }
}
function getPosition(event, vertical) {
    let currentPosition;
    switch (event.type) {
        case 'mousedown':
        case 'mousemove':
            currentPosition = !vertical ? event.clientX : event.clientY;
            break;
        case 'touchstart':
        case 'touchmove':
            currentPosition = !vertical
                ? event.touches[0].clientX
                : event.touches[0].clientY;
            break;
    }
    return currentPosition;
}
async function onMouseUpOrTouchEnd(slider, sliderLine, event, suppressEventCancelation) {
    await slider.invokeMethodAsync("MouseOrTouchEnd");
    if (event.type === 'mouseup') {
        Handler.removeListener(slider, "mousemove");
        Handler.removeListener(slider, "mouseup");
    }
    else if (event.type === 'touchend') {
        Handler.removeListener(slider, "touchmove");
        Handler.removeListener(slider, "touchend");
    }
}
function onKeyDown(slider, event) {
    let diff;
    let value;
    switch (event.which) {
        case 39 /* right */: //right arrow
        case 38 /* up */: //up arrow
            slider.invokeMethodAsync("OnKeyDown", { step: +1 });
            break;
        case 37 /* left */: //left arrow
        case 40 /* down */: //down arrow
            slider.invokeMethodAsync("OnKeyDown", { step: -1 });
            break;
        case 36 /* home */: //home
            slider.invokeMethodAsync("OnKeyDown", { min: true });
            break;
        case 35 /* end */: //end
            slider.invokeMethodAsync("OnKeyDown", { max: true });
            break;
        default:
            return;
    }
    event.preventDefault();
    event.stopPropagation();
}
export function unregisterHandlers(ref) {
    Handler.removeListener(ref, "mousedown");
    Handler.removeListener(ref, "touchstart");
    Handler.removeListener(ref, "keydown");
}
