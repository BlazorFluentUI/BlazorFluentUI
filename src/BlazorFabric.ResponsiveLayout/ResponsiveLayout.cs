using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class ResponsiveLayout : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        
        [Inject] private IJSRuntime _jsRuntime { get; set; }

        public IEnumerable<ResponsiveLayoutItem> ActiveItems { get; set; } = new List<ResponsiveLayoutItem>();
        

        private bool _jsAvailable = false;
        private OrderedDictionary<ResponsiveLayoutItem, string> _mediaQueries = new OrderedDictionary<ResponsiveLayoutItem, string>();
        private Dictionary<ResponsiveLayoutItem, double> _handlers = new Dictionary<ResponsiveLayoutItem, double>();
        private bool _jsInvoke;

        //private SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {            
            builder.OpenComponent<CascadingValue<ResponsiveLayout>>(0);
            builder.AddAttribute(1, "Value", this);
            builder.AddAttribute(2, "ChildContent", ChildContent);
            builder.CloseComponent();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Debug.WriteLine("FirstRender of responsiveLayout");
                _jsAvailable = true;

            }
            Debug.WriteLine("Secondary render of responsiveLayout");
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task AddQueryAsync(ResponsiveLayoutItem item, string query)
        {                        
            _mediaQueries[item] = query;
            Debug.WriteLine($"registering query: {query}");
            var handler = await _jsRuntime.InvokeAsync<double>("BlazorFabricResponsiveLayout.registerMediaQueryWatcher", DotNetObjectReference.Create(this), query);
            if (_handlers.ContainsKey(item))
            {
                await _jsRuntime.InvokeVoidAsync("BlazorFabricResponsiveLayout.unregisterHandler", _handlers[item]);
                _handlers.Remove(item);
            }
            _handlers.Add(item, handler);

            var valid = await _jsRuntime.InvokeAsync<bool>("BlazorFabricResponsiveLayout.testQuery", query);

            if (valid)
            {
                Debug.WriteLine($"query was valid: {query}");
                var activeItems = GetActiveResponsiveItems(query);
                var itemsToRemove = ActiveItems.Except(activeItems).ToList();
                var itemsToAdd = activeItems.Except(ActiveItems).ToList();
                if (itemsToAdd.Any() || itemsToRemove.Any())
                    ActiveItems = activeItems;
                foreach (var changeItem in itemsToRemove)
                    changeItem.NotifyStateChange();
                foreach (var changeItem in itemsToAdd)
                    changeItem.NotifyStateChange();

                //Force a new render since we don't want the Default to stay on screen if it's not correct
                StateHasChanged();
                //if (ActiveItems.Except(activeItems).Any() || activeItems.Except(ActiveItems).Any())
                //{
                //    ActiveItems = activeItems;
                //    StateHasChanged();
                //}
            }
            //_semaphore.Release();
        }

        private IEnumerable<ResponsiveLayoutItem> GetActiveResponsiveItems(string query)
        {
            var queries = _mediaQueries.Where(x => x.Value == query);
            if (queries.Count() == 0)
                return new List<ResponsiveLayoutItem> { _mediaQueries.FirstOrDefault().Key };
            else
                return queries.Select(x => x.Key); //get Default!
        }

        [JSInvokable]
        public void QueryChanged(string query)
        {
            var activeItems = GetActiveResponsiveItems(query);
            var itemsToRemove = ActiveItems.Except(activeItems).ToList();
            var itemsToAdd = activeItems.Except(ActiveItems).ToList();
            if (itemsToAdd.Any() || itemsToRemove.Any())
                ActiveItems = activeItems;
            foreach (var changeItem in itemsToRemove)
                changeItem.NotifyStateChange();
            foreach (var changeItem in itemsToAdd)
                changeItem.NotifyStateChange();

            
            //if (ActiveItems.Count() != activeItems.Count() || ActiveItems.Except(activeItems).Any() || activeItems.Except(ActiveItems).Any())
            //{
            //    ActiveItems = activeItems;
            //    Debug.WriteLine("QueryChanged state changed called");
            //    StateHasChanged();
            //}
        }


        //private void SetActiveQuery(string query)
        //{
        //    if (MinWidthContent.ContainsKey(minWidth))
        //    {
        //        CurrentView = MinWidthContent[minWidth];
        //    }
        //    else
        //        throw new Exception("The minWidth wasn't in the dictionary.");
        //}
    }
}
