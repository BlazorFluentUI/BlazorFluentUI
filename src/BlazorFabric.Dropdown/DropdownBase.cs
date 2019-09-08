using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Dropdown
{
    public class DropdownBase<TItem> : FabricComponentBase
    {
        [Parameter] public string AriaLabel { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public IEnumerable<string> DefaultSelectedKeys { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public int DropdownWidth { get; set; } = 0;
        [Parameter] public string ErrorMessage { get; set; }
        [Parameter] public IList<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public bool MultiSelect { get; set; }
        [Parameter] public EventCallback<(string itemKey, bool isAdded)> OnChange { get; set; } 
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public bool Required { get; set; }
        [Parameter] public ResponsiveMode ResponsiveMode { get; set; }
        [Parameter] public List<string> SelectedKeys { get; set; } = new List<string>();
        [Parameter] public EventCallback<List<string>> SelectedKeysChanged { get; set; }





        protected bool isOpen { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected bool isSmall = false;
        protected Rectangle dropDownBounds = new Rectangle();
        //private bool firstRender = true;


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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
                dropDownBounds = await this.GetBoundsAsync();
                //firstRender = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
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


        protected Task ClickHandler(MouseEventArgs args)
        {
            if (!this.Disabled)
                isOpen = !isOpen;
            return Task.CompletedTask;
        }
        protected Task FocusHandler(FocusEventArgs args)
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
