@page "/detailsListGroupedPage"

@using DynamicData
@using DynamicData.Binding
@using System.Collections.ObjectModel
@using System.Reactive.Linq
@using System.Reactive.Subjects
<header class="root">
    <h1 class="title">DetailsList - Grouped Data</h1>
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

            <Stack Horizontal="true" Tokens="new StackTokens { ChildrenGap = new double[] { 10.0 } }">
                <Toggle Label="IsVirtualizing" OnText="true" OffText="false" @bind-Checked="isVirtualizing" />
                <Toggle Label="IsCompact" OnText="true" OffText="false" @bind-Checked="isCompact" />
                <Dropdown ItemsSource=@selectionModeOptions
                          @bind-SelectedOption=selectedModeOption
                          Style="max-width:300px;">
                </Dropdown>
            </Stack>
            <div data-is-scrollable="true" style="height:400px; overflow-y:auto;">
                <DetailsList ItemsSource="ReadonlyList"
                             @ref="detailsList"
                             Columns="ReadonlyColumns"
                             GetKey="(item)=>item.Key"
                             TItem="GroupedDataItem"
                             Compact="@isCompact.GetValueOrDefault()"
                             IsVirtualizing="@isVirtualizing.GetValueOrDefault()"
                             SubGroupSelector=@(item=> item.ObservableData)
                             GroupTitleSelector=@(item=>item.DisplayName)
                             LayoutMode="DetailsListLayoutMode.Justified"
                             Selection="selection"
                             SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), selectedModeOption.Key))>
                </DetailsList>
            </div>


        </div>
    </div>
</div>
@code {
    //ToDo: Add Demo sections
    bool? isVirtualizing = true;
    bool? isCompact = false;
    IDropdownOption selectedModeOption;
    List<IDropdownOption> selectionModeOptions;

    Selection<GroupedDataItem> selection = new Selection<GroupedDataItem>();

    SourceCache<DataItem, string> dataSource = new SourceCache<DataItem, string>(x => x.Key);
    public System.ComponentModel.BindingList<GroupedDataItem> ReadonlyList = new System.ComponentModel.BindingList<GroupedDataItem>();
    int count = 0;

    SourceCache<DetailsRowColumn<GroupedDataItem>, string> columnsSource = new SourceCache<DetailsRowColumn<GroupedDataItem>, string>(x => x.Key);
    public ReadOnlyObservableCollection<DetailsRowColumn<GroupedDataItem>> ReadonlyColumns;

    DetailsList<GroupedDataItem> detailsList;

    ObservableDataContainer dataContainer = new ObservableDataContainer();

    class ObservableDataContainer : AbstractNotifyPropertyChanged
    {

        private bool descending = false;
        public bool Descending { get => descending; set => SetAndRaise(ref descending, value); }

        private Func<GroupedDataItem, object> sortSelector = x => x.KeyNumber;
        public Func<GroupedDataItem, object> SortSelector { get => sortSelector; set => SetAndRaise(ref sortSelector, value); }

        public IObservable<SortExpressionComparer<GroupedDataItem>> DynamicSortExpression { get; private set; }

        public IObservable<bool> IsFiltered { get; private set; }

        public ObservableDataContainer()
        {
            DynamicSortExpression = this.WhenValueChanged(@this => @this.SortSelector).CombineLatest(this.WhenValueChanged(@this => @this.Descending), (selector, isDescending) =>
            {

                if (isDescending)
                {
                    return SortExpressionComparer<GroupedDataItem>.Descending(selector.ConvertToIComparable());
                }
                else
                {
                    return SortExpressionComparer<GroupedDataItem>.Ascending(selector.ConvertToIComparable());
                }
            });
        }
    }


    protected override void OnInitialized()
    {
        selectionModeOptions = Enum.GetValues(typeof(SelectionMode)).Cast<SelectionMode>()
           .Select(x => new DropdownOption { Key = x.ToString(), Text = x.ToString() })
           .Cast<IDropdownOption>()
           .ToList();
        selectedModeOption = selectionModeOptions.FirstOrDefault(x => x.Key == "Multiple");

        columnsSource.AddOrUpdate(new DetailsRowColumn<GroupedDataItem, int>("Key", x => x.KeyNumber) { MaxWidth = 70, Index = 0, OnColumnClick = this.OnColumnClick });
        columnsSource.AddOrUpdate(new DetailsRowColumn<GroupedDataItem, string>("Name", x => x.DisplayName) { Index = 1, MaxWidth = 150, OnColumnClick = this.OnColumnClick, IsResizable = true });
        var descColumn = new DetailsRowColumn<GroupedDataItem, string>("Description", x => x.Description) { Index = 2, OnColumnClick = this.OnColumnClick };
        columnsSource.AddOrUpdate(descColumn);


        var data = new System.Collections.Generic.List<DataItem>();

        for (var i = 0; i < 100; i++)
        {
            count++;
            data.Add(new DataItem(count));
        }

        dataSource.AddOrUpdate(data);

        dataSource.Connect()
            .Group(x => (x.KeyNumber - 1) / 10)
            .Transform(x => new GroupedDataItem(x, dataContainer.DynamicSortExpression))
            .Bind(ReadonlyList)
            .Subscribe();

        columnsSource.Connect()
            .Sort(SortExpressionComparer<DetailsRowColumn<GroupedDataItem>>.Ascending(x => x.Index))
            .Bind(out ReadonlyColumns)
            .Do(_ => StateHasChanged())  //when a column is clicked, that column's details will update... but other columns won't.  Need to call StateHasChanged to redraw all.
            .Subscribe();

        base.OnInitialized();
    }

    private void OnColumnClick(DetailsRowColumn<GroupedDataItem> column)
    {

        if (column.IsSorted)
        {
            column.IsSortedDescending = !column.IsSortedDescending;
            columnsSource.AddOrUpdate(column);
        }
        else
        {
            var copyList = columnsSource.Items.ToList();
            foreach (var col in copyList)
            {
                col.IsSorted = false;
                if (col == column)
                {
                    col.IsSorted = true;
                    col.IsSortedDescending = false;
                }
            }
            columnsSource.AddOrUpdate(copyList);
        }
        dataContainer.Descending = column.IsSortedDescending;
        dataContainer.SortSelector = column.FieldSelector;

        detailsList?.ForceUpdate();  //needed because GroupedList uses a transformed version of ItemsSource that doesn't update automatically.

    }

    private Func<DataItem, IComparable> GetSortSelector(string key)
    {
        if (key == "Key")
            return (item) => item.Key;
        else if (key == "Name")
            return (item) => item.DisplayName;
        else
            return item => item.Description;
    }


}
