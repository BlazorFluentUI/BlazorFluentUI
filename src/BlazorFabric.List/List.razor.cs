using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorFabric
{
    public partial class List<TItem> : FabricComponentBase, IDisposable
    {
        //protected bool firstRender = false;

        protected const int DEFAULT_ITEMS_PER_PAGE = 10;
        protected const int DEFAULT_RENDERED_WINDOWS_BEHIND = 2;
        protected const int DEFAULT_RENDERED_WINDOWS_AHEAD = 2;

        private double thresholdChangePercent = 0.10;

        protected ElementReference rootDiv;
        protected ElementReference surfaceDiv;

        private double averagePageHeight = 100;
        private bool isFirstRender = true;
        private bool shouldRender = false;

        private int minRenderedPage;
        private int maxRenderedPage;

        private double _height;

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public Func<TItem, MouseEventArgs, Task> ItemClicked { get; set; }
        [Parameter] public IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;
        [Parameter] public bool ItemFocusable { get; set; } = false;

        private IEnumerable<TItem> _itemsSource;

        protected RenderFragment ItemPagesRender { get; set; }

        private ISubject<(int index, double height)> pageMeasureSubject = new Subject<(int index, double height)>();

        private IDisposable heightSub;

        private System.Collections.Generic.List<ListPage<TItem>> renderedPages = new System.Collections.Generic.List<ListPage<TItem>>();

        private System.Collections.Generic.List<TItem> selectedItems = new System.Collections.Generic.List<TItem>();
        private string _resizeRegistration;

        protected override Task OnInitializedAsync()
        {
            ItemPagesRender = RenderPages(0, 0);
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (_itemsSource == null && ItemsSource != null)
            {
                if (this.ItemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    (this.ItemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged += ListBase_CollectionChanged;
                }
            }
            else if (_itemsSource != null && ItemsSource != _itemsSource)
            {
                if (this._itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    (this._itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
                }
                if (this.ItemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    (this.ItemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged += ListBase_CollectionChanged;
                }
            }
            return base.OnParametersSetAsync();
        }


        private void ListBase_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                return true;
            }
            return false;
        }


        private RenderFragment RenderPages(int startPage, int endPage, double leadingPadding = 0) => builder =>
          {
              try
              {
                  renderedPages.Clear();
                  var totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);

                  builder.OpenComponent(0, typeof(ListVirtualizationSpacer));
                  builder.AddAttribute(1, "Height", averagePageHeight * startPage);
                  builder.CloseComponent();

                  const int lineCount = 9;
                  for (var i = 0; i <= totalPages; i++)
                  {
                      //Debug.WriteLine($"Drawing page {i}");
                      if (startPage <= i && endPage >= i)
                      {
                          builder.OpenComponent(i * lineCount + 2, typeof(ListPage<TItem>));
                          builder.AddAttribute(i * lineCount + 3, "ItemTemplate", ItemTemplate);
                          builder.AddAttribute(i * lineCount + 4, "ItemsSource", ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE));//(ItemsSource.Count() > 0 ? ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE) : ItemsSource));
                          builder.AddAttribute(i * lineCount + 5, "StartIndex", i * DEFAULT_ITEMS_PER_PAGE);
                          builder.AddAttribute(i * lineCount + 6, "PageMeasureSubject", pageMeasureSubject);
                          builder.AddAttribute(i * lineCount + 7, "ItemClicked", (Func<object, MouseEventArgs, Task>)OnItemClick);
                          builder.AddAttribute(i * lineCount + 8, "SelectedItems", selectedItems);
                          builder.AddAttribute(i * lineCount + 9, "ItemFocusable", SelectionMode != SelectionMode.None ? true : ItemFocusable);
                          builder.AddComponentReferenceCapture(i * lineCount + 10, (comp) => renderedPages.Add((ListPage<TItem>)comp));
                          builder.CloseComponent();
                      }
                  }

                  builder.OpenComponent(totalPages * lineCount, typeof(ListVirtualizationSpacer));
                  builder.AddAttribute(totalPages * lineCount + 1, "Height", averagePageHeight * (totalPages - endPage - 1));
                  builder.CloseComponent();
              }
              catch (Exception ex)
              {
                  Debug.WriteLine(ex.ToString());
              }

          };

        protected Task OnItemClick(object item, MouseEventArgs e)
        {
            var castItem = (TItem)item;
            switch (SelectionMode)
            {
                case SelectionMode.Multiple:
                    if (selectedItems.Contains(castItem))
                        selectedItems.Remove(castItem);
                    else
                        selectedItems.Add(castItem);
                    shouldRender = true;
                    break;
                case SelectionMode.Single:
                    if (selectedItems.Contains(castItem))
                        selectedItems.Remove(castItem);
                    else
                    {
                        selectedItems.Clear();
                        selectedItems.Add(castItem);
                    }
                    shouldRender = true;
                    break;
                case SelectionMode.None:
                    break;

            }

            ItemClicked?.Invoke(castItem, e);


            if (shouldRender == true)
                this.StateHasChanged();

            return Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _resizeRegistration = await JSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "ResizeHandler");

                await MeasureContainerAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task MeasureContainerAsync()
        {
            var surfaceRect = await this.JSRuntime.InvokeAsync<JSRect>("BlazorFabricList.measureElementRect", this.surfaceDiv);
            _height = surfaceRect.height;

            if (heightSub != null)
            {
                heightSub.Dispose();
                heightSub = null;
            }

            heightSub = pageMeasureSubject.Subscribe(x =>
            {
                if (isFirstRender && x.index == 0)
                {
                    averagePageHeight = x.height;
                    var aheadSpace = surfaceRect.height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
                    minRenderedPage = 0;
                    if (averagePageHeight != 0)
                        maxRenderedPage = (int)Math.Ceiling(aheadSpace / averagePageHeight);

                    isFirstRender = false;
                    shouldRender = true;
                    ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                    this.StateHasChanged();
                }
                else if (!isFirstRender)
                {
                    if (averagePageHeight != 0 && ((x.height - averagePageHeight) / averagePageHeight - 1 > thresholdChangePercent))
                    {
                        averagePageHeight = x.height;

                        shouldRender = true;
                        ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                        this.StateHasChanged();
                    }
                    else
                    {
                        averagePageHeight = (averagePageHeight + x.height) / 2;
                    }
                }
            });
        }

        [JSInvokable]
        public async void ResizeHandler(double width, double height)
        {
            await MeasureContainerAsync();
        }


        public async Task OnScroll(EventArgs args)
        {
            try
            {
                var scrollRect = await this.JSRuntime.InvokeAsync<JSRect>("BlazorFabricList.measureScrollWindow", this.surfaceDiv);

                var rearSpace = _height * DEFAULT_RENDERED_WINDOWS_BEHIND;
                var aheadSpace = _height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
                var totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);
                var currentPage = (int)Math.Floor(scrollRect.top / averagePageHeight);


                var minPage = Math.Max(0, (int)Math.Ceiling((scrollRect.top - rearSpace) / averagePageHeight) - 1);

                var maxPage = Math.Min(Math.Max(totalPages - 1, 0), Math.Max((int)Math.Ceiling((scrollRect.top + aheadSpace) / averagePageHeight) - 1, 0));

                if (minRenderedPage != minPage || maxRenderedPage != maxPage)
                {
                    minRenderedPage = minPage;
                    maxRenderedPage = maxPage;
                                        
                    shouldRender = true;
                    Debug.WriteLine($"Scroll causing pages {minPage} to {maxPage} to rerender.");
                    ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                    this.StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

        public async void Dispose()
        {            
            if (_itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
            {
                (_itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
            }
            if (_resizeRegistration != null)
            {
                await JSRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.deregisterResizeEvent", _resizeRegistration);
            }
            Debug.WriteLine("List was disposed");
        }
    }
}
