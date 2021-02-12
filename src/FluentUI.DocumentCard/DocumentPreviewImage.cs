namespace FluentUI
{
    public class DocumentPreviewImage
    {
        /// <summary>
        /// File name for the document this preview represents.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Path to the preview image. 
        /// </summary>
        public string? PreviewImageSrc { get; set; }

        /// <summary>
        /// Path to the icon associated with this document type.
        /// </summary>
        public string? IconSrc { get; set; }
        /// <summary>
        /// If provided, forces the preview image to be this width.
        /// </summary>
        public double Width { get; set; } = double.NaN;

        /// <summary>
        /// If provided, forces the preview image to be this height.
        /// </summary>
        public double Height { get; set; } = double.NaN;
        /// <summary>
        /// Used to determine how to size the image to fit the dimensions of the component.
        /// If both dimensions are provided, then the image is fit using ImageFit.scale, otherwise ImageFit.none is used.
        /// </summary>
        public ImageFit ImageFit { get; set; } = ImageFit.Unset;
        /// <summary>
        /// The props for the preview icon container classname.
        /// If provided, icon container classname will be used.
        /// </summary>
        public string? PreviewIconContainerClass { get; set; }
        public string? Styles { get; set; }
        public IconProperties? PreviewIconProps { get; set; }

        /// <summary>
        /// The props for the preview icon.
        /// If provided, icon will be rendered instead of image.
        /// </summary>
        public LinkProperties? LinkProperties { get; set; }
    }
}