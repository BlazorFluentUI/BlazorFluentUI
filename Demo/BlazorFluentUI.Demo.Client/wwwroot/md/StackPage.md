@page "/stackPage"

<h1>Stack</h1>

<BFUStack>
    <BFUStack>
        <BFUStackItem Tokens=@(new BFUStackItemTokens { Padding = 10 }) Align=@Alignment.End>
            <span>Here's one</span>
        </BFUStackItem>
        <BFUStackItem>
            <span>Here's two</span>
        </BFUStackItem>
        <BFUStackItem>
            <span>Here's three</span>
        </BFUStackItem>
        <BFUStackItem>
            <span>Here's four</span>
        </BFUStackItem>
    </BFUStack>

    <BFUStack Tokens=@(new BFUStackTokens { ChildrenGap = new[] { 10.0 }, Padding="5px 2px" })>
        <span>Here's one</span>
        <span>Here's two</span>
        <span>Here's three</span>
        <span>Here's four</span>
    </BFUStack>

    <BFUStack Wrap="true" Tokens=@(new BFUStackTokens { ChildrenGap = new[] { 10.0 }, Padding="5px 2px" })>
        <span>Here's one</span>
        <span>Here's two</span>
        <span>Here's three</span>
        <span>Here's four</span>
    </BFUStack>

    <BFUStack Horizontal="true" Tokens=@(new BFUStackTokens { ChildrenGap = new[] { 10.0 }, Padding="5px 2px" })>
        <span>Here's one</span>
        <span>Here's two</span>
        <span>Here's three</span>
        <span>Here's four</span>
    </BFUStack>

    <BFUStack Horizontal="true" Style=@mainStyle Tokens=@tokens>
        <BFUStackItem Grow="1" Style=@itemStyle>
            <span>Here's one</span>
        </BFUStackItem>
        <BFUStackItem Grow="3" Style=@itemStyle>
            <span>Here's two</span>
        </BFUStackItem>
        <BFUStackItem Grow="3" Style=@itemStyle>
            <span>Here's three</span>
        </BFUStackItem>
        <BFUStackItem Grow="1" Style=@itemStyle>
            <span>Here's four</span>
        </BFUStackItem>
    </BFUStack>

</BFUStack>

@code {
    string mainStyle = "background:var(--palette-ThemeTertiary);";
    string itemStyle = "background:var(--palette-ThemePrimary);";
    BFUStackTokens tokens = new BFUStackTokens { ChildrenGap = new double[] { 4.0 }, Padding = 8.0 };
}
