/// <reference path="baseComponent.d.ts" />
// /// <reference path="quill.d.ts" />

////import Quill from "./quill";
//let Inline = Quill.import('blots/inline');

//class BoldBlot extends Inline { }
//BoldBlot.blotName = 'bold';
//BoldBlot.tagName = 'strong';

//class ItalicBlot extends Inline { }
//ItalicBlot.blotName = 'italic';
//ItalicBlot.tagName = 'em';

//class UnderlineBlot extends Inline {
//    static create(url) {
//        let node = super.create();
//        node.setAttribute('style', "text-decoration:underline;");
//        return node;
//    }
//}
//UnderlineBlot.blotName = 'underline';
//UnderlineBlot.tagName = 'span';

//Quill.register(BoldBlot);
//Quill.register(ItalicBlot);
//Quill.register(UnderlineBlot);

function preventZoom(event) {
    if (event.ctrlKey === true && (event.which === 61 || event.which === 107 || event.which === 173 || event.which === 109 || event.which === 187 || event.which === 189)) {
        event.preventDefault();
    }
}

var BlazorFabricRichTextEditor = {

    //interface Map<T> {
    //    [K: number]: T;
    //}
    //interface DotNetReferenceType {

    //    invokeMethod<T>(methodIdentifier: string, ...args: any[]): T;
    //    invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
    //}

    count: 0,
    allInstances: {},
    zoomPrevented:false,

    register: function(editorWindow, richTextEditorRef) {
        let currentId = this.count++;
        let quill = new Quill(editorWindow);
        quill.on('text-change', function (delta, oldDelta, source) {
            richTextEditorRef.invokeMethodAsync("TextChangedAsync", { html: quill.root.innerHTML, source: source });
        });

        this.allInstances[currentId] = quill;
        return currentId;
    },

    unregister: function(id) {
        let quill = this.allInstances[id];
        //if (richTextEditor) {
        //    richTextEditor.unRegister();
        //}
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
        var format = quill.getFormat();
        if (format.script !== undefined) {
            if (format.script === "super")
                format.superscript = true;
            else if (format.script === "sub")
                format.subscript = true;
        }
        return format;
    }

};

