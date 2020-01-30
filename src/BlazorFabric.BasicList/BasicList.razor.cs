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
using System.Reactive;

namespace BlazorFabric
{
    public partial class BasicList<TItem> : FabricComponentBase, IDisposable
    {
        //protected bool firstRender = false;
        private Dictionary<string, ElementReference> _listItemReferences = new Dictionary<string, ElementReference>();

        private const double RESIZE_DELAY = 16;
        private const double MIN_SCROLL_UPDATE_DELAY = 100;
        private const double MAX_SCROLL_UPDATE_DELAY = 500;
        private const double IDLE_DEBOUNCE_DELAY = 200;
        private const double DONE_SCROLLING_WAIT = 500;
        private const int DEFAULT_ITEMS_PER_PAGE = 10;
        private const int DEFAULT_RENDERED_WINDOWS_BEHIND = 2;
        private const int DEFAULT_RENDERED_WINDOWS_AHEAD = 2;
        private const double DEFAULT_PAGE_HEIGHT = 30;
        private const string PAGE_KEY_PREFIX = "page-";
        private const string SPACER_KEY_PREFIX = "spacer-";

        private double thresholdChangePercent = 0.10;

        private ElementReference surfaceDiv;

        private double averagePageHeight = 100;
        private bool isFirstRender = true;
        private bool shouldRender = false;

        private int minRenderedPage;
        private int maxRenderedPage;
        private ManualRectangle _surfaceRect = null;
        private double _height;

        private ManualRectangle _requiredRect = null;
        private ManualRectangle _allowedRect = null;
        private ManualRectangle _visibleRect = null;
        private ManualRectangle _materializedRect = null;
        private bool _jsAvailable;
        private int _listRegistration = -1;
        private string _resizeRegistration;
        private double _scrollHeight;
        private double _scrollTop;
        private double _estimatedPageHeight;
        private int _totalEstimates = 0;
        private int _requiredWindowsBehind = 0;
        private int _requiredWindowsAhead = 0;
        private int _focusedIndex = -1;

        class PageHeight
        {
            public double Height { get; set; }
            public int MeasureVersion { get; set; }
        }

        private Dictionary<int, PageHeight> _cachedPageHeights = new Dictionary<int, PageHeight>();
        //private Dictionary<string, (Page<TItem>, ListPage<TItem>)> _pageCache = new Dictionary<string, (Page<TItem>, ListPage<TItem>)>();

        //state
        private IEnumerable<Page<TItem>> _pages = new List<Page<TItem>>();
        private int _measureVersion = 0;
        private bool _isScrolling = false;
        
        
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public Func<int, ManualRectangle, int> GetItemCountForPage { get; set; }
        [Parameter] public Func<int, ManualRectangle, int, int> GetPageHeight { get; set; }
        [Parameter] public Func<int, ManualRectangle, PageSpecification> GetPageSpecification { get; set; }
        [Parameter] public EventCallback<TItem> ItemClicked { get; set; }
        [Parameter] public bool ItemFocusable { get; set; } = false;
        [Parameter] public IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
        [Parameter] public Func<bool> OnShouldVirtualize { get; set; } = () => true;
        [Parameter] public int RenderCount { get; set; } = -1;  //this means use items.Count
        [Parameter] public int RenderedWindowsAhead { get; set; } = 2;
        [Parameter] public int RenderedWindowsBehind { get; set; } = 2;
        [Parameter] public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;
        [Parameter] public int StartIndex { get; set; } = 0;

        private IEnumerable<TItem> _itemsSource;

        protected RenderFragment ItemPagesRender { get; set; }

        private ISubject<(int index, double height)> pageMeasureSubject = new Subject<(int index, double height)>();

        private IDisposable heightSub;

        //private System.Collections.Generic.List<ListPage<TItem>> renderedPages = new System.Collections.Generic.List<ListPage<TItem>>();

        private System.Collections.Generic.List<TItem> selectedItems = new System.Collections.Generic.List<TItem>();

        private Subject<Unit> _idleAsyncSubject = new Subject<Unit>();
        private IDisposable _idleAsyncSubscription;
        private bool _hasCompletedFirstRender;

