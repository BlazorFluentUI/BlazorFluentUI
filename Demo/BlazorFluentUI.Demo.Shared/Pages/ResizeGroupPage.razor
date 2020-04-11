@page "/resizeGroupPage"

<h1>ResizeGroupPage</h1>

<BFUResizeGroup OnGrowData=@onGrowData OnReduceData=@onReduceData
             Data=@(new ResizeGroupData<OverflowItem>(items, overflowItems, ComputeCacheKey(items)))
             GetCacheKey=@getCacheKey
>
    <DataTemplate Context="data">
        <BFUOverflowSet Items=@data.Items OverflowItems=@data.OverflowItems GetKey=@(x=> x.Key)>
            <ItemTemplate>
                <BFUCommandBarButton IconName="Add" Text=@context.Name />
            </ItemTemplate>
            <OverflowTemplate>
                <BFUCommandBarButton HideChevron="true" Style="min-width: 0; padding: 0 4px; align-self: stretch;" IconName="More" MenuItems=@(itemTransform(context)) />
            </OverflowTemplate>
            @*<OverflowItemTemplate>
                <ContextualMenuItem Text=@context.Name Key=@context.Key />
            </OverflowItemTemplate>*@
        </BFUOverflowSet>
    </DataTemplate>
</BFUResizeGroup>


@code {
    List<OverflowItem> items = new List<OverflowItem>();
    List<OverflowItem> overflowItems = new List<OverflowItem>();

    Func<ResizeGroupData<OverflowItem>, ResizeGroupData<OverflowItem>> onGrowData;
    Func<ResizeGroupData<OverflowItem>, ResizeGroupData<OverflowItem>>  onReduceData;

    Func<IEnumerable<OverflowItem>, IEnumerable<IBFUContextualMenuItem>> itemTransform => (items) =>
    {
        var menuItems = new List<IBFUContextualMenuItem>();
        if (items == null)
            return menuItems;
        foreach (var item in items)
        {
            var menuItem = new BFUContextualMenuItem()
            {
                Key = item.Key,
                Text = item.Name,
                IconName = "Add"
            };
            menuItems.Add(menuItem);
        }
        return menuItems;
    };

    @*Func<(IEnumerable<OverflowItem> primary, IEnumerable<OverflowItem> overflow, string cacheKey), RenderFragment> onRenderData = data =>@<BFUOverflowSet Items=@data.primary OverflowItems=@data.overflow>
    <ItemTemplate>
        <BFUCommandBarButton IconName="Add" Text=@context.Name />
    </ItemTemplate>
    <OverflowItemTemplate>
        <ContextualMenuItem Text=@context.Name Key=@context.Key />
    </OverflowItemTemplate>
</BFUOverflowSet>;*@
    Func<ResizeGroupData<OverflowItem>, string> getCacheKey = data => data.CacheKey;


    protected override Task OnInitializedAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            items.Add(new OverflowItem(i));
        }
        for (int i = 11; i < 20; i++)
        {
            overflowItems.Add(new OverflowItem(i));
        }

        onGrowData = data =>
        {
            if (overflowItems.Count == 0)
                return default;

            var firstItem = overflowItems.First();
            overflowItems.RemoveAt(0);
            items.Add(firstItem);

            return new ResizeGroupData<OverflowItem>(items, overflowItems, ComputeCacheKey(items));
        };

        onReduceData = data =>
        {
            if (items.Count == 0)
                return default;

            var lastItem = items.Last();
            items.Remove(lastItem);
            overflowItems.Insert(0, lastItem);

            return new ResizeGroupData<OverflowItem>(items, overflowItems, ComputeCacheKey(items));

        };

        return Task.CompletedTask;
    }

    private string ComputeCacheKey(IEnumerable<OverflowItem> items)
    {
        return items.Aggregate("", (acc, item) => acc + item.Key);
    }
}
