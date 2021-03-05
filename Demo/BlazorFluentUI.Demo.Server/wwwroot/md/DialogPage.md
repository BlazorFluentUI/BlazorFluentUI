@page "/dialogPage"

<header class="root">
    <h1 class="title">Dialog</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A dialog box (<code>Dialog</code>) is a temporary pop-up that takes focus from the page or app and requires people to interact with it. It’s primarily used for confirming actions, such as deleting a file, or asking people to make a choice.
            </p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <div class="subSection">

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

        </div>
    </div>
</div>

@code {
    //ToDo: Add Demo sections
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
