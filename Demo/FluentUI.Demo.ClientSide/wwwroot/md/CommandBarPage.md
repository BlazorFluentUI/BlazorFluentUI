@page "/commandBarPage"

<h1>CommandBar</h1>

<Demo Header="Basic CommandBar" Key="0" MetadataPath="CommandBarPage">
    <CommandBar Items=@items />
</Demo>

<Demo Header="CommandBar with right items" Key="1" MetadataPath="CommandBarPage">
    <CommandBar Items=@items FarItems=@farItems OverflowItems=@overflowItems />
</Demo>

<Demo Header="CommandBar with custom item rendering" Key="2" MetadataPath="CommandBarPage">
    <CommandBar Items=@customItems >
        <ItemTemplate>
            @if (context.ItemType == ContextualMenuItemType.Divider)
            {
                <CommandBarButton Disabled="true" IconName="GripperBarVertical" />
            }
            else
            {
                <CommandBarButton IconName=@context.IconName Text=@(!context.IconOnly ? context.Text : null)
                                     MenuItems=@context.Items
                                     Href=@context.Href
                                     OnClick=@(args => context.OnClick?.Invoke(new ItemClickedArgs() { MouseEventArgs = args, Key = context.Key }))
                                     Command=@context.Command CommandParameter=@context.CommandParameter
                                     Disabled=@context.Disabled AriaLabel=@context.AriaLabel Checked=@context.Checked ClassName=@context.ClassName
                                     Split=@context.Split Style=@context.Style />
            }
        </ItemTemplate>
    </CommandBar>
</Demo>

<Demo Header="Custom content in CommmandBarButton" Key="3" MetadataPath="CommandBarPage">
    <CommandBar Items=@contentItems>
        <ItemTemplate>
            <CommandBarButton IconName=@context.IconName Text=@(!context.IconOnly ? context.Text : null)
                                 MenuItems=@context.Items
                                 Href=@context.Href
                                 OnClick=@(args => context.OnClick?.Invoke(new ItemClickedArgs() { MouseEventArgs = args, Key = context.Key }))
                                 Command=@context.Command CommandParameter=@context.CommandParameter
                                 Disabled=@context.Disabled AriaLabel=@context.AriaLabel Checked=@context.Checked ClassName=@context.ClassName
                                 Split=@context.Split Style=@context.Style>
                @switch (context.Key)
                {
                    case "yellowCircle":
                        <svg height="14" width="40">
                            <ellipse cx="20" cy="8" rx="10" ry="5" style="fill:yellow;stroke:purple;stroke-width:2" />
                            Sorry, your browser does not support inline SVG.
                        </svg>
                        break;
                    case "greenCircle":
                        <svg height="14" width="40">
                            <ellipse cx="20" cy="8" rx="10" ry="5" style="fill:green;stroke:purple;stroke-width:2" />
                            Sorry, your browser does not support inline SVG.
                        </svg>
                        break;
                }
            </CommandBarButton>
        </ItemTemplate>
    </CommandBar>
</Demo>

<em>@debugText</em>

@code {
    private System.Windows.Input.ICommand buttonCommand;
                        private string debugText;
                        private int commandCount = 0;

                        private List<CommandBarItem> items;
                        private List<CommandBarItem> farItems;
                        private List<CommandBarItem> overflowItems;

                        private List<CommandBarItem> customItems;
                        private List<CommandBarItem> contentItems;

                        private Action<ItemClickedArgs> OnClick => args =>
                        {
                            var item = farItems.FirstOrDefault(x => x.Key == args.Key);
                            if (item != null)
                            {
                                item.Checked = !item.Checked;
                            }
                        };

                        protected override Task OnInitializedAsync()
                        {
                            buttonCommand = new Utils.RelayCommand((p) =>
                            {
                                debugText = $"{p.ToString()} button was clicked. {commandCount++}";
                                StateHasChanged();
                            });

                            items = new List<CommandBarItem> {
            new CommandBarItem() { Text= "First", IconName="Home", Key="1", Command=buttonCommand, CommandParameter="First"},
            new CommandBarItem() {Text= "Second", IconName="Add", Key="2", Command=buttonCommand, CommandParameter="Second"},
            new CommandBarItem() {Text= "Third", IconName="Remove", Key="3", Command=buttonCommand, CommandParameter="Third"},
            new CommandBarItem() {Text= "Fourth", IconName="Save", Key="4", Command=buttonCommand, CommandParameter="Fourth"}
        };

                            overflowItems = new List<CommandBarItem> {
            new CommandBarItem() { Text= "First", IconName="Home", Key="9",  Command=buttonCommand, CommandParameter="First Overflow"},
            new CommandBarItem() {Text= "Second", IconName="Home", Key="10", Command=buttonCommand, CommandParameter="Second Overflow"},
            new CommandBarItem() {Text= "Third", IconName="Home", Key="11", Command=buttonCommand, CommandParameter="Third Overflow"},
            new CommandBarItem() {Text= "Fourth", IconName="Home", Key="12", Command=buttonCommand, CommandParameter="Fourth Overflow"}
        };

                            farItems = new List<CommandBarItem> {
            new CommandBarItem() { Text= "First", IconName="Info", Key="5",IconOnly=true, CanCheck=true, OnClick=@OnClick, Command=buttonCommand, CommandParameter="First Far"},
            new CommandBarItem() {Text= "Second", IconName="Share", Key="6",IconOnly=true, CanCheck=true, OnClick=@OnClick, Command=buttonCommand, CommandParameter="Second Far"},
            new CommandBarItem() {Text= "Third", IconName="Tiles", Key="7", IconOnly=true, CanCheck=true, OnClick=@OnClick, Command=buttonCommand, CommandParameter="Third Far"},
            new CommandBarItem() {Text= "Fourth", IconName="SortLines", Key="8",IconOnly=true, CanCheck=true, OnClick=@OnClick, Command=buttonCommand, CommandParameter="Fourth Far"}
        };

                            customItems = new List<CommandBarItem> {
            new CommandBarItem() { Text= "First", IconName="Home", Key="1", Command=buttonCommand, CommandParameter="First"},
            new CommandBarItem() { Text= "Second", IconName="Add", Key="2", Command=buttonCommand, CommandParameter="Second"},
            new CommandBarItem() { Key="divider1", ItemType = ContextualMenuItemType.Divider },
            new CommandBarItem() { Text= "Third", IconName="Remove", Key="3", Command=buttonCommand, CommandParameter="Third"},
            new CommandBarItem() { Text= "Fourth", IconName="Save", Key="4", Command=buttonCommand, CommandParameter="Fourth"}
        };

                            contentItems = new List<CommandBarItem> {
            new CommandBarItem() { Text= "First", IconName="Home", Key="1", Command=buttonCommand, CommandParameter="First"},
            new CommandBarItem() { Text= "Second", IconName="Add", Key="2", Command=buttonCommand, CommandParameter="Second"},
            new CommandBarItem() { Key="yellowCircle" },
            new CommandBarItem() { Key="greenCircle" },
            new CommandBarItem() { Text= "Third", IconName="Remove", Key="3", Command=buttonCommand, CommandParameter="Third"},
            new CommandBarItem() { Text= "Fourth", IconName="Save", Key="4", Command=buttonCommand, CommandParameter="Fourth"}
        };

                            return Task.CompletedTask;
                        }
                    }
