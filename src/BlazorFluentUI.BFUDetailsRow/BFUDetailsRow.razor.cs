using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUDetailsRow<TItem> : BFUComponentBase, IDisposable, IHasPreloadableGlobalStyle
    {
        [CascadingParameter]
        public BFUSelectionZone<TItem> SelectionZone { get; set; }

        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public bool AnySelected { get; set; }

        [Parameter]
        public IEnumerable<BFUDetailsRowColumn<TItem>> Columns { get; set; }

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

        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public double RowWidth { get; set; } = 0;

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;

        private bool canSelect;
        private bool showCheckbox;
        private object columnMeasureInfo = null;
        private bool isSelected;
        private Rule _localCheckCoverRule;

        private ICollection<IRule> DetailsRowLocalRules { get; set; } = new List<IRule>();

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            return base.OnInitializedAsync();
        }

        private void CreateLocalCss()
        {
            _localCheckCoverRule = new Rule();
            _localCheckCoverRule.Selector = new ClassSelector() { SelectorName = "ms-DetailsRow-checkCover" };
            _localCheckCoverRule.Properties = new CssString() { Css = $"position:absolute;top:-1px;left:0;bottom:0;right:0;display:{(AnySelected ? "block" : "none")};" };
            DetailsRowLocalRules.Add(_localCheckCoverRule);
        }

        protected override Task OnParametersSetAsync()
        {
            showCheckbox = SelectionMode != SelectionMode.None && CheckboxVisibility != CheckboxVisibility.Hidden;

            canSelect = SelectionMode != SelectionMode.None;

            if (SelectionZone != null)
            {
                SelectionZone.SelectedItemsObservable.Subscribe(x =>
                {
                    if (x.Contains(this.Item))
                    {
                        if (isSelected != true)
                        {
                            isSelected = true;
                            StateHasChanged();
                        }
                    }
                    else
                    {
                        if (isSelected != false)
                        {
                            isSelected = false;
                            StateHasChanged();
                        }
                    }
                });
            }

            return base.OnParametersSetAsync();
        }

        private async Task OnClick(MouseEventArgs args)
        {
            SelectionZone.ClearSelection();
            SelectionZone.HandleToggle(Item);
            await SelectionZone.OnItemInvoked.InvokeAsync(Item);
        }

        public static int RowVerticalPadding = 11;
        public static int CompactRowVerticalPadding = 6;
        public static int RowHeight = 42;
        public static int CompactRowHeight = 32;
        public static int CellLeftPadding = 12;
        public static int CellRightPadding = 8;
        public static int CellExtraRightPadding = 24;

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var defaultHeaderText = theme.Palette.NeutralPrimary;
            var defaultMetaText = theme.Palette.NeutralSecondary;
            var defaultBackground = theme.Palette.White;

            // Default Hover
            var defaultHoverHeaderText = theme.Palette.NeutralDark;
            var defaultHoverMetaText = theme.Palette.NeutralPrimary;
            var defaultHoverBackground = theme.Palette.NeutralLighter;

            // Selected
            var selectedHeaderText = theme.Palette.NeutralDark;
            var selectedMetaText = theme.Palette.NeutralPrimary;
            var selectedBackground = theme.Palette.NeutralLight;

            // Selected Hover
            var selectedHoverHeaderText = theme.Palette.NeutralDark;
            var selectedHoverMetaText = theme.Palette.NeutralPrimary;
            var selectedHoverBackground = theme.Palette.NeutralQuaternaryAlt;

            // Focus
            var focusHeaderText = theme.Palette.NeutralDark;
            var focusMetaText = theme.Palette.NeutralPrimary;
            var focusBackground = theme.Palette.NeutralLight;
            var focusHoverBackground = theme.Palette.NeutralQuaternaryAlt;

            var detailsRowRules = new HashSet<IRule>();

            // Root
            var rootFocusStyleProps = new FocusStyleProps(theme);
            rootFocusStyleProps.BorderColor = theme.SemanticColors.FocusBorder;
            rootFocusStyleProps.OutlineColor = theme.Palette.White;
            var rootMergeStyleResults = FocusStyle.GetFocusStyle(rootFocusStyleProps, ".ms-DetailsRow");
            foreach (var rule in rootMergeStyleResults.AddRules)
                detailsRowRules.Add(rule);

            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};"+
                          rootMergeStyleResults.MergeRules +
                          $"border-bottom:1px solid {theme.Palette.NeutralLighter};" +
                          $"background:{theme.Palette.White};" +
                          $"color:{theme.Palette.NeutralSecondary};" +
                          $"display:inline-flex;" +
                          $"min-width:100%;" +
                          $"width:100%;" +  // added to make DetailsRow fit width of window
                          $"min-height:{RowHeight}px;" +
                          $"white-space:nowrap;" +
                          $"padding:0px;" +
                          $"box-sizing:border-box;" +
                          $"vertical-align:top;" +
                          $"text-align:left;"
                }
            });

            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-List-cell:first-child .ms-DetailsRow:before" },
                Properties = new CssString()
                {
                    Css= $"display:none;"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.Palette.NeutralLighter};" +
                          $"color:{theme.Palette.NeutralPrimary}"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow:hover .is-row-header" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralDark}"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow:hover .ms-DetailsRow-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-DetailsRow:focus .ms-DetailsRow-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });

            var rootSelectedFocusStyleProps = new FocusStyleProps(theme);
            rootSelectedFocusStyleProps.BorderColor = theme.SemanticColors.FocusBorder;
            rootSelectedFocusStyleProps.OutlineColor = theme.Palette.White;
            rootSelectedFocusStyleProps.Inset = -1;

            var rootSelectedMergeStyleResults = FocusStyle.GetFocusStyle(rootSelectedFocusStyleProps, ".ms-DetailsRow.is-selected");
            foreach (var rule in rootSelectedMergeStyleResults.AddRules)
                detailsRowRules.Add(rule);

            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected" },
                Properties = new CssString()
                {
                    Css = rootSelectedMergeStyleResults.MergeRules +
                          $"color:{selectedMetaText};" +
                          $"background:{selectedBackground};" +
                          $"border-bottom:1px solid {theme.Palette.White};"
                }
            });
            detailsRowRules.Add(new Rule()
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
                          $"border-top:1px solid {theme.Palette.White}"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.Palette.NeutralQuaternaryAlt};" +
                          $"color:{theme.Palette.NeutralPrimary};"
                }
            });
            detailsRowRules.Add(new Rule()
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
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-selected.is-row-header:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralDark};"
                }
            });
            detailsRowRules.Add(new Rule()
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

            // focus for is-selected
            detailsRowRules.AddCssStringSelector(".ms-DetailsRow.is-selected:focus").AppendCssStyles(
                $"background:{focusBackground}"
            );
            detailsRowRules.AddCssStringSelector(".ms-DetailsRow.is-selected:focus .ms-DetailsRow-cell").AppendCssStyles(
                $"color:{focusMetaText}"
            );
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-selected:focus .ms-DetailsRow-cell {" +
                           $"color:HighlightText;" +
                           "}" +
                           ".ms-DetailsRow.is-selected:focus .ms-DetailsRow-cell > a {" +
                           $"background:Highlight;" +
                           "}"
                }
            });

            detailsRowRules.AddCssStringSelector(".ms-DetailsRow.is-selected.is-row-header:focus ").AppendCssStyles(
                $"color:{focusHeaderText}"
            );
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-selected.is-row-header:focus {" +
                           $"color:HighlightText;" +
                           "}"
                }
            });

            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-selected:focus {" +
                           $"background:HighlightText;" +
                           "}"
                }
            });

            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-DetailsRow.is-selected{" +
                          "background:Highlight;" +
                          "color:HighlightText;" +
                          "-ms-high-contrast-adjust:none;" +
                          "}" +
                          ".ms-DetailsRow.is-selected a {"+
                          "color:HighlightText;"+
                          "}"
                }
            });

            //focus and hover state
            detailsRowRules.AddCssStringSelector(".ms-DetailsRow.is-selected.is-row-header:focus ").AppendCssStyles(
                $"background:{focusHoverBackground}"
            );

            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-compact" },
                Properties = new CssString()
                {
                    Css = $"min-height:{CompactRowHeight}px;" +
                                      "border:0"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-compact.is-selected:before" },
                Properties = new CssString()
                {
                    Css = $"display:none;"
                }
            });

            // CellUnpadded
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellUnpadded" },
                Properties = new CssString()
                {
                    Css = $"padding-right:{CellRightPadding}px;"
                }
            });
            // CellPadded
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellPadded" },
                Properties = new CssString()
                {
                    Css = $"padding-right:{CellExtraRightPadding + CellRightPadding}px;"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellPadded.ms-DetailsRow-cellCheck" },
                Properties = new CssString()
                {
                    Css = $"padding-right:0px;"
                }
            });

            //Cell
            var rules = GetDefaultCellStyles(".ms-DetailsRow-cell", theme);
            foreach (var rule in rules)
                detailsRowRules.Add(rule);

            // CellMeasurer
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-cellMeasurer" },
                Properties = new CssString()
                {
                    Css = $"overflow:visible;" +
                          $"white-space:nowrap;"
                }
            });

            // CheckCell
            detailsRowRules.Add(new Rule()
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
            detailsRowRules.Add(new Rule()
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

            //Fields
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow-fields" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                         $"align-items:stretch;"+
                         $"overflow-x:hidden;"
                }
            });

            //IsRowHeader
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-row-header" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};" +
                          $"font-size:{theme.FontStyle.FontSize.Medium};"
                }
            });
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-row-header.is-selected" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralDark};" +
                          $"font-weight:{theme.FontStyle.FontWeight.SemiBold};"
                }
            });
            detailsRowRules.Add(new Rule()
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
            detailsRowRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DetailsRow.is-multiline" },
                Properties = new CssString()
                {
                    Css = $"white-space:normal;" +
                          $"word-break:break-word;"+
                          $"text-overflow:clip;"
                }
            });

            var selectedfocusStyleProps = new FocusStyleProps(theme);
            selectedfocusStyleProps.Inset = -1;
            selectedfocusStyleProps.BorderColor = theme.SemanticColors.FocusBorder;
            selectedfocusStyleProps.OutlineColor = theme.Palette.White;
            var selectedMergeStyleResults = FocusStyle.GetFocusStyle(selectedfocusStyleProps, ".ms-List-cell.is-selected .ms-DetailsRow");

            return detailsRowRules;

        }

        private List<IRule> GetDefaultCellStyles(string selector, ITheme theme)
        {
            var rules = new List<IRule>();

            // Cell
            var cellfocusStyleProps = new FocusStyleProps(theme);
            cellfocusStyleProps.Inset = -1;
            var cellMergeStyleResults = FocusStyle.GetFocusStyle(cellfocusStyleProps, $".ms-DetailsRow {selector}");
            foreach (var result in cellMergeStyleResults.AddRules)
                rules.Add(result);

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
                          $"vertical-align:top;" +
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

            //not sure if this selector is working...
            var focusableFocusStylesProps = new FocusStyleProps(theme) { Inset = -1, BorderColor = theme.Palette.NeutralSecondary, OutlineColor = theme.Palette.White };
            var focusableFocusStyles = FocusStyle.GetFocusStyle(focusableFocusStylesProps, $".ms-DetailsRow {selector}[data-is-focusable='true']");
            foreach (var rule in focusableFocusStyles.AddRules)
                rules.Add(rule);
            rules.AddCssStringSelector($".ms-DetailsRow {selector}[data-is-focusable='true']").AppendCssStyles(
                focusableFocusStyles.MergeRulesList.ToArray()
                );


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DetailsRow.is-compact {selector}" },
                Properties = new CssString()
                {
                    Css = cellMergeStyleResults.MergeRules +
                          focusableFocusStyles.MergeRules +
                         $"min-height:{CompactRowHeight}px;" +
                         $"padding-top:{CompactRowVerticalPadding}px;" +
                         $"padding-bottom:{CompactRowVerticalPadding}px;" +
                         $"padding-left:{CellLeftPadding}px;"
                }
            });


            return rules;
        }

        public void Dispose()
        {
            //Debug.WriteLine("DetailsRow disposed");
        }
    }
}
