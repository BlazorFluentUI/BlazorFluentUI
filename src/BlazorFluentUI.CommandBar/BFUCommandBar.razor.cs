﻿using BlazorFluentUI.BFUCommandBarInternal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUCommandBar : BFUComponentBase
    {
        [Parameter] public IEnumerable<IBFUCommandBarItem> Items { get; set; }
        [Parameter] public IEnumerable<IBFUCommandBarItem> OverflowItems { get; set; }
        [Parameter] public IEnumerable<IBFUCommandBarItem> FarItems { get; set; }

        [Parameter] public EventCallback<IBFUCommandBarItem> OnDataReduced { get; set; }
        [Parameter] public EventCallback<IBFUCommandBarItem> OnDataGrown { get; set; }

        [Parameter] public bool ShiftOnReduce { get; set; }

        [Parameter] public RenderFragment<IBFUCommandBarItem> ItemTemplate { get; set; }

        protected Func<BFUCommandBarData, BFUCommandBarData> onGrowData;
        protected Func<BFUCommandBarData, BFUCommandBarData> onReduceData;

        protected BFUCommandBarData _currentData;

        protected override Task OnInitializedAsync()
        {
            onReduceData = (data) =>
            {
                if (data.PrimaryItems.Count > 0)
                {
                    IBFUCommandBarItem movedItem = data.PrimaryItems[ShiftOnReduce ? 0 : data.PrimaryItems.Count() - 1];
                    movedItem.RenderedInOverflow = true;

                    data.OverflowItems.Insert(0, movedItem);
                    data.PrimaryItems.Remove(movedItem);

                    data.CacheKey = ComputeCacheKey(data);

                    OnDataReduced.InvokeAsync(movedItem);

                    return data;
                }
                else
                    return null;
            };

            onGrowData = (data) =>
            {
                if (data.OverflowItems.Count > data.MinimumOverflowItems)
                {
                    var movedItem = data.OverflowItems[0];
                    movedItem.RenderedInOverflow = false;
                    data.OverflowItems.Remove(movedItem);

                    if (ShiftOnReduce)
                        data.PrimaryItems.Insert(0, movedItem);
                    else
                        data.PrimaryItems.Add(movedItem);

                    data.CacheKey = ComputeCacheKey(data);

                    OnDataGrown.InvokeAsync(movedItem);

                    return data;
                }
                else
                    return null;
            };

            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            _currentData = new BFUCommandBarData()
            {
                PrimaryItems = new List<IBFUCommandBarItem>(Items != null ? Items : new List<IBFUCommandBarItem>()),
                OverflowItems = new List<IBFUCommandBarItem>(OverflowItems != null ? OverflowItems : new List<IBFUCommandBarItem>()),
                FarItems = new List<IBFUCommandBarItem>(FarItems != null ? FarItems : new List<IBFUCommandBarItem>()),
                MinimumOverflowItems = OverflowItems != null ? OverflowItems.Count() : 0,
                CacheKey = ""
            };

            return base.OnParametersSetAsync();
        }

        private string ComputeCacheKey(BFUCommandBarData data)
        {
            var primaryKey = data.PrimaryItems.Aggregate("", (acc, item) => acc + item.CacheKey);
            var farKey = data.FarItems.Aggregate("", (acc, item) => acc + item.CacheKey);
            var overflowKey = data.OverflowItems.Aggregate("", (acc, item) => acc + item.CacheKey);
            return string.Join(" ", primaryKey, farKey, overflowKey);
        }

        
    }
}
