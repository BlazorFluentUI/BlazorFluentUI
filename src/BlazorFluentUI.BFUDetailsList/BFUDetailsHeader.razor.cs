using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUDetailsHeader<TItem> : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [CascadingParameter]
        private BFUSelectionZone<TItem> SelectionZone { get; set; }

        [Parameter]
        public string AriaLabelForSelectAllCheckbox { get; set; }

        [Parameter]
        public string AriaLabelForSelectionColumn { get; set; }

        [Parameter]
        public string AriaLabelForToggleAllGroup { get; set; }

        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; }

        [Parameter]
        public CollapseAllVisibility CollapseAllVisibility { get; set; }

        [Parameter]
        public RenderFragment<object> ColumnHeaderTooltipTemplate { get; set; }

        [Parameter]
        public object ColumnReorderOptions { get; set; }

        [Parameter]
        public object ColumnReorderProps { get; set; }

        [Parameter]
        public IEnumerable<BFUDetailsRowColumn<TItem>> Columns { get; set; }
               
        [Parameter]
        public RenderFragment DetailsCheckboxTemplate { get; set; }

        [Parameter]
        public int GroupNestingDepth { get; set; }

        [Parameter]
        public bool IsAllCollapsed { get; set; }

        [Parameter]
        public double IndentWidth { get; set; }

        [Parameter]
        public bool IsAllSelected { get; set; }

        [Parameter]
        public DetailsListLayoutMode LayoutMode { get; set; }

        [Parameter]
        public int MinimumPixelsForDrag { get; set; }

        [Parameter]
        public EventCallback OnAllSelected { get; set; }

        [Parameter]
        public EventCallback<ItemContainer<BFUDetailsRowColumn<TItem>>> OnColumnAutoResized { get; set; }

        [Parameter]
        public EventCallback<BFUDetailsRowColumn<TItem>> OnColumnClick { get; set; }

        [Parameter]
        public EventCallback<BFUDetailsRowColumn<TItem>> OnColumnContextMenu { get; set; }

        [Parameter]
        public EventCallback<object> OnColumnIsSizingChanged { get; set; }

        [Parameter]
        public EventCallback<ColumnResizedArgs<TItem>> OnColumnResized { get; set; }        

        [Parameter]
        public EventCallback<bool> OnToggleCollapsedAll { get; set; }

        [Parameter]
        public SelectAllVisibility SelectAllVisibility { get; set; } = SelectAllVisibility.Visible;

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public string TooltipHostClassName { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;


        private bool showCheckbox;
        private bool isCheckboxHidden;
        private bool isCheckboxAlwaysVisible;
        private int frozenColumnCountFromStart;
        private int frozenColumnCountFromEnd;

        private string id;
        private object dragDropHelper;
        private (int SourceIndex, int TargetIndex) onDropIndexInfo;
        private int currentDropHintIndex;
        private int draggedColumnIndex = -1;

        private bool isResizingColumn;

        //state
        //private bool isAllSelected;
        private bool isAllCollapsed;
        private bool isSizing;
        private int resizeColumnIndex;
        private double resizeColumnMinWidth;
        private double resizeColumnOriginX;



        protected override Task OnInitializedAsync()
        {
            id = "G" + Guid.NewGuid().ToString();
            onDropIndexInfo = (-1, -1);
            currentDropHintIndex = -1;

            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            showCheckbox = SelectAllVisibility != SelectAllVisibility.None;
            isCheckboxHidden = SelectAllVisibility == SelectAllVisibility.Hidden;
            isCheckboxAlwaysVisible = CheckboxVisibility == CheckboxVisibility.Always;

            isResizingColumn = isSizing;

            if (ColumnReorderProps!= null && ColumnReorderProps.ToString() == "something")
            {
                frozenColumnCountFromStart = 1234;
            }
            else
            {
                frozenColumnCountFromStart = 0;
            }
            if (ColumnReorderProps != null && ColumnReorderProps.ToString() == "something")
            {
                frozenColumnCountFromEnd = 1234;
            }
            else
            {
                frozenColumnCountFromEnd = 0;
            }

            return base.OnParametersSetAsync();
        }


        private void OnSelectAllClicked(MouseEventArgs mouseEventArgs)
        {
            if (!isCheckboxHidden)
            {
                OnAllSelected.InvokeAsync(null);
            }
        }

        private void OnToggleCollapseAll(MouseEventArgs mouseEventArgs)
        {
            
        }

        private void OnSizerMouseDown(MouseEventArgs args, int colIndex)
        {
            isSizing = true;
            resizeColumnIndex = colIndex - (showCheckbox ? 2 : 1);
            resizeColumnOriginX = args.ClientX;
            resizeColumnMinWidth = Columns.ElementAt(resizeColumnIndex).CalculatedWidth;
            
        }

        private void OnSizerMouseMove(MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.ClientX != resizeColumnOriginX)
            {
                //OnColumnIsSizingChanged.InvokeAsync();                
            }
            if (OnColumnResized.HasDelegate)
            {
                var movement = mouseEventArgs.ClientX - resizeColumnOriginX;
                //skipping RTL check

                OnColumnResized.InvokeAsync(new ColumnResizedArgs<TItem>(Columns.ElementAt(resizeColumnIndex), resizeColumnIndex, resizeColumnMinWidth + movement));

            }
            
        }
        private void OnSizerMouseUp(MouseEventArgs mouseEventArgs)
        {
            isSizing = false;
        }

        private void UpdateDragInfo(int itemIndex)
        {

        }


        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var headerRules = new List<IRule>();

            // ROOT           
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};"+
                          $"display:flex;" +  // inline-block is seeing all the razor whitespace artifacts and adding extra spaces... switched to flex.  
                          $"background:{theme.SemanticColors.BodyBackground};" +
                          $"position:relative;" +
                          $"min-width:100%;" +
                          $"vertical-align:top;" +
                          $"height:42px;" +
                          $"line-height:42px;" +
                          $"white-space:nowrap;" +
                          $"box-sizing:content-box;" +
                          $"padding-bottom:1px;" +
                          $"padding-top:16px;" +
                          $"border-bottom:1px solid {theme.SemanticColors.BodyDivider};" +
                          $"cursor:default;" +
                          $"user-select:none;"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader:hover .ms-DetailsHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-tooltipHost .ms-DetailsHeader-checkTootip" },
                Properties = new CssString()
                {
                    Css = $"display:block;"
                }
            });

            //Check
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-check" },
                Properties = new CssString()
                {
                    Css = $"height:42px;"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-DetailsHeader-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });

            //CellWrapperPadded
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellWrapperPadded" },
                Properties = new CssString()
                {
                    Css = $"padding-right:{BFUDetailsRow<TItem>.CellExtraRightPadding + BFUDetailsRow<TItem>.CellRightPadding}px;"
                }
            });

            //CellIsCheck
            var cellIsCheckCellStyles = GetCellStyles(".ms-DetailsHeader-cellIsCheck", theme);
            foreach (var rule in cellIsCheckCellStyles)
                headerRules.Add(rule);
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "div.ms-DetailsHeader-cellIsCheck" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                          $"padding:0px;" +
                          $"margin:0;" +
                          $"display:inline-flex;" +
                          $"align-items:center;" +
                          $"border:none;" 
                }
            });
            
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader.is-allSelected .ms-DetailsHeader-cellIsCheck" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" 
                }
            });

            //CallIsGroupExpander
            var cellIsGroupExpanderCellStyles = GetCellStyles(".ms-DetailsHeader-cellIsGroupExpander", theme);
            foreach (var rule in cellIsGroupExpanderCellStyles)
                headerRules.Add(rule);
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellIsGroupExpander" },
                Properties = new CssString()
                {
                    Css = $"display:inline-flex;" +
                          $"align-items:center;" +
                          $"justify-content:center;" +
                          $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"padding:0;" +
                          $"border:none;" +
                          $"width:36px;" +
                          $"color:{theme.Palette.NeutralSecondary};"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellIsGroupExpander:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellIsGroupExpander:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};"
                }
            });

            //cellIsActionable SKIPPED BECAUSE this is a property of DetailsColumn which is a child...
            //cellIsEmpty also skipped

            //CellSizer
            var focusClearStyles = FocusStyle.FocusClear(".ms-DetailsHeader-cellSizer");
            foreach (var rule in focusClearStyles)
                headerRules.Add(rule);
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellSizer" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;" +
                          $"position:relative;" +
                          $"cursor:ew-resize;" +
                          $"bottom:0;" +
                          $"top:0;" +
                          $"overflow:hidden;" +
                          $"height:inherit;" +
                          $"background:transparent;" +
                          $"z-index:1;" +
                          $"width:16px;"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellSizer:after" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                          $"position:absolute;" +
                          $"bottom:0;" +
                          $"top:0;" +
                          $"width:1px;" +
                          $"background:{theme.Palette.NeutralTertiaryAlt};" +
                          $"opacity:0;" +
                          $"left:50%;"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellSizer:focus:after,.ms-DetailsHeader-cellSizer:hover:after" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" +
                          $"transition:opacity 0.3s linear;"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellSizer.is-resizing:focus:after" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" +
                          $"transition:opacity 0.3s linear;" +
                          $"box-shadow: 0 0 5px 0 rgba(0, 0, 0, 0.4);"
                }
            });

            //CellSizerStart
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellSizerStart" },
                Properties = new CssString()
                {
                    Css = $"margin:0 -8px;" 
                }
            });

            //CellSizerEnd
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-cellSizerEnd" },
                Properties = new CssString()
                {
                    Css = $"margin:0;" +
                          $"margin-left:-16px"
                }
            });

            //CollapseButton
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-collapseButton" },
                Properties = new CssString()
                {
                    Css = $"transform-origin:50% 50%;" +
                          $"transition:transform .1s linear;"+
                          $"transform:rotate(90deg);"
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-collapseButton.is-collapsed" },
                Properties = new CssString()
                {
                    Css = $"transform:rotate(0deg);"
                }
            });

            //SizingOverlay
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-sizingOverlay.is-sizing" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                          $"left:0;"+
                          $"top:0;" +
                          $"right:0;" +
                          $"bottom:0;" +
                          $"cursor:ew-resize;" +
                          $"background:rgba(255,255,255,0);" 
                }
            });
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsHeader-sizingOverlay.is-sizing{" +
                          $"background:transparent;" +
                          $"-ms-high-contrast-adjust:none;"+
                          "}"
                }
            });

            //accessibleLabel  
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-accessibleLabel" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                         $"width:1px;" +
                         $"height:1px;" +
                         $"margin:-1px;" +
                         $"padding:0;" +
                         $"border:0;" +
                         $"overflow:hidden;"
                }
            });

            //dropHintCircle  //doesn't seem to be used
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-dropHintCircle" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;" +
                         $"visibility:hidden;" +
                         $"position:absolute;" +
                         $"bottom:0;" +
                         $"height:9px;" +
                         $"width:9px;" +
                         $"border-radius:50%;" +
                         $"margin-left:-5px;" +
                         $"top:34px;" +
                         $"overflow:visible;" +
                         $"z-index:10;" +
                         $"border:1px solid {theme.Palette.ThemePrimary};" +
                         $"background:{theme.Palette.White};"
                }
            });

            //dropHintCaret 
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-dropHintCaret" },
                Properties = new CssString()
                {
                    Css = $"display:none;" +
                         $"position:absolute;" +
                         $"top:-28px;" +
                         $"left:-6.5px;" +
                         $"font-size:{theme.FontStyle.FontSize.Medium};" +
                         $"color:{theme.Palette.ThemePrimary};" +
                         $"overflow:visible;" +
                         $"z-index:10;"
                }
            });

            //dropHintLine 
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-dropHintLine" },
                Properties = new CssString()
                {
                    Css = $"display:none;" +
                         $"position:absolute;" +
                         $"bottom:0px;" +
                         $"top:0px;" +
                         $"overflow:visible;" +
                         $"height:42px;" +
                         $"width:1px;" +
                         $"background:{theme.Palette.ThemePrimary};" +
                         $"z-index:10;"
                }
            });

            //dropHint
            headerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsHeader-dropHint" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;" +
                         $"position:absolute;" 
                }
            });
            //NOT DONE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Skipped 

            return headerRules;
        }

        public static ICollection<Rule> GetCellStyles(string rootSelectorName, ITheme theme)
        {
            var cellStyles = new HashSet<Rule>();

            var props = new FocusStyleProps(theme);
            var focusStyleRules = FocusStyle.GetFocusStyle(props, $"{rootSelectorName}");

            cellStyles.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{rootSelectorName}" },
                Properties = new CssString()
                {
                    Css = focusStyleRules.MergeRules +
                            $"color:{theme.SemanticTextColors.BodyText};" +
                            $"display:inline-block;" +
                            $"position:relative;" +
                            $"box-sizing:border-box;" +
                            $"padding: 0 {BFUDetailsRow<TItem>.CellRightPadding}px 0 {BFUDetailsRow<TItem>.CellLeftPadding}px;" +
                            $"line-height:inherit;"+
                            $"margin:0;"+
                            $"height:42px;"+
                            $"vertical-align:top;"+
                            $"white-space:nowrap;"+
                            $"text-overflow:ellipsis;"+
                            $"text-align:left;"
                }
            });
            foreach (var rule in focusStyleRules.AddRules)
                cellStyles.Add(rule);

           
            return cellStyles;
        }
    }
}
