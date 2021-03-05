@page  "/overflowSetPage"

<header class="root">
    <h1 class="title">OverflowSet</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>The OverflowSet is a flexible container component that is useful for displaying a primary set of content with additional content in an overflow callout.
               Note that the example below is only an example of how to render the component, not a specific use case.
            </p>
            <h3 id="accessibility">Accessibility</h3>
            <p>By default, the OverflowSet is simply <code>role=group</code>. If you used as a menu, you will need to add <code>role="menubar"</code> and add proper aria roles to
               each rendered item (<code>menuitem</code>, <code>menuitemcheckbox</code>, <code>menuitemradio</code>)
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
        </div>
    </div>
</div>
@code {
    //ToDo: Add Demo sections
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
