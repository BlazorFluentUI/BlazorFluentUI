@page "/ratingPage"

<header class="root">
    <h1 class="title">Rating</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                Ratings show people’s opinions of a product, helping others make more informed purchasing decisions. People can also rate products they’ve purchased.
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
            <Demo Header="Rating" Key="0" MetadataPath="RatingPage">
                <Label>Large Stars:</Label>
                <Rating Size="RatingSize.Large" />

                <Label>Small Stars:</Label>
                <Rating RatingValue="3" />

                <Label>10 Small Stars:</Label>
                <Rating Max="10" />

                <Label>Disabled:</Label>
                <Rating Disabled="true" />

                <Label>Half star in readOnly mode:</Label>
                <Rating ReadOnly="true" RatingValue="2.5" />

                <Label>Custom icons:</Label>
                <Rating RatingValue="2.5" IconName="StarburstSolid" UnselectedIcon="Starburst" />
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="Two-Way-Binding" Key="1" MetadataPath="RatingPage">
                <Rating Max="10" @bind-RatingValue="rateten" />
                @rateten of 10 Stars selected!
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="Button Controlled Rating" Key="2" MetadataPath="RatingPage">
                <Rating AllowZeroStars="true" RatingValue="rate" ReadOnly="true" />
                <PrimaryButton OnClick="SetRate" Text="@buttonText" />
            </Demo>
        </div>
    </div>
</div>

@code {
    private double rate = 0;
    private double rateten = 1;
    private string buttonText = "Click to change rating to 5";
    private void SetRate()
    {
        Console.WriteLine(rateten);
        if (rate == 0)
        {
            buttonText = "Click to change rating to 0";
            rate = 5;
        }
        else
        {
            buttonText = "Click to change rating to 5";
            rate = 0;
        }
    }
}


