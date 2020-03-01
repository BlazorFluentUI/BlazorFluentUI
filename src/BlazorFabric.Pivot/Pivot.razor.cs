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

        private void SetSelection(bool firstRender = false)
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

        private ICollection<Rule> CreateGlobalCss()
        {
            var MyRules = new List<Rule>();
            #region ms-Pivot-count
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-count" },
                Properties = new CssString()
                {
                    Css = $"display: inline-block;" +
                            $"vertical-align:top;"
                }
            });
            #endregion
            #region ms-Pivot-icon
            #endregion
            #region is-selected
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".is-selected.ms-Pivot-link" },
                Properties = new CssString()
                {
                    Css = $"font-weight:{Theme.FontStyle.FontWeight.SemiBold};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".is-selected.ms-Pivot-link::before" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.InputBackgroundChecked};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".is-selected.ms-Pivot-link:hover::before" },
                Properties = new CssString()
                {
                    Css = $"left:0;" +
                            $"right:0;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".is-selected.ms-Pivot-link::before{background-color:Highlight;}" +
                            ".is-selected.ms-Pivot-link{color:Highlight;}"
                }
            });
            #endregion
            #region ms-Pivot-link
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-link" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ActionLink};" +
                            $"display:inline-block;" +
                            $"line-height:44px;" +
                            $"height:44px;" +
                            $"margin-right:8px;" +
                            $"padding:0 8px;" +
                            $"text-align:center;" +
                            $"position:relative;" +
                            $"background-color:transparent;" +
                            $"border:0;" +
                            $"border-radius:0;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-link::before" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;" +
                            $"bottom:0;" +
                            $"content:{'"'}{'"'};" +
                            $"height:2px;" +
                            $"left:8px;" +
                            $"position:absolute;" +
                            $"right:8px;" +
                            $"transition:left var(--animation-DURATION_2) var(--animation-EASING_FUNCTION_2), right var(--animation-DURATION_2) var(--animation-EASING_FUNCTION_2);"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-link::after" },
                Properties = new CssString()
                {
                    Css = $"color:transparent;" +
                            $"content:attr(data-content);" +
                            $"display:block;" +
                            $"font-weight:{Theme.FontStyle.FontWeight.Bold}" +
                            $"height:1px;" +
                            $"overflow:hidden;" +
                            $"visibility:hidden;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-link:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.ButtonBackgroundHovered};" +
                            $"color:{Theme.SemanticTextColors.ButtonTextHovered};" +
                            $"cursor:pointer;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-link:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.ButtonBackgroundPressed};" +
                            $"color:{Theme.SemanticTextColors.ButtonTextHovered};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-link:focus" },
                Properties = new CssString()
                {
                    Css = $"outline:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Pivot-link:focus" },
                Properties = new CssString()
                {
                    Css = $"outline:1px solid {Theme.SemanticColors.FocusBorder};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Pivot-link:focus::after" },
                Properties = new CssString()
                {
                    Css = $"content:attr(data-content);" +
                            $"position:relative;" +
                            $"border:0;"
                }
            });
            #endregion
            #region ms-Pivot-linkContent
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-linkContent" },
                Properties = new CssString()
                {
                    Css = $"flex:0 1 100%;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-linkContent > *" },
                Properties = new CssString()
                {
                    Css = $"margin-left:4px;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-linkContent > *:first-child" },
                Properties = new CssString()
                {
                    Css = $"margin-left:0;"
                }
            });
            #endregion
            #region ms-Pivot
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                            $"color:{Theme.SemanticTextColors.Link};" +
                            $"white-space:nowrap;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot.ms-Pivot--large .ms-Pivot-link" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.Large};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot.ms-Pivot--tabs .ms-Pivot-link" },
                Properties = new CssString()
                {
                    Css = $"margin-right:0;" +
                            $"height:44px;" +
                            $"line-height:44px;" +
                            $"background-color:{Theme.SemanticColors.ButtonBackground};" +
                            $"padding:0 10px;" +
                            $"vertical-align:top;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot.ms-Pivot--tabs .ms-Pivot-link:focus" },
                Properties = new CssString()
                {
                    Css = $"outline-offset:-1px;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot.ms-Pivot--tabs .ms-Pivot-link:not(.is-selected):hover, .ms-Pivot.ms-Pivot--tabs .ms-Pivot-link:not(.is-selected):focus" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ButtonTextCheckedHovered};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot.ms-Pivot--tabs .ms-Pivot-link:not(.is-selected):active, .ms-Pivot.ms-Pivot--tabs .ms-Pivot-link:not(.is-selected):hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.PrimaryButtonText};" +
                            $"background-color:{Theme.SemanticColors.PrimaryButtonBackground};"
                }
            });
            #endregion
            #region ms-Pivot--large

            #endregion
            #region ms-Pivot--tabs
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot--tabs.ms-Pivot .is-selected" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.PrimaryButtonBackground};" +
                            $"color:{Theme.SemanticTextColors.PrimaryButtonText};" +
                            $"font-weight:{Theme.FontStyle.FontWeight.Regular};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot--tabs.ms-Pivot .is-selected::before" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;" +
                            $"transition:none;" +
                            $"position: absolute;" +
                            $"top:0;" +
                            $"left:0;" +
                            $"right:0;" +
                            $"bottom:0;" +
                            $"content:{'"'}{'"'};" +
                            $"height:auto;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot--tabs.ms-Pivot .is-selected:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.PrimaryButtonBackgroundHovered};" +
                            $"color:{Theme.SemanticTextColors.PrimaryButtonText};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot--tabs.ms-Pivot .is-selected:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.PrimaryButtonBackgroundPressed};" +
                            $"color:{Theme.SemanticTextColors.PrimaryButtonText};"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Pivot--tabs.ms-Pivot .ms-Pivot-link:focus::before" },
                Properties = new CssString()
                {
                    Css = $"height:auto;" +
                            $"background-color:transparent;" +
                            $"transition:none;"
                }
            });
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".ms-Pivot--tabs.ms-Pivot .is-selected{{font-weight:{Theme.FontStyle.FontWeight.SemiBold};color:HighlightText;background-color:Highlight;-ms-high-contrast-adjust:none;}}"
                }
            });
            #endregion
            #region ms-Pivot-text
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Pivot-text" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;" +
                            $"vertical-align:top;"
                }
            });
            #endregion

            return MyRules;
        }
    }
}