        private Subject<Unit> _scrollSubject = new Subject<Unit>();
        private IDisposable _scrollSubscription;

        private Subject<Unit> _asyncScrollSubject = new Subject<Unit>();
        private IDisposable _asyncScrollSubscription;

        private Subject<Unit> _scrollingDoneSubject = new Subject<Unit>();
        private IDisposable _scrollingDoneSubscription;

        public BasicList()
        {
            
        }

        protected override Task OnInitializedAsync()
        {
            //ItemPagesRender = RenderPages(0, 0);
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


        private async void ListBase_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (_jsAvailable)
            {
                shouldRender = true;
                await UpdatePagesAsync();
                //await InvokeAsync(StateHasChanged);
            }
        }

        protected override bool ShouldRender()
        {
            return base.ShouldRender();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsAvailable = true;
                _idleAsyncSubscription = _idleAsyncSubject.Throttle(TimeSpan.FromMilliseconds(IDLE_DEBOUNCE_DELAY))
                .Do(async _ =>
                {
                    await InvokeAsync(async () =>
                    {
                        var requiredWindowsAhead = _requiredWindowsAhead;
                        var requiredWindowsBehind = _requiredWindowsBehind;
                        Debug.WriteLine("IdleAsync called");
                        var windowsAhead = Math.Min(RenderedWindowsAhead, requiredWindowsAhead + 1);
                        var windowsBehind = Math.Min(RenderedWindowsBehind, requiredWindowsBehind + 1);

                        if (windowsAhead != requiredWindowsAhead || windowsBehind != requiredWindowsBehind)
                        {
                            this._requiredWindowsAhead = windowsAhead;
                            this._requiredWindowsBehind = windowsBehind;
                       
                                await UpdateRenderRectsAsync();
                                await UpdatePagesAsync();
                        
                        }

                        if (RenderedWindowsAhead > windowsAhead || RenderedWindowsBehind > windowsBehind)
                            _idleAsyncSubject.OnNext(Unit.Default);
                    });
                }).Subscribe();

                _scrollSubscription = _scrollSubject.Throttle(TimeSpan.FromMilliseconds(100))
                    .Do(async _ =>
                    {
                        await InvokeAsync(async () =>
                        {
                            Debug.WriteLine("Scroll called");
                            if (!_isScrolling)
                            {
                                Debug.WriteLine("Setting isScrolling to TRUE");
                                _isScrolling = true;
                                StateHasChanged();
                            }
                            ResetRequiredWindows();
                            _scrollingDoneSubject.OnNext(Unit.Default);
                        });
                    })
                    .Subscribe();

                _asyncScrollSubscription = _asyncScrollSubject.Throttle(TimeSpan.FromMilliseconds(MIN_SCROLL_UPDATE_DELAY))
                    .Do(async _ =>
                    {
                        await InvokeAsync(async () =>
                        {
                            Debug.WriteLine("AsyncScroll called");

                            await UpdateRenderRectsAsync();
                            // Only update pages when the visible rect falls outside of the materialized rect.
                            if (_materializedRect == null || !IsContainedWithin(this._requiredRect != null ? _requiredRect : new ManualRectangle(), this._materializedRect))
                            {
                                await UpdatePagesAsync();

                            }
                            else
                            {
                                // console.log('requiredRect contained in materialized', this._requiredRect, this._materializedRect);
                            }
                        });
                    })
                    .Subscribe();

                _scrollingDoneSubscription = _scrollingDoneSubject.Throttle(TimeSpan.FromMilliseconds(DONE_SCROLLING_WAIT))
                    .Do(_ =>
                    {
                        Debug.WriteLine("Scrolling Done called");

                        if (_isScrolling)
                        {
                            _isScrolling = false;
                            InvokeAsync(StateHasChanged);
                        }
                    })
                    .Subscribe();

                _listRegistration = await JSRuntime.InvokeAsync<int>("BlazorFabricBasicList.register", DotNetObjectReference.Create(this), RootElementReference);
                _resizeRegistration = await JSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "ResizeHandler");

