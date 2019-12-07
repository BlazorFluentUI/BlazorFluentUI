using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorFabric
{
    public partial class Persona : FabricComponentBase
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
        [Parameter] public PersonaSize Size { get; set; } = PersonaSize.Size48;
        [Parameter] public string TertiaryText { get; set; }
        [Parameter] public string Text { get; set; }

        protected string GetRootStyles()
        {
            string style = "";
            if (CoinSize != -1)
            {
                style += $"height:{CoinSize}px;min-width:{CoinSize}px;";
            }
            else
            {
                switch (Size)
                {
                    case PersonaSize.Size8:
                        style += $"height:8px;min-width:8px;";
                        break;
                    case PersonaSize.Size24:
                        if (ShowSecondaryText)
                        {
                            style += $"height:36px;min-width:24px;";
                        }
                        else
                        {
                            style += $"height:24px;min-width:24px;";
                        }
                        break;
                    case PersonaSize.Size32:
                        style += $"height:32px;min-width:32px;";
                        break;
                    case PersonaSize.Size48:
                        break;
                    case PersonaSize.Size56:
                        style += $"height:56px;min-width:56px;";
                        break;
                    case PersonaSize.Size72:
                        style += $"height:72px;min-width:72px;";
                        break;
                    case PersonaSize.Size100:
                        style += $"height:100px;min-width:100px;";
                        break;
                    case PersonaSize.Size120:
                        style += $"height:120px;min-width:120px;";
                        break;
                }
            }

            return style;
        }

        protected string GetDetailsStyles()
        {
            string styles = "";

            if (Size == PersonaSize.Size8)
                styles += "padding-left:17px;";
            else if (Size == PersonaSize.Size24 || Size == PersonaSize.Size32)
                styles += "padding:0 8px;";
            else if (Size == PersonaSize.Size40 || Size == PersonaSize.Size48)
                styles += "padding:0 12px;";

            return styles;
        }

        protected string GetPrimaryTextStyles()
        {
            string styles = "";

            if (ShowSecondaryText)
                styles += $"height:16px;line-height:16px;overflow-x:hidden;";

            if (Size == PersonaSize.Size8)
                styles += "font-size:12px;line-height:8px;";
            
            if ((Size == PersonaSize.Size24 || Size == PersonaSize.Size32 || Size == PersonaSize.Size40 || Size == PersonaSize.Size48) && ShowSecondaryText)
                styles += "height:18px;";
            
            if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120))
                styles += "font-size:21px;";

            if ((Size==PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120) && ShowSecondaryText)
                styles += "height:22px;";

            return styles;
        }

        protected string GetSecondaryTextStyles()
        {
            string styles = "";

            if (Size == PersonaSize.Size8 || Size == PersonaSize.Size24 || Size == PersonaSize.Size32)
                styles += "display:none;";

            if (ShowSecondaryText)
                styles += "display:block;height:16px;line-height:16px;overflow-x:hidden;";

            if (Size == PersonaSize.Size24 && ShowSecondaryText)
                styles += "height:18px;";

            if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120))
                styles += "font-size:14px;";

            if ((Size == PersonaSize.Size56 || Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120) && ShowSecondaryText)
                styles += "height:18px;";

            Debug.WriteLine(styles);
            return styles;
        }

        protected string GetTertiaryTextStyles()
        {
            string styles = "font-size:14px;";
                                             
            if (Size == PersonaSize.Size72 || Size == PersonaSize.Size100 || Size == PersonaSize.Size120)
                styles += "display:block;";

            return styles;
        }

        protected string GetOptionalTextStyles()
        {
            string styles = "font-size:14px;";

            if (Size == PersonaSize.Size100 || Size == PersonaSize.Size120)
                styles += "display:block;";

            return styles;
        }


    }
}
