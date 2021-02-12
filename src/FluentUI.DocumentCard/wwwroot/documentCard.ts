/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />

namespace FluentUIDocumentCard {
    class CardTitleMap {
        id: string;
        element: HTMLDivElement;
        dotnet: any;
        state: CardTitleState;
        resizeFunction: (e: any) => void;

        stateChanged() {
            this.dotnet.invokeMethodAsync("UpdateTitle", this.state.truncatedTitleFirstPiece, this.state.truncatedTitleSecondPiece);
        }
    }

    class CardTitleState {
        clientWidth: number;
        previousTitle: string;
        needMeasurement: boolean;
        originalTitle: string;
        truncatedTitleFirstPiece: string;
        truncatedTitleSecondPiece: string;
        watchResize: boolean;

        constructor(shouldTruncate: boolean) {
            this.needMeasurement = !!shouldTruncate;
        }
    }

    const cardTitles: Array<CardTitleMap> = new Array();

    export function getElement(id: string): CardTitleMap {
        for (var i = 0; i < cardTitles.length; i++) {
            if (cardTitles[i].id === id) {
                return cardTitles[i];
            }
        }
        return null;
    }
    export function addElement(id: string, element: HTMLDivElement, dotnet: any, shouldTruncate: boolean, orgTitle: string) {
        let title = new CardTitleMap();
        title.state = new CardTitleState(shouldTruncate);
        title.state.originalTitle = orgTitle;
        title.state.previousTitle = orgTitle;
        title.id = id;
        title.element = element;
        title.dotnet = dotnet;
        title.state.watchResize = shouldTruncate;
        title.resizeFunction = e => {
            if (!title.state.watchResize) {
                return;
            }
            title.state.watchResize = false;
            
            setTimeout(() => {
                console.log('resize');
                title.dotnet.invokeMethodAsync("UpdateneedMeasurement");
                title.state.truncatedTitleFirstPiece = '';
                title.state.truncatedTitleSecondPiece = '';
        
                truncateTitle(title);
                title.state.watchResize = true;
            }, 500);
        };

        window.addEventListener('resize', title.resizeFunction);

        cardTitles.push(title);
    }
    export function removelement(id: string) {
        let index = -1;
        for (let i = 0; i < cardTitles.length; i++) {
            if (cardTitles[i].id === id) {
                index = i;
                break;
            }
        }
        if (index >= 0) {
            let title = cardTitles[index];
            window.removeEventListener('resize', title.resizeFunction);
            cardTitles.splice(index, 1);
        }
    }

    function initInternal(title: CardTitleMap) {
        if (title.state.needMeasurement) {
            requestAnimationFrame(time => {
                truncateTitle(title);
            });
        }
    }

    export function initTitle(id: string, element: HTMLDivElement, dotnet: any, shouldTruncate: boolean, orgTitle: string): void {
        let title = getElement(id);
        if (title === null) {
            addElement(id, element, dotnet, shouldTruncate, orgTitle);
            title = getElement(id);
            initInternal(title);
        }
    }

    function truncateTitle(cardTitle: CardTitleMap):void {
        if (!cardTitle) {
            return;
        }
        const TRUNCATION_VERTICAL_OVERFLOW_THRESHOLD = 5;
        let el = document.getElementById(cardTitle.id);
        const style: CSSStyleDeclaration = getComputedStyle(el);
        if (style.width && style.lineHeight && style.height) {
            const { clientWidth, scrollWidth } = el;
            const lines: number = Math.floor(
                (parseInt(style.height, 10) + TRUNCATION_VERTICAL_OVERFLOW_THRESHOLD) / parseInt(style.lineHeight, 10),
            );
            const overFlowRate: number = scrollWidth / (parseInt(style.width, 10) * lines);
            if (overFlowRate > 1) {
                const truncatedLength: number = cardTitle.state.originalTitle.length / overFlowRate - 3 /** Saved for separator */;
                cardTitle.state.truncatedTitleFirstPiece = cardTitle.state.originalTitle.slice(0, truncatedLength / 2);
                cardTitle.state.truncatedTitleSecondPiece = cardTitle.state.originalTitle.slice(cardTitle.state.originalTitle.length - truncatedLength / 2);
                cardTitle.stateChanged();
            }
        }
    }
}