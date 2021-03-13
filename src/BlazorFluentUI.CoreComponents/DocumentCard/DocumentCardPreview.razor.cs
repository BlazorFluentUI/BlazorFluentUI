using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    public partial class DocumentCardPreview : FluentUIComponentBase
    {
        public const int LIST_ITEM_COUNT = 3;

        public int OverflowDocumentCount => PreviewImages == null ? 0 : PreviewImages.Length - LIST_ITEM_COUNT;

        /// <summary>
        ///  One or more preview images to display.
        /// </summary>
        [Parameter]
        public DocumentPreviewImage[]? PreviewImages { get; set; }

        /// <summary>
        /// The function return string that will describe the number of overflow documents.
        /// </summary>
        [Parameter]
        public Func<int, string> GetOverflowDocumentCountText { get; set; } = (i) => "+" + i;

        public bool IsFileList => PreviewImages != null && PreviewImages.Length > 1;

        public static Dictionary<string, string> GlobalClassNames = new()
        {
            {"root", "ms-DocumentCardPreview"},
            {"icon", "ms-DocumentCardPreview-icon"},
            {"iconContainer", "ms-DocumentCardPreview-iconContainer"}
        };
        private ICollection<IRule> DocumentCardDetailsLocalRules { get; set; } = new List<IRule>();

        private Rule RootRule = new();
        private Rule PreviewIconRule = new();
        private Rule IconRule = new();
        private Rule FileListRule = new();
        private Rule FileListLiRule = new();
        private Rule FileListIconRule = new();
        private Rule FileListLinkRule = new();
        private Rule FileListLinkHoverRule = new();
        private Rule FileListOverflowTextRule = new();

        private void CreateLocalCss()
        {
            RootRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardPreview" };
            DocumentCardDetailsLocalRules.Add(RootRule);
            PreviewIconRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardPreview-iconContainer" };
            DocumentCardDetailsLocalRules.Add(PreviewIconRule);
            IconRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardPreview-icon" };
            DocumentCardDetailsLocalRules.Add(IconRule);
            FileListRule.Selector = new ClassSelector() { SelectorName = $"fileList" };
            DocumentCardDetailsLocalRules.Add(FileListRule);

            FileListLiRule.Selector = new CssStringSelector() { SelectorName = $".fileList li" };
            DocumentCardDetailsLocalRules.Add(FileListLiRule);

            FileListIconRule.Selector = new ClassSelector() { SelectorName = $"fileListIcon" };
            DocumentCardDetailsLocalRules.Add(FileListIconRule);
            FileListLinkRule.Selector = new ClassSelector() { SelectorName = $"fileListLink" };
            DocumentCardDetailsLocalRules.Add(FileListLinkRule);
            FileListLinkHoverRule.Selector = new ClassSelector() { SelectorName = $"fileListLink:hover" };
            DocumentCardDetailsLocalRules.Add(FileListLinkHoverRule);

            FileListOverflowTextRule.Selector = new ClassSelector() { SelectorName = $"fileListOverflowText" };
            DocumentCardDetailsLocalRules.Add(FileListOverflowTextRule);
        }

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            SetStyle();
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        private void SetStyle()
        {
            RootRule.Properties = new CssString()
            {
                Css = $"font-size:{Theme.FontStyle.FontSize.Small};" +
                      $"background-color: {(IsFileList ? Theme.Palette.White : Theme.Palette.NeutralLighterAlt)};" +
                      $"border-bottom:1px solid {Theme.Palette.NeutralLight};" +
                      $"overflow: hidden;" +
                      $"position:relative;"
            };

            PreviewIconRule.Properties = new CssString()
            {
                Css = $"display:flex;" +
                      $"align-items: center;" +
                      $"justify-content: center;" +
                      $"height:100%;"
            };

            IconRule.Properties = new CssString()
            {
                Css = $"left:10px;" +
                      $"bottom:10px;" +
                      $"position: absolute;"
            };

            FileListRule.Properties = new CssString()
            {
                Css = $"padding:16px 16px 0 16px;" +
                      $"list-style-type:none;" +
                      $"margin: 0;"
            };

            FileListLiRule.Properties = new CssString()
            {
                Css = $"height:16px;" +
                      $"line-height:16px;" +
                      $"margin-bottom: 8px;" +
                      $"overflow: hidden;"
            };

            FileListIconRule.Properties = new CssString()
            {
                Css = $"display:inline-block;" +
                      $"margin-right:8px;"
            };

            FileListLinkRule.Properties = new CssString()
            {
                Css = $"box-sizing:border-box;" +
                      $"color: {Theme.Palette.NeutralDark};" +
                      $"overflow:hidden;" +
                      $"display:inline-block;" +
                      $"text-decoration:none;" +
                      $"text-overflow:ellipsis;" +
                      $"white-space:no-wrap;" +
                      $"width;calc(100%-24px)"
            };

            FileListLinkHoverRule.Properties = new CssString()
            {
                Css = $"color:{Theme.Palette.ThemePrimary};"
            };

            FileListOverflowTextRule.Properties = new CssString()
            {
                Css = $"padding:0px 16px 8px 16px;" +
                      "display:block;"
            };
        }
    }
}
