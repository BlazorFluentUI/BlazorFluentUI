/// <reference path="../../FluentUI.BaseComponent/wwwroot/baseComponent.ts" />
let BlockEmbed = Quill.import('blots/block/embed');

class ImageBlot extends BlockEmbed {
    static create(value) {
        let node = super.create();
        node.setAttribute('alt', value.alt);
        node.setAttribute('src', value.src);
        if (value.width !== undefined && value.width !== null)
            node.setAttribute('width', value.width);
        if (value.height !== undefined && value.height !== null)
            node.setAttribute('height', value.height);
        return node;
    }

    static value(node) {
        return {
            alt: node.getAttribute('alt'),
            url: node.getAttribute('src'),
            width: node.getAttribute('width'),
            height: node.getAttribute('height')
        };
    }
}
ImageBlot.blotName = 'image';
ImageBlot.tagName = 'img';

//Quill.register(BoldBlot);
//Quill.register(ItalicBlot);
//Quill.register(UnderlineBlot);
Quill.register(ImageBlot);

function preventZoom(event) {
    if (event.ctrlKey === true && (event.which === 61 || event.which === 107 || event.which === 173 || event.which === 109 || event.which === 187 || event.which === 189)) {
        event.preventDefault();
    }
}

var FluentUIRichTextEditor = {

    //interface Map<T> {
    //    [K: number]: T;
    //}
    //interface DotNetReferenceType {

    //    invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
    //    invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    //}

    count: 0,
    allInstances: {},
    zoomPrevented: false,
    lastSelection: null,

    register: function(editorWindow, richTextEditorRef) {
        let currentId = this.count++;
        let quill = new Quill(editorWindow);
        let that = this;
        quill.on('editor-change', function (eventName, ...args) {
            if (eventName === "text-change")
                richTextEditorRef.invokeMethodAsync("TextChangedAsync", { html: quill.root.innerHTML, source: args[2] });
            else if (eventName === "selection-change") {
                if (args[0] !== null && args[0] !== undefined) {
                    that.lastSelection = args[0];
                    //console.log(that.lastSelection);
                }
                if (args[1] !== "silent") {
                    if (args[0] === null) {
                        //this is a blur event.  Store old selection so that focus can restore selection.
                        //this.lastSelection = args[1];
                    //} else if (args[1] === null && args[2] !== "user") {
                    //    //this is probably a focus event that was made from setting formatting... restore last selection if not null
                    //    if (this.lastSelection !== null) {
                    //        quill.
                    //    }
                    } else {
                        richTextEditorRef.invokeMethodAsync("SelectionChangedAsync", that.getFormat(currentId));
                    }
                }
            }
        });

        this.allInstances[currentId] = quill;
        return currentId;
    },

    unregister: function(id) {
        let quill = this.allInstances[id];
        delete this.allInstances[id];
    },
    preventZoomEnable: function (enable) {
        if (!this.zoomPrevented && enable) {
            this.zoomPrevented = true;
            document.onkeydown = preventZoom;
        }
        else if (this.zoomPrevented && !enable) {
            this.zoomPrevented = false;
            document.onkeydown = null;
        }
    },
    setReadonly: function (id, setReadonly) {
        let quill = this.allInstances[id];
        if (setReadonly)
            quill.disable();
        else
            quill.enable();
    },

    setHtmlContent: function (id, contents) {
        let quill = this.allInstances[id];
        var sel = quill.getSelection();
        if (quill.root.innerHTML !== contents) {
            quill.root.innerHTML = contents;
            this.setEditorSelection(quill, sel);
        }
    },
    setEditorSelection: function (editor, range) {
        if (range) {
            // Validate bounds before applying.
            var length = editor.getLength();
            range.index = Math.max(0, Math.min(range.index, length - 1));
            range.length = Math.max(0, Math.min(range.length, (length - 1) - range.index));
        }
        editor.setSelection(range);
    },
    getHtmlContent: function (id) {
        let quill = this.allInstances[id];
        return quill.root.innerHTML;
    },

    setFormat: function(id, formatString, turnOn=true) {
        let quill = this.allInstances[id];
        if (quill === null)
            return null;
        if (formatString === "superscript") {
            quill.format("script", turnOn ? "super" : "");
        }
        else if (formatString === "subscript")
            quill.format("script", turnOn ? "sub" : "");
        else
            quill.format(formatString, turnOn);
    },

    getFormat: function(id) {
        let quill = this.allInstances[id];
        if (quill === null)
            return null;
        try {
            var format = quill.getFormat();
            if (format.script !== undefined) {
                if (format.script === "super")
                    format.superscript = true;
                else if (format.script === "sub")
                    format.subscript = true;
            }
            return format;
        } catch(ex) {
            return null;
        }
    },
    insertImage: function (id, imageUrl, imageAlt, imageWidth, imageHeight) {
        let quill = this.allInstances[id];
        //var sel = quill.getSelection();
        quill.insertEmbed(this.lastSelection ? this.lastSelection.index : 0, "image", { src: imageUrl, alt: imageAlt, width: imageWidth !== null ? imageWidth : undefined, height: imageHeight !== null ? imageHeight : undefined });
    }

};

