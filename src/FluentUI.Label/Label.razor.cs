using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class Label : FluentUIComponentBase
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether the associated form field is required or not
        /// </summary>
        [Parameter]
        public bool Required { get; set; }

        /// <summary>
        /// Renders the label as disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string HtmlFor { get; set; }  //not being used for anything.

       
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (string.IsNullOrWhiteSpace(Id))
                Id = Id = $"g{Guid.NewGuid()}";
        }
    }
}
