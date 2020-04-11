using Microsoft.AspNetCore.Components;
using System;

namespace BlazorFluentUI
{
    public class BFUDetailsRowColumn<TItem>
    {
        public BFUDetailsRowColumn()
        { }
        public BFUDetailsRowColumn(string fieldName, Func<TItem, IComparable> fieldSelector)
        {
            Name = fieldName;
            Key = fieldName;
            AriaLabel = fieldName;
            FieldSelector = fieldSelector;
        }

        public string AriaLabel { get; set; }
        public double CalculatedWidth { get; set; } = double.NaN;
        public ColumnActionsMode ColumnActionsMode { get; set; } = ColumnActionsMode.Clickable;
        public RenderFragment<IComparable> ColumnItemTemplate { get; set; }
        public Func<TItem, IComparable> FieldSelector { get; set; }
        public string FilterAriaLabel { get; set; }
        public string GroupAriaLabel { get; set; }
        public string IconClassName { get; set; }
        public string IconName { get; set; }
        /// <summary>
        /// Forces columns to be in a particular order.  Useful for libraries (like DynamicData) that don't maintain order of collections internally.
        /// </summary>
        public int Index { get; set; } 
        public bool IsCollapsible { get; set; }
        public bool IsFiltered { get; set; }
        public bool IsGrouped { get; set; }
        public bool IsIconOnly { get; set; }
        public bool IsMenuOpen { get; set; }
        public bool IsMultiline { get; set; }
        public bool IsPadded { get; set; }
        public bool IsResizable { get; set; }
        public bool IsRowHeader { get; set; }  // only one can be set, it's for the "role" (and a style is set, too)
        public bool IsSorted { get; set; }
        public bool IsSortedDescending { get; set; }
        public string Key { get; set; }
        public double MaxWidth { get; set; } = 300;
        public double MinWidth { get; set; } = 100;
        public string Name { get; set; }
        public Action<BFUDetailsRowColumn<TItem>> OnColumnClick { get; set; }
        public Action<BFUDetailsRowColumn<TItem>> OnColumnContextMenu { get; set; }
        public string SortedAscendingAriaLabel { get; set; }
        public string SortedDescendingAriaLabel { get; set; }
        public Type Type { get; set; }        
       
    }
    
}
