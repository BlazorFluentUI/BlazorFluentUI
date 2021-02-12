using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
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

        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
        //{
        //    var choiceGroupOptionRules = new HashSet<IRule>();
        //    #region Root
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField").AppendCssStyles(
        //        $"font-size:{theme.FontStyle.FontSize.Medium}",
        //        $"font-weight:{theme.FontStyle.FontWeight.Regular}",
        //        "display:flex",
        //        "align-items:center",
        //        "box-sizing:border-box",
        //        $"color:var(--semanticTextColors.BodyText}",
        //        "min-height:26px",
        //        "border:none",
        //        "position:relative",
        //        "margin-top:8px");
        //    #endregion

        //    #region Label
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceFieldLabel").AppendCssStyles(
        //        $"color: {theme.Palette.NeutralDark}",
        //        "display:inline-block");

        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField:not(.custom-content) .ms-ChoiceFieldLabel").AppendCssStyles(
        //        "padding-left:26px");

        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField.custom-content").AppendCssStyles(
        //        "display:inline-flex",
        //        "font-size:0",
        //        "margin:0 4px 4px 0",
        //        "padding-left:0",
        //        $"background-color:{theme.Palette.NeutralLighter}",
        //        "height:100%");

        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceFieldLabel:before").AppendCssStyles(
        //        $"color: var(--semanticColors.InputBorderHovered}",
        //        $"border-color: var(--semanticColors.InputBorderHovered}");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-checked .ms-ChoiceFieldLabel:before").AppendCssStyles(
        //        $"color: {theme.Palette.ThemeDark}",
        //        $"border-color: {theme.Palette.ThemeDark}");

        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField:not(.is-checked):not(.custom-content) .ms-ChoiceFieldLabel:after").AppendCssStyles(
        //        "content:''",
        //        "transition-property:background-color",
        //        "left:5px",
        //        "top:5px",
        //        "width:10px",
        //        "height:10px",
        //        $"background-color:{theme.Palette.NeutralSecondary}");

        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField.is-checked .ms-ChoiceFieldLabel:after").AppendCssStyles(
        //        $"border-color: {theme.Palette.ThemeDark}");

        //    #endregion

        //    #region ChoiceFieldWrapper
        //    // TODO: flesh this out
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField-wrapper:focus").AppendCssStyles(
        //        "");
        //    #endregion

        //    #region Input
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField-input").AppendCssStyles(
        //        "position:absolute",
        //        "opacity:0",
        //        "top:0px",
        //        "right:0px",
        //        "width:100%",
        //        "height:100%",
        //        "margin:0");
        //    #endregion

        //    #region Field
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField-field").AppendCssStyles(
        //         "display:inline-block",
        //         "cursor:pointer",
        //         "margin-top:0",
        //         "position:relative",
        //         "vertical-align:top",
        //         "user-select:none",
        //         "min-height:20px");

        //    this.AddFieldHoverOrFocusStyles(choiceGroupOptionRules, theme, "hover");
        //    this.AddFieldHoverOrFocusStyles(choiceGroupOptionRules, theme, "focus");

        //    choiceGroupOptionRules.AddCssStringSelector(".custom-content .ms-ChoiceField-field").AppendCssStyles(
        //        "box-sizing:content-box",
        //        "cursor:pointer",
        //        "padding-top:22px",
        //        "margin:0",
        //        "text-align:center",
        //        "transition-property:all",
        //        $"transition-duration:{_transitionDuration}",
        //        $"transition-timing-function:ease",
        //        "border: 1px solid transparent",
        //        "justify-content:center",
        //        "align-items:center",
        //        "display:flex",
        //        "flex-direction:column");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-checked.custom-content .ms-ChoiceField-field").AppendCssStyles(
        //       $"border-color:var(--semanticColors.InputBackgroundChecked}");

        //    this.AddFieldWithCustomContentRules(choiceGroupOptionRules, theme, "hover");
        //    this.AddFieldWithCustomContentRules(choiceGroupOptionRules, theme, "focus");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-disabled .ms-ChoiceField-field").AppendCssStyles(
        //        "cursor:default");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-disabled .ms-ChoiceFieldLabel").AppendCssStyles(
        //        $"color:var(--semanticTextColors.DisabledBodyText}");

        //    choiceGroupOptionRules.AddCssClassSelector($".is-disabled {CommonStyles.HighContrastSelector}").AppendCssStyles(
        //        "color:GrayText");

        //    choiceGroupOptionRules.AddCssClassSelector($".is-checked .is-disabled .ms-ChoiceField").AppendCssStyles(
        //       $"border-color:{theme.Palette.NeutralLighter}");

        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField-labelWrapper").AppendCssStyles(
        //        $"font-size:{theme.FontStyle.FontSize.Medium}",
        //        $"font-weight:{theme.FontStyle.FontWeight.Regular}");

        //    #endregion

        //    #region OuterCircle
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField-field:before").AppendCssStyles(
        //        "content:''",
        //        "display:inline-block",
        //        $"background-color:var(--semanticColors.BodyBackground}",
        //        "border-width:1px",
        //        "border-style:solid",
        //        $"border-color:{theme.Palette.NeutralPrimary}",
        //        $"width:{_choiceFieldSize}px",
        //        $"height:{_choiceFieldSize}px",
        //        "font-weight:normal",
        //        "position:absolute",
        //        "top:0",
        //        "left:0",
        //        "box-sizing:border-box",
        //        "transition-property:border-color",
        //        $"transition-duration:{_transitionDuration}",
        //        $"transition-timing-function:{_transitionTimingFunction}",
        //        "border-radius:50%");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-disabled .ms-ChoiceField-field:before").AppendCssStyles(
        //        $"border-color:var(--semanticTextColors.DisabledBodySubtext}");

        //    choiceGroupOptionRules.AddCssStringSelector($".is-disabled {CommonStyles.HighContrastSelector} .ms-ChoiceField-field:before").AppendCssStyles(
        //        "color:GrayText");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-checked .ms-ChoiceField-field:before").AppendCssStyles(
        //        $"border-color:var(--semanticColors.InputBackgroundChecked}");

        //    choiceGroupOptionRules.AddCssStringSelector(".custom-content .ms-ChoiceField-field:before").AppendCssStyles(
        //        $"top:{_radioButtonSpacing}px",
        //        $"right:{_radioButtonSpacing}px",
        //        "left:auto",
        //        "opacity:0");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-checked.custom-content .ms-ChoiceField-field:before").AppendCssStyles(
        //        "opacity:1");

        //    #endregion

        //    #region SelectionCircle
        //    choiceGroupOptionRules.AddCssStringSelector(".ms-ChoiceField-field:after").AppendCssStyles(
        //        "content:''",
        //        "width:0",
        //        "height:0",
        //        "border-radius:50%",
        //        "position: absolute",
        //        $"left:{_choiceFieldSize / 2}px",
        //        "right:0",
        //        "transition-property:border-width",
        //        $"transition-duration:{_transitionDuration}",
        //        $"transition-timing-function:{_transitionTimingFunction}",
        //        "box-sizing:border-box");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-checked .ms-ChoiceField-field:after").AppendCssStyles(
        //        "border-width:5px",
        //        "border-style:solid",
        //        $"border-color:var(--semanticColors.InputBackgroundChecked}",
        //        "left:5px",
        //        "top:5px",
        //        "width:10px",
        //        "height:10px");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-checked.custom-content .ms-ChoiceField-field:after").AppendCssStyles(
        //        $"top:{_radioButtonSpacing + _radioButtonInnerSize}px",
        //        $"right:{_radioButtonSpacing + _radioButtonInnerSize}px",
        //        "left:auto");
        //    #endregion

        //    #region Inner Field
        //    choiceGroupOptionRules.AddCssStringSelector(".custom-content .ms-ChoiceField-innerField").AppendCssStyles(
        //        "position:relative",
        //        "display:inline-block",
        //        "padding-left:30px",
        //        "padding-right:30px");

        //    choiceGroupOptionRules.AddCssStringSelector(".is-disabled.custom-content .ms-ChoiceField-innerField").AppendCssStyles(
        //        "opacity:0.25");

        //    choiceGroupOptionRules.AddCssStringSelector($".is-disabled.custom-content {CommonStyles.HighContrastSelector} .ms-ChoiceField-innerField").AppendCssStyles(
        //       "opacity:0.25");
        //    #endregion

        //    #region ImageWrapper
        //    this.AddCustomContentWrapperStyle(choiceGroupOptionRules, theme);
        //    #endregion

        //    #region LabelWrapper
        //    choiceGroupOptionRules.AddCssStringSelector($".custom-content .ms-ChoiceField-labelWrapper").AppendCssStyles(
        //        $"font-size:{theme.FontStyle.FontSize.Medium}",
        //        $"font-weight:{theme.FontStyle.FontWeight.Regular}",
        //        "display:flex",
        //        "position:relative",
        //        "margin:4px 8px 2px 8px",
        //        $"height:{_labelWrapperHeight}px",
        //        $"line-height:{_labelWrapperLineHeight}px",
        //        "overflow:hidden",
        //        "white-space:pre-wrap");
        //    #endregion

        //    return choiceGroupOptionRules;
        //}

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