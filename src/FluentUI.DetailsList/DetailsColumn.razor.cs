using FluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace FluentUI
{
    public partial class DetailsColumn<TItem> : FluentUIComponentBase
    {

        [Parameter]
        public DetailsRowColumn<TItem> Column { get; set; }

        [Parameter]
        public RenderFragment<object> ColumnHeaderTooltipTemplate { get; set; }

        [Parameter]
        public int ColumnIndex { get; set; }

        [Parameter]
        public object DragDropHelper { get; set; }

        [Parameter]
        public bool IsDraggable { get; set; }

        [Parameter]
        public bool IsDropped { get; set; }

        [Parameter]
        public EventCallback<DetailsRowColumn<TItem>> OnColumnClick { get; set; }

        [Parameter]
        public EventCallback<DetailsRowColumn<TItem>> OnColumnContextMenu { get; set; }

        [Parameter]
        public string ParentId { get; set; }

        [Parameter]
        public EventCallback<int> UpdateDragInfo { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;


        private bool HasAccessibleLabel()
        {
            return !string.IsNullOrEmpty(Column.AriaLabel)
                || !string.IsNullOrEmpty(Column.FilterAriaLabel)
                || !string.IsNullOrEmpty(Column.SortedAscendingAriaLabel)
                || !string.IsNullOrEmpty(Column.SortedDescendingAriaLabel)
                || !string.IsNullOrEmpty(Column.GroupAriaLabel);
        }

        private void HandleColumnClick(MouseEventArgs mouseEventArgs)
        {
            if (Column.ColumnActionsMode == ColumnActionsMode.Disabled)
                return;
            
            Column.OnColumnClick?.Invoke(Column);
            OnColumnClick.InvokeAsync(Column);
        }

        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
        //{
        //    var columnRules = new HashSet<IRule>();

        //    // ROOT
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".msDetailsColumn" },
        //        Properties = new CssString()
        //        {
        //            Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
        //                  $"font-weight:{theme.FontStyle.FontWeight.Regular};"
        //        }
        //    });

        //    var cellStyles = DetailsHeader<TItem>.GetCellStyles(".ms-DetailsColumn", theme);
        //    foreach (var rule in cellStyles)
        //        columnRules.Add(rule);

        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn.is-actionable:hover" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:var(--semanticTextColors.BodyText};" +
        //                    $"background:var(--semanticColors.ListHeaderBackgroundHovered};"
        //        }
        //    });
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn.is-actionable:active" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background:var(--semanticColors.ListHeaderBackgroundPressed};"
        //        }
        //    });
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn.is-empty" },
        //        Properties = new CssString()
        //        {
        //            Css = $"text-overflow:clip;" 
        //        }
        //    });
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn.is-padded" },
        //        Properties = new CssString()
        //        {
        //            Css = $"padding-right:{DetailsRow<TItem>.CellExtraRightPadding + DetailsRow<TItem>.CellRightPadding}px;"
        //        }
        //    });
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn.is-padded:hover i[data-icon-name='GripperBarVertical']" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:block;"
        //        }
        //    });

        //    //GripperBarVerticalStyle
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-gripperBar" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:none;"+
        //                  $"position:absolute;" +
        //                  $"text-align:left;" +
        //                  $"color:{theme.Palette.NeutralTertiary};" +
        //                  $"left:1px;" 
        //        }
        //    });

        //    //NearIcon
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-nearIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:var(--semanticTextColors.BodySubtext};" +
        //                  $"opacity:1;" +
        //                  $"padding-left:8px;" 
        //        }
        //    });

        //    //SortIcon
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-sortIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:var(--semanticTextColors.BodySubtext};" +
        //                  $"opacity:1;" +
        //                  $"padding-left:4px;"+
        //                  $"position:relative;"+
        //                  $"top:1px;"
        //        }
        //    });

        //    ////IconClassName
        //    //columnRules.Add(new Rule()
        //    //{
        //    //    Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-iconClassName" },
        //    //    Properties = new CssString()
        //    //    {
        //    //        Css = $"color:var(--semanticTextColors.BodySubtext};" +
        //    //              $"opacity:1;" +
        //    //              $"padding-left:8px;"
        //    //    }
        //    //});

        //    //FilterChevron
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-filterChevron" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.Palette.NeutralTertiary};" +
        //                  $"padding-left:6px;"+
        //                  $"vertical-align:middle;"+
        //                  $"font-size:{theme.FontStyle.FontSize.Small};"
        //        }
        //    });

        //    //CellTitle
        //    var cellTitleFocusStyles = FocusStyle.GetFocusStyle(new FocusStyleProps(theme), ".ms-DetailsColumn-cellTitle");
        //    foreach (var rule in cellTitleFocusStyles.AddRules)
        //        columnRules.Add(rule);
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-cellTitle" },
        //        Properties = new CssString()
        //        {
        //            Css = cellTitleFocusStyles.MergeRules + 
        //                  $"display:flex;" +
        //                  $"flex-direction:row;" +
        //                  $"justify-content:flex-start;" +
        //                  $"align-items:stretch;"+
        //                  $"box-sizing:border-box;"+
        //                  $"overflow:hidden;"+
        //                  $"padding:0 {DetailsRow<TItem>.CellRightPadding}px 0 {DetailsRow<TItem>.CellLeftPadding}px;"
        //        }
        //    });
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn.is-iconOnly .ms-DetailsColumn-cellTitle .ms-DetailsColumn-nearIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"padding-left:0;"
        //        }
        //    });

        //    //CellTitle
        //    columnRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-DetailsColumn-cellTooltip" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:block;" +
        //                  $"position:absolute;" +
        //                  $"top:0;"+
        //                  $"left:0;" +
        //                  $"bottom:0;" +
        //                  $"right:0;"
        //        }
        //    });

        //    return columnRules;
        //}
    }
}