                // only update after first render
                await UpdatePagesAsync();

            }

            if (GetPageHeight == null)
            {
                var heightsChanged = await UpdatePageMeasurementsAsync();
                if (heightsChanged)
                {
                    _materializedRect = null;
                    if (!_hasCompletedFirstRender)
                    {
                        _hasCompletedFirstRender = true;
                        await UpdatePagesAsync();
                    }
                    else
                    {
                        _asyncScrollSubject.OnNext(Unit.Default);
                    }
                }
                else
                {
                    _idleAsyncSubject.OnNext(Unit.Default);
                }
            }
            else
            {
                _idleAsyncSubject.OnNext(Unit.Default);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public void ScrollHandler()
        {
            _scrollSubject.OnNext(Unit.Default);
            _asyncScrollSubject.OnNext(Unit.Default);
        }

        [JSInvokable]
        public async void ResizeHandler(double width, double height)
        {
            //await MeasureContainerAsync();
        }

        private async Task UpdatePagesAsync()
        {
            if (this._requiredRect == null)
                await UpdateRenderRectsAsync();

            var newState = BuildPages();
            //can notify page changes here for future implementation

            if (_measureVersion != newState.Item2)
            {
                _pages = newState.Item1;
                _measureVersion = newState.Item2;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                if (_pages?.Count() == newState.Item1.Count())
                {
                    for (var i = 0; i < _pages.Count(); i++)
                    {
                        if (_pages.ElementAt(i).Key != newState.Item1.ElementAt(i).Key || _pages.ElementAt(i).ItemCount != newState.Item1.ElementAt(i).ItemCount)
                        {
                            _pages = newState.Item1;
                            _measureVersion = newState.Item2;
                            await InvokeAsync(StateHasChanged);
                            break;
                        }
                    }
                }
                else
                {
                    _pages = newState.Item1;
                    _measureVersion = newState.Item2;
                    await InvokeAsync(StateHasChanged);
                }
            }

            //await InvokeAsync(StateHasChanged);
        }

        private async Task UpdateRenderRectsAsync(bool forceUpdate = false)
        {
            if (!OnShouldVirtualize())
                return;

            var surfaceRect = _surfaceRect != null ? _surfaceRect : ManualRectangle.EmptyRect();

            // contains scrollHeight + scrollTop
            var rectangle = await JSRuntime.InvokeAsync<ManualRectangle>("BlazorFabricBasicList.getScrollDimensions", _listRegistration);
            
            var scrollHeight = rectangle.height;
            var scrollTop = rectangle.top;
            

            if (forceUpdate || 
                _pages != null || 
                double.IsNaN(scrollHeight) || scrollHeight != 0 || 
                double.IsNaN(scrollTop) || scrollTop != 0 ||
                scrollHeight != _scrollHeight ||
                Math.Abs(_scrollTop - scrollTop) > _estimatedPageHeight / 3)
            {
                _surfaceRect = await this.JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", this.surfaceDiv);
                surfaceRect = _surfaceRect;
                _scrollTop = scrollTop;
            }

            // If the scroll height has changed, something in the container likely resized and
            // we should redo the page heights incase their content resized.
            if (forceUpdate || double.IsNaN(scrollHeight) || scrollHeight == 0 || scrollHeight != this._scrollHeight)
            {
                this._measureVersion++;
                if (_jsAvailable)
                    StateHasChanged();
            }

            this._scrollHeight = scrollHeight;

            // If the surface is above the container top or below the container bottom, or if this is not the first
            // render return empty rect.
            // The first time the list gets rendered we need to calculate the rectangle. The width of the list is
            // used to calculate the width of the list items.
            var windowDimensions = await JSRuntime.InvokeAsync<ManualRectangle>("BlazorFabricBaseComponent.getWindowRect");
            var visibleTop = Math.Max(0, -surfaceRect.top);
            var visibleRect = new ManualRectangle
            {
                top = visibleTop,
                left = surfaceRect.left,
                bottom = visibleTop + windowDimensions.height,
                right=surfaceRect.right,
                width=surfaceRect.width,
                height=windowDimensions.height
            };

            // The required/allowed rects are adjusted versions of the visible rect.
            this._requiredRect = ExpandRect(visibleRect, this._requiredWindowsBehind, this._requiredWindowsAhead);
            this._allowedRect = ExpandRect(visibleRect, RenderedWindowsBehind, RenderedWindowsAhead);

            // store the visible rect for later use.
            this._visibleRect = visibleRect;
        }

        private async Task<bool> UpdatePageMeasurementsAsync()
        {
            bool heightChanged = false;
            if (!OnShouldVirtualize())
                return heightChanged;

            for (var i=0; i< _pages.Count(); i++)
            {
                var page = _pages.ElementAt(i);
                if (page.Items != null)
                {
                    heightChanged = (await MeasurePageAsync(page)) || heightChanged;
                }
            }

            return heightChanged;
        }

        private async Task<bool> MeasurePageAsync(Page<TItem> page)
        {
            bool hasChangedHeight = false;
            var refSuccess = page.Key != null && this._listItemReferences.TryGetValue(page.Key, out var pageElementRef);
            var cacheSuccess = this._cachedPageHeights.TryGetValue(page.StartIndex, out var cachedHeight);

            if (refSuccess && OnShouldVirtualize() && (!cacheSuccess || cachedHeight.MeasureVersion!=this._measureVersion))
            {
                var newClientRect = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElement", pageElementRef);

                hasChangedHeight = page.Height != newClientRect.height;

                page.Height = newClientRect.height;

                _cachedPageHeights[page.StartIndex] = new PageHeight { Height = newClientRect.height, MeasureVersion = _measureVersion };

                _estimatedPageHeight = Math.Round((_estimatedPageHeight * _totalEstimates + newClientRect.height) / (_totalEstimates + 1));

                _totalEstimates++;
            }

            return hasChangedHeight;
        }

        private ManualRectangle ExpandRect(ManualRectangle rect, int pagesBefore, int pagesAfter)
        {
            var top = rect.top - pagesBefore * rect.height;
            var height = rect.height + (pagesBefore + pagesAfter) * rect.height;

            return new ManualRectangle
            {
                top = top,
                bottom = top + height,
                height = height,
                left = rect.left,
                right = rect.right,
                width = rect.width
            };
        }

        private (IEnumerable<Page<TItem>>, int) BuildPages()
        {
            var materializedRect = ManualRectangle.EmptyRect();
            var pages = new List<Page<TItem>>();
            var renderCount = (ItemsSource != null && RenderCount == -1) ? ItemsSource.Count() - StartIndex : RenderCount;
            int itemsPerPage = 1;
            double pageTop = 0;
            var focusedIndex = this._focusedIndex;
            var endIndex = StartIndex + renderCount;
            var shouldVirtualize = OnShouldVirtualize();
            Page<TItem> currentSpacer = null;

            // First render is very important to track; when we render cells, we have no idea of estimated page height.
            // So we should default to rendering only the first page so that we can get information.
            // However if the user provides a measure function, let's just assume they know the right heights.
            var isFirstRender = this._estimatedPageHeight == 0 && GetPageHeight == null;

            var allowedRect = this._allowedRect;

            var count = 0;
            for (var itemIndex = StartIndex; itemIndex < endIndex; itemIndex += itemsPerPage)
            {
                var pageSpecification = GetPageSpecificationInternal(itemIndex, allowedRect);
                itemsPerPage = pageSpecification.ItemCount;

                var pageBottom = pageTop + pageSpecification.Height - 1;
                bool isPageRendered = false;
                if (_pages != null)
                {
                    isPageRendered = _pages.FirstOrDefault(x => x.StartIndex == StartIndex) != null;
                }
                var isPageInAllowedRange = allowedRect == null || (pageBottom >= allowedRect.top && pageTop <= allowedRect.bottom);
                var isPageInRequiredRange = this._requiredRect == null || (pageBottom >= _requiredRect.top && pageTop <= _requiredRect.bottom);
                var isPageVisible = (!isFirstRender && (isPageInRequiredRange || (isPageInAllowedRange && isPageRendered))) || !shouldVirtualize;
                var isPageFocused = focusedIndex >= itemIndex && focusedIndex < itemIndex + itemsPerPage;
                var isFirstPage = itemIndex == StartIndex;

                // Only render whats visible, focused, or first page,
                // or when running in fast rendering mode (not in virtualized mode), we render all current items in pages
                if (isPageVisible || isPageFocused || isFirstPage)
                {
                    if (currentSpacer != null)
                    {
                        pages.Add(currentSpacer);
                        currentSpacer = null;
                    }
                    var itemsInPage = Math.Min(itemsPerPage, endIndex - itemIndex);
                    var newPage = CreatePage(
                        pageSpecification.Key,
                        ItemsSource.Skip(itemIndex).Take(itemsInPage),
                        itemIndex,
                        itemsInPage,
                        pageSpecification.Data);

                    newPage.Top = pageTop;
                    newPage.Height = pageSpecification.Height;
                    if (this._visibleRect != null && this._visibleRect.bottom > 0)
                    {
                        newPage.IsVisisble = pageBottom >= this._visibleRect.top && pageTop <= this._visibleRect.bottom;
                    }

                    pages.Add(newPage);

                    if (isPageInRequiredRange && this._allowedRect != null)
                    {
                        materializedRect = MergeRect(
                            materializedRect, 
                            new ManualRectangle
                            {
                                top = pageTop,
                                bottom = pageBottom,
                                height = pageSpecification.Height,
                                left = allowedRect.left,
                                right = allowedRect.right,
                                width = allowedRect.width
                            });
                    }

                }
                else
                {
                    if (currentSpacer == null)
                    {
                        currentSpacer = CreatePage(SPACER_KEY_PREFIX + itemIndex, null, itemIndex, 0, pageSpecification.Data, true);
                    }
                    currentSpacer.Height = currentSpacer.Height + (pageBottom - pageTop) + 1;
                    currentSpacer.ItemCount += itemsPerPage;
                }
                pageTop += pageBottom - pageTop + 1;

                // in virtualized mode, we render need to render first page then break and measure,
                // otherwise, we render all items without measurement to make rendering fast
                if (isFirstRender && shouldVirtualize)
                {
                    break;
                }
                count++;
            }

            if (currentSpacer != null)
            {
                currentSpacer.Key = SPACER_KEY_PREFIX + "end";
                pages.Add(currentSpacer);
            }

            this._materializedRect = materializedRect;

            return (pages, _measureVersion);
        }

        private Page<TItem> CreatePage(string pageKey, IEnumerable<TItem> items, int startIndex, int count, object data, bool isSpacer=false)
        {
            //if (pageKey != null && _pageCache.ContainsKey(pageKey))
            //    return _pageCache[pageKey].Item1;
            //else
                return new Page<TItem>
                {
                    Key = pageKey,
                    Items = items,
                    StartIndex = startIndex,
                    ItemCount = count,
                    Top = 0,
                    Height =0,
                    Data = data,
                    IsSpacer = isSpacer
                };
        }

        private ManualRectangle MergeRect(ManualRectangle targetRect, ManualRectangle newRect)
        {
            targetRect.top = newRect.top < targetRect.top || targetRect.top == -1 ? newRect.top : targetRect.top;
            targetRect.left = newRect.left < targetRect.left || targetRect.left == -1 ? newRect.left : targetRect.left;
            targetRect.bottom = newRect.bottom > targetRect.bottom || targetRect.bottom == -1 ? newRect.bottom : targetRect.bottom;
            targetRect.right = newRect.right > targetRect.right || targetRect.right == -1 ? newRect.right : targetRect.right;
            targetRect.width = targetRect.right - targetRect.left + 1;
            targetRect.height = targetRect.bottom - targetRect.top + 1;
            return targetRect;
        }

        private PageSpecification GetPageSpecificationInternal(int itemIndex, ManualRectangle visibleRect)
        {
            if (GetPageSpecification != null)
            {
                var pageData = GetPageSpecification(itemIndex, visibleRect);
                pageData.ItemCount = GetItemCountForPage != null ? GetItemCountForPage(itemIndex, visibleRect) : DEFAULT_ITEMS_PER_PAGE;
                pageData.Height = GetPageHeightInternal(itemIndex, visibleRect, pageData.ItemCount);
                return pageData;
            }
            else
            {
                var itemCount = GetItemCountForPage != null ? GetItemCountForPage(itemIndex, visibleRect) : DEFAULT_ITEMS_PER_PAGE;
                var height = GetPageHeightInternal(itemIndex, visibleRect, itemCount);
                return new PageSpecification() { ItemCount = itemCount, Height = height, Key=itemIndex.ToString() };
            }
        }

        private double GetPageHeightInternal(int itemIndex, ManualRectangle visibleRect, int itemCount)
        {
            if (GetPageHeight != null)
                return GetPageHeight(itemIndex, visibleRect, itemCount);
            else
            {
                if (_cachedPageHeights.ContainsKey(itemIndex))
                    return _cachedPageHeights[itemIndex].Height;
                else
                {
                    if (double.IsNaN(_estimatedPageHeight))
                        return DEFAULT_PAGE_HEIGHT;
                    else
                        return _estimatedPageHeight;
                }

            }
        }

        private void ResetRequiredWindows()
        {
            _requiredWindowsAhead = 0;
            _requiredWindowsBehind = 0;
        }

        private bool IsContainedWithin(ManualRectangle innerRect, ManualRectangle outerRect)
        {
            return (
              innerRect.top >= outerRect.top &&
              innerRect.left >= outerRect.left &&
              innerRect.bottom <= outerRect.bottom &&
              innerRect.right <= outerRect.right
            );
        }

//private RenderFragment RenderPages(int startPage, int endPage, double leadingPadding = 0) => async builder =>
//  {
//      if (_requiredRect == null)
//          await UpdateRenderRectsAsync();

//      //really should build pages, compare with old, and notify new pages created and old pages removed

//      try
//      {
//          renderedPages.Clear();
//          var totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);

//          builder.OpenComponent(0, typeof(ListVirtualizationSpacer));
//          builder.AddAttribute(1, "Height", averagePageHeight * startPage);
//          builder.CloseComponent();

//          const int lineCount = 9;
//          for (var i = 0; i <= totalPages; i++)
//          {
//              //Debug.WriteLine($"Drawing page {i}");
//              if (startPage <= i && endPage >= i)
//              {
//                  builder.OpenComponent(i * lineCount + 2, typeof(ListPage<TItem>));
//                  builder.AddAttribute(i * lineCount + 3, "ItemTemplate", ItemTemplate);
//                  if (GetItemCountForPage != null)
//                  {
//                      builder.AddAttribute(i * lineCount + 4, "ItemsSource", ItemsSource.Skip(i * GetItemCountForPage(i,_surfaceRect)).Take(GetItemCountForPage(i, _surfaceRect)));//(ItemsSource.Count() > 0 ? ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE) : ItemsSource));
//                      builder.AddAttribute(i * lineCount + 5, "StartIndex", i * GetItemCountForPage(i, _surfaceRect));
//                  }
//                  else
//                  {
//                      builder.AddAttribute(i * lineCount + 4, "ItemsSource", ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE));//(ItemsSource.Count() > 0 ? ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE) : ItemsSource));
//                      builder.AddAttribute(i * lineCount + 5, "StartIndex", i * DEFAULT_ITEMS_PER_PAGE);
//                  }
//                  builder.AddAttribute(i * lineCount + 6, "PageMeasureSubject", pageMeasureSubject);
//                  builder.AddAttribute(i * lineCount + 7, "ItemClicked", EventCallback.Factory.Create<object>(this, OnItemClick));
//                  builder.AddAttribute(i * lineCount + 8, "SelectedItems", selectedItems);
//                  builder.AddAttribute(i * lineCount + 9, "ItemFocusable", SelectionMode != SelectionMode.None ? true : ItemFocusable);
//                  builder.AddComponentReferenceCapture(i * lineCount + 10, (comp) => renderedPages.Add((ListPage<TItem>)comp));
//                  builder.CloseComponent();
//              }
//          }

//          builder.OpenComponent(totalPages * lineCount, typeof(ListVirtualizationSpacer));
//          builder.AddAttribute(totalPages * lineCount + 1, "Height", averagePageHeight * (totalPages - endPage - 1));
//          builder.CloseComponent();
//      }
//      catch (Exception ex)
//      {
//          Debug.WriteLine(ex.ToString());
//      }

//  };

        protected Task OnItemClick(object item)
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

            ItemClicked.InvokeAsync(castItem);


            if (shouldRender == true)
                this.StateHasChanged();

            return Task.CompletedTask;
        }

       

