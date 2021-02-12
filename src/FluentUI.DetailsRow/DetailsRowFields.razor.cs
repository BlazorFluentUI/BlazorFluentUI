using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DetailsRowFields<TItem> : FluentUIComponentBase
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public int ItemIndex { get; set; }

        [Parameter]
        public double CellLeftPadding { get; set; } = 12;

        [Parameter]
        public double CellRightPadding { get; set; } = 8;

        [Parameter]
        public double CellExtraRightPadding { get; set; } = 24;

        [Parameter]
        public int ColumnStartIndex { get; set; }

        [Parameter]
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool EnableUpdateAnimations { get; set; }

        [Parameter]
        public string RowClassNames { get; set; }


        private string key;

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }
    }
}
