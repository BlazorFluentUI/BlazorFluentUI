using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class PersonaCoin : FluentUIComponentBase
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
        [Parameter] public PersonaInitialsColor InitialsColor { get; set; }
        [Parameter] public bool ShowInitialsUntilImageLoads { get; set; }
        [Parameter] public bool ShowUnknownPersonaCoin { get; set; }
        [Parameter] public string Size { get; set; }
        [Parameter] public string Text { get; set; }

        protected bool ShouldRenderInitials => !_isImageLoaded && (ShowInitialsUntilImageLoads && ImageUrl != null) || (ImageUrl == null || _isImageError || _hideImage);

        private bool _isImageLoaded;
        private bool _isImageError;
        private bool _hideImage;

        Regex UNWANTED_CHARS_REGEX = new Regex(@"\([^)]*\)|[\0-\u001F\!-/:-@\[-`\{-\u00BF\u0250-\u036F\uD800-\uFFFF]");
        Regex PHONENUMBER_REGEX = new Regex(@"^\d+[\d\s]*(:?ext|x|)\s*\d+$");
        Regex MULTIPLE_WHITESPACES_REGEX = new Regex(@"\s+");
        Regex UNSUPPORTED_TEXT_REGEX = new Regex(@"[\u0600-\u06FF\u0750-\u077F\u08A0-\u08FF\u1100-\u11FF\u3130-\u318F\uA960-\uA97F\uAC00-\uD7AF\uD7B0-\uD7FF\u3040-\u309F\u30A0-\u30FF\u3400-\u4DBF\u4E00-\u9FFF\uF900-\uFAFF]|[\uD840-\uD869][\uDC00-\uDED6]");

        private const string LocalSpecificityClass = "localPersonaCoinRule";

        private ICollection<IRule> PersonaCoinLocalRules { get; set; } = new List<IRule>();
        private Rule InitialsRule = new Rule();
        private Rule ImageAreaRule = new Rule();
        private Rule ImageRule = new Rule();
        

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

        protected Task OnPhotoLoadingStateChange(ImageLoadState imageLoadState)
        {
            _isImageLoaded = (imageLoadState == ImageLoadState.Loaded);
            _isImageError = (imageLoadState == ImageLoadState.Error);

            return Task.CompletedTask;
        }
       
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


        private void CreateLocalCss()
        {
            InitialsRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-initials", LiteralPrefix = $".{LocalSpecificityClass}" };
            ImageAreaRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-imageArea", LiteralPrefix = $".{LocalSpecificityClass}" };
            ImageRule.Selector = new ClassSelector() { SelectorName = "ms-Persona-image", LiteralPrefix = $".{LocalSpecificityClass}" };

            PersonaCoinLocalRules.Add(InitialsRule);
            PersonaCoinLocalRules.Add(ImageAreaRule);
            PersonaCoinLocalRules.Add(ImageRule);
        }

        private void SetStyle()
        {
            var dimension = CoinSize != -1 ? CoinSize : PersonaSize.SizeToPixels(Size);
            string fontSize = dimension switch
            {
                < 32 => Theme.FontStyle.FontSize.XSmall,
                >= 32 and < 40 => Theme.FontStyle.FontSize.Medium,
                >= 40 and < 48 => Theme.FontStyle.FontSize.MediumPlus,
                >= 48 and < 56 => Theme.FontStyle.FontSize.Large,
                >= 56 and < 72 => Theme.FontStyle.FontSize.XLarge,
                >= 72 and < 100 => Theme.FontStyle.FontSize.XxLarge,
                >= 100 => Theme.FontStyle.FontSize.SuperLarge,
            };

            string color = InitialsColor.ToString() != "" ? PersonaColorUtils.GetPersonaColorHexCode(InitialsColor) : PersonaColorUtils.GetPersonaColorHexCode(PersonaColorUtils.GetInitialsColorFromName(Text));

            InitialsRule.Properties = new CssString
            {

                Css = $"height:{dimension}px;" +
                     $"background-color:{color};" +
                     $"line-height:{(dimension == 48 ? 46 : dimension)}px;"+
                     $"font-size:{fontSize};"
            };


            ImageAreaRule.Properties = new CssString
            {
                Css = (dimension <= 10 ? 
                    $"overflow:visible;"+
                    $"background:transparent;"+
                    $"height:0;"+
                    $"width:0;"
                    :
                    $"width:{dimension}px;" +
                    $"height:{dimension}px;"
                   )
            };

            ImageRule.Properties = new CssString
            {
                Css = (dimension <= 10 ?
                    $"overflow:visible;" +
                    $"background:transparent;" +
                    $"height:0;" +
                    $"width:0;"
                    :
                    $"width:{dimension}px;" +
                    $"height:{dimension}px;"
                   )
            };


        }




    }
}
