# BlazorFabric
Simple port of Office Fabric React components and style to Blazor

## Demo
https://blazorfabric.azurewebsites.net/

## Release Notes
- v1.1.0-preview9 
    - Breaking changes: Changed all namespaces to `BlazorFabric`.  No more sub-namespaces using the control's name.

## Status - all updated to preview9 (That was a big one!)
- Label -done
- DefaultButton, PrimaryButton, ActionButton(CommandButton) -working, but incomplete
- Checkbox -done, except for icons
- List -supports `INotifyCollectionChanged`, but bad server-side experience
- TextField -done, except for icons
- Icon, only MS icons 
- Nav -done!
- ContextualMenu -in-progress
- Callout (part of ContextMenu) -working, not positioning perfectly
- Layer (part of Callout) -done?  only layers at root window right now.
- Dropdown -done? working well 
- Modal -done!, no modeless version


## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project.  NuGet packages planned...

## To use
1. Install NuGet package for the control you want.  _BlazorFabric.*_
2. If you are using Server-side Blazor, you'll need to add all javascript and CSS assets from the component packages manually.  Yes, it's tedious, but you can mostly just copy the links in the demo for server-side here:  https://github.com/limefrogyank/BlazorFabric/blob/master/Test/ServerSide/Pages/_Host.cshtml
(Keep in mind List virtualization is not a good experience on server-side yet due to excessive scroll event calls.)
3. Optionally, add Microsoft's assets package to your index.html or \_Hosts.cshtml file.

`<link rel="stylesheet" href="https://static2.sharepointonline.com/files/fabric/office-ui-fabric-core/10.0.0/css/fabric.min.css" />`

(Remember that the assets package has a more restrictive license.  You are required to use it with/for some type of Microsoft product.  However, one of their engineers said that using it hosted on Azure would be enough... but I'm not a lawyer, so use caution.)

4. If you're using any component that requires a `Layer` as part of its inner-workings (i.e. `Modal`, `Callout`, etc... anything that pops up over already drawn stuff), you need to wrap the `Router` with a `LayerHost`.
```
<BlazorFabric.Layer.LayerHost Style="display:flex; flex-direction: row;width:100vw">
    <Router AppAssembly="typeof(Startup).Assembly" />
</BlazorFabric.Layer.LayerHost>
```

