using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class ItemClickedArgs
    {
        public string Key { get; set; }
        public MouseEventArgs MouseEventArgs { get; set; }
    }
}
