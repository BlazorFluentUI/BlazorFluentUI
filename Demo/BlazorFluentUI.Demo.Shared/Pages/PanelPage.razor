@page  "/panelPage"

<h1>Panel</h1>

<BFUDropdown ItemsSource=@panelTypes
             MultiSelect="false"
             @bind-SelectedOption=@panelTypeSelected
             Style="width:300px;" />
<BFUTextField @bind-Value=@customWidth
              Style="width:300px;"
              Label="Custom Width (number only! valid only when Custom panel type selected)"/>

<BFUPrimaryButton Text="Open Panel" OnClick=@Click />

<BFUPrimaryButton Text="Open Light Dismiss Panel" OnClick=@(() => lightDismissPanelOpen=true ) />

<BFUPrimaryButton Text="Open Custom Nav Panel" OnClick=@(() => customNavPanelOpen=true ) />


<BFUPanel IsOpen=@isOpen
       OnDismiss=@PanelDismiss
       Type=@((PanelType)Enum.Parse(typeof(PanelType),panelTypeSelected.Key))
       IsLightDismiss="false"
          CustomWidth=@customWidth
          
       HeaderText="Panel Example">
    <p>
        Hey, there's some content in here.
        <BFUTextField Description="Can I type here?" />

        <BlazorFluentUI.BFULabel>Selected: @string.Join(", ", controlledMultiSelectionResult.Select(x => x.Text))</BlazorFluentUI.BFULabel>
        <BFUDropdown ItemsSource=@items
                     MultiSelect="true"
                     Placeholder="Select options..."
                     @bind-SelectedOptions=@controlledMultiSelectionResult
                     Style="width:220px;" />
    </p>
</BFUPanel>

<BFUPanel IsOpen=@lightDismissPanelOpen
       OnDismiss=@(() => lightDismissPanelOpen = !lightDismissPanelOpen)
       Type=@((PanelType)Enum.Parse(typeof(PanelType),panelTypeSelected.Key))
          CustomWidth=@customWidth
       IsLightDismiss="true"
       HeaderText="Light Dismiss Panel">
    <p>
        This is a light dismiss panel...
    </p>
</BFUPanel>

<BFUPanel IsOpen=@customNavPanelOpen
       OnDismiss=@(() => customNavPanelOpen = !customNavPanelOpen)
      Type=@((PanelType)Enum.Parse(typeof(PanelType),panelTypeSelected.Key))
          CustomWidth=@customWidth
       IsLightDismiss="false"
       HeaderText="Custom Nav Panel">
    <NavigationTemplate>
        <BFUIconButton OnClick=@(() => customNavPanelOpen = false)
                    IconName="GlobalNavButton" />
    </NavigationTemplate>
    <ChildContent>
        <p>
            This is a panel with a completely custom nav area (at the top)...
        </p>
    </ChildContent>
</BFUPanel>

@code {

    bool isOpen = false;
    bool lightDismissPanelOpen = false;
    bool customNavPanelOpen = false;

    IBFUDropdownOption panelTypeSelected;
    List<IBFUDropdownOption> panelTypes;
    string customWidth = "500";


    IEnumerable<IBFUDropdownOption> controlledMultiSelectionResult;
    List<IBFUDropdownOption> items;

    Task Click(MouseEventArgs args)
    {
        isOpen = true;

        return Task.CompletedTask;
    }

    Task PanelDismiss()
    {
        System.Diagnostics.Debug.WriteLine("Called PanelDismiss from page");
        isOpen = false;
        return Task.CompletedTask;
    }

    protected override Task OnInitializedAsync()
    {
        panelTypes = Enum.GetNames(typeof(PanelType)).Select(x => new BFUDropdownOption { Key = x, Text = x, ItemType = SelectableOptionMenuItemType.Normal }).Cast<IBFUDropdownOption>().ToList();
        panelTypeSelected = panelTypes.First(x => x.Key == "Medium");

        items = new List<DataItem>
        {
            new DataItem("Fruits", SelectableOptionMenuItemType.Header),
            new DataItem("Apple"),
            new DataItem("Banana"),
            new DataItem("Orange"),
            new DataItem("Grape"),
            new DataItem("divider1", SelectableOptionMenuItemType.Divider),
            new DataItem("Vegetables", SelectableOptionMenuItemType.Header),
            new DataItem("Broccoli"),
            new DataItem("Carrot"),
            new DataItem("Lettuce")
        }.Select(x => new BFUDropdownOption { Key = x.Key, Text = x.DisplayName, ItemType = x.Type }).Cast<IBFUDropdownOption>().ToList();

        controlledMultiSelectionResult = items.Where(x => x.Key == "Banana" || x.Key == "Orange");

        return base.OnInitializedAsync();
    }

}
