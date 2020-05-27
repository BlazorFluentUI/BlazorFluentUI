@page "/navBarPage"
<h1>NavBar</h1>

<ul>
    <li>
        Avoid adding a link to BFUNavBarItems that contain MenuItems.  While the link will work in vertical mode, it will not work properly when in horizontal mode.
    </li>
    <li>
        When horizontal menu gets too long for screen, BFUNavBarItems will automatically be placed into the Overflow area.  You can place them into the Overflow area by default, too.
    </li>
</ul>

<Demo Header="Horizontal NavBar" Key="0" MetadataPath="NavBarPage">
    <BFUStack Style="width:100%;">
        @debugText
        <BFUNavBar Header="TestHeader" Direction="LayoutDirection.Horizontal" Items=@items >
            <FooterTemplate>
                <div>A footer area.</div>
            </FooterTemplate>
        </BFUNavBar>
    </BFUStack>
</Demo>

<Demo Header="Vertical NavBar" Key="0" MetadataPath="NavBarPage">
    <BFUStack Style="width:100%;">
        @debugText
        <BFUNavBar Header="TestHeader" Direction="LayoutDirection.Vertical" Items=@items>
            <FooterTemplate>
                <div>A footer area.</div>
            </FooterTemplate>
        </BFUNavBar>
    </BFUStack>
</Demo>


@code{
    private List<BFUNavBarItem> items;
    private string debugText;

    protected override Task OnInitializedAsync()
    {

        items = new List<BFUNavBarItem> {
            new BFUNavBarItem() { 
                Text= "First", 
                Url="navBarPage#test1", 
                NavMatchType= NavMatchType.AnchorOnly, 
                Id="test1", 
                IconName="Home", 
                Key="1"},
            new BFUNavBarItem() {
                Text= "Second",
                Id="test2",
                IconName="Add",
                Key="2",
                IsExpanded=true,
                Items = new List<BFUNavBarItem> {
                    new BFUNavBarItem
                    {
                        Text="SubFirst",
                        Id="subtest1",
                        IconName="Save",
                        Key="5",
                        Items = new List<BFUNavBarItem>
                        {
                            new BFUNavBarItem
                            {
                                Text="SubSubFirst",
                                Url="navBarPage#subsubtest1",
                                NavMatchType=NavMatchType.AnchorOnly,
                                Id="subsubtest1",
                                IconName="Home",
                                Key="7"
                            },
                            new BFUNavBarItem
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
                    new BFUNavBarItem
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
            new BFUNavBarItem() {Text= "Third", Url="navBarPage#test3", NavMatchType= NavMatchType.AnchorOnly, Id="test3", IconName="Remove", Key="3"},
            new BFUNavBarItem() {Text= "Fourth", Url="navBarPage#test4", NavMatchType= NavMatchType.AnchorOnly, Id="test4", IconName="Save", Key="4"}
        };


        return base.OnInitializedAsync();
    }
}