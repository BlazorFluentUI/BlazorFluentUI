using System.Collections.Generic;

namespace BlazorFluentUI
{
    public static class CalendarStyle
    {
        public static ICollection<IRule> GetCalendarStyle(ITheme theme)
        {
            var MyRules = new List<IRule>();

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar" },
                Properties = new CssString()
                {
                    Css = $"box-sizing:border-box;" +
                        $"box-shadow:none;" +
                        $"margin:0;" +
                        $"padding:0;" +
                        $"overflow:visible;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar *::-moz-focus-inner" },
                Properties = new CssString()
                {
                    Css = $"border:0;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar *" },
                Properties = new CssString()
                {
                    Css = $"outline:transparent;" +
                        $"position:relative;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Calendar *:focus:after" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                        $"position:absolute;" +
                        $"top:0;" +
                        $"right:0;" +
                        $"bottom:0;" +
                        $"left:0;" +
                        $"pointer-events:none;" +
                        $"border:1px solid {theme.Palette.NeutralSecondary};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-picker" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.Black};" +
                        $"font-size:{theme.FontStyle.FontSize.Medium};" +
                        $"position:relative;" +
                        $"text-align:left;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-holder" },
                Properties = new CssString()
                {
                    Css = $"-webkit-overflow-scrolling:touch;" +
                        $"box-sizing:border-box;" +
                        $"display:none;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-picker.ms-Calendar-pickerIsOpened .ms-Calendar-holder" },
                Properties = new CssString()
                {
                    Css = $"box-sizing:border-box;" +
                        $"display:inline-block;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-picker--opened" },
                Properties = new CssString()
                {
                    Css = $"position:relative;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-frame" },
                Properties = new CssString()
                {
                    Css = $"position:relative;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-wrap" },
                Properties = new CssString()
                {
                    Css = $"min-height:212px;" +
                    $"padding:12px;" +
                    $"display:flex;" +
                    $"box-sizing:content-box;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-wrap.ms-Calendar-goTodaySpacing" },
                Properties = new CssString()
                {
                    Css = $"min-height:228px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-header" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                        $"display:inline-flex;" +
                        $"height:28px;" +
                        $"line-height:44px;" +
                        $"width:100%;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-divider" },
                Properties = new CssString()
                {
                    Css = $"top:0;" +
                        $"margin-top:-12px;" +
                        $"margin-bottom:-12px;" +
                        $"border-right:1px solid {theme.Palette.NeutralLight};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthAndYear,.ms-Calendar-year,.ms-Calendar-decade" },
                Properties = new CssString()
                {
                    Css = $"display:inline-flex;" +
                        $"flex-grow:1;" +
                        /*   @include ms-font-m; */
                        $"font-weight:600;" +
                        $"color:{theme.Palette.NeutralPrimary};" +
                        $"padding:0 5px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthAndYear:hover,.ms-Calendar-currentYear:hover,.ms-Calendar-currentDecade:hover" },
                Properties = new CssString()
                {
                    Css = $"cursor:default;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-table" },
                Properties = new CssString()
                {
                    Css = $"text-align:center;" +
                        $"border-collapse:collapse;" +
                        $"border-spacing:0;" +
                        $"table-layout:fixed;" +
                        $"font-size:inherit;" +
                        $"margin-top:3px;" +
                        $"width:197px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-table td" },
                Properties = new CssString()
                {
                    Css = $"margin:0;" +
                        $"padding:0;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayWrapper,.ms-Calendar-weekday" },
                Properties = new CssString()
                {
                    Css = $"width:28px;" +
                       $"height:28px;" +
                       $"padding:0;" +
                       $"line-height:28px;" +
                       $"font-size:{theme.FontStyle.FontSize.Small};" +
                       /*  @include ms-font-m-plus; */
                       $"color:{theme.Palette.NeutralPrimary};" +
                       $"box-sizing:border-box;" +
                       $"justify-content:center;" +
                       $"align-items:center;" +
                       $"cursor:default;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayWrapper *::-moz-focus-inner,.ms-Calendar-weekday *::-moz-focus-inner" },
                Properties = new CssString()
                {
                    Css = $"border:0;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayWrapper *,.ms-Calendar-weekday *" },
                Properties = new CssString()
                {
                    Css = $"outline:transparent;" +
                       $"position:relative;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Calendar-dayWrapper *:focus:after,.ms-Fabric--isFocusVisible .ms-Calendar-weekday *:focus:after" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                       $"position:absolute;" +
                       $"top:-2px;" +
                       $"right:-2px;" +
                       $"bottom:-2px;" +
                       $"left:-2px;" +
                       $"pointer-events:none;" +
                       $"border:1px solid {theme.Palette.NeutralSecondary};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-day" },
                Properties = new CssString()
                {
                    Css = $"width:24px;" +
                       $"height:24px;" +
                       $"border-radius:2px;" +
                       $"display:inline-flex;" +
                       $"align-items:center;" +
                       $"justify-content:center;" +
                       $"border:none;" +
                       $"padding:0;" +
                       $"background-color:transparent;" +
                       $"line-height:100%;" +
                       $"font-size:inherit;" +
                       $"color:inherit;" +
                       $"font-weight:inherit;" +
                       $"cursor:pointer;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-daySelection .ms-Calendar-day" },
                Properties = new CssString()
                {
                    /* high contrast */
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsToday" },
                Properties = new CssString()
                {
                    Css = $"border-radius:100%;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsToday,.ms-Calendar-dayIsToday:hover" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                       $" background-color:{theme.Palette.NeutralLighter};"
                    /* high contrast */
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar--dayIsToday span,.ms-Calendar--dayIsToday:hover span" },
                Properties = new CssString()
                {
                    /* high contrast */
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsToday:hover,.ms-Calendar-dayIsToday:hover:hover" },
                Properties = new CssString()
                {
                    Css = $"border-radius:100%;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsDisabled:before" },
                Properties = new CssString()
                {
                    Css = $"border-top-color:{theme.Palette.NeutralTertiary};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsUnfocused" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralSecondary}" +
                        $"font-weight:{theme.FontStyle.FontWeight.Regular};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsFocused:hover,.ms-Calendar-dayIsUnfocused:hover" },
                Properties = new CssString()
                {
                    Css = $"cursor:pointer;" +
                        $"color:{theme.Palette.NeutralDark};" +
                        $"background:{theme.Palette.NeutralLighter};"
                }
            });

