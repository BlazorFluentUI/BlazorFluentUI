using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace BlazorFluentUI.Lists
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
    }
}
