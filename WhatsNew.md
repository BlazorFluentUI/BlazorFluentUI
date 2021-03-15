# New for V5
Temporary demo: https://white-dune-0ef5c1e03.azurestaticapps.net/

## Renaming all the stuff
We renamed the root namespace to `BlazorFluentUI`.

We dropped the `BFU` prefix from all the component-and project names. In the cases where this would lead to collisions with already existing Blazor components or .NET classes, we prefixed with `FluentUI`

Both changes were made to align the library closer to the Fluent UI React environment. Also the codebase is not so cluttered with 'BFU' anymore.


# New for v4
Temporary demo: https://calm-rock-0f708be1e.azurestaticapps.net/


## Css Isolation and Speed
All components have been refactored to use css isolation for their global styles.  Styles should load as soon as the css file loads.  In version 3, styles would only load after the components had been created causing a slight delay while the styles loaded and the UI repainted itself.  If you are going to modify the styles of any of these components with your own isolated css, make sure you use the `::deep` selector.

## Blazor-based Forms Validation
BFU input components now work with `<EditForm>` and `<DataAnnotationsValidator>`.  The inputs that work with `<EditForm>` are:
- BFUTextField
- BFUCheckbox
- BFUDropdown
- BFUCalendar
- BFUDatePicker

You can see an example of how this works at the end of the each of their demo pages.  

In addition to those components, we have created a `BFUSubmitButton` whose only purpose is to disable itself in the UI when validation has failed.  As soon as validation has succeeded, the button will be enabled automatically.  The button is styled to use `BFUPrimaryButton` styling, but you can override that with a `ForceDefault` switch.

Finally, we have a modified ValidationSummary component called `BFUValidationSummary` that uses `BFUMessageBar` styling to show validation errors.  You can still use the original version and style the `<li>` elements however you like.

## BFUList, BFUDetailsList, & BFUGroupedList
These have been refactored to use a similar technique to the new blazor `Virtualize` component.  In addition, the method by which grouping was performed has been redone to increase performance dramatically.  Two breaking consequences of this change are a `GetKey` property requirement.  You need to define a key for each item in your list.  Second, you must define your own container for your list with appropriate styling and the attribute `data-is-scrollable`.

## BFUDetailsListAuto 
The original BFUDetailsList requires you to sort and filter your list data yourself for every change.  Setting a column's `IsSorted` property to `true` meant only that the filter icon would show.  

`BFUDetailstListAuto` is a new component that will do all of this for you **automatically**.  Provide the component with your items and define the columns as before.  However, when you click on a column header, the list will resort itself automatically without you having to modify your input list.  Filtering works similarly.  You will have to provide your own filter predicate:  `<Func<TProp,bool>` - return true if your property should be displayed, false if it should not be displayed.  

This works even for a *grouped* list.  `BFUDetailsListAuto` has a `GroupBy` property which is a `IList<Func<TItem, object>>`.  This is simply a list of grouping selectors.  You can provide just one to get a grouped list by whichever property you select, or provide more to have subgroups.  Filtering and sorting will work within the grouped lists, too.  Sorting groups themselves defaults to alphabetical ascending for now.

See the demo pages for samples!
