using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class GroupHeader : FabricComponentBase
    {
        
        bool isLoadingVisible;

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public int Count { get; set; }

        [Parameter]
        public bool CurrentlySelected { get; set; }

        [Parameter]
        public bool HasMoreData { get; set; }

        [Parameter]
        public bool IsCollapsed { get; set; }

        [Parameter]
        public EventCallback<bool> IsCollapsedChanged { get; set; }

        [Parameter]
        public Func<object,bool> IsGroupLoading { get; set; }

        [Parameter]
        public bool IsSelectionCheckVisible { get; set; }

        [Parameter]
        public string LoadingText { get; set; } = "Loading...";

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        public void OnToggleSelectGroupClick(MouseEventArgs mouseEventArgs)
        {

        }

        public void OnToggleCollapse(MouseEventArgs mouseEventArgs)
        {
            IsCollapsedChanged.InvokeAsync(!IsCollapsed);
            //isLoadingVisible = !isCollapsed && IsGroupLoading != null; // && IsGroupLoading(group);
            
        }
    }
}
