@page "/messageBarPage"

<Stack>
    <StackItem Align=Alignment.Start>
        <h1>MessageBar</h1>
    </StackItem>
    <Stack>
        <Stack Tokens=@(new StackTokens { ChildrenGap = new[] { 20.0 }, MaxWidth=650.0, Padding=8.0 })>
            <StackItem>
                <MessageBar>
                    Info/Default MessageBar.
                    <Link Href="https://github.com/FluentUI/FluentUI" Target="_blank">
                    Visit us on GitHub.
                    </Link>
                </MessageBar>
            </StackItem>
            <StackItem>
                <MessageBar MessageBarType="MessageBarType.Error" IsMultiline="false" OnDismiss="OnDismiss">
                    Error MessageBar with single line, with dismiss button.
                    <Link Href="https://github.com/FluentUI/FluentUI" Target="_blank">
                    Visit us on GitHub.
                    </Link>
                </MessageBar>
            </StackItem>
            <StackItem>
                <MessageBar MessageBarType=MessageBarType.Blocked
                            IsMultiline=false
                            OnDismiss="OnDismiss"
                            DismissButtonAriaLabel="Close"
                            Truncated=true
                            OverflowButtonAriaLabel="See more">
                    <b>Blocked MessageBar - single line, with dismiss button and truncated text.</b> Truncation is not available if you use action buttons
                    or multiline and should be used sparingly. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi luctus, purus a lobortis
                    tristique, odio augue pharetra metus, ac placerat nunc mi nec dui. Vestibulum aliquam et nunc semper scelerisque. Curabitur vitae orci
                    nec quam condimentum porttitor et sed lacus. Vivamus ac efficitur leo. Cras faucibus mauris libero, ac placerat erat euismod et. Donec
                    pulvinar commodo odio sit amet faucibus. In hac habitasse platea dictumst. Duis eu ante commodo, condimentum nibh pellentesque, laoreet
                    enim. Fusce massa lorem, ultrices eu mi a, fermentum suscipit magna. Integer porta purus pulvinar, hendrerit felis eget, condimentum
                    mauris.
                </MessageBar>
            </StackItem>
            <StackItem>
                <MessageBar MessageBarType=MessageBarType.SevereWarning>
                    <Actions>
                        <div>
                            <MessageBarButton Text="Yes" />
                            <MessageBarButton Text="No" />
                        </div>
                    </Actions>
                    <ChildContent>
                        SevereWarning MessageBar with action buttons which defaults to multiline.
                        <Link Href="https://github.com/FluentUI/FluentUI" Target="_blank">
                        Visit us on GitHub.
                        </Link>
                    </ChildContent>
                </MessageBar>
            </StackItem>
            <StackItem>
                <MessageBar MessageBarType=MessageBarType.Success
                            IsMultiline=false>
                    <Actions>
                        <div>
                            <MessageBarButton Text="Yes" />
                            <MessageBarButton Text="No" />
                        </div>
                    </Actions>
                    <ChildContent>
                        Success MessageBar with single line and action buttons.
                        <Link Href="https://github.com/FluentUI/FluentUI" Target="_blank">
                        Visit us on GitHub.
                        </Link>
                    </ChildContent>
                </MessageBar>
            </StackItem>
            <StackItem>
                <MessageBar MessageBarType=MessageBarType.Warning
                            IsMultiline=false
                            OnDismiss=@OnDismiss
                            DismissButtonAriaLabel="Close">
                    <Actions>
                        <div><MessageBarButton Text="Action" /></div>
                    </Actions>
                    <ChildContent>
                        Warning MessageBar content.
                        <Link Href="https://github.com/FluentUI/FluentUI" Target="_blank">
                        Visit us on GitHub.
                        </Link>
                    </ChildContent>
                </MessageBar>
            </StackItem>
            <StackItem>
                <MessageBar OnDismiss=@OnDismiss
                            DismissButtonAriaLabel="Close"
                            MessageBarType=MessageBarType.Warning>
                    <Actions>
                        <div>
                            <MessageBarButton Text="Yes" />
                            <MessageBarButton Text="No" />
                        </div>
                    </Actions>
                    <ChildContent>
                        <b>Warning defaults to multiline</b>. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi luctus, purus a lobortis tristique,
                        odio augue pharetra metus, ac placerat nunc mi nec dui. Vestibulum aliquam et nunc semper scelerisque. Curabitur vitae orci nec quam
                        condimentum porttitor et sed lacus. Vivamus ac efficitur leo. Cras faucibus mauris libero, ac placerat erat euismod et. Donec pulvinar
                        commodo odio sit amet faucibus. In hac habitasse platea dictumst. Duis eu ante commodo, condimentum nibh pellentesque, laoreet enim.
                        Fusce massa lorem, ultrices eu mi a, fermentum suscipit magna. Integer porta purus pulvinar, hendrerit felis eget, condimentum mauris.
                        <Link Href="https://github.com/FluentUI/FluentUI" Target="_blank">
                        Visit us on GitHub.
                        </Link>
                    </ChildContent>
                </MessageBar>
            </StackItem>
            @if (onDismissIsClicked)
            {
            <StackItem>
                <MessageBar OnDismiss=@OnDismissClose
                            DismissButtonAriaLabel="Close">
                    <ChildContent>
                        OnDismiss has been clicked!
                    </ChildContent>
                    <Actions>
                        <div>
                            <MessageBarButton OnClick=@OnDismissClose Text="Close" />
                        </div>
                    </Actions>
                </MessageBar>
            </StackItem>
            }
        </Stack>
    </Stack>
</Stack>

@code
{
    private bool onDismissIsClicked = false;
    private void OnDismiss()
    {
        onDismissIsClicked = true;
        StateHasChanged();
    }
    private void OnDismissClose()
    {
        onDismissIsClicked = false;
        StateHasChanged();
    }
}
