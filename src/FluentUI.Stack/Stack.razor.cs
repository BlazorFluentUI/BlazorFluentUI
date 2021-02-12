using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class Stack : FluentUIComponentBase
    {
        protected double rowGap;
        protected double columnGap;
        protected string horizontalMargin;
        protected string verticalMargin;

        private string _id = "g" + Guid.NewGuid().ToString();  //id selectors can't begin with a number
        protected string Id => _id; 

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool DisableShrink { get; set; } = false;
        [Parameter] public CssValue Grow { get; set; } 
        [Parameter] public bool Horizontal { get; set; } = false;
        [Parameter] public Alignment HorizontalAlign { get; set; } = Alignment.Unset;
        [Parameter] public bool Reversed { get; set; } = false;
        [Parameter] public Alignment VerticalAlign { get; set; } = Alignment.Unset;
        [Parameter] public bool VerticalFill { get; set; } = false;
        [Parameter] public bool Wrap { get; set; } = false;

        [Parameter] public StackTokens Tokens { get; set; } = new StackTokens();

        protected override Task OnParametersSetAsync()
        {
            rowGap = 0;
            columnGap = 0;

            if (Tokens.ChildrenGap != null)
            {
                if (Tokens.ChildrenGap.Length == 1)
                {
                    rowGap = Tokens.ChildrenGap[0];
                    columnGap = Tokens.ChildrenGap[0];
                }
                else if (Tokens.ChildrenGap.Length == 2)
                {
                    rowGap = Tokens.ChildrenGap[0];
                    columnGap = Tokens.ChildrenGap[1];
                }
                horizontalMargin = (columnGap * (-0.5)).ToString() + "px";
                verticalMargin = (rowGap * (-0.5)).ToString() + "px";
            }
            else
            {
                horizontalMargin = "0px";
                verticalMargin = "0px";
            }

            return base.OnParametersSetAsync();
        }

        protected string GetStyles()
        {
            string style = "";
                        
            if(Wrap)
            {
                style += "flex-wrap:wrap;";
                if (Tokens.MaxWidth != null)
                    style += $"max-width:{Tokens.MaxWidth.AsLength};";
                if (Tokens.MaxHeight != null)
                    style += $"max-height:{Tokens.MaxHeight.AsLength};";
                style += "width:auto;";
                style += "overflow:visible;";
                style += "height:100%;";

                if (HorizontalAlign != Alignment.Unset)
                    style += $"{(Horizontal ? "justify-content" : "align-items")}:{CssUtils.AlignMap[HorizontalAlign]};";
                if (VerticalAlign != Alignment.Unset)
                    style += $"{(Horizontal ? "align-items" : "justify-content")}:{CssUtils.AlignMap[VerticalAlign]};";

                style += "display:flex;";

                if (Horizontal)
                    style += $"height:{(VerticalFill ? "100%" : "auto")};";
            }
            else
            {
                style += "display:flex;";
                style += $"flex-direction:{(Horizontal ? (Reversed ? "row-reverse" : "row") : (Reversed ? "column-reverse" : "column"))};";
                style += "flex-wrap:nowrap;";
                style += "width:auto;";
                style += $"height:{(VerticalFill ? "100%" : "auto")};";
                if (Tokens.MaxWidth != null)
                    style += $"max-width:{Tokens.MaxWidth.AsLength};";
                if (Tokens.MaxHeight != null)
                    style += $"max-height:{Tokens.MaxHeight.AsLength};";
                if (Tokens.Padding != null)
                    style += $"padding:{Tokens.Padding?.AsLength};";
                style += "box-sizing:border-box;";

                if (Grow != null)
                    style += $"flex-grow:{(Grow.AsBooleanTrueExplicit == true ? "1" : Grow.AsString)};";
                if (HorizontalAlign != Alignment.Unset)
                    style += $"{(Horizontal ? "justify-content" : "align-items")}:{CssUtils.AlignMap[HorizontalAlign]};";
                if (VerticalAlign != Alignment.Unset)
                    style += $"{(Horizontal ? "align-items" : "justify-content")}:{CssUtils.AlignMap[VerticalAlign]};";
            }

            return style;
        }

        protected string GetInnerStyles()
        {            
            string style = "";
                        
            if (Wrap)
            {
                style += "display:flex;";
                style += "flex-wrap:wrap;";
                style += $"margin-left:{horizontalMargin};";
                style += $"margin-right:{horizontalMargin};";
                style += $"margin-top:{verticalMargin};";
                style += $"margin-bottom:{verticalMargin};";
                style += "overflow:visible;";
                style += "box-sizing:border-box;";
                if (Tokens.Padding != null)
                    style += $"padding:{Tokens.Padding.AsLength};";
                style += $"width:{(columnGap==0 ? "100%" : $"calc(100% + {columnGap}px)")};";
                style += "max-width:100vw;";

                if (HorizontalAlign != Alignment.Unset)
                    style += $"{(Horizontal ? "justify-content" : "align-items")}:{CssUtils.AlignMap[HorizontalAlign]};";
                if (VerticalAlign != Alignment.Unset)
                    style += $"{(Horizontal ? "align-items" : "justify-content")}:{CssUtils.AlignMap[VerticalAlign]};";
                if (Horizontal)
                {
                    style += $"flex-direction:{(Reversed ? "row-reverse" : "row")};";
                    style += $"height:{(rowGap == 0 ? "100%" : $"calc(100% - {rowGap}px)")};";
                }
                else
                {
                    style += $"flex-direction:{(Reversed ? "column-reverse" : "column")};";
                    style += $"height:calc(100% - {rowGap}px);";
                }

                if (Tokens.MaxWidth != null)
                    style += $"max-width:{Tokens.MaxWidth.AsLength};";
                if (Tokens.MaxHeight != null)
                    style += $"max-height:{Tokens.MaxHeight.AsLength};";
                style += "width:auto;";
                style += "overflow:visible;";
                style += "height:100%;";

                
                

                style += "display:flex;";

               
            }

            return style;
        }
    }
}
