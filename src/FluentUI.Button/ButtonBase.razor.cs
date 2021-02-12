using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FluentUI.Style;

namespace FluentUI
{
    public partial class ButtonBase : ButtonParameters, IAsyncDisposable
    {
        public ButtonBase()
        {

        }

        public ElementReference ButtonRef { get; set; }

        //[Parameter] public RenderFragment ChildContent { get; set; }
        //[Parameter] public string Href { get; set; }
        //[Parameter] public bool Primary { get; set; }
        //[Parameter] public bool Disabled { get; set; }
        //[Parameter] public bool AllowDisabledFocus { get; set; }
        //[Parameter] public bool PrimaryDisabled { get; set; }
        //[Parameter] public bool? Checked { get; set; }
        ////[Parameter] public string AriaLabel { get; set; }
        //[Parameter] public string AriaDescripton { get; set; }
        ////[Parameter] public bool AriaHidden { get; set; }
        //[Parameter] public string Text { get; set; }
        //[Parameter] public bool Toggle { get; set; }
        //[Parameter] public bool Split { get; set; }
        //[Parameter] public string IconName { get; set; }
        //[Parameter] public bool HideChevron { get; set; }

        //[Parameter] public IEnumerable<object> MenuItems { get; set; }
        ////[Parameter] public RenderFragment ContextualMenuContent { get; set; }
        ////[Parameter] public RenderFragment ContextualMenuItemsSource { get; set; }
        ////[Parameter] public RenderFragment ContextualMenuItemTemplate { get; set; }

        //[Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        //[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        //[Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        //[Parameter] public ICommand Command { get; set; }
        //[Parameter] public object CommandParameter { get; set; }
        //[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnknownProperties { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; }

        protected bool showMenu = false;

        private ICommand command;
        protected bool commandDisabled = false;

        protected bool isChecked = false;

        internal bool contextMenuShown = false;

        internal bool isCompoundButton = false;
        internal bool isSplitButton = false;
        private object _registrationToken;

        private bool _menuShouldFocusOnMount = true;

        protected override Task OnParametersSetAsync()
        {
            showMenu = MenuItems != null;

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
                commandDisabled = !command.CanExecute(CommandParameter);
                Command.CanExecuteChanged += Command_CanExecuteChanged;
            }

            if (Checked.HasValue)
            {
                isChecked = Checked.Value;
            }

            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
            }

            if (contextMenuShown && _registrationToken == null)
                await RegisterListFocusAsync();

            if (!contextMenuShown && _registrationToken != null)
                await DeregisterListFocusAsync();


