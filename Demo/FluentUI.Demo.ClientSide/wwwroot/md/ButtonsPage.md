@page "/buttonsPage"
@page "/"

<h1>Buttons</h1>
@buttonDebug

<Stack Tokens=@(new StackTokens { ChildrenGap = new double[] { 5 } })>

    <Toggle @bind-Checked="Disabled" DefaultChecked="false" InlineLabel="true" Label="Disable buttons" />

    <Toggle @bind-Checked="Checked" DefaultChecked="false" InlineLabel="true" Label="Mark as Checked" />

</Stack>

<Demo Header="Default Button" Key="0" MetadataPath="ButtonsPage">
    <Stack Horizontal="true" Tokens=@(new StackTokens() { ChildrenGap = new[] { 40.0 } })>
        <DefaultButton Text="Standard" OnClick=@(() => OnClickHandler("Default Standard")) Disabled="@Disabled.GetValueOrDefault()" Checked="Checked" />
        <PrimaryButton OnClick=@(() => OnClickHandler("Default Primary")) Disabled="@Disabled.GetValueOrDefault()" Checked="Checked">!Primary!</PrimaryButton>
    </Stack>
</Demo>

<Demo Header="Compound Button" Key="1" MetadataPath="ButtonsPage">
    <Stack Horizontal="true" Tokens=@(new StackTokens() { ChildrenGap = new[] { 40.0 } })>
        <CompoundButton Text="Standard" SecondaryText="This is the secondary text." OnClick=@(() => OnClickHandler("Compound Standard")) Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
        <CompoundButton Text="Primary" Primary="true" SecondaryText="This is the secondary text." OnClick=@(() => OnClickHandler("Compound Primary")) Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
    </Stack>
</Demo>

<Demo Header="Command Bar Button" Key="2" MetadataPath="ButtonsPage">
    <Stack Horizontal="true" Style=@("height:44px")>
        <CommandBarButton IconName="Add"
                             Text="New item"
                             MenuItems="MenuProperties"
                             Disabled="@Disabled.GetValueOrDefault()"
                             Checked="Checked" />
        <CommandBarButton IconName="Mail" Text="Send mail" Disabled="@Disabled.GetValueOrDefault()" Checked="Checked" />
    </Stack>
</Demo>

<Demo Header="Split Button Button" Key="3" MetadataPath="ButtonsPage">
    <Stack Horizontal="true" Wrap="true" Tokens=@(new StackTokens() { ChildrenGap = new[] { 40.0 } })>
        <DefaultButton Text="Standard"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Standart Split"))
                          Disabled=@Disabled.GetValueOrDefault()
                          Checked=Checked />
        <PrimaryButton Text="Primary"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Primary Split"))
                          Disabled=@Disabled.GetValueOrDefault()
                          Checked=Checked />
        <DefaultButton Text="Main action disabled"
                          PrimaryDisabled="true"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Main Action Disabled Split"))
                          Disabled=@Disabled.GetValueOrDefault()
                          Checked=Checked />
        <PrimaryButton Text="Disabled"
                          Disabled="true"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Disabled Split"))
                          Checked=Checked />
    </Stack>
</Demo>

<Demo Header="Icon Button" Key="4" MetadataPath="ButtonsPage">
    <Stack Horizontal="true" Tokens=@(new StackTokens() { ChildrenGap = new[] { 8.0 } })>
        <IconButton OnClick=@(() => OnClickHandler("Icon")) IconName="Emoji2" AriaLabel="Emoji" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
        <IconButton MenuItems="MenuProperties" IconName="Emoji2" AriaLabel="Emoji" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
    </Stack>
</Demo>

<Demo Header="Action Button" Key="5" MetadataPath="ButtonsPage">
    <ActionButton IconName="AddFriend" Disabled=@Disabled.GetValueOrDefault() Checked=Checked OnClick=@(() => OnClickHandler("Action"))>
        Create account
    </ActionButton>
</Demo>

<Demo Header="Command Button" Key="6" MetadataPath="ButtonsPage">
    <CommandButton IconName="Add" Text="New item" MenuItems="MenuProperties" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
</Demo>

<Demo Header="Button-like Anchor" Key="7" MetadataPath="ButtonsPage">
    <DefaultButton Text="FluentUI GitHub" Href="https://github.com/FluentUI/FluentUI" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
</Demo>

<Demo Header="Toggle Button" Key="8" MetadataPath="ButtonsPage">
    <DefaultButton Toggle="true"
                      Checked=@(muted || Checked.GetValueOrDefault())
                      Text=@(muted ? "Volume muted" : "Volume unmuted" )
                      IconName=@(muted ? "Volume0" : "Volume3")
                      OnClick=Mute
                      Disabled=Disabled.GetValueOrDefault() />
</Demo>


<Demo Header="Using Command &amp; CommandParameter with Button" Key="9" MetadataPath="ButtonsPage">
    <Stack Tokens=@(new StackTokens { ChildrenGap = new double[] { 5 } })>
        <Checkbox Label="Enable Button" @bind-Checked="CommandEnabled" Style="max-width:200px;" />
        <StackItem>
            <PrimaryButton Command="buttonCommand" CommandParameter=@((Func<bool>)(() => CommandEnabled)) Text="Trigger ICommand" />
        </StackItem>
        <div>
            Command Result: @buttonDebug
        </div>
    </Stack>
</Demo>

@code {

    Utils.RelayCommand buttonCommand;
    bool commandEnabled = false;
    bool CommandEnabled
    {
        get => commandEnabled;
        set
        {
            commandEnabled = value;
            buttonCommand.OnCanExecuteChanged();
        }
    }

    int commandCount = 0;
    bool isChecked = false;
    bool muted;
    string buttonDebug = "";
    bool? Checked;
    bool? Disabled;

    bool isToggled = false;

    IEnumerable<ContextualMenuItem> MenuProperties;

    private void OnClickHandler(string button)
    {
        buttonDebug = $"{button.ToString()} button was clicked. {commandCount++}";
    }

    private void Mute()
    {
        muted = !muted;
    }

    protected override Task OnInitializedAsync()
    {
        MenuProperties = new System.Collections.Generic.List<ContextualMenuItem>()
{
            new ContextualMenuItem() { Key = "emailMessage", Text = "Email message", IconName="Mail"},
            new ContextualMenuItem() { Key = "calendarEvent", Text = "Calendar event", IconName="Calendar"}
        };
        buttonCommand = new Utils.RelayCommand((p) =>
        {
            buttonDebug = $"Button with command was clicked. {commandCount++}";
            StateHasChanged();
        },
            p =>
            {
                return ((Func<bool>)p).Invoke();
            }
        );

        return base.OnInitializedAsync();
    }

}
