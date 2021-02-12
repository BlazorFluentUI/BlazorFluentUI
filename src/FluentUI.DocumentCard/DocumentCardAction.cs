using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FluentUI
{
    public class DocumentCardAction
    {
        public string? IconName { get; set; }
        public string? IconSrc { get; set; }

        public string? IconAriaLabel { get; set; }

        public bool Checked { get; set; }

        public bool Disabled { get; set; }

        public EventCallback<MouseEventArgs> OnClickHandler { get; set; }
    }
}