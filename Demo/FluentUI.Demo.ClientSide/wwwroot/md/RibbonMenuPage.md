@page  "/ribbonMenuPage"
@using System.Collections.ObjectModel;
<h1>Ribbon Menu</h1>
<RibbonMenu ItemsSource=@Items>
   <ItemTemplate Context="tab">
       <RibbonTab ItemsSource=@tab.Groups HeaderText=@tab.Header>
           <ItemTemplate Context="groupData">
               <RibbonGroup ItemsSource=@groupData ItemTransform=@itemTransform>
                   <ItemTemplate Context="item">
                       <TooltipHost>
                           <TooltipContent>
                               <div>@(((RibbonItem)item).Text)</div>             
                           </TooltipContent>
                           <ChildContent>
                               @switch (item)
                               {
                                   case ButtonViewModel buttonViewModel:
                                       <CommandBarButton IconName=@buttonViewModel.IconName
                                                            IconSrc=@buttonViewModel.IconSrc
                                                            Command=@buttonViewModel.Command
                                                            CommandParameter=@buttonViewModel.CommandParameter
                                                            Toggle=@buttonViewModel.Toggle />
                                       break;
                                   case DropDownViewModel dropDownViewModel:
                                       <Dropdown ItemsSource=@dropDownViewModel.DropdownOptions
                                                    Placeholder="Select an option"
                                                    @bind-SelectedOption=@dropDownViewModel.Selected
                                                    Style=@("display: inline-block;width:" + @dropDownViewModel.Width) />
                                       break;
                               }
                           </ChildContent>
                       </TooltipHost>
                   </ItemTemplate>
               </RibbonGroup>
           </ItemTemplate>
       </RibbonTab>
   </ItemTemplate>
</RibbonMenu>
<h1>Ribbon Menu with backstage</h1>
@*<Toggle @bind-Checked=@ShowBackstage OnText="On!" OffText="Off!" Label="Backstage state" />*@
<RibbonMenu ItemsSource=@Items BackstageHeader="File" @bind-ShowBackstage=ShowBackstage>
    <ItemTemplate Context="tab">
        <RibbonTab ItemsSource=@tab.Groups HeaderText=@tab.Header>
            <ItemTemplate Context="groupData">
                <RibbonGroup ItemsSource=@groupData ItemTransform=@itemTransform>
                    <ItemTemplate Context="item">
                        <TooltipHost>
                            <TooltipContent>
                                <div>@(((RibbonItem)item).Text)</div>
                            </TooltipContent>
                            <ChildContent>
                                @switch (item)
                                {
                                    case ButtonViewModel buttonViewModel:
                                        <CommandBarButton IconName=@buttonViewModel.IconName
                                                             IconSrc=@buttonViewModel.IconSrc
                                                             Command=@buttonViewModel.Command
                                                             CommandParameter=@buttonViewModel.CommandParameter
                                                             Toggle=@buttonViewModel.Toggle />
                                        break;
                                    case DropDownViewModel dropDownViewModel:
                                        <Dropdown ItemsSource=@dropDownViewModel.DropdownOptions
                                                     Placeholder="Select an option"
                                                     @bind-SelectedOption=@dropDownViewModel.Selected
                                                     Style=@("display: inline-block;width:" + @dropDownViewModel.Width) />
                                        break;
                                }
                            </ChildContent>
                        </TooltipHost>
                    </ItemTemplate>
                </RibbonGroup>
            </ItemTemplate>
        </RibbonTab>
    </ItemTemplate>
    <Backstage>
        <div style="width: 900px;background-color: white;">
            <div style="width:150px">
                <NavBar Direction="LayoutDirection.Vertical" Items=@backstageItems/>
            </div>
        </div>
    </Backstage>
</RibbonMenu>



