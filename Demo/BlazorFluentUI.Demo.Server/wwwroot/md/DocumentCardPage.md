@page "/documentcard"

<h1>DocumentCard</h1>

<Demo Header="Default Style" Key="0" MetadataPath="DocumentCardPage">
    <Stack Style="width:100%;" Tokens="@(new StackTokens() { ChildrenGap = new [] {20d}})">
        <DocumentCard Type="DocumentCardType.Normal" OnClickHref="http://bing.com">
            <DocumentCardPreview PreviewImages="@defaultImages.Take(1).ToArray()"></DocumentCardPreview>
            <DocumentCardTitle Title="this is a title a long title a really long title very long indeed this is a title a long title a really long title very long indeed" ShouldTruncate="true"></DocumentCardTitle>
            <DocumentCardActivity Activity="Created a few minutes ago" People="@persons.Take(1).ToArray()"></DocumentCardActivity>
        </DocumentCard>
    </Stack>
</Demo>

<Demo Header="Compact Style" Key="1" MetadataPath="DocumentCardPage">
    <Stack Style="width:100%;" Tokens="@(new StackTokens() { ChildrenGap = new [] {20d}})">
        <DocumentCard Type="DocumentCardType.Compact" OnClickHref="http://bing.com">
            <DocumentCardPreview PreviewImages="@images.Take(1).ToArray()"></DocumentCardPreview>
            <DocumentCardDetails>
                <DocumentCardTitle ShouldTruncate="false" Title="My Heading"></DocumentCardTitle>
                <DocumentCardActivity Activity="Test activity" People="@persons.Take(1).ToArray()"></DocumentCardActivity>
            </DocumentCardDetails>
        </DocumentCard>
        <DocumentCard Type="DocumentCardType.Compact" OnClickHref="http://bing.com">
            <DocumentCardPreview PreviewImages="@images"></DocumentCardPreview>
            <DocumentCardDetails>
                <DocumentCardTitle ShouldTruncate="false" Title="My Heading"></DocumentCardTitle>
                <DocumentCardActivity Activity="Test activity" People="@persons"></DocumentCardActivity>
            </DocumentCardDetails>
        </DocumentCard>
        <DocumentCard Type="DocumentCardType.Compact" OnClickHref="http://bing.com">
            <DocumentCardPreview PreviewImages="@previewImagesUsingIcon"></DocumentCardPreview>
            <DocumentCardDetails>
                <DocumentCardTitle ShouldTruncate="false" Title="View and share files"></DocumentCardTitle>
                <DocumentCardActivity Activity="Created a few minutes ago" People="@persons.Take(1).ToArray()"></DocumentCardActivity>
            </DocumentCardDetails>
        </DocumentCard>
        <DocumentCard Type="DocumentCardType.Compact" OnClickHref="http://bing.com">
            <DocumentCardPreview PreviewImages="@previewOutlookUsingIcon"></DocumentCardPreview>
            <DocumentCardDetails>
                <DocumentCardTitle ShouldTruncate="false" Title="View and share files"></DocumentCardTitle>
                <DocumentCardActivity Activity="Created a few minutes ago" People="@persons.Take(1).ToArray()"></DocumentCardActivity>
            </DocumentCardDetails>
        </DocumentCard>
    </Stack>
</Demo>

<Demo Header="Commands" Key="2" MetadataPath="DocumentCardPage">
    <Stack Style="width:100%;margin-bottom:50px;" Tokens="@(new StackTokens() { ChildrenGap = new [] {20d}})">
        <DocumentCard Type="DocumentCardType.Normal">
            <DocumentCardPreview PreviewImages="@images"></DocumentCardPreview>
            <DocumentCardLocation Location="Github" LocationHref="https://www.github.com"></DocumentCardLocation>
            <DocumentCardTitle Title="this is a title" ShouldTruncate="false"></DocumentCardTitle>
            <DocumentCardActivity Activity="Created a few minutes ago" People="@persons.Take(1).ToArray()"></DocumentCardActivity>
            <DocumentCardActions Actions="@actions" Views="342"></DocumentCardActions>
        </DocumentCard>
    </Stack>
</Demo>

