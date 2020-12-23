using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFURibbonMenu<TItem> : BFUComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public IEnumerable<TItem>? ItemsSource { get; set; }
        //[Parameter] public Func<TItem, string> GetKey { get; set; }
        [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
    }
   
}
