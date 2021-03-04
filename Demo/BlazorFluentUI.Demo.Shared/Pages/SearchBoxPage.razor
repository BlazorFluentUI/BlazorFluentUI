@page  "/searchBoxPage"
<h1>SearchBox</h1>
<Demo Header="Single select string type" Key="1" MetadataPath="SearchBoxPage">
    <BFUSearchBox ProvideSuggestions="@((filter) => { return ProvideSuggestions(filter); })"
                           @bind-SelectedItem="SelectedItem" />
                @if (SelectedItem != null)
                {
                    <h4>Selected item</h4>
                    @SelectedItem
                }
</Demo>
<Demo Header="Multi select string type" Key="2" MetadataPath="SearchBoxPage">
    <BFUSearchBox ProvideSuggestions="@((filter) => { return ProvideSuggestions(filter); })"
                           IsMultiSelect="true"
                           @bind-SelectedItems="SelectedItems" />
                @if (SelectedItems != null)
                {
                    <h4>Selected items</h4>
                    <ul>
                        @foreach (var item in SelectedItems)
                        {
                            <li>@item</li>
                        }
                    </ul>
                }
</Demo>
<Demo Header="Single select templated search suggestions" Key="3" MetadataPath="SearchBoxPage">
    <BFUSearchBox ProvideSuggestions="@((filter) => { return ProvideSearchItemSuggestions(filter); })"
                    ProvideString="@((element) => { return ((SearchItem)element).Name; })">
        <SearchItemTemplate>
            <BFUPersona Text=@(((SearchItem)context).Name)
                        SecondaryText=@(((SearchItem)context).JobDescription)
                        ImageUrl="personFace.jpg"
                        Presence=@PersonaPresenceStatus.Online
                        Size=@PersonaSize.Size48
                        ShowInitialsUntilImageLoads="true" />
        </SearchItemTemplate>
    </BFUSearchBox>
</Demo>
<Demo Header="Multi select templated search suggestions" Key="4" MetadataPath="SearchBoxPage">
    <BFUSearchBox ProvideSuggestions="@((filter) => { return ProvideSearchItemSuggestions(filter); })"
                  ProvideString="@((element) => { return ((SearchItem)element).Name; })"
                           IsMultiSelect="true"
                           @bind-SelectedItems="SelectedPersons">
        <SearchItemTemplate>
            <BFUPersona Text=@(((SearchItem)context).Name)
                        SecondaryText=@(((SearchItem)context).JobDescription)
                        ImageUrl="personFace.jpg"
                        Presence=@PersonaPresenceStatus.Online
                        Size=@PersonaSize.Size48
                        ShowInitialsUntilImageLoads="true" />
        </SearchItemTemplate>
    </BFUSearchBox>
                @if (SelectedPersons != null)
                {
                    <h4>Selected persons</h4>
                    <ul>
                        @foreach (var item in SelectedPersons)
                        {
                        <li>@(item.Name)</li>
                        }
                    </ul>
                }
</Demo>
<Demo Header="Multi select templated search suggestions and templated selected items" Key="5" MetadataPath="SearchBoxPage">
    <BFUSearchBox ProvideSuggestions="@((filter) => { return ProvideSearchItemSuggestions(filter); })"
                  ProvideString="@((element) => { return ((SearchItem)element).Name; })"
                  IsMultiSelect="true">
        <SearchItemTemplate>
            <BFUPersona Text=@(((SearchItem)context).Name)
                        SecondaryText=@(((SearchItem)context).JobDescription)
                        ImageUrl="personFace.jpg"
                        Presence=@PersonaPresenceStatus.Online
                        Size=@PersonaSize.Size48
                        ShowInitialsUntilImageLoads="true" />
        </SearchItemTemplate>
        <SelectedItemTemplate>
            <div style="display:inline;color:red;font-style:italic">@context.Name</div>
        </SelectedItemTemplate>
    </BFUSearchBox>
</Demo>

<Demo Header="'Tell me what you want to do' search box" Key="6" MetadataPath="SearchBoxPage">
    <div>Start with entering 'a' in order to find suggestions</div>
    <BFUSearchBox Placeholder="Tell me what you want to do"
                  IconName="Lightbulb"
                  ProvideSuggestions="@((filter) => { return ProvideWhatYouWantToDoSuggestions(filter); })"
                  InputWidth="300">
    </BFUSearchBox>
</Demo>

<BFUModal IsOpen=@modalIsOpen
          OnDismiss="DismissHandler">
    <div>@modalText clicked!</div>
