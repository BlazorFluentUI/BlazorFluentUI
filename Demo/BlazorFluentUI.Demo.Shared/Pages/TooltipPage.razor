@page "/tooltipPage"

<header class="root">
    <h1 class="title">TooltipPage</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A good tooltip briefly describes unlabeled controls or provides a bit of additional information about labeled controls, when this is useful. It can also help customers navigate the UI by offering additional—not redundant—information about control labels, icons, and links. A tooltip should always add valuable information; use sparingly.
            </p>
            <p>
                There are two components you can use to display a tooltip:
            </p>
            <ul>
                <li><strong>Tooltip:</strong> A styled tooltip that you can display on a chosen target.</li>
                <li><strong>TooltipHost:</strong> A wrapper that automatically shows a tooltip when the wrapped element is hovered or focused.</li>
            </ul>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <div class="subSection">
            <Demo Header="Default Tooltip" Key="0" MetadataPath="TooltipPage">
                <TooltipHost>
                    <TooltipContent></TooltipContent>
                    <ChildContent>
                        <DefaultButton>Hover to see a tooltip</DefaultButton>
                    </ChildContent>
                </TooltipHost>

                <TooltipHost GapSpace="10">
                    <TooltipContent>
                        Hey, look here!
                    </TooltipContent>
                    <ChildContent>
                        <DefaultButton>With gap</DefaultButton>
                    </ChildContent>
                </TooltipHost>

                <TooltipHost IsBeakVisible="false">
                    <TooltipContent>
                        Hey, look here!
                    </TooltipContent>
                    <ChildContent>
                        <DefaultButton>Without beak</DefaultButton>
                    </ChildContent>
                </TooltipHost>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Tooltip wrapping inline or inline-block elements" Key="1" MetadataPath="TooltipPage">
                <div>
                    In some cases when a TooltipHost is wrapping <code>inline-block</code> or <code>inline</code> elements, the
                    positioning of the Tooltip may be off. In these cases, it's recommended to set the TooltipHost's{' '}
                    <code>display</code> property to <code>inline-block</code>, as in the following example.
                    <br />
                    <br />
                    <TooltipHost GapSpace="0">
                        <TooltipContent>
                            Incorrect positioning
                        </TooltipContent>
                        <ChildContent>
                            <button style="padding:10px;">
                                Hover for incorrect positioning
                            </button>
                        </ChildContent>
                    </TooltipHost>
                    <TooltipHost Style=" display: inline-block;"
                                 GapSpace="0">
                        <TooltipContent>
                            Correct positioning
                        </TooltipContent>
                        <ChildContent>
                            <button style="padding:10px;">
                                Hover for correct positioning
                            </button>
                        </ChildContent>
                    </TooltipHost>
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Tooltip with custom content" Key="2" MetadataPath="TooltipPage">
                <TooltipHost Delay=TooltipDelay.Zero
                             DirectionalHint=DirectionalHint.BottomCenter
                             Style="display: inline-block;">
                    <TooltipContent>
                        <ul style="margin: 10px; padding: 0">
                            <li>1. One</li>
                            <li>2. Two</li>
                        </ul>
                    </TooltipContent>
                    <ChildContent>
                        <DefaultButton Text="Hover over me" />
                    </ChildContent>
                </TooltipHost>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Tooltip with a closing delay" Key="3" MetadataPath="TooltipPage">
                <TooltipHost CloseDelay="500"
                             GapSpace="0"
                             Style="display: inline-block;">
                    <TooltipContent>
                        This is the tooltip
                    </TooltipContent>
                    <ChildContent>
                        <DefaultButton Text="Interact with my tooltip" />
                    </ChildContent>
                </TooltipHost>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Tooltip only on overflow" Key="4" MetadataPath="TooltipPage">
                <div>
                    <Toggle Label="Force text to overflow" InlineLabel Checked=@shouldOverflow CheckedChanged="@OnOverFlowChange" />
                    <style>
                        .example {
                            margin-top: 20px;


                        }

                        .parent {
                            padding: 10px;
                            border: 2px dashed var(--palette-NeutralTertiary);


                        }

                        .overflow {
                            overflow: hidden;
                            text-overflow: ellipsis;
                            white-space: nowrap;
                            width: 200px;
                        }
                    </style>
                    @* Example of TooltipOverflowMode.Parent *@
                    <div class="example">
                        <Label>Show tooltip when parent's content overflows</Label>

                        @* This parent element will overflow *@
                        <div class="parent @(shouldOverflow ? "overflow" : "")">
                            <Label @ref=@parent>This is the parent element.</Label>
                            <TooltipHost OverflowMode=TooltipOverflowMode.Parent
                                         OnTooltipToggle=SetParentTooltipVisible
                                         Parent=@parent
                                         Style="display: block; padding: 10px; background-color: var(--palette-ThemeLighter)">
                                <TooltipContent>
                                    If the parent element's content overflows, hovering here will show a tooltip (anchored to the parent element).
                                </TooltipContent>
                                <ChildContent>
                                    This is the TooltipHost area.
                                    <span>@contentParent</span>
                                </ChildContent>
                            </TooltipHost>
                        </div>
                    </div>

                    @* Example of TooltipOverflowMode.Self *@
                    <div class="example" >
                        <Label>Show tooltip when TooltipHost's content overflows</Label>

                        <TooltipHost OverflowMode=TooltipOverflowMode.Self
                                     HostClassName="@(shouldOverflow ? "overflow" : "")"
                                     OnTooltipToggle=@SetParentTooltipVisible
                                     Style="display: block; padding: 10px; background-color: var(--palette-ThemeLighter)">
                            <TooltipContent>
                                @contentSelf
                            </TooltipContent>
                            <ChildContent>
                                This is the TooltipHost area. @contentSelf
                            </ChildContent>
                        </TooltipHost>
                    </div>
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Tooltip on absolutely-positioned element" Key="5" MetadataPath="TooltipPage">

            </Demo>
        </div>
    </div>
</div>
@code{

    bool shouldOverflow;
    bool isParentTooltipVisible;
    FluentUIComponentBase parent;

    string contentParent = "If the parent element's content overflows, hovering here will show a tooltip (anchored to the parent element).";
    string contentSelf = "If the TooltipHost's content overflows, hovering here will show a tooltip (anchored to the TooltipHost).";

    private void OnOverFlowChange()
    {
        shouldOverflow = !shouldOverflow;
    }

    private void SetParentTooltipVisible(bool state)
    {
        isParentTooltipVisible = state;
    }
}