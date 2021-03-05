@page "/detailsListPage"

@using DynamicData 
@using DynamicData.Binding 
@using System.Collections.ObjectModel
@using System.Reactive.Linq
@using System.Reactive.Subjects

<Stack Style="height:100%;">
    <h3>DetailsList</h3>
    <Stack Horizontal="true" Tokens="new StackTokens { ChildrenGap = new double[] { 10.0 } }">
        <Toggle Label="IsVirtualizing" OnText="true" OffText="false" @bind-Checked="isVirtualizing" />
        <Toggle Label="IsCompact" OnText="true" OffText="false" @bind-Checked="isCompact" />
        <Dropdown ItemsSource=@selectionModeOptions
                     @bind-SelectedOption=selectedModeOption
                     Style="max-width:300px;">
        </Dropdown>
        <Toggle Label="Disable Selection"
                   @bind-Checked="selectionDisabled"
                   OnText="disabled"
                   OffText="enabled"
                    />
    </Stack>
    <TextField Label="Filter Description"
                  @bind-Value=@dataContainer.Filter
                  @bind-Value:event="OnInput" />
        <div data-is-scrollable="true"
             style="height:400px;overflow-y:auto;">
            @if (ReadonlyList != null)
            {
                <DetailsList ItemsSource="null"
                                IsVirtualizing=@isVirtualizing.GetValueOrDefault()
                                Compact=@isCompact.GetValueOrDefault()
                                Columns="ReadonlyColumns"
                                GetKey=@(item => item.Key)
                                LayoutMode="DetailsListLayoutMode.Justified"
                                Selection="selection"
                                SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), selectedModeOption.Key))
                                DisableSelectionZone=@selectionDisabled.GetValueOrDefault()
                                />
            }
            else
            {
                @("Loading...")
            }
        </div>
</Stack>

