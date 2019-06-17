using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Dropdown
{
    public class DropdownBase<TItem> : FabricComponentBase
    {
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected int DropdownWidth { get; set; } = 0;
        [Parameter] protected string ErrorMessage { get; set; }
        [Parameter] protected string Label { get; set; }
        [Parameter] public bool MultiSelect { get; set; }
        [Parameter] protected string Placeholder { get; set; }
        [Parameter] protected bool Required { get; set; }
        [Parameter] protected ResponsiveMode ResponsiveMode { get; set; }


        [Parameter] protected RenderFragment ChildContent { get; set; }

        [Parameter] protected IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] protected RenderFragment<TItem> ItemTemplate { get; set; }

        protected bool isOpen { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected bool isSmall = false;

        protected List<string> selectedOptions = new List<string>();


        protected Task ClickHandler(UIMouseEventArgs args)
        {
            isOpen = !isOpen;
            return Task.CompletedTask;
        }
        protected Task FocusHandler(UIFocusEventArgs args)
        {
            // Could write logic to open on focus automatically...
            //isOpen = true;
            return Task.CompletedTask;
        }

        protected Task DismissHandler()
        {
            isOpen = false;
            return Task.CompletedTask;
        }

    }
}
