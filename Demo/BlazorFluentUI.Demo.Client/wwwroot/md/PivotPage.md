@page  "/pivotPage"

<h1>Pivot</h1>

<h4>Default Pivot</h4>
<BFUPivot AriaLabel="Basic Pivot Example">
    <BFUPivotItem HeaderText="My Files">
        <BFULabel Style="margin-top:10px">Pivot #1</BFULabel>
    </BFUPivotItem>
    <BFUPivotItem HeaderText="Recent">
        <BFULabel Style="margin-top:10px">Pivot #2</BFULabel>
    </BFUPivotItem>
    <BFUPivotItem HeaderText="Shared with me">
        <BFULabel Style="margin-top:10px">Pivot #3</BFULabel>
    </BFUPivotItem>
</BFUPivot>

<h4>Count and Icon</h4>

<div>
    <BFUPivot AriaLabel="Count and Icon Pivot Example">
        <BFUPivotItem HeaderText="My Files" ItemCount="42" IconName="Emoji2">
            <BFULabel Style="margin-top:10px">Pivot #1</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem ItemCount="23" IconName="Recent">
            <BFULabel Style="margin-top:10px">Pivot #2</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Placeholder" IconName="Globe">
            <BFULabel Style="margin-top:10px">Pivot #3</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Shared with me" IconName="Ringer" ItemCount="1">
            <BFULabel Style="margin-top:10px">Pivot #4</BFULabel>
        </BFUPivotItem>
    </BFUPivot>
</div>

<h4>Large link size</h4>
<div>
    <BFUPivot AriaLabel="Large Link Size Pivot Example" LinkSize=PivotLinkSize.Large>
        <BFUPivotItem HeaderText="My Files">
            <BFULabel>Pivot #1</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Recent">
            <BFULabel>Pivot #2</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Shared with me">
            <BFULabel>Pivot #3</BFULabel>
        </BFUPivotItem>
    </BFUPivot>
</div>

<h4>Links of tab style</h4>
<div>
    <BFUPivot AriaLabel="Links of Tab Style Pivot Example" LinkFormat=PivotLinkFormat.Tabs>
        <BFUPivotItem HeaderText="Foo">
            <BFULabel>Pivot #1</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Bar">
            <BFULabel>Pivot #2</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Bas">
            <BFULabel>Pivot #3</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Biz">
            <BFULabel>Pivot #4</BFULabel>
        </BFUPivotItem>
    </BFUPivot>
</div>

<h4>Links of large tab style</h4>
<div>
    <BFUPivot AriaLabel="Links of Large Tabs Pivot Example" LinkFormat=PivotLinkFormat.Tabs LinkSize=PivotLinkSize.Large>
        <BFUPivotItem HeaderText="Foo">
            <BFULabel>Pivot #1</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Bar">
            <BFULabel>Pivot #2</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Bas">
            <BFULabel>Pivot #3</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Biz">
            <BFULabel>Pivot #4</BFULabel>
        </BFUPivotItem>
    </BFUPivot>
</div>

<h4>Trigger onchange event</h4>
@if (!string.IsNullOrWhiteSpace(PivotItemKey))
{
    @($"{PivotItemKey} was clicked!")
}
<div>
    <BFUPivot AriaLabel="OnChange Pivot Example"
           LinkSize=PivotLinkSize.Large
           LinkFormat=PivotLinkFormat.Tabs
           OnLinkClick=OnLinkClick>
        <BFUPivotItem HeaderText="Foo">
            <BFULabel>Pivot #1</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Bar">
            <BFULabel>Pivot #2</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Bas">
            <BFULabel>Pivot #3</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Biz">
            <BFULabel>Pivot #4</BFULabel>
        </BFUPivotItem>
    </BFUPivot>
</div>

@*<h4>Show/Hide pivot item</h4>
<div>
    <Pivot AriaLabel="Remove Pivot Example" LinkSize=PivotLinkSize.Large LinkFormat=PivotLinkFormat.Tabs>
        @if (!hideItem)
        {
            <PivotItem HeaderText="Foo" ItemKey="Foo">
                <BFULabel>Click the button below to show/hide this pivot item.</BFULabel>
                <BFULabel>The selected item will not change when the number of pivot items changes.</BFULabel>
                <BFULabel>If the selected item was removed, the new first item will be selected.</BFULabel>
            </PivotItem>
        }
        <PivotItem HeaderText="Bar" ItemKey="Bar">
            <BFULabel>Pivot #2</BFULabel>
        </PivotItem>,
        <PivotItem HeaderText="Bas" ItemKey="Bas">
            <BFULabel>Pivot #3</BFULabel>
        </PivotItem>,
        <PivotItem HeaderText="Biz" ItemKey="Biz">
            <BFULabel>Pivot #4</BFULabel>
        </PivotItem>
    </Pivot>
    <div>
        <BFUDefaultButton AriaLive="AriaLive.Polite"
                       OnClick=@(() => hideItem = !hideItem)
                       Text=@($"{(hideItem ? "Hide" : "Show" )} First Pivot Item") />
    </div>
</div>*@

<h4>Override selected item</h4>
<div>
    <BFUPivot AriaLabel="Override Selected Item Pivot Example" @bind-SelectedKey=overrideSelectedKey>
        <BFUPivotItem HeaderText="My Files" ItemKey="0">
            <BFULabel>Pivot #1</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Recent" ItemKey="1">
            <BFULabel>Pivot #2</BFULabel>
        </BFUPivotItem>
        <BFUPivotItem HeaderText="Shared with me" ItemKey="2">
            <BFULabel>Pivot #3</BFULabel>
        </BFUPivotItem>
    </BFUPivot>
    <BFUDefaultButton OnClick=RotateSelection>Select next item</BFUDefaultButton>
</div>

<h4>Render content separately</h4>
<div>
    <div role="tabpanel"
         style="
            float: left;
            width: 100px;
            height: @(separateSelectedKey == "squareRed" ? "100px" : "200px");
            background: @(separateSelectedKey == "rectangleGreen" ? "green" : "red")" />
    <BFUPivot AriaLabel="Separately Rendered Content Pivot Example"
           @bind-SelectedKey=separateSelectedKey
           HeadersOnly=true>
        <BFUPivotItem HeaderText="Rectangle red" ItemKey="rectangleRed" />
        <BFUPivotItem HeaderText="Square red" ItemKey="squareRed" />
        <BFUPivotItem HeaderText="Rectangle green" ItemKey="rectangleGreen" />
    </BFUPivot>
</div>

@code{

    private string overrideSelectedKey = "0";
    private string separateSelectedKey;
    private string PivotItemKey;

    private void RotateSelection()
    {
        overrideSelectedKey = ((int.Parse(overrideSelectedKey) + 1) % 3).ToString();
    }

    private void OnLinkClick(BFUPivotItem item, MouseEventArgs ev)
    {
        PivotItemKey = item.HeaderText;
        StateHasChanged();

    }
}