using BlazorFluentUI;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI.BFUChoiceGroup
{
    public partial class BFUChoiceGroup<TItem> : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter] public IList<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> OptionTemplate { get; set; }
        [Parameter] public TItem Value { get; set; }
        [Parameter] public EventCallback<TItem> ValueChanged { get; set; }
        [Parameter] public FlexDirection ItemAlignment { get; set; } = FlexDirection.Column;

        public ICollection<Rule> CreateGlobalCss(ITheme theme)
        {
            var choiceGroupRules = new HashSet<Rule>();
            #region Root
            choiceGroupRules.AddCssStringSelector("ms-ChoiceFieldGroup").AppendCssStyles(
                $"font-size:{theme.FontStyle.FontSize.Medium}",
                $"font-weight:{theme.FontStyle.FontWeight.Regular}",
                "display:block");

            choiceGroupRules.AddCssStringSelector("ms-ChoiceFieldGroup-flexContainer").AppendCssStyles(
                "display:flex",
                "flex-direction:row",
                "flex-wrap:wrap");
            #endregion
            return choiceGroupRules;
        }

        private async Task OnChoiceOptionClicked(ChoiceGroupOptionClickedEventArgs choiceGroupOptionClickedEventArgs)
        {
            await this.ValueChanged.InvokeAsync((TItem)choiceGroupOptionClickedEventArgs.Item);
        }
    }
}