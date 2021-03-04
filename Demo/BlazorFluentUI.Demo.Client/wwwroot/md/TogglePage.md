﻿@page "/togglePage"


    <BFUStack Tokens=@(new BFUStackTokens { ChildrenGap = new double[] { 10 } })>
        <h1>Toggle</h1>

        <BFUToggle DefaultChecked="false" OnText="On!" OffText="Off!" Label="This is an uncontrolled toggle." />

        <BFUToggle DefaultChecked="false" Disabled="true" OnText="On!" OffText="Off!" Label="This is a disabled off toggle." />

        <BFUToggle DefaultChecked="true" Disabled="true" OnText="On!" OffText="Off!" Label="This is a disabled on toggle." />

        <BFUToggle Checked=@IsChecked CheckedChanged=@OnChecked OnText="On!" OffText="Off!" Label="This is a controlled toggle." />

        <BFUToggle DefaultChecked="false" OnText="On!" OffText="Off!" InlineLabel="true" Label="This is an inline toggle." />

        <BFUToggle @bind-Checked=@BoundChecked OnText="On!" OffText="Off!" Label="This is a toggle using binding." />

    </BFUStack>

@code {

    private bool? IsChecked=false;

    private bool? BoundChecked=false;

    Task OnChecked(bool? isChecked)
    {
        IsChecked = isChecked;
        return Task.CompletedTask;
    }

}
