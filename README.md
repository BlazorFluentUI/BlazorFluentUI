# BlazorFabric
Simple port of Office Fabric React components and style to Blazor

## Client-side Demo (web assembly)
https://limefrogyank.github.io/BlazorFabric/

## Server-side Demo (SignalR)

https://blazorfabric.azurewebsites.net/ 

## Telegram dev channel
https://t.me/blazorfabric

## Release Notes
- v2.2.0 (Jan 25, 2020)
	- Added `Pivot`
	- Fix #78 positioning problems with `Callout`
	- Heavy focus on keyboard-navigation.  Fixes to `ContextualMenu`, `Dropdown`
	- Refactored `ResponsiveLayout` to use media queries in style tags for immediate effect... BREAKING CHANGES HERE.
	- Added `ResponsiveWrapper` and `ResponsiveCascader` (not Fabric controls) that work using javascript.  Changes applied after 1st render.
	- Minor changes to button css files.  Only need to include `button.css` to have them all load automatically.
	- Many, many bug fixes.
- v2.1.0
	- Added `Text` (including custom styling)
	- Upgraded `TextField` with PrefixContent and SuffixContent for non-text-only content
	- Added non-Fabric `RichTextEditor` that uses Fabric controls (more functions TBA)
	- Added `Dialog`
	- Added non-Fabric `ComponentStyle` that uses unique css-class names for styles special for components and .Net-Classes to create css-classes to be more flexible in component-styling
- v2.0.0
	- Upgraded entire project to aspnetcore 3.1 (enables partial classes, stopProgation, preventDefault, and others)
	- Upgraded demos to aspnetcore 3.1 and netstandard 2.1
	- Added `Calendar`
	- Added `DatePicker`
