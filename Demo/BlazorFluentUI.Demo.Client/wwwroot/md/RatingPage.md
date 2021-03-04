﻿@page "/ratingPage"

<h1>Rating</h1>

<BFULabel>Large Stars:</BFULabel>
<BFURating Size="RatingSize.Large" />

<BFULabel>Small Stars:</BFULabel>
<BFURating Rating="3" />

<BFULabel>10 Small Stars:</BFULabel>
<BFURating Max="10" />

<BFULabel>Disabled:</BFULabel>
<BFURating Disabled="true" />

<BFULabel>Half star in readOnly mode:</BFULabel>
<BFURating ReadOnly="true" Rating="2.5" />

<BFULabel>Custom icons:</BFULabel>
<BFURating Rating="2.5" IconName="StarburstSolid" UnselectedIcon="Starburst" />

<BFULabel>Two-Way-Binding:</BFULabel>
<BFURating Max="10" @bind-Rating="rateten" />
@rateten of 10 Stars selected!

<BFULabel>Button binded rating (One-Way-Binding):</BFULabel>
<BFURating AllowZeroStars="true" Rating="rate" ReadOnly="true" />
<BFUPrimaryButton OnClick="SetRate" Text="@buttonText" />

@code
{
    private double rate = 0;
    private double rateten = 1;
    private string buttonText = "Click to change rating to 5";
    private void SetRate()
    {
        Console.WriteLine(rateten);
        if (rate == 0)
        {
            buttonText = "Click to change rating to 0";
            rate = 5;
        }
        else
        {
            buttonText = "Click to change rating to 5";
            rate = 0;
        }
    }
}


