
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorFabric
{
    public class Text : FabricComponentBase
    {
        [Parameter] public string As { get; set; } = "span";
        [Parameter] public bool Block { get; set; }
        [Parameter] public bool NoWrap { get; set; }
        [Parameter] public TextType Variant { get; set; }
        [Parameter]public RenderFragment ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //base.BuildRenderTree(builder);

            builder.OpenElement(0, As);
            builder.AddAttribute(1, "class", $"ms-text {(Block ? (As == "td" ? "ms-text--table" : "ms-text--block") : "")} {(NoWrap ? "ms-text--nowrap" : "")}" );
            builder.AddContent(2, ChildContent);
            builder.CloseElement();
        }
    }
}
