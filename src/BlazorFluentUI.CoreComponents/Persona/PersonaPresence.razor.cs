using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class PersonaPresence : FluentUIComponentBase
    {
        [Parameter] public int CoinSize { get; set; }
        [Parameter] public bool IsOutOfOffice { get; set; }
        [Parameter] public PersonaPresenceStatus Presence { get; set; }
        [Parameter] public string? PresenceTitle { get; set; }  //tooltip on hover
        [Parameter] public string? Size { get; set; }

        protected bool RenderIcon;
        private string? _presenceHeightWidth;
        private string? _presenceFontSize;

        private const int coinSizeFontScaleFactor = 6;
        private const int coinSizePresenceScaleFactor = 3;
        private const int presenceMaxSize = 40;
        private const int presenceFontMaxSize = 32;

        private ICollection<IRule> PersonaPresenceLocalRules { get; set; } = new List<IRule>();
        private Rule PresenceRule = new();
        private Rule PresenceAfterRule = new();
        private Rule PresenceBeforeRule = new();
        private Rule IconRule = new();

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

                //_presenceFontSize = CoinSize / coinSizeFontScaleFactor < presenceFontMaxSize
                //    ? CoinSize / coinSizeFontScaleFactor + "px"
                //    : presenceFontMaxSize + "px";
                _presenceFontSize = CoinSize / coinSizePresenceScaleFactor < presenceMaxSize
                    ? CoinSize / coinSizePresenceScaleFactor + "px"
                    : presenceMaxSize + "px";
            }

            RenderIcon = !(Size == PersonaSize.Size8 || Size == PersonaSize.Size24 || Size == PersonaSize.Size32) && (CoinSize == -1 || CoinSize > 32);

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

        protected static string DetermineIcon(PersonaPresenceStatus presence, bool isOutofOffice)
        {
            if (presence == PersonaPresenceStatus.None)
                return "";
            string? oofIcon = "presence_oof";

            return presence switch
            {
                PersonaPresenceStatus.Online => "presence_available",
                PersonaPresenceStatus.Busy => "presence_busy",
                PersonaPresenceStatus.Away => isOutofOffice ? oofIcon : "presence_away",
                PersonaPresenceStatus.DND => "presence_dnd",
                PersonaPresenceStatus.Offline => isOutofOffice ? oofIcon : "presence_offline",
                _ => "presence_unknown",
            };
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
            string? borderSize = (Size == PersonaSize.Size72 || Size == PersonaSize.Size100 ? "2px" : "1px");
            bool isOpenCirclePresence = Presence == PersonaPresenceStatus.Offline || (IsOutOfOffice && (Presence == PersonaPresenceStatus.Online || Presence == PersonaPresenceStatus.Busy || Presence == PersonaPresenceStatus.Away || Presence == PersonaPresenceStatus.DND));

            if (_presenceHeightWidth != null)
            {
                PresenceRule.Properties = new CssString
                {
                    Css = $"width:{_presenceHeightWidth}px;" +
                          $"height:{_presenceHeightWidth}px;"
                };

                IconRule.Properties = new CssString
                {
                    Css = $"font-size:{_presenceHeightWidth}px;" +
                          $"line-height:{_presenceHeightWidth}px;"
                };
            }

            string? pRight = null;
            string? pTop = null;
            string? pLeft = null;
            string? pBorder = null;
            string? pHeight = null;
            string? pWidth = null;
            string? pBackgroundColor = null;

            string? iPosition = null;
            string? iFontSize = null;
            string? iLineHeight = null;
            string? iLeft = null;
            string? iColor = null;
            string? iBackgroundColor = null;


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
                iBackgroundColor = PresenceColor.Available;

            if (Presence == PersonaPresenceStatus.Away)
                iBackgroundColor = PresenceColor.Away;
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
                iBackgroundColor = PresenceColor.Busy;
            if (Presence == PersonaPresenceStatus.DND)
                iBackgroundColor = PresenceColor.Dnd;
            if (Presence == PersonaPresenceStatus.Offline)
                iBackgroundColor = PresenceColor.Offline;


            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Online)
            {
                bpBorderColor = PresenceColor.Available;
                pBackgroundColor = PresenceColor.Available;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Busy)
            {
                bpBorderColor = PresenceColor.Busy;
                pBackgroundColor = PresenceColor.Busy;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Away)
            {
                bpBorderColor = PresenceColor.Oof;
                pBackgroundColor = PresenceColor.Oof;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.DND)
            {
                bpBorderColor = PresenceColor.Dnd;
                pBackgroundColor = PresenceColor.Dnd;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Offline)
            {
                bpBorderColor = PresenceColor.Offline;
                pBackgroundColor = PresenceColor.Offline;
            }
            if (isOpenCirclePresence && Presence == PersonaPresenceStatus.Offline && IsOutOfOffice)
            {
                bpBorderColor = PresenceColor.Oof;
                pBackgroundColor = PresenceColor.Oof;
            }


            if (isOpenCirclePresence || Presence == PersonaPresenceStatus.Blocked)
            {
                pBackgroundColor = Theme?.SemanticColors.BodyBackground;

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

            
            switch (Size)
            {
                case PersonaSize.Size24:
                    iFontSize = PersonaPresenceSize.Size12; //"8px";
                    iLineHeight = PersonaPresenceSize.Size12;
                    break;
                case PersonaSize.Size32:
                    iFontSize = PersonaPresenceSize.Size12; //Theme?.FontStyle.FontSize.Small;
                    iLineHeight = PersonaPresenceSize.Size12;
                    break;
                case PersonaSize.Size40:
                    iFontSize = PersonaPresenceSize.Size16; //Theme?.FontStyle.FontSize.Medium;
                    iLineHeight = PersonaPresenceSize.Size16;
                    break;
                case PersonaSize.Size48:
                    iFontSize = PersonaPresenceSize.Size16; //Theme?.FontStyle.FontSize.Medium;
                    iLineHeight = PersonaPresenceSize.Size16;
                    break;
                case PersonaSize.Size56:
                    iFontSize = PersonaPresenceSize.Size16; //"8px";
                    iLineHeight = PersonaPresenceSize.Size16;
                    break;
                case PersonaSize.Size72:
                    iFontSize = PersonaPresenceSize.Size20; //Theme?.FontStyle.FontSize.Small;
                    iLineHeight = PersonaPresenceSize.Size20;
                    break;
                case PersonaSize.Size100:
                    iFontSize = PersonaPresenceSize.Size28; //Theme?.FontStyle.FontSize.Medium;
                    iLineHeight = PersonaPresenceSize.Size28;
                    break;
                case PersonaSize.Size120:
                    iFontSize = PersonaPresenceSize.Size32; //Theme?.FontStyle.FontSize.Medium;
                    iLineHeight = PersonaPresenceSize.Size32;
                    break;
            }
            if (Presence == PersonaPresenceStatus.Away)
            {
                iPosition = "relative";
                if (!isOpenCirclePresence)
                    iLeft = "0px"; // was 1px
            }
            if (isOpenCirclePresence)
            {
                switch (Presence)
                {
                    case PersonaPresenceStatus.Online:
                        iColor = PresenceColor.Available;
                        pBackgroundColor = PresenceColor.Available;
                        break;
                    case PersonaPresenceStatus.Busy:
                        iColor = PresenceColor.Busy;
                        pBackgroundColor = PresenceColor.Busy;
                        break;
                    case PersonaPresenceStatus.Away:
                        iColor = PresenceColor.Away;
                        pBackgroundColor = PresenceColor.Away;
                        break;
                    case PersonaPresenceStatus.DND:
                        iColor = PresenceColor.Dnd;
                        pBackgroundColor = PresenceColor.Dnd;
                        break;
                    case PersonaPresenceStatus.Offline:
                        iColor = PresenceColor.Offline;
                        pBackgroundColor = PresenceColor.Offline;
                        break;
                }
                if ((Presence == PersonaPresenceStatus.Offline || Presence == PersonaPresenceStatus.Away) && IsOutOfOffice)
                {
                    iColor = PresenceColor.Oof;
                    pBackgroundColor = PresenceColor.Oof;
                }
            }


            IconRule.Properties = new CssString
            {
                Css = (iPosition != null ? $"position:{iPosition};" : "")+
                      (iFontSize != null ? $"font-size:{iFontSize};" : "") +
                      (iLineHeight != null ? $"line-height:{iLineHeight};" : "") +
                      (iLeft != null ? $"left:{iLeft};" : "") +
                      (iColor != null ? $"color:{iColor};" : "") +
                      //(iBorderColor != null ? $"border-color:{iBorderColor};" : "")+
                      (iBackgroundColor != null ? $"color:{iBackgroundColor};" : "")
            };
        }

    }
}
