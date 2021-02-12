using FluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DetailsHeader<TItem> : FluentUIComponentBase, IAsyncDisposable
    {
        //[CascadingParameter]
        //private SelectionZone<TItem> SelectionZone { get; set; }

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
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get; set; }
               
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
        public EventCallback<ItemContainer<DetailsRowColumn<TItem>>> OnColumnAutoResized { get; set; }

        [Parameter]
        public EventCallback<DetailsRowColumn<TItem>> OnColumnClick { get; set; }

        [Parameter]
        public EventCallback<DetailsRowColumn<TItem>> OnColumnContextMenu { get; set; }

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

        [Inject] private IJSRuntime? JSRuntime { get; set; }

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

        const double MIN_COLUMN_WIDTH = 100;

        //state
        //private bool isAllSelected;
        private bool isAllCollapsed;
        private bool isSizing;
        private int resizeColumnIndex;
        private double resizeColumnMinWidth;
        private double resizeColumnOriginX;

        private DotNetObjectReference<DetailsHeader<TItem>>? dotNetRef;
        private ElementReference cellSizer;

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

            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetRef = DotNetObjectReference.Create(this);
                await JSRuntime!.InvokeVoidAsync("FluentUIDetailsList.registerDetailsHeader", dotNetRef, RootElementReference);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public void OnSizerMouseDown(int columnIndex, double originX)
        {
            isSizing = true;
            resizeColumnIndex = columnIndex; //columnIndex - (showCheckbox ? 2 : 1);
            resizeColumnOriginX = originX;
            resizeColumnMinWidth = Columns.ElementAt(resizeColumnIndex).CalculatedWidth;
            InvokeAsync(StateHasChanged);
        }

        [JSInvokable]
        public void OnDoubleClick(int columnIndex)
        {
            //System.Diagnostics.Debug.WriteLine("DoubleClick happened.");
            OnColumnAutoResized.InvokeAsync(new ItemContainer<DetailsRowColumn<TItem>> { Item = Columns.ElementAt(columnIndex), Index = columnIndex });
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

        //private void OnSizerMouseDown(MouseEventArgs args, int colIndex)
        //{
        //    isSizing = true;
        //    resizeColumnIndex = colIndex - (showCheckbox ? 2 : 1);
        //    resizeColumnOriginX = args.ClientX;
        //    resizeColumnMinWidth = Columns.ElementAt(resizeColumnIndex).CalculatedWidth;
        //}

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
                var calculatedWidth = resizeColumnMinWidth + movement;
                var currentColumnMinWidth = Columns.ElementAt(resizeColumnIndex).MinWidth;
                var constrictedCalculatedWidth = Math.Max((currentColumnMinWidth < 0 || double.IsNaN(currentColumnMinWidth) ? MIN_COLUMN_WIDTH : currentColumnMinWidth), calculatedWidth);
                OnColumnResized.InvokeAsync(new ColumnResizedArgs<TItem>(Columns.ElementAt(resizeColumnIndex), resizeColumnIndex, constrictedCalculatedWidth));

            }
            
        }
        private void OnSizerMouseUp(MouseEventArgs mouseEventArgs)
        {
            isSizing = false;
        }

        private void UpdateDragInfo(int itemIndex)
        {

        }

        public async ValueTask DisposeAsync()
        {
            if (dotNetRef != null)
            {
                await JSRuntime!.InvokeVoidAsync("FluentUIDetailsList.unregisterDetailsHeader", dotNetRef);
                dotNetRef?.Dispose();
            }
        }
    }
}
