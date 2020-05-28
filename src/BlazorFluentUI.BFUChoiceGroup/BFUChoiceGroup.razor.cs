using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI.BFUChoiceGroup
{
    public partial class BFUChoiceGroup<TItem> : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter] public string Label { get; set; } = "Pick one";
        [Parameter] public IList<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> OptionTemplate { get; set; }
        [Parameter] public TItem Value { get; set; }
        [Parameter] public EventCallback<TItem> ValueChanged { get; set; }
        [Parameter] public FlexDirection ItemAlignment { get; set; } = FlexDirection.Column;
        [Parameter] public string Id { get; set; }
        [Parameter] public bool Required { get; set; } = false;

        public ICollection<Rule> CreateGlobalCss(ITheme theme)
        {
            var choiceGroupRules = new HashSet<Rule>();
            #region Root
            choiceGroupRules.AddCssStringSelector(".ms-ChoiceFieldGroup").AppendCssStyles(
                $"font-size:{theme.FontStyle.FontSize.Medium}",
                $"font-weight:{theme.FontStyle.FontWeight.Regular}",
                "display:block");

            choiceGroupRules.AddCssStringSelector(".ms-ChoiceFieldGroup-flexContainer.flex-direction-row").AppendCssStyles(
                "display:flex",
                "flex-direction:row",
                "flex-wrap:wrap");
            #endregion
            return choiceGroupRules;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (string.IsNullOrWhiteSpace(this.Id))
                this.Id = this.Id = $"g{Guid.NewGuid()}";
        }

        private async Task OnChoiceOptionClicked(ChoiceGroupOptionClickedEventArgs choiceGroupOptionClickedEventArgs)
        {
            await this.ValueChanged.InvokeAsync((TItem)choiceGroupOptionClickedEventArgs.Item);
        }
    }
}