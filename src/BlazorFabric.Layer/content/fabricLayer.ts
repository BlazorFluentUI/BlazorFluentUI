//declare interface Window { debounce(func: Function, wait: number, immediate: boolean): Function }


namespace BlazorFabricLayer {

    interface DotNetReferenceType {

        invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
        invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    }

    export function findHostElement(id: string) : HTMLElement{
        const element = document.getElementById(id);
        if (element === undefined) {
            return document.body;
        } else {
            return element;
        }
    }

    export function setLayerElement(id: string, classNames: string, insertFirst: boolean = false): HTMLElement {
        const host = findHostElement(id);

        const layerElement = document.createElement('div');
        layerElement.className = classNames;

        if (insertFirst) {
            host.insertBefore(layerElement, host.firstChild);
        } else {
            host.appendChild(layerElement);
        }

        return layerElement;
    }


}

window['BlazorFabricLayer'] = BlazorFabricLayer || {};

