@page "/navBarPage"
<h1>NavBar</h1>

<ul>
    <li>
        Avoid adding a link to NavBarItems that contain MenuItems.  While the link will work in vertical mode, it will not work properly when in horizontal mode.
    </li>
    <li>
        When horizontal menu gets too long for screen, NavBarItems will automatically be placed into the Overflow area.  You can place them into the Overflow area by default, too.
    </li>
</ul>

<Demo Header="Horizontal NavBar" Key="0" MetadataPath="NavBarPage">
    <Stack Style="width:100%;">
        @debugText
        <NavBar Header="TestHeader" Direction="LayoutDirection.Horizontal" Items=@items >
            <FooterTemplate>
                <div>A footer area.</div>
            </FooterTemplate>
        </NavBar>
    </Stack>
</Demo>

<Demo Header="Vertical NavBar" Key="0" MetadataPath="NavBarPage">
    <Stack Style="width:100%;">
        @debugText
        <NavBar Header="TestHeader" Direction="LayoutDirection.Vertical" Items=@items>
            <FooterTemplate>
                <div>A footer area.</div>
            </FooterTemplate>
        </NavBar>
    </Stack>
</Demo>


@code{
    private List<NavBarItem> items;
    private string debugText;

    protected override Task OnInitializedAsync()
    {

        items = new List<NavBarItem> {
            new NavBarItem() { 
                Text= "First", 
                Url="navBarPage#test1", 
                NavMatchType= NavMatchType.AnchorOnly, 
                Id="test1", 
                IconName="Home", 
                Key="1"},
            new NavBarItem() {
                Text= "Second",
                Id="test2",
                IconName="Add",
                Key="2",
                IsExpanded=true,
                Items = new List<NavBarItem> {
                    new NavBarItem
                    {
                        Text="SubFirst",
                        Id="subtest1",
                        IconName="Save",
                        Key="5",
                        Items = new List<NavBarItem>
                        {
                            new NavBarItem
                            {
                                Text="SubSubFirst",
                                Url="navBarPage#subsubtest1",
                                NavMatchType=NavMatchType.AnchorOnly,
                                Id="subsubtest1",
                                IconName="Home",
                                Key="7"
                            },
                            new NavBarItem
                            {
                                Text="SubSubSecond",
                                Url="navBarPage#subsubtest2",
                                NavMatchType=NavMatchType.AnchorOnly,
                                Id="subsubtest2",
                                IconName="Add",
                                Key="8"
                            }
                        }
                    },
                    new NavBarItem
                    {
                        Text="SubSecond",
                        Url="navBarPage#subtest2",
                        NavMatchType=NavMatchType.AnchorOnly,
                        Id="subtest2",
                        IconName="Save",
                        Key="6"
                    }
                }
            },
            new NavBarItem() {Text= "Third", Url="navBarPage#test3", NavMatchType= NavMatchType.AnchorOnly, Id="test3", IconName="Remove", Key="3"},
            new NavBarItem() {Text= "Fourth", Url="navBarPage#test4", NavMatchType= NavMatchType.AnchorOnly, Id="test4", IconName="Save", Key="4"}
        };


        return base.OnInitializedAsync();
    }
}