using FluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class GroupHeader : FluentUIComponentBase, IDisposable
    {
        
        bool isLoadingVisible;

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public int Count { get; set; }

        [Parameter]
        public bool CurrentlySelected { get; set; }

        [Parameter]
        public int GroupLevel { get; set; }

        [Parameter]
        public bool HasMoreData { get; set; }

        [Parameter]
        public bool IsOpen { get; set; }

        [Parameter]
        public Action<bool> OnOpenChanged { get; set; }

        [Parameter]
        public Func<object,bool> IsGroupLoading { get; set; }

        [Parameter]
        public bool IsSelectionCheckVisible { get; set; }

        //[Parameter]
        //public int ListIndex { get; set; }

        [Parameter]
        public string LoadingText { get; set; } = "Loading...";

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public Action OnClick { get; set; }

        [Parameter]
        public Action OnToggle { get; set; }

      
        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [CascadingParameter]
        private SelectionZone<object> SelectionZone { get; set; }

        protected bool isSelected { get; set; }

         protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }
       

        public void OnToggleOpen(MouseEventArgs mouseEventArgs)
        {
            OnOpenChanged(!IsOpen);
            //isLoadingVisible = !isCollapsed && IsGroupLoading != null; // && IsGroupLoading(group);
            
        }

        public void Dispose()
        {
            
        }
    }
}
