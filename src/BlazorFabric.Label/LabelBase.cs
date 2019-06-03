using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Label
{
    public class LabelBase : ComponentBase
    {
        [Parameter]
        protected RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether the associated form field is required or not
        /// </summary>
        [Parameter]
        protected bool Required { get; set; }

        /// <summary>
        /// Renders the label as disabled.
        /// </summary>
        [Parameter]
        protected bool Disabled { get; set; }

        [Parameter]
        protected string HtmlFor { get; set; }

    }
}
