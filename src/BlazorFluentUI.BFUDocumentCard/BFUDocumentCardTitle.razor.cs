using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    public partial class BFUDocumentCardTitle : BFUComponentBase
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
        

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardTitle"}
        };

        public BFUDocumentCardTitle()
        {
            ShouldTruncate = true;
            ClassName = GlobalClassNames["root"];
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
    }
}