            await base.OnAfterRenderAsync(firstRender);
        }

        protected RenderFragment<ButtonBase> CustomBuildRenderTree = button => builder =>
        {
            //base.BuildRenderTree(builder);
            button.StartRoot(builder, "");

        };


        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            commandDisabled = !Command.CanExecute(CommandParameter);
            InvokeAsync(StateHasChanged);
        }

        protected async void ClickHandler(MouseEventArgs args)
        {
            if (Toggle)
            {
                isChecked = !isChecked;
                await CheckedChanged.InvokeAsync(isChecked);
            }
            if (!isSplitButton && MenuItems != null)
            {
                contextMenuShown = !contextMenuShown;
            }

            await OnClick.InvokeAsync(args);

            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }

        private void KeyDownHandler(KeyboardEventArgs keyboardEventArgs)
        {
            OnKeyDown.InvokeAsync(keyboardEventArgs);
        }

        private void MenuClickHandler(MouseEventArgs args)
        {
            contextMenuShown = !contextMenuShown;
        }

        private async Task RegisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await DeregisterListFocusAsync();
            }
            _registrationToken = await JSRuntime.InvokeAsync<string>("FluentUIBaseComponent.registerKeyEventsForList", RootElementReference);
        }

        private async Task DeregisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await JSRuntime.InvokeVoidAsync("FluentUIBaseComponent.deregisterKeyEventsForList", _registrationToken);
                _registrationToken = null;
            }
        }

        public static void Focus()
        {

        }

        public static void DismissMenu(bool isDismissed)
        {

        }

        public static void OpenMenu(bool shouldFocusOnContainer, bool shouldFocusOnMount)
        {

        }

        //protected ICollection<IRule> CreateBaseGlobalCss(ITheme theme)
        //{
        //    var buttonRules = new HashSet<IRule>();

        //    var props = new FocusStyleProps(theme);
        //    props.Inset = 1;
        //    props.BorderColor = "transparent";
        //    props.HighContrastStyle = "left:-2px;top:-2px;bottom:-2px;right:-2px;border:none;outline-color:ButtonText;";
        //    var rootFocusStyles = FocusStyle.GetFocusStyle(props, ".ms-Button");

        //    foreach (var rule in rootFocusStyles.AddRules)
        //        buttonRules.Add(rule);

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button" },
        //        Properties = new CssString()
        //        {
        //            Css = rootFocusStyles.MergeRules +
        //                  $"font-size:{theme.FontStyle.FontSize.Medium};" +
        //                  $"font-weight:{theme.FontStyle.FontWeight.Regular};" +
        //                  $"box-sizing:border-box;" +
        //                  $"border: 1px solid {theme.SemanticColors.ButtonBorder};" +
        //                  $"user-select:none;" +
        //                  $"display:inline-block;" +
        //                  $"text-decoration:none;" +
        //                  $"text-align:center;" +
        //                  $"cursor:pointer;" +
        //                  $"vertical-align:top;" +
        //                  $"padding:0 16px;" +
        //                  $"min-width:80px;" +
        //                  $"height:32px;" +
        //                  $"border-radius:{theme.Effects.RoundedCorner2};"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button:active > *" },
        //        Properties = new CssString()
        //        {
        //            Css = $"position:relative;" +
        //                  $"left:0;" +
        //                  $"top:0;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-disabled" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.SemanticColors.DisabledBackground};" +
        //                 $"color:{theme.SemanticTextColors.DisabledText};" +
        //                 $"cursor:default;" +
        //                 $"pointer-events:none;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button:hover, .ms-Button:focus" },
        //        Properties = new CssString()
        //        {
        //            Css = $"text-decoration:none;" +
        //                 $"outline:0;"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button:link, .ms-Button:visited, .ms-Button:active" },
        //        Properties = new CssString()
        //        {
        //            Css = $"text-decoration:none;"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
        //        Properties = new CssString()
        //        {
        //            Css = ".ms-Button{color: GrayText; border-color:GrayText;}"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-expanded" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.Palette.NeutralLight};" +
        //                 $"color:{theme.Palette.NeutralDark};"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--primary.is-expanded" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.Palette.ThemeDark};" +
        //                 $"color:{theme.Palette.White};"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-expanded .ms-Button-icon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.Palette.ThemeDark};"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--primary.is-expanded .ms-Button-icon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.Palette.White};"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-expanded .ms-Button-menuIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.Palette.NeutralPrimary};"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--primary.is-expanded .ms-Button-menuIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.Palette.White};"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-expanded:hover" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.Palette.NeutralQuaternaryAlt};"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--primary.is-expanded:hover" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.Palette.ThemeDark};"
        //        }
        //    });


        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-disabled .ms-Button-icon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.SemanticTextColors.DisabledText};"
        //        }
        //    });
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button.is-disabled .ms-Button-menuIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:{theme.SemanticTextColors.DisabledText};"
        //        }
        //    });

        //    //FlexContainer
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-flexContainer" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:flex;" +
        //                  $"height:100%;" +
        //                  $"flex-wrap:nowrap;" +
        //                  $"justify-content:center;" +
        //                  $"align-items:center;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-textContainer" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:block;" +
        //                  $"flex-grow:1;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-description" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:block;" +
        //                  $"font-size:{theme.FontStyle.FontSize.Small};" +
        //                  $"font-weight:{theme.FontStyle.FontWeight.Regular};"
        //        }
        //    });

        //    //Icon
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-icon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"font-size:{theme.FontStyle.FontSize.MediumPlus};" +  //originally FontSize.Icon
        //                  $"margin:0px 4px;" +
        //                  $"height:16px;" +
        //                  $"line-height:16px;" +
        //                  $"text-align:center;" +
        //                  $"vertical-align:middle;" +
        //                  $"flex-shrink:0;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-menuIcon, .ms-Button-menuIcon .ms-Button-icon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
        //                  $"margin:0px 4px;" +
        //                  $"height:16px;" +
        //                  $"line-height:16px;" +
        //                  $"text-align:center;" +
        //                  $"vertical-align:middle;" +
        //                  $"flex-shrink:0;"
        //        }
        //    });

        //    //Label
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-label" },
        //        Properties = new CssString()
        //        {
        //            Css = $"font-weight:{theme.FontStyle.FontWeight.SemiBold};" +
        //                  $"margin:0px 4px;" +
        //                  $"line-height:100%;" +
        //                  $"display:block;"
        //        }
        //    });

        //    //ScreenReaderText
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-screenReaderText" },
        //        Properties = new CssString()
        //        {
        //            Css = $"position:absolute;" +
        //                  $"width:1px;" +
        //                  $"height:1px;" +
        //                  $"margin:-1px;" +
        //                  $"padding:0px;" +
        //                  $"border:0px;" +
        //                  $"overflow:hidden;"
        //        }
        //    });

        //    //Split stuff
        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-splitContainer" },
        //        Properties = new CssString()
        //        {
        //            Css = $"display:inline-flex;" +
        //                  $"position:relative;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-splitContainer .ms-Button--default:not(.ms-Button-menuIcon)" },
        //        Properties = new CssString()
        //        {
        //            Css = $"border-top-right-radius:0;" +
        //                  $"border-bottom-right-radius:0;" +
        //                  $"border-right:none;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-splitContainer .ms-Button--primary:not(.ms-Button-menuIcon)" },
        //        Properties = new CssString()
        //        {
        //            Css = $"border-top-right-radius:0;" +
        //                  $"border-bottom-right-radius:0;" +
        //                  $"border-right:none;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-splitContainer .ms-Button-menuIcon" },
        //        Properties = new CssString()
        //        {
        //            Css = $"padding:6px;" +
        //                  $"height:auto;" +
        //                  $"box-sizing:border-box;" +
        //                  $"border-radius:0;" +
        //                  $"border-top-right-radius:{theme.Effects.RoundedCorner2};" +
        //                  $"border-bottom-right-radius:{theme.Effects.RoundedCorner2};" +
        //                  $"border-left:none;" +
        //                  $"outline:transparent;" +
        //                  $"user-select:none;" +
        //                  $"display:inline-block;" +
        //                  $"text-decoration:none;" +
        //                  $"text-align:center;" +
        //                  $"cursor:pointer;" +
        //                  $"vertical-align:top;" +
        //                  $"min-width:32px;" +
        //                  $"width:32px;" +
        //                  $"margin-left:-1px;" +
        //                  $"margin-top:0px;" +
        //                  $"margin-right:0px;" +
        //                  $"margin-bottom:0px;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-divider" },
        //        Properties = new CssString()
        //        {
        //            Css = $"position:absolute;" +
        //                $"width:1px;" +
        //                $"right:31px;" +
        //                $"top:8px;" +
        //                $"bottom:8px;"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.ms-Button-divider" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.Palette.White};"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.ms-Button-divider" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.SemanticColors.BodyDivider};"
        //        }
        //    });

        //    buttonRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-Button-divider.disabled" },
        //        Properties = new CssString()
        //        {
        //            Css = $"background-color:{theme.SemanticColors.BodyDivider};"
        //        }
        //    });

        //    return buttonRules;
        //}

        protected void StartRoot(RenderTreeBuilder builder, string buttonClassName)
        {

            isSplitButton = (Split && OnClick.HasDelegate && MenuItems != null);
            isCompoundButton = SecondaryText != null;
            if (isSplitButton)
            {
                AddSplit(builder, buttonClassName);
            }
            else
            {
                builder.OpenComponent<KeytipData>(21);
                //save attribute space 22, 23,24
                builder.AddAttribute(25, "ChildContent", (RenderFragment)(builder2 =>
                {
                    AddContent(builder2, buttonClassName);
                }));
                builder.CloseComponent();
            }

        }

        protected void AddSplit(RenderTreeBuilder builder, string buttonClassName)
        {
            builder.OpenElement(11, "div");
            builder.AddAttribute(12, "class", $"ms-Button-splitContainer");
            if (!Disabled && !PrimaryDisabled && !commandDisabled)
            {
                builder.AddAttribute(13, "tabindex", 0);
            }

            builder.OpenElement(14, "span");
            builder.AddAttribute(15, "style", "display: flex;");

            AddContent(builder, buttonClassName);
            AddSplitButtonMenu(builder, buttonClassName);
            AddSplitButtonDivider(builder, buttonClassName);

            builder.CloseElement(); // closes span 14
            builder.CloseElement(); //closes div 11
        }


        protected virtual void AddContent(RenderTreeBuilder builder, string buttonClassName)
        {

            if (Href == null)
            {
                builder.OpenElement(25, "button");
            }
            else
            {
                builder.OpenElement(25, "a");
                builder.AddAttribute(26, "href", Href);

            }

            if (Primary)
            {
                buttonClassName += " ms-Button--primary";
            }
            else
            {
                buttonClassName += " ms-Button--default";
            }
            if (isSplitButton)
            {
                builder.AddAttribute(27, "class", $"ms-Button {buttonClassName} {ClassName} {(Disabled || PrimaryDisabled || commandDisabled ? "is-disabled" : "")} {(isChecked ? "is-checked" : "")}");
                builder.AddAttribute(28, "disabled", (Disabled || PrimaryDisabled || commandDisabled) && !AllowDisabledFocus);
            }
            else
            {
                builder.AddAttribute(27, "class", $"ms-Button {buttonClassName} {ClassName} {(Disabled || commandDisabled ? " is-disabled" : "")}{(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(28, "disabled", (Disabled || commandDisabled) && !AllowDisabledFocus);
            }
            builder.AddAttribute(29, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, ClickHandler));
            builder.AddAttribute(30, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, KeyDownHandler));

            builder.AddAttribute(31, "data-is-focusable", !Disabled && !PrimaryDisabled && !commandDisabled && !isSplitButton);
            builder.AddAttribute(32, "style", Style);
            builder.AddMultipleAttributes(33, UnknownProperties);

            builder.AddElementReferenceCapture(34, (elementRef) => { RootElementReference = elementRef; });

            builder.OpenElement(35, "span");
            builder.AddAttribute(36, "class", "ms-Button-flexContainer");

            if (IconName != null | IconSrc != null)
            {
                builder.OpenComponent<Icon>(40);
                builder.AddAttribute(41, "ClassName", "ms-Button-icon");
                builder.AddAttribute(42, "IconName", IconName);
                builder.AddAttribute(42, "IconSrc", IconSrc);
                builder.CloseComponent(); //closes Icon 40
            }
            if (Text != null || (isCompoundButton && SecondaryText != null))
            {
                builder.OpenElement(51, "span");
                builder.AddAttribute(52, "class", "ms-Button-textContainer");

                builder.OpenElement(53, "span");
                builder.AddAttribute(54, "class", "ms-Button-label");
                builder.AddContent(55, Text ?? "");
                builder.CloseElement();  //closes span (53)

                if (isCompoundButton && SecondaryText != null)
                {
                    builder.OpenElement(61, "span");
                    builder.AddAttribute(62, "class", "ms-Button-description");
                    builder.AddContent(63, SecondaryText);
                    builder.CloseElement(); //closes div 61
                }
                builder.CloseElement();//closes div (51)
            }
            if (AriaDescripton != null)
            {
                builder.OpenElement(71, "span");
                builder.AddAttribute(72, "class", "ms-Button-screenReaderText");
                builder.AddContent(73, AriaDescripton);
                builder.CloseElement(); //closes span 71
            }
            if (Text == null && ContentTemplate != null)
            {
                builder.AddContent(81, ContentTemplate);
            }
            if (!isSplitButton && MenuItems != null && !HideChevron)
            {
                builder.OpenComponent<Icon>(90);
                builder.AddAttribute(91, "IconName", "ChevronDown");
                builder.AddAttribute(92, "IconSrc", "IconSrc");
                builder.AddAttribute(92, "ClassName", "ms-Button-menuIcon");
                builder.CloseComponent(); //closes Icon 90
            }
            if (MenuItems != null && contextMenuShown)
            {
                builder.OpenComponent<ContextualMenu>(100);
                builder.AddAttribute(101, "FabricComponentTarget", this);
                builder.AddAttribute(102, "ShouldFocusOnMount", _menuShouldFocusOnMount);
                builder.AddAttribute(103, "OnDismiss", EventCallback.Factory.Create<bool>(this, (isDismissed) =>
                {
                    contextMenuShown = false;
                }));
                builder.AddAttribute(104, "Items", MenuItems);
                builder.AddAttribute(105, "DirectionalHint", DirectionalHint.BottomLeftEdge);
                builder.AddAttribute(106, "ItemTemplate", MenuItemTemplate);
                builder.AddAttribute(107, "SubordinateItemTemplate", SubordinateItemTemplate);

                builder.CloseComponent();  //closes ContextualMenu 100
            }

            builder.CloseElement(); //closes span 35

            //if (false)
            //{
            //    //render Menu, donotlayer,  not yet made
            //}
            //if (false) // menu causes inline-block div
            //{
            //    builder.CloseElement();
            //}

            builder.CloseElement();  // closing button or a
        }


        protected void AddSplitButtonMenu(RenderTreeBuilder builder, string buttonClassName)
        {
            if (Primary)
            {
                builder.OpenComponent<PrimaryButton>(105);
                builder.AddAttribute(106, "IconName", "ChevronDown");
                builder.AddAttribute(107, "ClassName", $"ms-Button-menuIcon{(Disabled || commandDisabled ? " is-disabled" : "")} {(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(108, "OnClick", EventCallback.Factory.Create(this, MenuClickHandler));
                builder.AddAttribute(109, "Disabled", Disabled);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<DefaultButton>(105);
                builder.AddAttribute(106, "IconName", "ChevronDown");
                builder.AddAttribute(107, "ClassName", $"ms-Button-menuIcon{(Disabled || commandDisabled ? " is-disabled" : "")} {(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(108, "OnClick", EventCallback.Factory.Create(this, MenuClickHandler));
                builder.AddAttribute(109, "Disabled", Disabled);
                builder.CloseComponent();
            }
        }

        protected void AddSplitButtonDivider(RenderTreeBuilder builder, string buttonClassName)
        {
            builder.OpenElement(110, "span");
            if (Primary)
            {
                builder.AddAttribute(111, "class", $"ms-Button-divider ms-Button--primary{(Disabled ? " is-disabled" : "")}");
            }
            else
            {
                builder.AddAttribute(111, "class", $"ms-Button-divider ms-Button--default{(Disabled ? " is-disabled" : "")}");
            }
            builder.AddAttribute(112, "aria-hidden", true);
            builder.CloseElement();

        }

        public async ValueTask DisposeAsync()
        {
            if (_registrationToken != null)
                await DeregisterListFocusAsync();
        }
    }
}