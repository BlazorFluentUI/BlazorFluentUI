using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI.BFUChoiceGroup
{
    public partial class BFUChoiceGroupOption<TItem> : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        private const int _choiceFieldSize = 20;
        private const string _transitionDuration = "200ms";
        private const string _transitionTimingFunction = "cubic-bezier(.4, 0, .23, 1)";
        private const int _labelWrapperLineHeight = 15;
        private const int _labelWrapperHeight = _labelWrapperLineHeight * 2 + 2;

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
        private Rule _choiceGroupOptionLabelRule = new Rule
        {
            Selector = new ClassSelector { SelectorName = "ms-ChoiceFieldLabel", PseudoElement = PseudoElements.After },
            Properties = new CssString(),
        };

        private bool _isSelected = false;

        private static int _currentAutoId = 0;

        public ICollection<Rule> CreateGlobalCss(ITheme theme)
        {
            var choiceGroupOptionRules = new HashSet<Rule>();
            #region Root
            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField").AppendCssStyles(
                $"font-size:{Theme.FontStyle.FontSize.Medium}",
                $"font-weight:{Theme.FontStyle.FontWeight.Regular}",
                "display:flex",
                "align-items:center",
                "box-sizing:border-box",
                $"color:{Theme.SemanticTextColors.BodyText}",
                "min-height:26px",
                "border:none",
                "position:relative",
                "margin-top:8px");
            #endregion

            #region Label
            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceFieldLabel").AppendCssStyles(
                $"color: {Theme.Palette.NeutralDark}",
                "display:inline-block",
                "padding-left:26px");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceFieldLabel:before").AppendCssStyles(
                $"color: {Theme.SemanticColors.InputBorderHovered}");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceFieldLabel.is-checked:before").AppendCssStyles(
                $"color: {Theme.Palette.ThemeDark}");

            #endregion

            #region Input
            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-input").AppendCssStyles(
                "position:absolute",
                "opacity:0",
                "top:0px",
                "right:0px",
                "width:100%",
                "height:100%",
                "margin:0");
            #endregion

            #region Field
            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-field").AppendCssStyles(
                 "display:inline-block",
                 "cursor:pointer",
                 "margin-top:0",
                 "position:relative",
                 "vertical-align:top",
                 "user-select:none",
                 "min-height:20px");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-field:before").AppendCssStyles(
                "content:''",
                "display:inline-block",
                $"background-color:{theme.SemanticColors.BodyBackground}",
                "border-width:1px",
                "border-style:solid",
                $"border-color:{theme.Palette.NeutralPrimary}",
                $"width:{_choiceFieldSize}px",
                $"height:{_choiceFieldSize}px",
                "font-weight:normal",
                "position:absolute",
                "top:0",
                "left:0",
                "box-sizing:border-box",
                "transition-property:border-color",
                $"transition-duration:{_transitionDuration}",
                $"transition-timing-function:{_transitionTimingFunction}",
                "border-radius:50%");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-field.is-checked:before").AppendCssStyles(
                $"border-color:{theme.SemanticColors.InputBackgroundChecked}");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-field:after").AppendCssStyles(
                "content:''",
                "width:0",
                "height:0",
                "border-radius:50%",
                "position: absolute",
                $"left:{_choiceFieldSize / 2}px",
                "right:0",
                "transition-property:border-width",
                $"transition-duration:{_transitionDuration}",
                $"transition-timing-function:{_transitionTimingFunction}",
                "box-sizing:border-box");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-field.is-checked:after").AppendCssStyles(
                "border-width:5px",
                "border-style:solid",
                $"border-color:{Theme.SemanticColors.InputBackgroundChecked}",
                "left:5px",
                "top:5px",
                "width:10px",
                "height:10px");

            choiceGroupOptionRules.AddCssStringSelector("ms-ChoiceField-labelWrapper").AppendCssStyles(
                $"font-size:{Theme.FontStyle.FontSize.Medium}",
                $"font-weight:{Theme.FontStyle.FontWeight.Regular}");

            #endregion

            return choiceGroupOptionRules;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            this.LocalCssRules.Add(this._choiceGroupOptionLabelRule);
        }

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

            this.CreateLocalCss();
        }

        protected override void OnThemeChanged()
        {
            base.OnThemeChanged();
            this.CreateLocalCss();
        }

        private void CreateLocalCss()
        {
            #region Root
            if (string.IsNullOrWhiteSpace(this.Icon) && this.OptionTemplate == null && !this._isSelected)
            {
                _choiceGroupOptionLabelRule.SetCssStyles(
                    "content:''",
                    "transition-property:background-color",
                    "left:5px",
                    "top:5px",
                    "width:10px",
                    "height:10px",
                    $"background-color:{Theme.Palette.NeutralSecondary}");
            }
            else if (this._isSelected)
            {
                _choiceGroupOptionLabelRule.SetCssStyles(
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