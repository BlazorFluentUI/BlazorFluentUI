# BlazorFluentUI
Port of Fluent UI React components and style (formerly Office Fabric) to Blazor

## Blazor Webassembly Demo
https://www.blazorfluentui.net/

## Blazor Server Demo
https://blazorfluentui.azurewebsites.net/ (Older version!)

## How to use the library
[Installation](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/Installation) \
How To Use: [Blazor Webassembly](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/How-To-Use:-Blazor-WebAssembly) \
How To Use: [Blazor Server](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/How-To-Use:-Blazor-Server) \
[Theming](https://github.com/BlazorFluentUI/BlazorFluentUI/wiki/Theming---defaults-and-custom) 

## Revision history
### V5.5
- Added parameter `DropdownHeight`to limit the height of the dropdownlist. See first sample on https://www.blazorfluentui.net/dropdownPage for usage (also showing DropdownWidth)

### V5.4
- DetailsList and DetailsListAuto speed vastly improved
- Callout no longer tracking references in script. No more double clicking needed for opening Dropdown, etc.

### V5.3
- `TextFieldNumber` is renamed to `NumericTextField`
- `MaskedTextFiled` is now available as a **BETA** component. See https://www.blazorfluentui.net/MaskedTextFieldPage for examples

### V5.2
- Library is now using JavaScript isolation (see https://docs.microsoft.com/en-us/aspnet/core/blazor/call-javascript-from-dotnet?view=aspnetcore-5.0#blazor-javascript-isolation-and-object-references). No longer needed to reference the scripts in your `_Host.cshtml` / `index.html`! (with exception of the still experimental RichTextEditor script and acompanying Quill library). Blazor Server in .NET 5.0 no longer supports IE11/Legacy edge so neither does the demo.

### V5.1
- Add editable column for `DetailsList, DetailsListAuto` 
- Add new component `TextFieldNumber` (will be renamed to NumberTextField soon), based on `TextField`. Supports `int, long, short, float, double and decimal`. 
  See https://www.blazorfluentui.net/TextFieldNumberPage for examples


## Breaking Changes for major releases
### V5.0: Renaming and demo revamp
We dropped the `BFU` prefix from all the component- and project names. Also, all components are now bundled in only two packages (to make a distinction between components dependant on DynamicData package an those who are not).

In cases where renaming would lead to collisions with already existing Blazor components or .NET classes, we placed those components in seperate namespace (which you need to include explicitly) and/or you need to use the full name of the component (so including the namespace) in your .razor files.

Both changes were made to align the library closer to the Fluent UI React environment. Also the codebase is not so cluttered with 'BFU' anymore.

The demo sites have been given a lot of attention and look a lot better now (more in line with Fluent UI Rect docs).


### Breaking Changes for v4.0 
Library is now running on .NET 5  

#### List, DetailsList, GroupedList
You will now have to define your list's container separately from the list component.  Pick a `<div>` or other container element and apply the `data-is-scrollable` to it.  You'll also have to style it appropriately for your page.  (For example, add `overflow-y:auto;height:100%;` or something similar.)  You do **not** have to place your list component as a *direct* descendant of this container.  If you fail to place the `data-is-scrollable` tag, then the component will traverse the nodetree to find the first container element that has `overflow-y:auto;` set.

You also have to define `GetKey` for your list component.  This is a selector for your items to tell the component how to uniquely identify each item.  

A big change is that `OnItemInvoked` will only work when `SelectionMode` is set to `None`.   If you need Selection active and the invoke item function to work at the same time, you can attach an event handler to the `Selection.OnSelelectionChanged` event or subscribe to the `SelectionChanged` observable.  

#### Removed BFUGlobalCS, BFUStylePreloader, IHasPreloadableGlobalStyle
These components and interfaces must be removed as they no longer function within this library. Switch to using CSS isolation with your razor components instead.  `BFULocalCS` is still present and encouraged for dynamic styling that requires flexibility.  (Using the style attribute on an element makes it difficult for users to override the style.)

If you need to reference the theme from a css file, you can reference the global css variable instead.  For example, where before you would get a white color in C# as `Theme.Palette.White`, now you will write in css, `var(--palette-White)`.  Css global variables always start with two dashes and a lowercase name.  Instead of a dot, use a dash followed by capitalized names.


## Info
There are no MergeStyles in this port.  It's just each control packaged into its own project so you can limit what gets added to your Blazor project. 

## Dev Nuget Packages
On each commit new dev NuGet packages will be created automatically. To access those, add a new Package Source to Visual Studio or your project with the following URL:

```
https://pkgs.dev.azure.com/blazor-fluentui/5355f597-190a-4424-83da-cd89cd362a24/_packaging/DevChannelV5/nuget/v3/index.json
```
For a more detailed tutorial, head over to our [Public Feed](https://dev.azure.com/blazor-fluentui/Blazor%20FluentUI/_packaging?_a=feed&feed=DevChannelV5), click on "Connect To Feed" and select the platform. 

## Telegram dev channel
https://t.me/joinchat/TuHGR8AZmYe1SKlH

