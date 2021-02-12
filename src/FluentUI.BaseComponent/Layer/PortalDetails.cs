using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public class PortalDetails
    {
        public string? Id { get; set; }
        public RenderFragment? Fragment { get; set; }
        //public string? Style { get; set; }
        public ElementReference Parent { get; set; }
    }
}