            /*// Highlighted and hovered/focused dates.*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-daySelection.ms-Calendar-dayIsHighlighted:hover,.ms-Calendar-pickerIsFocused .ms-Calendar-dayIsHighlighted.ms-Calendar-daySelection" },
                Properties = new CssString()
                {
                    Css = $"cursor:pointer;" +
                        $"background-color:{theme.Palette.NeutralLight};" +
                        $"border-radius: 2px;"

                    /*@include high-contrast {
                        border: 2px solid Highlight;

                        :not(.dayIsToday) span {
                            color: Highlight;
                        }
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsHighlighted button.ms-Calendar-dayIsToday" },
                Properties = new CssString()
                {
                    /*@include high-contrast {
                        border-radius: 100%;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsHighlighted button.ms-Calendar-dayIsToday span" },
                Properties = new CssString()
                {
                    /*@include high-contrast {
                        border-radius: 100%;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsFocused:active,.ms-Calendar-dayIsHighlighted" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.ThemeLight};"
                }
            });


            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsFocused:active.day,.ms-Calendar-dayIsHighlighted.day" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralDark};" +
                        $"background-color:{theme.Palette.NeutralLight};"
                }
            });

            /*// Disabled and highlighted dates.*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsHighlighted.dayDisabled,.ms-Calendar-dayIsHighlighted.dayDisabled:hover" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.Palette.NeutralTertiary};"
                }
            });

            /*// Highlighted date squares*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayBackground,.ms-Calendar-dayBackground:hover,.ms-Calendar-dayBackground:active" },
                Properties = new CssString()
                {
                    Css = $"border-radius:2px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayHover,.ms-Calendar-dayHover:hover" },
                Properties = new CssString()
                {
                    Css = $"cursor:pointer;" +
                        $"color:{theme.Palette.NeutralDark};" +
                        $"background:{theme.Palette.NeutralLighter};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayPress,.ms-Calendar-dayPress:hover" },
                Properties = new CssString()
                {
                    Css = $"cursor:pointer;" +
                        $"color:{theme.Palette.NeutralDark};" +
                        $"background:{theme.Palette.NeutralLighter};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayPress .ms-Calendar-dayIsToday,.ms-Calendar-dayPress:hover .ms-Calendar-dayIsToday" },
                Properties = new CssString()
                {
                    Css = $"background:{theme.Palette.ThemePrimary};" +
                        $"border-radius: 100%;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsUnfocused:active,.ms-Calendar-dayIsFocused:active,.ms-Calendar-dayIsHighlighted,.ms-Calendar-dayIsHighlighted:hover,.ms-Calendar-dayIsHighlighted:active,.ms-Calendar-weekBackground,.ms-Calendar-weekBackground:hover,.ms-Calendar-weekBackground:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                       $"color:{theme.Palette.NeutralDark};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsToday,.ms-Calendar-pickerIsFocused .ms-Calendar-dayIsToday,.ms-Calendar-dayIsToday.ms-Calendar-day:active" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                       $"color:{theme.Palette.White};" +
                       $"font-weight:{theme.FontStyle.FontWeight.SemiBold};" +
                       $"background:{theme.Palette.ThemePrimary};" +
                       $"border-radius:100%;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-showWeekNumbers .ms-Calendar-weekNumbers" },
                Properties = new CssString()
                {
                    Css = $"border-right:1px solid {theme.Palette.NeutralLight};" +
                       $"box-sizing:border-box;" +
                       $"width:28px;" +
                       $"padding:0;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-showWeekNumbers .ms-Calendar-weekNumbers .dayWrapper" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralSecondary};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-showWeekNumbers .ms-Calendar-weekNumbers .dayWrapper.weekIsHighlighted" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-showWeekNumbers .ms-Calendar-table" },
                Properties = new CssString()
                {
                    Css = $"width:225px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-showWeekNumbers .ms-Calendar-table .ms-Calendar-dayWrapper,.ms-Calendar-showWeekNumbers .ms-Calendar-table .ms-Calendar-weekday" },
                Properties = new CssString()
                {
                    Css = $"width:30px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthComponents,.ms-Calendar-yearComponents,.ms-Calendar-decadeComponents" },
                Properties = new CssString()
                {
                    Css = $"display:inline-flex;" +
                        $"align-self:flex-end;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-closeButton,.ms-Calendar-prevMonth,.ms-Calendar-nextMonth,.ms-Calendar-prevYear,.ms-Calendar-nextYear,.ms-Calendar-prevDecade,.ms-Calendar-nextDecade" },
                Properties = new CssString()
                {
                    Css = $"width:24px;" +
                        $"height:24px;" +
                        $"display:block;" +
                        $"text-align:center;" +
                        $"line-height:24px;" +
                        $"font-size:{theme.FontStyle.FontSize.Small};" +
                        $"color:{theme.Palette.NeutralPrimary};" +
                        $"border-radius:2px;" +
                        $"position:relative;" +
                        $"background-color:transparent;" +
                        $"border:none;" +
                        $"padding:0;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-closeButton:hover,.ms-Calendar-prevMonth:hover,.ms-Calendar-nextMonth:hover,.ms-Calendar-prevYear:hover,.ms-Calendar-nextYear:hover,.ms-Calendar-prevDecade:hover,.ms-Calendar-nextDecade:hover" },
                Properties = new CssString()
                {
                    Css = $"cursor:pointer;" +
                        $"color:{theme.Palette.NeutralDark};" +
                        $"outline:1px solid transparent;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-prevMonthIsDisabled,.ms-Calendar-nextMonthIsDisabled,.ms-Calendar-prevYearIsDisabled,.ms-Calendar-nextYearIsDisabled,.ms-Calendar-prevDecadeIsDisabled,.ms-Calendar-nextDecadeIsDisabled" },
                Properties = new CssString()
                {
                    Css = $"pointer-events:none;" +
                        $"color:{theme.Palette.NeutralTertiaryAlt};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-headerToggleView" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                        $"align-items:center;" +
                        $"padding:4px 8px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-headerToggleView:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.Black};" +
                        $"cursor:pointer;"
                    /* HighContrast */
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-headerToggleView:hover:active" },
                Properties = new CssString()
                {
                    /* HighContrast */
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-currentYear,.ms-Calendar-currentDecade" },
                Properties = new CssString()
                {
                    Css = $"display:inline-flex;" +
                        $"flex-grow:1;" +
                        $"padding:0 5px;" +
                        /*@include ms-font-m;*/
                        $"font-weight:600;" +
                        $"color:{theme.Palette.NeutralPrimary};" +
                        $"height:28px;" +
                        $"line-height:28px;" +
                        $"margin-left:5px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-optionGrid" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                        $"height:210px;" +
                        $"width:196px;" +
                        $"margin:4px, 0, 0, 0;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthOption,.ms-Calendar-yearOption" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};" +
                          $"width:60px;" +
                        $"height:60px;" +
                        $"line-height:100%;" +
                        $"cursor:pointer;" +
                        $"float:left;" +
                        $"margin:0 10px 10px 0;" +
                        /*@include ms-font-s-plus;*/
                        $"color:{theme.Palette.NeutralPrimary};" +
                        $"text-align:center;" +
                        $"border:none;" +
                        $"padding:0;" +
                        $"background-color:transparent;" +
                        $"border-radius:2px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthOption:hover,.ms-Calendar-yearOption:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralDark};" +
                        $"background-color:{theme.Palette.NeutralLighter};" +
                        $"outline: 1px solid transparent;"
                    /*@include high-contrast {
                       outline-color: highlight;
                   }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthOption:active,.ms-Calendar-yearOption:active" },
                Properties = new CssString()
                {
                    /*@include high-contrast {
                       outline-color: highlight;
                   }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthOption.ms-Calendar-isHighlighted,.ms-Calendar-yearOption.ms-Calendar-isHighlighted" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.NeutralDark};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-dayIsDisabled,.ms-Calendar-monthOptionIsDisabled,.ms-Calendar-yearOptionIsDisabled" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralTertiaryAlt};" +
                        $"pointer-events:none;"
                }
            });

            /*Button to navigate to the current date.*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-goToday" },
                Properties = new CssString()
                {
                    Css = $" bottom:0;" +
                        $"color:{theme.Palette.ThemePrimary};" +
                        $"cursor:pointer;" +
                        /*@include ms-font-s;*/
                        $"color:{theme.Palette.NeutralPrimary};" +
                        $"height:30px;" +
                        $"line-height:30px;" +
                        $"padding:0 10px;" +
                        $"background-color: transparent;" +
                        $"border:none;" +
                        $"position:absolute !important;" +
                        $"box-sizing:content-box;" +
                        $"right:13px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-goToday:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};" +
                        $"outline:1px solid transparent;"
                    /*@include high-contrast {
                        outline-color: highlight;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-goToday:active" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDark};"
                    /*@include high-contrast {
                        color: highlight;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-goToTodayIsDisabled" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralTertiaryAlt};" +
                    $"pointer-events:none;"
                    /*@include high-contrast {
                        color: highlight;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-goTodayInlineMonth" },
                Properties = new CssString()
                {
                    Css = $"top:212px;"
                }
            });

            /*Additional 28px padding needed when "Go to today" button is visible*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-wrap.ms-Calendar-goTodaySpacing" },
                Properties = new CssString()
                {
                    Css = $"padding-bottom:28px;"
                }
            });

            /*State: The picker is showing the year components.*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-isPickingYears .ms-Calendar-dayPicker,.ms-Calendar-isPickingYears .ms-Calendar-monthComponents" },
                Properties = new CssString()
                {
                    Css = $"display:none;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-isPickingYears .ms-Calendar-monthPicker" },
                Properties = new CssString()
                {
                    Css = $"display:none;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-isPickingYears .ms-Calendar-yearPicker" },
                Properties = new CssString()
                {
                    Css = $"display:block;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media (min-device-width: 460px)" },
                Properties = new CssString()
                {
                    Css = ".ms-Calendar-wrap{" +
                        "    padding:12px;" +
                        "}" +
                        ".ms-Calendar-dayPicker," +
                        ".ms-Calendar-monthPicker{" +
                        "    min-height:200px;" +
                        "}" +
                        ".ms-Calendar-header{" +
                        "    height:28px;" +
                        "    line-height:28px;" +
                        "    width:100%;" +
                        "}" +
                        ".ms-Calendar-dayWrapper," +
                        ".ms-Calendar-weekday{" +
                        "    width:28px;" +
                        "    height:28px;" +
                        "    line-height:28px;" +
                        $"    font-size:{theme.FontStyle.FontSize.Small};" +
                        "}" +
                        ".ms-Calendar-closeButton," +
                        ".ms-Calendar-prevMonth," +
                        ".ms-Calendar-nextMonth," +
                        ".ms-Calendar-prevYear," +
                        ".ms-Calendar-nextYear," +
                        ".ms-Calendar-prevDecade," +
                        ".ms-Calendar-nextDecade{" +
                        $"    font-size:{theme.FontStyle.FontSize.Small};" +
                        "    width:28px;" +
                        "    height:28px;" +
                        "    line-height:28px;" +
                        "}" +
                        ".ms-Calendar-holder{" +
                        "    display:inline-block;" +
                        "    height:auto;" +
                        "    overflow:hidden;" +
                        "}" +
                        ".ms-Calendar-monthAndYear," +
                        ".ms-Calendar-year," +
                        ".ms-Calendar-decade{" +
                        $"    font-size:{theme.FontStyle.FontSize.Medium};" +
                        $"    color:{theme.Palette.NeutralPrimary};" +
                        "}" +
                        ".ms-Calendar-yearComponents{" +
                        "    margin-left:1px;" +
                        "}" +
                        ".ms-Calendar.ms-Calendar-goToday {" +
                        "    padding: 0 3px;" +
                        /*@include ms-right(20px);*/
                        "}" +
                        ".ms-Calendar-showWeekNumbers ms-Calendar-table .ms-Calendar-dayWrapper," +
                        ".ms-Calendar-showWeekNumbers ms-Calendar-table .ms-Calendar-weekday {" +
                        "    width:28px;" +
                        "}" +

                        ".ms-Calendar-monthPickerVisible .ms-Calendar-wrap {" +
                        "    padding:12px;" +
                        "}" +
                        ".ms-Calendar-monthPickerVisible .ms-Calendar-dayPicker {" +
                        "    margin: -10px 0;" +
                        "    padding: 10px 0;" +
                        "    box-sizing: border-box;" +
                        "    width: 212px;" +
                        "    min-height: 200px;" +
                        "}" +
                        ".ms-Calendar-monthPickerVisible .ms-Calendar-monthPicker {" +
                        "    display:block;" +
                        "}" +
                        ".ms-Calendar-monthPickerVisible .ms-Calendar-optionGrid {" +
                        "    height:150px;" +
                        "    width:196px;" +
                        "}" +
                        ".ms-Calendar-toggleMonthView{" +
                        "    display:none;" +
                        "}" +
                        ".ms-Calendar-currentYear," +
                        ".ms-Calendar-currentDecade{" +
                        $"    font-size:{theme.FontStyle.FontSize.Medium};" +
                        "    margin:0;" +
                        "    height:28px;" +
                        "    line-height:28px;" +
                        "    display:inline-block;" +
                        "}" +
                        ".ms-Calendar-monthOption," +
                        ".ms-Calendar-yearOption{" +
                        "    width:40px;" +
                        "    height:40px;" +
                        "    line-height:100%;" +
                        $"    font-size:{theme.FontStyle.FontSize.Small};" +
                        "    margin:0 12px 16px 0;" +
                        "}" +
                        "    .ms-Calendar-monthOption:hover," +
                        "    .ms-Calendar-yearOption:hover {" +
                        "        outline: 1px solid transparent;" +
                        "    }" +
                        "    .ms-Calendar-monthOption:nth-child(4n+4)," +
                        "    .ms-Calendar-yearOption:nth-child(4n+4) {" +
                        "        margin: 0 0px 16px 0;" +
                        "    }" +

                        ".ms-Calendar-goToday {" +
                        $"    font-size:{theme.FontStyle.FontSize.Small};" +
                        "    height: 28px;" +
                        "    line-height: 28px;" +
                        "    padding: 0 10px;" +
                        "    right: 8px;" +
                        "    text-align:right;" +
                        "}" +
                        ".ms-Calendar-isPickingYears .ms-Calendar-dayPicker," +
                        ".ms-Calendar-isPickingYears .ms-Calendar-monthComponents {" +
                        "    display:block;" +
                        "}" +
                        ".ms-Calendar-isPickingYears .ms-Calendar-monthPicker{" +
                        "    display:none;" +
                        "}" +
                        ".ms-Calendar-isPickingYears .ms-Calendar-yearPicker{" +
                        "    display:block;" +
                        "}" +

                        ".ms-Calendar-calendarsInline .ms-Calendar-wrap {" +
                        "    padding:12px;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-holder {" +
                        "    height:auto;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-table {" +
                        "    margin-right: 12px;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-dayPicker {" +
                        "    width: auto;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-monthPicker {" +
                        "    margin-left: 12px;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-yearPicker {" +
                        "    margin-left: 12px;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-goToday {" +
                        "    margin-right: 14px;" +
                        "    padding: 0 10px;" +
                        "}" +
                        ".ms-Calendar-calendarsInline .ms-Calendar-monthComponents {" +
                        "    margin-right: 12px;" +
                        "}" +

                        ".ms-Calendar-monthPickerOnly .ms-Calendar-wrap{" +
                        "    padding:12px;" +
                        "}" +

                        ".ms-Calendar-monthPickerAsOverlay .ms-Calendar-wrap {" +
                        "    padding-bottom: 28px;" +
                        "    margin-bottom: 6px;" +
                        "}" +
                        ".ms-Calendar-monthPickerAsOverlay .ms-Calendar-holder {" +
                        "    height: 240px;" +
                        "    min-height: 240px;" +
                        "}" +
                        ".ms-Calendar-monthPickerAsOverlay .ms-Calendar-holderWithButton {" +
                        "    padding-top: 6px;" +
                        "    height: auto;" +
                        "}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media (max-device-width: 459px)" },
                Properties = new CssString()
                {
                    Css = ".ms-Calendar-calendarsInline .ms-Calendar-monthPicker," +
                        ".ms-Calendar-calendarsInline .ms-Calendar-yearPicker {" +
                        /*// Month and year pickers, hidden on small screens by default. .monthPicker, .yearPicker*/
                        "    display: none;" +
                        "}" +

                        /*// position year components so they do not move when switching between calendars (overlayed calendars)*/
                        ".ms-Calendar-yearComponents {" +
                        "    margin-top: 2px;" +
                        "}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-goToday" },
                Properties = new CssString()
                {
                    Css = $"width:auto;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-closeButton,.ms-Calendar-nextMonth,.ms-Calendar-prevMonth,.ms-Calendar-nextYear,.ms-Calendar-prevYear,.ms-Calendar-nextDecade,.ms-Calendar-prevDecade" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-closeButton:hover,.ms-Calendar-nextMonth:hover,.ms-Calendar-prevMonth:hover,.ms-Calendar-nextYear:hover,.ms-Calendar-prevYear:hover,.ms-Calendar-nextDecade:hover,.ms-Calendar-prevDecade:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};" +
                        $"color:{theme.Palette.NeutralDark};"
                    /*@include high-contrast {
                        outline: 1px solid highlight;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-closeButton:active,.ms-Calendar-nextMonth:active,.ms-Calendar-prevMonth:active,.ms-Calendar-nextYear:active,.ms-Calendar-prevYear:active,.ms-Calendar-nextDecade:active,.ms-Calendar-prevDecade:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};"
                    /*@include high-contrast {
                        outline: 1px solid highlight;
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthIsHighlighted" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.NeutralDark};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthIsHighlighted.ms-Calendar-monthOption:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};"
                    /*@include high-contrast {
                        color: highlight;
                        border: 2px solid highlight;
                        border-radius: 2px;

                        &:hover {
                            outline: 0 !important;
                        }
                    }*/
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthIsCurrentMonth" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.White}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthIsCurrentMonth.ms-Calendar-monthOption:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};" +
                        $"color:{theme.Palette.White}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-monthOption:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.NeutralDark}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-yearIsHighlighted" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.NeutralDark}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-yearIsHighlighted.ms-Calendar-yearOption:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-yearIsCurrentYear" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.White}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-yearIsCurrentYear.yearOption:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};" +
                        $"color:{theme.Palette.White}"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-yearOption:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                        $"color:{theme.Palette.NeutralDark}"
                }
            });

            /*// Highlighted Month date styling.*/
            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-topLeftCornerDate" },
                Properties = new CssString()
                {
                    Css = $"border-top-left-radius:2px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-topRightCornerDate" },
                Properties = new CssString()
                {
                    Css = $"border-top-right-radius:2px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-bottomLeftCornerDate" },
                Properties = new CssString()
                {
                    Css = $"border-bottom-left-radius:2px;"
                }
            });

            MyRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Calendar-bottomRightCornerDate" },
                Properties = new CssString()
                {
                    Css = $"border-bottom-right-radius:2px;"
                }
            });

            return MyRules;
        }
    }
}
