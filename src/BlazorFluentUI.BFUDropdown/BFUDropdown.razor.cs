using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUDropdown<TItem> : BFUResponsiveComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public IEnumerable<string> DefaultSelectedKeys { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public int DropdownWidth { get; set; } = 0;
        [Parameter] public string ErrorMessage { get; set; }
        [Parameter] public IList<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public bool MultiSelect { get; set; }
        [Parameter] public EventCallback<(string itemKey, bool isAdded)> OnChange { get; set; } 
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool Required { get; set; }
        [Parameter] public ResponsiveMode ResponsiveMode { get; set; }
        [Parameter] public string SelectedKey { get; set; }
        [Parameter] public EventCallback<string> SelectedKeyChanged { get; set; }
        [Parameter] public List<string> SelectedKeys { get; set; } = new List<string>();
        [Parameter] public EventCallback<List<string>> SelectedKeysChanged { get; set; }

        [Inject]
        private IJSRuntime jSRuntime { get; set; }

        protected bool isOpen { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected bool isSmall = false;
        protected Rectangle dropDownBounds = new Rectangle();

        private ElementReference calloutReference;
        private ElementReference panelReference;
        private ElementReference _chosenReference;
        private string _registrationToken;

        private BFUFocusZone calloutFocusZone;
        private CalloutPositionedInfo _calloutPositionedInfo;

        //private bool firstRender = true;

        #region Style
        private ICollection<IRule> DropdownLocalRules { get; set; } = new List<IRule>();

        private Rule DropdownTitleOpenRule = new Rule();
        private Rule DropdownCalloutRule = new Rule();
        private Rule DropdownCalloutMainRule = new Rule();
        #endregion

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            SetStyle();
            return base.OnInitializedAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        public void ResetSelection()
        {
            SelectedKeys.Clear();
            SelectedKey = null;

            if (MultiSelect)
            {
                if (SelectedKeysChanged.HasDelegate)
                    SelectedKeysChanged.InvokeAsync(SelectedKeys);
            }
            else
            {
                if (SelectedKeyChanged.HasDelegate)
                    SelectedKeyChanged.InvokeAsync(SelectedKey);
            }
            StateHasChanged();
        }

        public void AddSelection(string key)
        {
            if (MultiSelect)
            {
                if (SelectedKeys.Contains(key))
                    throw new Exception("This key was already selected.");

                if (OnChange.HasDelegate)
                    OnChange.InvokeAsync((key, true));

                SelectedKeys.Add(key);

                if (SelectedKeysChanged.HasDelegate)
                    SelectedKeysChanged.InvokeAsync(SelectedKeys);
            }
            else
            {
                if (SelectedKey!= key)
                {
                    SelectedKey = key;
                    if (OnChange.HasDelegate)
                        OnChange.InvokeAsync((key, true));
                    if (SelectedKeyChanged.HasDelegate)
                        SelectedKeyChanged.InvokeAsync(SelectedKey);
                }
                isOpen = false;
            }
            StateHasChanged();
        }

        public void RemoveSelection(string key)
        {
            if (MultiSelect)
            {
                if (!SelectedKeys.Contains(key))
                    throw new Exception("This key was not already selected.");

                if (OnChange.HasDelegate)
                    OnChange.InvokeAsync((key, false));

                SelectedKeys.Remove(key);  //this used to be following the next command.  A bug?  I moved it here...

                if (SelectedKeysChanged.HasDelegate)
                    SelectedKeysChanged.InvokeAsync(SelectedKeys);

            }
            else
            {
                if (SelectedKey != null)
                {
                    SelectedKey = null;
                    if (OnChange.HasDelegate)
                        OnChange.InvokeAsync((key, false));

                    if (SelectedKeyChanged.HasDelegate)
                        SelectedKeyChanged.InvokeAsync(SelectedKey);
                }
            }
            StateHasChanged();
        }

        [JSInvokable]
        public override async Task OnResizedAsync(double windowWidth, double windowHeight)
        {
            var oldBounds = dropDownBounds;
            dropDownBounds = await this.GetBoundsAsync();
            if (oldBounds.width != dropDownBounds.width)
            {
                StateHasChanged();
            }
            await base.OnResizedAsync(windowWidth, windowHeight);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
                dropDownBounds = await this.GetBoundsAsync();
                //firstRender = false;
                StateHasChanged();
            }
            if (isOpen && _registrationToken == null)
                await RegisterListFocusAsync();

            if (!isOpen && _registrationToken != null)
                await DeregisterListFocusAsync();

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (this.DefaultSelectedKeys != null)
            {
                foreach (var key in this.DefaultSelectedKeys)
                    AddSelection(key);
            }
        }

        private async Task RegisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await DeregisterListFocusAsync();
            }
            if ((int)CurrentMode <= (int)ResponsiveMode.Medium)
                _chosenReference = panelReference;
            else
                _chosenReference = calloutReference;
            _registrationToken = await jSRuntime.InvokeAsync<string>("BlazorFluentUiBaseComponent.registerKeyEventsForList", _chosenReference);
        }

        private async Task DeregisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await jSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.deregisterKeyEventsForList", _registrationToken);
                _registrationToken = null;
            }
        }

        private void OnPositioned(CalloutPositionedInfo calloutPositionedInfo)
        {
            _calloutPositionedInfo = calloutPositionedInfo;
            SetStyle();
            calloutFocusZone.FocusFirstElement();
        }

        private Task KeydownHandler(KeyboardEventArgs args)
        {
            bool containsExpandCollapseModifier = args.AltKey || args.MetaKey;
            switch (args.Key)
            {
                case "Enter":
                case " ":
                    isOpen = !isOpen;
                    break;
                case "Escape":
                    isOpen = false;
                    break;
                case "ArrowDown":
                    if (containsExpandCollapseModifier)
                    {
                        isOpen = true;
                    }
                    break;
            }
            return Task.CompletedTask;
        }

        protected Task ClickHandler(MouseEventArgs args)
        {
            if (!this.Disabled)
                isOpen = !isOpen;  //There is a problem here.  Clicking when open causes automatic dismissal (light dismiss) so this just opens it again.
            return Task.CompletedTask;
        }
        protected Task FocusHandler(FocusEventArgs args)
        {
            // Could write logic to open on focus automatically...
            //isOpen = true;
            return Task.CompletedTask;
        }

        protected async Task DismissHandler()
        {
            isOpen = false;
        }

        private void CreateLocalCss()
        {
            DropdownTitleOpenRule.Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-open .ms-Dropdown-title" };
            DropdownCalloutRule.Selector = new ClassSelector() { SelectorName = "ms-Dropdown-callout" };
            DropdownCalloutMainRule.Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-callout .ms-Callout-main" };

            DropdownLocalRules.Add(DropdownTitleOpenRule);
            DropdownLocalRules.Add(DropdownCalloutRule);
            DropdownLocalRules.Add(DropdownCalloutMainRule);
        }

        private void SetStyle()
        {
            DropdownTitleOpenRule.Properties = new CssString()
            {
                Css = $"border-radius:{(_calloutPositionedInfo?.TargetEdge == RectangleEdge.Bottom ? $"{Theme.Effects.RoundedCorner2} {Theme.Effects.RoundedCorner2} 0 0" : $"0 0 {Theme.Effects.RoundedCorner2} {Theme.Effects.RoundedCorner2}")};"
            };
            DropdownCalloutRule.Properties = new CssString()
            {
                Css = $"border-radius:{(_calloutPositionedInfo?.TargetEdge == RectangleEdge.Bottom ? $"0 0 {Theme.Effects.RoundedCorner2} {Theme.Effects.RoundedCorner2}" : $"{Theme.Effects.RoundedCorner2} {Theme.Effects.RoundedCorner2} 0 0")};"
            };
            DropdownCalloutMainRule.Properties = new CssString()
            {
                Css = $"border-radius:{(_calloutPositionedInfo?.TargetEdge == RectangleEdge.Bottom ? $"0 0 {Theme.Effects.RoundedCorner2} {Theme.Effects.RoundedCorner2}" : $"{Theme.Effects.RoundedCorner2} {Theme.Effects.RoundedCorner2} 0 0")};"
            };


        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var dropdownRules = new HashSet<IRule>();
            #region Root
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown" },
                Properties = new CssString()
                {
                    Css = $"box-shadow:none;"+
                          $"margin:0;"+
                          $"padding:0;"+
                          $"box-sizing:border-box;"+
                          $"font-size:{theme.FontStyle.FontSize.Medium};"+
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};"+
                          $"color:{theme.SemanticTextColors.MenuItemText};" +
                          $"border-color:{theme.SemanticColors.FocusBorder};"+
                          $"position:relative;"+
                          $"outline:0;"+
                          $"user-select:none;"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:focus:after" },
                Properties = new CssString()
                {
                    Css = $"pointer-events:none;" +
                         $"content:'';" +
                         $"position:absolute;" +
                         $"box-sizing:border-box;" +
                         $"top:0;" +
                         $"left:0;" +
                         $"width:100%;" +
                         $"height:100%;" +
                         $"border:2px solid {theme.Palette.ThemePrimary};" +
                         $"border-radius:2px;"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-disabled:focus:after" },
                Properties = new CssString()
                {
                    Css = $"border:none;"
                }
            });

            #endregion
            #region Title
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"box-shadow:none;" +
                          $"margin:0;" +
                          $"padding:0;" +
                          $"box-sizing:border-box;" + 
                          $"background-color:{theme.SemanticColors.InputBackground};" +
                          $"border-width:1px;" +
                          $"border-style:solid;" +
                          $"border-color:{theme.SemanticColors.InputBorder};" +
                          $"border-radius:{theme.Effects.RoundedCorner2};"+  //local style when is-open
                          $"cursor:pointer;"+
                          $"display:block;"+
                          $"height:32px;"+
                          $"line-height:30px;"+
                          $"padding:0 28px 0 8px;"+
                          $"position:relative;" +
                          $"overflow:hidden;" +
                          $"white-space:nowrap;" +
                          $"text-overflow:ellipsis;"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-open .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.Palette.ThemePrimary};" 
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-placeholder .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.InputPlaceholderText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-error .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.Palette.Red};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-error.is-open .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.Palette.RedDark};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-disabled .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.SemanticColors.DisabledBackground};"+
                          $"border:none;"+
                          $"color:{theme.Palette.NeutralTertiary};"+
                          $"cursor:default;"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:hover .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.Palette.NeutralPrimary};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-open:hover .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.Palette.NeutralSecondary};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:not(.is-disabled):hover .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemTextHovered};"
                }
            });

            //dropdownRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:focus .ms-Dropdown-title" },
            //    Properties = new CssString()
            //    {
            //        Css = $"border-color:{theme.Palette.NeutralPrimary};"
            //    }
            //});
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:not(.is-disabled):focus .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemTextHovered};"
                }
            });

           
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:active .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"border-color:{theme.Palette.ThemePrimary};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:not(.is-disabled):active .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemTextHovered};"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:not(.is-disabled):hover .ms-Dropdown-caretDown" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:not(.is-disabled):focus .ms-Dropdown-caretDown" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown:not(.is-disabled):active .ms-Dropdown-caretDown" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemText};"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-placeholder:not(.is-disabled):hover .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-placeholder:not(.is-disabled):focus .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-placeholder:not(.is-disabled):active .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemText};"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-error:hover .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ErrorText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-error:focus .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ErrorText};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.has-error:active .ms-Dropdown-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ErrorText};"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-required:not(.has-label):after" },
                Properties = new CssString()
                {
                    Css = $"content:'*';"+
                          $"color:{theme.SemanticTextColors.ErrorText};"+
                          $"position:absolute;"+
                          $"top:-5px;"+
                          $"right:-10px;"
                }
            });
            #endregion
            #region CaretDownWrapper
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-caretDownWrapper" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                          $"top:1px;" +
                          $"right:8px;"+
                          $"height:32px;"+
                          $"line-height:30px;"+
                          $"cursor:pointer;"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-disabled .ms-Dropdown-caretDownWrapper" },
                Properties = new CssString()
                {
                    Css = $"cursor:default;"
                }
            });
            #endregion
            #region CaretDown
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-caretDown" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralSecondary};" +
                          $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"pointer-events:none;"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown.is-disabled .ms-Dropdown-caretDown" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralTertiary};"
                }
            });
            #endregion
            #region ErrorMessage
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-errorMessage" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};"+
                          $"color:{theme.Palette.RedDark};" +
                          $"padding-top:5px;"
                }
            });
            #endregion
            #region Callout
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-callout" },
                Properties = new CssString()
                {
                    Css = $"border:none;" +
                          $"box-shadow:{theme.Effects.Elevation8};"
                }
            });
            #endregion
            #region ItemsWrapper
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-itemsWrapper:focus" },
                Properties = new CssString()
                {
                    Css = $"outline:0;"
                }
            });
            #endregion
            #region Items
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-items" },
                Properties = new CssString()
                {
                    Css = $"display:block;"
                }
            });
            #endregion
            #region ItemsHeader
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-itemHeader" },
                Properties = new CssString()
                {
                    Css = $"font-weight:{theme.FontStyle.FontWeight.SemiBold};"+
                          $"color:{theme.SemanticColors.MenuHeader};"+
                          $"background:none;"+
                          $"background-color:transparent;"+
                          $"border:none;"+
                          $"height:36px;"+
                          $"line-height:36px;"+
                          $"cursor:default;"+
                          $"padding:0 8px;"+
                          $"user-select:none;"+
                          $"text-align:left;"
                }
            });
            #endregion
            #region Divider
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-divider" },
                Properties = new CssString()
                {
                    Css = $"height:1px;" +
                          $"background-color:{theme.SemanticColors.BodyDivider};"
                }
            });
            #endregion
            #region Item
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;" +
                          $"box-sizing:border-box;"+
                          $"cursor:pointer;" +
                          $"display:flex;" +
                          $"align-items:center;" +
                          $"padding:0 8px;" +
                          $"width:100%;" +
                          $"min-height:36px;" +
                          $"line-height:20px;" +
                          $"height:auto;" +
                          $"position:relative;"+
                          $"border:1px solid transparent;"+
                          $"border-radius:0;"+
                          $"word-wrap:break-word;"+
                          $"overflow-wrap:break-word;"+
                          $"text-align:left;"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-itemsWrapper .ms-Dropdown-dropdownItems .ms-Dropdown-item.ms-Button" },
                Properties = new CssString()
                {
                    Css = $"padding:0 8px;"  // not sure if this is still needed... a hack for specificity of css rules.
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-itemsWrapper .ms-Dropdown-dropdownItems .ms-Dropdown-item:not(.ms-Checkbox-checkbox):hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemTextHovered};"+
                          $"background-color:{theme.SemanticColors.MenuItemBackgroundHovered};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item:focus" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item:active" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemTextHovered};" +
                          $"background-color:{theme.SemanticColors.MenuBackground};"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item.selected, .ms-Dropdown-item.ms-Button.ms-Button--action.selected" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.MenuItemTextHovered};" +
                         $"background-color:{theme.SemanticColors.MenuItemBackgroundPressed};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item.selected:hover:focus, .ms-Dropdown-item.ms-Button.ms-Button--action.selected:hover:focus" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.SemanticColors.MenuItemBackgroundPressed};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item.selected:focus, .ms-Dropdown-item.ms-Button.ms-Button--action.selected:focus" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.SemanticColors.MenuItemBackgroundPressed};"
                }
            });
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item.selected:active, .ms-Dropdown-item.ms-Button.ms-Button--action.selected:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.SemanticColors.MenuItemBackgroundHovered};"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Dropdown-item:focus:after" },
                Properties = new CssString()
                {
                    Css = $"left:0;"+
                          $"top:0;"+
                          $"bottom:0;" +
                          $"right:0;" 
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item.is-disabled" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralTertiary};"+
                          $"cursor:default;"
                }
            });

            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-item.is-hidden" },
                Properties = new CssString()
                {
                    Css = $"display:none;"
                }
            });

            #endregion
            #region OptionText
            dropdownRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dropdown-optionText" },
                Properties = new CssString()
                {
                    Css = $"overflow:hidden;"+
                          $"white-space:nowrap;"+
                          $"text-overflow:ellipsis;"+
                          $"min-width:0;"+
                          $"max-width:100%;"+
                          $"word-wrap:break-word;"+
                          $"overflow-wrap:break-word;"+
                          $"margin:1px;"
                }
            });
            #endregion

            return dropdownRules;
        }


    }
}
