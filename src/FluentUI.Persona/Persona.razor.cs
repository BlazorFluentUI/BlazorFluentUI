using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class Persona : FluentUIComponentBase
    {
        [Parameter] public bool AllowPhoneInitials { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public int CoinSize { get; set; } = -1;
        [Parameter] public bool HidePersonalDetails { get; set; }
        [Parameter] public string ImageAlt { get; set; }
        [Parameter] public string ImageInitials{ get; set; }
        [Parameter] public bool ImageShouldFadeIn { get; set; }
        [Parameter] public bool ImageShouldStartVisible { get; set; }
        [Parameter] public string ImageUrl { get; set; }
        [Parameter] public string InitialsColor { get; set; }
        [Parameter] public bool IsOutOfOffice { get; set; }
        [Parameter] public string OptionalText { get; set; }
        [Parameter] public PersonaPresenceStatus Presence { get; set; }
        [Parameter] public string PresenceTitle { get; set; }  //tooltip on hover
        [Parameter] public string SecondaryText { get; set; }
        [Parameter] public bool ShowInitialsUntilImageLoads { get; set; }
        [Parameter] public bool ShowSecondaryText { get; set; }
        [Parameter] public bool ShowUnknownPersonaCoin { get; set; }
        [Parameter] public string Size { get; set; } = PersonaSize.Size48;
        [Parameter] public string TertiaryText { get; set; }
        [Parameter] public string Text { get; set; }

        private ICollection<IRule> PersonaLocalRules { get; set; } = new List<IRule>();
        private Rule PersonaRootRule = new Rule();
        private Rule DetailsRule = new Rule();
        private Rule PrimaryTextRule = new Rule();
        private Rule SecondaryTextRule = new Rule();
        private Rule TertiaryTextRule = new Rule();
        private Rule OptionalTextRule = new Rule();

        private const string LocalSpecificityClass = "localPersonaRule";

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            return base.OnInitializedAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        protected override Task OnParametersSetAsync()
        {
            SetStyle();
            return base.OnParametersSetAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        private void CreateLocalCss()
        {
            PersonaRootRule.Selector = new ClassSelector() { SelectorName = "ms-Persona", LiteralPrefix = $".{LocalSpecificityClass}" };
            DetailsRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-details", LiteralPrefix = $".{LocalSpecificityClass}" };
            PrimaryTextRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-primaryText", LiteralPrefix = $".{LocalSpecificityClass}" };
            SecondaryTextRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-secondaryText", LiteralPrefix = $".{LocalSpecificityClass}" };
            TertiaryTextRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-tertiaryText", LiteralPrefix = $".{LocalSpecificityClass}" };
            OptionalTextRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-optionalText", LiteralPrefix = $".{LocalSpecificityClass}" };

            PersonaRootRule.Properties = new CssString() { Css = "" };
            DetailsRule.Properties = new CssString() { Css = "" };
            PrimaryTextRule.Properties = new CssString() { Css = "" };
            SecondaryTextRule.Properties = new CssString() { Css = "" };
            TertiaryTextRule.Properties = new CssString() { Css = "" };
            OptionalTextRule.Properties = new CssString() { Css = "" };

            PersonaLocalRules.Add(PersonaRootRule);
            PersonaLocalRules.Add(DetailsRule);
            PersonaLocalRules.Add(PrimaryTextRule);
            PersonaLocalRules.Add(SecondaryTextRule);
            PersonaLocalRules.Add(TertiaryTextRule);
            PersonaLocalRules.Add(OptionalTextRule);
        }
 

        private void SetStyle()
        {
            int primaryHeight = -1;
            string primaryLineHeight = null;
            string primaryOverflowX = null;
            string primaryFontSize = null;

            if (ShowSecondaryText)
            {
                primaryHeight = 16;
                primaryLineHeight = "16px";
                primaryOverflowX = "hidden";
            }

            if (CoinSize != -1)
            {
                PersonaRootRule.Properties = new CssString()
                {
                    Css = $"height:{CoinSize}px;" +
                          $"min-width:{CoinSize}px;"
                };
            }
            else
            {
                switch (Size)
                {
                    case PersonaSize.Size8:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size8};" +
                                  $"min-width:{PersonaSize.Size8};"
                        };
                        DetailsRule.Properties = new CssString()
                        {
                            Css="padding-left:17px;"
                        };
                        primaryFontSize = Theme.FontStyle.FontSize.Small;
                        primaryLineHeight = PersonaSize.Size8;
                        break;
                    case PersonaSize.Size24:
                        if (ShowSecondaryText)
                        {
                            PersonaRootRule.Properties = new CssString()
                            {
                                Css = $"height:36px;" +
                                  $"min-width:{PersonaSize.Size24};"
                            };
                        }
                        else
                        {
                            PersonaRootRule.Properties = new CssString()
                            {
                                Css = $"height:{PersonaSize.Size24};" +
                                  $"min-width:{PersonaSize.Size24};"
                            };
                        }
                        DetailsRule.Properties = new CssString()
                        {
                            Css = "padding:0 8px;"
                        };
                        if (ShowSecondaryText)
                            primaryHeight = 18;
                        break;
                    case PersonaSize.Size32:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size32};" +
                                  $"min-width:{PersonaSize.Size32};"
                        };
                        DetailsRule.Properties = new CssString()
                        {
                            Css = "padding:0 8px;"
                        };
                        if (ShowSecondaryText)
                            primaryHeight = 18;
                        break;
                    case PersonaSize.Size40:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size40};" +
                                  $"min-width:{PersonaSize.Size40};"
                        };
                        DetailsRule.Properties = new CssString()
                        {
                            Css = "padding:0 12px;"
                        };
                        if (ShowSecondaryText)
                            primaryHeight = 18;
                        break;
                    case PersonaSize.Size48:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = ""
                        };
                        DetailsRule.Properties = new CssString()
                        {
                            Css = "padding:0 12px;"
                        };
                        if (ShowSecondaryText)
                            primaryHeight = 18;
                        break;
                    case PersonaSize.Size56:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size56};" +
                                  $"min-width:{PersonaSize.Size56};"
                        };
                        primaryFontSize= Theme.FontStyle.FontSize.XLarge;
                        if (ShowSecondaryText)
                            primaryHeight = 22;
                        break;
                    case PersonaSize.Size72:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size72};" +
                                  $"min-width:{PersonaSize.Size72};"
                        };
                        primaryFontSize = Theme.FontStyle.FontSize.XLarge;
                        break;
                    case PersonaSize.Size100:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size100};" +
                                  $"min-width:{PersonaSize.Size100};"
                        };
                        primaryFontSize = Theme.FontStyle.FontSize.XLarge;
                        if (ShowSecondaryText)
                            primaryHeight = 22;
                        break;
                    case PersonaSize.Size120:
                        PersonaRootRule.Properties = new CssString()
                        {
                            Css = $"height:{PersonaSize.Size120};" +
                                  $"min-width:{PersonaSize.Size120};"
                        };
                        primaryFontSize = Theme.FontStyle.FontSize.XLarge;
                        if (ShowSecondaryText)
                            primaryHeight = 22;
                        break;
                }
            }

            PrimaryTextRule.Properties = new CssString()
            {
                Css = (primaryHeight != -1 ? $"height:{primaryHeight}px;" : "") +
                      (primaryLineHeight != null ? $"line-height:{primaryLineHeight};" : "") +
                      (primaryOverflowX != null ? $"overflow-x:{primaryOverflowX};" : "") +
                      (primaryFontSize != null ? $"font-size:{primaryFontSize};" : "")
            };

            int secondaryHeight = -1;
            int secondaryLineHeight = -1;
            int secondaryFontSize = -1;
            string secondaryDisplay = null;
            string secondaryOverflowX = null;

            if (Size == PersonaSize.Size8 | Size == PersonaSize.Size24 || Size == PersonaSize.Size32)
                secondaryDisplay = "none";
            if (ShowSecondaryText)
            {
                secondaryDisplay = "block";
                secondaryHeight = 16;
                secondaryLineHeight = 16;
                secondaryOverflowX = "hidden";
            }
            if (Size == PersonaSize.Size24 && ShowSecondaryText)
                secondaryHeight = 18;
            if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120))
                secondaryFontSize = 14;
            if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120) && ShowSecondaryText)
                secondaryHeight = 18;

            SecondaryTextRule.Properties = new CssString()
            {
                Css = (secondaryDisplay != null ? $"display:{secondaryDisplay};" : "") + 
                      (secondaryHeight != -1 ? $"height:{secondaryHeight}px;" : "") +
                      (secondaryLineHeight != -1 ? $"line-height:{secondaryLineHeight}px;" : "") +
                      (secondaryOverflowX != null ? $"overflow-x:{secondaryOverflowX};" : "") +
                      (secondaryFontSize != -1 ? $"font-size:{secondaryFontSize}px;" : "")
            };

            int tertiaryFontSize = 14;
            string tertiaryDisplay = "none";
            if (Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120)
                tertiaryDisplay = "block";

            TertiaryTextRule.Properties = new CssString()
            {
                Css = $"display:{tertiaryDisplay};" +
                      $"font-size:{tertiaryFontSize}px;"
            };

            int optionalFontSize = 14;
            string optionalDisplay = "none";

            if (Size == PersonaSize.Size100 || Size == PersonaSize.Size120)
                optionalDisplay = "block";

            OptionalTextRule.Properties = new CssString()
            {
                Css = (optionalDisplay != null ? $"display:{optionalDisplay};" : "") +
                      $"font-size:{optionalFontSize}px;"
            };

            
        }

       

            //protected string GetRootStyles()
            //{
            //    string style = "";
            //    if (CoinSize != -1)
            //    {
            //        style += $"height:{CoinSize}px;min-width:{CoinSize}px;";
            //    }
            //    else
            //    {
            //        switch (Size)
            //        {
            //            case PersonaSize.Size8:
            //                style += $"height:8px;min-width:8px;";
            //                break;
            //            case PersonaSize.Size24:
            //                if (ShowSecondaryText)
            //                {
            //                    style += $"height:36px;min-width:24px;";
            //                }
            //                else
            //                {
            //                    style += $"height:24px;min-width:24px;";
            //                }
            //                break;
            //            case PersonaSize.Size32:
            //                style += $"height:32px;min-width:32px;";
            //                break;
            //            case PersonaSize.Size48:
            //                break;
            //            case PersonaSize.Size56:
            //                style += $"height:56px;min-width:56px;";
            //                break;
            //            case PersonaSize.Size72:
            //                style += $"height:72px;min-width:72px;";
            //                break;
            //            case PersonaSize.Size100:
            //                style += $"height:100px;min-width:100px;";
            //                break;
            //            case PersonaSize.Size120:
            //                style += $"height:120px;min-width:120px;";
            //                break;
            //        }
            //    }

            //    return style;
            //}

            //protected string GetDetailsStyles()
            //{
            //    string styles = "";

            //    if (Size == PersonaSize.Size8)
            //        styles += "padding-left:17px;";
            //    else if (Size == PersonaSize.Size24 || Size == PersonaSize.Size32)
            //        styles += "padding:0 8px;";
            //    else if (Size == PersonaSize.Size40 || Size == PersonaSize.Size48)
            //        styles += "padding:0 12px;";

            //    return styles;
            //}

            //protected string GetPrimaryTextStyles()
            //{
            //    string styles = "";

            //    if (ShowSecondaryText)
            //        styles += $"height:16px;line-height:16px;overflow-x:hidden;";

            //    if (Size == PersonaSize.Size8)
            //        styles += "font-size:12px;line-height:8px;";

            //    if ((Size == PersonaSize.Size24 || Size == PersonaSize.Size32 || Size == PersonaSize.Size40 || Size == PersonaSize.Size48) && ShowSecondaryText)
            //        styles += "height:18px;";

            //    if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120))
            //        styles += "font-size:21px;";

            //    if ((Size==PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120) && ShowSecondaryText)
            //        styles += "height:22px;";

            //    return styles;
            //}

            //protected string GetSecondaryTextStyles()
            //{
            //    string styles = "";

            //    if (Size == PersonaSize.Size8 || Size == PersonaSize.Size24 || Size == PersonaSize.Size32)
            //        styles += "display:none;";

            //    if (ShowSecondaryText)
            //        styles += "display:block;height:16px;line-height:16px;overflow-x:hidden;";

            //    if (Size == PersonaSize.Size24 && ShowSecondaryText)
            //        styles += "height:18px;";

            //    if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120))
            //        styles += "font-size:14px;";

            //    if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120) && ShowSecondaryText)
            //        styles += "height:18px;";

            //    Debug.WriteLine(styles);
            //    return styles;
            //}

            //protected string GetTertiaryTextStyles()
            //{
            //    string styles = "font-size:14px;";

            //    if (Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120)
            //        styles += "display:block;";

            //    return styles;
            //}

            //protected string GetOptionalTextStyles()
            //{
            //    string styles = "font-size:14px;";

            //    if (Size == PersonaSize.Size100 || Size == PersonaSize.Size120)
            //        styles += "display:block;";

            //    return styles;
            //}


        }
}
