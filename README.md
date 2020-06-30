# BlazorFluentUI
Simple port of Fluent UI (formerly Office Fabric) React components and style to Blazor

## Client-side Demo (web assembly)
https://www.blazorfluentui.net/

## Server-side Demo (SignalR)
https://blazorfluentui.azurewebsites.net/

## Telegram dev channel
https://t.me/blazorfabric

## See the wiki for all usage notes
https://github.com/BlazorFluentUI/BlazorFluentUI/wiki

## Status of Controls

### Basic Inputs
| Control     | State | Information      |
| :---------- | :---: | :--------------- |
| Button      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Checkbox    | ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done  | except for icons |
| ChoiceGroup | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| ComboBox    | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| Dropdown    | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Label       | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Link        | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Rating      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| SearchBox   | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| Slider      | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| SpinButton  | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| TextField   | ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done  | except masking   |
| Toggle      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |

### Galleries & Pickers
| Control           | State | Information |
| :---------------- | :---: | :---------- |
| Calendar          | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| ColorPicker       | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |
| DatePicker        | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| PeoplePicker      | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |
| Pickers           | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |
| SwatchColorPicker | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |

### Items & Lists
| Control      | State | Information                         |
| :----------- | :---: | :---------------------------------- |
| ActivityItem | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                                     |
| DetailsList  | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                                     |
| DocumentCard | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                                     |
| Facepile     | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                                     |
| GroupedList  | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                                     |
| HoverCard    | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                                     |
| List         | ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done  | supports `INotifyCollectionChanged` |
| Persona      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                                     |

### Commands, Menus & Navs
| Control        | State | Information |
| :------------- | :---: | :---------- |
| Breadcrumb     | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |
| CommandBar     | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| ContextualMenu | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| Nav            | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| OverflowSet    | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| Pivot          | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |

### Notification & Engagement
| Control        | State | Information |
| :------------- | :---: | :---------- |
| Coachmark      | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |
| MessageBar     | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| TeachingBubble | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |

### Progress
| Control           | State | Information |
| :---------------- | :---: | :---------- |
| ProgressIndicator | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |
| Shimmer           | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |             |
| Spinner           | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |             |

