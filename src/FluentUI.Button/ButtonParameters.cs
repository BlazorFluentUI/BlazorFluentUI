using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FluentUI
{
    public class ButtonParameters : FluentUIComponentBase
    {
        [Parameter] public RenderFragment ContentTemplate { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Href { get; set; }
        [Parameter] public bool Primary { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool AllowDisabledFocus { get; set; }
        [Parameter] public bool PrimaryDisabled { get; set; }
        [Parameter] public bool? Checked { get; set; }
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public string AriaDescripton { get; set; }
        //[Parameter] public bool AriaHidden { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public string SecondaryText { get; set; }
        [Parameter] public bool Toggle { get; set; }
        [Parameter] public bool Split { get; set; }
        
        [Parameter] public string IconName { get; set; }
        [Parameter] public string IconSrc { get; set; }
        [Parameter] public bool HideChevron { get; set; }
        #region MenuItems
        [Parameter] public IEnumerable<object> MenuItems { get; set; }
        [Parameter] public RenderFragment<object> MenuItemTemplate { get; set; }
        [Parameter] public bool SubordinateItemTemplate { get; set; }
        #endregion
        //[Parameter] public RenderFragment ContextualMenuContent { get; set; }
        //[Parameter] public RenderFragment ContextualMenuItemsSource { get; set; }
        //[Parameter] public RenderFragment ContextualMenuItemTemplate { get; set; }

        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter] public ICommand Command { get; set; }
        [Parameter] public object CommandParameter { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnknownProperties { get; set; }
    }
}
