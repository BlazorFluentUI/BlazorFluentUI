@page "/marqueeSelectionPage"

<h1>MarqueeSelection</h1>

<style>
    .photoCell.is-selected {
        background: var(--palette-ThemeLighter);
        border: 1px solid var(--palette-ThemePrimary);
    }
</style>

<Demo Header="Basic Selection Example" Key="0" MetadataPath="MarqueeSelectionPage">

    <MarqueeSelection TItem="Photo" 
                         IsEnabled="toggleIsMarqueeEnabled"
                         Selection="selection"
                         >
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
        for (var i=0; i< 250;i++)
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
