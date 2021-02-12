using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FluentUI.DropdownInternal
{
    public class DropdownItem: FluentUIComponentBase
    {
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool Hidden { get; set; }
        [Parameter] public SelectableOptionMenuItemType ItemType { get; set; } = SelectableOptionMenuItemType.Normal;
        [Parameter] public string Key { get; set; }
        //[Parameter] protected string Selected { get; set; }
        [Parameter] public string Text { get; set; }
        

        [CascadingParameter] protected Dropdown Dropdown { get; set; }

        private bool isSelected = false;

        protected override Task OnParametersSetAsync()
        {
            if (Dropdown!= null && 
                (Dropdown.SelectedOptions.Count() > 0|| Dropdown.SelectedOption != null))
            {
                if (Dropdown.SelectedOptions.FirstOrDefault(x => x.Key == Key) != null)
                    isSelected = true;
                else if (Dropdown.SelectedOption?.Key == Key)
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

        private void ApplyChange()
        {
            if (Dropdown.MultiSelect)
            {
                if (isSelected) 
                { 
                    Dropdown.RemoveSelection(Key); 
                } 
                else { 
                    Dropdown.AddSelection(Key); 
                }
            }
            else
            {
                Dropdown.ResetSelection(); 
                Dropdown.AddSelection(Key);
            }
        }

        private void BuildOption(RenderTreeBuilder builder, int i=0)
        {
            if (Dropdown.MultiSelect)
            {
                builder.OpenComponent<Checkbox>(i);
                    //builder.AddAttribute(i + 2, "Key", this.Key);
                builder.AddAttribute(i + 2, "Disabled", Disabled);
                builder.AddAttribute(i + 3, "ClassName", $"ms-Dropdown-item {(Disabled ? "is-disabled" : "")} {(Hidden ? "is-hidden" : "")}  {(isSelected ? "selected" : "")}");
                builder.AddAttribute(i + 4, "Label", Text);
                builder.AddAttribute(i + 5, "Checked", isSelected);
                builder.AddAttribute(i + 6, "CheckedChanged", EventCallback.Factory.Create<bool>(this, __value => { ApplyChange(); }));
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<CommandButton>(i);
                //builder.AddAttribute(i + 1, "Key", this.Key);
                builder.AddAttribute(i + 2, "Disabled", Disabled);
                builder.AddAttribute(i + 3, "ClassName", $"ms-Dropdown-item {(Disabled ? "is-disabled" : "")} {(Hidden ? "is-hidden" : "")}  {(isSelected ? "selected" : "")}");
                builder.AddAttribute(i + 4, "OnClick", EventCallback.Factory.Create<MouseEventArgs>(this, __value => { ApplyChange(); }));
                builder.AddAttribute(i + 5, "ChildContent", (RenderFragment)((builder2) =>
                {
                    builder2.OpenElement(i + 6, "span");
                    builder2.AddAttribute(i + 7, "class", "ms-Dropdown-optionText");
                    builder2.AddContent(i + 8, Text);
                    builder2.CloseElement();
                }));                
                builder.CloseComponent();
            }
        }

        private void BuildHeader(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "mediumFont ms-Dropdown-itemHeader");
            builder.AddElementReferenceCapture(2, element => RootElementReference = element);
                builder.OpenElement(3, "span");
                builder.AddAttribute(4, "class", "ms-Dropdown-optionText");
                builder.AddContent(5, Text);
                builder.CloseElement();
            builder.CloseElement();
        }

        private void BuildSeparator(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "role", "separator");
            //builder.AddAttribute(3, "key", 1);
            builder.AddAttribute(2, "class", "ms-Dropdown-divider");
            builder.AddElementReferenceCapture(3, element => RootElementReference = element);

            builder.CloseElement();
        }
    }
}
