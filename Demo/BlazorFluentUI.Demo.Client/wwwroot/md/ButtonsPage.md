﻿@page "/buttonsPage"
@page "/"

<h1>Buttons</h1>
@buttonDebug

<BFUStack Tokens=@(new BFUStackTokens { ChildrenGap = new double[] { 5 } })>

    <BFUToggle @bind-Checked="Disabled" DefaultChecked="false" InlineLabel="true" Label="Disable buttons" />

    <BFUToggle @bind-Checked="Checked" DefaultChecked="false" InlineLabel="true" Label="Mark as Checked" />

</BFUStack>

<Demo Header="Default Button" Key="0" MetadataPath="ButtonsPage">
    <BFUStack Horizontal="true" Tokens=@(new BFUStackTokens() { ChildrenGap = new[] { 40.0 } })>
        <BFUDefaultButton Text="Standard" OnClick=@(() => OnClickHandler("Default Standard")) Disabled="@Disabled.GetValueOrDefault()" Checked="Checked" />
        <BFUPrimaryButton Text="Primary" OnClick=@(() => OnClickHandler("Default Primary")) Disabled="@Disabled.GetValueOrDefault()" Checked="Checked" />
    </BFUStack>
</Demo>

<Demo Header="Compound Button" Key="1" MetadataPath="ButtonsPage">
    <BFUStack Horizontal="true" Tokens=@(new BFUStackTokens() { ChildrenGap = new[] { 40.0 } })>
        <BFUCompoundButton Text="Standard" SecondaryText="This is the secondary text." OnClick=@(() => OnClickHandler("Compound Standard")) Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
        <BFUCompoundButton Text="Primary" Primary="true" SecondaryText="This is the secondary text." OnClick=@(() => OnClickHandler("Compound Primary")) Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
    </BFUStack>
</Demo>

<Demo Header="Command Bar Button" Key="2" MetadataPath="ButtonsPage">
    <BFUStack Horizontal="true" Style=@("height:44px")>
        <BFUCommandBarButton IconName="Add"
                             Text="New item"
                             MenuItems="MenuProperties"
                             Disabled="@Disabled.GetValueOrDefault()"
                             Checked="Checked" />
        <BFUCommandBarButton IconName="Mail" Text="Send mail" Disabled="@Disabled.GetValueOrDefault()" Checked="Checked" />
    </BFUStack>
</Demo>

<Demo Header="Split Button Button" Key="3" MetadataPath="ButtonsPage">
    <BFUStack Horizontal="true" Wrap="true" Tokens=@(new BFUStackTokens() { ChildrenGap = new[] { 40.0 } })>
        <BFUDefaultButton Text="Standard"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Standart Split"))
                          Disabled=@Disabled.GetValueOrDefault()
                          Checked=Checked />
        <BFUPrimaryButton Text="Primary"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Primary Split"))
                          Disabled=@Disabled.GetValueOrDefault()
                          Checked=Checked />
        <BFUDefaultButton Text="Main action disabled"
                          PrimaryDisabled="true"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Main Action Disabled Split"))
                          Disabled=@Disabled.GetValueOrDefault()
                          Checked=Checked />
        <BFUPrimaryButton Text="Disabled"
                          Disabled="true"
                          Split="true"
                          MenuItems="MenuProperties"
                          OnClick=@(() => OnClickHandler("Disabled Split"))
                          Checked=Checked />
    </BFUStack>
</Demo>

<Demo Header="Icon Button" Key="4" MetadataPath="ButtonsPage">
    <BFUStack Horizontal="true" Tokens=@(new BFUStackTokens() { ChildrenGap = new[] { 8.0 } })>
        <BFUIconButton OnClick=@(() => OnClickHandler("Icon")) IconName="Emoji2" AriaLabel="Emoji" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
        <BFUIconButton MenuItems="MenuProperties" IconName="Emoji2" AriaLabel="Emoji" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
    </BFUStack>
</Demo>

<Demo Header="Action Button" Key="5" MetadataPath="ButtonsPage">
    <BFUActionButton IconName="AddFriend" Disabled=@Disabled.GetValueOrDefault() Checked=Checked OnClick=@(() => OnClickHandler("Action"))>
        Create account
    </BFUActionButton>
</Demo>

<Demo Header="Command Button" Key="6" MetadataPath="ButtonsPage">
    <BFUCommandButton IconName="Add" Text="New item" MenuItems="MenuProperties" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
</Demo>

<Demo Header="Button-like Anchor" Key="7" MetadataPath="ButtonsPage">
    <BFUDefaultButton Text="BlazorFluentUI GitHub" Href="https://github.com/BlazorFluentUI/BlazorFluentUI" Disabled=@Disabled.GetValueOrDefault() Checked=Checked />
</Demo>

<Demo Header="Toggle Button" Key="8" MetadataPath="ButtonsPage">
    <BFUDefaultButton Toggle="true"
                      Checked=@(muted || Checked.GetValueOrDefault())
                      Text=@(muted ? "Volume muted" : "Volume unmuted" )
                      IconName=@(muted ? "Volume0" : "Volume3")
                      OnClick=Mute
                      Disabled=Disabled.GetValueOrDefault() />
</Demo>


<Demo Header="Using Command &amp; CommandParameter with Button" Key="9" MetadataPath="ButtonsPage">
    <BFUStack Tokens=@(new BFUStackTokens { ChildrenGap = new double[] { 5 } })>
        <BFUCheckbox Label="Enable Button" @bind-Checked="CommandEnabled" Style="max-width:200px;" />
        <BFUStackItem>
            <BFUPrimaryButton Command="buttonCommand" CommandParameter=@((Func<bool>)(() => CommandEnabled)) Text="Trigger ICommand" />
        </BFUStackItem>
        <div>
            Command Result: @buttonDebug
        </div>
    </BFUStack>
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

    IEnumerable<BFUContextualMenuItem> MenuProperties;

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
        MenuProperties = new System.Collections.Generic.List<BFUContextualMenuItem>()
{
            new BFUContextualMenuItem() { Key = "emailMessage", Text = "Email message", IconName="Mail"},
            new BFUContextualMenuItem() { Key = "calendarEvent", Text = "Calendar event", IconName="Calendar"}
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