</BFUModal>


            @code {

                List<string> allSuggestions = new List<string> { "Aahron", "Aalam", "Aamir", "Abad", "Abbas", "Abbott", "Aberham", "Baaron", "Backstere", "Baen", "Babet", "Bellamie", "Beltran", "Benn" };
                List<SearchItem> allSearchItemSuggestions;
                List<BFUContextualMenuItem> suggestedItems;
    string SelectedItem { get; set; }
    ICollection<string> SelectedItems { get; set; }
    ICollection<SearchItem> SelectedPersons { get; set; }
                bool modalIsOpen;
                string modalText;
                IEnumerable<string> ProvideSuggestions(string filter)
                {
                   // System.Threading.Thread.Sleep(1000); // Test the non blocking behavior of the control
                    if (filter == "")
                    {
                        return new List<string>();
                    }
                    var filteredSuggestions = allSuggestions.FindAll(suggestion => suggestion.ToLower().Contains(filter.ToLower()));

                    return filteredSuggestions;
                }

                IEnumerable<SearchItem> ProvideSearchItemSuggestions(string filter)
                {
                    if (filter == "")
                    {
                        return new List<SearchItem>();
                    }
                    if (allSearchItemSuggestions == null)
                    {
                        allSearchItemSuggestions = new List<SearchItem>();
                        bool toggle = false;
                        foreach (string suggestion in allSuggestions)
                        {
                            allSearchItemSuggestions.Add(new SearchItem() { Name = suggestion, JobDescription = toggle ? "developer" : "manager" });
                            toggle = !toggle;
                        }
                    }

                    var filteredSuggestions = allSearchItemSuggestions.FindAll(suggestion => suggestion.Name.ToLower().Contains(filter.ToLower()));
                    return filteredSuggestions;

                }


                IEnumerable<BFUContextualMenuItem> ProvideWhatYouWantToDoSuggestions(string filter)
                {
                    if(filter == "")
                    {
                        return new List<BFUContextualMenuItem>();
                    }
                    if (suggestedItems == null)
                    {
                        Utils.RelayCommand buttonCommand = new Utils.RelayCommand((p) =>
                        {
                            modalText = (string)p;
                            modalIsOpen = true;
                            StateHasChanged();
                        });


                        suggestedItems = new List<BFUContextualMenuItem>();
                        #region subItems
                        var subSuggestions = new List<BFUContextualMenuItem>();
                        subSuggestions.Add(new BFUContextualMenuItem() { Text = "Accept and Move to Next", IconName = "PageEdit", Command = buttonCommand, CommandParameter = "Accept and Move to Next" });
                        subSuggestions.Add(new BFUContextualMenuItem() { Text = "Accept This Change", IconName = "PageEdit", Command = buttonCommand, CommandParameter = "Accept This Change" });
                        subSuggestions.Add(new BFUContextualMenuItem() { Text = "Accept All Changes Shown", Command = buttonCommand, CommandParameter = "Accept All Changes Shown" });
                        subSuggestions.Add(new BFUContextualMenuItem() { Text = "Accept All Changes", Command = buttonCommand, CommandParameter = "Accept All Changes" });
                        subSuggestions.Add(new BFUContextualMenuItem() { Text = "Accept All Changes and Stop Tracking", Command = buttonCommand, CommandParameter = "Accept All Changes and Stop Tracking" });
                        #endregion

                        // FIXXXME subitems currently not working   suggestedItems.Add(new BFUContextualMenuItem() { Text = "Accept", IconName = "PageEdit", Items = subSuggestions, Command = buttonCommand, CommandParameter = "Accept" });
                        suggestedItems.Add(new BFUContextualMenuItem() { Text = "Accept", IconName = "PageEdit", Command = buttonCommand, CommandParameter = "Accept" });
                        suggestedItems.Add(new BFUContextualMenuItem() { Text = "Accent", IconName = "LocalLanguage", Command = buttonCommand, CommandParameter = "Accent" });
                        suggestedItems.Add(new BFUContextualMenuItem() { Text = "Alt Text Pane", Command = buttonCommand, CommandParameter = "Alt Text Pane" });
                        suggestedItems.Add(new BFUContextualMenuItem() { Text = "All Assistants", Command = buttonCommand, CommandParameter = "All Assistants" });
                        suggestedItems.Add(new BFUContextualMenuItem() { Text = "AutoFit", IconName = "AutoFitContents", Command = buttonCommand, CommandParameter = "AutoFit" });

                    }

                    var filteredSuggestions = suggestedItems.FindAll(suggestion => suggestion.Text.ToLower().Contains(filter.ToLower()));
                    return filteredSuggestions;
                }
                void DismissHandler(EventArgs args)
                {
                    modalIsOpen = false;
                }



            }
