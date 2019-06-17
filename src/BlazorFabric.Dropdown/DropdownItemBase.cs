using BlazorFabric.BaseComponent;
using BlazorFabric.Button;
using BlazorFabric.Checkbox;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Dropdown
{
    public class DropdownItem<TItem>: FabricComponentBase
    {
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool Hidden { get; set; }
        [Parameter] protected SelectableOptionMenuItemType ItemType { get; set; } = SelectableOptionMenuItemType.Normal;
        [Parameter] protected string Key { get; set; }
        [Parameter] protected string Text { get; set; }
        [Parameter] protected string Title { get; set; }

        [CascadingParameter] protected DropdownBase<TItem> Dropdown { get; set; }

        private bool isSelected = false;


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            switch (ItemType)
            {
                case SelectableOptionMenuItemType.Divider:
                    BuildSeparator(builder);
                    break;
                case SelectableOptionMenuItemType.Header:
                    BuildHeader(builder);
                    break;
                default:
                    BuildOption(builder);
                    break;
            }

        }

        private void BuildOption(RenderTreeBuilder builder, int i=0)
        {
            if (Dropdown.MultiSelect)
            {
                builder.OpenComponent<Checkbox.Checkbox>(i);
                builder.AddAttribute(i + 1, "key", this.Key);
                builder.AddAttribute(i + 2, "Disabled", Disabled);
                builder.AddAttribute(i + 3, "ClassName", $"ms-Dropdown-item {(Disabled ? "is-disabled" : "")} {(Hidden ? "is-hidden" : "")}  {(isSelected ? "selected" : "")}");
                builder.AddAttribute(i + 4, "Label", this.Text);
                builder.AddAttribute(i + 5, "Title", this.Title != null ? this.Title : this.Text);
                builder.AddAttribute(i + 6, "Checked", isSelected);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<ActionButton>(i);
                builder.AddAttribute(i + 1, "key", this.Key);
                builder.AddAttribute(i + 2, "Disabled", Disabled);
                builder.AddAttribute(i + 3, "ClassName", $"ms-Dropdown-item {(Disabled ? "is-disabled" : "")} {(Hidden ? "is-hidden" : "")}  {(isSelected ? "selected" : "")}");
                builder.CloseComponent();
            }
        }

        private void BuildHeader(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "key", this.Key);
            builder.AddAttribute(2, "class", "ms-Dropdown-itemHeader");
            BuildOption(builder, 3);
            builder.CloseElement();
        }

        private void BuildSeparator(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "role", "separator");
            builder.AddAttribute(2, "key", 1);
            builder.AddAttribute(3, "class", "ms-Dropdown-divider");
            builder.CloseElement();
        }
    }
}
