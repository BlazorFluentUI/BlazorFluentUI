using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    partial class RibbonMenuIntern : Pivot
    {
        [Parameter] public string? BackstageHeader { get; set; }
        [Parameter] public RenderFragment? Backstage { get; set; }
        //bool showBackstage;
        [Parameter] public bool ShowBackstage { get; set; }

        [Parameter] public EventCallback<bool> ShowBackstageChanged { get; set; }
        //private async Task OnShowBackstageChanged(ChangeEventArgs e)
        //{
        //    bool showBackstage = (bool)e.Value;

        //    await ShowBackstageChanged.InvokeAsync(showBackstage);
        //}
        FluentUIComponentBase? calloutTarget;
        string? backstageItemKey;
        public override PivotItem? Selected
        {
            get => base.Selected;
            set
            {
                if (_selected != null && PivotItems != null)
                {
                    if (backstageItemKey != null && value?.ItemKey == backstageItemKey)
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
                    SelectedKeyChanged.InvokeAsync(_selected?.ItemKey);
                    StateHasChanged();
                }
            }
        }

        protected override void SetSelection(bool firstRender = false)
        {
            if (firstRender)
            {
                if (BackstageHeader != null && PivotItems?.Count > 1 && DefaultSelectedKey == null)
                {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                    backstageItemKey = PivotItems[0].ItemKey = $"k_{Guid.NewGuid().ToString().Replace("-", "")}";
                    PivotItem? firstItemAfterBackstage = PivotItems[1];
                    if (firstItemAfterBackstage.ItemKey == null)
                    {

                        firstItemAfterBackstage.ItemKey = $"k_{Guid.NewGuid().ToString().Replace("-", "")}";

                    }
                    DefaultSelectedKey = firstItemAfterBackstage.ItemKey;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
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
