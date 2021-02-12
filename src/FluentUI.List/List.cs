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

namespace FluentUI
{
    public class List<TItem> : FluentUIComponentBase, IAsyncDisposable
    {
        private IJSRuntime? _jsInterop;

        private ElementReference _spacerBefore;

        private ElementReference _spacerAfter;

        private int _itemsBefore;

        private int _visibleItemCapacity;

        private int _itemCount;

        private int _loadedItemsStartIndex;

        private int _lastRenderedItemCount;

        private int _lastRenderedPlaceholderCount;

        private float _itemSize;

        private IEnumerable<TItem>? _loadedItems;

        private CancellationTokenSource? _refreshCts;

        private Exception? _refreshException;

        private ItemsProviderDelegate<TItem> _itemsProvider = default!;

        private RenderFragment<IndexedItem<TItem>>? _itemTemplate;

        private RenderFragment<PlaceholderContext>? _placeholder;
        private DotNetObjectReference<List<TItem>> _selfReference;
        private int _listId = -1;

        private double _containerWidth;

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;
        //[Parameter]
        //public EventCallback<Viewport> OnViewportChanged { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Gets or sets the item template for the list.
        /// </summary>
        [Parameter]
        public RenderFragment<IndexedItem<TItem>>? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the item template for the list.
        /// </summary>
        [Parameter]
        public RenderFragment<IndexedItem<TItem>>? ItemTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template for items that have not yet been loaded in memory.
        /// </summary>
        [Parameter]
        public RenderFragment<PlaceholderContext>? Placeholder { get; set; }

        /// <summary>
        /// Gets the size of each item in pixels. Defaults to 48px.
        /// </summary>
        [Parameter]
        public float ItemSize { get; set; } = 48f;

        /// <summary>
        /// Gets the width of each item in pixels. Defaults to 48px.
        /// </summary>
        [Parameter]
        public float ItemWidth { get; set; } = 48f;

        /// <summary>
        /// Gets or sets the function providing items to the list.
        /// </summary>
        [Parameter]
        public ItemsProviderDelegate<TItem>? ItemsProvider { get; set; }

        /// <summary>
        /// Gets or sets the fixed item source.
        /// </summary>
        [Parameter]
        public ICollection<TItem>? ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets a value that determines how many additional items will be rendered
        /// before and after the visible region. This help to reduce the frequency of rendering
        /// during scrolling. However, higher values mean that more elements will be present
        /// in the page.
        /// </summary>
        [Parameter]
        public int OverscanCount { get; set; } = 3;

        /// <summary>
        /// For GroupedList and others, when using multiple lists, each new list needs to know how to continue the count from the previous one.
        /// </summary>
        [Parameter]
        public int StartIndex { get; set; } = 0;


        /// <summary>
        /// Experimental support for grid layout instead of list layout
        /// </summary>
        [Parameter]
        public bool UseGridFlexLayout { get; set; }

        /// <summary>
        /// Instructs the component to re-request data from its <see cref="ItemsProvider"/>.
        /// This is useful if external data may have changed. There is no need to call this
        /// when using <see cref="Items"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
        public async Task RefreshDataAsync()
        {
            // We don't auto-render after this operation because in the typical use case, the
            // host component calls this from one of its lifecycle methods, and will naturally
            // re-render afterwards anyway. It's not desirable to re-render twice.
            await RefreshDataCoreAsync(renderOnSuccess: false);
        }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (ItemSize <= 0)
            {
                throw new InvalidOperationException(
                    $"{GetType()} requires a positive value for parameter '{nameof(ItemSize)}'.");
            }

            if (_itemSize <= 0)
            {
                _itemSize = ItemSize;
            }

            if (ItemsProvider != null)
            {
                if (ItemsSource != null)
                {
                    throw new InvalidOperationException(
                        $"{GetType()} can only accept one item source from its parameters. " +
                        $"Do not supply both '{nameof(ItemsSource)}' and '{nameof(ItemsProvider)}'.");
                }

                _itemsProvider = ItemsProvider;
            }
            else if (ItemsSource != null)
            {
                _itemsProvider = DefaultItemsProvider;

                // When we have a fixed set of in-memory data, it doesn't cost anything to
                // re-query it on each cycle, so do that. This means the developer can add/remove
                // items in the collection and see the UI update without having to call RefreshDataAsync.
                var refreshTask = RefreshDataCoreAsync(renderOnSuccess: false);

                // We know it's synchronous and has its own error handling
                Debug.Assert(refreshTask.IsCompletedSuccessfully);
            }
            else
            {
                throw new InvalidOperationException(
                    $"{GetType()} requires either the '{nameof(ItemsSource)}' or '{nameof(ItemsProvider)}' parameters to be specified " +
                    $"and non-null.");
            }

