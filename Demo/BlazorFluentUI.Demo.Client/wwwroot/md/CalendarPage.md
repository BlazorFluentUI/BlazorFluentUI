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
            <Demo Header="Calendar" Key="0" MetadataPath="CalendarPage">
                Selected date: @(calendar1Value != null ? ((DateTime)calendar1Value).ToString("D") : "-")
                <Calendar @bind-Value=@calendar1Value />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with overlaid month picker when header is clicked" Key="1" MetadataPath="CalendarPage">
                Selected date: @(calendar1Value != null ? ((DateTime)calendar1Value).ToString("D") : "-")
                <br />
                Calendar Date range: @(calendar1Range != null && calendar1Range.Count > 0 ? calendar1Range.Min().ToString("D") + " - " + calendar1Range.Max().ToString("D") : "")

                <Calendar @bind-Value=@calendar1Value @bind-Range=@calendar1Range ShowMonthPickerAsOverlay="true" HighlightSelectedMonth="true" />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with week selection" Key="2" MetadataPath="CalendarPage">
                Selected date: @(calendar2Value != null ? ((DateTime)calendar2Value).ToString("D") : "-")
                <br />
                Selected range: @(calendar2Range != null && calendar2Range.Count > 0 ? calendar2Range.Min().ToString("D") + " - " + calendar2Range.Max().ToString("D") : "")
                <Calendar DateRangeType="DateRangeType.Week" @bind-Value="calendar2Value" @bind-Range="calendar2Range" HighlightSelectedMonth="true" />
            </Demo>

        </div>
        <div class="subSection">
            <Demo Header="Calendar with month selection" Key="3" MetadataPath="CalendarPage">
                Selected date: @(calendar3Value != null ? ((DateTime)calendar3Value).ToString("D") : "-")
                <br />
                Selected range: @(calendar3Range != null && calendar3Range.Count > 0 ? calendar3Range.Min().ToString("D") + " - " + calendar3Range.Max().ToString("D") : "")

                <Calendar DateRangeType="DateRangeType.Month" @bind-Value="calendar3Value" @bind-Range="calendar3Range" HighlightSelectedMonth="true" />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with week numbers" Key="4" MetadataPath="CalendarPage">
                Selected date: @(calendar4Value != null ? ((DateTime)calendar4Value).ToString("D") : "-")
                <br />
                <Calendar @bind-Value="calendar4Value" HighlightSelectedMonth="true" ShowWeekNumbers="true" />
            </Demo>
            </div>
        <div class="subSection">
            <Demo Header="Calendar displaying 6 weeks" Key="5" MetadataPath="CalendarPage">
                Selected date:@(calendar4Value != null ? ((DateTime)calendar4Value).ToString("D") : "-")
                <br />
                <Calendar @bind-Value="calendar4Value" HighlightSelectedMonth="true" ShowSixWeeksByDefault="true" />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with month picker and no day picker" Key="6" MetadataPath="CalendarPage">
                Selected date: @(calendar4Value != null ? ((DateTime)calendar4Value).ToString("D") : "-")
                <br />
                <Calendar @bind-Value="calendar4Value" DateRangeType="DateRangeType.Month" HighlightSelectedMonth="true" IsDayPickerVisible="false" />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar date boundary and disabled dates" Key="7" MetadataPath="CalendarPage">
                Selected date:@(calendar5Value != null ? ((DateTime)calendar5Value).ToString("D") : "-")
                <br />
                Date boundary: @(((DateTime)minDate).ToString("D")) - @(((DateTime)maxDate).ToString("D"))
                <br />
                Disabled dates: @(restrictedDates != null ? string.Join(", ", restrictedDates.Select(x => x.ToShortDateString())) : "")
                <br />
                <Calendar @bind-Value="calendar5Value" MinDate="(DateTime)minDate" MaxDate="(DateTime)maxDate" RestrictedDates="restrictedDates" HighlightSelectedMonth="true" />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with custom selectable days" Key="8" MetadataPath="CalendarPage">
                Selected date: @(calendar4Value != null ? ((DateTime)calendar4Value).ToString("D") : "-")
                <br />
                Selected range: @(calendar4Range != null && calendar4Range.Count > 0 ? calendar4Range.Min().ToString("D") + " - " + calendar4Range.Max().ToString("D") : "")
                <br />
                <Calendar @bind-Value="calendar4Value" DateRangeType="DateRangeType.WorkWeek" WorkWeekDays="workWeekDays" @bind-Range="calendar4Range" HighlightSelectedMonth="true" />
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar with custom selectable days, week starting on Monday" Key="9" MetadataPath="CalendarPage">
                Selected date: @(calendar4Value != null ? ((DateTime)calendar4Value).ToString("D") : "-")
                <br />
                Selected range: @(calendar4Range != null && calendar4Range.Count > 0 ? calendar4Range.Min().ToString("D") + " - " + calendar4Range.Max().ToString("D") : "")
                <br />
                <Calendar @bind-Value="calendar4Value" DateRangeType="DateRangeType.WorkWeek" WorkWeekDays="workWeekDays" FirstDayOfWeek="DayOfWeek.Monday" @bind-Range="calendar4Range" HighlightSelectedMonth="true" />
            </Demo>
        </div>

        <div class="subSection">
            <Demo Header="Calendar with Blazor Forms Validation" Key="10" MetadataPath="CalendarPage">
                <EditForm Model=@exampleModel OnValidSubmit=@HandleValidSubmit>
                    <DataAnnotationsValidator />
                    <FluentUIValidationSummary />
                    <Calendar @bind-Value=@exampleModel.DateTime @bind-Range=@calendar1Range ShowMonthPickerAsOverlay="true" />
                    <SubmitButton Text="Submit" />
                </EditForm>
            </Demo>
        </div>
        <div class="subSection">
            <Demo Header="Calendar launched from a button" Key="11" MetadataPath="CalendarPage">

                <DefaultButton OnClick=ClickHandler @ref="buttonContainerRef"
                               Text=@(calendar6Value == null ? "Click for Calendar" : ((DateTime)calendar6Value).ToString("D")) />

                @if (!calloutHidden)
                {
                    <Callout IsBeakVisible="false"
                             GapSpace="0"
                             DoNotLayer="false"
                             FabricComponentTarget="@buttonContainerRef"
                             DirectionalHint=DirectionalHint.BottomLeftEdge
                             SetInitialFocus="true"
                             OnDismiss=@DismissHandler>
                        <FocusTrapZone IsClickableOutsideFocusTrap="true">
                            <Calendar @bind-Value="calendar6Value"
                                      IsMonthPickerVisible="true"
                                      HighlightCurrentMonth="true"
                                      IsDayPickerVisible="true"
                                      ShowGoToToday="true"
                                      OnDismiss=@DismissHandler
                                      OnSelectDate="@DismissHandler"/>
                        </FocusTrapZone>
                    </Callout>
                }
            </Demo>
        </div>
        @*<div class="subSection">
                <Demo Header="Calendar with multi-day view" Key="11" MetadataPath="CalendarPage">
                    Selected date: @(((DateTime)calendar4Value).ToString("D"))
                    <br />
                    Selected range: @(calendar4Range != null && calendar4Range.Count > 0 ? calendar4Range.Min().ToString("D") + " - " + calendar4Range.Max().ToString("D") : "")
                    <br />
                    <Calendar @bind-Value="calendar4Value" DateRangeType="DateRangeType.WorkWeek" @bind-Range="calendar4Range" HighlightSelectedMonth="true" />
                </Demo>
            </div>*@
        @*<div class="subSection">
                <Demo Header="Calendar a tooltip for each day and disabling weekends " Key="12" MetadataPath="CalendarPage">
                    Selected date: @(((DateTime)calendar4Value).ToString("D"))
                    <br />
                    Selected range: @(calendar4Range != null && calendar4Range.Count > 0 ? calendar4Range.Min().ToString("D") + " - " + calendar4Range.Max().ToString("D") : "")
                    <br />
                    <Calendar @bind-Value="calendar4Value" DateRangeType="DateRangeType.WorkWeek" WorkWeekDays="workWeekDays" FirstDayOfWeek="DayOfWeek.Monday" @bind-Range="calendar4Range" HighlightSelectedMonth="true" />
                </Demo>
            </div>*@
    </div>
