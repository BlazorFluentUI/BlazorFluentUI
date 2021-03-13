using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public partial class SearchBox<T> : FluentUIComponentBase

    {
        [Parameter] public int Delay { get; set; }
        string filter;
        [Parameter] public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                if(filter != value)
                {
                    filter = value;
                    filterChanged += 2;
                    SearchNewEntries();
                    if (!IsMultiSelect)
                    {
                        SelectedItemChanged.InvokeAsync();
                    }
                }
            }
        }
        int filterChanged;
        [Parameter] public double InputWidth { get; set; } = 200;
        [Parameter] public string IconName { get; set; } = "Search";
        [Parameter] public string IconSrc { get; set; }
        [Parameter] public bool IsDropDownOpen { get; set; }
        [Parameter] public bool IsLoading { get; set; }

        [Parameter] public T SelectedItem { get; set; }
        [Parameter] public EventCallback<T> SelectedItemChanged { get; set; }
        [Parameter] public ICollection<T> SelectedItems { get; set; }
        [Parameter] public EventCallback<ICollection<T>> SelectedItemsChanged { get; set; }
        [Parameter] public string Placeholder { get; set; } = "Enter here";
        [Parameter] public bool IsMultiSelect { get; set; }
        [Parameter] public Func<string, IEnumerable<T>> ProvideSuggestions { get; set; }
        [Parameter] public Func<object, string> ProvideString { get; set; }
        [Parameter] public int DropdownWidth { get; set; } = 0;
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        [Parameter] public EventCallback<bool> ContextMenuShownChanged { get; set; }
        [Parameter] public RenderFragment<T> SearchItemTemplate { get; set; }
        [Parameter] public RenderFragment<T> SelectedItemTemplate { get; set; }

        List<object> suggestions = new();
        protected bool IsOpen { get; set; }
        TextField textFieldRef;
        List<SelectedItem<T>> selectedItemsVisuals = new();

        private ICollection<IRule> DropdownLocalRules { get; set; } = new List<IRule>();

        void SearchNewEntries()
        {
            suggestions.Clear();
            IEnumerable<T>? suggestionsInt = ProvideSuggestions(filter);
            if (suggestionsInt != null)
            {
                foreach (T? suggestionInt in suggestionsInt)
                {
                     suggestions.Add(suggestionInt);
                }
            }
            IsOpen = true;
        }

        protected  override async void OnAfterRender(bool firstRender)
        {
            if (filterChanged > 0)
            {
                await textFieldRef.Focus();
                filterChanged--;
            }
            base.OnAfterRender(firstRender);
        }


        protected void DismissHandler()
        {
            IsOpen = false;
        }

        void ClickedSelectHandler(SearchItem<T> searchItem)
        {
            if (IsMultiSelect)
            {
                Filter = "";
                if(SelectedItems == null)
                {
                    SelectedItems = new List<T>();
                }
                if (!SelectedItems.Contains((T)searchItem.Content))
                {
                    SelectedItems.Add((T)searchItem.Content);
                }
                SelectedItemsChanged.InvokeAsync(SelectedItems);
            }
            else
            {
                if (searchItem.Content is string stringContent)
                {
                    Filter = stringContent;
                }
                else
                {
                    Filter = ProvideString((T)searchItem.Content);
                }
                SelectedItemChanged.InvokeAsync((T)searchItem.Content);
            }
            IsOpen = false;
        }

        void ClickedDeletedHandler(SelectedItem<T> selectedItem)
        {
            SelectedItems.Remove(selectedItem.Content);
            SelectedItemsChanged.InvokeAsync(SelectedItems);
        }
    }
}