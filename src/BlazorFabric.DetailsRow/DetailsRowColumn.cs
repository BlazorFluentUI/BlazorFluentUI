using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace BlazorFabric
{
    public class DetailsRowColumn<TItem>
    {
        public bool IsFiltered { get; set; }
        public bool IsGrouped { get; set; }
        public bool IsMultiline { get; set; }
        public bool IsPadded { get; set; }
        public bool IsResizable { get; set; }
        public bool IsRowHeader { get; set; }  // only one can be set, it's for the "role"
        public bool IsSorted { get; set; }
        public bool IsSortedDescending { get; set; }
        public double MinWidth { get; set; } = -1;
        public double MaxWidth { get; set; } = -1;
        public string Name { get; set; }
        public Type Type { get; set; }
        public Func<TItem, object> FieldSelector { get; set; }
        public RenderFragment<object> ColumnItemTemplate { get; set; }
    }
    
}
