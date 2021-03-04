@page "/dropdownPage"
@using System.ComponentModel.DataAnnotations

<h1>Dropdown</h1>

<Demo Header="Uncontrolled Single Selection" Key="0" MetadataPath="DropdownPage">
    <BFUDropdown ItemsSource=@items
                 Placeholder="Select an option"
                 OnChange=@UncontrolledSingleChangeHandler
                 Label=@($"Selected: {uncontrolledSingleSelectionResult?.Text}")
                 Style="width:300px;" />
</Demo>

<Demo Header="Uncontrolled Multi-Selection" Key="1" MetadataPath="DropdownPage">
    <BlazorFluentUI.BFULabel>Selected: @string.Join(", ", uncontrolledMultiSelectionResult.Select(x=>x.Text))</BlazorFluentUI.BFULabel>
    <BFUDropdown ItemsSource=@items
                 MultiSelect="true"
                 Placeholder="Select options..."
                 OnChange=@UncontrolledMultiChangeHandler
                 Style="width:300px;" />
</Demo>

<Demo Header="Controlled Single Selection" Key="2" MetadataPath="DropdownPage">
    <BlazorFluentUI.BFULabel>Selected: @(controlledSingleSelectionResult?.Text)</BlazorFluentUI.BFULabel>
    <BFUDropdown ItemsSource=@items 
                 Placeholder="Select an option" 
                 @bind-SelectedOption=@controlledSingleSelectionResult 
                 Style="width:300px;" />
</Demo>

<Demo Header="Controlled Multi-Selection" Key="3" MetadataPath="DropdownPage">
    <BlazorFluentUI.BFULabel>Selected: @string.Join(", ", controlledMultiSelectionResult.Select(x=>x.Text))</BlazorFluentUI.BFULabel>
    <BFUDropdown ItemsSource=@items 
                 MultiSelect="true" 
                 Placeholder="Select options..." 
                 @bind-SelectedOptions=@controlledMultiSelectionResult 
                 Style="width:300px;" />
</Demo>

<Demo Header="Disabled" Key="4" MetadataPath="DropdownPage">
    <BFUDropdown ItemsSource=@items 
                 Disabled="true" 
                 Placeholder="Select an option"
                 Style="width:300px;" />
</Demo>

<Demo Header="Disabled with Selected" Key="5" MetadataPath="DropdownPage">
    <BFUDropdown ItemsSource=@items
                 Disabled="true"
                 Placeholder="Select an option"
                 @bind-SelectedOption=@controlledSingleSelectionResult
                 Style="width:300px;" />
</Demo>

<Demo Header="Dropdown using Blazor Forms Validation" Key="6" MetadataPath="DropdownPage">
    <EditForm Model="exampleModel" OnValidSubmit=@HandleValidSubmit>
        <DataAnnotationsValidator />
        <BFUValidationSummary />
        <BFUDropdown ItemsSource=@items
                     Placeholder="Select an option"
                     @bind-SelectedOption=@(exampleModel.SelectionResult)
                     Style="width:300px;" />
        <BFUSubmitButton Text="Submit" />
    </EditForm>
</Demo>

@code {

    IBFUDropdownOption uncontrolledSingleSelectionResult;
    IEnumerable<IBFUDropdownOption> uncontrolledMultiSelectionResult = new List<IBFUDropdownOption>();

    IBFUDropdownOption controlledSingleSelectionResult;
    IEnumerable<IBFUDropdownOption> controlledMultiSelectionResult;

    List<IBFUDropdownOption> items;

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
        }.Select(x => new BFUDropdownOption { Key = x.Key, Text = x.DisplayName, ItemType = x.Type }).Cast<IBFUDropdownOption>().ToList();

        controlledSingleSelectionResult = items.First(x => x.Key == "Banana");

        controlledMultiSelectionResult = items.Where(x => x.Key == "Banana" || x.Key == "Orange");

        return base.OnInitializedAsync();
    }

    void UncontrolledSingleChangeHandler(BFUDropdownChangeArgs args)
    {
        uncontrolledSingleSelectionResult = args.Option;
    }

    void UncontrolledMultiChangeHandler(BFUDropdownChangeArgs args)
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
        public IBFUDropdownOption SelectionResult { get; set; }
    }

    public void HandleValidSubmit()
    {
        var i = 3;
    }

}
