using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class ChoiceGroupOption<TItem> : FluentUIComponentBase
    {
        private const int _choiceFieldSize = 20;
        private const string _transitionDuration = "200ms";
        private const string _transitionTimingFunction = "cubic-bezier(.4, 0, .23, 1)";
        private const int _labelWrapperLineHeight = 15;
        private const int _labelWrapperHeight = _labelWrapperLineHeight * 2 + 2;
        private const int _radioButtonSpacing = 3;
        private const int _radioButtonInnerSize = 5;

        [Parameter] public bool Focused { get; set; }
        [Parameter] public bool Required { get; set; } = false;
        [Parameter] public string Label { get; set; }
        [Parameter] public bool IsDisabled { get; set; } = false;
        [Parameter] public bool IsVisible { get; set; } = true;
        [Parameter] public TItem Item { get; set; }
        [Parameter] public RenderFragment<TItem> OptionTemplate { get; set; }
        [Parameter] public EventCallback<ChoiceGroupOptionClickedEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback<ChoiceGroupOptionFocusEventArgs> OnFocus { get; set; }
        [Parameter] public EventCallback<ChoiceGroupOptionFocusEventArgs> OnBlur { get; set; }
        [Parameter] public string Id { get; set; }
        [CascadingParameter] protected ChoiceGroup<TItem> ChoiceGroup { get; set; }

        private bool _isSelected = false;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Item is string itemAsString)
            {
                Label = itemAsString;
            }
            else if (Item is IChoiceGroupOption choiceGroupOption)
            {
                Label = choiceGroupOption.Label;
                IsVisible = choiceGroupOption.IsVisible;
                IsDisabled = choiceGroupOption.IsDisabled;
            }

            _isSelected = Equals(ChoiceGroup.Value, Item);

            if (string.IsNullOrWhiteSpace(Id))
                Id = Id = $"g{Guid.NewGuid()}";
        }

        //private void AddFieldHoverOrFocusStyles(HashSet<IRule> rules, ITheme theme, string pseudoSelector)
        //{
        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled) .ms-ChoiceField-field:{pseudoSelector} .ms-ChoiceFieldLabel").AppendCssStyles(
        //       $"color:{theme.Palette.NeutralDark}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled) .is-checked .ms-ChoiceField-field:{pseudoSelector}:before").AppendCssStyles(
        //        $"border-color:{theme.Palette.ThemeDark}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled):not(.is-checked) .ms-ChoiceField-field:{pseudoSelector}:before").AppendCssStyles(
        //        $"border-color:var(--semanticColors.InputBorderHovered}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled):not(.is-checked):not(.custom-content) .ms-ChoiceField-field:{pseudoSelector}:after").AppendCssStyles(
        //        "content:''",
        //        "transition-property:background-color",
        //        "left:5px",
        //        "top:5px",
        //        "width:10px",
        //        "height:10px",
        //        $"background-color:{theme.Palette.NeutralSecondary}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled) .is-checked .ms-ChoiceField-field:{pseudoSelector}").AppendCssStyles(
        //        $"border-color:{theme.Palette.ThemeDark}");
        //}

        //private void AddFieldWithCustomContentRules(HashSet<IRule> rules, ITheme theme, string pseudoSelector)
        //{
        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled).is-checked.custom-content .ms-ChoiceField-field:{pseudoSelector}").AppendCssStyles(
        //      $"border-color:{theme.Palette.ThemeDark}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled):not(.is-checked).custom-content .ms-ChoiceField-field:{pseudoSelector}").AppendCssStyles(
        //      $"border-color:var(--semanticColors.InputBorderHovered}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled) .is-checked.custom-content .ms-ChoiceField-field:{pseudoSelector}:before").AppendCssStyles(
        //        "opacity:1",
        //        $"border-color:{theme.Palette.ThemeDark}");

        //    rules.AddCssStringSelector($".ms-ChoiceField:not(.is-disabled):not(.is-checked).custom-content .ms-ChoiceField-field:{pseudoSelector}:before").AppendCssStyles(
        //        "opacity:1",
        //        $"border-color:var(--semanticColors.InputBorderHovered}");
        //}

        //private void AddCustomContentWrapperStyle(HashSet<IRule> rules, ITheme theme)
        //{
        //    rules.AddCssStringSelector(".custom-content .ms-ChoiceField-customContentWrapper").AppendCssStyles(
        //        "padding-bottom:2px",
        //        "transition-property:opacity",
        //        $"transition-duration:{_transitionDuration}",
        //        "transition-timing-function:ease");

        //    rules.AddCssStringSelector(".ms-ChoiceField-customContentWrapper .ms-Image").AppendCssStyles(
        //        "display:inline-block",
        //        "border-style:none");
        //}

        private async Task OnOptionClick(MouseEventArgs mouseEventArgs)
        {
            if (!IsDisabled)
                await OnClick.InvokeAsync(new ChoiceGroupOptionClickedEventArgs { Item = Item, MouseEventArgs = mouseEventArgs });
        }

        private async Task OnOptionFocus(FocusEventArgs focusEventArgs)
        {
            await OnFocus.InvokeAsync(new ChoiceGroupOptionFocusEventArgs { Item = Item, FocusEventArgs = focusEventArgs });
        }

        private async Task OnOptionBlur(FocusEventArgs focusEventArgs)
        {
            await OnBlur.InvokeAsync(new ChoiceGroupOptionFocusEventArgs { Item = Item, FocusEventArgs = focusEventArgs });
        }
    }

    public class ChoiceGroupOptionClickedEventArgs : EventArgs
    {
        public MouseEventArgs MouseEventArgs { get; set; }

        public object Item { get; set; }
    }

    public class ChoiceGroupOptionFocusEventArgs : EventArgs
    {
        public FocusEventArgs FocusEventArgs { get; set; }

        public object Item { get; set; }
    }
}