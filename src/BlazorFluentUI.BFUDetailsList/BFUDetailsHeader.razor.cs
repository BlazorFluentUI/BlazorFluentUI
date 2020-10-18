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
    public partial class BFUDetailsHeader<TItem> : BFUComponentBase
    {
        //[CascadingParameter]
        //private BFUSelectionZone<TItem> SelectionZone { get; set; }

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

            // TBD
            if (ColumnReorderProps!= null && ColumnReorderProps.ToString() == "something")
            {
                frozenColumnCountFromStart = 1234;
            }
            else
            {
                frozenColumnCountFromStart = 0;
            }
            //if (ColumnReorderProps != null && ColumnReorderProps.ToString() == "something")
            //{
            //    frozenColumnCountFromEnd = 1234;
            //}
            //else
            //{
            //    frozenColumnCountFromEnd = 0;
            //}

            return base.OnParametersSetAsync();
        }


        private void OnSelectAllClicked(MouseEventArgs mouseEventArgs)
        {
            if (!isCheckboxHidden)
            {
                Selection?.ToggleAllSelected();
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


       
    }
}
