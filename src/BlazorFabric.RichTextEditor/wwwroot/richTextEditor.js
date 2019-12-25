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

    register: function(editorWindow, richTextEditorRef) {
        let currentId = this.count++;
        let quill = new Quill(editorWindow);

        this.allInstances[currentId] = quill;
        return currentId;
    },

    unregister: function(id) {
        let richTextEditor = this.allInstances[id];
        //if (richTextEditor) {
        //    richTextEditor.unRegister();
        //}
        delete this.allInstances[id];
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

