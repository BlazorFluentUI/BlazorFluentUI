@page "/textPage"

<header class="root">
    <h1 class="title">Text</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                Text is a component for displaying text. You can use Text to standardize text across your web app.
            </p>
            <p>
                You can specify the <code>variant</code> prop to apply font styles to Text. This variant pulls from the Fluent UI React theme loaded on the page. If you do not specify the <code>variant</code> prop, by default, Text applies the styling from specifying the <code>variant</code> value to <code>medium</code>.
            </p>
            <p>
                The Text control is inline wrap by default. You can specify <code>block</code> to enable block and <code>nowrap</code> to enable <code>nowrap</code>. For ellipsis on overflow to work properly, <code>block</code> and <code>nowrap</code> should be manually set to <code>true</code>.
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
            <Demo Header="Texts" Key="0" MetadataPath="TextPage">
                <Stack Tokens="new StackTokens { ChildrenGap = new double[] { 10.0 }}">
                    <Stack Tokens="new StackTokens { ChildrenGap = new double[] { 5.0 }}">
                        <Text Variant=TextType.Large Block="true">
                            Text Ramp Example
                        </Text>
                        @foreach (TextType variant in Enum.GetValues(typeof(TextType)))
                        {
                            if (variant == TextType.None)
                                continue;
                            <div>
                                <Text Variant=variant>
                                    The quick brown fox jumped over the lazy dog.
                                </Text>
                            </div>
                        }
                    </Stack>
                    <Stack Tokens="new StackTokens { ChildrenGap = new double[] { 5.0 }}">
                        <Text Variant=TextType.Large Block="true">
                            Wrap (Default)
                        </Text>
                        <Text>
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                            ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in
                            reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt
                            in culpa qui officia deserunt mollit anim id est laborum.
                        </Text>
                    </Stack>
                    <Stack Tokens="new StackTokens { ChildrenGap = new double[] { 5.0 }}">
                        <Text Variant=TextType.Large Block=true>
                            No Wrap
                        </Text>
                        <Text NoWrap="true">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                            ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in
                            reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt
                            in culpa qui officia deserunt mollit anim id est laborum.
                        </Text>
                    </Stack>
                    <Stack Tokens="new StackTokens { ChildrenGap = new double[] { 5.0 }}">
                        <Text Variant=TextType.Large Block=true>
                            Custom Text Style With 'CustomVariant'
                        </Text>
                        <Text CustomVariant="customStyle">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                            ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in
                            reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt
                            in culpa qui officia deserunt mollit anim id est laborum.
                        </Text>
                    </Stack>
                    <Stack Tokens="new StackTokens { ChildrenGap = new double[] { 5.0 }}">
                        <Text Variant=TextType.Large Block=true>
                            Custom Text Style With 'Style'
                        </Text>
                        <Text Style="color:red;font-size:12px">
                            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim
                            ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in
                            reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt
                            in culpa qui officia deserunt mollit anim id est laborum.
                        </Text>
                    </Stack>
                </Stack>
            </Demo>
        </div>
    </div>
</div>
@code {
    MsTextStyle? customStyle;

    protected override void OnInitialized()
    {
        customStyle = new MsTextStyle()
        {
            Color = "green",
            FontSize = "22px"
        };
    }

}
