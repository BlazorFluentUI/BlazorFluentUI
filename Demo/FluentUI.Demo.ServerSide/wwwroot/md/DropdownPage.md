@page "/dropdownPage"
@using System.ComponentModel.DataAnnotations

<h1>Dropdown</h1>

<Demo Header="Uncontrolled Single Selection" Key="0" MetadataPath="DropdownPage">
    <Dropdown ItemsSource=@items
                 Placeholder="Select an option"
                 OnChange=@UncontrolledSingleChangeHandler
                 Label=@($"Selected: {uncontrolledSingleSelectionResult?.Text}")
                 Style="width:300px;" />
</Demo>

<Demo Header="Uncontrolled Multi-Selection" Key="1" MetadataPath="DropdownPage">
    <FluentUI.Label>Selected: @string.Join(", ", uncontrolledMultiSelectionResult.Select(x=>x.Text))</FluentUI.Label>
    <Dropdown ItemsSource=@items
                 MultiSelect="true"
                 Placeholder="Select options..."
                 OnChange=@UncontrolledMultiChangeHandler
                 Style="width:300px;" />
</Demo>

<Demo Header="Controlled Single Selection" Key="2" MetadataPath="DropdownPage">
    <FluentUI.Label>Selected: @(controlledSingleSelectionResult?.Text)</FluentUI.Label>
    <Dropdown ItemsSource=@items 
                 Placeholder="Select an option" 
                 @bind-SelectedOption=@controlledSingleSelectionResult 
                 Style="width:300px;" />
</Demo>

<Demo Header="Controlled Multi-Selection" Key="3" MetadataPath="DropdownPage">
    <FluentUI.Label>Selected: @string.Join(", ", controlledMultiSelectionResult.Select(x=>x.Text))</FluentUI.Label>
    <Dropdown ItemsSource=@items 
                 MultiSelect="true" 
                 Placeholder="Select options..." 
                 @bind-SelectedOptions=@controlledMultiSelectionResult 
                 Style="width:300px;" />
</Demo>

<Demo Header="Disabled" Key="4" MetadataPath="DropdownPage">
    <Dropdown ItemsSource=@items 
                 Disabled="true" 
                 Placeholder="Select an option"
                 Style="width:300px;" />
</Demo>

<Demo Header="Disabled with Selected" Key="5" MetadataPath="DropdownPage">
    <Dropdown ItemsSource=@items
                 Disabled="true"
                 Placeholder="Select an option"
                 @bind-SelectedOption=@controlledSingleSelectionResult
                 Style="width:300px;" />
</Demo>

<Demo Header="Dropdown using Blazor Forms Validation" Key="6" MetadataPath="DropdownPage">
    <EditForm Model="exampleModel" OnValidSubmit=@HandleValidSubmit>
        <DataAnnotationsValidator />
        <FluentUIValidationSummary />
        <Dropdown ItemsSource=@items
                     Placeholder="Select an option"
                     @bind-SelectedOption=@(exampleModel.SelectionResult)
                     Style="width:300px;" />
        <SubmitButton Text="Submit" />
    </EditForm>
</Demo>

@code {

    IDropdownOption uncontrolledSingleSelectionResult;
    IEnumerable<IDropdownOption> uncontrolledMultiSelectionResult = new List<IDropdownOption>();

    IDropdownOption controlledSingleSelectionResult;
    IEnumerable<IDropdownOption> controlledMultiSelectionResult;

    List<IDropdownOption> items;

    protected override Task OnInitializedAsync()
    {
        items = new List<DataItem>
{
            new DataItem("Fruits", SelectableOptionMenuItemType.Header),
            new DataItem("Apple"),
            new DataItem("Banana"),
            new DataItem("Orange"),
            new DataItem("Grape"),
            new DataItem("divider1", SelectableOptionMenuItemType.Divider),
            new DataItem("Vegetables", SelectableOptionMenuItemType.Header),
            new DataItem("Broccoli"),
            new DataItem("Carrot"),
            new DataItem("Lettuce")
        }.Select(x => new DropdownOption { Key = x.Key, Text = x.DisplayName, ItemType = x.Type }).Cast<IDropdownOption>().ToList();

        controlledSingleSelectionResult = items.First(x => x.Key == "Banana");

        controlledMultiSelectionResult = items.Where(x => x.Key == "Banana" || x.Key == "Orange");

        return base.OnInitializedAsync();
    }

    void UncontrolledSingleChangeHandler(DropdownChangeArgs args)
    {
        uncontrolledSingleSelectionResult = args.Option;
    }

    void UncontrolledMultiChangeHandler(DropdownChangeArgs args)
    {
        if (args.IsAdded)
            uncontrolledMultiSelectionResult = uncontrolledMultiSelectionResult.Append(args.Option);
        else
            uncontrolledMultiSelectionResult = uncontrolledMultiSelectionResult.Where(x=> x != args.Option);
    }


    ExampleModel exampleModel = new ExampleModel();

    class ExampleModel
    {
        [Required]
        public IDropdownOption SelectionResult { get; set; }
    }

    public void HandleValidSubmit()
    {
        var i = 3;
    }

}
