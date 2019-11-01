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
        //[Parameter] public double[] MinWidthArray { get; set; }
        
        //[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> MinWidthContent { get; set; }  // do zero first (small phone) up to largest size (desktop)
        
        
        //public CascadingValue<double> MinWidth { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        
        [Inject] private IJSRuntime _jsRuntime { get; set; }

        public ResponsiveLayoutItem ActiveItem { get; set; }
        

        private bool _jsAvailable = false;
        private OrderedDictionary<ResponsiveLayoutItem, string> _mediaQueries = new OrderedDictionary<ResponsiveLayoutItem, string>();
        private Dictionary<ResponsiveLayoutItem, double> _handlers = new Dictionary<ResponsiveLayoutItem, double>();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            
            builder.OpenComponent<CascadingValue<ResponsiveLayout>>(0);
            builder.AddAttribute(1, "Value", this);
            builder.AddAttribute(2, "ChildContent", ChildContent);
            builder.CloseComponent();

            //base.BuildRenderTree(builder);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Debug.WriteLine("FirstRender of responsiveLayout");
                _jsAvailable = true;
                _semaphore.Release();
                Debug.WriteLine("Semaphore release");
                //var minWidth = await _jsRuntime.InvokeAsync<double>("BlazorFabricResponsiveLayout.getInitialMediaQuery", MinWidthContent.Select(x => x.Key));
                //SetCurrentView(minWidth);
                //await _jsRuntime.InvokeAsync<double[]>("BlazorFabricResponsiveLayout.registerMediaQueryWatchers", DotNetObjectReference.Create(this), MinWidthContent.Select(x => x.Key));
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async void AddQuery(ResponsiveLayoutItem item, string query)
        {
            Debug.WriteLine($"waiting for semaphore: {query}");
            await _semaphore.WaitAsync();
            Debug.WriteLine($"got through: {query}");
            if (_mediaQueries.ContainsKey(item) && _handlers.ContainsKey(item))
            {
                if (_mediaQueries[item] == query)
                {
                    _semaphore.Release();
                    return;
                }

                await _jsRuntime.InvokeVoidAsync("BlazorFabricResponsiveLayout.unregisterHandler", _handlers[item]);
                _handlers.Remove(item);
            }
            
            _mediaQueries[item] = query;
            Debug.WriteLine($"registering query: {query}");
            var handler = await _jsRuntime.InvokeAsync<double>("BlazorFabricResponsiveLayout.registerMediaQueryWatcher", DotNetObjectReference.Create(this), query);
            _handlers.Add(item, handler);

            var valid = await _jsRuntime.InvokeAsync<bool>("BlazorFabricResponsiveLayout.testQuery", query);

            if (valid)
            {
                Debug.WriteLine($"query was valid: {query}");
                var activeItem = GetActiveResponsiveItem(query);
                if (activeItem != ActiveItem)
                {
                    ActiveItem = activeItem;
                    StateHasChanged();
                }
            }
            _semaphore.Release();
        }

        private ResponsiveLayoutItem GetActiveResponsiveItem(string query)
        {
            var item = _mediaQueries.FirstOrDefault(x => x.Value == query);
            if (item.Equals(default(KeyValuePair<ResponsiveLayoutItem, string>)))
                return _mediaQueries.FirstOrDefault().Key;
            else
                return item.Key;
        }

        [JSInvokable]
        public void QueryChanged(string query)
        {
            var activeItem = GetActiveResponsiveItem(query);
            if (activeItem != ActiveItem)
            {
                ActiveItem = activeItem;
                StateHasChanged();
            }
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
