using BlazorFabric.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class DetailsRow<TItem> : FabricComponentBase
    {
        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public bool AnySelected { get; set; }

        //[Parameter]
        //public bool CanSelect { get; set; }

        [Parameter]
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool EnableUpdateAnimations { get; set; }

        [Parameter]
        public int GroupNestingDepth { get; set; }

        [Parameter]
        public double IndentWidth { get; set; } = 36;

        [Parameter]
        public bool IsCheckVisible { get; set; }

        [Parameter]
        public bool IsRowHeader { get; set; }

        //[Parameter]
        //public bool IsSelected { get; set; }

        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public int ItemIndex { get; set; }

        [Parameter]
        public double RowWidth { get; set; } = 0;

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }
        
        [Parameter]
        public bool UseFastIcons { get; set; } = true;

        private bool canSelect;
        private bool showCheckbox;
        private object columnMeasureInfo = null;
        private bool isSelected;
        private Rule _localCheckCoverRule;

        private ICollection<Rule> DetailsRowGlobalRules { get; set; } = new List<Rule>();
        private ICollection<Rule> DetailsRowLocalRules { get; set; } = new List<Rule>();

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            return base.OnInitializedAsync();
        }

        private void CreateLocalCss()
        {
            _localCheckCoverRule = new Rule();
            _localCheckCoverRule.Selector = new ClassSelector() { SelectorName = "ms-DetailsRow-checkCover" };            
            DetailsRowLocalRules.Add(_localCheckCoverRule);
        }
        protected override Task OnParametersSetAsync()
        {
            showCheckbox = SelectionMode != SelectionMode.None && CheckboxVisibility != CheckboxVisibility.Hidden;

            canSelect = Selection != null;

            if (Selection != null)
            {
                isSelected = Selection.SelectedItems.Contains(this.Item);
            }
            //CreateCss();
            return base.OnParametersSetAsync();
        }

        const int RowVerticalPadding = 11;
        const int CompactRowVerticalPadding = 6;
        const int RowHeight = 42;
        const int CompactRowHeight = 32;
        const int CellLeftPadding = 12;
        const int CellRightPadding = 8;
        const int CellExtraRightPadding = 24;

        protected void CreateCss()
        {
            
            DetailsRowLocalRules = new List<Rule>();
            DetailsRowGlobalRules = new List<Rule>();

            // Root
            var rootFocusStyleProps = new FocusStyleProps(this.Theme);
            rootFocusStyleProps.BorderColor = Theme.SemanticColors.FocusBorder;
            rootFocusStyleProps.OutlineColor = Theme.Palette.White;
            var rootMergeStyleResults = FocusStyle.GetFocusStyle(rootFocusStyleProps, ".ms-DetailsRow");
            foreach (var rule in rootMergeStyleResults.AddRules)
                DetailsRowGlobalRules.Add(rule);

            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow" },
                Properties = new CssString()
                {
                    Css = rootMergeStyleResults.MergeRules +
                          $"border-bottom:1px solid {Theme.Palette.NeutralLighter};" +
                          $"background:{Theme.Palette.White};" +
                          $"color:{Theme.Palette.NeutralSecondary};" +
                          $"display:inline-flex;" +
                          $"min-width:100%;" +
                          $"min-height:{RowHeight}px;" +
                          $"white-space:nowrap;" +
                          $"padding:0px;" +
                          $"box-sizing:border-box;" +
                          $"vertical-align:top;" +
                          $"text-align:left;"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-compact" },
                Properties = new CssString()
                {
                    Css = $"min-height:{CompactRowHeight}px;" + 
                          "border:0px"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.Palette.NeutralLighter};" +
                          $"color:{Theme.Palette.NeutralPrimary}"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow:hover .is-row-header" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralDark}"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow:hover .ms-DetailsRow-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-DetailsRow:focus .ms-DetailsRow-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });

            var rootSelectedFocusStyleProps = new FocusStyleProps(this.Theme);
            rootSelectedFocusStyleProps.BorderColor = Theme.SemanticColors.FocusBorder;
            rootSelectedFocusStyleProps.OutlineColor = Theme.Palette.White;

            var rootSelectedMergeStyleResults = FocusStyle.GetFocusStyle(rootSelectedFocusStyleProps, ".ms-DetailsRow.is-selected");
            foreach (var rule in rootSelectedMergeStyleResults.AddRules)
                DetailsRowGlobalRules.Add(rule);

            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected" },
                Properties = new CssString()
                {
                    Css = rootSelectedMergeStyleResults.MergeRules + 
                          $"color:{Theme.Palette.NeutralDark};" +
                          $"background:{Theme.Palette.NeutralLight};" +
                          $"border-bottom:1px solid {Theme.Palette.White};"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected:before" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                          $"display:block;" +
                          $"top:-1px;" +
                          $"height:1px;" +
                          $"bottom:0px;" +
                          $"left:0px;" +
                          $"right:0px;" +
                          $"content:'';" +
                          $"border-top:1px solid {Theme.Palette.White}"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.Palette.NeutralQuaternaryAlt};" +
                          $"color:{Theme.Palette.NeutralPrimary};"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-selected:hover .ms-DetailsRow-cell {" +
                          $"color:HighlightText;" +
                          "}" +
                          ".ms-DetailsRow.is-selected:hover .ms-DetailsRow-cell > a {" +
                          $"color:HighlightText;" +
                          "}" 
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected.is-row-header:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralDark};"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-selected.is-row-header:hover {" +
                          $"color:HighlightText;" +
                          "}" +
                          ".ms-DetailsRow.is-selected:hover {" +
                          $"background:Highlight;" +
                          "}"
                }
            });

            // #####################################################################################
            // selected NOT FINISHED!
            // #####################################################################################

            // CellUnpadded
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellUnpadded" },
                Properties = new CssString()
                {
                    Css = $"padding-right:{CellRightPadding}px;"
                }
            });
            // CellPadded
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellPadded" },
                Properties = new CssString()
                {
                    Css = $"padding-right:{CellExtraRightPadding + CellRightPadding}px;"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellPadded.ms-DetailsRow-cellCheck" },
                Properties = new CssString()
                {
                    Css = $"padding-right:0px;"
                }
            });

            //Cell
            var rules = GetDefaultCellStyles(".ms-DetailsRow-cell");
            foreach (var rule in rules)
                DetailsRowGlobalRules.Add(rule);

            // CellMeasurer
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellMeasurer" },
                Properties = new CssString()
                {
                    Css = $"overflow:visible;" +
                          $"white-space:nowrap;" 
                }
            });

            // CheckCell
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-checkCell" },
                Properties = new CssString()
                {
                    Css = $"padding:0px;" +
                          $"padding-top:1px;" +
                          $"margin-top:-1px;" +
                          $"flex-shrink:0;"
                }
            });


            // CheckCover
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-checkCover" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                          $"top:-1px;" +
                          $"left:0px;" +
                          $"bottom:0px;"+
                          $"right:0px;"
                }
            });
            _localCheckCoverRule.Properties = new CssString()
            {
                Css = $"display:{(AnySelected ? "block" : "none")}"
            };

            //Fields
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-fields" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                         $"align-items:stretch;"
                }
            });

            //IsRowHeader
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-row-header" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};" +
                          $"font-size:{Theme.FontStyle.FontSize.Medium};"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-row-header.is-selected" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralDark};" +
                          $"font-weight:{Theme.FontStyle.FontWeight.SemiBold};"
                }
            });
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-row-header.is-selected {" +
                          $"color:HighlightText;" +
                          "}"
                }
            });

            // IsMultiline
            DetailsRowGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-multiline" },
                Properties = new CssString()
                {
                    Css = $"white-space:normal;" +
                          $"word-break:break-word;"+
                          $"text-overflow:clip;"
                }
            });

            
           
            //DetailsRowGlobalRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cell[data-is-focusable='true']" },
            //    Properties = new CssString()
            //    {
            //        Css = "max-width:100%;"
            //    }
            //});

            var selectedfocusStyleProps = new FocusStyleProps(this.Theme);
            selectedfocusStyleProps.Inset = -1;
            selectedfocusStyleProps.BorderColor = Theme.SemanticColors.FocusBorder;
            selectedfocusStyleProps.OutlineColor = Theme.Palette.White;
            var selectedMergeStyleResults = FocusStyle.GetFocusStyle(selectedfocusStyleProps, ".ms-List-cell.is-selected .ms-DetailsRow");



        }

        private List<Rule> GetDefaultCellStyles(string selector)
        {
            var rules = new List<Rule>();

            // Cell
            var cellfocusStyleProps = new FocusStyleProps(this.Theme);
            cellfocusStyleProps.Inset = -1;
            cellfocusStyleProps.BorderColor = Theme.Palette.NeutralSecondary;
            cellfocusStyleProps.OutlineColor = Theme.Palette.White;
            var cellMergeStyleResults = FocusStyle.GetFocusStyle(cellfocusStyleProps, $".ms-DetailsRow {selector}");

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DetailsRow {selector}" },
                Properties = new CssString()
                {
                    Css = cellMergeStyleResults.MergeRules +
                          $"display:inline-block;" +
                          $"position:relative;" +
                          $"box-sizing:border-box;" +
                          $"min-height:{RowHeight}px;" +
                          $"vertial-align:top;" +
                          $"white-space:nowrap;" +
                          $"overflow:hidden;" +
                          $"text-overflow:ellipsis;" +
                          $"padding-top:{RowVerticalPadding}px;" +
                          $"padding-bottom:{RowVerticalPadding}px;" +
                          $"padding-left:{CellLeftPadding}px;"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DetailsRow {selector} > button" },
                Properties = new CssString()
                {
                    Css = "max-width:100%;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = $".ms-DetailsRow.is-selected {selector} {{" +
                          $"background:Highlight;" +
                          $"color:HighlightText;" +
                          $"-ms-high-contrast-adjust:none;" +
                         "}" +
                         $".ms-DetailsRow.is-selected {selector} > a {{" +
                          $"color:HighlightText;" +
                         "}"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DetailsRow.is-compact {selector}" },
                Properties = new CssString()
                {
                    Css = cellMergeStyleResults.MergeRules +
                          $"min-height:{CompactRowHeight}px;" +
                          $"padding-top:{CompactRowVerticalPadding}px;" +
                          $"padding-bottom:{CompactRowVerticalPadding}px;" +
                          $"padding-left:{CellLeftPadding}px;"
                }
            });

            return rules;
        }
       

        
    }
}
