# BlazorFluentUI
Simple port of Fluent UI (formerly Office Fabric) React components and style to Blazor

## ClientSide Demo (web assembly)
https://white-dune-0ef5c1e03.azurestaticapps.net/

## ServerSide Demo (SignalR)
https://blazorfluentui.azurewebsites.net/

## Telegram dev channel
https://t.me/joinchat/TuHGR8AZmYe1SKlH

## Dev Nuget Packages
On each commit new dev nuget builds will be created automatically. To access those, add a new Package Source to Visual Studio or your project with the following URL:

```
https://pkgs.dev.azure.com/blazor-fluentui/5355f597-190a-4424-83da-cd89cd362a24/_packaging/DevChannel/nuget/v3/index.json
```
For a more detailed tutorial, head over to our [Public Feed](https://dev.azure.com/blazor-fluentui/Blazor%20FluentUI/_packaging?_a=feed&feed=DevChannel), click on "Connect To Feed" and select the platform. 

## Breaking Changes for v5.0
#### Renaming stuff and demo revamp
All components have dropped the `BFU` prefix and names are now on par with their Fluent UI React counterparts (or will be soon). Change was made to not litter the codebase with the term `BFU` everywhere.

The demo sites have been given some attention and look a lot better now (more in line with Fluent UI Rect docs).

## Breaking Changes for v4.0 (net5)

#### List, DetailsList, GroupedList
You will now have to define your list's container separately from the list component.  Pick a `<div>` or other container element and apply the `data-is-scrollable` to it.  You'll also have to style it appropriately for your page.  (For example, add `overflow-y:auto;height:100%;` or something similar.)  You do **not** have to place your list component as a *direct* descendant of this container.  If you fail to place the `data-is-scrollable` tag, then the component will traverse the nodetree to find the first container element that has `overflow-y:auto;` set.

You also have to define `GetKey` for your list component.  This is a selector for your items to tell the component how to uniquely identify each item.  

A big change is that `OnItemInvoked` will only work when `SelectionMode` is set to `None`.   If you need Selection active and the invoke item function to work at the same time, you can attach an event handler to the `Selection.OnSelelectionChanged` event or subscribe to the `SelectionChanged` observable.  

#### Removed BFUGlobalCS, BFUStylePreloader, IHasPreloadableGlobalStyle
These components and interfaces must be removed as they no longer function within this library. Switch to using CSS isolation with your razor components instead.  `BFULocalCS` is still present and encouraged for dynamic styling that requires flexibility.  (Using the style attribute on an element makes it difficult for users to override the style.)

If you need to reference the theme from a css file, you can reference the global css variable instead.  For example, where before you would get a white color in C# as `Theme.Palette.White`, now you will write in css, `var(--palette-White)`.  Css global variables always start with two dashes and a lowercase name.  Instead of a dot, use a dash followed by capitalized names.

## See the wiki for all usage notes
[Home](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki) \
[Installation](https://github.com/limefrogyank/BlazorFabric/wiki/Installation) \
How To Use: [ClientSide Blazor](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/How-To-Use:-ClientSide-Blazor) \
How To Use: [ServerSide Blazor](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/How-To-Use:-ServerSide-Blazor) \
[Theming](https://github.com/limefrogyank/BlazorFabric/wiki/Theming---defaults-and-custom) \

## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project. 

## Status of Controls

### Basic Inputs
| Control     | State | Information      |
| :---------- | :---: | :--------------- |
| Button      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Checkbox    | ![#dff6dd](https://via.placeholder.com/15/dff6dd/000000?text=+) Done  | except for icons |
| ChoiceGroup | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| ComboBox    | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| Dropdown    | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Label       | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Link        | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| Rating      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
| SearchBox   | ![#a80000](https://via.placeholder.com/15/a80000/000000?text=+) ToDo  |                  |
| Slider      | ![#107c10](https://via.placeholder.com/15/107c10/000000?text=+) Done  |                  |
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
| DocumentCard | ![#fff4ce](https://via.placeholder.com/15/fff4ce/000000?text=+) In Progress  | Missing focus styles and ShouldTruncate doesn't work yet.                                    |
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
