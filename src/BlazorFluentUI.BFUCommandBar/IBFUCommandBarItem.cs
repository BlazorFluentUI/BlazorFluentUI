namespace BlazorFluentUI
{
    public interface IBFUCommandBarItem : IBFUContextualMenuItem
    {
        //buttonStyles
        string CacheKey { get; set; }
        //commandBarButtonAs
        bool IconOnly { get; set; }
        bool RenderedInOverflow { get; set; }
        //tooltipHostProps
    }
}
