@page "/commandBarPage"

<header class="root">
    <h1 class="title">CommandBar</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                CommandBar is a surface that houses commands that operate on the content of
                the window, panel, or parent region it resides above. CommandBars are one of the most
                visible and recognizable ways to surface commands, and can be an intuitive
                method for interacting with content on the page; however, if overloaded or
                poorly organized, they can be difficult to use and hide valuable commands from
                your user. CommandBars can also display a search box for finding content, hold
                simple commands as well as menus, or display the status of ongoing actions.
            </p>
            <p>
                Commands should be sorted in order of importance, from left-to-right or right-to-left
                depending on the culture. Secondarily, organize commands in logical
                groupings for easier recall. CommandBars work best when they display no more
                than 5-7 commands. This helps users quickly find your most valuable features.
                If you need to show more commands, consider using the overflow menu. If you
                need to render status or viewing controls, these go on the right side of the
                CommandBar (or left side if in a left-to-right experience). Do not display
                more than 2-3 items on the right side as it will make the overall CommandBar
                difficult to parse.
            </p>
            <p>
                All command items should have an icon and a label. Commands can render as
                labels only as well. In smaller widths, commands can just use icon only, but
                only for the most recognizable and frequently used commands. All other
                commands should go into an overflow where text labels can be shown.
            </p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <em>@debugText</em>
        <div class="subSection">
            <Demo Header="Basic CommandBar" Key="0" MetadataPath="CommandBarPage">
                <CommandBar Items=@items />
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="CommandBar with right items" Key="1" MetadataPath="CommandBarPage">
                <CommandBar Items=@items FarItems=@farItems OverflowItems=@overflowItems />
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="CommandBar with custom item rendering" Key="2" MetadataPath="CommandBarPage">
                <CommandBar Items=@customItems>
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
        </div>

        <div class="subSection">
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
        </div>

        <div class="subSection">
            <Demo Header="CommandBar with RadioButtons" Key="4" MetadataPath="CommandBarPage">
                <CommandBar Items=@itemsWithRadioButtons />
            </Demo>
        </div>
    </div>
</div>

@code {
    private System.Windows.Input.ICommand? buttonCommand;
    private string? debugText;
    private int commandCount = 0;

    private List<CommandBarItem>? items;
    private List<CommandBarItem>? farItems;
    private List<CommandBarItem>? overflowItems;

    private List<CommandBarItem>? customItems;
    private List<CommandBarItem>? contentItems;

    private List<CommandBarItem>? itemsWithRadioButtons;

    private Action<ItemClickedArgs> OnClick => args =>
    {
        var item = farItems?.FirstOrDefault(x => x.Key == args.Key);
        if (item != null)
        {
            item.Checked = !item.Checked;
        }
    };

    protected override Task OnInitializedAsync()
    {
        buttonCommand = new RelayCommand((p) =>
        {
            debugText = $"{p?.ToString()} button was clicked. {commandCount++}";
            StateHasChanged();
        });

        items = new List<CommandBarItem> {
            new CommandBarItem() { Text= "First", IconName="Home", Key="1", Command=buttonCommand, CommandParameter="First"},
            new CommandBarItem() {Text= "Second", IconName="Add", Key="2", Command=buttonCommand, CommandParameter="Second"},
            new CommandBarItem() {Text= "Third", IconName="Remove", Key="3", Command=buttonCommand, CommandParameter="Third"},
            new CommandBarItem() {Text= "Fourth", IconName="Save", Key="4", Command=buttonCommand, CommandParameter="Fourth"},
            new CommandBarItem() {Text= "Link", IconName="Share", Key="5", Href="http://www.blazorfluentui.net"},
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
        itemsWithRadioButtons = new List<CommandBarItem>
{
            new CommandBarItem() { Text= "Non radio", IconName="Home", Key="1", Command=buttonCommand, CommandParameter="Non radio", Toggle=true},
            new CommandBarItem() { Text= "RadioA1", IconName="Add", Key="2", Command=buttonCommand, CommandParameter="RadioA1", IsRadioButton = true, GroupName = "firstRadioButtonGroup"},
            new CommandBarItem() { Text= "RadioA2", IconName="Add", Key="3", Command=buttonCommand, CommandParameter="RadioA2", IsRadioButton = true, GroupName = "firstRadioButtonGroup"},
            new CommandBarItem() { Text= "RadioA3", IconName="Add", Key="4", Command=buttonCommand, CommandParameter="RadioA3", IsRadioButton = true, GroupName = "firstRadioButtonGroup"},
            new CommandBarItem() { Text= "RadioB1", IconName="Remove", Key="5", Command=buttonCommand, CommandParameter="RadioB1", IsRadioButton = true, GroupName = "secondRadioButtonGroup"},
            new CommandBarItem() { Text= "RadioB2", IconName="Remove", Key="6", Command=buttonCommand, CommandParameter="RadioB2", IsRadioButton = true, GroupName = "secondRadioButtonGroup"},
            new CommandBarItem() { Text= "RadioB3", IconName="Remove", Key="7", Command=buttonCommand, CommandParameter="RadioB3", IsRadioButton = true, GroupName = "secondRadioButtonGroup"}
        };

        return Task.CompletedTask;
    }
}