@code {
    [Inject] public ThemeProvider ThemeProvider { get; set; }

    public ITheme Theme => ThemeProvider.Theme;

    DocumentPreviewImage[] defaultImages;
    DocumentPreviewImage[] previewImagesUsingIcon;
    DocumentPreviewImage[] previewOutlookUsingIcon;
    DocumentCardActivityPerson[] persons;
    DocumentPreviewImage[] images;
    DocumentCardAction[] actions;

    protected override Task OnParametersSetAsync()
    {
        previewImagesUsingIcon = new DocumentPreviewImage[]
        {
            new DocumentPreviewImage()
            {
                PreviewIconProps = new IconProperties()
                {
                    IconName = "Mail",
                    Styles = $"color: white; font-size: {Theme.FontStyle.FontSize.SuperLarge};"
            },
                Width = 144,
                Styles = $"background-color:{Theme.Palette.ThemePrimary}"
        }
               };
        previewOutlookUsingIcon = new DocumentPreviewImage[]
   {
            new DocumentPreviewImage()
            {
                PreviewIconProps = new IconProperties()
                {
                    IconName = "OutlookLogo",
                    Styles = $"color: #0078d7; font-size: {Theme.FontStyle.FontSize.SuperLarge};background-color:{Theme.Palette.NeutralLighterAlt};"
            },
                Width = 144,
                Styles = $"background-color:{Theme.Palette.NeutralLighterAlt}"
        }
          };

        actions = new DocumentCardAction[]
        {
            new DocumentCardAction()
            {
                IconName = "Share",
                OnClickHandler = EventCallback.Factory.Create<MouseEventArgs>(this, ClickHandler)
            },
             new DocumentCardAction()
            {
                IconName = "Pin",
                OnClickHandler = EventCallback.Factory.Create<MouseEventArgs>(this, ClickHandler)
            },
              new DocumentCardAction()
            {
                IconName = "Ringer",
                OnClickHandler = EventCallback.Factory.Create<MouseEventArgs>(this, ClickHandler)
            }

        };
        return base.OnParametersSetAsync();
    }

    public void ClickHandler(MouseEventArgs e)
    {
        Console.WriteLine("Clicked");
    }

    public DocumentCardPage()
    {
        defaultImages = new DocumentPreviewImage[]
        {
            new DocumentPreviewImage()
            {
                Width = 318,
                Height = 196,
                ImageFit = ImageFit.Cover,
                Name = "A preview image",
                PreviewImageSrc = "sampleImage.jpg",
                IconSrc = "smallSampleImage.jpg",
                LinkProperties = new LinkProperties()
                {
                    Href = "https://www.bing.com",
                    Target = "_blank"
                }
            }
                                    };


        List<DocumentCardActivityPerson> p = new List<DocumentCardActivityPerson>();
        p.Add(new DocumentCardActivityPerson()
        {
            Name = "Albert Einstein",
            AllowPhoneInitials = true
        });
        p.Add(new DocumentCardActivityPerson()
        {
            Name = "Marie Curie",
            AllowPhoneInitials = true,
            ProfileImageSrc = "personFace.jpg"
        });

        persons = p.ToArray();

        List<DocumentPreviewImage> i = new List<DocumentPreviewImage>();
        i.Add(new DocumentPreviewImage()
        {
            Width = 144,
            Name = "A preview image",
            PreviewImageSrc = "sampleImage.jpg",
            IconSrc = "smallSampleImage.jpg",
            LinkProperties = new LinkProperties()
            {
                Href = "https://www.bing.com",
                Target = "_blank"
            }
        });

        i.Add(new DocumentPreviewImage()
        {
            Width = 144,
            Name = "A preview image",
            PreviewImageSrc = "sampleImage.jpg",
            IconSrc = "smallSampleImage.jpg",
            LinkProperties = new LinkProperties()
            {
                Href = "https://www.bing.com",
                Target = "_blank"
            }
        });

        i.Add(new DocumentPreviewImage()
        {
            Width = 144,
            Name = "A preview image",
            PreviewImageSrc = "sampleImage.jpg",
            IconSrc = "smallSampleImage.jpg",
            LinkProperties = new LinkProperties()
            {
                Href = "https://www.bing.com",
                Target = "_blank"
            }
        });

        i.Add(new DocumentPreviewImage()
        {
            Width = 144,
            Name = "A preview image",
            PreviewImageSrc = "sampleImage.jpg",
            IconSrc = "smallSampleImage.jpg",
            LinkProperties = new LinkProperties()
            {
                Href = "https://www.bing.com",
                Target = "_blank"
            }
        });

        i.Add(new DocumentPreviewImage()
        {
            Width = 144,
            Name = "A preview image",
            PreviewImageSrc = "sampleImage.jpg",
            IconSrc = "smallSampleImage.jpg",
            LinkProperties = new LinkProperties()
            {
                Href = "https://www.bing.com",
                Target = "_blank"
            }
        });

        images = i.ToArray();
    }

}

