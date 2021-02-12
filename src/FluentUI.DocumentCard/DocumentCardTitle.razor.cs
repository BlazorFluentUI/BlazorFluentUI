using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI
{
    public partial class DocumentCardTitle : FluentUIComponentBase, IAsyncDisposable
    {
        public string Id { get; set; }

        private DotNetObjectReference<DocumentCardTitle>? _dotNetObjectReference;

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

        private bool _needMeasurement = true;

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardTitle"}
        };

        public DocumentCardTitle()
        {
            ShouldTruncate = true;
            Id = $"documentCard" + Guid.NewGuid();
        }

        protected override void OnParametersSet()
        {
            _needMeasurement = ShouldTruncate;
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            _dotNetObjectReference = DotNetObjectReference.Create(this);
            await jSRuntime.InvokeVoidAsync("FluentUIDocumentCard.initTitle", Id, RootElementReference, _dotNetObjectReference, ShouldTruncate, Title).ConfigureAwait(false);
        }

        [JSInvokable]
        public void UpdateTitle(string title1, string title2)
        {
            TruncatedTitleFirstPiece = title1;
            TruncatedTitleSecondPiece = title2;
            _needMeasurement = false;
            StateHasChanged();
        }

        [JSInvokable]
        public void UpdateneedMeasurement()
        {
            _needMeasurement = true;
            TruncatedTitleSecondPiece = "";
            TruncatedTitleFirstPiece = "";
            StateHasChanged();
        }

        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
        //{
        //    var documentCardTitleRules = new HashSet<IRule>();

        //    documentCardTitleRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["root"]}" },
        //        Properties = new CssString()
        //        {
        //            Css = $"font-size:{(ShowAsSecondaryTitle ? theme.FontStyle.FontSize.Medium : theme.FontStyle.FontSize.Large)};" +
        //                  $"padding: 8px 16px;" +
        //                  $"overflow: hidden;" +
        //                  "word-wrap: break-word;" +
        //                  $"height:{(ShowAsSecondaryTitle ? "45px" : "38px")};" +
        //                  $"line-height:{(ShowAsSecondaryTitle ? "18px" : "21px")};" +
        //                  $"color:{(ShowAsSecondaryTitle ? theme.Palette.NeutralSecondary : theme.Palette.NeutralPrimary)};"
        //        }
        //    });
        //    return documentCardTitleRules;
        //}

        public async ValueTask DisposeAsync()
        {
            await jSRuntime.InvokeVoidAsync("FluentUIDocumentCard.removelement", Id).ConfigureAwait(false);
            _dotNetObjectReference?.Dispose();
        }
    }
}
