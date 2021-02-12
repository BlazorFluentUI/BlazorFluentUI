using FluentUI.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class RibbonGroup: FluentUIComponentBase
    {
        [Parameter] public RenderFragment<IRibbonItem>? ItemTemplate { get; set; }

        [Parameter] public RenderFragment<IEnumerable<object>>? OverflowTemplate { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public ResizeGroupData ItemsSource { get; set; }
        [Parameter] public bool showDelimiter { get; set; }



        private Task<Rectangle>? boundsTask;
        private CancellationTokenSource boundsCTS = new CancellationTokenSource();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
            //    boundsTask = this.GetBoundsAsync(boundsCTS.Token);
            //    var bounds = await boundsTask;
            //    double newContainerDimension = bounds.width; 
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine("Task was cancelled in ResizeGroup");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        List<IRibbonItem> items = new List<IRibbonItem>();
        List<IRibbonItem> overflowItems = new List<IRibbonItem>();

        Func<ResizeGroupData, ResizeGroupData> onGrowData;
        Func<ResizeGroupData, ResizeGroupData> onReduceData;

        [Parameter] public Func<IEnumerable<object>, IEnumerable<object>> ItemTransform { get; set; }


     //   Func<ResizeGroupData, string> getCacheKey = data => data.CacheKey;

        protected override Task OnParametersSetAsync()
        {
            SetItemsAndOverflowItems();
            return base.OnParametersSetAsync();
        }
        
        protected override Task OnInitializedAsync()
        {
            
            ItemsSource.Changed += ItemsSource_Changed;
            showDelimiter = ItemsSource.ShowDelimiter;
            return Task.CompletedTask;
        }

        void SetItemsAndOverflowItems()
        {
            items.Clear();
            overflowItems.Clear();
            foreach (var item in ItemsSource.Items)
            {
                items.Add(item);
            }
            foreach (var item in ItemsSource.OverflowItems)
            {
                overflowItems.Add(item);
            }
        }

        private void ItemsSource_Changed(object sender, EventArgs e)
        {
            SetItemsAndOverflowItems();
            showDelimiter = ItemsSource.ShowDelimiter;
            StateHasChanged();
        }
    }
}
