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
            <Demo Header="Bound Toggles" Key="0" MetadataPath="TogglePage">
                <Stack Tokens="@(new StackTokens() { ChildrenGap = new [] {10d}})">
                    <Label>This is a toggle using binding (controlled) which is now @(BoundChecked1 ? "on" :"off")</Label>
                    <Toggle @bind-Checked=@BoundChecked1 OnText="On" OffText="Off" />
                    <Toggle @bind-Checked=@BoundChecked2 OnText="On" OffText="Off" Label="This is a toggle using binding." />
                </Stack>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Basic Toggles" Key="1" MetadataPath="TogglePage">
                <Stack Tokens="@(new StackTokens() { ChildrenGap = new [] {10d}})">
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
                </Stack>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Toggles with Custom Labels" Key="2" MetadataPath="TogglePage">
                <Stack Tokens="@(new StackTokens() { ChildrenGap = new [] {10d}})">
                    <Toggle OnText="On" OffText="Off" CheckedChanged=@OnChecked>
                        <CustomLabel>
                                Custom label&nbsp;
                                <TooltipHost>
                                    <TooltipContent>
                                        Info tooltip
                                    </TooltipContent>
                                    <ChildContent>
                                        <Icon iconName="Info" aria-label="Info tooltip" />
                                    </ChildContent>
                                </TooltipHost>
                            </CustomLabel>
                    </Toggle>

                    <Toggle InlineLabel="true" OnText="On" OffText="Off" CheckedChanged=@OnChecked>
                        <CustomLabel>
                                Custom inline label&nbsp;
                                <TooltipHost>
                                    <TooltipContent>
                                        Info tooltip
                                    </TooltipContent>
                                    <ChildContent>
                                        <Icon iconName="Info" aria-label="Info tooltip" />
                                    </ChildContent>
                                </TooltipHost>
                        </CustomLabel>
                    </Toggle>
                </Stack>

            </Demo>
        </div>
    </div>
</div>

@code {
    private bool BoundChecked1 = true;

    private bool BoundChecked2 = false;

    Task OnChecked()
    {

        return Task.CompletedTask;
    }

}
