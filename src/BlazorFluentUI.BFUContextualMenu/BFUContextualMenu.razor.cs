using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUContextualMenu : BFUResponsiveComponentBase, IDisposable
    {
        [Parameter] public bool AlignTargetEdge { get; set; }
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public Rectangle Bounds { get; set; }
        //[Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public IEnumerable<IBFUContextualMenuItem> Items { get; set; }

        [Parameter] public bool CoverTarget { get; set; }
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public BFUComponentBase FabricComponentTarget { get; set; }
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public bool IsBeakVisible { get; set; } = false;

        //[Parameter] public IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<IBFUContextualMenuItem> ItemTemplate { get; set; }
        //[Parameter] public double SubMenuHoverDelay { get; set; } = 250;
        [Parameter] public string Title { get; set; }
        [Parameter] public bool UseTargetWidth { get; set; } = false;
        [Parameter] public bool UseTargetAsMinWidth { get; set; } = false;

        [Parameter] public EventCallback<bool> OnDismiss { get; set; }
        [Parameter] public EventCallback<BFUContextualMenu> OnMenuDismissed { get; set; }
        [Parameter] public EventCallback<BFUContextualMenu> OnMenuOpened { get; set; }

        [Parameter] public bool IsSubMenu { get; set; } = false;

        [Parameter] public bool ShouldFocusOnMount { get; set; }

        // for debugging only
        [CascadingParameter(Name = "PortalId")] public string PortalId { get; set; }

        private bool isOpen = false;

        private bool HasIcons = false; //needed to shift margins and make space for all 
        private bool HasCheckables = false;

        private BFUFocusZone _focusZoneReference;

        public string SubmenuActiveKey { get; set; }
        //public void SetSubmenuActiveKey(string key)
        //{

        //    if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(SubmenuActiveKey))
        //        return;
        //    System.Diagnostics.Debug.WriteLine($"SetSubmenuActiveKey(\"{key}\") from {this.DirectionalHint}");
        //    SubmenuActiveKey = key;
        //    StateHasChanged();
        //}
        private void KeyDownHandler(KeyboardEventArgs args)
        {
            if (args.Key == "ArrowLeft" && IsSubMenu)
            {
                Dismiss(false);
            }
        }


        private void OnCalloutPositioned()
        {
            _focusZoneReference.FocusFirstElement();
        }

        protected Action OnNotifyCalloutDismiss => () =>
        {

        };


        protected void Dismiss(bool dismissAll = false)
        {
            this.OnDismiss.InvokeAsync(dismissAll);
        }

        protected Action OnCalloutDismiss => () =>
        {
            //if (string.IsNullOrEmpty(SubmenuActiveKey))
            //{
            //    if (this.IsSubMenu)
            //        this.OnNotifyCalloutDismiss();
            //    else
            Dismiss(true);
            //}
        };

        protected Action OnOpenSubmenu => () =>
        {

        };

        protected Action<string> OnSetSubmenu => (key) =>
        {
            this.SubmenuActiveKey = key;
        };

        protected override Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating ContextualMenu");
            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (this.Items != null)
            {
                if (this.Items.Count(x => x.IconName != null) > 0)
                    HasIcons = true;
                if (this.Items.Count(x => x.CanCheck == true) > 0)
                    HasCheckables = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await OnMenuOpenedAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnMenuOpenedAsync()
        {
            await this.OnMenuOpened.InvokeAsync(this);
        }

        public override void Dispose()
        {
            this.OnMenuDismissed.InvokeAsync(this);
            base.Dispose();
        }


        private ICollection<IRule> CreateGlobalCss()
        {
            var menuRules = new HashSet<IRule>();
            // ROOT
            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.Medium};"+
                          $"font-weight:{Theme.FontStyle.FontWeight.Regular};"+
                          $"background-color:{Theme.SemanticColors.MenuBackground};" +
                          $"min-width:180px;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-container:focus" },
                Properties = new CssString()
                {
                    Css = $"outline:0;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-list" },
                Properties = new CssString()
                {
                    Css = $"list-style-type:none;" +
                          $"margin:0;" +
                          $"padding:0;" 
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-header" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.Small};" +
                          $"font-weight:{Theme.FontStyle.FontWeight.SemiBold};" +
                          $"color:{Theme.SemanticColors.MenuHeader};" +
                          $"background:none;"+
                          $"background-color:transparent;"+
                          $"border:none;"+
                          $"height:36px;"+
                          $"line-height:36px;"+
                          $"cursor:default;"+
                          $"padding:0 6px;"+
                          $"user-select:none;"+
                          $"text-align:left;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-title" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.MediumPlus};" +
                         $"padding-right:14px;" +
                         $"padding-left:14px;" +
                         $"padding-bottom:5px;" +
                         $"padding-top:5px;" +
                         $"background-color:{Theme.SemanticColors.MenuItemBackgroundPressed};"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu .ms-Callout" },
                Properties = new CssString()
                {
                    Css = $"box-shadow:{Theme.Effects.Elevation8};"
                }
            });

            var focusProps = new Style.FocusStyleProps(Theme);
            var focusStyles = BlazorFluentUI.Style.FocusStyle.GetFocusStyle(focusProps, ".ms-ContextualMenu-item");
            foreach (var rule in focusStyles.AddRules)
                menuRules.Add(rule);

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-item" },
                Properties = new CssString()
                {
                    Css = focusStyles.MergeRules +
                          $"color:{Theme.SemanticTextColors.BodyText};"+
                          $"position:relative;"+
                          $"box-sizing:border-box;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-divider" },
                Properties = new CssString()
                {
                    Css = $"display:block;"+
                          $"height:1px;"+
                         $"background-color:{Theme.SemanticColors.BodyDivider};" +
                         $"position:relative;"
                }
            });

            var linkFocusStyles = BlazorFluentUI.Style.FocusStyle.GetFocusStyle(focusProps, ".ms-ContextualMenu-link");
            foreach (var rule in linkFocusStyles.AddRules)
                menuRules.Add(rule);

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link" },
                Properties = new CssString()
                {
                    Css = linkFocusStyles.MergeRules +
                         $"color:{Theme.SemanticTextColors.BodyText};" +
                         $"background-color:transparent;" +
                         $"border:none;"+
                         $"width:100%;" +
                         $"height:36px;" +
                         $"line-height:36px;" +
                         $"display:block;" +
                         $"cursor:pointer;" +
                         $"padding:0 8px 0 4px;" +
                         $"text-align:left;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-item.is-disabled .ms-ContextualMenu-link" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.DisabledBodyText};" +
                         $"cursor:default;" +
                         $"pointer-events:none;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:focus" },
                Properties = new CssString()
                {
                    Css = $"outline:0;"// +
                         //$"background-color:{Theme.Palette.White};" //!!!!! This won't work with dark mode, doesn't seem to be needed for light mode.
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.MenuItemTextHovered};" +
                         $"background-color:{Theme.SemanticColors.MenuItemBackgroundHovered};"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:hover .ms-ContextualMenu-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDarkAlt};"
                        
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:hover .ms-ContextualMenu-submenuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"

                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-item.is-checked .ms-ContextualMenu-checkmarkIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"

                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.MenuItemBackgroundPressed};"

                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:active .ms-ContextualMenu-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDark};"

                }
            });
            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-link:active .ms-ContextualMenu-submenuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"

                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-item.is-expanded .ms-ContextualMenu-link" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.SemanticColors.MenuItemBackgroundPressed};"+
                          $"color:{Theme.SemanticTextColors.BodyTextChecked};"

                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-linkContent" },
                Properties = new CssString()
                {
                    Css = $"white-space:nowrap;" +
                          $"height:inherit;"+
                          $"display:flex;"+
                          $"align-items:center;"+
                          $"max-width:100%;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-anchorLink" },
                Properties = new CssString()
                {
                    Css = $"padding:0 8px 0 4px;" +
                          $"text-rendering:auto;" +
                          $"color:inherit;" +
                          $"letter-spacing:normal;" +
                          $"word-spacing:normal;" +
                          $"text-transform:none;" +
                          $"text-indent:0px;" +
                          $"text-shadow:none;" +
                          $"text-decoration:none;" +
                          $"box-sizing:border-box;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-itemText" },
                Properties = new CssString()
                {
                    Css = $"margin:0 4px;" +
                         $"vertical-align:middle;" +
                         $"display:inline-block;" +
                         $"flex-grow:1;" +
                         $"text-overflow:ellipsis;" +
                         $"overflow:hidden;" +
                         $"white-space:nowrap;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-secondaryText" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralSecondary};" +
                        $"padding-left:20px;" +
                        $"text-align:right;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-icon" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;" +
                        $"min-height:1px;" +
                        $"max-height:36px;"+
                        $"font-size:{Theme.FontStyle.IconFontSize.Medium};"+
                        $"width:{Theme.FontStyle.IconFontSize.Medium};"+
                        $"margin:0 4px;"+
                        $"vertical-align:middle;"+
                        $"flex-shrink:0;"
                }
            });
            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"@media only screen and (min-width: ${0}px) and (max-width: ${Theme.CommonStyle.ScreenWidthMaxMedium}px)" },
                Properties = new CssString()
                {
                    Css = ".ms-ContextualMenu-icon {" +
                        $"font-size:{Theme.FontStyle.IconFontSize.Large};" +
                        $"width:{Theme.FontStyle.IconFontSize.Large};" +
                        "}"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-iconColor" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticColors.MenuIcon};"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu.is-disabled .ms-ContextualMenu-iconColor" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.DisabledBodyText};"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-checkmarkIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.BodySubtext};"+
                          $"margin: 0 4px;"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-submenuIcon" },
                Properties = new CssString()
                {
                    Css = $"height:36px;" +
                       $"line-height:36px;" +
                       $"color:{Theme.Palette.NeutralSecondary};" +
                       $"text-align:center;" +
                       $"display:inline-block;" +
                       $"vertical-align:middle;" +
                       $"flex-shrink:0;"+
                       $"font-size:{Theme.FontStyle.IconFontSize.Small};"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-submenuIcon:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"
                }
            });
            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-submenuIcon:active" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"
                }
            });

            menuRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-ContextualMenu-splitButtonFlexContainer" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                          $"height:36px;"+
                          $"flex-wrap:nowrap;"+
                          $"justify-content:center;"+
                          $"align-items:flex-start;"
                }
            });

            return menuRules;
        }
    }
}