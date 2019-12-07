using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class PersonaPresence : FabricComponentBase
    {
        [Parameter] public int CoinSize { get; set; }
        [Parameter] public bool IsOutOfOffice { get; set; }
        [Parameter] public PersonaPresenceStatus Presence { get; set; }
        [Parameter] public string PresenceTitle { get; set; }  //tooltip on hover
        [Parameter] public PersonaSize Size { get; set; }

        protected bool RenderIcon;
        private string _presenceHeightWidth;
        private string _presenceFontSize;

        private const int coinSizeFontScaleFactor = 6;
        private const int coinSizePresenceScaleFactor = 3;
        private const int presenceMaxSize = 40;
        private const int presenceFontMaxSize = 20;

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

            return base.OnParametersSetAsync();
        }

        protected string GetSizeStyle()
        {
            if (CoinSize != -1)
            {
                return $"width:{_presenceHeightWidth}px;height:{_presenceHeightWidth}px;{Style}";
            }
            else
                return Style;
        }

        protected string GetIconStyle()
        {
            if (CoinSize != -1)
            {
                return $"font-size:{_presenceFontSize};line-height: {_presenceHeightWidth};";
            }
            else
                return "";
        }

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
    }
}
