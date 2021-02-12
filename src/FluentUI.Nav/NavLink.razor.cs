using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FluentUI
{
    public partial class NavLink : FluentUIComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }  //LINKS

        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool ForceAnchor { get; set; } //unused for now
        
        [Parameter] public string IconName { get; set; }
        [Parameter] public string IconSrc { get; set; }
        [Parameter] public bool IsButton { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public string Target { get; set; }  //link <a> target
        [Parameter] public string Title { get; set; } //tooltip and ARIA
        [Parameter] public string Id { get; set; }
        [Parameter] public bool IsExpanded { get => isExpanded; set => isExpanded = value; }
        [Parameter] public string Url { get; set; }

        [Parameter] public int NestedDepth { get; set; }
        [Parameter] public NavMatchType NavMatchType { get; set; } = NavMatchType.RelativeLinkOnly;

        [Parameter] public EventCallback<NavLink> OnClick { get; set; }
        [Parameter] public EventCallback<bool> IsExpandedChanged { get; set; }

        [CascadingParameter(Name = "ClearSelectionAction")] Action ClearSelectionAction { get; set; }
        [Parameter] public ICommand Command { get; set; }
        [Parameter] public object CommandParameter { get; set; }
        protected bool isExpanded { get; set; }


        protected bool isSelected { get; set; }
        protected string depthClass = "";

        private Rule NavLinkLeftPaddingRule = new Rule();
        private Rule ChevronButtonLeftRule = new Rule();
        private ICollection<IRule> NavLinkLocalRules { get; set; } = new List<IRule>();


        protected override Task OnInitializedAsync()
        {
            //System.Diagnostics.Debug.WriteLine("Initializing NavFabricLinkBase");
            ProcessUri(NavigationManager.Uri);
            NavigationManager.LocationChanged += UriHelper_OnLocationChanged;
            CreateLocalCss();

            return base.OnInitializedAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        private void CreateLocalCss()
        {
            NavLinkLeftPaddingRule.Selector = new ClassSelector() { SelectorName = "ms-Nav-link", LiteralPrefix = ".ms-Nav .ms-Nav-compositeLink " };
            NavLinkLocalRules.Add(NavLinkLeftPaddingRule);

            ChevronButtonLeftRule.Selector = new ClassSelector() { SelectorName = "ms-Nav-chevronButton", LiteralPrefix = ".ms-Nav-compositeLink:not(.is-button) " };
            NavLinkLocalRules.Add(ChevronButtonLeftRule);
        }

        private void SetStyle()
        {
            NavLinkLeftPaddingRule.Properties = new CssString()
            {
                Css = $"padding-left:{14 * NestedDepth + 3 + 24}px;" //(string.IsNullOrEmpty(Icon) ? 24 : 0 )}px;"
            };

            ChevronButtonLeftRule.Properties = new CssString()
            {
                Css = $"left:{14 * NestedDepth + 1}px;"
            };
        }

        private void UriHelper_OnLocationChanged(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        {
            ProcessUri(e.Location);
        }

        private void ProcessUri(string uri)
        {
            if (uri.StartsWith(NavigationManager.BaseUri))
                uri = uri.Substring(NavigationManager.BaseUri.Length, uri.Length - NavigationManager.BaseUri.Length);

            string processedUri = null;
            switch (NavMatchType)
            {
                case NavMatchType.RelativeLinkOnly:
                    processedUri = uri.Split('?', '#')[0];
                    break;
                case NavMatchType.AnchorIncluded:
                    var split = uri.Split('?');
                    processedUri = split[0];
                    if (split.Length > 1)
                    {
                        var anchorSplit = split[1].Split('#');
                        if (anchorSplit.Length > 1)
                            processedUri += "#" + anchorSplit[1];
                    }
                    else
                    {
                        var anchorSplit = split[0].Split('#');
                        if (anchorSplit.Length > 1)
                            processedUri += "#" + anchorSplit[1];
                    }
                    break;
                case NavMatchType.AnchorOnly:
                    var split2 = uri.Split('#');
                    if (split2.Length > 1)
                        processedUri = split2[1];
                    else
                        processedUri = "";
                    break;
            }

            if (processedUri.Equals(Id) && !isSelected)
            {
                isSelected = true;
                StateHasChanged();
            }
            else if (!processedUri.Equals(Id) && isSelected)
            {
                isSelected = false;
                StateHasChanged();
            }
        }

        protected override Task OnParametersSetAsync()
        {
            //switch (NestedDepth)
            //{
            //    case 0:
            //        depthClass = "";
            //        break;
            //    case 1:
            //        depthClass = "depth-one";
            //        break;
            //    case 2:
            //        depthClass = "depth-two";
            //        break;
            //    case 3:
            //        depthClass = "depth-three";
            //        break;
            //    case 4:
            //        depthClass = "depth-four";
            //        break;
            //    case 5:
            //        depthClass = "depth-five";
            //        break;
            //    case 6:
            //        depthClass = "depth-six";
            //        break;
            //}
            SetStyle();



            return base.OnParametersSetAsync();
        }

        protected Task ExpandHandler(MouseEventArgs args)
        {
            isExpanded = !isExpanded;
            return IsExpandedChanged.InvokeAsync(isExpanded);
            //return Task.CompletedTask;
        }

        protected async Task ClickHandler(MouseEventArgs args)
        {
            await OnClick.InvokeAsync(this);
        }
    }
}
