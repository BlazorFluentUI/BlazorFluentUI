@page "/marqueeSelectionPage"

<header class="root">
    <h1 class="title">MarqueeSelection</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                The MarqueeSelection component provides a service which allows the user to drag a rectangle to be drawn around
                items to select them. This works in conjunction with a selection object, which can be used to generically store selection state, separate from a component that consumes the state.
            </p>
            <p>MarqueeSelection also works in conjunction with the AutoScroll utility to automatically scroll the container when we drag a rectangle within the vicinity of the edges.</p><p class="root-158">When a selection rectangle is dragged, we look for elements with the <strong>data-selection-index</strong> attribute populated. We get these elements' boundingClientRects and compare them with the root's rect to determine selection state. We update the selection state appropriately.</p><p class="root-158">
                In virtualization cases where items that were once selected are dematerialized, we will keep the item in its
                previous state until we know definitively if it's on/off. (In other words, this works with List.)
            </p>
        </div>
    </div>
</div>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <style>
            .photoCell.is-selected {
                background: var(--palette-ThemeLighter);
                border: 1px solid var(--palette-ThemePrimary);
            }
        </style>

        <div class="subSection">
            <Demo Header="Basic Selection Example" Key="0" MetadataPath="MarqueeSelectionPage">

                <MarqueeSelection TItem="Photo"
                                  IsEnabled="toggleIsMarqueeEnabled"
                                  Selection="selection">
                    <Checkbox Label="Is marquee enabled"
                              Style="margin:10px 0;"
                              @bind-Checked="toggleIsMarqueeEnabled" />
                    <p>Drag a rectangle around the items below to select them:</p>
                    <ul style="display:inline-block;border:1px solid var(--palette-NeutralTertiary);margin:0;padding:10px;overflow:hidden;user-select:none;">
                        @foreach (var photo in photos)
                        {
                            <div key=@(photo.Key)
                                 class=@($"photoCell {(selection.IsKeySelected(photo.Key) ? "is-selected" : "")}")
                                 data-is-focusable="true"
                                 data-selection-index=@photo.Key
                                 style="width: @(photo.Width)px;height: @(photo.Height)px;position:relative;display:inline-block;margin:2px;box-sizing:border-box;background:var(--palette-NeutralLighter);line-height:100px;vertical-align:middle;text-align:center;">
                                @photo.Key
                            </div>
                        }
                    </ul>
                </MarqueeSelection>
            </Demo>
        </div>
    </div>
</div>

@code {
    [Inject] ThemeProvider ThemeProvider { get; set; }

    bool toggleIsMarqueeEnabled = true;
    List<Photo> photos = new List<Photo>();
    Selection<Photo> selection = new Selection<Photo>();

    class Photo
    {
        public string Url { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Key { get; set; }
    }

    protected override Task OnInitializedAsync()
    {
        var random = new Random();
        for (var i = 0; i < 250; i++)
        {
            var width = 50 + Math.Floor(random.NextDouble() * 150);
            photos.Add(new Photo
            {
                Key = i,
                Url = $"http://placehold.it/{width}x100",
                Width = width,
                Height = 100
            });
        }
        selection.GetKey = (item) => item.Key;
        selection.SetItems(photos);
        selection.SelectionMode = SelectionMode.Multiple;
        //selection.SelectionChanged.Subscribe(_ =>
        //{
        //    InvokeAsync(StateHasChanged);
        //});
        return base.OnInitializedAsync();
    }


}
