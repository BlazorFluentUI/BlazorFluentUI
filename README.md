# BlazorFluentUI
Simple port of Fluent UI (formerly Office Fabric) React components and style to Blazor

## ClientSide Demo (web assembly)
https://www.blazorfluentui.net/

## ServerSide Demo (SignalR)
https://blazorfluentui.azurewebsites.net/

## Telegram dev channel
https://t.me/blazorfabric

## See the wiki for all usage notes
[Home](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki) \
[Installation](https://github.com/limefrogyank/BlazorFabric/wiki/Installation) \
How To Use: [ClientSide Blazor](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/How-To-Use:-ClientSide-Blazor) \
How To Use: [ServerSide Blazor](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/How-To-Use:-ServerSide-Blazor) \
[Theming](https://github.com/limefrogyank/BlazorFabric/wiki/Theming---defaults-and-custom) \
[Preloading Styles](https://github.com/limefrogyank/BlazorFabric/wiki/Preloading-Styles)

## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project. 

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
