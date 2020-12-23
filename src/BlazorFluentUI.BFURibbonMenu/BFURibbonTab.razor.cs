using BlazorFluentUI.Models;
using BlazorFluentUI.Resize;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BlazorFluentUI
{
    public partial class BFURibbonTab : BFUResizeComponentBase<int>
    {
        [Inject] IJSRuntime jSRuntime { get; set; }
        [Parameter] public string? HeaderText { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public IEnumerable<IGroup>? ItemsSource { get; set; }
        [Parameter] public RenderFragment<ResizeGroupData>? ItemTemplate { get; set; }

        Collection<ResizeGroupData> ResizableGroups = new Collection<ResizeGroupData>();
     //   BFUResizeComponentBase<int> BFUResizeComponentBase { get; set; } = new BFUResizeComponentBase<int>();


        public BFURibbonTab()
        {
            
           OnGrowData = data =>
            {

                if (ResizableGroups.Count > 0)
                {
                  
                    var groupToGrow = ResizableGroups[0];
                    foreach (var resizableGroup in ResizableGroups)
                    {
                        if(groupToGrow.HighestPriorityInOverflowItems() <= resizableGroup.HighestPriorityInOverflowItems())
                        {
                            groupToGrow = resizableGroup;
                        }
      
                    }
                    if (groupToGrow.Grow())
                    {
                        return true;
                    }
                }

                return false;
                //if (ItemsSource.Count() == 0)
                //    return default;

                //var firstItem = overflowItems.First();
                //overflowItems.RemoveAt(0);
                //items.Add(firstItem);

                //return new ResizeGroupData<TItem>(items, overflowItems, ComputeCacheKey(items));
            };

            OnReduceData = data =>
            {
                if (ResizableGroups.Count > 0)
                {
                    var groupToShrink = ResizableGroups[0];
                    foreach (var resizableGroup in ResizableGroups)
                    {
                        if (groupToShrink.LowestPriorityInItems() >= resizableGroup.LowestPriorityInItems())
                        {
                            groupToShrink = resizableGroup;
                        }

                    }
                    if (groupToShrink.Shrink())
                    {
                        return true;
                    }

          
                }
                
                return false;
                //if (items.Count == 0)
                //    return default;

                //var lastItem = items.Last();
                //items.Remove(lastItem);
                //overflowItems.Insert(0, lastItem);

                //return new ResizeGroupData<TItem>(items, overflowItems, ComputeCacheKey(items));

            };
            GetCacheKey = data =>
            {
                return "A";
            };


        }

        protected override Task OnInitializedAsync()
        {
  
            if (ItemsSource != null)
            {
                foreach (var group in ItemsSource)
                {
                    ResizableGroups.Add(new ResizeGroupData(group.ItemsSource, group == ItemsSource.Last()));
                }
            }
            return base.OnInitializedAsync();
        }

    }
}
