@page "/listPage"
@using System.Linq 

<h1>List</h1>

<Demo Key="0" Header="Basic Vertical List with selection" MetadataPath="listPage">
    <Stack Style="height:500px;">
        <div style="height:20px;">
            @DebugText
        </div>

        <Dropdown ItemsSource=@selectionModeOptions
                     @bind-SelectedOption=SelectedModeOption
                     Style="max-width:300px;">
        </Dropdown>

        <PrimaryButton Text="Add 10 items" OnClick="@ClickHandler" />
        <PrimaryButton Text="Add 5000 items" OnClick="@ClickHandler2" />

        <Label>Virtualized List with selection modes</Label>

        <div data-is-scrollable="true" style="overflow-y:auto;height:400px;">

            <SelectionZone SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), SelectedModeOption.Key))
                              DisableRenderOnSelectionChanged="true"
                              Selection=@selection
                              TItem="DataItem"
                              @ref="selectionZone">
                <FocusZone>
                    <List ItemsSource=@data
                             TItem="DataItem">
                        <ItemTemplate>

                            <div style="display:flex; flex-direction:row;"
                                 data-selection-index=@context.Index
                                 class=@($"ms-List-cell-default{(selection.IsKeySelected(context.Item.Key) ? " is-selected":"")}")
                                 data-is-focusable="true"
                                 @onclick=@(()=> {

                                            //selectionZone.HandleClick(context);
                                            DebugText = context.Item.Key + " clicked";
                                        })>
                                <img height="25" width="25" src=@context.Item.ImgUrl />
                                <em>This is item #@context.Item.Key</em>
                                <span style="margin-left:10px;">@context.Item.DisplayName</span>
                            </div>
                        </ItemTemplate>
                    </List>

                </FocusZone>
            </SelectionZone>
        </div>
    </Stack>
</Demo>

<Demo Key="1" Header="Basic Grid List with selection" MetadataPath="listPage">
    <Stack Style="height:500px;">
        <div style="height:20px;">
            @DebugText
        </div>

        <Dropdown ItemsSource=@selectionModeOptions
                     @bind-SelectedOption=SelectedModeOption
                     Style="max-width:300px;">
        </Dropdown>

        <PrimaryButton Text="Add 10 items" OnClick="@ClickHandler" />
        <PrimaryButton Text="Add 5000 items" OnClick="@ClickHandler2" />

        <Label>Virtualized List with selection modes</Label>

        <div data-is-scrollable="true" style="overflow-y:auto;height:400px;">

            <SelectionZone SelectionMode=@((SelectionMode)Enum.Parse(typeof(SelectionMode), SelectedModeOption.Key))
                              DisableRenderOnSelectionChanged="true"
                              Selection=@selection
                              TItem="DataItem"
                              @ref="selectionZone">
                <FocusZone>
                    <List ItemsSource=@data
                             UseGridFlexLayout="true"
                             ItemWidth="120"
                             TItem="DataItem">
                        <ItemTemplate>

                            <div style="display:flex; flex-direction:column; height:114px;width:114px;margin:3px;background-color:lightblue;overflow:hidden;"
                                 data-selection-index=@context.Index
                                 class=@($"{(selection.IsKeySelected(context.Item.Key) ? " is-selected":"")}")
                                 data-is-focusable="true"
                                 @onclick=@(()=> {

                                            //selectionZone.HandleClick(context);
                                            DebugText = context.Item.Key + " clicked";
                                        })>
                                <img height="25" width="25" src=@context.Item.ImgUrl />
                                <em>This is item #@context.Item.Key</em>
                                <span style="margin-left:10px;">@context.Item.DisplayName</span>
                            </div>
                        </ItemTemplate>
                    </List>

                </FocusZone>
            </SelectionZone>
        </div>
    </Stack>
</Demo>

@code {
    int count = 0;
    System.Collections.ObjectModel.RangeObservableCollection<DataItem> data;
    string DebugText = "";

    private IDropdownOption selectedModeOption;
    IDropdownOption SelectedModeOption { get => selectedModeOption; set { selectedModeOption = value; this.selection.SelectionMode = (SelectionMode)Enum.Parse(typeof(SelectionMode), value.Key); } }

    List<IDropdownOption> selectionModeOptions;

    Selection<DataItem> selection = new Selection<DataItem>();
    SelectionZone<DataItem> selectionZone;

    protected override Task OnInitializedAsync()
    {
        data = new System.Collections.ObjectModel.RangeObservableCollection<DataItem>();
        selection.GetKey = (item) => item.Key;
        selection.SetItems(data);
        selection.SelectionChanged.Subscribe(_ => InvokeAsync(StateHasChanged));

        selectionModeOptions = Enum.GetValues(typeof(SelectionMode)).Cast<SelectionMode>()
            .Select(x => new DropdownOption { Key = x.ToString(), Text = x.ToString() })
            .Cast<IDropdownOption>()
            .ToList();
        selectedModeOption = selectionModeOptions.FirstOrDefault(x => x.Key == "Single");

        return Task.CompletedTask;
    }


    Task ClickHandler(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        
        System.Diagnostics.Debug.WriteLine("Clicked!");
        for (var i = 0; i < 10; i++)
        {
            count++;
            data.Add(new DataItem(count));
        }
        selection.SetItems(data);
        System.Diagnostics.Debug.WriteLine($"List has {data.Count} items.");
        return Task.CompletedTask;
    }

    Task ClickHandler2(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        var tempData = new System.Collections.Generic.List<DataItem>();
        System.Diagnostics.Debug.WriteLine("Clicked!");
        for (var i = 0; i < 5000; i++)
        {
            count++;

            tempData.Add(new DataItem(count));
        }
        data.AddRange(tempData);
        selection.SetItems(data);
        //data = new System.Collections.ObjectModel.ObservableCollection<DataItem>(tempData);

        System.Diagnostics.Debug.WriteLine($"List has {data.Count} items.");
        return Task.CompletedTask;
    }

    Task ClickHandler3(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        System.Diagnostics.Debug.WriteLine("Clicked!");
        for (var i = 0; i < 2; i++)
        {
            count++;
            data.Add(new DataItem(count));
        }
        selection.SetItems(data);
        System.Diagnostics.Debug.WriteLine($"List has {data.Count} items.");
        return Task.CompletedTask;
    }



}
