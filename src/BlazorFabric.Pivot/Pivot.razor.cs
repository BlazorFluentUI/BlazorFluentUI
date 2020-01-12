using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [Parameter] public EventCallback<string> SelectedKeyChanged { get; set; }
        [Parameter]
        public string SelectedKey
        {
            get => _selectedKey;
            set
            {
                if (_selectedKey == value)
                {
                    return;
                }
                _selectedKey = value;
            }
        }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public IList<PivotItem> PivotItems { get; set; }
        public PivotItem Selected
        {
            get => _selected;
            set
            {
                if (_selected == value)
                    return;

                if (!HeadersOnly)
                {
                    _redraw = true;
                    _oldIndex = PivotItems.IndexOf(_selected);
                    _oldChildContent = _selected?.ChildContent;
                }
                _selected = value;
                SelectedKeyChanged.InvokeAsync(_selected.ItemKey);
                StateHasChanged();
            }
        }

        private PivotItem _selected;
        private bool _isControlled;
        private string _selectedKey;
        private bool _redraw;
        private RenderFragment _oldChildContent;
        private int _oldIndex = 0;

                protected override void OnInitialized()
        {
            PivotItems = new List<PivotItem>();
            if (DefaultSelectedIndex == null && DefaultSelectedKey == null)
            {
                _isControlled = true;
            }
            else
            {
                _isControlled = false;
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (_isControlled && PivotItems.Count != 0 && SelectedKey != Selected?.ItemKey)
            {
                SetSelection();
            }

            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                SetSelection(firstRender);
            }
            base.OnAfterRender(firstRender);
            if (_redraw && !HeadersOnly)
            {
                _redraw = false;
                StateHasChanged();
            }

        }

        private void SetSelection(bool firstRender = false)
        {
            if (!_isControlled && firstRender)
            {
                if (!string.IsNullOrWhiteSpace(DefaultSelectedKey) && PivotItems.FirstOrDefault(item => item.ItemKey == DefaultSelectedKey) != null)
                {
                    Selected = PivotItems.FirstOrDefault(item => item.ItemKey == DefaultSelectedKey);
                }
                else if (DefaultSelectedIndex.HasValue && DefaultSelectedIndex < PivotItems.Count())
                {
                    Selected = PivotItems.ElementAt(DefaultSelectedIndex.Value);
                }
                else
                {
                    Selected = PivotItems.FirstOrDefault();
                }
            }
            else if (_isControlled)
            {
                if (!string.IsNullOrWhiteSpace(SelectedKey) && PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey) != null)
                {
                    Selected = PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey);
                }
                else
                {
                    Selected = PivotItems.FirstOrDefault();
                }
            }
            return;
        }
    }
}