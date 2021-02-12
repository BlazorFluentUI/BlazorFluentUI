@page "/contextualMenuPage"

<h1>ContextualMenu</h1>

<Demo Header="Basic ContextMenu" Key="0" MetadataPath="ContextualMenuPage">
    <DefaultButton Text="Show ContextualMenu" MenuItems=@contextualMenuItems />
</Demo>

<p>
    <em>@debugText</em>
</p>
@*<h3>Starting to look good.  Finish Header, Section, submenu, and wire up events properly</h3>*@


@code {
    List<IContextualMenuItem> contextualMenuItems;

    private System.Windows.Input.ICommand buttonCommand;
    private string debugText;
    private int commandCount = 0;

    private Action<ItemClickedArgs> OnClick => args =>
    {
        InvokeAsync(() =>
        {
            System.Diagnostics.Debug.WriteLine($"ItemClick handler: key={args.Key}");
            var item = contextualMenuItems.Where(x => x.Items != null).SelectMany(x => x.Items).Concat(contextualMenuItems.Where(x => x.Items == null)).FirstOrDefault(x => x.Key == args.Key);
            if (item != null)
            {
                item.Checked = !item.Checked;
            }
        });
    };

    protected override Task OnInitializedAsync()
    {
        buttonCommand = new Utils.RelayCommand((p) =>
        {
            debugText = $"{p.ToString()} button was clicked. {commandCount++}";
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
