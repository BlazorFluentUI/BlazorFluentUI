@page "/groupedListPage"
@using DynamicData

<header class="root">
    <h1 class="title">GroupedList</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A grouped list (<code>GroupedList</code>) allows you to render a set of items as multiple lists with various grouping properties.
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
            <Stack Style="height:calc(100% - 0px);">
                <Toggle Label="IsVirtualizing" @bind-Checked="isVirtualizing" />
                <Toggle OffText="Normal" OnText="Compact" Label="Enable compact mode" @bind-Checked="isCompact" />
                <Label>Grouped List</Label>

                <div data-is-scrollable="true"
                     style="height:100%;overflow-y:auto;">
                    <SelectionZone Selection=@selection
                                   DisableRenderOnSelectionChanged="true"
                                   SelectionMode=@SelectionMode.Multiple>
                        <FocusZone Direction="FocusZoneDirection.Vertical">
                            <GroupedList ItemsSource=@groupedData
                                         TKey="object"
                                         GetKey="item => item.Key"
                                         Compact=@isCompact.GetValueOrDefault()
                                         IsVirtualizing=@isVirtualizing.GetValueOrDefault()
                                         TItem="GroupedDataItem"
                                         GroupTitleSelector=@(x=>x.DisplayName)
                                         Selection=@selection
                                         SubGroupSelector=@(x=>x.Data)
                                         SelectionMode=@SelectionMode.Multiple>
                                <ItemTemplate>
                                    <DetailsRow Item=@context.Item.Item
                                                Columns=@columns
                                                Compact=@isCompact.GetValueOrDefault()
                                                ItemIndex=@context.Index
                                                Selection=@selection
                                                GroupNestingDepth=@context.Item.Depth
                                                SelectionMode=@SelectionMode.Multiple />
                                </ItemTemplate>
                            </GroupedList>
                        </FocusZone>
                    </SelectionZone>
                </div>
            </Stack>
        </div>
    </div>
</div>
@code {
    //ToDo: Add Demo sections
    string DebugText = "";
    bool? isCompact;
    bool? isVirtualizing = true;
    int count = 0;
    //GroupedDataItem rootGroup;
    List<DataItem> data;
    List<GroupedDataItem> groupedData;
    Selection<GroupedDataItem> selection = new Selection<GroupedDataItem>();

    List<DetailsRowColumn<GroupedDataItem>> columns;

    protected override Task OnInitializedAsync()
    {

        columns = new List<DetailsRowColumn<GroupedDataItem>>();
        columns.Add(new DetailsRowColumn<GroupedDataItem> { FieldSelector = x => x.Key, Name = "Key", MinWidth = 60 });
        columns.Add(new DetailsRowColumn<GroupedDataItem> { FieldSelector = x => x.DisplayName, Name = "Name" });
        columns.Add(new DetailsRowColumn<GroupedDataItem> { FieldSelector = x => x.Description, Name = "Description" });

        data = new List<DataItem>();

        for (var i = 0; i < 100; i++)
        {
            count++;
            data.Add(new DataItem(count));
        }

        groupedData = data.GroupBy(x =>
        {
            var number = int.Parse(x.Key);
            int group = (number - 1) / 10;
            return group;
        }).Select(x =>
        {
            var subGroup = new GroupedDataItem(x);
            subGroup.Data = subGroup.Data.GroupBy(y =>
            {
                var number = int.Parse(y.Key);
                int group = (number - 1) / 5;
                return group;
            }).Select(y =>
            {
                var subSubGroup = new GroupedDataItem(y);
                return subSubGroup;
            }).ToList();

            return subGroup;
        }).ToList();

        //groupedData.Add(new GroupedDataItem(new DataItem("TEST!") ) );
        //rootGroup = new GroupedDataItem(new DataItem("root"));
        //rootGroup.Data = groupedData;

        return Task.CompletedTask;
    }
}