@code{
    System.Collections.Generic.List<TabItem<GroupItem>> Items { get; set; } = new System.Collections.Generic.List<TabItem<GroupItem>>();

    private List<NavBarItem> backstageItems;

    [Parameter] public bool ShowBackstage { get; set; }
    [Parameter] public EventCallback<bool> ShowBackstageChanged { get; set; }

    System.Windows.Input.ICommand BackCommand { get; set; }
    void BackAction(object param)
    {
        ShowBackstage = false;
        ShowBackstageChanged.InvokeAsync(false);
        StateHasChanged();
    }


    protected override void OnInitialized()
    {
        BackCommand = new RelayCommand(BackAction);

        var undoRedoGroup = new GroupItem();
        undoRedoGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Undo", IconName = "Undo"});
        undoRedoGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Redo", IconName = "Redo"});

        var clipBoardGroup = new GroupItem();
        clipBoardGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Cut", IconName = "Cut" });
        clipBoardGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Copy", IconName = "Copy" });
        clipBoardGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Paste", IconName = "Paste",Priority=10 });



        var formatGroup = new GroupItem();
        formatGroup.ItemsSource.Add(new ButtonViewModel() { Text="Underline", IconName="Underline", Toggle = true});
        formatGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Bolid", IconName = "Bold", Toggle = true });
        formatGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Italic", IconName = "Italic", Toggle = true });
        var fonts = new List<DropdownOption>();
        fonts.Add(new DropdownOption() { Text = "Calibri" });
        fonts.Add(new DropdownOption() { Text = "Courier" });
        fonts.Add(new DropdownOption() { Text = "Arial" });
        fonts.Add(new DropdownOption() { Text = "Calibri" });
        formatGroup.ItemsSource.Add(new DropDownViewModel() { DropdownOptions = fonts, Selected = fonts[1],Text="Selected font" });
        var sizes = new List<DropdownOption>();
        sizes.Add(new DropdownOption() { Text = "10" });
        sizes.Add(new DropdownOption() { Text = "12" });
        sizes.Add(new DropdownOption() { Text = "15" });
        sizes.Add(new DropdownOption() { Text = "20" });
        sizes.Add(new DropdownOption() { Text = "25" });
        formatGroup.ItemsSource.Add(new DropDownViewModel() { DropdownOptions = sizes,Width="80px", Selected = sizes[1], Text = "Font size" });

        #region Paragraph group

        var paragraphGroup = new GroupItem();
        paragraphGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Align left", IconName = "AlignLeft", Toggle = true });
        paragraphGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Center", IconName = "AlignCenter", Toggle = true });
        paragraphGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Align right", IconName = "AlignRight", Toggle = true });
        paragraphGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Justify", IconName = "AlignJustify", Toggle = true, Priority = 12 });
        paragraphGroup.ItemsSource.Add(new ButtonViewModel() { Text = "Sample image button", IconSrc = "sampleImage.jpg"}); //, IconName = "AlignJustify"
        #endregion


        var homeGroups = new System.Collections.ObjectModel.ObservableCollection<FluentUI.Models.IGroup>();
        homeGroups.Add(undoRedoGroup);
        homeGroups.Add(clipBoardGroup);
        homeGroups.Add(formatGroup);
        homeGroups.Add(paragraphGroup);
        Items.Add(new TabItem<GroupItem>() { Header = "Home", Groups = homeGroups });
        Items.Add(new TabItem<GroupItem>() { Header = "Insert"});
        Items.Add(new TabItem<GroupItem>() { Header = "Design"});


        #region backstage items

        backstageItems = new List<NavBarItem> {
            new NavBarItem() {NavMatchType= NavMatchType.AnchorOnly,IconName="Back",Key="1", Command=BackCommand},
            new NavBarItem() {Text= "First",IconName="Add",Key="2"},
            new NavBarItem() {Text= "Second", IconName="Remove", Key="3"},
            new NavBarItem() {Text= "Third", IconName="Save", Key="4"}
        };

        #endregion

        base.OnInitialized();
    }



    Func<IEnumerable<object>, IEnumerable<object>> itemTransform => (items) =>
    {
        var menuItems = new List<object>();
        if (items == null)
            return menuItems;
        foreach (var item in items)
        {
            if (item is ButtonViewModel bFUButtonViewModel)
            {
                var menuItem = new ContextualMenuItem()
                {
                    Key = "2",
                    Text = bFUButtonViewModel.Text,
                    IconName = bFUButtonViewModel.IconName,
                    IconSrc = bFUButtonViewModel.IconSrc
                };
                menuItems.Add(menuItem);
            }
            else
            {
                menuItems.Add(item);
            }
        }
        return menuItems;

    };
}