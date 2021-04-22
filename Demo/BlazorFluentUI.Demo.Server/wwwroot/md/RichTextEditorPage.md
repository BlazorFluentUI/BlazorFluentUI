@page "/richTextEditorPage";

<header class="root">
    <h1 class="title">RichTextEditor</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>

            </p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <div class="subSection">
            <Demo Header="RichText Editor" Key="0" MetadataPath="RichTextEditorPage">
                <Toggle Label="Readonly" @bind-Checked=@isReadonly />
                <RichTextEditor @bind-RichText=@htmlContents
                                ReadOnly=@isReadonly />

                @htmlContents
            </Demo>
        </div>
    </div>
</div>
@code {
    bool isReadonly = false;
    string htmlContents = "";
}
