
@page  "/overflowSetPage"
@using Models
<h1>OverflowSet</h1>

<h4>OverflowSet Horizontal Example</h4>
<BFUOverflowSet Items=@items OverflowItems=@items GetKey=@(x=> x.Key)>
    <ItemTemplate>
        <BFUCommandBarButton IconName="Add" Text=@context.Name />
    </ItemTemplate>
    <OverflowTemplate>
        <BFUCommandBarButton HideChevron="true" Style="min-width: 0; padding: 0 4px; align-self: stretch;" IconName="More" MenuItems=@(itemTransform(context)) />
    </OverflowTemplate>
</BFUOverflowSet>

<h4>OverflowSet Vertical Example</h4>
<BFUOverflowSet Items=@smallerItems OverflowItems=@smallerItems Vertical="true" GetKey=@(x=> x.Key)>
    <ItemTemplate>
        <BFUCommandBarButton IconName="Add" Text=@context.Name />
    </ItemTemplate>
    <OverflowTemplate>
        <BFUCommandBarButton HideChevron="true" Style="min-width: 0; padding: 10px; font-size:16px;" IconName="More" MenuItems=@(itemTransform(context)) />
    </OverflowTemplate>
</BFUOverflowSet>

@code {
    System.Collections.Generic.List<OverflowItem> items;
    System.Collections.Generic.List<OverflowItem> smallerItems;

    Func<IEnumerable<OverflowItem>, IEnumerable<IBFUContextualMenuItem>> itemTransform => (items) =>
    {
        var menuItems = new System.Collections.Generic.List<IBFUContextualMenuItem>();
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

    protected override Task OnInitializedAsync()
    {
        items = new System.Collections.Generic.List<OverflowItem>();
        for (int i = 0; i < 10; i++)
        {
            items.Add(new OverflowItem(i));
        }
        smallerItems = new System.Collections.Generic.List<OverflowItem>();
        for (int i = 0; i < 3; i++)
        {
            smallerItems.Add(new OverflowItem(i));
        }

        return Task.CompletedTask;
    }
}
