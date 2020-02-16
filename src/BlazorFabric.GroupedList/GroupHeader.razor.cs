using BlazorFabric.BaseComponent.FocusStyle;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GroupHeader : FabricComponentBase
    {
        
        bool isLoadingVisible;

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public int Count { get; set; }

        [Parameter]
        public bool CurrentlySelected { get; set; }

        [Parameter]
        public bool HasMoreData { get; set; }

        [Parameter]
        public bool IsOpen { get; set; }

        [Parameter]
        public EventCallback<bool> IsOpenChanged { get; set; }

        [Parameter]
        public Func<object,bool> IsGroupLoading { get; set; }

        [Parameter]
        public bool IsSelectionCheckVisible { get; set; }

        [Parameter]
        public string LoadingText { get; set; } = "Loading...";

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        private ICollection<Rule> GroupHeaderGlobalRules { get; set; } = new System.Collections.Generic.List<Rule>();

        protected override Task OnInitializedAsync()
        {
            if (!CStyle.ComponentStyleExist(this))
            {
                CreateCss();
            }
            return base.OnInitializedAsync();
        }
        protected override void OnThemeChanged()
        {
            CreateCss();
            base.OnThemeChanged();
        }

        public void OnToggleSelectGroupClick(MouseEventArgs mouseEventArgs)
        {

        }

        public void OnToggleOpen(MouseEventArgs mouseEventArgs)
        {
            IsOpenChanged.InvokeAsync(!IsOpen);
            //isLoadingVisible = !isCollapsed && IsGroupLoading != null; // && IsGroupLoading(group);
            
        }


        protected void CreateCss()
        {
            GroupHeaderGlobalRules.Clear();

            // Root
            var rootFocusStyleProps = new FocusStyleProps(this.Theme);
            var rootMergeStyleResults = FocusStyle.GetFocusStyle(rootFocusStyleProps, ".ms-GroupHeader");
            foreach (var rule in rootMergeStyleResults.AddRules)
                GroupHeaderGlobalRules.Add(rule);

            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader" },
                Properties = new CssString()
                {
                    Css = rootMergeStyleResults.MergeRules +
                          $"border-bottom:1px solid {Theme.SemanticColors.ListBackground};" + // keep the border for height but color it so it's invisible.
                          $"cursor:default;" +
                          $"user-select:none;"
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ListItemBackgroundHovered};" + 
                          $"color:{Theme.SemanticTextColors.ActionLinkHovered};" 
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader:hover .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" 
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-GroupHeader:focus .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });

            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupedList-group.is-dropping > .ms-GroupHeader .ms-GroupHeader-dropIcon" },
                Properties = new CssString()
                {
                    Css = $"transition:transform var(--animation-DURATION_4) cubic-bezier(0.075, 0.820, 0.165, 1.000) opacity var(--animation-DURATION_1) cubic-bezier(0.390, 0.575, 0.565, 1.000);" +
                          $"transition-delay:var(--animation-DURATION_3);" +
                          $"opacity: 1;"+
                          $"transform:rotate(0.2deg) scale(1);" // rotation prevents jittery motion in IE
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupedList-group.is-dropping > .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity: 0;"
                }
            });

            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ListItemBackgroundChecked};"
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ListItemBackgroundCheckedHovered};"
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });


            //GroupHeaderContainer
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-groupHeaderContainer" },
                Properties = new CssString()
                {
                    Css = $"display:flex;"+
                          $"align-items:center;" +
                          $"height:48px;"
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader--compact .ms-GroupHeader-groupHeaderContainer" },
                Properties = new CssString()
                {
                    Css = $"height:40px;"
                }
            });

            //HeaderCount
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-headerCount" },
                Properties = new CssString()
                {
                    Css = $"padding:0px 4px;"
                }
            });


            //Check           
            var checkMergeStyleResults = FocusStyle.GetFocusStyle(rootFocusStyleProps, ".ms-GroupHeader-check");
            foreach (var rule in checkMergeStyleResults.AddRules)
                GroupHeaderGlobalRules.Add(rule);

            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = checkMergeStyleResults.MergeRules + 
                          $"cursor:default;" +
                          $"background:none;" +
                          $"background-color:transparent;" +
                          $"border:none;" +
                          $"padding:0;" +
                          $"display:flex;" +
                          $"align-items:center;" +
                          $"justify-content:center;" +
                          $"padding-top:1px;" +
                          $"margin-top:-1px;" +
                          $"opacity:0;" +
                          $"width:48px;" +
                          $"height:48px;"
                }
            });
            GroupHeaderGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-GroupHeader-check:focus" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
        }

    }
}
