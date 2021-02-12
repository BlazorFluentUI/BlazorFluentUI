using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class PersonaPresence : FluentUIComponentBase
    {
        [Parameter] public int CoinSize { get; set; }
        [Parameter] public bool IsOutOfOffice { get; set; }
        [Parameter] public PersonaPresenceStatus Presence { get; set; }
        [Parameter] public string PresenceTitle { get; set; }  //tooltip on hover
        [Parameter] public string Size { get; set; }

        protected bool RenderIcon;
        private string _presenceHeightWidth;
        private string _presenceFontSize;

        private const int coinSizeFontScaleFactor = 6;
        private const int coinSizePresenceScaleFactor = 3;
        private const int presenceMaxSize = 40;
        private const int presenceFontMaxSize = 20;

        private ICollection<IRule> PersonaPresenceLocalRules { get; set; } = new List<IRule>();
        private Rule PresenceRule = new Rule();
        private Rule PresenceAfterRule = new Rule();
        private Rule PresenceBeforeRule = new Rule();
        private Rule IconRule = new Rule();

        private const string LocalSpecificityClass = "localPersonaPresenceRule";


        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (CoinSize != -1)
            {
                _presenceHeightWidth = CoinSize / coinSizePresenceScaleFactor < presenceMaxSize
                    ? CoinSize / coinSizePresenceScaleFactor + "px"
                    : presenceMaxSize + "px";

                _presenceFontSize = CoinSize / coinSizeFontScaleFactor < presenceFontMaxSize
                    ? CoinSize / coinSizeFontScaleFactor + "px"
                    : presenceFontMaxSize + "px";
            }

            RenderIcon = !(Size == PersonaSize.Size8 || Size == PersonaSize.Size24 || Size == PersonaSize.Size32) && (CoinSize != -1 ? CoinSize > 32 : true);

            SetStyle();

            return base.OnParametersSetAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        //protected string GetSizeStyle()
        //{
        //    if (CoinSize != -1)
        //    {
        //        return $"width:{_presenceHeightWidth}px;height:{_presenceHeightWidth}px;{Style}";
        //    }
        //    else
        //        return Style;
        //}

        //protected string GetIconStyle()
        //{
        //    if (CoinSize != -1)
        //    {
        //        return $"font-size:{_presenceFontSize};line-height: {_presenceHeightWidth};";
        //    }
        //    else
        //        return "";
        //}

        protected string DetermineIcon(PersonaPresenceStatus presence, bool isOutofOffice)
        {
            if (presence == PersonaPresenceStatus.None)
                return "";
            var oofIcon = "SkypeArrow";

            switch (presence)
            {
                case PersonaPresenceStatus.Online:
                    return "SkypeCheck";
                case PersonaPresenceStatus.Away:
                    return isOutofOffice ? oofIcon : "SkypeClock";
                case PersonaPresenceStatus.DND:
                    return "SkypeMinus";
                case PersonaPresenceStatus.Offline:
                    return isOutofOffice ? oofIcon : "";
                default:
                    return "";
            }

        }


        private void CreateLocalCss()
        {
            PresenceRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-presence", LiteralPrefix = $".{LocalSpecificityClass}" };
            PresenceAfterRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-presence", PseudoElement = PseudoElements.After, LiteralPrefix = $".{LocalSpecificityClass}" };
            PresenceBeforeRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-presence", PseudoElement = PseudoElements.Before, LiteralPrefix = $".{LocalSpecificityClass}" };
            IconRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-icon", LiteralPrefix = $".{LocalSpecificityClass}" };

            PersonaPresenceLocalRules.Add(PresenceRule);
            PersonaPresenceLocalRules.Add(PresenceAfterRule);
            PersonaPresenceLocalRules.Add(PresenceBeforeRule);
            PersonaPresenceLocalRules.Add(IconRule);
        }

        private void SetStyle()
        {
            var borderSize = (Size == PersonaSize.Size72 || Size == PersonaSize.Size100 ? "2px" : "1px");
            var isOpenCirclePresence = Presence == PersonaPresenceStatus.Offline || (IsOutOfOffice && (Presence == PersonaPresenceStatus.Online || Presence == PersonaPresenceStatus.Busy || Presence == PersonaPresenceStatus.Away || Presence == PersonaPresenceStatus.DND));

            if (_presenceHeightWidth != null)
            {
                PresenceRule.Properties = new CssString
                {
                    Css = $"width:{_presenceHeightWidth}px;" +
                          $"height:{_presenceHeightWidth}px;"
                };

                IconRule.Properties = new CssString
                {
                    Css = $"font-size:{_presenceFontSize}px;" +
                          $"line-height:{_presenceHeightWidth}px;"
                };
            }

            string pRight = null;
            string pTop = null;
            string pLeft = null;
            string pBorder = null;
            string pHeight = null;
            string pWidth = null;
            string pBackgroundColor = null;

            string bpBorderColor = PresenceColor.Busy;

            if (Size == PersonaSize.Size8)
            {
                pRight = "auto";
                pTop = "7px";
                pLeft = "0";
                pBorder = "0";
            }
            if (Size == PersonaSize.Size8 || Size == PersonaSize.Size24 || Size == PersonaSize.Size32)
            {
                pWidth = PersonaPresenceSize.Size8;
                pHeight = PersonaPresenceSize.Size8;
            }
            else if (Size == PersonaSize.Size40 || Size == PersonaSize.Size48)
            {
                pWidth = PersonaPresenceSize.Size12;
                pHeight = PersonaPresenceSize.Size12;
            }
            else if (Size == PersonaSize.Size56)
            {
                pWidth = PersonaPresenceSize.Size16;
                pHeight = PersonaPresenceSize.Size16;
            }
            else if (Size == PersonaSize.Size72)
            {
                pWidth = PersonaPresenceSize.Size20;
                pHeight = PersonaPresenceSize.Size20;
            }
            else if (Size == PersonaSize.Size100)
            {
                pWidth = PersonaPresenceSize.Size28;
                pHeight = PersonaPresenceSize.Size28;
            }
            else if (Size == PersonaSize.Size120)
            {
                pWidth = PersonaPresenceSize.Size32;
                pHeight = PersonaPresenceSize.Size32;
            }

            if (Presence == PersonaPresenceStatus.Online)
                pBackgroundColor = PresenceColor.Available;

            if (Presence == PersonaPresenceStatus.Away)
                pBackgroundColor = PresenceColor.Away;
            if (Presence == PersonaPresenceStatus.Blocked)
            {
                if (Size == PersonaSize.Size40 || Size == PersonaSize.Size48 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100)
                {
                    PresenceAfterRule.Properties = new CssString
                    {
                        Css = $"content:'';" +
                              $"width:100%;" +
                              $"height:{borderSize};" +
                              $"background-color:{PresenceColor.Busy};" +
                              $"transform:translateY(-50%) rotate(-45deg);" +
                              $"position:absolute;" +
                              $"top:50%;" +
                              $"left:0;"
                    };
                }
                else
                {
                    PresenceAfterRule.Properties = new CssString { Css = "" };
                }
            }
            if (Presence == PersonaPresenceStatus.Busy)
                pBackgroundColor = PresenceColor.Busy;
            if (Presence == PersonaPresenceStatus.DND)
                pBackgroundColor = PresenceColor.Dnd;
            if (Presence == PersonaPresenceStatus.Offline)
                pBackgroundColor = PresenceColor.Offline;


            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Online)
            {
                bpBorderColor = PresenceColor.Available;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Busy)
            {
                bpBorderColor = PresenceColor.Busy;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Away)
            {
                bpBorderColor = PresenceColor.Oof;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.DND)
            {
                bpBorderColor = PresenceColor.Dnd;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Offline)
            {
                bpBorderColor = PresenceColor.Offline;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Offline && IsOutOfOffice)
            {
                bpBorderColor = PresenceColor.Oof;
            }


            if (isOpenCirclePresence || Presence == PersonaPresenceStatus.Blocked)
            {
                pBackgroundColor = Theme.SemanticColors.BodyBackground;

                PresenceBeforeRule.Properties = new CssString
                {
                    Css = $"content:'';" +
                        $"width:100%;" +
                        $"height:100%;" +
                        $"position:absolute;" +
                        $"top:0;" +
                        $"left:0;" +
                        $"border:{borderSize} solid {bpBorderColor};" +
                        $"border-radius:50%;" +
                        $"box-sizing:border-box;"
                };
            }

            PresenceRule.Properties = new CssString
            {
                Css = (pWidth != null ? $"width:{pWidth};": "") +
                      (pHeight != null ? $"height:{pHeight};" : "") +
                      (pRight != null ? $"right:{pRight};" : "") +
                      (pTop != null ? $"top:{pTop};" : "") +
                      (pLeft != null ? $"left:{pLeft};" : "") +
                      (pBorder != null ? $"border:{pBorder};" : "") +
                      (pBackgroundColor != null ? $"background-color:{pBackgroundColor};" : "")
            };

            string iPosition = null;
            string iFontSize = null;
            string iLineHeight = null;
            string iLeft = null;
            string iColor = null;
            string iBorderColor = null;

            switch (Size)
            {
                case PersonaSize.Size56:
                    iFontSize = "8px";
                    iLineHeight = PersonaPresenceSize.Size16;
                    break;
                case PersonaSize.Size72:
                    iFontSize = Theme.FontStyle.FontSize.Small;
                    iLineHeight = PersonaPresenceSize.Size20;
                    break;
                case PersonaSize.Size100:
                    iFontSize = Theme.FontStyle.FontSize.Medium;
                    iLineHeight = PersonaPresenceSize.Size28;
                    break;
                case PersonaSize.Size120:
                    iFontSize = Theme.FontStyle.FontSize.Medium;
                    iLineHeight = PersonaPresenceSize.Size32;
                    break;
            }
            if (Presence == PersonaPresenceStatus.Away)
            {
                iPosition = "relative";
                if (!isOpenCirclePresence)
                    iLeft = "1px";
            }
            if (isOpenCirclePresence)
            {
                if (Presence == PersonaPresenceStatus.Online)
                {
                    iColor = PresenceColor.Available;
                    iBorderColor = PresenceColor.Available;
                }
                else if (Presence == PersonaPresenceStatus.Busy)
                {
                    iColor = PresenceColor.Busy;
                    iBorderColor = PresenceColor.Busy;
                }
                else if (Presence == PersonaPresenceStatus.Away)
                {
                    iColor = PresenceColor.Away;
                    iBorderColor = PresenceColor.Away;
                }
                else if (Presence == PersonaPresenceStatus.DND)
                {
                    iColor = PresenceColor.Dnd;
                    iBorderColor = PresenceColor.Dnd;
                }
                else if (Presence == PersonaPresenceStatus.Offline)
                {
                    iColor = PresenceColor.Offline;
                    iBorderColor = PresenceColor.Offline;
                }
                if (Presence == PersonaPresenceStatus.Offline && IsOutOfOffice)
                {
                    iColor = PresenceColor.Oof;
                    iBorderColor = PresenceColor.Oof;
                }
            }


            IconRule.Properties = new CssString
            {
                Css = (iPosition != null ? $"position:{iPosition};" : "")+
                      (iFontSize != null ? $"font-size:{iFontSize};" : "") +
                      (iLineHeight != null ? $"line-height:{iLineHeight};" : "") +
                      (iLeft != null ? $"left:{iLeft};" : "") +
                      (iColor != null ? $"color:{iColor};" : "") +
                      (iBorderColor != null ? $"border-color:{iBorderColor};" : "")
            };
        }

    }
}
