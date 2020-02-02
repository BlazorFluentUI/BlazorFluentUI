using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GroupedListSection<TItem> : FabricComponentBase
    {
        private IEnumerable<Group<TItem>> _itemsHaveGroups;
        private IEnumerable<Group<TItem>> _itemsWithoutGroups;


        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public Group<TItem> Group { get; set; }

        [Parameter]
        public int GroupIndex { get; set; }

        [Parameter]
        public Func<TItem, object> GroupKeySelector { get; set; }

        [Parameter]
        public int GroupNestingDepth { get; set; }

        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter]
        public Func<bool> OnShouldVirtualize { get; set; } = () => true;

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }

        [Parameter]
        public EventCallback<double> OnHeightChanged { get; set; }

        //[Parameter]
        //public IEnumerable<IGrouping<object, TItem>> Groups { get; set; }

        [Parameter] public IObservable<Unit> OnScrollExternalObservable { get; set; }  //For nested lists like GroupedList, too many javascript scroll events bogs down the whole system
        [Parameter] public ManualRectangle ExternalProvidedScrollDimensions { get; set; }  //For nested lists like GroupedList

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }


        protected override Task OnParametersSetAsync()
        {
            if (GroupKeySelector != null && SubGroupSelector != null)
            {
                //_itemsHaveGroups = Group.Children.Where(x => SubGroupSelector(x) != null).Select(x => new Group<TItem>(x, GroupKeySelector, SubGroupSelector, Group.Level++));
                //_itemsWithoutGroups = Group.Children.Where(x => SubGroupSelector(x) == null);
                _itemsHaveGroups = Group.Children.Where(x => x.Children != null);
                _itemsWithoutGroups = Group.Children.Where(x => x.Children == null);

                //groups = Items.Select(SubGroupSelector);
            }
            return base.OnParametersSetAsync();
        }

        private void HandleListScrollerHeightChanged((double, object) details)
        {
            Debug.WriteLine($"Height changed: {details.Item1} for {(int)details.Item2}");
            OnHeightChanged.InvokeAsync(details.Item1);
        }

        private string GetGroupKey(Group<TItem> group, int index)
        {
            return $"group-{(group != null && !string.IsNullOrEmpty(group.Key) ? group.Key : group.Level.ToString())}{index.ToString()}";
        }
    }
}