- v1.3.6
	- Custom NavigationTemplate close button inside `Panel` would not show the close animation, now it does.
	- Fixed [#33](https://github.com/limefrogyank/BlazorFabric/issues/33)
	- Fixed `Callout` positioning bug [#2](https://github.com/limefrogyank/BlazorFabric/issues/2)
	- Added single string key binding to `Dropdown`
	- Added `ProgressIndicator`
	- Added `MessageBar`
	- Fixed [#50](https://github.com/limefrogyank/BlazorFabric/issues/50)
	- Added `Link`
	- Added `FocusZone`
	- Enhanced `Rating` with `FocusZone`
- v1.3.5
	- Added `Rating` (contributed by Eweol)
	- Added autocomplete to `TextField` + style fixes include description field
	- Added validation to `TextField` (contributed by Eweol) 
	- Added `Keytip` placeholder... doesn't do anything yet.  But required internally for `Toggle`
- v1.3.3
	- Added `Toggle`
	- Added password functionality to `TextField` (contributed by Eweol)
	- Added `ResponsiveLayout` <- *not a Fabric control, but made so you don't have to use CSS media queries!* 
	- Changed the demo's layout to use `ResponsiveLayout` and `Stack`, slowly removing CSS!
- v1.3.2
	- Added `Spinner`
	- Added `Persona`
	- Added `Image`
	- Added `Tooltip`
	- Added `Stack` <- *awesome abstraction of flexbox... the Fabric guys don't give this one enough credit*
- v1.2.6 *(aspnetcore-3.0 out of preview!)*
	- Fixed `Overlay` so that it blocks body scrolling
	- Added `Panel`
	- Added `FocusTrapZone` (and added it to `Modal` to make focus interactions work better)
	- disregard previous fix!  Current guidelines are that devs must manually add js and css files to your html files.  See the sample for a copy/paste opportunity.
- v1.2.2-preview9
	- Fixed new components so they would automatically add their css/js to your index.html in client-side blazor
	  (This is temporary.  You're supposed to add them in manually anyways.  This will stop working in a future version of Blazor)
- v1.2.1-preview9
	- Fixed wrong thread problem with `ContextualMenuItem` (using a timer)
- v1.2.0-preview9
	- Breaking changes: `ContextualMenu` attached to all buttons requires a new way to create menu items.  Use the built-in `ContextualMenuItem` class or create your own with the `IContextualMenuItem` interface.
	- Added: `CommandBar`, `ResizeGroup`, `OverflowSet` and fixed `ContextualMenu` click/dismiss problems.
- v1.1.1-preview9
	- Fixed Button contextmenu icon
- v1.1.0-preview9 
    - Breaking changes: Changed all namespaces to `BlazorFabric`.  No more sub-namespaces using the control's name.

## Status of Controls

### Basic Inputs
| Control     | State | Information      |
| :---------- | :---: | :--------------- |
| Button      | Done  |                  |
| Checkbox    | Done  | except for icons |
| ChoiceGroup | ToDo  |                  |
| ComboBox    | ToDo  |                  |
| Dropdown    | Done  |                  |
| Label       | Done  |                  |
| Link        | Done  |                  |
| Rating      | Done  |                  |
| SearchBox   | ToDo  |                  |
| Slider      | ToDo  |                  |
| SpinButton  | ToDo  |                  |
| TextField   | Done  | except masking   |
| Toggle      | Done  |                  |

### Galleries & Pickers
| Control           | State | Information |
| :---------------- | :---: | :---------- |
| Calendar          | Done  |             |
| ColorPicker       | ToDo  |             |
| DatePicker        | Done  |             |
| PeoplePicker      | ToDo  |             |
| Pickers           | ToDo  |             |
| SwatchColorPicker | ToDo  |             |

### Items & Lists
| Control      | State | Information                         |
| :----------- | :---: | :---------------------------------- |
| ActivityItem | ToDo  |                                     |
| DetailsList  | ToDo  |                                     |
| DocumentCard | ToDo  |                                     |
| Facepile     | ToDo  |                                     |
| GroupedList  | ToDo  |                                     |
| HoverCard    | ToDo  |                                     |
| List         | Done  | supports `INotifyCollectionChanged` |
| Persona      | Done  |                                     |

### Commands, Menus & Navs
| Control        | State | Information |
| :------------- | :---: | :---------- |
| Breadcrumb     | ToDo  |             |
| CommandBar     | Done  |             |
| ContextualMenu | Done  |             |
| Nav            | Done  |             |
| OverflowSet    | Done  |             |
| Pivot          | Done  |             |

### Notification & Engagement
| Control        | State | Information |
| :------------- | :---: | :---------- |
| Coachmark      | ToDo  |             |
| MessageBar     | Done  |             |
| TeachingBubble | ToDo  |             |

### Progress
| Control           | State | Information |
| :---------------- | :---: | :---------- |
| ProgressIndicator | Done  |             |
| Shimmer           | ToDo  |             |
| Spinner           | Done  |             |

### Surfaces
| Control        |    State    | Information                                                                                         |
| :------------- | :---------: | :-------------------------------------------------------------------------------------------------- |
| Callout        |    Done     |                                                                                                     |
| Dialog         |    Done     | can't drag                                                                                          |
| Modal          |    Done     | no modeless version                                                                                 |
| Panel          |    Done     |                                                                                                     |
| ScrollablePane |    ToDo     |                                                                                                     |
| Tooltip        | In Progress | limited functionality.  will show, but can't interact with it yet, doesn't respond to overflow yet. |

### Utilities
| Control          |    State    | Information                           |
| :--------------- | :---------: | :------------------------------------ |
| Announced        |    ToDo     |                                       |
| FocusTrapZone    |    Done     |                                       |
| FocusZone        |    Done     |                                       |
| Icon             |    Done     | only MS icons                         |
| Image            |    Done     |                                       |
| Keytips          |    ToDo     |                                       |
| Layer            | In Progress | only layers at root window right now. |
| MarqueeSelection |    ToDo     |                                       |
| Overlay          |    ToDo     |                                       |
| ResizeGroup      |    Done     |                                       |
| Selection        |    ToDo     |                                       |
| Separator        |    ToDo     |                                       |
| Stack            |    Done     |                                       |
| Text             |    Done     |                                       |
| Themes           |    ToDo     |                                       |

### Non-Fabric-Component
| Control          |    State    | Information                           |
| :--------------- | :---------: | :------------------------------------ |
| ResponsiveLayout |    Done     |                                       |
| RichTextEditor   | In Progress | Works with limited styles available   |
| ComponentStyle   | In Progress |                                       |

## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project. 

## To use
1. Install NuGet package for the control you want.  _BlazorFabric.*_  (be sure to select preview packages)
2. The Blazor team has been inconsistent with how static files from component libraries are added to projects in the past.  Going forward, you'll need to **add all javascript and CSS assets from the component packages manually**.  You can just copy/paste the section from the test app's index.html.
You can also use my helper VSIX extension (may have major bugs!  Be sure to compile once for assets to show up.): https://marketplace.visualstudio.com/items?itemName=LeeMcPherson.BlazorLibraryAssetHelper&ssr=false#overview  
3. Optionally, add Microsoft's assets package to your index.html or \_Hosts.cshtml file.

`<link rel="stylesheet" href="https://static2.sharepointonline.com/files/fabric/office-ui-fabric-core/11.0.0/css/fabric.min.css" />`

**OR**

Use the tool here to generate your own MS font package: https://uifabricicons.azurewebsites.net/

(Remember that the assets package has a more restrictive license.  You are required to use it with/for some type of Microsoft product.  However, one of their engineers said that using it hosted on Azure would be enough... but I'm not a lawyer, so use caution.)

4. If you're using any component that requires a `Layer` as part of its inner-workings (i.e. `Modal`, `Callout`, etc... anything that pops up over already drawn stuff), you need to wrap the `Router` with a `LayerHost`.
```
<BlazorFabric.Layer.LayerHost Style="display:flex; flex-direction: row;width:100vw">
    <Router AppAssembly="typeof(Startup).Assembly" />
</BlazorFabric.Layer.LayerHost>
```

5. Add "AddBlazorFabric" to Startup.cs in Service-Configuration-Method

```
	services.AddBlazorFabric();
```