</div>


@code {

    bool calloutHidden = true;
    FluentUIComponentBase buttonContainerRef = new FluentUIComponentBase();

    DateTime? calendar1Value = DateTime.Now;
    System.Collections.Generic.List<DateTime>? calendar1Range;

    DateTime? calendar2Value = DateTime.Now;
    System.Collections.Generic.List<DateTime>? calendar2Range;

    DateTime? calendar3Value = DateTime.Now;
    System.Collections.Generic.List<DateTime>? calendar3Range;

    DateTime? calendar4Value = DateTime.Now;
    System.Collections.Generic.List<DateTime>? calendar4Range;

    DateTime? calendar5Value = DateTime.Now;
    DateTime? calendar6Value;

    DateTime maxDate = DateTime.Now + TimeSpan.FromDays(60);
    DateTime minDate = DateTime.Now - TimeSpan.FromDays(60);
    System.Collections.Generic.List<DateTime> restrictedDates = new System.Collections.Generic.List<DateTime>() { DateTime.Now + TimeSpan.FromDays(4), DateTime.Now + TimeSpan.FromDays(5) };

    List<DayOfWeek> workWeekDays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday };
    ExampleModel exampleModel = new ExampleModel();

    class MondayValidation : ValidationAttribute
    {
        public MondayValidation()
        {
            ErrorMessage = "Only Mondays are allowed.";
        }

        public override bool IsValid(object? value)
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

    Task ClickHandler(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        calloutHidden = !calloutHidden;
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected Task DismissHandler()
    {
        calloutHidden = true;
        return Task.CompletedTask;
    }
}
