﻿@page "/ratingPage"

<h1>Rating</h1>

<Label>Large Stars:</Label>
<Rating Size="RatingSize.Large" />

<Label>Small Stars:</Label>
<Rating Rating="3" />

<Label>10 Small Stars:</Label>
<Rating Max="10" />

<Label>Disabled:</Label>
<Rating Disabled="true" />

<Label>Half star in readOnly mode:</Label>
<Rating ReadOnly="true" Rating="2.5" />

<Label>Custom icons:</Label>
<Rating Rating="2.5" IconName="StarburstSolid" UnselectedIcon="Starburst" />

<Label>Two-Way-Binding:</Label>
<Rating Max="10" @bind-Rating="rateten" />
@rateten of 10 Stars selected!

<Label>Button binded rating (One-Way-Binding):</Label>
<Rating AllowZeroStars="true" Rating="rate" ReadOnly="true" />
<PrimaryButton OnClick="SetRate" Text="@buttonText" />

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


