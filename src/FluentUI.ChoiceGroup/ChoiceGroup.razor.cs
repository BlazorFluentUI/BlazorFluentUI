using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class ChoiceGroup<TItem> : FluentUIComponentBase
    {
        [Parameter] public string Label { get; set; } = "Pick one";
        [Parameter] public IList<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> OptionTemplate { get; set; }
        [Parameter] public TItem Value { get; set; }
        [Parameter] public EventCallback<TItem> ValueChanged { get; set; }
        [Parameter] public FlexDirection ItemAlignment { get; set; } = FlexDirection.Column;
        [Parameter] public string Id { get; set; }
        [Parameter] public bool Required { get; set; } = false;

        private TItem focusedItem = default;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (string.IsNullOrWhiteSpace(Id))
                Id = Id = $"g{Guid.NewGuid()}";
        }

        private async Task OnChoiceOptionClicked(ChoiceGroupOptionClickedEventArgs choiceGroupOptionClickedEventArgs)
        {
            await ValueChanged.InvokeAsync((TItem)choiceGroupOptionClickedEventArgs.Item);
        }

        private void OnFocus(ChoiceGroupOptionFocusEventArgs args)
        {
            focusedItem = (TItem)args.Item;
        }
        private void OnBlur(ChoiceGroupOptionFocusEventArgs args)
        {
            focusedItem = default;
        }
    }
}