using FluentUI.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class Rating : FluentUIComponentBase
    {
        private double rating = -1;

        protected ElementReference[] starReferences { get; set; }

        [Parameter]
        public bool AllowZeroStars { get; set; }

        [Parameter]
        public string IconName { get; set; } = "FavoriteStarFill";
        [Parameter]
        public int Max { get; set; } = 5;
        [Parameter]
        public double RatingValue
        {
            get => rating;
            set
            {
                if (value == rating)
                {
                    return;
                }
                rating = value;
                RatingValueChanged.InvokeAsync(value);
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
        public EventCallback<double> RatingValueChanged { get; set; }
        [Parameter]
        public EventCallback<double> OnChange { get; set; }

        private const int ratingLargeIconSize = 20;
        private const int ratingSmallIconSize = 16;
        private const int ratingVerticalPadding = 8;
        private const int ratingHorizontalPadding = 2;

        protected override Task OnInitializedAsync()
        {
            RatingValue = GetRatingSecure();
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

            RatingValue = value;
            return Task.CompletedTask;
        }

        private double GetRatingSecure()
        {
            return Math.Min(Math.Max(RatingValue, (AllowZeroStars ? 0 : 1)), Max);
        }

        protected double GetFullRating()
        {
            return Math.Ceiling(RatingValue);
        }

        protected double GetPercentageOf(int starNumber)
        {
            double fullRating = GetFullRating();
            double fullStar = 100;

            if (starNumber == RatingValue)
            {
                fullStar = 100;
            }
            else if (starNumber == fullRating)
            {
                fullStar = 100 * (RatingValue % 1);
            }
            else if (starNumber > fullRating)
            {
                fullStar = 0;
            }

            return fullStar;
        }


    }
}
