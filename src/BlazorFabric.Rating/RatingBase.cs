using BlazorFabric.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class RatingBase : FabricComponentBase
    {
        private double rating = -1;

        protected ElementReference[] starReferences { get; set; }

        [Parameter]
        public bool AllowZeroStars { get; set; }
        [Parameter]
        public string Icon { get; set; } = "FavoriteStarFill";
        [Parameter]
        public int Max { get; set; } = 5;
        [Parameter]
        public double Rating
        {
            get => rating;
            set
            {
                if (value == rating)
                {
                    return;
                }
                rating = value;
                RatingChanged.InvokeAsync(value);
                OnChange.InvokeAsync(value);
                //StateHasChanged();
            }
        }
        [Parameter]
        public bool Disabled { get; set; }
        [Parameter]
        public bool ReadOnly { get; set; }
        [Parameter]
        public RatingSize Size { get; set; } = RatingSize.Small;
        [Parameter]
        public string UnselectedIcon { get; set; } = "FavoriteStar";
        [Parameter]
        public Func<double, double, string> GetAriaLabel { get; set; }
        [Parameter]
        public EventCallback<double> RatingChanged { get; set; }
        [Parameter]
        public EventCallback<double> OnChange { get; set; }

        private const int ratingLargeIconSize = 20;
        private const int ratingSmallIconSize = 16;
        private const int ratingVerticalPadding = 8;
        private const int ratingHorizontalPadding = 2;

        protected override Task OnInitializedAsync()
        {
            Rating = GetRatingSecure();
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (starReferences == null)
            {
                starReferences = new ElementReference[Max];
            }
            else if (Max != starReferences.Length)
            {
                starReferences = new ElementReference[Max];
            }

            return base.OnParametersSetAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);

        }

        protected Task OnFocus(ChangeEventArgs args)
        {
            Console.WriteLine("Focused");
            return Task.CompletedTask;
        }

        protected Task OnClick(int value)
        {
            if (ReadOnly || Disabled)
                return Task.CompletedTask;

            Rating = value;
            return Task.CompletedTask;
        }

        private double GetRatingSecure()
        {
            return Math.Min(Math.Max(Rating, (AllowZeroStars ? 0 : 1)), Max);
        }

        protected double GetFullRating()
        {
            return Math.Ceiling(Rating);
        }

        protected double GetPercentageOf(int starNumber)
        {
            double fullRating = GetFullRating();
            double fullStar = 100;

            if (starNumber == Rating)
            {
                fullStar = 100;
            }
            else if (starNumber == fullRating)
            {
                fullStar = 100 * (Rating % 1);
            }
            else if (starNumber > fullRating)
            {
                fullStar = 0;
            }

            return fullStar;
        }

        protected ICollection<Rule> CreateGlobalCss()
        {
            var ratingGlobalRules = new HashSet<Rule>();
            var FocusStyleRatingFocusZone = FocusStyle.GetFocusStyle(new FocusStyleProps(Theme), ".ms-Rating-focuszone");
            var FocusStyleRatingButton = FocusStyle.GetFocusStyle(new FocusStyleProps(Theme), ".ms-Rating-button");
            foreach (var rule in FocusStyleRatingFocusZone.AddRules)
                ratingGlobalRules.Add(rule);
            foreach (var rule in FocusStyleRatingButton.AddRules)
                ratingGlobalRules.Add(rule);

            #region ms-RatingStar-root
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-RatingStar-root:hover .ms-RatingStar-back" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralPrimary};"
                }
            });
            #endregion
            #region ms-RatingStar-root--small 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-RatingStar-root--small" },
                Properties = new CssString()
                {
                    Css = $"height:{ratingSmallIconSize + ratingVerticalPadding * 2}px;"
                }
            });
            #endregion
            #region ms-RatingStar-root--large 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-RatingStar-root--large" },
                Properties = new CssString()
                {
                    Css = $"height:{ratingLargeIconSize + ratingVerticalPadding * 2}px;"
                }
            });
            #endregion
            #region ms-RatingStar-container 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-RatingStar-container" },
                Properties = new CssString()
                {
                    Css = $"display: inline-block;" +
                            $"position: relative;" +
                            $"height: inherit;"
                }
            });
            #endregion
            #region ms-RatingStar-back 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-RatingStar-back" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralSecondary};" +
                            $"width:100;"
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-RatingStar-back--disabled" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.DisabledBodySubtext};" +
                            $"width:100;"
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-RatingStar-back--disabled{color:GrayText;}"
                }
            });
            #endregion
            #region ms-RatingStar-front 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-RatingStar-front" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                            $"height:100%;" +
                            $"left:0;" +
                            $"top:0;" +
                            $"text-align:center;" +
                            $"vertical-align:middle;" +
                            $"overflow:hidden;" +
                            $"color:{Theme.Palette.NeutralPrimary};" +
                            $"width:0px"
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-RatingStar-front{color:Highlight;}"
                }
            });
            #endregion
            #region ms-Rating-button 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating-button" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;" +
                            $"padding:{ratingVerticalPadding}px {ratingHorizontalPadding}px;" +
                            $"box-sizing:content-box;" +
                            $"margin:0;" +
                            $"border:none;" +
                            $"cursor:pointer;" + FocusStyleRatingButton.MergeRules
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating-button:not(.ms-Rating-button--disabled):hover ~.ms-Rating-button .ms-RatingStar-back" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralSecondary};"
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating-button:not(.ms-Rating-button--disabled):hover ~.ms-Rating-button .ms-RatingStar-front" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralSecondary};"
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating-button--disabled" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;" +
                            $"padding:{ratingVerticalPadding}px {ratingHorizontalPadding}px;" +
                            $"box-sizing:content-box;" +
                            $"margin:0;" +
                            $"border:none;" +
                            $"cursor:default;" + FocusStyleRatingButton.MergeRules
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Rating-button:hover .ms-RatingStar-back" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemePrimary}"
                }
            });
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Rating-button:hover .ms-RatingStar-front" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDark}"
                }
            });
            #endregion
            #region ms-Rating--small 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating--small" },
                Properties = new CssString()
                {
                    Css = $"font-size:{ratingSmallIconSize}px;" +
                            $"line-height:{ratingSmallIconSize}px;" +
                            $"height:{ratingSmallIconSize}px;"
                }
            });
            #endregion
            #region ms-Rating--large 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating--large" },
                Properties = new CssString()
                {
                    Css = $"font-size:{ratingLargeIconSize}px;" +
                            $"line-height:{ratingLargeIconSize}px;" +
                            $"height:{ratingLargeIconSize}px;"
                }
            });
            #endregion
            #region ms-Rating-labelText 
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-Rating-labelText" },
                Properties = new CssString()
                {
                    Css = ContentStyle.HiddenContentStyle()
                }
            });
            #endregion
            #region ms-Rating-focuszone
            ratingGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Rating-focuszone" },
                Properties = new CssString()
                {
                    Css = $"display:inline-block;" + FocusStyleRatingFocusZone.MergeRules
                }
            });
            #endregion

            return ratingGlobalRules;
        }
    }
}
