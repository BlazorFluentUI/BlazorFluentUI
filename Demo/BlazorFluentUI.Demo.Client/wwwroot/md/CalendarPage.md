@page "/calendarPage"
@using System.ComponentModel.DataAnnotations

<header class="root">
    <h1 class="title">Calendar</h1>
</header>
<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading hiddenContent">Overview</h2>
    </div>
    <div class="content">
        <div class="ms-Markdown">
            <p>
                The calendar control lets people select and view a single date or a range of dates in their calendar. It’s made up of 3 separate views: the month view, year view, and decade view.
            </p>
        </div>
    </div>
</div>

<div class="section" style="transition-delay: 0s;">
    <div id="overview" tabindex="-1">
        <h2 class="subHeading">Usage</h2>
    </div>
    <div>
        <div class="subSection">
            <Demo Header="single date range" Key="0" MetadataPath="CalendarPage">
                Calendar Date selected: @(((DateTime)calendar1Value).ToString("D"))
                <br />
                Calendar Date range: @(calendar1Range != null && calendar1Range.Count > 0 ? calendar1Range.Min().ToString("D") + " - " + calendar1Range.Max().ToString("D") : "")
                <Calendar @bind-Value=@calendar1Value @bind-Range=@calendar1Range />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with week range" Key="1" MetadataPath="CalendarPage">
                Calendar Date selected: @(((DateTime)calendar2Value).ToString("D"))
                <br />
                Calendar Date range: @(calendar2Range != null && calendar2Range.Count > 0 ? calendar2Range.Min().ToString("D") + " - " + calendar2Range.Max().ToString("D") : "")
                <Calendar DateRangeType="DateRangeType.Week" @bind-Value="calendar2Value" @bind-Range="calendar2Range" />
            </Demo>

        </div>
        <div class="subSection">
<Demo Header="Calendar with month range" Key="2" MetadataPath="CalendarPage">

    Calendar Date selected: @(((DateTime)calendar3Value).ToString("D"))
    <br />
    Calendar Date range: @(calendar3Range != null && calendar3Range.Count > 0 ? calendar3Range.Min().ToString("D") + " - " + calendar3Range.Max().ToString("D") : "")

    <Calendar DateRangeType="DateRangeType.Month" @bind-Value="calendar3Value" @bind-Range="calendar3Range" />
</Demo>
        </div>
        <div class="subSection">
<Demo Header="Calendar with minimum, maximum, and restricted dates" Key="3" MetadataPath="CalendarPage">
    Minimum date: @minDate.ToString("D")
    <br />
    Maximum date: @maxDate.ToString("D")
    <br />
    Restricted dates: @(restrictedDates != null ? string.Join(", ", restrictedDates.Select(x => x.ToShortDateString())) : "")
    <br />
    Calendar Date selected: @((DateTime)calendar4Value).ToString("D")
    <br />
    Calendar Date range: @(calendar4Range != null && calendar4Range.Count > 0 ? calendar4Range.Min().ToString("D") + " - " + calendar4Range.Max().ToString("D") : "")

    <Calendar @bind-Value="calendar4Value" @bind-Range="calendar4Range" MinDate="minDate" MaxDate="maxDate" RestrictedDates="restrictedDates" />
</Demo>
        </div>
        <div class="subSection">
<Demo Header="Calendar with just dates, Header click reveals months" Key="4" MetadataPath="CalendarPage">
    Calendar Date selected: @(((DateTime)calendar1Value).ToString("D"))
    <br />
    Calendar Date range: @(calendar1Range != null && calendar1Range.Count > 0 ? calendar1Range.Min().ToString("D") + " - " + calendar1Range.Max().ToString("D") : "")

    <Calendar @bind-Value=@calendar1Value @bind-Range=@calendar1Range ShowMonthPickerAsOverlay="true" />
</Demo>
        </div>
        <div class="subSection">
<Demo Header="Calendar with Blazor Forms Validation" Key="5" MetadataPath="CalendarPage">
    <EditForm Model=@exampleModel OnValidSubmit=@HandleValidSubmit>
        <DataAnnotationsValidator />
        <ValidationSummary />
        <Calendar @bind-Value=@exampleModel.DateTime @bind-Range=@calendar1Range ShowMonthPickerAsOverlay="true" />
        <SubmitButton Text="Submit" />
    </EditForm>
</Demo>
        </div>
    </div>
</div>


@code {

        DateTime? calendar1Value = DateTime.Now;
        System.Collections.Generic.List<DateTime> calendar1Range;

        DateTime? calendar2Value = DateTime.Now;
        System.Collections.Generic.List<DateTime> calendar2Range;

        DateTime? calendar3Value = DateTime.Now;
        System.Collections.Generic.List<DateTime> calendar3Range;

        DateTime? calendar4Value = DateTime.Now;
        System.Collections.Generic.List<DateTime> calendar4Range;
        DateTime maxDate = DateTime.Now + TimeSpan.FromDays(60);
        DateTime minDate = DateTime.Now - TimeSpan.FromDays(60);
        System.Collections.Generic.List<DateTime> restrictedDates = new System.Collections.Generic.List<DateTime>() { DateTime.Now + TimeSpan.FromDays(4), DateTime.Now + TimeSpan.FromDays(5) };

        ExampleModel exampleModel = new ExampleModel();

    class MondayValidation : ValidationAttribute
    {
        public MondayValidation()
        {
            ErrorMessage = "Only Mondays are allowed.";
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
               
                if (dt.DayOfWeek == DayOfWeek.Monday)
                    return true;
            }
            return false;
        }
    }

    class ExampleModel
    {
        [MondayValidation]
        public DateTime? DateTime { get; set; }

    }

    public void HandleValidSubmit()
    {
    }
}
