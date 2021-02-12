using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    partial class RibbonMenuIntern : Pivot
    {
        [Parameter] public string BackstageHeader { get; set; }
        [Parameter] public RenderFragment Backstage { get; set; }
        bool showBackstage;
        [Parameter] public bool ShowBackstage { get; set; } 

        [Parameter] public EventCallback<bool> ShowBackstageChanged { get; set; }
        //private async Task OnShowBackstageChanged(ChangeEventArgs e)
        //{
        //    bool showBackstage = (bool)e.Value;

        //    await ShowBackstageChanged.InvokeAsync(showBackstage);
        //}
        FluentUIComponentBase calloutTarget;
        string backstageItemKey;
        public override PivotItem Selected
        {
            get => base.Selected;
            set
            {
                if (backstageItemKey != null && value.ItemKey == backstageItemKey)
                {
                    ShowBackstageChanged.InvokeAsync(true);
                }
                else
                {
                    ShowBackstageChanged.InvokeAsync(false);
                    if (_selected == value)
                        return;

                    if (!HeadersOnly)
                    {
                        _redraw = true;
                        _oldIndex = PivotItems.IndexOf(_selected);
                        _oldChildContent = _selected?.ChildContent;
                    }
                    _selected = value;
                }
                SelectedKeyChanged.InvokeAsync(_selected.ItemKey);
                StateHasChanged();
            }
        }

        protected override void SetSelection(bool firstRender = false)
        {
            if(firstRender)
            {
                if(BackstageHeader != null && PivotItems.Count >1 && DefaultSelectedKey == null)
                {
                    backstageItemKey = PivotItems[0].ItemKey = Guid.NewGuid().ToString();
                    var firstItemAfterBackstage = PivotItems[1];
                    if (firstItemAfterBackstage.ItemKey == null)
                    {
                        firstItemAfterBackstage.ItemKey = Guid.NewGuid().ToString();
                    }
                    DefaultSelectedKey = firstItemAfterBackstage.ItemKey;
                }
            }
            base.SetSelection(firstRender);
        }

        Task PanelDismiss()
        {
            ShowBackstageChanged.InvokeAsync(false);
            return Task.CompletedTask;
        }
    }
}
