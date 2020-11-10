using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        public bool DisableRenderOnSelectionChanged { get; set; } = false;

        [Parameter]
        public bool EnterModalOnTouch { get; set; }

        [Parameter]
        public bool IsSelectedOnFocus { get; set; } = true;

        [Parameter]
        public EventCallback<TItem> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemInvoked { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public EventCallback<Selection<TItem>> SelectionChanged { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }


        private HashSet<TItem> selectedItems = new HashSet<TItem>();

        private BehaviorSubject<ICollection<TItem>> selectedItemsSubject;

        public IObservable<ICollection<TItem>> SelectedItemsObservable { get; private set; }

        private bool doNotRenderOnce = false;

        protected override bool ShouldRender()
        {
            if (doNotRenderOnce && DisableRenderOnSelectionChanged)
            {
                doNotRenderOnce = false;
                return false;
            }
            else
                doNotRenderOnce = false;

            return true;
        }

        protected override void OnInitialized()
        {
            selectedItemsSubject = new BehaviorSubject<ICollection<TItem>>(selectedItems);
            SelectedItemsObservable = selectedItemsSubject.AsObservable();
            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Selection != null && Selection.SelectedItems != selectedItems)
            {
                selectedItems = new System.Collections.Generic.HashSet<TItem>(Selection.SelectedItems);
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
            {
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }

        public void AddItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                if (!selectedItems.Contains(item))
                    selectedItems.Add(item);
            }

            if (items != null && items.Count() > 0)
            {
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }

        public void RemoveItems(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                selectedItems.Remove(item);
            }

            if (items != null && items.Count() > 0)
            {
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
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

            if ((itemsToAdd != null && itemsToAdd.Count() > 0) || (itemsToRemove != null && itemsToRemove.Count() > 0))
            {
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }



        public void ClearSelection()
        {
            if (selectedItems.Count>0)
            {
                selectedItems.Clear();
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }

        // For end-users to let SelectionMode handle what to do.
        public void HandleClick(TItem item)
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
            {
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }

        // For end-users to let SelectionMode handle what to do.
        public void HandleToggle(TItem item)
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
            {
                doNotRenderOnce = true;
                selectedItemsSubject.OnNext(selectedItems);
                SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            }
        }

    }
}
