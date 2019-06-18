using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.ContextualMenu
{
    public class ContextualMenuItem : FabricComponentBase
    {
        [Parameter] protected string Href { get; set; }
        [Parameter] protected string Key { get; set; }
        [Parameter] protected string Text { get; set; }
        [Parameter] protected string SecondaryText { get; set; }
        [Parameter] protected ContextualMenuItemType ItemType { get; set; }

        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool CanCheck { get; set; }
        [Parameter] protected bool Checked { get; set; }
        [Parameter] protected bool Split { get; set; }

        [Parameter] protected EventCallback<UIMouseEventArgs> OnClick { get; set; }

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
            builder.AddAttribute(12, "class", "ms-ContextualMenu-item");
            RenderItemKind(builder);
            builder.CloseElement();
        }

        private void RenderItemKind(RenderTreeBuilder builder)
        {
            if (this.Href!=null)
            {
                RenderMenuAnchor(builder);
            }
        }

        private void RenderMenuAnchor(RenderTreeBuilder builder)
        {
            builder.OpenElement(20, "div");
            //skip KeytipData
            builder.OpenElement(21, "a");
            builder.AddAttribute(22, "href", this.Href);
            builder.AddAttribute(23, "onclick", this.OnClick);
            builder.AddAttribute(24, "role", "menuitem");
            builder.AddAttribute(25, "class", "ms-ContextualMenu-link");

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

            RenderItemIcon(builder);

            builder.CloseElement();
        }

        private void RenderItemIcon(RenderTreeBuilder builder)
        {
            
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
