@page "/contextualMenuPage"

<header class="root">
    <h1 class="title">ContextualMenu</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>ContextualMenus are lists of commands that are based on the context of selection, mouse hover or keyboard focus. They are one of the most effective and highly used command surfaces, and can be used in a variety of places.</p>
            <p>There are variants that originate from a command bar, or from cursor or focus. Those that come from CommandBars use a beak that is horizontally centered on the button. Ones that come from right click and menu button do not have a beak, but appear to the right and below the cursor. ContextualMenus can have submenus from commands, show selection checks, and icons.</p>
            <p>Organize commands in groups divided by rules. This helps users remember command locations, or find less used commands based on proximity to others. One should also group sets of mutually exclusive or multiple selectable options. Use icons sparingly, for high value commands, and don't mix icons with selection checks, as it makes parsing commands difficult. Avoid submenus of submenus as they can be difficult to invoke or remember.</p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>


        <div class="subSection">
            <Demo Header="Basic ContextMenu" Key="0" MetadataPath="ContextualMenuPage">
                <DefaultButton Text="Show ContextualMenu" MenuItems=@contextualMenuItems />
            </Demo>
        </div>

        <p>
            <em>@debugText</em>
        </p>
        @*<h3>Starting to look good.  Finish Header, Section, submenu, and wire up events properly</h3>*@


    </div>
</div>
@code {
    List<IContextualMenuItem>? contextualMenuItems;

    private System.Windows.Input.ICommand? buttonCommand;
    private string? debugText;
    private int commandCount = 0;

    private Action<ItemClickedArgs> OnClick => args =>
    {
        InvokeAsync(() =>
        {
            System.Diagnostics.Debug.WriteLine($"ItemClick handler: key={args.Key}");
            var item = contextualMenuItems?.Where(x => x.Items != null).SelectMany(x => x.Items!).Concat(contextualMenuItems.Where(x => x.Items == null)).FirstOrDefault(x => x.Key == args.Key);
            if (item != null)
            {
                item.Checked = !item.Checked;
            }
        });
    };

    protected override Task OnInitializedAsync()
    {
        buttonCommand = new RelayCommand((p) =>
        {
            debugText = $"{p?.ToString()} button was clicked. {commandCount++}";
            StateHasChanged();
        });

        contextualMenuItems = new List<IContextualMenuItem>()
    {
            new ContextualMenuItem()
            {
                Text = "Header1",
                ItemType = ContextualMenuItemType.Header,
                Key = "Header1",
                Command = buttonCommand,
                CommandParameter = "Header1"
            },
            new ContextualMenuItem()
            {
                Text = "Test1",
                ItemType = ContextualMenuItemType.Normal,
                Key = "Test1",
                IconName= "Mail",
                Command = buttonCommand,
                CommandParameter = "Test1"
            },
            new ContextualMenuItem()
            {
                Text = "Test2",
                ItemType = ContextualMenuItemType.Normal,
                Key = "Test2",
                CanCheck=true,
                OnClick=@OnClick,
                Command = buttonCommand,
                CommandParameter = "Test2"
            },
            new ContextualMenuItem()
            {
                ItemType = ContextualMenuItemType.Divider,
                Key = "Divider1"
            },
            new ContextualMenuItem()
            {
                Text = "Test3",
                ItemType = ContextualMenuItemType.Normal,
                Key = "Test3",
                Command = buttonCommand,
                CommandParameter = "Test3",
                Items = new System.Collections.Generic.List<IContextualMenuItem>()
            {
                    new ContextualMenuItem()
                    {
                        Text = "SubTest1",
                        ItemType = ContextualMenuItemType.Normal,
                        Key = "SubTest1",
                        Command = buttonCommand,
                        CommandParameter = "SubTest1",
                    },
                    new ContextualMenuItem()
                    {
                        Text = "SubTest2",
                        ItemType = ContextualMenuItemType.Normal,
                        Key = "SubTest2",
                        IconName = "Home",
                        Command = buttonCommand,
                        CommandParameter = "SubTest2"
                    }

                }
            },
        };

        return base.OnInitializedAsync();
    }


}
