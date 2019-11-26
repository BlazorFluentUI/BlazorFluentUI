using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        public double Rating {
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
        public EventCallback<double> OnChange{ get; set; }


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
            //isFocused = true;
            //StateHasChanged();
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

    }
}
