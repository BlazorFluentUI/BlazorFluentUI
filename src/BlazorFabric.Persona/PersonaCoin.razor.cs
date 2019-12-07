using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class PersonaCoin : FabricComponentBase
    {
        [Parameter] public bool AllowPhoneInitials { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public int CoinSize { get; set; }
        [Parameter] public string ImageAlt { get; set; }
        [Parameter] public bool ImageShouldFadeIn { get; set; }
        [Parameter] public bool ImageShouldStartVisible { get; set; }
        [Parameter] public string ImageUrl { get; set; }
        [Parameter] public bool IsOutOfOffice { get; set; }
        [Parameter] public PersonaPresenceStatus Presence { get; set; }
        [Parameter] public string PresenceTitle { get; set; }  //tooltip on hover
        [Parameter] public bool ShowInitialsUntilImageLoads { get; set; }
        [Parameter] public bool ShowUnknownPersonaCoin { get; set; }
        [Parameter] public PersonaSize Size { get; set; }
        [Parameter] public string Text { get; set; }

        protected bool ShouldRenderInitials => !_isImageLoaded && (ShowInitialsUntilImageLoads && ImageUrl != null) || (ImageUrl == null || _isImageError || _hideImage);

        private bool _isImageLoaded;
        private bool _isImageError;
        private bool _hideImage;

        Regex UNWANTED_CHARS_REGEX = new Regex(@"\([^)]*\)|[\0-\u001F\!-/:-@\[-`\{-\u00BF\u0250-\u036F\uD800-\uFFFF]");
        Regex PHONENUMBER_REGEX = new Regex(@"^\d+[\d\s]*(:?ext|x|)\s*\d+$");
        Regex MULTIPLE_WHITESPACES_REGEX = new Regex(@"\s+");
        Regex UNSUPPORTED_TEXT_REGEX = new Regex(@"[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\u1100-\u11FF\u3130-\u318F\uA960-\uA97F\uAC00-\uD7AF\uD7B0-\uD7FF\u3040-\u309F\u30A0-\u30FF\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]|[\uD840-\uD869][\uDC00-\uDED6]");

        protected Task OnPhotoLoadingStateChange(ImageLoadState imageLoadState)
        {
            _isImageLoaded = (imageLoadState == ImageLoadState.Loaded);
            _isImageError = (imageLoadState == ImageLoadState.Error);

            return Task.CompletedTask;
        }

        protected string GetInitialsStyle()
        {
            string style = "";
            var dimension = CoinSize != -1 ? CoinSize : sizeToPixels[Size];

            style += $"width:{dimension}px;height:{dimension}px;";
            
            style += $"background-color:{PersonaColorUtils.GetPersonaColorHexCode(PersonaColorUtils.GetInitialsColorFromName(Text))};";

            style += $"line-height:{(dimension == 48 ? 46 : dimension)}px;";

            if (dimension < 32)
                style += $"font-size:var(--fontSize-XSmall);";
            else if (dimension >= 32 && dimension < 40)
                style += $"font-size:var(--fontSize-Medium);";
            else if (dimension >= 40 && dimension < 56)
                style += $"font-size:var(--fontSize-MediumPlus);";
            else if (dimension >= 56 && dimension < 72)
                style += $"font-size:var(--fontSize-XLarge);";
            else if (dimension >= 72 && dimension < 100)
                style += $"font-size:var(--fontSize-XxLarge);";
            else if (dimension >= 100)
                style += $"font-size:var(--fontSize-SuperLarge);";


            return style;
        }

        protected string GetImageAreaStyle()
        {
            string style = "";
            var dimension = CoinSize != -1 ? CoinSize : sizeToPixels[Size];
            
            style += $"width:{dimension}px;height:{dimension}px;";

            if (dimension <=10)
            {
                style += "overflow:visible;background:transparent;height:0;width:0;";
            }            

            return style;
        }
        protected string GetImageStyle()
        {
            string style = "";
            var dimension = CoinSize != -1 ? CoinSize : sizeToPixels[Size];

            if (dimension > 10)
            {
                style += $"width:{dimension}px;height:{dimension}px;";
            }
            else
            {
                style += "overflow:visible;background:transparent;height:0;width:0;";
            }

            return style;
        }


        protected Dictionary<PersonaSize, int> sizeToPixels = new Dictionary<PersonaSize, int>()
        {
            [PersonaSize.Size8] = 8,
            [PersonaSize.Size24] = 24,
            [PersonaSize.Size32] = 32,
            [PersonaSize.Size40] = 40,
            [PersonaSize.Size48] = 48,
            [PersonaSize.Size56] = 56,
            [PersonaSize.Size72] = 72,
            [PersonaSize.Size100] = 100,
            [PersonaSize.Size120] = 120
        };
       
        protected string GetInitials(string displayName, bool isRTL, bool allowPhoneInitials)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return "";

            displayName = CleanupDisplayName(displayName);

            // For names containing CJK characters, and phone numbers, we don't display initials
            if (UNSUPPORTED_TEXT_REGEX.IsMatch(displayName) || (!allowPhoneInitials && PHONENUMBER_REGEX.IsMatch(displayName)))
            {
                return "";
            }

            return GetInitialsLatin(displayName, isRTL);
        }

        private string CleanupDisplayName(string displayName)
        {
            displayName = UNWANTED_CHARS_REGEX.Replace(displayName, "");
            displayName = MULTIPLE_WHITESPACES_REGEX.Replace(displayName, " ");
            displayName = displayName.Trim();
            return displayName;
        }

        private string GetInitialsLatin(string displayName, bool isRtl)
        {
            var initials = "";

            string[] splits = displayName.Split(' ');

            if (splits.Length == 2)
            {
                initials += splits[0].ToUpper()[0];
                initials += splits[1].ToUpper()[0];
            }
            else if (splits.Length == 3)
            {
                initials += splits[0].ToUpper()[0];
                initials += splits[2].ToUpper()[0];
            }
            else if (splits.Length != 0)
            {
                initials += splits[0].ToUpper()[0];
            }

            if (isRtl && initials.Length > 1)
            {
                return "" + initials[1] + initials[0];
            }
            
            return initials;
        }
    }
}
