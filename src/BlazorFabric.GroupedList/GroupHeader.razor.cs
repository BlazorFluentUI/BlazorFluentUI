using BlazorFabric.Style;
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
        public int GroupLevel { get; set; }

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
        public EventCallback OnClick { get; set; }

        [Parameter]
        public EventCallback OnToggle { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

         protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }
       

        public void OnToggleOpen(MouseEventArgs mouseEventArgs)
        {
            IsOpenChanged.InvokeAsync(!IsOpen);
            //isLoadingVisible = !isCollapsed && IsGroupLoading != null; // && IsGroupLoading(group);
            
        }


        private ICollection<Rule> CreateGlobalCss()
        {
            var groupHeaderRules = new HashSet<Rule>();

            // Root
            var rootFocusStyleProps = new FocusStyleProps(this.Theme);
            var rootMergeStyleResults = FocusStyle.GetFocusStyle(rootFocusStyleProps, ".ms-GroupHeader");
            foreach (var rule in rootMergeStyleResults.AddRules)
                groupHeaderRules.Add(rule);

            groupHeaderRules.Add(new Rule()
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
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ListItemBackgroundHovered};" + 
                          $"color:{Theme.SemanticTextColors.ActionLinkHovered};" 
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader:hover .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" 
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-GroupHeader:focus .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });

            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupedList-group.is-dropping > .ms-GroupHeader .ms-GroupHeader-dropIcon" },
                Properties = new CssString()
                {
                    Css = $"transition:transform var(--animation-DURATION_4) cubic-bezier(0.075, 0.820, 0.165, 1.000), opacity var(--animation-DURATION_1) cubic-bezier(0.390, 0.575, 0.565, 1.000);" +
                          $"transition-delay:var(--animation-DURATION_3);" +
                          $"opacity: 1;"+
                          $"transform:rotate(0.2deg) scale(1);" // rotation prevents jittery motion in IE
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupedList-group.is-dropping > .ms-GroupHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity: 0;"
                }
            });

            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ListItemBackgroundChecked};"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ListItemBackgroundCheckedHovered};"
                }
            });
            //groupHeaderRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected .ms-GroupHeader-check" },
            //    Properties = new CssString()
            //    {
            //        Css = $"opacity:1;"
            //    }
            //});


            //GroupHeaderContainer
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-groupHeaderContainer" },
                Properties = new CssString()
                {
                    Css = $"display:flex;"+
                          $"align-items:center;" +
                          $"height:48px;"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader--compact .ms-GroupHeader-groupHeaderContainer" },
                Properties = new CssString()
                {
                    Css = $"height:40px;"
                }
            });

            //HeaderCount
            groupHeaderRules.Add(new Rule()
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
                groupHeaderRules.Add(rule);

            groupHeaderRules.Add(new Rule()
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
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-GroupHeader-check:focus" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });

            //Expand           
            var expandMergeStyleResults = FocusStyle.GetFocusStyle(rootFocusStyleProps, ".ms-GroupHeader-expand");
            foreach (var rule in expandMergeStyleResults.AddRules)
                groupHeaderRules.Add(rule);

            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-expand" },
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
                          $"font-size:{Theme.FontStyle.FontSize.Small};"+
                          $"width:48px;" +
                          $"height:48px;"+
                          $"color:{Theme.Palette.NeutralSecondary};"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader--compact .ms-GroupHeader-expand" },
                Properties = new CssString()
                {
                    Css = $"height:40px;"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected .ms-GroupHeader-expand" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-expand:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralLight};"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected .ms-GroupHeader-expand:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralQuaternary};"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-expand:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralQuaternaryAlt};"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-selected .ms-GroupHeader-expand:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralTertiaryAlt};"
                }
            });

            //ExpandIsCollapsed
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-expandIsCollapsed" },
                Properties = new CssString()
                {
                    Css = $"transform:rotate(90deg);" +
                          $"transform-origin:50% 50%;" +
                          $"transition:transform .1s linear;" 
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-collapsed .ms-GroupHeader-expandIsCollapsed" },
                Properties = new CssString()
                {
                    Css = $"transform:rotate(0deg);"                 }
            });

            //Title
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-title" },
                Properties = new CssString()
                {
                    Css = $"padding-left:12px;" +
                          $"font-size:{Theme.FontStyle.FontSize.MediumPlus};" +
                          $"font-weight:{Theme.FontStyle.FontWeight.SemiBold};"+
                          $"cursor:pointer;"+
                          $"outline:0;"+
                          $"white-space:nowrap;"+
                          $"text-overflow:ellipsis;"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader--compact .ms-GroupHeader-title" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.Medium};" 
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader.is-collapsed .ms-GroupHeader-title" },
                Properties = new CssString()
                {
                    Css = $"font-weight:{Theme.FontStyle.FontWeight.Regular};"
                }
            });


            //DropIcon
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-dropIcon" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                          $"left:-26px;" +
                          $"font-size:20px;" + //used IconFontSize theme style which we haven't implemented yet.
                          $"color:{Theme.Palette.NeutralSecondary};" +
                          $"transition:transform var(--animation-DURATION_2) cubic-bezier(0.600, -0.280, 0.735, 0.045), opacity var(--animation-DURATION_4) cubic-bezier(0.390, 0.575, 0.565, 1.000);" +
                          $"opacity:0;" +
                          $"transform:rotate(0.2deg) scale(0.65);"+
                          $"transform-origin:10px 10px;"
                }
            });
            groupHeaderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-GroupHeader-dropIcon .ms-Icon--Tag" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" 
                }
            });

            return groupHeaderRules;
        }

    }
}
