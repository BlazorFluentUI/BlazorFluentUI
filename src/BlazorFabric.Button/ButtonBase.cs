using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlazorFabric.Button
{
    public class ButtonBase : ComponentBase
    {
        public ElementRef ButtonRef { get; set; }

        /// <summary>
        ///  If provided, this component will be rendered as an anchor.
        /// </summary>
        [Parameter]
        protected string Href { get; set; }

        /// <summary>
        ///  Changes the visual presentation of the button to be emphasized (if defined)
        /// </summary>
        /// <value>
        /// false
        /// </value>
        [Parameter]
        public bool Primary { get; set; }

        /// <summary>
        /// Whether the button is disabled
        /// </summary>
        ///
        [Parameter]
        protected bool Disabled { get; set; }


        /// <summary>
        ///  Whether the button can have focus in disabled mode
        /// </summary>
        ///
        [Parameter]
        protected bool AllowDisabledFocus { get; set; }


        /// <summary>
        /// If set to true and if this is a splitButton (split == true) then the primary action of a split button is disabled.
        /// </summary>
        [Parameter]
        protected bool PrimaryDisabled { get; set; }


        ///// <summary>
        ///// Theme provided by HOC.
        ///// </summary>
        //[Parameter]
        //protected Theme Theme { get; set; }

        /// <summary>
        ///  Whether the button is checked
        /// </summary>
        [Parameter]
        protected bool? Checked { get; set; }

        [Parameter]
        protected EventCallback<bool> CheckedChanged { get; set; }

        /// <summary>
        /// The aria label of the button for the benefit of screen readers.
        /// </summary>
        ///
        [Parameter]
        protected string AriaLabel { get; set; }

        /// <summary>
        ///  Detailed description of the button for the benefit of screen readers.
        ///
        ///  Besides the compound button, other button types will need more information provided to screen reader.
        /// </summary>
        [Parameter]
        protected string AriaDescripton { get; set; }

        /// <summary>
        /// If provided and is true it adds an 'aria-hidden' attribute instructing screen readers to ignore the element.
        /// </summary>
        [Parameter]
        protected bool AriaHidden { get; set; }

        /// <summary>
        /// Text to render button label. If text is supplied, it will override any string in button children. Other children components will be passed through after the text.
        /// </summary>
        [Parameter]
        protected string Text { get; set; }

        [Parameter]
        protected string SecondaryText { get; set; }

        [Parameter]
        protected bool Toggle { get; set; }

        [Parameter]
        protected bool Split { get; set; }

        [Parameter]
        protected Func<ButtonBase, UIMouseEventArgs, Task> Clicked { get; set; }

        [Parameter]
        protected ICommand Command { get; set; }

        [Parameter]
        protected object CommandParameter { get; set; }


        private ICommand command;
        protected bool commandDisabled = false;

        protected bool isChecked = false;

        protected override Task OnParametersSetAsync()
        {
            if (Command == null && command != null)
            {
                command.CanExecuteChanged -= Command_CanExecuteChanged;
                command = null;
            }
            if (Command != null && Command != command)
            {
                if (command != null)
                {
                    command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                command = Command;
                Command.CanExecuteChanged += Command_CanExecuteChanged;
            }

            if (this.Checked.HasValue)
            {
                isChecked = this.Checked.Value;
            }

            return base.OnParametersSetAsync();
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            commandDisabled = Command.CanExecute(CommandParameter);
        }

        protected async Task OnClick(UIMouseEventArgs args)
        {
            if (Toggle)
            {
                this.isChecked = !this.isChecked;
                await this.CheckedChanged.InvokeAsync(this.isChecked);
            }

            if (Clicked != null)
            {
                await Clicked.Invoke(this, args);
            }
            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }


        public void Focus()
        {

        }

        public void DismissMenu()
        {

        }

        public void OpenMenu(bool shouldFocusOnContainer, bool shouldFocusOnMount)
        {

        }

      

        //protected override void ApplyStyles()
        //{

        //    var theme = ThemingEngine.DefaultTheme.Value;

        //    var isDisabled = this.Disabled || this.commandDisabled;

        //    var baseStyles = this.GetBaseStyles();

        //    // Root Styles
        //    var rootStyles = new List<Style>();

        //    var name = new Style();
        //    name = "ms-Button";
        //    rootStyles.Add(name);

        //    rootStyles.Add(baseStyles.Root);

        //    if (this.Checked)
        //    {
        //        var checkedStyle = new Style();
        //        checkedStyle = "is-Checked";
        //        rootStyles.Add(checkedStyle);
        //        rootStyles.Add(baseStyles.RootChecked);
        //    }
        //    //IMPLEMENT isExpanded (part of split button)


        //    if (isDisabled)
        //    {
        //        var disabledStyle = new Style();
        //        disabledStyle = "is-disabled";
        //        rootStyles.Add(disabledStyle);
        //        rootStyles.Add(baseStyles.RootDisabled);
        //    }
        //    else
        //    {
        //        if (!this.Checked /* && !isExpanded */)
        //        {

        //            var style = new Style();
        //            style.Selectors.Add(":hover", baseStyles.RootHovered);
        //            style.Selectors.Add($":hover .{ButtonGlobalClassNames.msButtonLabel}", baseStyles.LabelHovered);
        //            style.Selectors.Add($":hover .{ButtonGlobalClassNames.msButtonIcon}", baseStyles.IconHovered);
        //            style.Selectors.Add($":hover .{ButtonGlobalClassNames.msButtonDescription}", baseStyles.DescriptionHovered);
        //            style.Selectors.Add($":hover .{ButtonGlobalClassNames.msButtonMenuIcon}", baseStyles.MenuIconHovered);
        //            style.Selectors.Add(":focus", baseStyles.RootFocused);
        //            style.Selectors.Add(":active", baseStyles.RootPressed);
        //            style.Selectors.Add($":active .{ButtonGlobalClassNames.msButtonIcon}", baseStyles.IconPressed);
        //            style.Selectors.Add($":active .{ButtonGlobalClassNames.msButtonDescription}", baseStyles.DescriptionPressed);
        //            style.Selectors.Add($":active .{ButtonGlobalClassNames.msButtonMenuIcon}", baseStyles.MenuIconPressed);
        //            rootStyles.Add(style);
        //        }
        //    }
        //    if (isDisabled && this.Checked)
        //    {
        //        rootStyles.Add(baseStyles.RootCheckedDisabled);
        //    }
        //    if (!isDisabled && this.Checked)
        //    {
        //        var style = new Style();
        //        style.Selectors.Add(":hover", baseStyles.RootCheckedHovered);
        //        style.Selectors.Add(":active", baseStyles.RootCheckedPressed);
        //        rootStyles.Add(style);
        //    }

        //    // Other Styles

        //    var nameFlexContainerStyle = new Style();
        //    nameFlexContainerStyle = "ms-Button-flexContainer";

        //    var nameTextContainerStyle = new Style();
        //    nameTextContainerStyle = "ms-Button-textContainer";

        //    // Icon Styles
        //    var nameIconStyle = new Style("ms-Button-icon");
        //    // TO-DO ... finish icon styles on button


        //    //Label
        //    var nameLabelStyle = new Style("ms-Button-label");

        //    //MenuIcon
        //    //TODO

        //    //Description
        //    var nameDescriptionStyle = new Style("ms-Button-description");


        //    this.baseButtonStyles = new ButtonStyles
        //    {
        //        Root = new Style(rootStyles.ToArray()),
        //        FlexContainer = new Style(new[] { nameFlexContainerStyle, baseStyles.FlexContainer }),
        //        TextContainer = new Style(new[] { nameTextContainerStyle, baseStyles.TextContainer }),
        //        //Icon
        //        Label = new Style(new[] { nameLabelStyle, baseStyles.Label, this.Checked ? baseStyles.LabelChecked : new Style(), this.Disabled ? baseStyles.LabelDisabled : new Style() }),
        //        //MenuIcon
        //        Description = new Style(new[] { nameDescriptionStyle, baseStyles.Description, this.Checked ? baseStyles.DescriptionChecked : new Style(), this.Disabled ? baseStyles.DescriptionDisabled : new Style() })
        //        //ScreenReader
        //    };




        //}

        //private ButtonStyles GetBaseStyles()
        //{
        //    var theme = ThemingEngine.DefaultTheme.Value;

        //    var themeStyles = this.GetThemeStyle();


        //    var fontRootStyle = theme.Fonts.Medium;
        //    var baseRootStyle = new Style()
        //    {
        //        //getFocusStyles() // need to implement
        //        BoxSizing = BoxSizing.BorderBox,
        //        BorderWidth = this.Primary ? "0px" : "1px",
        //        BorderStyle = "solid",
        //        //Border = "1px solid " + theme.SemanticColors.ButtonBorder,
        //        UserSelect = UserSelect.None,
        //        Display = "inline-block",
        //        TextDecoration = "none",
        //        TextAlign = "center",
        //        Cursor = "pointer",
        //        VerticalAlign = "top",
        //        Padding = "0 16px"
        //    };
        //    baseRootStyle.Selectors.Add(":active > *", new Style
        //    {
        //        Position = Position.Relative,
        //        Left = 0,
        //        Top = 0
        //    });

        //    var baseRootDisabledStyle = new Style
        //    {
        //        BackgroundColor = theme.SemanticColors.DisabledBackground,
        //        Color = theme.SemanticColors.DisabledText,
        //        Cursor = "default",
        //        PointerEvents = "none"
        //    };
        //    baseRootStyle.Selectors.Add(":hover", new Style() { Outline = 0 });
        //    baseRootStyle.Selectors.Add(":focus", new Style() { Outline = 0 });
        //    baseRootStyle.Selectors.Add(CommonStyles.HighContrastSelector, new Style() { Color = "grayText", BorderColor = "grayText" });

        //    var iconDisabledStyle = new Style { Color = theme.SemanticColors.DisabledText };
        //    var menuIconDisabledStyle = new Style { Color = theme.SemanticColors.DisabledText };

        //    var flexContainerStyle = new Style
        //    {
        //        Display = "flex",
        //        Height = "100%",
        //        FlexWrap = FlexWrap.Nowrap,
        //        JustifyContent = JustifyContent.Center,
        //        AlignItems = AlignItems.Center
        //    };

        //    var textContainerStyle = new Style { FlexGrow = 1 };

        //    var iconStyle = new Style
        //    {
        //        FontSize = Styling.Fonts.FontSizes.DefaultFontSizes.Icon,
        //        Margin = "0px 4px",
        //        Height = "16px",
        //        LineHeight = "16px",
        //        TextAlign = "center",
        //        VerticalAlign = "middle",
        //        FlexShrink = 0
        //    };

        //    var menuIconStyle = new Style { FontSize = Styling.Fonts.FontSizes.DefaultFontSizes.Small };

        //    var labelStyle = new Style
        //    {
        //        Margin = "0px 4px",
        //        LineHeight = "100%"
        //    };

        //    var screenReaderTextStyle = new Style
        //    {
        //        Position = Position.Absolute,
        //        Width = 1,
        //        Height = 1,
        //        Margin = -1,
        //        Padding = 0,
        //        Border = 0,
        //        Overflow = Overflow.Hidden
        //    };


        //    return new ButtonStyles()
        //    {
        //        Root = new Style(new[] { themeStyles.Root, fontRootStyle, baseRootStyle }),
        //        RootDisabled = new Style(new[] { themeStyles.RootDisabled, baseRootDisabledStyle }),
        //        RootHovered = themeStyles.RootHovered,
        //        RootPressed = themeStyles.RootPressed,
        //        RootExpanded = themeStyles.RootExpanded,
        //        RootChecked = themeStyles.RootChecked,
        //        RootCheckedHovered = themeStyles.RootCheckedHovered,
        //        IconDisabled = iconDisabledStyle,
        //        MenuIconDisabled = menuIconDisabledStyle,
        //        FlexContainer = flexContainerStyle,
        //        TextContainer = textContainerStyle,
        //        Icon = iconStyle,
        //        MenuIcon = new Style(new[] { iconStyle, menuIconStyle }),
        //        Label = labelStyle,
        //        ScreenReaderText = screenReaderTextStyle
        //    };

        //}




        //protected ButtonStyles GetThemeStyle()
        //{
        //    var theme = ThemingEngine.DefaultTheme.Value;
        //    var buttonBackground = theme.SemanticColors.ButtonBackground;
        //    var buttonBackgroundChecked = theme.SemanticColors.ButtonBackgroundChecked;
        //    var buttonBackgroundHovered = theme.SemanticColors.ButtonBackgroundHovered;

        //    var buttonText = theme.SemanticColors.ButtonText;
        //    var buttonTextHovered = theme.SemanticColors.ButtonTextHovered;
        //    var buttonTextChecked = theme.SemanticColors.ButtonTextChecked;
        //    var buttonTextCheckedHovered = theme.SemanticColors.ButtonTextCheckedHovered;


        //    if (!this.Primary)
        //    {
        //        var variantName = new Style("ms-Button--default");
        //        var rootStyle = new Style
        //        {
        //            Color = buttonText,
        //            BorderRadius = theme.Effects.RoundedCorner2,
        //            BackgroundColor = theme.Palette.White,
        //            Border = $"1px solid {theme.Palette.NeutralSecondaryAlt}"
        //            //getFocusStyle(theme).... TO DO 
        //        };
        //        var rootHoveredStyle = new Style
        //        {
        //            BackgroundColor = theme.Palette.NeutralLighter,
        //            Color = buttonTextHovered
        //        };
        //        rootHoveredStyle.Selectors.Add(CommonStyles.HighContrastSelector, new Style { BorderColor = "Highlight", Color = "Highlight" });
        //        rootHoveredStyle.Selectors.Add(".ms-Button--primary", new Style { BackgroundColor = theme.Palette.ThemeDarkAlt });

        //        var rootPressedStyle = new Style { BackgroundColor = theme.Palette.NeutralLight, Color = buttonTextChecked };
        //        var rootExpandedStyle = new Style { BackgroundColor = buttonBackgroundChecked, Color = buttonTextChecked };
        //        var rootCheckedStyle = new Style { BackgroundColor = theme.Palette.NeutralLight, Color = buttonTextChecked };
        //        var rootDisabledStyle = new Style { BackgroundColor = theme.Palette.NeutralLighter, BorderColor = theme.Palette.NeutralLighter };

        //        var rootCheckedHoveredStyle = new Style { BackgroundColor = theme.Palette.NeutralLight };

        //        // TO-DO :  split button styles

        //        return new ButtonStyles
        //        {
        //            Root = new Style(new[] { variantName, rootStyle }),
        //            RootHovered = rootHoveredStyle,
        //            RootPressed = rootPressedStyle,
        //            RootExpanded = rootExpandedStyle,
        //            RootChecked = rootCheckedStyle,
        //            RootCheckedHovered = rootCheckedHoveredStyle,
        //            RootDisabled = rootDisabledStyle
        //        };

        //    }
        //    else
        //    {
        //        var variantName = new Style("ms-Button--primary");
        //        var rootStyle = new Style
        //        {
        //            Color = theme.Palette.White,
        //            BorderRadius = theme.Effects.RoundedCorner2,
        //            BackgroundColor = theme.Palette.ThemePrimary,
        //            Border = "none"
        //            //getFocusStyle(theme).... TO DO 
        //        };
        //        rootStyle.Selectors.Add(CommonStyles.HighContrastSelector, new Style
        //        {
        //            Color = "Window",
        //            BackgroundColor = "WindowText",
        //            MsHighContrastAdjust = "none"
        //        });
        //        var rootHoveredStyle = new Style
        //        {
        //            BackgroundColor = theme.Palette.ThemeDarkAlt,
        //            Color = theme.Palette.White
        //        };
        //        rootHoveredStyle.Selectors.Add(CommonStyles.HighContrastSelector, new Style { BackgroundColor = "Highlight", Color = "Window" });

        //        var rootPressedStyle = new Style { BackgroundColor = theme.Palette.ThemeDark, Color = theme.Palette.White };
        //        rootPressedStyle.Selectors.Add(CommonStyles.HighContrastSelector, new Style
        //        {
        //            Color = "Window",
        //            BackgroundColor = "WindowText",
        //            MsHighContrastAdjust = "none"
        //        });
        //        var rootExpandedStyle = new Style { BackgroundColor = theme.Palette.ThemeDark, Color = theme.Palette.White};
        //        var rootCheckedStyle = new Style { BackgroundColor = theme.Palette.ThemeDark, Color = theme.Palette.White };
        //        var rootCheckedHoveredStyle = new Style { BackgroundColor = theme.Palette.ThemePrimary, Color = theme.Palette.White };
        //        var rootDisabledStyle = new Style();// { BackgroundColor = theme.Palette.NeutralLighter, BorderColor = theme.Palette.NeutralLighter };
        //        rootDisabledStyle.Selectors.Add(CommonStyles.HighContrastSelector, new Style
        //        {
        //            Color = "GrayText",
        //            BackgroundColor = "Window",
        //            BorderColor= "GrayText"
        //        });

        //        // TO-DO :  split button styles

        //        return new ButtonStyles
        //        {
        //            Root = new Style(new[] { variantName, rootStyle }),
        //            RootHovered = rootHoveredStyle,
        //            RootPressed = rootPressedStyle,
        //            RootExpanded = rootExpandedStyle,
        //            RootChecked = rootCheckedStyle,
        //            RootCheckedHovered = rootCheckedHoveredStyle,
        //            RootDisabled = rootDisabledStyle
        //        };

        //    }
        //}

    }
}
