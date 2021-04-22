@page "/togglePage"

<header class="root">
    <h1 class="title">Toggle</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A toggle represents a physical switch that allows someone to choose between two mutually exclusive options.  For example, “On/Off”, “Show/Hide”. Choosing an option should produce an immediate result.
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
            <Demo Header="Toggles" Key="0" MetadataPath="TogglePage">
                <Toggle DefaultChecked="false" OnText="On!" OffText="Off!" Label="This is an uncontrolled toggle." />
                <Toggle DefaultChecked="false" Disabled="true" OnText="On!" OffText="Off!" Label="This is a disabled off toggle." />
                <Toggle DefaultChecked="true" Disabled="true" OnText="On!" OffText="Off!" Label="This is a disabled on toggle." />
                <Toggle Checked=@IsChecked CheckedChanged=@OnChecked OnText="On!" OffText="Off!" Label="This is a controlled toggle." />
                <Toggle DefaultChecked="false" OnText="On!" OffText="Off!" InlineLabel="true" Label="This is an inline toggle." />
                <Toggle @bind-Checked=@BoundChecked OnText="On!" OffText="Off!" Label="This is a toggle using binding." />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Basic Toggles" Key="1" MetadataPath="TogglePage">
                <Toggle Label="Enabled and checked" DefaultChecked="true" OnText="On" OffText="Off" CheckedChanged=@OnChecked />

                <Toggle Label="Enabled and unchecked" OnText="On" OffText="Off" CheckedChanged=@OnChecked />

                <Toggle Label="Disabled and checked" DefaultChecked="true" Disabled="true" OnText="On" OffText="Off" />

                <Toggle Label="Disabled and unchecked" Disabled="true" OnText="On" OffText="Off" />

                <Toggle Label="With inline label" InlineLabel="true" OnText="On" OffText="Off" CheckedChanged=@OnChecked />

                <Toggle Label="Disabled with inline label" InlineLabel="true" Disabled="true" OnText="On" OffText="Off" />

                <Toggle Label="With inline label and without OnText and OffText" InlineLabel="true" CheckedChanged=@OnChecked />

                <Toggle Label="Disabled with inline label and without OnText and OffText" InlineLabel="true" Disabled="true" />

                <Toggle Label="Enabled and checked (ARIA 1.0 compatible)"
                        DefaultChecked="true"
                        OnText="On"
                        OffText="Off"
                        CheckedChanged=@OnChecked
                        Role=ToggleRole.Checkbox />
                </Demo>
        </div>
    </div>
</div>

@code {
    private bool IsChecked = false;

    private bool BoundChecked = false;

    Task OnChecked(bool isChecked)
    {
        isChecked = !isChecked;
        return Task.CompletedTask;
    }

}
