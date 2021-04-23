@page  "/pivotPage"

<header class="root">
    <h1 class="title">Pivot</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                The Pivot control and related tabs pattern are used for navigating frequently accessed, distinct content categories.
                Pivots allow for navigation between two or more content views and relies on text headers to articulate the different sections of content.
            </p>
            <ul>
                <li>Tapping on a pivot item header navigates to that header's section content.</li>
            </ul>
            <p>
                Tabs are a visual variant of Pivot that use a combination of icons and text or just icons to articulate section content.
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
            <Demo Header="Default Pivot" Key="0" MetadataPath="PivotPage">
                <Pivot AriaLabel="Basic Pivot Example">
                    <PivotItem HeaderText="My Files">
                        <Label Style="margin-top:10px">Pivot #1</Label>
                    </PivotItem>
                    <PivotItem HeaderText="Recent">
                        <Label Style="margin-top:10px">Pivot #2</Label>
                    </PivotItem>
                    <PivotItem HeaderText="Shared with me">
                        <Label Style="margin-top:10px">Pivot #3</Label>
                    </PivotItem>
                </Pivot>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Count nd Icon" Key="1" MetadataPath="PivotPage">
                <div>
                    <Pivot AriaLabel="Count and Icon Pivot Example">
                        <PivotItem HeaderText="My Files" ItemCount="42" IconName="Emoji2">
                            <Label Style="margin-top:10px">Pivot #1</Label>
                        </PivotItem>
                        <PivotItem ItemCount="23" IconName="Recent">
                            <Label Style="margin-top:10px">Pivot #2</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Placeholder" IconName="Globe">
                            <Label Style="margin-top:10px">Pivot #3</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Shared with me" IconName="Ringer" ItemCount="1">
                            <Label Style="margin-top:10px">Pivot #4</Label>
                        </PivotItem>
                    </Pivot>

                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Large link size" Key="2" MetadataPath="PivotPage">

                <div>
                    <Pivot AriaLabel="Large Link Size Pivot Example" LinkSize=PivotLinkSize.Large>
                        <PivotItem HeaderText="My Files">
                            <Label>Pivot #1</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Recent">
                            <Label>Pivot #2</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Shared with me">
                            <Label>Pivot #3</Label>
                        </PivotItem>
                    </Pivot>
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Links of tab style" Key="3" MetadataPath="PivotPage">
                <div>
                    <Pivot AriaLabel="Links of Tab Style Pivot Example" LinkFormat=PivotLinkFormat.Tabs>
                        <PivotItem HeaderText="Foo">
                            <Label>Pivot #1</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Bar">
                            <Label>Pivot #2</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Bas">
                            <Label>Pivot #3</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Biz">
                            <Label>Pivot #4</Label>
                        </PivotItem>
                    </Pivot>
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Links of large tab style" Key="4" MetadataPath="PivotPage">
                <div>
                    <Pivot AriaLabel="Links of Large Tabs Pivot Example" LinkFormat=PivotLinkFormat.Tabs LinkSize=PivotLinkSize.Large>
                        <PivotItem HeaderText="Foo">
                            <Label>Pivot #1</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Bar">
                            <Label>Pivot #2</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Bas">
                            <Label>Pivot #3</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Biz">
                            <Label>Pivot #4</Label>
                        </PivotItem>
                    </Pivot>
                </div>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Trigger onchange event" Key="5" MetadataPath="PivotPage">

                @if (!string.IsNullOrWhiteSpace(PivotItemKey))
                {
                    @($"{PivotItemKey} was clicked!")
                }
                <div>
                    <Pivot AriaLabel="OnChange Pivot Example"
                           LinkSize=PivotLinkSize.Large
                           LinkFormat=PivotLinkFormat.Tabs
                           OnLinkClick=OnLinkClick>
                        <PivotItem HeaderText="Foo">
                            <Label>Pivot #1</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Bar">
                            <Label>Pivot #2</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Bas">
                            <Label>Pivot #3</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Biz">
                            <Label>Pivot #4</Label>
                        </PivotItem>
                    </Pivot>
                </div>
            </Demo>
            @*<h4>Show/Hide pivot item</h4>
                <div>
                    <Pivot AriaLabel="Remove Pivot Example" LinkSize=PivotLinkSize.Large LinkFormat=PivotLinkFormat.Tabs>
                        @if (!hideItem)
                        {
                            <PivotItem HeaderText="Foo" ItemKey="Foo">
                                <Label>Click the button below to show/hide this pivot item.</Label>
                                <Label>The selected item will not change when the number of pivot items changes.</Label>
                                <Label>If the selected item was removed, the new first item will be selected.</Label>
                            </PivotItem>
                        }
                        <PivotItem HeaderText="Bar" ItemKey="Bar">
                            <Label>Pivot #2</Label>
                        </PivotItem>,
                        <PivotItem HeaderText="Bas" ItemKey="Bas">
                            <Label>Pivot #3</Label>
                        </PivotItem>,
                        <PivotItem HeaderText="Biz" ItemKey="Biz">
                            <Label>Pivot #4</Label>
                        </PivotItem>
                    </Pivot>
                    <div>
                        <DefaultButton AriaLive="AriaLive.Polite"
                                       OnClick=@(() => hideItem = !hideItem)
                                       Text=@($"{(hideItem ? "Hide" : "Show" )} First Pivot Item") />
                    </div>
                </div>*@
        </div>
        <div class="subSection">
            <Demo Header="Override selected item" Key="6" MetadataPath="PivotPage">
                <div>
                    <Pivot AriaLabel="Override Selected Item Pivot Example" @bind-SelectedKey=overrideSelectedKey>
                        <PivotItem HeaderText="My Files" ItemKey="0">
                            <Label>Pivot #1</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Recent" ItemKey="1">
                            <Label>Pivot #2</Label>
                        </PivotItem>
                        <PivotItem HeaderText="Shared with me" ItemKey="2">
                            <Label>Pivot #3</Label>
                        </PivotItem>
                    </Pivot>
                    <DefaultButton OnClick=RotateSelection>Select next item</DefaultButton>
                </div>
            </Demo>
        </div>
        <div class="subSection" style="height: 250px;">
            <Demo Header="Renden content separately" Key="7" MetadataPath="PivotPage">
                <div>
                    <div role="tabpanel"
                         style="
            float: left;
            width: 100px;
            height: @(separateSelectedKey == "squareRed" ? "100px" : "200px");
            background: @(separateSelectedKey == "rectangleGreen" ? "green" : "red")" />
                    <Pivot AriaLabel="Separately Rendered Content Pivot Example"
                           @bind-SelectedKey=separateSelectedKey
                           HeadersOnly=true>
                        <PivotItem HeaderText="Rectangle red" ItemKey="rectangleRed" />
                        <PivotItem HeaderText="Square red" ItemKey="squareRed" />
                        <PivotItem HeaderText="Rectangle green" ItemKey="rectangleGreen" />
                    </Pivot>
                </div>
            </Demo>
        </div>
    </div>
</div>

@code{

    private string overrideSelectedKey = "0";
    private string? separateSelectedKey;
    private string? PivotItemKey;

    private void RotateSelection()
    {
        overrideSelectedKey = ((int.Parse(overrideSelectedKey) + 1) % 3).ToString();
    }

    private void OnLinkClick(PivotItem item, MouseEventArgs ev)
    {
        PivotItemKey = item.HeaderText!;
        StateHasChanged();

    }
}