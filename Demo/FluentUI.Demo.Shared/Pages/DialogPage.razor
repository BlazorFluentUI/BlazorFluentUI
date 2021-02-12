@page "/dialogPage"

<h1>Dialog</h1>

<Checkbox Label="Is Blocking Dialog" @bind-Checked=@isBlocking />
<DefaultButton Text="Open Dialog" OnClick=@(args=> dialogOpen=true) />
<Dialog Title="This is a dialog menu."
        SubText="This is the subtext area.  Below is the ChildContent area for components."
        IsOpen=@dialogOpen
        IsBlocking=@isBlocking
        OnDismiss=@(args=> dialogOpen=false )>
    <ChildContent>
        <p>
            <TextField Label="Sample TextField" />
        </p>
        <p>
            <Dropdown ItemsSource=@items.Select(x=>new DropdownOption { Key=x.DisplayName, Text=x.DisplayName}) 
                         Placeholder="Select an option" 
                         OnChange=@UncontrolledSingleChangeHandler />
        </p>
    </ChildContent>
    <FooterTemplate>
        <DefaultButton Text="Cancel" OnClick=@(args=> dialogOpen=false) />
        <PrimaryButton Text="OK" OnClick=@(args=> dialogOpen=false) />
    </FooterTemplate>
</Dialog>

<DefaultButton Text="Open Large Dialog" OnClick=@(args=> largeDialogOpen=true) />
<Dialog Title="This is a large dialog menu."
        SubText="This is the subtext area.  Below is the ChildContent area for components."
        IsOpen=@largeDialogOpen
        IsBlocking=@isBlocking
        DialogType=@DialogType.LargeHeader
        OnDismiss=@(args=> largeDialogOpen=false )>
    <ChildContent>
        <p>
            <TextField Label="Sample TextField" />
        </p>
        <p>
            <Dropdown ItemsSource=@items.Select(x=>new DropdownOption { Key=x.DisplayName, Text=x.DisplayName}) 
                         Placeholder="Select an option" 
                         OnChange=@UncontrolledSingleChangeHandler />
        </p>
    </ChildContent>
    <FooterTemplate>
        <DefaultButton Text="Cancel" OnClick=@(args=> largeDialogOpen=false) />
        <PrimaryButton Text="OK" OnClick=@(args=> largeDialogOpen=false) />
    </FooterTemplate>
</Dialog>

@code {
    bool dialogOpen = false;
    bool largeDialogOpen = false;
    string uncontrolledSingleSelectionResult;
    bool isBlocking = false;

    List<DataItem> items;

    protected override Task OnInitializedAsync()
    {
        items = new List<DataItem>
{
            new DataItem("First"),
            new DataItem("Second"),
            new DataItem("Third"),
            new DataItem("Fourth"),
            new DataItem("Fifth")
        };
        return base.OnInitializedAsync();
    }

    void UncontrolledSingleChangeHandler(DropdownChangeArgs args)
    {
        uncontrolledSingleSelectionResult = args.Option?.Key;
    }
}
