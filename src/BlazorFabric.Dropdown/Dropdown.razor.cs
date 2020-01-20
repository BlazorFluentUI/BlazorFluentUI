using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Dropdown<TItem> : ResponsiveFabricComponentBase
    {
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
        [Parameter] public string SelectedKey { get; set; }
        [Parameter] public EventCallback<string> SelectedKeyChanged { get; set; }
        [Parameter] public List<string> SelectedKeys { get; set; } = new List<string>();
        [Parameter] public EventCallback<List<string>> SelectedKeysChanged { get; set; }

        [Inject]
        private IJSRuntime jSRuntime { get; set; }

        protected bool isOpen { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected bool isSmall = false;
        protected Rectangle dropDownBounds = new Rectangle();

        private ElementReference calloutReference;
        private ElementReference panelReference;
        private ElementReference _chosenReference;
        private string _registrationToken;

        private FocusZone calloutFocusZone;

        //private bool firstRender = true;

        public void ResetSelection()
        {
            SelectedKeys.Clear();
            SelectedKey = null;

            if (MultiSelect)
            {
                if (SelectedKeysChanged.HasDelegate)
                    SelectedKeysChanged.InvokeAsync(SelectedKeys);
            }
            else
            {
                if (SelectedKeyChanged.HasDelegate)
                    SelectedKeyChanged.InvokeAsync(SelectedKey);
            }
            StateHasChanged();
        }

        public void AddSelection(string key)
        {
            if (MultiSelect)
            {
                if (SelectedKeys.Contains(key))
                    throw new Exception("This key was already selected.");

                if (OnChange.HasDelegate)
                    OnChange.InvokeAsync((key, true));

                SelectedKeys.Add(key);

                if (SelectedKeysChanged.HasDelegate)
                    SelectedKeysChanged.InvokeAsync(SelectedKeys);
            }
            else
            {
                if (SelectedKey!= key)
                {
                    SelectedKey = key;
                    if (OnChange.HasDelegate)
                        OnChange.InvokeAsync((key, true));
                    if (SelectedKeyChanged.HasDelegate)
                        SelectedKeyChanged.InvokeAsync(SelectedKey);
                }
                isOpen = false;
            }
            StateHasChanged();
        }

        public void RemoveSelection(string key)
        {
            if (MultiSelect)
            {
                if (!SelectedKeys.Contains(key))
                    throw new Exception("This key was not already selected.");

                if (OnChange.HasDelegate)
                    OnChange.InvokeAsync((key, false));

                SelectedKeys.Remove(key);  //this used to be following the next command.  A bug?  I moved it here...

                if (SelectedKeysChanged.HasDelegate)
                    SelectedKeysChanged.InvokeAsync(SelectedKeys);

            }
            else
            {
                if (SelectedKey != null)
                {
                    SelectedKey = null;
                    if (OnChange.HasDelegate)
                        OnChange.InvokeAsync((key, false));

                    if (SelectedKeyChanged.HasDelegate)
                        SelectedKeyChanged.InvokeAsync(SelectedKey);
                }
            }
            StateHasChanged();
        }

        [JSInvokable]
        public override async Task OnResizedAsync(double windowWidth, double windowHeight)
        {
            var oldBounds = dropDownBounds;
            dropDownBounds = await this.GetBoundsAsync();
            if (oldBounds.width != dropDownBounds.width)
            {
                StateHasChanged();
            }
            await base.OnResizedAsync(windowWidth, windowHeight);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
                dropDownBounds = await this.GetBoundsAsync();
                //firstRender = false;
                StateHasChanged();
            }
            if (isOpen && _registrationToken == null)
                await RegisterListFocusAsync();

            if (!isOpen && _registrationToken != null)
                await DeregisterListFocusAsync();

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (this.DefaultSelectedKeys != null)
            {
                foreach (var key in this.DefaultSelectedKeys)
                    AddSelection(key);
            }
        }

        private async Task RegisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await DeregisterListFocusAsync();
            }
            if ((int)CurrentMode <= (int)ResponsiveMode.Medium)
                _chosenReference = panelReference;
            else
                _chosenReference = calloutReference;
            _registrationToken = await jSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerKeyEventsForList", _chosenReference);
        }

        private async Task DeregisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await jSRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.deregisterKeyEventsForList", _registrationToken);
                _registrationToken = null;
            }
        }

        private void OnPositioned()
        {
            calloutFocusZone.FocusFirstElement();
        }

        private Task KeydownHandler(KeyboardEventArgs args)
        {
            bool containsExpandCollapseModifier = args.AltKey || args.MetaKey;
            switch (args.Key)
            {
                case "Enter":
                case " ":
                    isOpen = !isOpen;
                    break;
                case "Escape":
                    isOpen = false;
                    break;
                case "ArrowDown":
                    if (containsExpandCollapseModifier)
                    {
                        isOpen = true;
                    }
                    break;
            }
            return Task.CompletedTask;
        }

        protected Task ClickHandler(MouseEventArgs args)
        {
            if (!this.Disabled)
                isOpen = !isOpen;  //There is a problem here.  Clicking when open causes automatic dismissal (light dismiss) so this just opens it again.
            return Task.CompletedTask;
        }
        protected Task FocusHandler(FocusEventArgs args)
        {
            // Could write logic to open on focus automatically...
            //isOpen = true;
            return Task.CompletedTask;
        }

        protected async Task DismissHandler()
        {
            isOpen = false;
        }

      

    }
}
