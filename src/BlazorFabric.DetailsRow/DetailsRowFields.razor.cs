using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class DetailsRowFields<TItem> : FabricComponentBase
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public int ItemIndex { get; set; }

        [Parameter]
        public int ColumnStartIndex { get; set; }

        [Parameter]
        public object[] Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public string RowClassNames { get; set; }

        private bool enableUpdateAnimations;
    }
}
