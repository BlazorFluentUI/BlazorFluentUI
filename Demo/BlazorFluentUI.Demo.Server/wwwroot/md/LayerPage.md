@page "/layerPage"

<header class="root">
    <h1 class="title">Layer</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A Layer is a technical component that does not have specific Design guidance.
            </p>
            <p>
                Layers are used to render content outside of a DOM tree, at the end of the document. This allows content to escape traditional boundaries caused by "overflow: hidden" css rules and keeps it on the top without using z-index rules. This is useful for example in ContextualMenu and Tooltip scenarios, where the content should always overlay everything else.
            </p>
            <p>
                There are some special considerations. Due to the nature of rendering content elsewhere asynchronously, React refs within content will not be resolvable synchronously at the time the Layer is mounted. Therefore, to use refs correctly, use functional refs <code class="root-363">ref={ (el) =&gt; { this._root = el; }</code> rather than string refs <code class="root-363">ref='root'</code>. Additionally measuring the physical Layer element will not include any of the children, since it won't render it. Events that propgate from within the content will not go through the Layer element as well.
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
            <h3>This is not in a layer</h3>

            This is a layer host.
            <LayerHost Id="pageHost"
                       style="height:250px;width:400px;background-color:lightyellow;border:1px solid black;position:relative;overflow:hidden;">

            </LayerHost>

            <Toggle @bind-Checked=@trapPanel Label="Trap Panel in LayerHost" />
            <PrimaryButton Text="Open Panel"
                           OnClick=@(()=>
                            {
                                panelIsOpen = true;
                            }) />


            <Panel IsLightDismiss="true"
                   HostId=@(trapPanel.GetValueOrDefault() ? "pageHost" : null)
                   IsOpen=@panelIsOpen
                   OnDismiss=@(() => panelIsOpen = false)>
                <p>
                    Hey, there's some content in here.
                </p>
            </Panel>

            <Layer>
                <div style="position:absolute;left:50%;top:80%;width:200px;height:200px;background-color:var(--semanticColors-BodyBackgroundHovered);">
                    This <b>is</b> in a layer
                </div>
            </Layer>


        </div>
    </div>
</div>
@code{
    //ToDo: Add Demo sections
    [CascadingParameter(Name = "Theme")]
    public ITheme Theme { get; set; }

    bool panelIsOpen = false;
    bool? trapPanel = false;
}