            _itemTemplate = ItemTemplate ?? ChildContent;
            _placeholder = Placeholder ?? DefaultPlaceholder;
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _selfReference = DotNetObjectReference.Create(this);
                _listId = await JSRuntime.InvokeAsync<int>($"FluentUIList.initialize", _selfReference, _spacerBefore, _spacerAfter);
            }
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_refreshException != null)
            {
                var oldRefreshException = _refreshException;
                _refreshException = null;

                throw oldRefreshException;
            }
            builder.OpenElement(0, "div");

            if (UseGridFlexLayout)
                builder.AddAttribute(1, "style", "display:flex;flex-flow: row wrap;");

            builder.OpenElement(2, "div");
            builder.AddAttribute(3, "style", GetSpacerStyle(_itemsBefore));
            builder.AddElementReferenceCapture(4, elementReference => _spacerBefore = elementReference);
            builder.CloseElement();



            var lastItemIndex = Math.Min(_itemsBefore + _visibleItemCapacity, _itemCount);
            var renderIndex = _itemsBefore;
            var placeholdersBeforeCount = Math.Min(_loadedItemsStartIndex, lastItemIndex);

            builder.OpenRegion(5);

            // Render placeholders before the loaded items.
            for (; renderIndex < placeholdersBeforeCount; renderIndex++)
            {
                // This is a rare case where it's valid for the sequence number to be programmatically incremented.
                // This is only true because we know for certain that no other content will be alongside it.
                builder.AddContent(renderIndex, _placeholder, new PlaceholderContext(renderIndex, _itemSize));
            }

            builder.CloseRegion();

            _lastRenderedItemCount = 0;

            // Render the loaded items.
            if (_loadedItems != null && _itemTemplate != null)
            {
                var itemsToShow = _loadedItems
                    .Skip(_itemsBefore - _loadedItemsStartIndex)
                    .Take(lastItemIndex - _loadedItemsStartIndex);

                builder.OpenRegion(6);



                foreach (var item in itemsToShow)
                {
                    builder.OpenElement(11, "div");
                    builder.SetKey(item);
                    //builder.AddAttribute(8, "data-selection-index", _lastRenderedItemCount + _itemsBefore - _loadedItemsStartIndex + StartIndex);
                    _itemTemplate(new IndexedItem<TItem> {Item=item, Index = renderIndex + _lastRenderedItemCount + StartIndex })(builder);
                    _lastRenderedItemCount++;

                    builder.CloseElement();
                }

                renderIndex += _lastRenderedItemCount;

                builder.CloseRegion();
            }

            _lastRenderedPlaceholderCount = Math.Max(0, lastItemIndex - _itemsBefore - _lastRenderedItemCount);

            builder.OpenRegion(12);

            // Render the placeholders after the loaded items.
            for (; renderIndex < lastItemIndex; renderIndex++)
            {
                builder.AddContent(renderIndex, _placeholder, new PlaceholderContext(renderIndex, _itemSize));
            }

            builder.CloseRegion();

            var itemsAfter = Math.Max(0, _itemCount - _visibleItemCapacity - _itemsBefore);

            builder.OpenElement(13, "div");
            builder.AddAttribute(14, "style", GetSpacerStyle(itemsAfter));
            builder.AddElementReferenceCapture(15, elementReference => _spacerAfter = elementReference);

            builder.CloseElement();

            //builder.AddElementReferenceCapture(13, elemRef => RootElementReference = elemRef);
            builder.CloseElement();

        }

        private string GetSpacerStyle(int itemsInSpacer)
            => $"height: {itemsInSpacer * _itemSize}px;{(UseGridFlexLayout ? "width:100%;" : "")}";

        [JSInvokable]
        public void ResizeHandler(double width)
        {
            _containerWidth = width;
        }

        [JSInvokable]
        public void OnBeforeSpacerVisible(float spacerSize, float spacerSeparation, float containerSize)
        {
            if (IsVirtualizing)
            {
                CalcualteItemDistribution(spacerSize, spacerSeparation, containerSize, out var itemsBefore, out var visibleItemCapacity);

                UpdateItemDistribution(itemsBefore, visibleItemCapacity);
            }
            else
            {
                if (_itemsBefore != 0 || _visibleItemCapacity != ItemsSource.Count())
                {
                    UpdateItemDistribution(0, ItemsSource.Count());
                    InvokeAsync(StateHasChanged);
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        [JSInvokable]
        public void OnAfterSpacerVisible(float spacerSize, float spacerSeparation, float containerSize)
        {
            if (IsVirtualizing)
            {
                CalcualteItemDistribution(spacerSize, spacerSeparation, containerSize, out var itemsAfter, out var visibleItemCapacity);

                var itemsBefore = Math.Max(0, _itemCount - itemsAfter - visibleItemCapacity);

                if (UseGridFlexLayout)
                {
                    //itemsBefore needs to be a multiple of numberPerRow
                    var numberPerRow = (int)Math.Floor(_containerWidth / ItemWidth);
                    itemsBefore = (int)Math.Ceiling((double)itemsBefore / numberPerRow) * numberPerRow;
                }

                UpdateItemDistribution(itemsBefore, visibleItemCapacity);
            }
            else
            {
                if (_itemsBefore != 0 || _visibleItemCapacity != ItemsSource.Count())
                {
                    UpdateItemDistribution(0, ItemsSource.Count());
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        private void CalcualteItemDistribution(
            float spacerSize,
            float spacerSeparation,
            float containerSize,
            out int itemsInSpacer,
            out int visibleItemCapacity)
        {
            if (_lastRenderedItemCount > 0)
            {
                _itemSize = (spacerSeparation - (_lastRenderedPlaceholderCount * _itemSize)) / _lastRenderedItemCount;
            }

            if (_itemSize <= 0)
            {
                // At this point, something unusual has occurred, likely due to misuse of this component.
                // Reset the calculated item size to the user-provided item size.
                _itemSize = ItemSize;
            }

            itemsInSpacer = Math.Max(0, (int)Math.Floor(spacerSize / _itemSize) - OverscanCount);
            visibleItemCapacity = (int)Math.Ceiling(containerSize / _itemSize) + 2 * OverscanCount;

            if (UseGridFlexLayout)
            {
                var numberPerRow = (int)Math.Floor(_containerWidth / ItemWidth);
                visibleItemCapacity = visibleItemCapacity * numberPerRow;
            }
        }

        private void UpdateItemDistribution(int itemsBefore, int visibleItemCapacity)
        {
            if (itemsBefore != _itemsBefore || visibleItemCapacity != _visibleItemCapacity)
            {
                _itemsBefore = itemsBefore;
                _visibleItemCapacity = visibleItemCapacity;
                var refreshTask = RefreshDataCoreAsync(renderOnSuccess: true);

                if (!refreshTask.IsCompleted)
                {
                    StateHasChanged();
                }
            }
        }

        private async ValueTask RefreshDataCoreAsync(bool renderOnSuccess)
        {
            _refreshCts?.Cancel();
            CancellationToken cancellationToken;

            if (_itemsProvider == DefaultItemsProvider)
            {
                // If we're using the DefaultItemsProvider (because the developer supplied a fixed
                // Items collection) we know it will complete synchronously, and there's no point
                // instantiating a new CancellationTokenSource
                _refreshCts = null;
                cancellationToken = CancellationToken.None;
            }
            else
            {
                _refreshCts = new CancellationTokenSource();
                cancellationToken = _refreshCts.Token;
            }

            var request = new ItemsProviderRequest(_itemsBefore, _visibleItemCapacity, cancellationToken);

            try
            {
                var result = await _itemsProvider(request);

                // Only apply result if the task was not canceled.
                if (!cancellationToken.IsCancellationRequested)
                {
                    _itemCount = result.TotalItemCount;
                    _loadedItems = result.Items;
                    _loadedItemsStartIndex = request.StartIndex;

                    if (renderOnSuccess)
                    {
                        StateHasChanged();
                    }
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException oce && oce.CancellationToken == cancellationToken)
                {
                    // No-op; we canceled the operation, so it's fine to suppress this exception.
                }
                else
                {
                    // Cache this exception so the renderer can throw it.
                    _refreshException = e;

                    // Re-render the component to throw the exception.
                    StateHasChanged();
                }
            }
        }

        private ValueTask<ItemsProviderResult<TItem>> DefaultItemsProvider(ItemsProviderRequest request)
        {
            return ValueTask.FromResult(new ItemsProviderResult<TItem>(
                ItemsSource!.Skip(request.StartIndex).Take(request.Count),
                ItemsSource!.Count));
        }

        private RenderFragment DefaultPlaceholder(PlaceholderContext context) => (builder) =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", $"height: {_itemSize}px;");
            builder.CloseElement();
        };

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            _refreshCts?.Cancel();

            if (_selfReference != null)
            {
                if (_listId != -1)
                {
                    await JSRuntime.InvokeVoidAsync("FluentUIList.removeList", _listId);
                }
                _selfReference.Dispose();
            }
        }
    }
}
