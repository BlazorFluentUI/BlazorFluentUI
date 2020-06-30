using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorFluentUI
{
    public class BFUDocumentCardAction
    {
        public string? IconName { get; set; }

        public string? IconAriaLabel { get; set; }

        public bool Checked { get; set; }

        public bool Disabled { get; set; }

        public EventCallback<MouseEventArgs> OnClickHandler { get; set; }
    }
}