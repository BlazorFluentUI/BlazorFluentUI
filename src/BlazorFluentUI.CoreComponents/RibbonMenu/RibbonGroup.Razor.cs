using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using BlazorFluentUI.Models;

using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    public partial class RibbonGroup : FluentUIComponentBase
    {
        [Parameter] public RenderFragment<IRibbonItem>? ItemTemplate { get; set; }

        [Parameter] public RenderFragment<IEnumerable<object>>? OverflowTemplate { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public ResizeGroupData? ItemsSource { get; set; }
        [Parameter] public bool ShowDelimiter { get; set; }



        //private Task<Rectangle>? boundsTask;
        private readonly CancellationTokenSource boundsCTS = new();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                //    boundsTask = this.GetBoundsAsync(boundsCTS.Token);
                //    var bounds = await boundsTask;
                //    double newContainerDimension = bounds.width;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task was cancelled in ResizeGroup");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        readonly List<IRibbonItem> items = new();
        readonly List<IRibbonItem> overflowItems = new();

        //Func<ResizeGroupData, ResizeGroupData?>? onGrowData;
        //Func<ResizeGroupData, ResizeGroupData?>? onReduceData;

        [Parameter] public Func<IEnumerable<object>, IEnumerable<object>>? ItemTransform { get; set; }


        //   Func<ResizeGroupData, string> getCacheKey = data => data.CacheKey;

        protected override Task OnParametersSetAsync()
        {
            SetItemsAndOverflowItems();
            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {
            if (ItemsSource != null)
            {
                ItemsSource.Changed += ItemsSource_Changed;
                ShowDelimiter = ItemsSource.ShowDelimiter;
            }
            return Task.CompletedTask;
        }

        void SetItemsAndOverflowItems()
        {
            items.Clear();
            overflowItems.Clear();

            if (ItemsSource != null)
            {
                foreach (IRibbonItem? item in ItemsSource.Items)
                {
                    items.Add(item);
                }
                foreach (IRibbonItem? item in ItemsSource.OverflowItems)
                {
                    overflowItems.Add(item);
                }
            }
        }

        private void ItemsSource_Changed(object? sender, EventArgs e)
        {
            if (ItemsSource != null)
            {
                SetItemsAndOverflowItems();
                ShowDelimiter = ItemsSource.ShowDelimiter;
                StateHasChanged();
            }
        }
    }
}
