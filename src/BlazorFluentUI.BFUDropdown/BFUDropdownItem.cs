using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public class BFUDropdownItem<TItem>: BFUComponentBase
    {
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool Hidden { get; set; }
        [Parameter] public SelectableOptionMenuItemType ItemType { get; set; } = SelectableOptionMenuItemType.Normal;
        [Parameter] public string ItemKey { get; set; }
        //[Parameter] protected string Selected { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public string Title { get; set; }

        [CascadingParameter] protected BFUDropdown<TItem> Dropdown { get; set; }

        private bool isSelected = false;

        protected override Task OnParametersSetAsync()
        {
            if (this.Dropdown!= null && (this.Dropdown.SelectedKeys.Count > 0 || this.Dropdown.SelectedKey != null))
            {
                if (this.Dropdown.SelectedKeys.Contains(this.ItemKey))
                    isSelected = true;
                else if (this.Dropdown.SelectedKey == this.ItemKey)
                    isSelected = true;
                else
                    isSelected = false;
            }
            else
            {
                isSelected = false;
            }

            return base.OnParametersSetAsync();
        }


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
                builder.OpenComponent<BFUCheckbox>(i);
                //builder.AddAttribute(i + 2, "Key", this.Key);
                builder.AddAttribute(i + 2, "Disabled", Disabled);
                builder.AddAttribute(i + 3, "ClassName", $"ms-Dropdown-item {(Disabled ? "is-disabled" : "")} {(Hidden ? "is-hidden" : "")}  {(isSelected ? "selected" : "")}");
                builder.AddAttribute(i + 4, "Label", this.Text);
                builder.AddAttribute(i + 5, "Checked", isSelected);
                builder.AddAttribute(i + 6, "CheckedChanged", EventCallback.Factory.Create<bool>(this, __value => { if (isSelected) { this.Dropdown.RemoveSelection(this.ItemKey); } else { this.Dropdown.AddSelection(this.ItemKey); } }));
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<BFUCommandButton>(i);
                //builder.AddAttribute(i + 1, "Key", this.Key);
                builder.AddAttribute(i + 2, "Disabled", Disabled);
                builder.AddAttribute(i + 3, "ClassName", $"ms-Dropdown-item {(Disabled ? "is-disabled" : "")} {(Hidden ? "is-hidden" : "")}  {(isSelected ? "selected" : "")}");
                builder.AddAttribute(i + 4, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, __value => { this.Dropdown.ResetSelection(); this.Dropdown.AddSelection(this.ItemKey); }));
                builder.AddAttribute(i + 5, "ChildContent", (RenderFragment)((builder2) =>
                {
                    builder2.OpenElement(i + 6, "span");
                    builder2.AddAttribute(i + 7, "class", "ms-Dropdown-optionText");
                    builder2.AddContent(i + 8, this.Text);
                    builder2.CloseElement();
                }));                
                builder.CloseComponent();
            }
        }

        private void BuildHeader(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "mediumFont ms-Dropdown-itemHeader");
            builder.AddElementReferenceCapture(2, element => this.RootElementReference = element);
                builder.OpenElement(3, "span");
                builder.AddAttribute(4, "class", "ms-Dropdown-optionText");
                builder.AddContent(5, this.Text);
                builder.CloseElement();
            builder.CloseElement();
        }

        private void BuildSeparator(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "role", "separator");
            //builder.AddAttribute(3, "key", 1);
            builder.AddAttribute(2, "class", "ms-Dropdown-divider");
            builder.AddElementReferenceCapture(3, element => this.RootElementReference = element);

            builder.CloseElement();
        }
    }
}
