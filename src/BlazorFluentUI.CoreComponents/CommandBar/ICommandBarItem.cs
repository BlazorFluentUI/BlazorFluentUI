namespace BlazorFluentUI
{
    public interface ICommandBarItem : IContextualMenuItem
    {
        //buttonStyles
        string CacheKey { get; set; }
        //commandBarButtonAs
        bool IconOnly { get; set; }
        bool RenderedInOverflow { get; set; }
        //tooltipHostProps
        #region RadioButton feature
        public bool IsRadioButton { get; set; }
        public string GroupName { get; set; }

        #endregion
    }
}
