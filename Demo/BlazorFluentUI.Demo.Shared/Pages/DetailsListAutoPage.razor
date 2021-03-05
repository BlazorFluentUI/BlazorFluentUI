@page "/detailsListAutoPage"
@using Microsoft.AspNetCore.Components.Web
@using DynamicData
@using System.Reactive.Linq

<header class="root">
    <h1 class="title">DetailsListAuto</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A details list (<code>DetailsList</code>) is a robust way to display an information-rich collection of items, and allow people to sort, group, and filter the content. Use a details list when information density is critical.
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
            <Demo MetadataPath="DetailsListAutoPage" Key="0" Header="DetailsListAuto with many options">
                <Stack Horizontal="true" Tokens="new StackTokens { ChildrenGap = new double[] { 10.0 } }">
                    <Toggle Label="IsVirtualizing" OnText="true" OffText="false" @bind-Checked="isVirtualizing" />
                    <Toggle Label="IsCompact" OnText="true" OffText="false" @bind-Checked="isCompact" />
                    <Dropdown ItemsSource=@selectionModeOptions
                              @bind-SelectedOption=selectedModeOption
                              Style="max-width:300px;">
                    </Dropdown>
                    <DefaultButton OnClick=@(arg => selection1.SelectedItems = (new List<DataItem> { dataSource[2], dataSource[3] }) ) Text="Set #3 & #4 selected" />
                </Stack>
                <TextField Label="Filter Description"
                           Value=@filter
                           OnInput=@(val => { filter = val; descriptionColumn.FilterPredicate = prop => prop.Contains(filter); }) />
                <div data-is-scrollable="true" style="height:400px;overflow-y:auto;">
                    <DetailsListAuto ItemsSource="dataSource"
                                     IsVirtualizing="@isVirtualizing.GetValueOrDefault()"
                                     TItem="DataItem"
                                     Compact="@isCompact.GetValueOrDefault()"
                                     Columns="columnsSource"
                                     GetKey=@(x=>x.Key)
                                     LayoutMode="DetailsListLayoutMode.Justified"
                                     Selection="selection1"
                                     SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), selectedModeOption.Key)) />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo MetadataPath="DetailsListAutoPage" Key="1" Header="DetailsListAuto with Fixed Columns">
                <div data-is-scrollable="true" style="height:400px;overflow-y:auto;">
                    <DetailsListAuto ItemsSource="dataSource"
                                     IsVirtualizing="true"
                                     Compact="true"
                                     Columns="fixedColumnsSource"
                                     GetKey=@(x=>x.Key)
                                     LayoutMode="DetailsListLayoutMode.FixedColumns"
                                     Selection="selection2"
                                     SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), selectedModeOption.Key)) />
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo MetadataPath="DetailsListAutoPage" Key="2" Header="DetailsListAuto with MarqueeSelection">
                <div data-is-scrollable="true" style="height:400px;overflow-y:auto;">
                    <MarqueeSelection Selection=@selection3>
                        <DetailsListAuto ItemsSource="dataSource"
                                         IsVirtualizing="true"
                                         Compact="true"
                                         Columns="columnsSource"
                                         GetKey=@(x=>x.Key)
                                         LayoutMode="DetailsListLayoutMode.Justified"
                                         Selection="selection3"
                                         SelectionMode="SelectionMode.Multiple" />
                    </MarqueeSelection>
                </div>
            </Demo>
        </div>
    </div>
</div>
@code {

    bool? isVirtualizing = true;
    bool? isCompact = false;
    IDropdownOption selectedModeOption;
    List<IDropdownOption> selectionModeOptions;

    Selection<DataItem> selection1 = new Selection<DataItem>();
    Selection<DataItem> selection2 = new Selection<DataItem>();
    Selection<DataItem> selection3 = new Selection<DataItem>();

    List<DataItem> dataSource = new List<DataItem>();

    int count = 0;

    // We're creating another container for the column array that needs to be defined to show columns in DetailsList.
    List<DetailsRowColumn<DataItem>> columnsSource = new List<DetailsRowColumn<DataItem>>();
    List<DetailsRowColumn<DataItem>> fixedColumnsSource = new List<DetailsRowColumn<DataItem>>();

    string filter = "";
    DetailsRowColumn<DataItem, string> descriptionColumn;


    protected override void OnInitialized()
    {
        selectionModeOptions = Enum.GetValues(typeof(SelectionMode)).Cast<SelectionMode>()
           .Select(x => new DropdownOption { Key = x.ToString(), Text = x.ToString() })
           .Cast<IDropdownOption>()
           .ToList();
        selectedModeOption = selectionModeOptions.FirstOrDefault(x => x.Key == "Single");

        // We load the column data into the columnsSource SourceCache.
        columnsSource.Add(new DetailsRowColumn<DataItem, int>("Key", x => x.KeyNumber) { MinWidth = 20, MaxWidth = 70, Index = 0, IsResizable = true });
        columnsSource.Add(new DetailsRowColumn<DataItem, string>("Name", x => x.DisplayName) { Index = 1, MinWidth = 100, MaxWidth = 150, IsResizable = true });
        descriptionColumn = new DetailsRowColumn<DataItem, string>("Description", x => x.Description) { Index = 2 };
        columnsSource.Add(descriptionColumn);

        fixedColumnsSource.Add(new DetailsRowColumn<DataItem, int>("Key", x => x.KeyNumber) { Index = 0 });
        fixedColumnsSource.Add(new DetailsRowColumn<DataItem, string>("Name", x => x.DisplayName) { Index = 1 });
        fixedColumnsSource.Add(new DetailsRowColumn<DataItem, string>("Description", x => x.Description) { Index = 2 });

        // We're loading our sample data into the dataSource SourceCache.
        for (var i = 0; i < 100; i++)
        {
            count++;
            dataSource.Add(new DataItem(count));
        }


        base.OnInitialized();
    }




}