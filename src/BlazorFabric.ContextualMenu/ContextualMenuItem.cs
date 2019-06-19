using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.ContextualMenu
{
    public class ContextualMenuItem : FabricComponentBase
    {
        [Parameter] public string Href { get; set; }
        [Parameter] public string Key { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public string SecondaryText { get; set; }
        [Parameter] public string IconName { get; set; }
        [Parameter] public ContextualMenuItemType ItemType { get; set; }

        [Parameter] public RenderFragment SubmenuContent { get; set; }
        [Parameter] public IEnumerable ItemsSource { get; set; }
        [Parameter] public IEnumerable ItemTemplate { get; set; }

        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool CanCheck { get; set; }
        [Parameter] public bool Checked { get; set; }
        [Parameter] public bool Split { get; set; }

        [Parameter] public EventCallback<UIMouseEventArgs> OnClick { get; set; }

        [CascadingParameter] public ContextualMenuBase<object> ContextualMenu { get; set; }

        public bool IsExpanded { get; set; }

        public override Task SetParametersAsync(ParameterCollection parameters)
        {
            if (IconName != null && parameters.GetValueOrDefault<string>("IconName") == null)
            {
                if (ContextualMenu != null)
                    ContextualMenu.HasIconCount--;
            }
            return base.SetParametersAsync(parameters);
        }

        protected override Task OnParametersSetAsync()
        {
            if (IconName != null)
                ContextualMenu.HasIconCount++;


            return base.OnParametersSetAsync();
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (this.ItemType == ContextualMenuItemType.Divider)
            {
                RenderSeparator(builder);
            }
            else if (this.ItemType == ContextualMenuItemType.Header)
            {
                RenderSeparator(builder);
                RenderHeader(builder);
            }
            else if (this.ItemType == ContextualMenuItemType.Section)
            {

            }
            else
            {
                RenderNormalItem(builder);
            }
        }

        private void RenderHeader(RenderTreeBuilder builder)
        {
            builder.OpenElement(11, "li");
            {
                builder.AddAttribute(12, "class", "ms-ContextualMenu-item");
                builder.OpenElement(13, "div");
                {
                    builder.AddAttribute(14, "class", "ms-ContextualMenu-header");
                    builder.OpenComponent<ContextualMenuItem>(15);
                    builder.CloseComponent();
                }
                builder.CloseElement();
            }
            builder.CloseElement();
        }

        private void RenderSeparator(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "li");
            builder.AddAttribute(1, "role", "separator");
            builder.AddAttribute(2, "class", "ms-ContextualMenu-divider");
            builder.AddAttribute(3, "aria-hidden", true);
            builder.CloseElement();
        }

        private void RenderNormalItem(RenderTreeBuilder builder)
        {
            builder.OpenElement(11, "li");
            builder.AddAttribute(12, "class", $"ms-ContextualMenu-item mediumFont {(Disabled ? "is-disabled" : "")} {(Checked ? "is-checked" : "")} {(IsExpanded ? "is-expanded" : "")}");
            RenderItemKind(builder);
            builder.CloseElement();
        }

        private void RenderItemKind(RenderTreeBuilder builder)
        {
            if (this.Href!=null)
            {
                RenderMenuAnchor(builder);
                return;
            }
            if (this.Split && SubmenuContent != null)
            {
                //RenderSplitButton
                return;
            }
            RenderButtonItem(builder);
        }

        private void RenderButtonItem(RenderTreeBuilder builder)
        {
            builder.OpenElement(20, "div");
            //skip KeytipData
            builder.OpenElement(21, "button");
            builder.AddAttribute(22, "onclick", this.OnClick);
            builder.AddAttribute(23, "role", "menuitem");
            builder.AddAttribute(24, "class", "ms-ContextualMenu-link mediumFont");

            RenderMenuItemContent(builder);

            builder.CloseElement();
            builder.CloseElement();
        }

        private void RenderMenuAnchor(RenderTreeBuilder builder)
        {
            builder.OpenElement(20, "div");
            //skip KeytipData
            builder.OpenElement(21, "a");
            builder.AddAttribute(22, "href", this.Href);
            builder.AddAttribute(23, "onclick", this.OnClick);
            builder.AddAttribute(24, "role", "menuitem");
            builder.AddAttribute(25, "class", "ms-ContextualMenu-link mediumFont");

            RenderMenuItemContent(builder);

            builder.CloseElement();
            builder.CloseElement();
        }



        private void RenderMenuItemContent(RenderTreeBuilder builder)
        {
            builder.OpenElement(40, "div");
            builder.AddAttribute(41, "class", this.Split ? "ms-ContextualMenu-linkContentMenu" : "ms-ContextualMenu-linkContent");

            if (CanCheck)
                RenderCheckMarkIcon(builder);
            if (ContextualMenu.HasIconCount > 0)
                RenderItemIcon(builder);
            RenderItemName(builder);
            if (SecondaryText != null)
                RenderSecondaryText(builder);
            if (SubmenuContent != null)
                RenderSubMenuIcon(builder);

            builder.CloseElement();
        }

        private void RenderSubMenuIcon(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Icon.Icon>(65);
            builder.AddAttribute(66, "ClassName", "ms-ContextualMenu-submenuIcon");
            builder.AddAttribute(67, "IconName", "ChevronRight");  //ignore RTL for now.
            builder.CloseComponent();
        }


        private void RenderSecondaryText(RenderTreeBuilder builder)
        {
            builder.OpenElement(61, "span");
            builder.AddAttribute(62, "class", "ms-ContextualMenu-secondaryText");
            builder.AddContent(63, this.SecondaryText);
            builder.CloseElement();
        }

        private void RenderItemName(RenderTreeBuilder builder)
        {
            builder.OpenElement(55, "span");
            builder.AddAttribute(56, "class", "ms-ContextualMenu-label");
            builder.AddContent(57, this.Text);
            builder.CloseElement();
        }


        private void RenderItemIcon(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Icon.Icon>(51);
            builder.AddAttribute(52, "IconName", this.IconName);
            builder.AddAttribute(53, "ClassName", "ms-ContextualMenu-icon");
            builder.CloseComponent();
        }

        private void RenderCheckMarkIcon(RenderTreeBuilder builder)
        {          
            builder.OpenComponent<Icon.Icon>(45);
            builder.AddAttribute(46, "IconName", this.Checked ? "CheckMark" : "");
            builder.AddAttribute(47, "ClassName", "ms-ContextualMenu-checkmarkIcon");
            builder.CloseComponent();
        }
    }
}
