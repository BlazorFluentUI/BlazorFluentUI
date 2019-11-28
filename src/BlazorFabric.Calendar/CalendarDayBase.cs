using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class CalendarDayBase : FabricComponentBase
    {
        [Parameter] public bool AllFocusable { get; set; }
        [Parameter] public bool AutoNavigateOnSelection { get; set; }
        [Parameter] public DateRangeType DateRangeType { get; set; }
        [Parameter] public DateTimeFormatter DateTimeFormatter { get; set; }
        [Parameter] public DayOfWeek FirstDayOfWeek { get; set; }
        [Parameter] public FirstWeekOfYear FirstWeekOfYear { get; set; }
        [Parameter] public DateTime MaxDate { get; set; }
        [Parameter] public DateTime MinDate { get; set; }
        [Parameter] public DateTime NavigatedDate { get; set; }
        [Parameter] public EventCallback<bool> OnHeaderSelect { get; set; }
        [Parameter] public EventCallback OnDismiss { get; set; }
        [Parameter] public EventCallback<NavigatedDateResult> OnNavigateDate { get; set; }
        [Parameter] public EventCallback<SelectedDateResult> OnSelectDate { get; set; }
        [Parameter] public List<DateTime> RestrictedDates { get; set; }
        [Parameter] public DateTime SelectedDate { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; }
        [Parameter] public bool ShowSixWeeksByDefault { get; set; }
        [Parameter] public bool ShowWeekNumbers { get; set; }
        [Parameter] public DateTime? Today { get; set; }
        [Parameter] public List<DayOfWeek> WorkWeekDays { get; set; }


        protected string DayPickerId = Guid.NewGuid().ToString();
        protected string MonthAndYearId = Guid.NewGuid().ToString();

        protected string PreviousMonthAriaLabel = "Previous month"; //needs localization!
        protected string NextMonthAriaLabel = "Next month"; //needs localization!
        protected string CloseButtonAriaLabel = "Close"; //needs localization!
        protected string WeekNumberFormatString = "Week number {0}"; //needs localization!

        protected bool PrevMonthInBounds;
        protected bool NextMonthInBounds;

        protected List<List<DayInfo>> Weeks;
        protected List<int> WeekNumbers;

        protected override Task OnParametersSetAsync()
        {
            GenerateWeeks();

            if (ShowWeekNumbers)
            {
                WeekNumbers = GetWeekNumbersInMonth(Weeks.Count, FirstDayOfWeek, FirstWeekOfYear, NavigatedDate);
            }
            else
                WeekNumbers = null;

            // determine if previous/next months are in bounds

            var firstDayOfMonth = new DateTime(NavigatedDate.Year, NavigatedDate.Month, 1);
            PrevMonthInBounds = DateTime.Compare(MinDate, firstDayOfMonth) < 0;
            NextMonthInBounds = DateTime.Compare(firstDayOfMonth.AddMonths(1).AddDays(-1), MaxDate) < 0;

            return base.OnParametersSetAsync();
        }


        protected Task OnHeaderSelectInternal(MouseEventArgs mouseEventArgs)
        {
            OnHeaderSelect.InvokeAsync(true);
            return Task.CompletedTask;
        }

        protected Task OnSelectPrevMonth()
        {
            return OnNavigateDate.InvokeAsync(new NavigatedDateResult() { Date = NavigatedDate.AddMonths(-1), FocusOnNavigatedDay = false });
        }

        protected Task OnSelectNextMonth()
        {
            return OnNavigateDate.InvokeAsync(new NavigatedDateResult() { Date = NavigatedDate.AddMonths(1), FocusOnNavigatedDay = false });
        }


        protected Task OnClose(MouseEventArgs mouseEventArgs)
        {
            return OnDismiss.InvokeAsync(null);
        }


        protected void OnTableMouseLeave(System.EventArgs eventArgs)
        {

        }
        protected void OnTableMouseUp(MouseEventArgs mouseEventArgs)
        {   
            
        }


        protected Task OnDayMouseOver(MouseEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }
        protected Task OnDayMouseLeave(EventArgs eventArgs)
        {
            return Task.CompletedTask;
        }
        protected Task OnDayMouseDown(MouseEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }
        protected Task OnDayMouseUp(MouseEventArgs eventArgs)
        {
            return Task.CompletedTask;
        }

        private void OnSelectDateInternal(DateTime selectedDate)
        {
            // stop propagation - needed?

            var dateRange = GetDateRangeArray(selectedDate, DateRangeType, FirstDayOfWeek, WorkWeekDays);
            if (DateRangeType != DateRangeType.Day)
                dateRange = GetBoundedDateRange(dateRange, MinDate, MaxDate);
            dateRange = dateRange.Where(d => !GetIsRestrictedDate(d)).ToList();

            OnSelectDate.InvokeAsync(new SelectedDateResult() { Date = selectedDate, SelectedDateRange = dateRange });

            // Navigate to next or previous month if needed
            if (AutoNavigateOnSelection && selectedDate.Month != NavigatedDate.Month)
            {
                var compareResult = DateTime.Compare(selectedDate.Date, NavigatedDate.Date);
                if (compareResult < 0)
                    OnSelectPrevMonth();
                else if (compareResult > 0)
                    OnSelectNextMonth();

            }
        }


        private void GenerateWeeks()
        {
            var date = new DateTime(NavigatedDate.Year, NavigatedDate.Month, 1);
            var todaysDate = DateTime.Now;
            Weeks = new List<List<DayInfo>>();

            // cycle backwards to get first day of week
            while (date.DayOfWeek != FirstDayOfWeek)
                date = date - TimeSpan.FromDays(1);

            // a flag to indicate whether all days of the week are in the month
            var isAllDaysOfWeekOutOfMonth = false;

            // in work week view we want to select the whole week
            var selecteDateRangeType = DateRangeType == DateRangeType.WorkWeek ? DateRangeType.Week : DateRangeType;
            var selectedDates = GetDateRangeArray(SelectedDate, selecteDateRangeType, FirstDayOfWeek, WorkWeekDays);
            if (DateRangeType != DateRangeType.Day)
            {
                selectedDates = GetBoundedDateRange(selectedDates, MinDate, MaxDate);
            }

            var shouldGetWeeks = true;
            for (var weekIndex = 0; shouldGetWeeks; weekIndex++)
            {
                List<DayInfo> week = new List<DayInfo>();
                isAllDaysOfWeekOutOfMonth = true;

                for (var dayIndex = 0; dayIndex < 7; dayIndex++)
                {
                    var originalDate = new DateTime(date.Year, date.Month, date.Day);
                    var dayInfo = new DayInfo()
                    {
                        Key = date.ToString(),
                        Date = date.Date.ToString("D"),
                        OriginalDate = originalDate,
                        IsInMonth = date.Month == NavigatedDate.Month,
                        IsToday = DateTime.Compare(DateTime.Now.Date, originalDate) == 0,
                        IsSelected = IsInDateRangeArray(date, selectedDates),
                        OnSelected = () => OnSelectDateInternal(originalDate),
                        IsInBounds =
                            (DateTime.Compare(MinDate, date) < 1 ) &&
                            (DateTime.Compare(date, MaxDate) < 1 ) &&
                            !GetIsRestrictedDate(date)
                    };

                    week.Add(dayInfo);
                    if (dayInfo.IsInMonth)
                        isAllDaysOfWeekOutOfMonth = false;

                    date = date.AddDays(1);
                }

                // We append the condition of the loop depending upon the ShowSixWeeksByDefault parameter.
                shouldGetWeeks = ShowSixWeeksByDefault ? !isAllDaysOfWeekOutOfMonth || weekIndex <= 5 : !isAllDaysOfWeekOutOfMonth;
                if (shouldGetWeeks)
                    Weeks.Add(week);
            }
        }

        private bool GetIsRestrictedDate(DateTime date)
        {
            if (RestrictedDates == null) 
                return false;
            
            if (RestrictedDates.Select(x=>x.Date).Contains(date))
            {
                return true;
            }
            return false;
        }

        private bool IsInDateRangeArray(DateTime date, List<DateTime> dateRange)
        {
            foreach (var dateInRange in dateRange) {
                if (DateTime.Compare(date, dateInRange) == 0) {
                    return true;
                }
            }
            return false;
        }

        private List<DateTime> GetBoundedDateRange(List<DateTime> dateRange, DateTime? minDate = null, DateTime? maxDate = null)
        {
            var boundedDateRange = dateRange;
            if (minDate.HasValue) {
                boundedDateRange = boundedDateRange.Where(date => DateTime.Compare(date.Date, minDate.Value) >= 0).ToList();
            }
            if (maxDate.HasValue) {
                boundedDateRange = boundedDateRange.Where(date => DateTime.Compare(date.Date, maxDate.Value) <= 0).ToList();
            }
            return boundedDateRange;
        }

        private List<DateTime> GetDateRangeArray(DateTime date, DateRangeType dateRangeType, DayOfWeek firstDayOfWeek, List<DayOfWeek> workWeekDays, int daysToSelectInDayView = 1) 
        {
            var datesArray = new List<DateTime>();
            DateTime startDate;
            DateTime endDate;

            if (workWeekDays == null) {
                workWeekDays = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            }

            daysToSelectInDayView = Math.Max(daysToSelectInDayView, 1);

            switch (dateRangeType) {
                case DateRangeType.Day:
                    startDate = date.Date;
                    endDate = startDate.AddDays(daysToSelectInDayView);
                    break;

                case DateRangeType.Week:
                case DateRangeType.WorkWeek:
                    startDate = GetStartDateOfWeek(date.Date, firstDayOfWeek);
                    endDate = startDate.AddDays(7);
                    break;

                case DateRangeType.Month:
                    startDate = new DateTime(date.Year, date.Month, 1);
                    endDate = startDate.AddMonths(1);
                    break;

                default:
                    throw new Exception("This should never be reached.");
            }

            // Populate the dates array with the dates in range
            var nextDate = startDate;

            do
            {
                if (dateRangeType != DateRangeType.WorkWeek)
                {
                    // push all days not in work week view
                    datesArray.Add(nextDate);
                }
                else if (workWeekDays.IndexOf(nextDate.DayOfWeek) != -1)
                {
                    datesArray.Add(nextDate);
                }
                nextDate = nextDate.AddDays(1);
            } while (nextDate != endDate);

            return datesArray;
        }

        private DateTime GetStartDateOfWeek(DateTime date, DayOfWeek firstDayOfWeek)
        {
            var daysOffset = firstDayOfWeek - date.DayOfWeek;
            if (daysOffset > 0) {
                // If first day of week is > date, go 1 week back, to ensure resulting date is in the past.
                daysOffset -= 7;
            }
            return date.AddDays(daysOffset);
        }


private List<int> GetWeekNumbersInMonth(int weeksInMonth, DayOfWeek firstDayOfWeek, FirstWeekOfYear firstWeekOfYear, DateTime navigatedDate)
        {
            var selectedYear = navigatedDate.Year;
            var selectedMonth = navigatedDate.Month;
            int dayOfMonth = 1;
            var fistDayOfMonth = new DateTime(selectedYear, selectedMonth, dayOfMonth);
            var endOfFirstWeek = dayOfMonth + (firstDayOfWeek + 7 - 1) - AdjustWeekDay(firstDayOfWeek, fistDayOfMonth.DayOfWeek);
            var endOfWeekRange = new DateTime(selectedYear, selectedMonth, (int)endOfFirstWeek);
            dayOfMonth = endOfWeekRange.Day;
            var weeksArray = new List<int>();
            for (var i = 0; i < weeksInMonth; i++)
            {
                // Get week number for end of week
                weeksArray.Add(GetWeekNumber(endOfWeekRange, firstDayOfWeek, firstWeekOfYear));
                dayOfMonth += 7;
                endOfWeekRange = new DateTime(selectedYear, selectedMonth, dayOfMonth);
            }
            return weeksArray;
        }

        private int AdjustWeekDay(DayOfWeek firstDayOfWeek, DayOfWeek dateWeekDay)
        {
            return firstDayOfWeek != DayOfWeek.Sunday && dateWeekDay < firstDayOfWeek ? (int)dateWeekDay + 7 : (int)dateWeekDay;
        }

        private int GetWeekNumber(DateTime date, DayOfWeek firstDayOfWeek, FirstWeekOfYear firstWeekOfYear) {
            // First four-day week of the year - minumum days count
            int fourDayWeek = 4;

            switch (firstWeekOfYear)
            {
                case FirstWeekOfYear.FirstFullWeek:
                    return GetWeekOfYearFullDays(date, firstDayOfWeek, 7);

                case FirstWeekOfYear.FirstFourDayWeek:
                    return GetWeekOfYearFullDays(date, firstDayOfWeek, fourDayWeek);

                default:
                    return GetFirstDayWeekOfYear(date, firstDayOfWeek);
            }
        }

        private int GetFirstDayWeekOfYear(DateTime date, DayOfWeek firstDayOfWeek) {
            var num = date.DayOfYear - 1;
            var num2 = date.DayOfWeek - (num % 7);
            var num3 = ((int)num2 - (int)firstDayOfWeek + 2 * 7) % 7;

            return (num + num3) / 7 + 1;
        }

        private int GetWeekOfYearFullDays(DateTime date, DayOfWeek firstDayOfWeek, int numberOfFullDays)
        {
            var dayOfYear = date.DayOfYear - 1;
            var num = date.DayOfWeek - (dayOfYear % 7);

            var lastDayOfPrevYear = new DateTime(date.Year, 12, 31); 
            var daysInYear = lastDayOfPrevYear.DayOfYear - 1;

            var num2 = (firstDayOfWeek - num + 2 * 7) % 7;
            if (num2 != 0 && num2 >= numberOfFullDays) 
            {
                num2 -= 7;
            }

            var num3 = dayOfYear - num2;
            if (num3 < 0) 
            {
                num -= daysInYear % 7;
                num2 = (firstDayOfWeek - num + 2 * 7) % 7;
                if (num2 != 0 && num2 + 1 >= numberOfFullDays) 
                {
                    num2 -= 7;
                }

                num3 = daysInYear - num2;
            }

            return num3 / 7 + 1;
        }

}
}
