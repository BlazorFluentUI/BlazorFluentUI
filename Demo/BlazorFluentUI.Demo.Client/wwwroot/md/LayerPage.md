﻿@page "/layerPage"

<h1>Layer</h1>

<h3>This is not in a layer</h3>

This is a layer host.
<BFULayerHost Id="pageHost"
                style="height:250px;width:400px;background-color:lightyellow;border:1px solid black;position:relative;overflow:hidden;">

</BFULayerHost>

<BFUToggle @bind-Checked=@trapPanel Label="Trap Panel in LayerHost" />
<BFUPrimaryButton Text="Open Panel" 
                  OnClick=@(()=>
                            {
                                panelIsOpen = true;
                            }) />


    <BFUPanel IsLightDismiss="true"
              HostId=@(trapPanel.GetValueOrDefault() ? "pageHost" : null)
              IsOpen=@panelIsOpen
              OnDismiss=@(() => panelIsOpen = false)>
        <p>
            Hey, there's some content in here.
        </p>
    </BFUPanel>

<BFULayer>
    <div style="position:absolute;left:50%;top:80%;width:200px;height:200px;background-color:var(--semanticColors-BodyBackgroundHovered);">
        This <b>is</b> in a layer
    </div>
</BFULayer>


@code{
    [CascadingParameter(Name = "Theme")]
    public ITheme Theme { get; set; }

    bool panelIsOpen = false;
    bool? trapPanel = false;
}