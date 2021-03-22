@page  "/searchBoxPage"
<header class="root">
    <h1 class="title">SearchBox</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A search box (<code>SearchBox</code>) provides an input field for searching content within a site or app to find specific items.
            </p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>

        <div class="subSection">
            <Demo Header="Single select string type" Key="1" MetadataPath="SearchBoxPage">
                <SearchBox ProvideSuggestions="@((filter) => { return ProvideSuggestions(filter); })"
                           @bind-SelectedItem="SelectedItem" />
                @if (SelectedItem != null)
                {
                    <h4>Selected item</h4>
                    @SelectedItem
                }
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Multi select string type" Key="2" MetadataPath="SearchBoxPage">
                <SearchBox ProvideSuggestions="@((filter) => { return ProvideSuggestions(filter); })"
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
        </div>
        <div class="subSection">
            <Demo Header="Single select templated search suggestions" Key="3" MetadataPath="SearchBoxPage">
                <SearchBox ProvideSuggestions="@((filter) => { return ProvideSearchItemSuggestions(filter); })"
                           ProvideString="@((element) => { return ((SearchItem)element).Name; })">
                    <SearchItemTemplate>
                        <Persona Text=@(((SearchItem)context).Name)
                                 SecondaryText=@(((SearchItem)context).JobDescription)
                                 ImageUrl="personFace.jpg"
                                 Presence=@PersonaPresenceStatus.Online
                                 Size=@PersonaSize.Size48
                                 ShowInitialsUntilImageLoads="true" />
                    </SearchItemTemplate>
                </SearchBox>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Multi select templated search suggestions" Key="4" MetadataPath="SearchBoxPage">
                <SearchBox ProvideSuggestions="@((filter) => { return ProvideSearchItemSuggestions(filter); })"
                           ProvideString="@((element) => { return ((SearchItem)element).Name; })"
                           IsMultiSelect="true"
                           @bind-SelectedItems="SelectedPersons">
                    <SearchItemTemplate>
                        <Persona Text=@(((SearchItem)context).Name)
                                 SecondaryText=@(((SearchItem)context).JobDescription)
                                 ImageUrl="personFace.jpg"
                                 Presence=@PersonaPresenceStatus.Online
                                 Size=@PersonaSize.Size48
                                 ShowInitialsUntilImageLoads="true" />
                    </SearchItemTemplate>
                </SearchBox>
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
        </div>
        <div class="subSection">
            <Demo Header="Multi select templated search suggestions and templated selected items" Key="5" MetadataPath="SearchBoxPage">
                <SearchBox ProvideSuggestions="@((filter) => { return ProvideSearchItemSuggestions(filter); })"
                           ProvideString="@((element) => { return ((SearchItem)element).Name; })"
                           IsMultiSelect="true">
                    <SearchItemTemplate>
                        <Persona Text=@(((SearchItem)context).Name)
                                 SecondaryText=@(((SearchItem)context).JobDescription)
                                 ImageUrl="personFace.jpg"
                                 Presence=@PersonaPresenceStatus.Online
                                 Size=@PersonaSize.Size48
                                 ShowInitialsUntilImageLoads="true" />
                    </SearchItemTemplate>
                    <SelectedItemTemplate>
                        <div style="display:inline;color:red;font-style:italic">@context.Name</div>
                    </SelectedItemTemplate>
                </SearchBox>
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="'Tell me what you want to do' search box" Key="6" MetadataPath="SearchBoxPage">
                <div>Start with entering 'a' in order to find suggestions</div>
                <SearchBox Placeholder="Tell me what you want to do"
                           IconName="Lightbulb"
                           ProvideSuggestions="@((filter) => { return ProvideWhatYouWantToDoSuggestions(filter); })"
                           InputWidth="300">
                </SearchBox>
            </Demo>
        </div>

        <Modal IsOpen=@modalIsOpen
               OnDismiss="DismissHandler">
            <div>@modalText clicked!</div>
        </Modal>


    </div>
</div>

@code {

    List<string> allSuggestions = new List<string> { "Aahron", "Aalam", "Aamir", "Abad", "Abbas", "Abbott", "Aberham", "Baaron", "Backstere", "Baen", "Babet", "Bellamie", "Beltran", "Benn" };
    List<SearchItem> allSearchItemSuggestions;
    List<ContextualMenuItem> suggestedItems;

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


    IEnumerable<ContextualMenuItem> ProvideWhatYouWantToDoSuggestions(string filter)
    {
        if (filter == "")
        {
            return new List<ContextualMenuItem>();
        }
        if (suggestedItems == null)
        {
            Utils.RelayCommand buttonCommand = new Utils.RelayCommand((p) =>
            {
                modalText = (string)p;
                modalIsOpen = true;
                StateHasChanged();
            });


            suggestedItems = new List<ContextualMenuItem>();
            #region subItems
            var subSuggestions = new List<ContextualMenuItem>();
            subSuggestions.Add(new ContextualMenuItem() { Text = "Accept and Move to Next", IconName = "PageEdit", Command = buttonCommand, CommandParameter = "Accept and Move to Next" });
            subSuggestions.Add(new ContextualMenuItem() { Text = "Accept This Change", IconName = "PageEdit", Command = buttonCommand, CommandParameter = "Accept This Change" });
            subSuggestions.Add(new ContextualMenuItem() { Text = "Accept All Changes Shown", Command = buttonCommand, CommandParameter = "Accept All Changes Shown" });
            subSuggestions.Add(new ContextualMenuItem() { Text = "Accept All Changes", Command = buttonCommand, CommandParameter = "Accept All Changes" });
            subSuggestions.Add(new ContextualMenuItem() { Text = "Accept All Changes and Stop Tracking", Command = buttonCommand, CommandParameter = "Accept All Changes and Stop Tracking" });
            #endregion

            // FIXXXME subitems currently not working   suggestedItems.Add(new ContextualMenuItem() { Text = "Accept", IconName = "PageEdit", Items = subSuggestions, Command = buttonCommand, CommandParameter = "Accept" });
            suggestedItems.Add(new ContextualMenuItem() { Text = "Accept", IconName = "PageEdit", Command = buttonCommand, CommandParameter = "Accept" });
            suggestedItems.Add(new ContextualMenuItem() { Text = "Accent", IconName = "LocalLanguage", Command = buttonCommand, CommandParameter = "Accent" });
            suggestedItems.Add(new ContextualMenuItem() { Text = "Alt Text Pane", Command = buttonCommand, CommandParameter = "Alt Text Pane" });
            suggestedItems.Add(new ContextualMenuItem() { Text = "All Assistants", Command = buttonCommand, CommandParameter = "All Assistants" });
            suggestedItems.Add(new ContextualMenuItem() { Text = "AutoFit", IconName = "AutoFitContents", Command = buttonCommand, CommandParameter = "AutoFit" });

        }

        var filteredSuggestions = suggestedItems.FindAll(suggestion => suggestion.Text.ToLower().Contains(filter.ToLower()));
        return filteredSuggestions;
    }
    void DismissHandler(EventArgs args)
    {
        modalIsOpen = false;
    }
}
