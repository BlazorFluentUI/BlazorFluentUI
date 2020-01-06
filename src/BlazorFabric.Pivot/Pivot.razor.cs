using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components .Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorFabric
{
    public partial class Pivot : FabricComponentBase
    {
        [Parameter] public int? DefaultSelectedIndex { get; set; }
        [Parameter] public string DefaultSelectedKey { get; set; }
        [Parameter] public bool HeadersOnly { get; set; }
        [Parameter] public PivotLinkFormat LinkFormat { get; set; }
        [Parameter] public PivotLinkSize LinkSize { get; set; }
        [Parameter] public Action<PivotItem, MouseEventArgs> OnLinkClick { get; set; }
        [Parameter] public EventCallback<string> SelectedKeyChanged{ get; set; }
        [Parameter] public string SelectedKey 
        { 
            get => Selected?.ItemKey;
            set
            {
                if(_selectedKey == value)
                {
                    return;
                }
                if (PivotItems.Count == 0)
                {
                    return;
                }
                if (PivotItems.FirstOrDefault(item => item.ItemKey == value) != null)
                {
                    Selected = PivotItems.FirstOrDefault(item => item.ItemKey == value);
                }
                else
                {
                    Selected = PivotItems.First();
                }
            }   
        }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public IList<PivotItem> PivotItems { get; set; } = new List<PivotItem>();
        public PivotItem Selected 
        { 
            get => _selected;
            set
            {
                if (_selected == value)
                    return;

                _selected = value;
                SelectedKeyChanged.InvokeAsync(_selected.ItemKey);
                StateHasChanged();
            }
        }

        private PivotItem _selected;
        private string _selectedKey;

        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                SetFirstSelection();
            }
            base.OnAfterRender(firstRender);
        }

        private void SetFirstSelection()
        {
            if (!string.IsNullOrWhiteSpace(DefaultSelectedKey) && PivotItems.FirstOrDefault(item => item.ItemKey == DefaultSelectedKey) != null)
            {
                Selected = PivotItems.FirstOrDefault(item => item.ItemKey == DefaultSelectedKey);
            }
            else if (DefaultSelectedIndex.HasValue && DefaultSelectedIndex < PivotItems.Count())
            {
                Selected = PivotItems.ElementAt(DefaultSelectedIndex.Value);
            }
            else if (!string.IsNullOrWhiteSpace(SelectedKey) && PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey) != null)
            {
                Selected = PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey);
            }
            else 
            {
                Selected = PivotItems.FirstOrDefault();
            }
        }
    }
}