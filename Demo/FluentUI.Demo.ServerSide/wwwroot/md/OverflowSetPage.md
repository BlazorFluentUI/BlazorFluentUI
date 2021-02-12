@page  "/overflowSetPage"

<h1>OverflowSet</h1>

<h4>OverflowSet Horizontal Example</h4>
<OverflowSet Items=@items OverflowItems=@items GetKey=@(x=> x.Key)>
    <ItemTemplate>
        <CommandBarButton IconName="Add" Text=@context.Name />
    </ItemTemplate>
    <OverflowTemplate>
        <CommandBarButton HideChevron="true" Style="min-width: 0; padding: 0 4px; align-self: stretch;" IconName="More" MenuItems=@(itemTransform(context)) />
    </OverflowTemplate>
</OverflowSet>

<h4>OverflowSet Vertical Example</h4>
<OverflowSet Items=@smallerItems OverflowItems=@smallerItems Vertical="true" GetKey=@(x=> x.Key)>
    <ItemTemplate>
        <CommandBarButton IconName="Add" Text=@context.Name />
    </ItemTemplate>
    <OverflowTemplate>
        <CommandBarButton HideChevron="true" Style="min-width: 0; padding: 10px; font-size:16px;" IconName="More" MenuItems=@(itemTransform(context)) />
    </OverflowTemplate>
</OverflowSet>

@code {
    System.Collections.Generic.List<OverflowItem> items;
    System.Collections.Generic.List<OverflowItem> smallerItems;

    Func<IEnumerable<OverflowItem>, IEnumerable<IContextualMenuItem>> itemTransform => (items) =>
    {
        var menuItems = new System.Collections.Generic.List<IContextualMenuItem>();
        foreach (var item in items)
        {
            var menuItem = new ContextualMenuItem()
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
