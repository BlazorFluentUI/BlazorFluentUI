using BlazorFabric.CommandBarInternal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class CommandBar : FabricComponentBase
    {

        [Parameter] public IEnumerable<ICommandBarItem> Items { get; set; }
        [Parameter] public IEnumerable<ICommandBarItem> OverflowItems { get; set; }
        [Parameter] public IEnumerable<ICommandBarItem> FarItems { get; set; }

        [Parameter] public EventCallback<ICommandBarItem> OnDataReduced { get; set; }
        [Parameter] public EventCallback<ICommandBarItem> OnDataGrown { get; set; }

        [Parameter] public bool ShiftOnReduce { get; set; }

        protected Func<CommandBarData, CommandBarData> onGrowData;
        protected Func<CommandBarData, CommandBarData> onReduceData;

        protected CommandBarData _currentData;

        protected override Task OnInitializedAsync()
        {
            onReduceData = (data) =>
            {
                if (data.PrimaryItems.Count > 0)
                {
                    ICommandBarItem movedItem = data.PrimaryItems[ShiftOnReduce ? 0 : data.PrimaryItems.Count() - 1];
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
            _currentData = new CommandBarData()
            {
                PrimaryItems = new List<ICommandBarItem>(Items != null ? Items : new List<ICommandBarItem>()),
                OverflowItems = new List<ICommandBarItem>(OverflowItems != null ? OverflowItems : new List<ICommandBarItem>()),
                FarItems = new List<ICommandBarItem>(FarItems != null ? FarItems : new List<ICommandBarItem>()),
                MinimumOverflowItems = OverflowItems != null ? OverflowItems.Count() : 0,
                CacheKey = ""
            };

            return base.OnParametersSetAsync();
        }

        private string ComputeCacheKey(CommandBarData data)
        {
            var primaryKey = data.PrimaryItems.Aggregate("", (acc, item) => acc + item.CacheKey);
            var farKey = data.FarItems.Aggregate("", (acc, item) => acc + item.CacheKey);
            var overflowKey = data.OverflowItems.Aggregate("", (acc, item) => acc + item.CacheKey);
            return string.Join(" ", primaryKey, farKey, overflowKey);
        }

        private ICollection<Rule> CreateGlobalCss()
        {
            var commandBarRules = new HashSet<Rule>();
            commandBarRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-CommandBar" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                            $"background-color:{Theme.SemanticColors.BodyBackground};" +
                            $"padding:0 14px 0 24px;" +
                            $"height:44px;"
                }
            });
            commandBarRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-CommandBar-primarySet" },
                Properties = new CssString()
                {
                    Css = $"flex-grow:1;" +
                            $"display:flex;" +
                            $"align-items:stretch;"
                }
            });
            commandBarRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-CommandBar-secondarySet" },
                Properties = new CssString()
                {
                    Css = $"flex-shrink:0;" +
                            $"display:flex;" +
                            $"align-items:stretch;"
                }
            });

            return commandBarRules;
        }
    }
}
