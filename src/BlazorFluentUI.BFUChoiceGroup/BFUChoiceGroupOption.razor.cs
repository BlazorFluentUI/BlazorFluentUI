using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI.BFUChoiceGroup
{
    public partial class BFUChoiceGroupOption<TItem> : BFUComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool IsDisabled { get; set; } = false;
        [Parameter] public bool IsVisible { get; set; } = true;
        [Parameter] public TItem Item { get; set; }
        [Parameter] public RenderFragment<TItem> OptionTemplate { get; set; }
        [Parameter] public string Icon { get; set; }
        [Parameter] public EventCallback<ChoiceGroupOptionClickedEventArgs> OnClick { get; set; }
        [Parameter] public string Id { get; set; }
        [CascadingParameter] protected BFUChoiceGroup<TItem> ChoiceGroup { get; set; }

        private ICollection<IRule> LocalCssRules { get; set; } = new List<IRule>();
        private bool _isSelected = false;

        private static int _currentAutoId = 0;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (this.Item is string itemAsString)
            {
                this.Label = itemAsString;
            }
            else if (this.Item is IChoiceGroupOption choiceGroupOption)
            {
                this.Label = choiceGroupOption.Text;
                this.IsVisible = choiceGroupOption.IsVisible;
                this.IsDisabled = choiceGroupOption.IsDisabled;
            }

            this._isSelected = Equals(this.ChoiceGroup.Value, this.Item);

            if (string.IsNullOrWhiteSpace(this.Id))
                this.Id = $"autoId_choiceGroupOption_{_currentAutoId++}";
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            this.CreateLocalCss();
        }

        private void CreateLocalCss()
        {
            #region Root
            LocalCssRules.AddCssClassSelector("ms-ChoiceField").AppendCssStyles(
                $"font-size:{Theme.FontStyle.FontSize.Medium}",
                $"font-weight:{Theme.FontStyle.FontWeight.Regular}",
                "display:flex",
                "align-items:center",
                "box-sizing:border-box",
                $"color:{Theme.SemanticTextColors.BodyText}",
                "min-height:26",
                "border:none",
                "position:relative",
                "margin-top:8");
            #endregion


            #region Label
            LocalCssRules.AddCssClassSelector("ms-ChoiceFieldLabel").AppendCssStyles(
                $"color: {Theme.Palette.NeutralDark}");

            LocalCssRules.AddCssClassSelector("ms-ChoiceFieldLabel:before").AppendCssStyles(
                $"color: {Theme.SemanticColors.InputBorderHovered}");

            LocalCssRules.AddCssClassSelector("ms-ChoiceFieldLabel.is-checked:before").AppendCssStyles(
                $"color: {Theme.Palette.ThemeDark}");

            if (string.IsNullOrWhiteSpace(this.Icon) && this.OptionTemplate == null && !this._isSelected)
            {
                LocalCssRules.AddCssClassSelector("ms-ChoiceFieldLabel:after").AppendCssStyles(
                    "content:''",
                    "transition-property:background-color",
                    "left:5",
                    "top:5",
                    "width:10",
                    "height:10",
                    $"background-color:{Theme.Palette.NeutralSecondary}");
            }
            else if (this._isSelected)
            {
                LocalCssRules.AddCssClassSelector("ms-ChoiceFieldLabel:after").AppendCssStyles(
                    $"border-color: {Theme.Palette.ThemeDark}");
            }
            #endregion
        }

        private async Task OnOptionClick(MouseEventArgs mouseEventArgs)
        {
            if (!this.IsDisabled)
                await this.OnClick.InvokeAsync(new ChoiceGroupOptionClickedEventArgs { Item = this.Item, MouseEventArgs = mouseEventArgs });
        }
    }

    public class ChoiceGroupOptionClickedEventArgs : EventArgs
    {
        public MouseEventArgs MouseEventArgs { get; set; }

        public object Item { get; set; }
    }
}