@page "/detailsListPageBasic"
@using BlazorFluentUI.Lists
<header class="root">
    <h1 class="title">DetailsList Basic</h1>
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
            <Demo Header="Basic Details List" Key="0" MetadataPath="DetailsListPageBasic">
                <div data-is-scrollable="true" style="height:400px;overflow-y:auto;">
                    <DetailsList ItemsSource="InputList"
                                 Columns="Columns"
                                 GetKey=@(item=>item.Key)
                                 LayoutMode="DetailsListLayoutMode.Justified"
                                 TItem="DataItem"
                                 OnItemInvoked="OnClick"
                                 Selection="selection"
                                 SelectionMode="SelectionMode.Multiple">
                    </DetailsList>
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Details List with custom cells" Key="1" MetadataPath="DetailsListPageBasic">
                <div data-is-scrollable="true" style="height:400px;overflow-y:auto;">
                    <DetailsList ItemsSource="InputList"
                                 Columns="CustomColumns"
                                 GetKey=@(item=>item.Key)
                                 LayoutMode="DetailsListLayoutMode.Justified"
                                 TItem="DataItem"
                                 OnItemInvoked="OnClick"
                                 Selection="selection"
                                 SelectionMode="SelectionMode.Multiple">
                    </DetailsList>
                </div>
            </Demo>
        </div>
    </div>
</div>
@code {

    System.Collections.Generic.List<DataItem> InputList = new();

    Selection<DataItem> selection = new Selection<DataItem>();

    public System.Collections.Generic.List<IDetailsRowColumn<DataItem>> Columns = new();
    public System.Collections.Generic.List<IDetailsRowColumn<DataItem>> CustomColumns = new();

    protected override void OnInitialized()
    {
        selection.GetKey = (item => item.Key);
        Columns.Add(new DetailsRowColumn<DataItem>("Key", x => x.KeyNumber) { MaxWidth = 70, Index = 0 });
        Columns.Add(new DetailsRowColumn<DataItem>("Name", x => x.DisplayName!) { Index = 1, MaxWidth = 150, OnColumnClick = this.OnColumnClick, IsResizable = true });
        Columns.Add(new DetailsRowColumn<DataItem>("Description", x => x.Description!) { Index = 2 });

        // Do NOT use the DetailsRowColumn with two generic parameters.  It does not create an expression that can be used with DynamicAccessor.
        CustomColumns.Add(new DetailsRowColumn<DataItem>("Key", x => x.KeyNumber) { MaxWidth = 70, Index = 0 });
        CustomColumns.Add(new DetailsRowColumn<DataItem>("Name", x => x.DisplayName!) { Index = 1, MaxWidth = 150, OnColumnClick = this.OnColumnClick, IsResizable = true });
        CustomColumns.Add(new DetailsRowColumn<DataItem,DataItem>("Notes", x => x)
        {
            Index = 2,
            // Two issues:
            // 1.  Using a value type directly with a RenderFragment prevents any updating of the original value since it is copied to this RenderFragment.
            //     Instead, the output of ColumnItemTemplate is a DynamicAccessor<object> that has a single property of Value.  This way, changes are passed back
            //     to the original property because DynamicAccessor creates a setter from your original getter (the field selector).
            // 2.  Blazor cannot do type conversions directly with binding yet.  Since the output (of DynamicAccessor's Value) is an object, we need to manually 
            //     create the callback that sets the Value from the change.  We can't use generics (easily) because they would all have to be the same type.
            ColumnItemTemplate = obj => @<TextField @bind-Value="obj.Description" @bind-Value:event="OnChange"/>
            //ColumnItemTemplate = description => builder =>
            //{
            //    builder.OpenComponent<TextField>(0);
            //    builder.AddAttribute(1, "Value", description.Value);
            //    builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(this, changedText =>
            //    {
            //        description.Value = changedText;
            //    }));
            //    builder.CloseComponent();
            //}

        });


        int count = 0;
        for (var i = 0; i < 100; i++)
        {
            count++;

            InputList.Add(new DataItem(count));
        }

        base.OnInitialized();
    }

    private void OnColumnClick(IDetailsRowColumn<DataItem> column)
    {
        // since we're creating a new list, we need to make a copy of what was previously selected
        var selected = selection.GetSelection();

        //create new sorted list
        InputList = new System.Collections.Generic.List<DataItem>(column.IsSorted ? InputList.OrderBy(x => x.DisplayName) : InputList.OrderByDescending(x => x.DisplayName));

        //clear old selection and create new selection
        //selection.SetKeySelected(selected, true);

        column.IsSorted = !column.IsSorted;
        StateHasChanged();
    }

    private void OnClick(DataItem item)
    {
        Console.WriteLine("Clicked!");
    }
}
