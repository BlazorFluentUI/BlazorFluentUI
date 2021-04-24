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
            <Demo Header="Tooltip" Key="0" MetadataPath="TooltipPage">
                <TooltipHost>
                    <TooltipContent>
                        Hey, look here!
                    </TooltipContent>
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
                
                <TooltipHost IsBreakVisible="false">
                    <TooltipContent>
                        Hey, look here!
                    </TooltipContent>
                    <ChildContent>
                        <DefaultButton>Without break</DefaultButton>
                    </ChildContent>
                </TooltipHost>
            </Demo>
        </div>
    </div>
</div>