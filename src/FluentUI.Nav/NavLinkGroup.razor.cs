using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class NavLinkGroup : FluentUIComponentBase
    {
        [Parameter] public bool CollapseByDefault { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public RenderFragment<string> GroupHeaderTemplate { get; set; }
        [Parameter] public string Name { get; set; }

        [CascadingParameter] protected string ExpandButtonAriaLabel { get; set; }

        [Parameter] public EventCallback<NavLinkGroup> OnClick { get; set; }

        private bool isCollapsed;
        public bool IsCollapsed => isCollapsed;
        
        private bool hasRenderedOnce;

        protected async Task ClickHandler(MouseEventArgs args)
        {
            isCollapsed = !isCollapsed;
            await OnClick.InvokeAsync(this);
            //return Task.CompletedTask;
        }

        protected override Task OnInitializedAsync()
        {
            isCollapsed = false;
            //System.Diagnostics.Debug.WriteLine("Initializing NavLinkGroupBase");
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (!hasRenderedOnce)
                isCollapsed = CollapseByDefault;
            return base.OnParametersSetAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                hasRenderedOnce = true;
            }

            base.OnAfterRender(firstRender);
        }

    }
}