### Surfaces
| Control        |    State    | Information                                                                                         |
| :------------- | :---------: | :-------------------------------------------------------------------------------------------------- |
| Callout        |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                                                                                     |
| Dialog         |    ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done     | can't drag                                                                                          |
| Modal          |    ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done     | no modeless version                                                                                 |
| Panel          |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                                                                                     |
| ScrollablePane |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                                                                                     |
| Tooltip        | ![#fff4ce](https://via.placeholder.com/15/fff4ce/000000?text=+) In Progress | limited functionality.  will show, but can't interact with it yet, doesn't respond to overflow yet. |

### Utilities
| Control          |    State    | Information                           |
| :--------------- | :---------: | :------------------------------------ |
| Announced        |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |
| FocusTrapZone    |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| FocusZone        |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| Icon             |    ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done     | only MS icons                         |
| Image            |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| Keytips          |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |
| Layer            | ![#fff4ce](https://via.placeholder.com/15/fff4ce/000000?text=+) In Progress | only layers at root window right now. |
| MarqueeSelection |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |
| Overlay          |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |
| ResizeGroup      |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| Selection        |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |
| Separator        |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |
| Stack            |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| Text             |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| Themes           |    ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo     |                                       |

### Non-Fabric-Component
| Control          |    State    | Information                           |
| :--------------- | :---------: | :------------------------------------ |
| ResponsiveLayout |    ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done     |                                       |
| RichTextEditor   | ![#fff4ce](https://via.placeholder.com/15/fff4ce/000000?text=+) In Progress | Works with limited styles available   |
| GlobalCS		   | ![#fff4ce](https://via.placeholder.com/15/fff4ce/000000?text=+) In Progress |                                       |
| LocalCS		   | ![#fff4ce](https://via.placeholder.com/15/fff4ce/000000?text=+) In Progress |                                       |

## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project. 

## To use
1. Install NuGet package for the control you want.  _BlazorFluentUI.*_  
   (There is also a package that will install all packages at once:  *BlazorFluentUI.AllComponents*
2. Add the javascript to your index.html or index.cshtml file:
```
<script src="_content/BlazorFluentUI.BFUBaseComponent/blazorFluentUi.min.js"></script>
```
3. Optionally, add Microsoft's assets package to your index.html or \_Hosts.cshtml file.

`<link rel="stylesheet" href="https://static2.sharepointonline.com/files/fabric/office-ui-fabric-core/11.0.0/css/fabric.min.css" />`

**OR**

Use the tool here to generate your own MS font package: https://uifabricicons.azurewebsites.net/

(Remember that the assets package has a more restrictive license.  You are required to use it with/for some type of Microsoft product.  However, one of their engineers said that using it hosted on Azure would be enough... but I'm not a lawyer, so use caution.)

4. Add "AddBlazorFluentUI" to ServiceProvider

ServerSide: Startup.cs
```
public void ConfigureServices(IServiceCollection services)
{
	...;
	services.AddBlazorFluentUI();
	...;
}
```

ClientSide from version 'Blazor WebAssembly 3.2.0'  : Program.cs
```
public static async Task Main(string[] args)
{
	...;
	builder.Services.AddBlazorFluentUI();
	...;
}
```

5. Add css-class tag in

ServerSide: Pages/_Host.cshtml
```
<head>
	...;
	<component type="typeof(BFUGlobalRules)" render-mode="ServerPrerendered" />
	...;
</head>
```
ClientSide: wwwroot/index.html
```
<head>
	...;
	<style id="staticcs"></style>
	...;
</head>
```
You may need to add an `@using BlazorFluentUI` to the top of your `_Hosts.cshmtl` to avoid a 'GlobalRules could not be found` error.

6. Add following to get controll over css-classes tag you created before

ServerSide: issn't needed

ClientSide from version 'Blazor WebAssembly 3.2.0'  : Program.cs
```
public static async Task Main(string[] args)
{
	...;
	builder.RootComponents.Add<BFUGlobalRules>("#staticcs");
	...;
}
```
7. For simplicity, add a `@using BlazorFluentUI` to your `_Imports.razor` file.  If you don't, you'll have to add this using statement to every one of your pages.  Almost every component is prefixed with BFU*, so you won't find many name collisions.

8. For Theme's add following in `App.razor` as most outside Component. You can have a look in Demo application's `App.razor`

```
<BFUTheme>
	<...>
		<Router AppAssembly="typeof(Startup).Assembly" />
	</...>
</BFUTheme>
```
If you're creating your own theme and don't need to dynamically change it during runtime, you can set the `InitialPalette` and optionally, the `InitialSemanticColors` and `InitialSemanticTextColors` on `BFUTheme`.  

9. If you're using any component that requires a `Layer` as part of its inner-workings (i.e. `BFUModal`, `BFUCallout`, `BFUTooltip`, etc... anything that pops up over already drawn stuff), you need to wrap the `Router` with a `LayerHost`.
```
<BFULayerHost Style="display:flex; flex-direction: row;width:100vw">
    <Router AppAssembly="typeof(Startup).Assembly" />
</BFULayerHost>
```

10. For client-side (webassembly), until AOT compiling has been enabled in Blazor, it is suggested that you add `BFUStylePreloader` just after the `BFUTheme`.  This will scan the assemblies for components that implement `IHasPreloadableGlobalStyles` and automatically load them into the page style.  For server-side, it is not necessary. 
```
<BFUTheme>
	<BFUStylePreloader AppAssembly="typeof(MainLayout).Assembly" LoadAllComponents=true /> 
	...
```

