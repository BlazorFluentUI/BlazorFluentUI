using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class Pivot : FluentUIComponentBase
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
        public virtual PivotItem Selected
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

        protected PivotItem _selected;
        private bool _isControlled;
        private string _selectedKey;
        protected bool _redraw;
        protected RenderFragment _oldChildContent;
        protected int _oldIndex = 0;

        protected override void OnInitialized()
        {
            PivotItems = new List<PivotItem>();
            if (SelectedKey != null)
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
            if (_redraw && !HeadersOnly)
            {
                _redraw = false;
                StateHasChanged();
            }
            base.OnAfterRender(firstRender);

        }

        protected virtual void SetSelection(bool firstRender = false)
        {
            if (!_isControlled && firstRender)
            {
                if (!string.IsNullOrWhiteSpace(DefaultSelectedKey) && PivotItems.FirstOrDefault(item => item.ItemKey == DefaultSelectedKey) != null)
                {
                    _selected = PivotItems.FirstOrDefault(item => item.ItemKey == DefaultSelectedKey);
                }
                else if (DefaultSelectedIndex.HasValue && DefaultSelectedIndex < PivotItems.Count())
                {
                    _selected = PivotItems.ElementAt(DefaultSelectedIndex.Value);
                }
                else
                {
                    _selected = PivotItems.FirstOrDefault();
                }
                _oldIndex = PivotItems.IndexOf(_selected);
                _oldChildContent = _selected?.ChildContent;
                StateHasChanged();
            }
            else if (_isControlled)
            {
                if (!string.IsNullOrWhiteSpace(SelectedKey) && PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey) != null)
                {
                    if (firstRender)
                    {
                        _selected = PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey);
                        _oldIndex = PivotItems.IndexOf(_selected);
                        _oldChildContent = _selected?.ChildContent;
                        StateHasChanged();
                    }
                    else
                    {
                        Selected = PivotItems.FirstOrDefault(item => item.ItemKey == SelectedKey);
                    }
                }
                else
                {
                    if (firstRender)
                    {
                        _selected = PivotItems.FirstOrDefault();
                        _oldIndex = PivotItems.IndexOf(_selected);
                        _oldChildContent = _selected?.ChildContent;
                        StateHasChanged();
                    }
                    else
                    {
                        Selected = PivotItems.FirstOrDefault();
                    }
                }

            }
            return;
        }

       
    }
}