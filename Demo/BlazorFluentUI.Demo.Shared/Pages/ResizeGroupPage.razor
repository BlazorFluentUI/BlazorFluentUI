@page "/resizeGroupPage"

<header class="root">
    <h1 class="title">ResizeGroupPage</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p class="root-158">
                ResizeGroup is a React component that can be used to help fit the right amount of content within a container. The consumer of the ResizeGroup provides some initial data, a reduce function, and a render function. The render function is responsible for populating the contents of a the container when given some data. The initial data should represent the data that should be rendered when the ResizeGroup has infinite width. If the contents returned by the render function do not fit within the ResizeGroup, the reduce function is called to get a version of the data whose rendered width should be smaller than the data that was just rendered.
            </p>
            <p class="root-158">
                An example scenario is shown below, where controls that do not fit on screen are rendered in an overflow menu. The data in this situation is a list of 'primary' controls that are rendered on the top level and a set of overflow controls rendered in the overflow menu. The initial data in this case has all the controls in the primary set. The implementation of onReduceData moves a control from the overflow well into the primary control set.
            </p>
            <p class="root-158">
                This component queries the DOM for the dimensions of elements. Too many of these dimension queries will negatively affect the performance of the component and could cause poor interactive performance on websites. One way to reduce the number of measurements performed by the component is to provide a cacheKey in the initial data and in the return value of onReduceData. Two data objects with the same cacheKey are assumed to have the same width, resulting in measurements being skipped for that data object. In the controls with an overflow example, the cacheKey is simply the concatenation of the keys of the controls that appear in the top level.
            </p>
            <p class="root-158">
                There is a boolean context property (isMeasured) added to let child components know if they are only being used for measurement purposes. When isMeasured is true, it will signify that the component is not the instance visible to the user. This will not be needed for most scenarios. It is intended to be used to to skip unwanted side effects of mounting a child component more than once. This includes cases where the component makes API requests, requests focus to one of its elements, expensive computations, and/or renders markup unrelated to its size. Be careful not to use this property to change the components rendered output in a way that effects it size in any way.
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
            <ResizeGroup OnGrowData=@onGrowData OnReduceData=@onReduceData
                         Data=@(new ResizeGroupData<OverflowItem>(items, overflowItems, ComputeCacheKey(items)))
                         GetCacheKey=@getCacheKey>
                <DataTemplate Context="data">
                    <OverflowSet Items=@data.Items OverflowItems=@data.OverflowItems GetKey=@(x=> x.Key)>
                        <ItemTemplate>
                            <CommandBarButton IconName="Add" Text=@context.Name />
                        </ItemTemplate>
                        <OverflowTemplate>
                            <CommandBarButton HideChevron="true" Style="min-width: 0; padding: 0 4px; align-self: stretch;" IconName="More" MenuItems=@(itemTransform(context)) />
                        </OverflowTemplate>
                        @*<OverflowItemTemplate>
                                <ContextualMenuItem Text=@context.Name Key=@context.Key />
                            </OverflowItemTemplate>*@
                    </OverflowSet>
                </DataTemplate>
            </ResizeGroup>
        </div>
    </div>
</div>

@code {
    //ToDo: Add Demo sections
    List<OverflowItem> items = new List<OverflowItem>();
    List<OverflowItem> overflowItems = new List<OverflowItem>();

    Func<ResizeGroupData<OverflowItem>, ResizeGroupData<OverflowItem>> onGrowData;
    Func<ResizeGroupData<OverflowItem>, ResizeGroupData<OverflowItem>> onReduceData;

    Func<IEnumerable<OverflowItem>, IEnumerable<IContextualMenuItem>> itemTransform => (items) =>
    {
        var menuItems = new List<IContextualMenuItem>();
        if (items == null)
            return menuItems;
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

    @*Func<(IEnumerable<OverflowItem> primary, IEnumerable<OverflowItem> overflow, string cacheKey), RenderFragment> onRenderData = data =>@<OverflowSet Items=@data.primary OverflowItems=@data.overflow>
            <ItemTemplate>
                <CommandBarButton IconName="Add" Text=@context.Name />
            </ItemTemplate>
            <OverflowItemTemplate>
                <ContextualMenuItem Text=@context.Name Key=@context.Key />
            </OverflowItemTemplate>
        </OverflowSet>;*@
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
