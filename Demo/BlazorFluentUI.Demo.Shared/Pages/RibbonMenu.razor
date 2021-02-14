@page  "/ribbonMenuPage"
@using System.Collections.ObjectModel;
<h1>Ribbon Menu</h1>
<BFURibbonMenu ItemsSource=@Items>
   <ItemTemplate Context="tab">
       <BFURibbonTab ItemsSource=@tab.Groups HeaderText=@tab.Header>
           <ItemTemplate Context="groupData">
               <BFURibbonGroup ItemsSource=@groupData ItemTransform=@itemTransform>
                   <ItemTemplate Context="item">
                       <BFUTooltipHost>
                           <TooltipContent>
                               <div>@(((RibbonItem)item).Text)</div>             
                           </TooltipContent>
                           <ChildContent>
                               @switch (item)
                               {
                                   case BFUButtonViewModel buttonViewModel:
                                       <BFUCommandBarButton IconName=@buttonViewModel.IconName
                                                            IconSrc=@buttonViewModel.IconSrc
                                                            Command=@buttonViewModel.Command
                                                            CommandParameter=@buttonViewModel.CommandParameter
                                                            Toggle=@buttonViewModel.Toggle />
                                       break;
                                   case BFUDropDownViewModel dropDownViewModel:
                                       <BFUDropdown ItemsSource=@dropDownViewModel.DropdownOptions
                                                    Placeholder="Select an option"
                                                    @bind-SelectedOption=@dropDownViewModel.Selected
                                                    Style=@("display: inline-block;width:" + @dropDownViewModel.Width) />
                                       break;
                               }
                           </ChildContent>
                       </BFUTooltipHost>
                   </ItemTemplate>
               </BFURibbonGroup>
           </ItemTemplate>
       </BFURibbonTab>
   </ItemTemplate>
</BFURibbonMenu>
<h1>Ribbon Menu with backstage</h1>
@*<BFUToggle @bind-Checked=@ShowBackstage OnText="On!" OffText="Off!" Label="Backstage state" />*@
<BFURibbonMenu ItemsSource=@Items BackstageHeader="File" @bind-ShowBackstage=ShowBackstage>
    <ItemTemplate Context="tab">
        <BFURibbonTab ItemsSource=@tab.Groups HeaderText=@tab.Header>
            <ItemTemplate Context="groupData">
                <BFURibbonGroup ItemsSource=@groupData ItemTransform=@itemTransform>
                    <ItemTemplate Context="item">
                        <BFUTooltipHost>
                            <TooltipContent>
                                <div>@(((RibbonItem)item).Text)</div>
                            </TooltipContent>
                            <ChildContent>
                                @switch (item)
                                {
                                    case BFUButtonViewModel buttonViewModel:
                                        <BFUCommandBarButton IconName=@buttonViewModel.IconName
                                                             IconSrc=@buttonViewModel.IconSrc
                                                             Command=@buttonViewModel.Command
                                                             CommandParameter=@buttonViewModel.CommandParameter
                                                             Toggle=@buttonViewModel.Toggle 
                                                             IsRadioButton=@buttonViewModel.IsRadioButton
                                                             GroupName=@buttonViewModel.GroupName/>
                                        break;
                                    case BFUDropDownViewModel dropDownViewModel:
                                        <BFUDropdown ItemsSource=@dropDownViewModel.DropdownOptions
                                                     Placeholder="Select an option"
                                                     @bind-SelectedOption=@dropDownViewModel.Selected
                                                     Style=@("display: inline-block;width:" + @dropDownViewModel.Width) />
                                        break;
                                }
                            </ChildContent>
                        </BFUTooltipHost>
                    </ItemTemplate>
                </BFURibbonGroup>
            </ItemTemplate>
        </BFURibbonTab>
    </ItemTemplate>
    <Backstage>
        <div style="width: 900px;background-color: white;">
            <div style="width:150px">
                <BFUNavBar Direction="LayoutDirection.Vertical" Items=@backstageItems/>
            </div>
        </div>
    </Backstage>
