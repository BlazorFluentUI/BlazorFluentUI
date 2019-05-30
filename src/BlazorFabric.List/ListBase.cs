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

using Blazor.OfficeUiFabric.Components.List;
using Microsoft.JSInterop;

namespace BlazorFabric.List
{
    public class ListBase<TItem> : ComponentBase
    {
        protected bool firstRender = false;

        protected const int DEFAULT_ITEMS_PER_PAGE = 10;
        protected const int DEFAULT_RENDERED_WINDOWS_BEHIND = 2;
        protected const int DEFAULT_RENDERED_WINDOWS_AHEAD = 2;

        private double thresholdChangePercent = 0.10;
        //private int averageCount = 1;

        protected ElementRef scrollableDiv;
        protected ElementRef rootDiv;
        protected ElementRef surfaceDiv;

        private double averagePageHeight = 0;
        private bool isFirstRender = true;
        private bool shouldRender = false;

        private int minRenderedPage;
        private int maxRenderedPage;

        private double height;

        //private bool isScrollRegistered = false;
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        protected IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter]
        protected RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter]
        protected SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        private Func<TItem, UIMouseEventArgs, Task> ItemClicked { get; set; }
             
        protected RenderFragment ItemPagesRender { get; set; }

        private Subject<(int index, double height)> pageMeasureSubject = new Subject<(int index, double height)>();

        private IDisposable heightSub;
        private System.Collections.Generic.List<ListPage<TItem>> renderedPages = new System.Collections.Generic.List<ListPage<TItem>>();

        private System.Collections.Generic.List<TItem> selectedItems = new System.Collections.Generic.List<TItem>();

        protected override Task OnInitAsync()
        {
            ItemPagesRender = RenderPages(0, 0);
            return base.OnInitAsync();
        }

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                return true;
            }
            return false;
            //return base.ShouldRender();
        }

        //protected override void ApplyStyles()
        //{
        //    var theme = ThemingEngine.DefaultTheme.Value;

        //    var rootStyle = new Style
        //    {
        //        Overflow = Overflow.Auto,
        //        Height = "400px"
        //    };
        //    rootStyle.Selectors.Add(".is-active", new Style { BackgroundColor = theme.Palette.NeutralLight });

        //    var listItemStyles = new Style
        //    {
        //        Padding = 10,
        //        BoxSizing = BoxSizing.BorderBox,
        //        BorderBottom = $"1px solid {theme.SemanticColors.BodyDivider}",
        //        Display = "flex"
        //    };
        //    listItemStyles.Selectors.Add(":hover", new Style { BackgroundColor = theme.Palette.NeutralLighter });


        //    var listStyles = new ListStyles
        //    {
        //        Root = rootStyle,
        //        ListItem = listItemStyles
        //    };

        //    var mergedStyles = Blazor.Extensions.MergeStyles.StyleEngine.MergeStyleSets(listStyles, Styles);

        //    this.MergedStyles = mergedStyles;
        //}

        private RenderFragment RenderPages(int startPage, int endPage, double leadingPadding = 0) => builder =>
          {
              renderedPages.Clear();
              var totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);

              builder.OpenComponent(0, typeof(ListVirtualizationSpacer));
              builder.AddAttribute(1, "Height", averagePageHeight * startPage);
              builder.CloseComponent();

              const int lineCount = 8;
              for (var i = 0; i <= totalPages; i++)
              {

                  if (startPage <= i && endPage >= i)
                  {
                      builder.OpenComponent(i * lineCount + 2, typeof(ListPage<TItem>));
                      builder.AddAttribute(i * lineCount + 3, "ItemTemplate", ItemTemplate);
                      builder.AddAttribute(i * lineCount + 4, "ItemsSource", ItemsSource.Skip(i * DEFAULT_ITEMS_PER_PAGE).Take(DEFAULT_ITEMS_PER_PAGE));
                      builder.AddAttribute(i * lineCount + 5, "StartIndex", i * DEFAULT_ITEMS_PER_PAGE);
                      builder.AddAttribute(i * lineCount + 6, "PageMeasureSubject", pageMeasureSubject);
                      builder.AddAttribute(i * lineCount + 7, "ItemClicked", (Func<object, UIMouseEventArgs, Task>)OnItemClick);
                      builder.AddAttribute(i * lineCount + 8, "SelectedItems", selectedItems);
                      builder.AddComponentReferenceCapture(i * lineCount + 9, (comp) => renderedPages.Add((ListPage<TItem>)comp));
                      builder.CloseComponent();
                  }
              }

              builder.OpenComponent(totalPages * lineCount, typeof(ListVirtualizationSpacer));
              builder.AddAttribute(totalPages * lineCount + 1, "Height", averagePageHeight * (totalPages - endPage - 1));
              builder.CloseComponent();


          };

        protected Task OnItemClick(object item, UIMouseEventArgs e)
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

        protected override async Task OnAfterRenderAsync()
        {
            var scrollDivInfo = await this.JSRuntime.InvokeAsync<Dictionary<string, double>>("BlazorFabricList.measureElementRect", scrollableDiv);
            height = scrollDivInfo["height"];


            if (heightSub != null)
            {
                heightSub.Dispose();
                heightSub = null;
            }
            //this.heightSub = Observable.Zip(this.renderedPages.Select(x => x.Height)).SkipWhile(x => x.Contains(0)).Subscribe(x =>
            heightSub = pageMeasureSubject.Subscribe(x =>
            {
                if (isFirstRender && x.index == 0)
                {
                    averagePageHeight = x.height;
                    var aheadSpace = height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
                    minRenderedPage = 0;
                    maxRenderedPage = (int)Math.Ceiling(aheadSpace / averagePageHeight);

                    isFirstRender = false;
                    shouldRender = true;
                    ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                    this.StateHasChanged();
                }
                else if (!isFirstRender)
                {
                    if ((x.height - averagePageHeight) / averagePageHeight > thresholdChangePercent)
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

            await base.OnAfterRenderAsync();


        }


        public async Task OnScroll(UIEventArgs args)
        {
            var scrollRect = await this.JSRuntime.InvokeAsync<Dictionary<string, double>>("BlazorFabricList.measureScrollWindow", scrollableDiv);
            //Debug.WriteLine($"top: {scrollRect["top"]}");

            var rearSpace = height * DEFAULT_RENDERED_WINDOWS_BEHIND;
            var aheadSpace = height * (DEFAULT_RENDERED_WINDOWS_AHEAD + 1);
            var totalPages = (int)Math.Ceiling(ItemsSource.Count() / (double)DEFAULT_ITEMS_PER_PAGE);
            var currentPage = (int)Math.Floor(scrollRect["top"] / averagePageHeight);


            var minPage = Math.Max(0, (int)Math.Ceiling((scrollRect["top"] - rearSpace) / averagePageHeight) - 1);

            var maxPage = Math.Min(totalPages - 1, (int)Math.Ceiling((scrollRect["top"] + aheadSpace) / averagePageHeight) - 1);

            if (minRenderedPage != minPage || maxRenderedPage != maxPage)
            {
                minRenderedPage = minPage;
                maxRenderedPage = maxPage;

                //Debug.WriteLine($"MinPage: {this.minRenderedPage},  MaxPage: {this.maxRenderedPage}");
                //page that intersects or aligns with top

                shouldRender = true;
                ItemPagesRender = RenderPages(minRenderedPage, maxRenderedPage);
                this.StateHasChanged();
            }



        }
    }
}
