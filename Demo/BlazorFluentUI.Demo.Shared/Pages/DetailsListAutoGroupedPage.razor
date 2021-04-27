@page "/detailsListAutoGroupedPage"
@using System.Reactive.Linq
@using DynamicData
@using BlazorFluentUI.Lists

<header class="root">
    <h1 class="title">DetailsListAuto-Grouped</h1>
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
        <Demo Header="Details List Auto - Grouped Data" Key="0" MetadataPath="DetailsListAutoGroupedPage">
            <Stack Style="height:100%;">
                <Stack Horizontal="true" Tokens="new StackTokens { ChildrenGap = new double[] { 10.0 } }">
                    <Toggle Label="IsVirtualizing" OnText="true" OffText="false" @bind-Checked="isVirtualizing" />
                    <Toggle Label="IsCompact" OnText="true" OffText="false" @bind-Checked="isCompact" />
                    <Dropdown ItemsSource=@selectionModeOptions
                              @bind-SelectedOption=selectedModeOption
                              Style="max-width:300px;">
                    </Dropdown>
                </Stack>
                <TextField Label="Filter Description"
                           Value=@filter
                           OnInput=@(val => { filter = val; descriptionColumn!.FilterPredicate = prop => (prop as string)!.Contains(filter); }) />
                <div data-is-scrollable="true" style="height:100%;overflow-y:auto;">
                    <DetailsListAuto ItemsSource="dataSource"
                                     @ref="detailsList"
                                     Columns="columnsSource"
                                     Compact="@isCompact"
                                     GetKey=@(x=>x.Key)
                                     TItem="DataItem"
                                     IsVirtualizing="@isVirtualizing"
                                     GroupBy=@(new System.Collections.Generic.List<Func<DataItem,object>>
                                              {
                                                  x=>x.GroupName!,
                                                  x=>x.KeyNumber % 2 == 0 ? "even" :"odd"
                                              })
                                     LayoutMode="DetailsListLayoutMode.Justified"
                                     SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), selectedModeOption!.Key!))>
                    </DetailsListAuto>
                </div>
            </Stack>
        </Demo>
    </div>
</div>
@code {
    bool isVirtualizing = true;
    bool isCompact = false;
    IDropdownOption? selectedModeOption;
    System.Collections.Generic.List<IDropdownOption>? selectionModeOptions;

    Selection<DataItem> selection = new Selection<DataItem>();

    System.Collections.Generic.List<DataItem> dataSource = new();
    int count = 0;

    System.Collections.Generic.List<IDetailsRowColumn<DataItem>> columnsSource = new();

    DetailsListAuto<DataItem>? detailsList;

    string filter = "";
    IDetailsRowColumn<DataItem>? descriptionColumn;

    protected override void OnInitialized()
    {
        selectionModeOptions = Enum.GetValues(typeof(SelectionMode)).Cast<SelectionMode>()
           .Select(x => new DropdownOption { Key = x.ToString(), Text = x.ToString() })
           .Cast<IDropdownOption>()
           .ToList();
        selectedModeOption = selectionModeOptions.FirstOrDefault(x => x.Key == "Multiple");

        columnsSource.Add(new DetailsRowColumn<DataItem, int>("Key", x => x.KeyNumber) { MaxWidth = 70, Index = 0 });
        columnsSource.Add(new DetailsRowColumn<DataItem, string>("Name", x => x.DisplayName!) { Index = 1, MaxWidth = 150, IsResizable = true });
        descriptionColumn = new DetailsRowColumn<DataItem, string>("Description", x => x.Description!) { Index = 2 };
        columnsSource.Add(descriptionColumn);


        var data = new System.Collections.Generic.List<DataItem>();

        for (var i = 0; i < 40; i++)
        {
            count++;
            data.Add(new DataItem(count));
        }

        dataSource.AddRange(data);

        base.OnInitialized();
    }


    //private Func<DataItem, IComparable> GetSortSelector(string key)
    //{
    //    if (key == "Key")
    //        return (item) => item.Key;
    //    else if (key == "Name")
    //        return (item) => item.DisplayName;
    //    else
    //        return item => item.Description;
    //}


}