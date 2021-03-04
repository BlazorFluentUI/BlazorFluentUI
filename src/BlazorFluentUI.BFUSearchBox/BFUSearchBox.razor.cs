using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace BlazorFluentUI
{
    public partial class BFUSearchBox<T> : BFUComponentBase

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
                }
            }
        }
        int filterChanged;
        [Parameter] public double InputWidth { get; set; } = 200;
        [Parameter] public string IconName { get; set; } = "Search";
        [Parameter] public string IconSrc { get; set; }
        [Parameter] public bool IsDropDownOpen { get; set; }
        [Parameter] public bool IsLoading { get; set; }

        [Parameter] public object SelectedItem { get; set; }
        [Parameter] public ICollection<T> SelectedItems { get; set; }
        [Parameter] public string Placeholder { get; set; } = "Enter here";
        [Parameter] public bool IsMultiSelect { get; set; }
        [Parameter] public Func<string, IEnumerable<T>> ProvideSuggestions { get; set; }
        [Parameter] public Func<object, string> ProvideString { get; set; }
        [Parameter] public int DropdownWidth { get; set; } = 0;
        [Inject] private IJSRuntime? jSRuntime { get; set; }

        [Parameter] public EventCallback<bool> ContextMenuShownChanged { get; set; }
        [Parameter] public RenderFragment<T> SearchItemTemplate { get; set; }
        [Parameter] public RenderFragment<T> SelectedItemTemplate { get; set; }

        List<object> suggestions = new List<object>();
        protected bool isOpen { get; set; }
        BFUTextField textFieldRef;
        List<SelectedItem<T>> selectedItemsVisuals = new List<SelectedItem<T>>();

        private ICollection<IRule> DropdownLocalRules { get; set; } = new List<IRule>();

        void SearchNewEntries()
        {
            suggestions.Clear();
            var suggestionsInt = ProvideSuggestions(filter);
            if (suggestionsInt != null)
            {
                foreach (var suggestionInt in suggestionsInt)
                {
                     suggestions.Add(suggestionInt);
                }
            }
            isOpen = true;
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
            isOpen = false;
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
            }
            isOpen = false;
        }

        void ClickedDeletedHandler(SelectedItem<T> selectedItem)
        {
            SelectedItems.Remove(selectedItem.Content);
        }
    }
}