        //private async Task MeasureContainerAsync()
        //{
        //    var rect = await this.JSRuntime.InvokeAsync<JSRect>("BlazorFabricList.measureElementRect", this.surfaceDiv);
        //    _surfaceRect = new Rectangle(rect.left, rect.width, rect.top, rect.height);
        //    _height = _surfaceRect.height;

        //    if (heightSub != null)
        //    {
        //        heightSub.Dispose();
        //        heightSub = null;
        //    }

        //    heightSub = pageMeasureSubject.Subscribe(x =>
        //    {
        //        if (isFirstRender && x.index == 0)
        //        {
        //            averagePageHeight = x.height;
        //            var aheadSpace = _surfaceRect.height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
        //            minRenderedPage = 0;
        //            if (averagePageHeight != 0)
        //                maxRenderedPage = (int)Math.Ceiling(aheadSpace / averagePageHeight);

        //            isFirstRender = false;
        //            shouldRender = true;
        //            ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
        //            this.StateHasChanged();
        //        }
        //        else if (!isFirstRender)
        //        {
        //            if (averagePageHeight != 0 && ((x.height - averagePageHeight) / averagePageHeight - 1 > thresholdChangePercent))
        //            {
        //                averagePageHeight = x.height;

        //                shouldRender = true;
        //                ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
        //                this.StateHasChanged();
        //            }
        //            else
        //            {
        //                averagePageHeight = (averagePageHeight + x.height) / 2;
        //            }
        //        }
        //    });
        //}

        