</BFURibbonMenu>



@code{
    System.Collections.Generic.List<TabItem<GroupItem>> Items { get; set; } = new System.Collections.Generic.List<TabItem<GroupItem>>();

    private List<BFUNavBarItem> backstageItems;

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
        undoRedoGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Undo", IconName = "Undo"});
        undoRedoGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Redo", IconName = "Redo"});

        var clipBoardGroup = new GroupItem();
        clipBoardGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Cut", IconName = "Cut" });
        clipBoardGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Copy", IconName = "Copy" });
        clipBoardGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Paste", IconName = "Paste",Priority=10 });



        var formatGroup = new GroupItem();
        formatGroup.ItemsSource.Add(new BFUButtonViewModel() { Text="Underline", IconName="Underline", Toggle = true});
        formatGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Bolid", IconName = "Bold", Toggle = true });
        formatGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Italic", IconName = "Italic", Toggle = true });
        var fonts = new List<BFUDropdownOption>();
        fonts.Add(new BFUDropdownOption() { Text = "Calibri" });
        fonts.Add(new BFUDropdownOption() { Text = "Courier" });
        fonts.Add(new BFUDropdownOption() { Text = "Arial" });
        fonts.Add(new BFUDropdownOption() { Text = "Calibri" });
        formatGroup.ItemsSource.Add(new BFUDropDownViewModel() { DropdownOptions = fonts, Selected = fonts[1],Text="Selected font" });
        var sizes = new List<BFUDropdownOption>();
        sizes.Add(new BFUDropdownOption() { Text = "10" });
        sizes.Add(new BFUDropdownOption() { Text = "12" });
        sizes.Add(new BFUDropdownOption() { Text = "15" });
        sizes.Add(new BFUDropdownOption() { Text = "20" });
        sizes.Add(new BFUDropdownOption() { Text = "25" });
        formatGroup.ItemsSource.Add(new BFUDropDownViewModel() { DropdownOptions = sizes,Width="80px", Selected = sizes[1], Text = "Font size" });

        #region Paragraph group

        var paragraphGroup = new GroupItem();
        paragraphGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Align left", IconName = "AlignLeft", IsRadioButton = true, GroupName = "Align"});
        paragraphGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Center", IconName = "AlignCenter", IsRadioButton = true, GroupName = "Align" });
        paragraphGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Align right", IconName = "AlignRight", IsRadioButton = true, GroupName = "Align" });
        paragraphGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Justify", IconName = "AlignJustify", IsRadioButton = true, GroupName = "Align", Priority = 12 });
        paragraphGroup.ItemsSource.Add(new BFUButtonViewModel() { Text = "Sample image button", IconSrc = "sampleImage.jpg"}); //, IconName = "AlignJustify"
        #endregion


        var homeGroups = new System.Collections.ObjectModel.ObservableCollection<BlazorFluentUI.Models.IGroup>();
        homeGroups.Add(undoRedoGroup);
        homeGroups.Add(clipBoardGroup);
        homeGroups.Add(formatGroup);
        homeGroups.Add(paragraphGroup);
        Items.Add(new TabItem<GroupItem>() { Header = "Home", Groups = homeGroups });
        Items.Add(new TabItem<GroupItem>() { Header = "Insert"});
        Items.Add(new TabItem<GroupItem>() { Header = "Design"});


        #region backstage items

        backstageItems = new List<BFUNavBarItem> {
            new BFUNavBarItem() {NavMatchType= NavMatchType.AnchorOnly,IconName="Back",Key="1", Command=BackCommand},
            new BFUNavBarItem() {Text= "First",IconName="Add",Key="2"},
            new BFUNavBarItem() {Text= "Second", IconName="Remove", Key="3"},
            new BFUNavBarItem() {Text= "Third", IconName="Save", Key="4"}
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
            if (item is BFUButtonViewModel bFUButtonViewModel)
            {
                var menuItem = new BFUContextualMenuItem()
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