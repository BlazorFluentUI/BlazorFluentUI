using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUSelectionZone<TItem> : BFUComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool DisableAutoSelectOnInputElements { get; set; }

        [Parameter]
        public bool EnterModalOnTouch { get; set; }

        [Parameter]
        public bool IsSelectedOnFocus { get; set; } = true;

        [Parameter]
        public EventCallback<ItemContainer<TItem>> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<ItemContainer<TItem>> OnItemInvoked { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public EventCallback<Selection<TItem>> SelectionChanged { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }


        private List<TItem> selectedItems = new List<TItem>();

        protected override async Task OnParametersSetAsync()
        {
            if (Selection != null && Selection.SelectedItems != selectedItems)
            {
                selectedItems = new System.Collections.Generic.List<TItem>(Selection.SelectedItems);
                //StateHasChanged();
            }

            if (SelectionMode == SelectionMode.Single && selectedItems.Count() > 1)
            {
                selectedItems.Clear();
                await SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
            else if (SelectionMode == SelectionMode.None && selectedItems.Count() > 0)
            {
                selectedItems.Clear();
                await SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// For DetailsRow
        /// </summary>
        /// <param name="item"></param>
        /// <param name="asSingle">On click, force list to select one even if set to multiple</param>
        public void SelectItem(TItem item, bool asSingle=false)
        {
            bool hasChanged = false;
            if (SelectionMode == SelectionMode.Multiple && !asSingle)
            {
                hasChanged = true;
                if (selectedItems.Contains(item))
                    selectedItems.Remove(item);
                else
                    selectedItems.Add(item);
            }
            else if (SelectionMode == SelectionMode.Multiple && asSingle)
            {
                //same as single except we need to clear other items if they are selected, too
                hasChanged = true;
                selectedItems.Clear();
                selectedItems.Add(item);
            }
            else if (SelectionMode == SelectionMode.Single)
            {
                if (!selectedItems.Contains(item))
                {
                    hasChanged = true;
                    selectedItems.Clear();
                    selectedItems.Add(item);
                }
            }

            if (hasChanged)
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
        }

        public void AddItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                if (!selectedItems.Contains(item))
                    selectedItems.Add(item);
            }

            if (items != null && items.Count() > 0)
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
        }
                
        public void RemoveItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                selectedItems.Remove(item);
            }

            if (items != null && items.Count() > 0)
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
        }

        public void AddAndRemoveItems(IEnumerable<TItem> itemsToAdd, IEnumerable<TItem> itemsToRemove)
        {
            foreach (var item in itemsToAdd)
            {
                if (!selectedItems.Contains(item))
                    selectedItems.Add(item);
            }
            foreach (var item in itemsToRemove)
            {
                selectedItems.Remove(item);
            }

            if ((itemsToAdd != null && itemsToAdd.Count() > 0)||(itemsToRemove != null && itemsToRemove.Count() > 0))
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
        }

        

        public void ClearSelection()
        {
            if (selectedItems.Count>0)
            {
                selectedItems.Clear();
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }

        // For end-users to let SelectionMode handle what to do.
        public void HandleClick(TItem item, int index)
        {
            bool hasChanged = false;
            if (SelectionMode == SelectionMode.Multiple)
            {
                hasChanged = true;
                if (selectedItems.Contains(item))
                    selectedItems.Remove(item);
                else
                    selectedItems.Add(item);
            }
            else if (SelectionMode == SelectionMode.Single )
            {
                if (!selectedItems.Contains(item))
                {
                    hasChanged = true;
                    selectedItems.Clear();
                    selectedItems.Add(item);
                }
            }

            if (hasChanged)
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
        }

        // For end-users to let SelectionMode handle what to do.
        public void HandleToggle(TItem item, int index)
        {
            bool hasChanged = false;
            switch (SelectionMode)
            {
                case SelectionMode.Multiple:
                    hasChanged = true;
                    if (selectedItems.Contains(item))
                        selectedItems.Remove(item);
                    else
                        selectedItems.Add(item);
                    break;
                case SelectionMode.Single:
                    hasChanged = true;
                    if (selectedItems.Contains(item))
                        selectedItems.Remove(item);
                    else
                    {
                        selectedItems.Clear();
                        selectedItems.Add(item);
                    }
                    break;
                case SelectionMode.None:
                    break;
            }

            if (hasChanged)
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
        }


        //private void OnSelectedChanged(Selection<GroupedListItem<TItem>> selection)
        //{
        //    List<string> s = new List<string>();

        //    var finalList = new System.Collections.Generic.List<GroupedListItem<TItem>>(selection.SelectedItems);

        //    var itemsToAdd = selection.SelectedItems.Except(internalSelection.SelectedItems).ToList();
        //    var itemsToRemove = internalSelection.SelectedItems.Except(selection.SelectedItems).ToList();
        //    itemsToRemove.ForEach(x =>
        //    {
        //        var remove = GetChildrenRecursive(x);
        //        finalList.Remove(remove);
        //    });

        //    itemsToAdd.ForEach(x =>
        //    {
        //        var add = GetChildrenRecursive(x);
        //        finalList.Add(add);
        //    });

        //    //check to see if a header needs to be turned OFF because all of its children are *not* selected.
        //    restart:
        //    var headers = finalList.Where(x => x is HeaderItem<TItem>).Cast<HeaderItem<TItem>>().ToList();
        //    foreach (var header in headers)
        //    {
        //        if (header.Children.Except(finalList).Count() > 0)
        //        {
        //            finalList.Remove(header);
        //            //start loop over again, simplest way to start over is a goto statement.  This is needed when a header turns off, but it's parent needs to turn off, too.
        //            goto restart;
        //        }
        //    }

        //    //check to see if a header needs to be turned ON because all of its children *are* selected.
        //    var potentialHeaders = finalList.Select(x => x.Parent).Where(x => x != null).Distinct().ToList();
        //    foreach (var header in potentialHeaders)
        //    {
        //        if (header.Children.Except(finalList).Count() == 0)
        //            finalList.Add(header);
        //    }

        //    SelectionChanged.InvokeAsync(new BFUSelection<TItem>(finalList.Select(x => x.Item)));
        //}

    }
}
