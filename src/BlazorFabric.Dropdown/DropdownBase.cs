using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Dropdown
{
    public class DropdownBase<TItem> : FabricComponentBase
    {
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected IEnumerable<string> DefaultSelectedKeys { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected int DropdownWidth { get; set; } = 0;
        [Parameter] protected string ErrorMessage { get; set; }
        [Parameter] protected IList<TItem> ItemsSource { get; set; }
        [Parameter] protected RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] protected string Label { get; set; }
        [Parameter] public bool MultiSelect { get; set; }
        [Parameter] protected EventCallback<(string itemKey, bool isAdded)> OnChange { get; set; } 
        [Parameter] protected string Placeholder { get; set; }
        [Parameter] protected bool Required { get; set; }
        [Parameter] protected ResponsiveMode ResponsiveMode { get; set; }
        [Parameter] public List<string> SelectedKeys { get; set; } = new List<string>();
        [Parameter] protected EventCallback<List<string>> SelectedKeysChanged { get; set; }





        protected bool isOpen { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected bool isSmall = false;
        protected Rectangle dropDownBounds = new Rectangle();
        private bool firstRender = true;


        public void ResetSelection()
        {
            SelectedKeys.Clear();

            if (SelectedKeysChanged.HasDelegate)
                SelectedKeysChanged.InvokeAsync(SelectedKeys);

            StateHasChanged();
        }
        public void AddSelection(string key)
        {
            if (SelectedKeys.Contains(key))
                throw new Exception("This key was already selected.");

            if (OnChange.HasDelegate)
                OnChange.InvokeAsync((key, true));
                        
            SelectedKeys.Add(key);

            if (SelectedKeysChanged.HasDelegate)
                SelectedKeysChanged.InvokeAsync(SelectedKeys);

            if (!this.MultiSelect)
                isOpen = false;

            StateHasChanged();
        }
        public void RemoveSelection(string key)
        {
            if (!SelectedKeys.Contains(key))
                throw new Exception("This key was not already selected.");

            if (OnChange.HasDelegate)
                OnChange.InvokeAsync((key, false));

            if (SelectedKeysChanged.HasDelegate)
                SelectedKeysChanged.InvokeAsync(SelectedKeys);

            SelectedKeys.Remove(key);
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync()
        {
            if (firstRender)
            { 
                dropDownBounds = await this.GetBoundsAsync();
                firstRender = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (this.DefaultSelectedKeys != null)
            {
                foreach (var key in this.DefaultSelectedKeys)
                    AddSelection(key);
            }
            return base.OnParametersSetAsync();
        }


        protected Task ClickHandler(UIMouseEventArgs args)
        {
            if (!this.Disabled)
                isOpen = !isOpen;
            return Task.CompletedTask;
        }
        protected Task FocusHandler(UIFocusEventArgs args)
        {
            // Could write logic to open on focus automatically...
            //isOpen = true;
            return Task.CompletedTask;
        }

        protected Task DismissHandler()
        {
            isOpen = false;
            return Task.CompletedTask;
        }

    }
}
