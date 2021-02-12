using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class RibbonMenu<TItem> : FluentUIComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment Backstage { get; set; }
        [Parameter] public string BackstageHeader { get; set; }
        bool showBackstage;
        [Parameter] public bool ShowBackstage 
        { 
            get
            {
                return showBackstage;
            }
            set
            {
                if(showBackstage != value)
                {
                    showBackstage = value;
                    ShowBackstageChanged.InvokeAsync(showBackstage);
                }
            }
        }
        [Parameter] public EventCallback<bool> ShowBackstageChanged { get; set; }
        //private async Task OnShowBackstageChanged(ChangeEventArgs e)
        //{
        //    bool showBackstage = (bool)e.Value;
        //    await ShowBackstageChanged.InvokeAsync(showBackstage);
        //}

        [Parameter] public IEnumerable<TItem>? ItemsSource { get; set; }
        //[Parameter] public Func<TItem, string> GetKey { get; set; }
        [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
    }
   
}