@code {

    // You DO NOT have to use DynamicData and Observables with DetailsList.  You can do it all manually.  See 
    // https://developer.microsoft.com/en-us/fluentui#/controls/web/detailslist for examples of how to use DetailsList
    // other ways.

    bool? isVirtualizing = true;
    bool? isCompact = false;
    bool? selectionDisabled = false;
    IDropdownOption selectedModeOption;
    List<IDropdownOption> selectionModeOptions;


    Selection<DataItem> selection = new Selection<DataItem>();

    // SourceCache is from DynamicData and is basically a container for your items that you can dynamically transform by filtering, sorting, etc. 
    SourceCache<DataItem, string> dataSource = new SourceCache<DataItem, string>(x => x.Key);
    public ReadOnlyObservableCollection<DataItem> ReadonlyList;
    int count = 0;

    // We're creating another container for the column array that needs to be defined to show columns in DetailsList.
    SourceCache<DetailsRowColumn<DataItem>, string> columnsSource = new SourceCache<DetailsRowColumn<DataItem>, string>(x => x.Key);
    public ReadOnlyObservableCollection<DetailsRowColumn<DataItem>> ReadonlyColumns;

    // This class just holds some properties that make it easier to sort and filter data. 
    ObservableDataContainer dataContainer = new ObservableDataContainer();

    class ObservableDataContainer : AbstractNotifyPropertyChanged
    {
        private string filter = "";
        public string Filter { get => filter; set => SetAndRaise(ref filter, value); }

        private bool descending = false;
        public bool Descending { get => descending; set => SetAndRaise(ref descending, value); }

        // When you click a column, we change this so DynamicData can know which property to sort by.
        private Func<DataItem, object> sortSelector = x=>x.KeyNumber;
        public Func<DataItem, object> SortSelector { get => sortSelector; set => SetAndRaise(ref sortSelector, value); }

        // This is an observable that is similar to the above callback, just in observable form.  This tells us when and how to change filtering.
        public IObservable<Func<DataItem, bool>> DynamicDescriptionFilter { get; private set; }

        public IObservable<SortExpressionComparer<DataItem>> DynamicSortExpression { get; private set; }

        public IObservable<bool> IsFiltered { get; private set; }

        public ObservableDataContainer()
        {
            // This creates the observable that outputs a new filtering function.  Basically, return true if the string contains x.
            // Throttle is an operator that limits how often the observable is fired.  This is useful when you type so the filter 
            // observable doesn't fire every time you hit a key.  This requires threading to work smoothly so doesn't work well
            // in client-side blazor yet.
            DynamicDescriptionFilter = this.WhenValueChanged(@this => @this.Filter).Throttle(TimeSpan.FromMilliseconds(250), System.Reactive.Concurrency.TaskPoolScheduler.Default)
                //.Throttle(TimeSpan.FromMilliseconds(250))  //this freezes the ui in wasm since threads are not working well yet
                .Select<string, Func<DataItem, bool>>(f => item => item.Description.Contains(f));

            // DynamicData requires a specific expression for sorting.  This is where the expression is created.  The SortSelector 
            // property is combined with the IsDescending property.  When either of them is changed, the DynamicSortExpression 
            // observable is fired with the newly created SortExpressionComparer.
            DynamicSortExpression = this.WhenValueChanged(@this => @this.SortSelector).CombineLatest(this.WhenValueChanged(@this => @this.Descending), (selector, isDescending) =>
            {

                if (isDescending)
                {
                    return SortExpressionComparer<DataItem>.Descending(selector.ConvertToIComparable());
                }
                else
                {
                    return SortExpressionComparer<DataItem>.Ascending(selector.ConvertToIComparable());
                }
            });


            // We create the IsFiltered observable here.  Basically, when the Filter property is changed, fire the IsFiltered observable.
            IsFiltered = this.WhenValueChanged(@this => @this.Filter).Throttle(TimeSpan.FromMilliseconds(250), System.Reactive.Concurrency.TaskPoolScheduler.Default).Select(x =>
            {
                return !string.IsNullOrWhiteSpace(x);
            });
        }
    }


    protected override async Task OnInitializedAsync()
    {
        selectionModeOptions = Enum.GetValues(typeof(SelectionMode)).Cast<SelectionMode>()
           .Select(x => new DropdownOption { Key = x.ToString(), Text = x.ToString() })
           .Cast<IDropdownOption>()
           .ToList();
        selectedModeOption = selectionModeOptions.FirstOrDefault(x => x.Key == "Single");

        await Task.Run(() =>
        {
            // We load the column data into the columnsSource SourceCache.
            columnsSource.AddOrUpdate(new DetailsRowColumn<DataItem>("Key", x => x.KeyNumber) { MaxWidth = 70, Index = 0, OnColumnClick = this.OnColumnClick, IsResizable = true });
            columnsSource.AddOrUpdate(new DetailsRowColumn<DataItem>("Name", x => x.DisplayName) { Index = 1, MaxWidth = 150, OnColumnClick = this.OnColumnClick, IsResizable = true });
            var descColumn = new DetailsRowColumn<DataItem>("Description", x => x.Description) { Index = 2, OnColumnClick = this.OnColumnClick };
            columnsSource.AddOrUpdate(descColumn);

            // We're loading our sample data into the dataSource SourceCache.
            for (var i = 0; i < 100; i++)
            {

                count++;
                dataSource.AddOrUpdate(new DataItem(count));
            }

            // We subscribe to the IsFiltered observable so that we know when to mark the description column with a Filter icon.  
            // All we have to do is change the column class and use the AddOrUpdate method on the SourceCache.  DynamicData will
            // automatically change the contents and notify anything that is watching that list.
            dataContainer.IsFiltered.Subscribe(isFiltered =>
            {
                descColumn.IsFiltered = isFiltered;
                //InvokeAsync(StateHasChanged);
                columnsSource.AddOrUpdate(descColumn);
            });



            // This is the meat of DynamicData.  We connect to the data SourceCache, apply the Filter (which is dynamic), 
            // apply the Sort expression (which is also dynamic), and Bind it to a readonly list property.  This is what we're
            // going to display in the DetailsList.  It will update automatically which any changes we make.  Since DynamicData is 
            // based on ReactiveExtensions, we have to Subscribe to it to make it "do stuff".
            dataSource.Connect()
                    .Filter(dataContainer.DynamicDescriptionFilter)
                    .Sort(dataContainer.DynamicSortExpression)
                    .Bind(out ReadonlyList)
                    .Subscribe();

            // Likewise, we connect to the column SourceCache.  This lets us make a change in one column and everything gets updated 
            // automatically.  We also have a Do operator in there that calls a StateHasChanged whenever the contents are changed or if
            // the Sort expression changes.
            columnsSource.Connect()
                .Sort(SortExpressionComparer<DetailsRowColumn<DataItem>>.Ascending(x => x.Index))
                .Bind(out ReadonlyColumns)
                .Do(_ => InvokeAsync(StateHasChanged))  //when a column is clicked, that column's details will update... but other columns won't.  Need to call StateHasChanged to redraw all.
                .Subscribe();
        });


        await base.OnInitializedAsync();
    }

    // This callback is fired when a column header is clicked.  We go through each column item and change their sort properties.
    // To update the changes in our UI, we just use the `AddOrUpdate` method and DynamicData will make the changes in the bound
    // list.
    private void OnColumnClick(DetailsRowColumn<DataItem> column)
    {
        if (column.IsSorted)
        {
            column.IsSortedDescending = !column.IsSortedDescending;
            dataContainer.Descending = column.IsSortedDescending;
            dataContainer.SortSelector = column.FieldSelector;
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

    }


}
