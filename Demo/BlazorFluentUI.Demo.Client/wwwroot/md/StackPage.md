@page "/stackPage"

<header class="root">
    <h1 class="title">Stack</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                A <code>Stack</code> is a container-type component that abstracts the implementation of a flexbox in order to define the layout of its children components.
            </p>
            <h3 id="stack-properties">Stack Properties</h3>
            <p>
                Although the <code>Stack</code> component has a number of different properties, there are three in particular that define the overall layout that the component has:
            </p>
            <ol start="1">
                <li>Direction: Refers to whether the stacking of children components is horizontal or vertical. By default the <code>Stack</code> component is vertical, but can be turned horizontal by adding the <code>horizontal</code> property when using the component.</li>
                <li>Alignment: Refers to how the children components are aligned inside the container. This is controlled via the <code>verticalAlign</code> and <code>horizontalAlign</code> properties. One thing to notice here is that while flexbox containers align always across the cross axis, <code>Stack</code> aims to remove the mental strain involved in this process by making the <code>verticalAlign</code> and <code>horizontalAlign</code> properties always follow the vertical and horizontal axes, respectively, regardless of the direction of the <code>Stack</code>.</li>
                <li>Spacing: Refers to the space that exists between children components inside the <code>Stack</code>. This is controlled via the <code>gap</code> and <code>verticalGap</code> properties.</li>
            </ol>
            <h2 id="stack-items">Stack Items</h2>
            <p>
                The <code>Stack</code> component provides an abstraction of a flexbox container but there are some flexbox related properties that are applied on specific children of the flexbox instead of being applied on the container. This is where <code>Stack Items</code> comes into play.
            </p>
            <p>
                A <code>Stack Item</code> abstracts those properties that are or can be specifically applied on flexbox's children, like <code>grow</code> and <code>shrink</code>.
            </p>
            <p>
                To use a <code>Stack Item</code> in an application, the <code>Stack</code> component should be imported and <code>Stack.Item</code> should be used inside of a <code>Stack</code>. This is done so that the existence of the <code>Stack Item</code> is inherently linked to the <code>Stack</code> component.
            </p>
            <h3 id="stack-wrapping">Stack Wrapping</h3>
            <p>
                Aside from the previously mentioned properties, there is another property called <code>wrap</code> that determines if items overflow the <code>Stack</code> container or wrap around it. The wrap property only works in the direction of the <code>Stack</code>, which means that the children components can still overflow in the perpendicular direction (i.e. in a <code>Vertical Stack</code>, items might overflow horizontally and vice versa).
            </p>
            <h3 id="stack-nesting">Stack Nesting</h3>
            <p>
                <code>Stacks</code> can be nested inside one another in order to be able to configure the layout of the application as desired.
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
            <Stack>
                <Stack>
                    <StackItem Tokens=@(new StackItemTokens { Padding = 10 }) Align=@Alignment.End>
                        <span>Here's one</span>
                    </StackItem>
                    <StackItem>
                        <span>Here's two</span>
                    </StackItem>
                    <StackItem>
                        <span>Here's three</span>
                    </StackItem>
                    <StackItem>
                        <span>Here's four</span>
                    </StackItem>
                </Stack>

                <Stack Tokens=@(new StackTokens { ChildrenGap = new[] { 10.0 }, Padding="5px 2px" })>
                    <span>Here's one</span>
                    <span>Here's two</span>
                    <span>Here's three</span>
                    <span>Here's four</span>
                </Stack>

                <Stack Wrap="true" Tokens=@(new StackTokens { ChildrenGap = new[] { 10.0 }, Padding="5px 2px" })>
                    <span>Here's one</span>
                    <span>Here's two</span>
                    <span>Here's three</span>
                    <span>Here's four</span>
                </Stack>

                <Stack Horizontal="true" Tokens=@(new StackTokens { ChildrenGap = new[] { 10.0 }, Padding="5px 2px" })>
                    <span>Here's one</span>
                    <span>Here's two</span>
                    <span>Here's three</span>
                    <span>Here's four</span>
                </Stack>

                <Stack Horizontal="true" Style=@mainStyle Tokens=@tokens>
                    <StackItem Grow="1" Style=@itemStyle>
                        <span>Here's one</span>
                    </StackItem>
                    <StackItem Grow="3" Style=@itemStyle>
                        <span>Here's two</span>
                    </StackItem>
                    <StackItem Grow="3" Style=@itemStyle>
                        <span>Here's three</span>
                    </StackItem>
                    <StackItem Grow="1" Style=@itemStyle>
                        <span>Here's four</span>
                    </StackItem>
                </Stack>

            </Stack>
        </div>
    </div>
</div>
@code {
    //ToDo: Add Demo sections
    string mainStyle = "background:var(--palette-ThemeTertiary);";
    string itemStyle = "background:var(--palette-ThemePrimary);";
    StackTokens tokens = new StackTokens { ChildrenGap = new double[] { 4.0 }, Padding = 8.0 };
}
