using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Reactive;
using BlazorFabric.Style;

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

        private double _averagePageHeight = 100;
        private bool isFirstRender = true;
        private bool _shouldRender = false;

        private int minRenderedPage;
        private int maxRenderedPage;
        private ElementMeasurements _lastScrollRect = new ElementMeasurements();
        private ElementMeasurements _scrollRect = new ElementMeasurements();
        //private double _scrollHeight;
        private Rectangle surfaceRect = new Rectangle();
        private double _height;
        public double CurrentHeight => _height;
        private bool _jsAvailable = false;

        //private object _lastVersion = null;

        [Inject] 
        private IJSRuntime JSRuntime { get; set; }

        [Parameter] 
        public object Data { get; set; }

        [Parameter] 
        public Func<int, Rectangle, int> GetItemCountForPage { get; set; }

        //[Parameter] 
        //public EventCallback<ItemContainer<TItem>> ItemClicked { get; set; }

        [Parameter] 
        public bool ItemFocusable { get; set; } = false;

        [Parameter] 
        public IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter] 
        public RenderFragment<ItemContainer<TItem>> ItemTemplate { get; set; }

        [Parameter] 
        public EventCallback<(double, object)> OnListScrollerHeightChanged { get; set; }

        [Parameter]
        public EventCallback<Viewport> OnViewportChanged { get; set; }

        //[Parameter] public Selection<TItem> Selection { get; set; }
        //[Parameter] public EventCallback<Selection<TItem>> SelectionChanged { get; set; }
        //[Parameter] public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;
        //[Parameter]
        //public bool UseDefaultStyling { get; set; } = true;


        //[Parameter] public bool UseInternalScrolling { get; set; } = true;
        
       

        private IEnumerable<TItem> _itemsSource;

        protected RenderFragment ItemPagesRender { get; set; }

        private ISubject<(int index, double height)> pageMeasureSubject = new Subject<(int index, double height)>();
        private IDisposable _heightSub;

        private ISubject<Unit> _scrollSubject = new Subject<Unit>();
        private IDisposable _scrollSubscription;

        private ISubject<Unit> _scrollDoneSubject = new Subject<Unit>();
        private IDisposable _scrollDoneSubscription;

        private System.Collections.Generic.List<ListPage<TItem>> renderedPages = new System.Collections.Generic.List<ListPage<TItem>>();

        private System.Collections.Generic.List<TItem> selectedItems = new System.Collections.Generic.List<TItem>();
        private string _resizeRegistration;

        private Dictionary<int, double> _pageSizes = new Dictionary<int, double>();
        private bool _needsRemeasure = true;

        private Viewport _viewport = new Viewport();
        private ElementMeasurements _surfaceRect = new ElementMeasurements();

        //private IDisposable _updatesSubscription;

        //private ICollection<Rule> ListRules { get; set; } = new System.Collections.Generic.List<Rule>();

        protected override Task OnInitializedAsync()
        {
            _heightSub = pageMeasureSubject.Subscribe(x =>
            {
                if (isFirstRender && x.index == 0)
                {
                    _averagePageHeight = x.height;

                    _pageSizes.Add(x.index, x.height);
                    var aheadSpace = surfaceRect.height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
                    minRenderedPage = 0;
                    if (_averagePageHeight != 0)
                        maxRenderedPage = (int)Math.Ceiling(aheadSpace / _averagePageHeight);

                    isFirstRender = false;
                    _shouldRender = true;
                    ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                    this.StateHasChanged();
                }
                else if (!isFirstRender)
                {
                    if (_pageSizes.ContainsKey(x.index))
                    {
                        if (_pageSizes[x.index] != x.height)
                        {
                            _pageSizes[x.index] = x.height;
                            _shouldRender = true;
                        }
                    }
                    else
                    {
                        _pageSizes.Add(x.index, x.height);
                    }

                    if (_pageSizes.Count > 1 && ((_pageSizes.Take(_pageSizes.Count - 1).Select(x => x.Value).Average() - _averagePageHeight) / _averagePageHeight > thresholdChangePercent))
                    {
                        _averagePageHeight = _pageSizes.Take(_pageSizes.Count - 1).Select(x => x.Value).Average();
                        _shouldRender = true;
                    }
                    else
                    {
                        double averagePageHeight = 0;
                        if (_pageSizes.Count > 1)
                            averagePageHeight = _pageSizes.Take(_pageSizes.Count - 1).Select(x => x.Value).Average();
                        else if (_pageSizes.Count == 1)
                            averagePageHeight = _pageSizes.Select(x => x.Value).First();
                        else
                            averagePageHeight = 0;

                        if (averagePageHeight != _averagePageHeight)
                        {
                            _averagePageHeight = averagePageHeight;
                            _shouldRender = true;
                        }
                    }

                    if (_shouldRender)
                    {
                        ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                        InvokeAsync(StateHasChanged);
                    }

                }
            });

            _scrollDoneSubscription = _scrollDoneSubject.Throttle(TimeSpan.FromMilliseconds(200))
                .Do(async _ =>
                {
                    await InvokeAsync(async () =>
                    {
                        Debug.WriteLine($"Scrolling done.");
                        _viewport.IsScrolling = false;
                        _viewport.ScrollDirection = (ScrollDirection.None, ScrollDirection.None);
                        await OnViewportChanged.InvokeAsync(_viewport);
                    });
                })
                .Subscribe();

            _scrollSubscription = _scrollSubject.SampleFirst(TimeSpan.FromMilliseconds(100))
                   .Do(async _ =>
                   {
                       await InvokeAsync(async () =>
                       {
                           try
                           {
                               _viewport.IsScrolling = true;
                               _lastScrollRect = _scrollRect;
                               _scrollRect = await this.JSRuntime.InvokeAsync<ElementMeasurements>("BlazorFabricList.measureScrollWindow", this.surfaceDiv);

                               var xDistance = _scrollRect.left - _lastScrollRect.left;
                               var yDistance = _scrollRect.top - _lastScrollRect.top;
                               _viewport.ScrollDirection = (
                                    xDistance > 0 ? ScrollDirection.Forward : (xDistance < 0 ? ScrollDirection.Backward : ScrollDirection.None),
                                    yDistance > 0 ? ScrollDirection.Forward : (yDistance < 0 ? ScrollDirection.Backward : ScrollDirection.None)
                               );
                               _viewport.ScrollDistance = (_scrollRect.left, _scrollRect.top);
                               _viewport.Height = _surfaceRect.cheight;
                               _viewport.Width = _surfaceRect.cwidth;
                               _viewport.ScrollHeight = _scrollRect.height;
                               _viewport.ScrollWidth = _scrollRect.width;

                               var rearSpace = _height * DEFAULT_RENDERED_WINDOWS_BEHIND;
                               var aheadSpace = _height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
                               int totalPages = 0;
                               if (GetItemCountForPage != null)
                                   totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)GetItemCountForPage(0, null));
                               else
                                   totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);
                               var currentPage = (int)Math.Floor(_scrollRect.top / _averagePageHeight);

                               var minPage = Math.Max(0, (int)Math.Ceiling((_scrollRect.top - rearSpace) / _averagePageHeight) - 1);

                               var maxPage = Math.Min(Math.Max(totalPages - 1, 0), Math.Max((int)Math.Ceiling((_scrollRect.top + aheadSpace) / _averagePageHeight) - 1, 0));

                               if (minRenderedPage != minPage || maxRenderedPage != maxPage)
                               {
                                   minRenderedPage = minPage;
                                   maxRenderedPage = maxPage;

                                   _shouldRender = true;
                                   Debug.WriteLine($"Scroll causing pages {minPage} to {maxPage} to rerender.");
                                   ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                                   StateHasChanged();
                               }
                               await OnViewportChanged.InvokeAsync(_viewport);
                           }
                           catch (Exception ex)
                           {
                               Debug.WriteLine(ex.ToString());
                           }
                       });
                   })
                   .Subscribe();

            //if (!CStyle.ComponentStyleExist(this))
            //{
            //    CreateCss();
            //}

            return base.OnInitializedAsync();
        }

        //protected override void OnThemeChanged()
        //{
        //    CreateCss();
        //    base.OnThemeChanged();
        //}

        protected override async Task OnParametersSetAsync()
        {
            //if (Selection != null && Selection.SelectedItems != selectedItems)
            //{
            //    selectedItems = new System.Collections.Generic.List<TItem>(Selection.SelectedItems);
            //    _shouldRender = true;
            //}

            //if (SelectionMode == SelectionMode.Single && selectedItems.Count() > 1)
            //{
            //    selectedItems.Clear();
            //    _shouldRender = true;
            //    await SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            //}
            //else if (SelectionMode == SelectionMode.None && selectedItems.Count() > 0)
            //{
            //    selectedItems.Clear();
            //    _shouldRender = true;
            //    await SelectionChanged.InvokeAsync(new Selection<TItem>(selectedItems));
            //}

            if (_itemsSource != ItemsSource)
            {
                if (this._itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    (this._itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
                }

                _itemsSource = ItemsSource;

                if (this.ItemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    (this.ItemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged += ListBase_CollectionChanged;
                }
                
                _shouldRender = true;
                _needsRemeasure = true;
            }

            //CreateCss();
            await base.OnParametersSetAsync();
        }


        private void ListBase_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        //protected override bool ShouldRender()
        //{

        //    if (_shouldRender)
        //    {
        //        //Debug.WriteLine("LIST RERENDERING");
        //        _shouldRender = false;
        //        return true;
        //    }
        //    //Debug.WriteLine("list wants to rerender... but can't");
        //    return false;
        //}
        private ICollection<Rule> CreateGlobalCss()
        {
            var listRules = new HashSet<Rule>();
            //creates a method that pulls in focusstyles the way the react controls do it.
            var focusStyleProps = new FocusStyleProps(this.Theme);
            var mergeStyleResults = FocusStyle.GetFocusStyle(focusStyleProps, ".ms-List-cell-default");

            listRules.Clear();
            // Cell only
            listRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default" },
                Properties = new CssString()
                {
                    Css = $"padding-top:11px;" +
                          $"padding-bottom:11px;" +
                          $"min-height:42px;" +
                          $"min-width:100%;" +
                          $"overflow:hidden;" +
                          $"box-sizing:border-box;" +
                          $"border-bottom:1px solid {Theme.Palette.NeutralLighter};" +
                          $"display:inline-flex;"
                          +
                          mergeStyleResults.MergeRules
                          //$"outline:transparent;" +
                          //$"position:relative;"
                }
            });
            listRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralLighter};" 
                }
            });
            listRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default.is-selected" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralLight};"
                }
            });

            foreach (var rule in mergeStyleResults.AddRules)
                listRules.Add(rule);
            //ListRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-List-cell::-moz-focus-inner" },
            //    Properties = new CssString()
            //    {
            //        Css = $"border:0;"
            //    }
            //});
            //ListRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-List-cell:focus::after" },
            //    Properties = new CssString()
            //    {
            //        Css = $"content:'';" +
            //              $"position:absolute;" +
            //              $"left:1px;" +
            //              $"top:1px;" +
            //              $"bottom:1px;" +
            //              $"right:1px;" +
            //              $"border:1px solid transparent;" +
            //              $"outline:1px solid {Theme.Palette.NeutralSecondary};" +
            //              $"z-index:var(--zindex-FocusStyle);"
            //    }
            //});
            //ListRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
            //    Properties = new CssString()
            //    {
            //        Css = ".ms-Fabric--isFocusVisible .ms-List-cell:focus::after {" +
            //              $"left:-2px;" +
            //              $"top:-2px;" +
            //              $"bottom:-2px;" +
            //              $"right:-2px;" +
            //              $"border:none;" +
            //              $"outline-color:ButtonText;" +
            //              "}"
            //    }
            //});
            return listRules;
        }

        public void ForceUpdate()
        {

            MeasureContainerAsync();
        }

        private RenderFragment RenderPages(int startPage, int endPage, double leadingPadding = 0) => builder =>
          {
              try
              {
                  renderedPages.Clear();
                  //int totalPages = 0;
                  //if (GetItemCountForPage != null)
                  //  totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)GetItemCountForPage();
                  //else


                  builder.OpenComponent(0, typeof(ListVirtualizationSpacer));
                  builder.AddAttribute(1, "Height", _averagePageHeight * startPage);
                  builder.CloseComponent();

                  const int lineCount = 11;
                  var totalItemsRendered = 0;
                  for (var i = startPage; i <= endPage; i++)
                  {
                      //Debug.WriteLine($"Drawing page {i}");
                      //if (startPage <= i && endPage >= i)
                      //{
                      builder.OpenComponent(i * lineCount + 2, typeof(ListPage<TItem>));
                      builder.AddAttribute(i * lineCount + 3, "ItemTemplate", ItemTemplate);
                      if (GetItemCountForPage != null)
                      {
                          totalItemsRendered += GetItemCountForPage(i, surfaceRect);
                          builder.AddAttribute(i * lineCount + 4, "ItemsSource", ItemsSource.Skip(i * GetItemCountForPage(i, surfaceRect)).Take(GetItemCountForPage(i, surfaceRect)));//(ItemsSource.Count() > 0 ? ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE) : ItemsSource));
                          builder.AddAttribute(i * lineCount + 5, "StartIndex", i * GetItemCountForPage(i, surfaceRect));
                      }
                      else
                      {
                          totalItemsRendered += DEFAULT_ITEMS_PER_PAGE;
                          builder.AddAttribute(i * lineCount + 4, "ItemsSource", ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE));//(ItemsSource.Count() > 0 ? ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE) : ItemsSource));
                          builder.AddAttribute(i * lineCount + 5, "StartIndex", i * DEFAULT_ITEMS_PER_PAGE);
                      }
                      builder.AddAttribute(i * lineCount + 6, "PageMeasureSubject", pageMeasureSubject);
                      //builder.AddAttribute(i * lineCount + 7, "ItemClicked", EventCallback.Factory.Create<ItemContainer<TItem>>(this, OnItemClick));
                      //builder.AddAttribute(i * lineCount + 8, "SelectedItems", selectedItems);
                      //builder.AddAttribute(i * lineCount + 9, "ItemFocusable", SelectionMode != SelectionMode.None ? true : ItemFocusable);
                      //builder.AddAttribute(i * lineCount + 10, "UseDefaultStyling", UseDefaultStyling);
                      builder.AddComponentReferenceCapture(i * lineCount + 11, (comp) => renderedPages.Add((ListPage<TItem>)comp));
                      builder.CloseComponent();
                      //}
                  }

                  int totalPages = 0;
                  if (GetItemCountForPage != null)
                      totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)GetItemCountForPage(0, null));
                  else
                      totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);
                  builder.OpenComponent(totalPages * lineCount, typeof(ListVirtualizationSpacer));
                  builder.AddAttribute(totalPages * lineCount + 1, "Height", _averagePageHeight * (totalPages - endPage - 1));
                  builder.CloseComponent();
              }
              catch (Exception ex)
              {
                  Debug.WriteLine(ex.ToString());
              }

          };

        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _resizeRegistration = await JSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerResizeEvent", DotNetObjectReference.Create(this), "ResizeHandler");


                await MeasureContainerAsync();
                Debug.WriteLine($"List after render, after measurecontainerasync: {_scrollRect.width}");

                

                //if (_needsRemeasure)
                //{
                //    //_shouldRender = true; //probably because the component rendered before all of the registrations could be made, need to force a re-render
                //    await MeasureContainerAsync();
                //}
                ItemPagesRender = RenderPages(0, 0);


                _shouldRender = true;
                StateHasChanged();
            }

            //if (_needsRemeasure)
            //{
            //    _needsRemeasure = false;

            //}
            if (_viewport.Height != _surfaceRect.cheight
                && _viewport.Width != _surfaceRect.cwidth
                && _viewport.ScrollHeight != _scrollRect.height
                && _viewport.ScrollWidth != _scrollRect.width)
            {
                _viewport.Height = _surfaceRect.cheight;
                _viewport.Width = _surfaceRect.cwidth;
                _viewport.ScrollHeight = _scrollRect.height;
                _viewport.ScrollWidth = _scrollRect.width;
                //_viewport.ScrollDistance = (0, 0);
                //_viewport.ScrollDirection = (ScrollDirection.None, ScrollDirection.None);
                //_viewport.IsScrolling = false;
                await OnViewportChanged.InvokeAsync(_viewport);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task MeasureContainerAsync()
        {
            _surfaceRect = await this.JSRuntime.InvokeAsync<ElementMeasurements>("BlazorFabricList.measureElementRect", this.surfaceDiv);
            //var oldScrollHeight = _scrollHeight;
            _lastScrollRect = _scrollRect;
            _scrollRect = await this.JSRuntime.InvokeAsync<ElementMeasurements>("BlazorFabricList.measureScrollWindow", this.surfaceDiv);

            //_scrollHeight = await JSRuntime.InvokeAsync<double>("BlazorFabricBaseComponent.getScrollHeight", this.surfaceDiv);
            surfaceRect = new Rectangle(_surfaceRect.left, _surfaceRect.width, _surfaceRect.top, _surfaceRect.height);

            if (_height != surfaceRect.height)
            {
                _height = surfaceRect.height;
                _shouldRender = true;
                StateHasChanged();
            }

            
            if (_lastScrollRect.height != _scrollRect.height)
                await OnListScrollerHeightChanged.InvokeAsync((_scrollRect.height, Data));
        }

        private async void HandleListScrollerHeightChanged(object sender, double height)
        {
            Debug.WriteLine($"Height changed: {height}");
            await MeasureContainerAsync();
            
        }

        [JSInvokable]
        public async void ResizeHandler(double width, double height)
        {
            await MeasureContainerAsync();

            _viewport.Height = _surfaceRect.cheight;
            _viewport.Width = _surfaceRect.cwidth;
            _viewport.ScrollHeight = _scrollRect.height;
            _viewport.ScrollWidth = _scrollRect.width;
            await OnViewportChanged.InvokeAsync(_viewport);
        }


        public void OnScroll(EventArgs args)
        {
            _scrollSubject.OnNext(Unit.Default);
            _scrollDoneSubject.OnNext(Unit.Default);
        }

        public async void Dispose()
        {
            //if (OnListScrollerHeightChanged.HasDelegate)
            //    await OnListScrollerHeightChanged.InvokeAsync((0, Data));
            _heightSub?.Dispose();
            _scrollSubscription?.Dispose();

            //_updatesSubscription?.Dispose();
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
