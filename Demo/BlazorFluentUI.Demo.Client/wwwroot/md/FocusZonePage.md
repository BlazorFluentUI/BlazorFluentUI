﻿@page  "/focusZonePage"

<h1>FocusZone</h1>

<h4>FocusZone with horizontal movement</h4>

<div style="padding: 50px; background-color:yellow;">
    <BFUFocusZone Direction="FocusZoneDirection.Horizontal">
        <BFUDefaultButton Text="First Button" OnClick=@OnInnerClick />
        <BFUDefaultButton Text="Second Button" OnClick=@OnInnerClick />
        <BFUDefaultButton Text="Third Button" OnClick=@OnInnerClick />
    </BFUFocusZone>
</div>

<BFUDefaultButton Text="Outside Button" OnClick=@OnOuterClick />

<h4>FocusZone with vertical &amp; circular movement</h4>

    <BFUFocusZone Direction="FocusZoneDirection.Vertical" IsCircularNavigation="true">
        <BFUList ItemsSource=@items >
            <ItemTemplate>
                <div style="display:flex;flex-direction:row;width:100%;" data-is-focusable="true">
                    <BFUImage Src="redArrow.jpg" Height="50" Width="50" />
                    <BFULabel>This is an item #@context</BFULabel>
                </div>
            </ItemTemplate>
        </BFUList>
    </BFUFocusZone>

<style>
    .photoCell {
        position: relative;
        display: inline-block;
        padding: 2px;
        box-sizing: border-box;
    }

        .photoCell:focus {
            outline: none;
        }

            .photoCell:focus:after {
                content: "";
                position: absolute;
                right: 4px;
                left: 4px;
                top: 4px;
                bottom: 4px;
                border: 1px solid @(Theme.Palette.White);
                outline: 2px solid @(Theme.Palette.ThemePrimary);
            }
</style>

<h4>FocusZone with bidirectional movement</h4>
<BFUFocusZone Direction="FocusZoneDirection.Bidirectional"
           Style="display:inline-block;border:1px solid var(--palette-NeutralTertiary);padding:10px;line-height:0;overflow:hidden;">
    @for (var i = 0; i < photos.Count; i++)
    {
        <li @key=@i
            data-is-focusable="true"
            class="photoCell">
            <BFUImage Src=@photos[i].Url
                   Width=@photos[i].Width
                   Height=@photos[i].Height />
        </li>
    }
</BFUFocusZone>

@code {
    bool isFocusTrapped = false;
    string debugText = "";

    [Inject] public ThemeProvider ThemeProvider { get; set; }

    public ITheme Theme => ThemeProvider.Theme;

    System.Collections.Generic.List<string> items = new System.Collections.Generic.List<string>();

    class Photo
    {
        public string Url { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
    }

    System.Collections.Generic.List<Photo> photos = new System.Collections.Generic.List<Photo>();

    override protected Task OnInitializedAsync()
    {
        for (int i = 0; i < 6; i++)
        {
            items.Add(i.ToString());
        }

        var random = new Random();

        for (int i = 0; i < 20; i++)
        {
            var randomWidth = 50 + Math.Floor(random.NextDouble() * 150);
            photos.Add(new Photo() { Url = $"http://placehold.it/{randomWidth}x100", Height = 100, Width = randomWidth });
        }
        return Task.CompletedTask;
    }

    Task OnInnerClick(MouseEventArgs args)
    {
        return Task.CompletedTask;
    }
    Task OnOuterClick(MouseEventArgs args)
    {
        return Task.CompletedTask;
    }
}
