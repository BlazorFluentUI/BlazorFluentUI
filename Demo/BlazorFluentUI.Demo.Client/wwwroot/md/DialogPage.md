@page "/dialogPage"

<h1>Dialog</h1>

<BFUCheckbox Label="Is Blocking Dialog" @bind-Checked=@isBlocking />
<BFUDefaultButton Text="Open Dialog" OnClick=@(args=> dialogOpen=true) />
<BFUDialog Title="This is a dialog menu."
        SubText="This is the subtext area.  Below is the ChildContent area for components."
        IsOpen=@dialogOpen
        IsBlocking=@isBlocking
        OnDismiss=@(args=> dialogOpen=false )>
    <ChildContent>
        <p>
            <BFUTextField Label="Sample TextField" />
        </p>
        <p>
            <BFUDropdown ItemsSource=@items.Select(x=>new BFUDropdownOption { Key=x.DisplayName, Text=x.DisplayName}) 
                         Placeholder="Select an option" 
                         OnChange=@UncontrolledSingleChangeHandler />
        </p>
    </ChildContent>
    <FooterTemplate>
        <BFUDefaultButton Text="Cancel" OnClick=@(args=> dialogOpen=false) />
        <BFUPrimaryButton Text="OK" OnClick=@(args=> dialogOpen=false) />
    </FooterTemplate>
</BFUDialog>

<BFUDefaultButton Text="Open Large Dialog" OnClick=@(args=> largeDialogOpen=true) />
<BFUDialog Title="This is a large dialog menu."
        SubText="This is the subtext area.  Below is the ChildContent area for components."
        IsOpen=@largeDialogOpen
        IsBlocking=@isBlocking
        DialogType=@DialogType.LargeHeader
        OnDismiss=@(args=> largeDialogOpen=false )>
    <ChildContent>
        <p>
            <BFUTextField Label="Sample TextField" />
        </p>
        <p>
            <BFUDropdown ItemsSource=@items.Select(x=>new BFUDropdownOption { Key=x.DisplayName, Text=x.DisplayName}) 
                         Placeholder="Select an option" 
                         OnChange=@UncontrolledSingleChangeHandler />
        </p>
    </ChildContent>
    <FooterTemplate>
        <BFUDefaultButton Text="Cancel" OnClick=@(args=> largeDialogOpen=false) />
        <BFUPrimaryButton Text="OK" OnClick=@(args=> largeDialogOpen=false) />
    </FooterTemplate>
</BFUDialog>

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

    void UncontrolledSingleChangeHandler(BFUDropdownChangeArgs args)
    {
        uncontrolledSingleSelectionResult = args.Option?.Key;
    }
}
