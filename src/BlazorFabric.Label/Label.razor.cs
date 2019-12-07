using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Label : FabricComponentBase
    {
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

    }
}