        //public async Task OnScroll(EventArgs args)
        //{
        //    try
        //    {
        //        var scrollRect = await this.JSRuntime.InvokeAsync<JSRect>("BlazorFabricList.measureScrollWindow", this.surfaceDiv);

        //        var rearSpace = _height * DEFAULT_RENDERED_WINDOWS_BEHIND;
        //        var aheadSpace = _height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
        //        var totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);
        //        var currentPage = (int)Math.Floor(scrollRect.top / averagePageHeight);


        //        var minPage = Math.Max(0, (int)Math.Ceiling((scrollRect.top - rearSpace) / averagePageHeight) - 1);

        //        var maxPage = Math.Min(Math.Max(totalPages - 1, 0), Math.Max((int)Math.Ceiling((scrollRect.top + aheadSpace) / averagePageHeight) - 1, 0));

        //        if (minRenderedPage != minPage || maxRenderedPage != maxPage)
        //        {
        //            minRenderedPage = minPage;
        //            maxRenderedPage = maxPage;
                                        
        //            shouldRender = true;
        //            Debug.WriteLine($"Scroll causing pages {minPage} to {maxPage} to rerender.");
        //            ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
        //            this.StateHasChanged();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }

        //}

        public async void Dispose()
        {            
            if (_itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
            {
                (_itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
            }

            _idleAsyncSubscription.Dispose();
            _asyncScrollSubject.Dispose();
            _scrollSubject.Dispose();
            _scrollingDoneSubject.Dispose();

            _listItemReferences.Clear();
            _pages = null;

            if (_listRegistration >= 0)
            {
                await JSRuntime.InvokeVoidAsync("BlazorFabricBasicList.unregister", _listRegistration);
                _listRegistration = -1;
            }
            if (_resizeRegistration != null)
            {
                await JSRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.deregisterResizeEvent", _resizeRegistration);
                _resizeRegistration = null;
            }
            Debug.WriteLine("List was disposed");
        }
    }
}
