@page  "/pivotPage"

<h1>Pivot</h1>

<h4>Default Pivot</h4>
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

<h4>Count and Icon</h4>

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

<h4>Large link size</h4>
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

<h4>Links of tab style</h4>
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

<h4>Links of large tab style</h4>
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

<h4>Trigger onchange event</h4>
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

<h4>Override selected item</h4>
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

<h4>Render content separately</h4>
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

@code{

    private string overrideSelectedKey = "0";
    private string separateSelectedKey;
    private string PivotItemKey;

    private void RotateSelection()
    {
        overrideSelectedKey = ((int.Parse(overrideSelectedKey) + 1) % 3).ToString();
    }

    private void OnLinkClick(PivotItem item, MouseEventArgs ev)
    {
        PivotItemKey = item.HeaderText;
        StateHasChanged();

    }
}