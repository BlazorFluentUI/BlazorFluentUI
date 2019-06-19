using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Nav
{
    public class NavFabricLinkBase : FabricComponentBase
    { 
        [Inject] protected IUriHelper UriHelper { get; set; }
        
        [Parameter] protected RenderFragment ChildContent { get; set; }  //LINKS

        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool ForceAnchor { get; set; }
        [Parameter] protected string Icon { get; set; }
        [Parameter] protected bool IsButton { get; set; }
        [Parameter] protected string Name { get; set; }
        [Parameter] protected string Target { get; set; }  //link <a> target
        [Parameter] protected string Title { get; set; } //tooltip and ARIA
        [Parameter] protected string Id { get; set; }
        [Parameter] protected string Url { get; set; }

        [Parameter] protected int NestedDepth { get; set; }

        [Parameter] EventCallback<NavFabricLinkBase> OnClick { get; set; }

        [CascadingParameter(Name="ClearSelectionAction")] Action ClearSelectionAction { get; set; }

        protected bool isExpanded { get; set; }


        protected bool isSelected { get; set; }
        protected string depthClass = "";
       
       
        protected override Task OnInitAsync()
        {
            System.Diagnostics.Debug.WriteLine("Initializing NavFabricLinkBase");
            ProcessUri(UriHelper.GetAbsoluteUri());
            UriHelper.OnLocationChanged += UriHelper_OnLocationChanged;
            return base.OnInitAsync();
        }
                

        private void UriHelper_OnLocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            ProcessUri(e.Location);
        }

        private void ProcessUri(string uri)
        {
            if (uri.EndsWith(this.Id) && !isSelected)
            {
                isSelected = true;
                StateHasChanged();
            }
            else if (!uri.EndsWith(this.Id) && isSelected)
            {
                isSelected = false;
                StateHasChanged();
            }
        }

        protected override Task OnParametersSetAsync()
        {
            switch (this.NestedDepth)
            {
                case 0:
                    depthClass = "";
                    break;
                case 1:
                    depthClass = "depth-one";
                    break;
                case 2:
                    depthClass = "depth-two";
                    break;
                case 3:
                    depthClass = "depth-three";
                    break;
                case 4:
                    depthClass = "depth-four";
                    break;
                case 5:
                    depthClass = "depth-five";
                    break;
                case 6:
                    depthClass = "depth-six";
                    break;
            }

           

            return base.OnParametersSetAsync();
        }

        protected Task ExpandHandler(UIMouseEventArgs args)
        {
            this.isExpanded = !this.isExpanded;
            return Task.CompletedTask;
        }

        protected async Task ClickHandler(UIMouseEventArgs args)
        {          
            await OnClick.InvokeAsync(this);
        }
    }
}
