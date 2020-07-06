using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    // TODO: support for ShouldTruncate = true
    public partial class BFUDocumentCardTitle : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        /// <summary>
        /// Title text.
        /// If the card represents more than one document, this should be the title of one document and a "+X" string.
        /// For example, a collection of four documents would have a string of "Document.docx +3".
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Parameter] public string? Title { get; set; }
        /// <summary>
        /// Whether we truncate the title to fit within the box. May have a performance impact. Default is true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [should truncate]; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool ShouldTruncate { get; set; }
        /// <summary>
        ///  Whether show as title as secondary title style such as smaller font and lighter color.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show as secondary title]; otherwise, <c>false</c>.
        /// </value>
        [Parameter] public bool ShowAsSecondaryTitle { get; set; }

        [Inject]
        internal IJSRuntime? jSRuntime { get; set; }

        private string? TruncatedTitleFirstPiece { get; set; }
        private string? TruncatedTitleSecondPiece { get; set; }

        private bool _needMeasurement;

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardTitle"}
        };

        public BFUDocumentCardTitle()
        {
            ShouldTruncate = true;
        }

        protected override void OnParametersSet()
        {
            _needMeasurement = ShouldTruncate;
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            //TruncateTitle();
        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var documentCardTitleRules = new HashSet<IRule>();

            documentCardTitleRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = $"font-size:{(ShowAsSecondaryTitle ? theme.FontStyle.FontSize.Medium : theme.FontStyle.FontSize.Large)};" +
                          $"padding: 8px 16px;" +
                          $"overflow: hidden;" +
                          "word-wrap: break-word;" +
                          $"height:{(ShowAsSecondaryTitle ? "45px" : "38px")};" +
                          $"line-height:{(ShowAsSecondaryTitle ? "18px" : "21px")};" +
                          $"color:{(ShowAsSecondaryTitle ? theme.Palette.NeutralSecondary : theme.Palette.NeutralPrimary)};"
                }
            });
            return documentCardTitleRules;
        }

        //private void TruncateTitle()
        //{
        //    if (_elementReference == null || !_elementReference.HasValue)
        //    {
        //        return;
        //    }

        //}
    }
}
