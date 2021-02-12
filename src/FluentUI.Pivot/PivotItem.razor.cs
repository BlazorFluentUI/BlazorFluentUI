using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace FluentUI
{

    // ToDo Test KeyTip
    // ToDo Add OnRenderItemLink
    public partial class PivotItem : FluentUIComponentBase, IDisposable
    {
        [Parameter] public string HeaderText { get; set; }
        [Parameter] public string ItemKey { get; set; }
        [Parameter] public string ItemCount { get; set; }

        [Parameter] public string IconName { get; set; }
        [Parameter] public string IconSrc { get; set; }
        [Parameter] public string KeyTip { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [CascadingParameter(Name = "Pivot")] protected Pivot ParentPivot { get; set; }

        private string dataContent;

        protected override void OnInitialized()
        {
            ParentPivot.PivotItems.Add(this);
            dataContent = $"{(string.IsNullOrWhiteSpace(HeaderText) ? "" : HeaderText)}{(string.IsNullOrWhiteSpace(ItemCount) ? "" : $" ({ItemCount})")}{(string.IsNullOrWhiteSpace(IconName) ? "" : " xx")}";
            base.OnInitialized();
        }

        private Task SelectItem(MouseEventArgs ev)
        {
            if (!(ParentPivot.Selected == this))
            {
                ParentPivot.Selected = this;
            }
            if (ParentPivot.OnLinkClick != null)
            {
                ParentPivot.OnLinkClick.Invoke(this, ev);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            ParentPivot.PivotItems.Remove(this);

            GC.SuppressFinalize(this);
            return;
        }
    }
}