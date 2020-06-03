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
using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorFluentUI
{
    public partial class BFUList<TItem> : BFUComponentBase, IDisposable, IHasPreloadableGlobalStyle
    {
        //protected bool firstRender = false;

        protected const int DEFAULT_ITEMS_PER_PAGE = 10;
        protected const int DEFAULT_RENDERED_WINDOWS_BEHIND = 2;
        protected const int DEFAULT_RENDERED_WINDOWS_AHEAD = 2;

        private double thresholdChangePercent = 0.10;

        //protected ElementReference rootDiv;
        protected ElementReference surfaceDiv;
        protected ElementReference contentDiv;
        protected ElementReference spacerBefore;
        protected ElementReference spacerAfter;

        private bool hasMeasuredAverageHeightOnce = false;


        long renderCount;
        //private double _averagePageHeight = 100;
        private bool isFirstRender = true;
        private bool _shouldRender = false;

        private int listId;
        private int numItemsToSkipBefore;
        private int numItemsToShow;
        private double averageHeight = 43;
        private int ItemsToSkipAfter => itemContainers.Count() - numItemsToSkipBefore - numItemsToShow;


        //private int minRenderedPage;
        //private int maxRenderedPage;
        private Rectangle _lastScrollRect = new Rectangle();
        private ElementMeasurements _scrollRect = new ElementMeasurements();
        //private double _scrollHeight;
        private Rectangle surfaceRect = new Rectangle();
        private double _height;
        public double CurrentHeight => _height;
        
        
        private bool _jsAvailable = false;
        private bool _lastIsVirtualizing = true;

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
        public bool IsVirtualizing { get; set; } = true;

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

        //[Parameter] public BFUSelection<TItem> Selection { get; set; }
        //[Parameter] public EventCallback<BFUSelection<TItem>> SelectionChanged { get; set; }
        //[Parameter] public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;
        //[Parameter]
        //public bool UseDefaultStyling { get; set; } = true;


        //[Parameter] public bool UseInternalScrolling { get; set; } = true;
        
       

        private IEnumerable<TItem> _itemsSource;

        protected List<ItemContainer<TItem>> itemContainers = new List<ItemContainer<TItem>>();


        private List<TItem> selectedItems = new List<TItem>();
        private string _resizeRegistration;

        private Dictionary<int, double> _pageSizes = new Dictionary<int, double>();
        private bool _needsRemeasure = true;

        private Viewport _viewport = new Viewport();
        private ElementMeasurements _surfaceRect = new ElementMeasurements();

        //private IDisposable _updatesSubscription;

        //private ICollection<Rule> ListRules { get; set; } = new System.Collections.Generic.List<Rule>();

        //protected override void BuildRenderTree(RenderTreeBuilder builder)
        //{
        //    builder.OpenComponent<BFUGlobalCS>(0);
        //    builder.AddAttribute(1, "Component", this);
        //    builder.AddAttribute(2, "CreateGlobalCss", new System.Func<ICollection<IRule>>(() => CreateGlobalCss(Theme)));
        //    builder.CloseComponent();
            
        //    // Render actual content
        //    builder.OpenElement(3, "div");
        //    builder.AddAttribute(4, "class", $"ms-List mediumFont {ClassName}");
        //    builder.AddAttribute(5, "role", "list");
        //    builder.AddAttribute(6, "style", $"{Style}overflow-y:hidden;height:100%;");
        //    builder.AddElementReferenceCapture(7, (element) => RootElementReference = element);

        //    builder.OpenElement(11, "div");
        //    builder.AddAttribute(12, "class", $"ms-List-surface");
        //    builder.AddAttribute(13, "role", "presentation");
        //    builder.AddAttribute(14, "style", $"overflow-y:auto;height:100%;");
        //    builder.AddElementReferenceCapture(15, (element) => surfaceDiv = element);

        //    builder.OpenElement(21, "div");
        //    var translateY = numItemsToSkipBefore * averageHeight;
        //    builder.AddAttribute(22, "style", $"transform: translateY({ translateY }px);");
        //    builder.AddAttribute(23, "data-translateY", translateY);
        //    builder.AddAttribute(24, "role", "presentation");
        //    builder.AddAttribute(25, "class", "ms-List-viewport");
        //    builder.AddElementReferenceCapture(26, (element) => contentDiv = element);

        //    builder.OpenRegion(27);
        //    int index = 0;
        //    foreach (var item in ItemsSource.Skip(numItemsToSkipBefore).Take(numItemsToShow))
        //    {
        //        index++;
        //        builder.OpenElement( (numItemsToSkipBefore *2 + index*2), "div");
        //        builder.AddAttribute( (numItemsToSkipBefore * 2 + index *2), "data-index", numItemsToSkipBefore + index);
        //        ItemTemplate(new ItemContainer<TItem>() {Index = numItemsToSkipBefore + index, Item=item })(builder);
        //        builder.CloseElement();
        //    }
        //    builder.CloseRegion();

        //    builder.CloseElement();

        //    // Also emit a spacer that causes the total vertical height to add up to Items.Count()*numItems
        //    builder.OpenElement(28, "div");
        //    var numHiddenItems = ItemsSource.Count() - numItemsToShow;
        //    builder.AddAttribute(29, "style", $"width: 1px; height: { numHiddenItems * averageHeight }px;");
        //    builder.CloseElement();

        //    builder.CloseElement();

        //    builder.CloseElement();

            

            
        //}

        protected RenderFragment<RenderFragment<ItemContainer<TItem>>> ItemContainer { get; set; }

        protected override Task OnInitializedAsync()
        {


            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {

            if (_itemsSource != ItemsSource)
            {
                if (this._itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
                {
                    (this._itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
                }

                _itemsSource = ItemsSource;
                if (_itemsSource != null)
                    itemContainers = _itemsSource.Select((x, i) => new ItemContainer<TItem> { Item = x, Index = i }).ToList();
                else
                    itemContainers.Clear();

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
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems != null)
                        {
                            foreach (var item in e.NewItems)
                            {
                                itemContainers.Add(new ItemContainer<TItem>() { Item = (TItem)item, Index = itemContainers.Count });
                            }
                        }
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        if (e.OldItems != null)
                        {
                            foreach (var item in e.OldItems)
                            {
                                var found = itemContainers.FirstOrDefault(x => x.Item.Equals((TItem)item));
                                if (found != null)
                                    itemContainers.Remove(found);
                            }
                        }
                        break;
                    }
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    {
                        //itemContainers.Clear();
                        itemContainers = _itemsSource.Select((x, i) => new ItemContainer<TItem> { Item = x, Index = i }).ToList();
                        break;
                    }
            }
            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var listRules = new HashSet<IRule>();
            //creates a method that pulls in focusstyles the way the react controls do it.
            var focusStyleProps = new FocusStyleProps(theme);
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
                          $"border-bottom:1px solid {theme.Palette.NeutralLighter};" +
                          $"display:inline-flex;"
                          +
                          mergeStyleResults.MergeRules
                }
            });
            listRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};" 
                }
            });
            listRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-List-cell-default.is-selected" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};"
                }
            });

            foreach (var rule in mergeStyleResults.AddRules)
                listRules.Add(rule);

            return listRules;
        }

       
        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {            
            if (firstRender)
            {
                _jsAvailable = true;
                if (IsVirtualizing)
                {
                    var objectRef = DotNetObjectReference.Create(this);
                    var initResult = await JSRuntime.InvokeAsync<DOMRect>("BlazorFluentUiList.initialize", objectRef, surfaceDiv, spacerBefore, spacerAfter);
                    this.listId = (int)initResult.Left;
                    await UpdateViewportAsync(initResult.Right, initResult.Width, initResult.Bottom, initResult.Height);
                }
                else
                {
                     var viewportMeasurement = await JSRuntime.InvokeAsync<DOMRect>("BlazorFluentUiList.getViewport", surfaceDiv);
                    await UpdateViewportAsync(viewportMeasurement.Right, viewportMeasurement.Width, viewportMeasurement.Bottom, viewportMeasurement.Height);
                }
            }
            else
            {
                if (_lastIsVirtualizing != IsVirtualizing)
                {
                    _lastIsVirtualizing = IsVirtualizing;  //need to make sure this area is run once, otherwise mulitple observers will be set for this viewport leading to blinking
                    if (IsVirtualizing)
                    {
                        var objectRef = DotNetObjectReference.Create(this);
                        var initResult = await JSRuntime.InvokeAsync<DOMRect>("BlazorFluentUiList.initialize", objectRef, surfaceDiv, spacerBefore, spacerAfter, true);
                        this.listId = (int)initResult.Left;
                        await UpdateViewportAsync(initResult.Right, initResult.Width, initResult.Bottom, initResult.Height);
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("BlazorFluentUiList.removeList", this.listId);
                    }
                }                
            }
            

            if (IsVirtualizing)//(!hasMeasuredAverageHeightOnce)
            {
                var averageHeight = await JSRuntime.InvokeAsync<int>("BlazorFluentUiList.getInitialAverageHeight", this.listId);
                if (averageHeight != 0 && averageHeight != this.averageHeight)
                {
                    hasMeasuredAverageHeightOnce = true;
                    this.averageHeight = averageHeight;
                    StateHasChanged();
                }

            }

            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public async void OnSpacerVisible(string spacerType, DOMRect visibleRect, double containerHeight, double spacerBeforeHeight, double spacerAfterHeight)
        {
            // Reset to match values corresponding to this event
            numItemsToSkipBefore = (int)Math.Round(spacerBeforeHeight / averageHeight);
            numItemsToShow = itemContainers.Count() - numItemsToSkipBefore - (int)Math.Round(spacerAfterHeight / averageHeight);

            if (spacerType == "before" && numItemsToSkipBefore > 0)
            {
                var visibleTop = visibleRect.Top;
                var firstVisibleItemIndex = (int)Math.Floor(visibleTop / averageHeight);
                numItemsToShow = (int)Math.Ceiling(containerHeight / averageHeight) + 1;
                numItemsToSkipBefore = Math.Max(0, firstVisibleItemIndex);
                StateHasChanged();
            }
            else if (spacerType == "after" && ItemsToSkipAfter > 0)
            {
                var visibleBottom = visibleRect.Top + visibleRect.Height;
                var lastVisibleItemIndex = numItemsToSkipBefore + numItemsToShow + (int)Math.Ceiling(visibleBottom / averageHeight);
                numItemsToShow = (int)Math.Ceiling(containerHeight / averageHeight) + 1;
                numItemsToSkipBefore = Math.Max(0, lastVisibleItemIndex - numItemsToShow);
                StateHasChanged();
            }

            await UpdateViewportAsync(visibleRect.Right, visibleRect.Width, visibleRect.Bottom, visibleRect.Height);
        }



        [JSInvokable]
        public async void ResizeHandler(double width, double height)
        {
            //await MeasureContainerAsync();

            _viewport.Height = _surfaceRect.cheight;
            _viewport.Width = _surfaceRect.cwidth;
            _viewport.ScrollHeight = _scrollRect.height;
            _viewport.ScrollWidth = _scrollRect.width;
            await OnViewportChanged.InvokeAsync(_viewport);
        }

        [JSInvokable]
        public async void OnScroll(ScrollEventArgs args)
        {            
            averageHeight = args.AverageHeight;
            // TODO: Support horizontal scrolling too
            var relativeTop = args.ContainerRect.Top - args.ContentRect.Top;
            numItemsToSkipBefore = Math.Max(0, (int)(relativeTop / averageHeight));

            var visibleHeight = args.ContainerRect.Bottom - (args.ContentRect.Top + numItemsToSkipBefore * averageHeight);
            numItemsToShow = (int)Math.Ceiling(visibleHeight / averageHeight) * 3;

            await UpdateViewportAsync(args.ScrollRect.width, args.ContainerRect.Width, args.ScrollRect.height, args.ContainerRect.Height);

            _lastScrollRect = args.ScrollRect;

            StateHasChanged();
        }

        private async Task UpdateViewportAsync(double scrollWidth, double width, double scrollHeight, double height)
        {
            bool hasChanged = false;
            if (_viewport.ScrollWidth != scrollWidth)
            {
                hasChanged = true;
                _viewport.ScrollWidth = scrollWidth;
            }
            if (_viewport.Width != width)
            {
                hasChanged = true;
                _viewport.Width = width;
            }
            if (_viewport.ScrollHeight != scrollHeight)
            {
                hasChanged = true;
                _viewport.ScrollHeight = scrollHeight;
            }
            if (_viewport.Height != height)
            {
                hasChanged = true;
                _viewport.Height = height;
            }

            if (hasChanged)
                await OnViewportChanged.InvokeAsync(_viewport);

        }


        public async void Dispose()
        {
            //if (OnListScrollerHeightChanged.HasDelegate)
            //    await OnListScrollerHeightChanged.InvokeAsync((0, Data));
            //_heightSub?.Dispose();
            //_scrollSubscription?.Dispose();

            //_updatesSubscription?.Dispose();
            if (_itemsSource is System.Collections.Specialized.INotifyCollectionChanged)
            {
                (_itemsSource as System.Collections.Specialized.INotifyCollectionChanged).CollectionChanged -= ListBase_CollectionChanged;
            }
            if (_resizeRegistration != null)
            {
                await JSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.deregisterResizeEvent", _resizeRegistration);
            }
            Debug.WriteLine("List was disposed");
        }

        public class ScrollEventArgs
        {
            public DOMRect ContainerRect { get; set; }
            public Rectangle ScrollRect { get; set; }
            public DOMRect ContentRect { get; set; }

            public double AverageHeight { get; set; }
        }

        public class DOMRect
        {
            public double Top { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
            public double Right { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
        }
    }
}
