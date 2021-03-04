@page "/calloutPage"

<h1>Callout</h1>

<BFUDropdown
          Style="max-width:300px;"
          ItemsSource=@options
          @bind-SelectedOption=@selectedOption />

<Demo Header="Callout" Key="0" MetadataPath="CalloutPage">

    <div style="height:200px;"></div>
    <div style="display:inline;overflow-x:auto;">
        <div style="width:300px"></div>
        <BFUDefaultButton Text="Show Callout" OnClick=ClickHandler @ref="calloutTarget" />
        <div style="width:300px"></div>
    </div>
    <div style="height:400px;"></div>
    @if (!calloutHidden)
    {
        <BFUCallout FabricComponentTarget=@calloutTarget
                    DirectionalHint=@((DirectionalHint)Enum.Parse(typeof(DirectionalHint),selectedOption.Key))
                    OnDismiss=@DismissHandler>
            <div Style="max-width:300px; padding:20px;">
                <h2>Callout Test</h2>
                <p style="min-width: 150px;">
                    There are a few things in here.
                </p>
                <BFUPrimaryButton Text="Ok" />
            </div>
        </BFUCallout>
    }

</Demo>

@code {

        bool isInitialized = false;
        bool calloutHidden = true;

        BFUComponentBase calloutTarget;

        List<IBFUDropdownOption> options;
        IBFUDropdownOption selectedOption;
        //string SelectedDirection = DirectionalHint.BottomLeftEdge.ToString();

    protected override Task OnInitializedAsync()
    {
        options = Enum.GetValues(typeof(DirectionalHint)).Cast<DirectionalHint>().Select(x => new BFUDropdownOption { Key = x.ToString(), Text = x.ToString() }).Cast<IBFUDropdownOption>().ToList();
        selectedOption = options.FirstOrDefault(x => x.Key == "BottomLeftEdge");

        return base.OnInitializedAsync();
    }

    Task ClickHandler(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        calloutHidden = !calloutHidden;
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected Task DismissHandler()
    {
        calloutHidden = true;
        return Task.CompletedTask;
    }
}
