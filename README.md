# BlazorFabric
Simple port of Office Fabric React components and style to Blazor

## Demo
https://blazorfabric.azurewebsites.net/

## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project.  NuGet packages planned...

## To use
1. Install NuGet package for the control you want.  
2. Optionally, add Microsoft's assets package to your index.html or \_Hosts.cshtml file.  
`<link rel="stylesheet" href="https://static2.sharepointonline.com/files/fabric/office-ui-fabric-core/10.0.0/css/fabric.min.css" />`
(Remember that the assets package has a more restrictive license.  You are required to use it with some type of Microsoft products.  However, one of their engineers said that using it hosted on Azure would be enough... but I'm not a lawyer, so use caution.)
