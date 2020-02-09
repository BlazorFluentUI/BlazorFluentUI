using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class DetailsRow<TItem> : FabricComponentBase
    {
        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public bool AnySelected { get; set; }

        //[Parameter]
        //public bool CanSelect { get; set; }

        [Parameter]
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool EnableUpdateAnimations { get; set; }

        [Parameter]
        public int GroupNestingDepth { get; set; }

        [Parameter]
        public double IndentWidth { get; set; } = 36;

        [Parameter]
        public bool IsCheckVisible { get; set; }

        [Parameter]
        public bool IsRowHeader { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public int ItemIndex { get; set; }

        [Parameter]
        public double RowWidth { get; set; } = 0;

        [Parameter]
        public object Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }
        
        [Parameter]
        public bool UseFastIcons { get; set; } = true;

        bool canSelect;
        bool showCheckbox;
        bool columnMeasureInfo = true;

        protected override Task OnParametersSetAsync()
        {
            showCheckbox = SelectionMode != SelectionMode.None && CheckboxVisibility != CheckboxVisibility.Hidden;

            canSelect = Selection != null;

            return base.OnParametersSetAsync();
        }
    }
}
