using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FluentUI
{
    public class NavBarItem : INavBarItem
    {
        public string CacheKey { get; set; }

        public bool IconOnly { get; set; }

        public bool RenderedInOverflow { get; set; }

        public string AriaLabel { get; set; }

        public bool CanCheck { get; set; } = true;

        public bool Checked { get; set; }

        public string ClassName { get; set; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public object Data { get; set; }

        public bool Disabled { get; set; }

        public string Href { get => Url; set => Url = value; }

        public string? IconName { get; set; }
        public string? IconSrc { get; set; }

        public string Id { get; set; }

        public bool IsExpanded { get; set; }

        public IEnumerable<IContextualMenuItem> Items { get; set; }

        public ContextualMenuItemType ItemType { get; set; }

        public string Key { get; set; }

        public object KeytipProps { get; set; }

        public NavMatchType NavMatchType { get; set; } = NavMatchType.AnchorIncluded;

        public Action<ItemClickedArgs> OnClick { get; set; }

        public bool PrimaryDisabled { get; set; }

        public string Rel { get; set; }

        public string Role { get; set; }

        public string SecondaryText { get; set; }

        public string ShortCut { get; set; }

        public bool Split { get; set; }

        public string Style { get; set; }

        public string Target { get; set; } //link <a> target

        public string Text { get; set; }

        public string Title { get; set; } //tooltip and ARIA

        public string Url { get; set; }

    }
}
