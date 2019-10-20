using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class StackBase : FabricComponentBase
    {
        protected double rowGap;
        protected double columnGap;
        protected string horizontalMargin;
        protected string verticalMargin;

        protected string Id { get; set; } = Guid.NewGuid().ToString();

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
                horizontalMargin = (columnGap * 0.5).ToString() + "px";
                verticalMargin = (rowGap * 0.5).ToString() + "px";
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
                if (!double.IsNaN(Tokens.MaxWidth))
                    style += $"max-width:{Tokens.MaxWidth};";
                if (!double.IsNaN(Tokens.MaxHeight))
                    style += $"max-height:{Tokens.MaxHeight};";
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
                if (!double.IsNaN(Tokens.MaxWidth))
                    style += $"max-width:{Tokens.MaxWidth};";
                if (!double.IsNaN(Tokens.MaxHeight))
                    style += $"max-height:{Tokens.MaxHeight};";
                style += $"padding:{Tokens.Padding?.AsPixels};";
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
                style += $"padding:{Tokens.Padding?.AsPixels};";
                style += $"width:{(columnGap==0 ? "100%" : $"calc(100% + {columnGap}px)")};";

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

                if (!double.IsNaN(Tokens.MaxWidth))
                    style += $"max-width:{Tokens.MaxWidth};";
                if (!double.IsNaN(Tokens.MaxHeight))
                    style += $"max-height:{Tokens.MaxHeight};";
                style += "width:auto;";
                style += "overflow:visible;";
                style += "height:100%;";

                
                

                style += "display:flex;";

               
            }

            return style;
        }
    }
}
