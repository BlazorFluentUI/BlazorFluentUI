﻿@page "/richTextEditorPage";

<h1>RichTextEditor</h1>

<BFUToggle Label="Readonly" @bind-Checked=@isReadonly />
<BFURichTextEditor @bind-RichText=@htmlContents
                ReadOnly=@isReadonly.GetValueOrDefault() />

@htmlContents

@code {
    bool? isReadonly = false;
    string htmlContents = "";
}
