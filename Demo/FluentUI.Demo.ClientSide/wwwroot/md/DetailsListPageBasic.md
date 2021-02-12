@page "/detailsListPageBasic"

<Demo MetadataPath="DetailsListPageBasic" Key="0" Header="Basic DetailsList">
    <div style="height:400px;" data-is-scrollable>
        <Stack Style="height:100%;">
            <h3>DetailsList</h3>
            <DetailsList ItemsSource="InputList"
                            Columns="Columns"
                            GetKey=@(item=>item.Key)
                            LayoutMode="DetailsListLayoutMode.Justified"
                            TItem="DataItem"
                            OnItemInvoked="OnClick"
                            Selection="selection"
                            SelectionMode="SelectionMode.Multiple">
            </DetailsList>
        </Stack>
    </div>
</Demo>
    @code {

        List<DataItem> InputList = new List<DataItem>();

        Selection<DataItem> selection = new Selection<DataItem>();

        public List<DetailsRowColumn<DataItem>> Columns = new List<DetailsRowColumn<DataItem>>();

        protected override void OnInitialized()
        {
            selection.GetKey = (item => item.Key);
            Columns.Add(new DetailsRowColumn<DataItem>("Key", x => x.KeyNumber) { MaxWidth = 70, Index = 0 });
            Columns.Add(new DetailsRowColumn<DataItem>("Name", x => x.DisplayName) { Index = 1, MaxWidth = 150, OnColumnClick = this.OnColumnClick, IsResizable = true });
            Columns.Add(new DetailsRowColumn<DataItem>("Description", x => x.Description) { Index = 2 });

            int count = 0;
            for (var i = 0; i < 1000; i++)
            {
                count++;

                InputList.Add(new DataItem(count));
            }

            base.OnInitialized();
        }

        private void OnColumnClick(DetailsRowColumn<DataItem> column)
        {
            // since we're creating a new list, we need to make a copy of what was previously selected
            var selected = selection.GetSelection();

            //create new sorted list
            InputList = new List<DataItem>(column.IsSorted ? InputList.OrderBy(x => x.DisplayName) : InputList.OrderByDescending(x => x.DisplayName));

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
