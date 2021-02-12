using FluentUI.Models;
using FluentUI.Resize;
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


namespace FluentUI
{
    public partial class RibbonTab : ResizeComponentBase
    {
        [Inject] IJSRuntime jSRuntime { get; set; }
        [Parameter] public string? HeaderText { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public IEnumerable<IGroup>? ItemsSource { get; set; }
        [Parameter] public RenderFragment<ResizeGroupData>? ItemTemplate { get; set; }

        Collection<ResizeGroupData> ResizableGroups = new Collection<ResizeGroupData>();


        public RibbonTab()
        {
            
           OnGrowData = () =>
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
            };

            OnReduceData = () =>
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
