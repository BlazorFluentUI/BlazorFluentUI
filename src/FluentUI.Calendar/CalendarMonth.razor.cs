using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class CalendarMonth : FluentUIComponentBase
    {
        [Parameter] public DateTimeFormatter DateTimeFormatter { get; set; }
        [Parameter] public bool HighlightCurrentMonth { get; set; }
        [Parameter] public bool HighlightSelectedMonth { get; set; }
        [Parameter] public DateTime MaxDate { get; set; }
        [Parameter] public DateTime MinDate { get; set; }
        [Parameter] public DateTime NavigatedDate { get; set; }
        [Parameter] public EventCallback<bool> OnHeaderSelect { get; set; }
        [Parameter] public EventCallback<NavigatedDateResult> OnNavigateDate { get; set; }
        [Parameter] public DateTime SelectedDate { get; set; }
        [Parameter] public DateTime Today { get; set; }
        [Parameter] public bool YearPickerHidden { get; set; }

        protected string MonthPickerId = Guid.NewGuid().ToString();
        protected bool IsYearPickerVisible;
        protected bool IsPrevYearInBounds;
        protected bool IsNextYearInBounds;
        
        protected string PreviousYearAriaLabel = "Previous year"; //needs localization!
        protected string NextYearAriaLabel = "Next year"; //needs localization!

        protected List<int> RowIndexes;

        protected string[] ShortMonthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
        protected string[] MonthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;

        protected List<Action> SelectMonthCallbacks = new List<Action>();

        protected bool focusOnUpdate;

        protected override Task OnInitializedAsync()
        {
            for (var i=0; i< ShortMonthNames.Length; i++)
            {
                var index = i;
                SelectMonthCallbacks.Add(() => OnSelectMonth(index + 1));
            }

            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {            
            var firstDayOfYear = new DateTime(NavigatedDate.Year, 1, 1);
            IsPrevYearInBounds = DateTime.Compare(MinDate, firstDayOfYear) < 0;
            IsNextYearInBounds = DateTime.Compare(firstDayOfYear.AddYears(1).AddDays(-1), MaxDate) < 0;

            RowIndexes = new List<int>();
            for (var i=0; i < 12 / 4; i++) //12 months, 4 per row
            {
                RowIndexes.Add(i);
            }

            return base.OnParametersSetAsync();
        }

        protected Task OnHeaderSelectInternal() 
        {
            if (!YearPickerHidden) 
            {
                focusOnUpdate = true;
                IsYearPickerVisible = true;
            } 
            else if (OnHeaderSelect.HasDelegate) 
            {
                return OnHeaderSelect.InvokeAsync(true);
            }
            return Task.CompletedTask;
        }

        protected Task OnHeaderKeyDownInternal(KeyboardEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.Key == "Enter" || keyboardEventArgs.Key == " ")
            {
                OnHeaderSelectInternal();
            }
            return Task.CompletedTask;
        }

        protected Task OnSelectYear(int selectedYear)
        {
            focusOnUpdate = true;
            var navYear = NavigatedDate.Year;
            if (navYear != selectedYear)
            {
                var newNavDate = new DateTime(NavigatedDate.Year, NavigatedDate.Month, NavigatedDate.Day);
                newNavDate = newNavDate.AddYears(selectedYear - newNavDate.Year);
                if (newNavDate > MaxDate)
                {
                    newNavDate = newNavDate.AddMonths(MaxDate.Month - newNavDate.Month);
                }
                else if (newNavDate < MinDate)
                {
                    newNavDate = newNavDate.AddMonths(MinDate.Month - newNavDate.Month);
                }
                OnNavigateDate.InvokeAsync(new NavigatedDateResult { Date = newNavDate, FocusOnNavigatedDay = true });
            }
            IsYearPickerVisible = false;
            return Task.CompletedTask;
        }

        protected Task OnSelectPrevYear()
        {
            return OnNavigateDate.InvokeAsync(new NavigatedDateResult { Date = NavigatedDate.AddYears(-1), FocusOnNavigatedDay = false });
        }

        protected Task OnSelectNextYear()
        {
            return OnNavigateDate.InvokeAsync(new NavigatedDateResult { Date = NavigatedDate.AddYears(+1), FocusOnNavigatedDay = false });
        }

        protected string GetMonthClasses(int monthIndex, bool isInBounds)
        {
            var isCurrentMonth = (monthIndex + 1 == Today.Month && NavigatedDate.Year == Today.Year);
            var isNavigatedMonth = NavigatedDate.Month == (monthIndex + 1);
            var isSelectedMonth = SelectedDate.Month == (monthIndex + 1);
            var isSelectedYear = SelectedDate.Year == NavigatedDate.Year;

            string classNames = "";
            classNames += "ms-Calendar-monthOption";
            if (HighlightCurrentMonth && isCurrentMonth)
                classNames += " ms-Calendar-day--today ms-Calendar-monthIsCurrentMonth";
            if ((HighlightCurrentMonth || HighlightSelectedMonth) && isSelectedMonth && isSelectedYear)
                classNames += " ms-Calendar-day--highlighted ms-Calendar-monthIsHighlighted";
            if (!isInBounds)
                classNames += " ms-Calendar-monthOption--disabled ms-Calendar-monthOptionIsDisabled";
            return classNames;
        }

        private void OnSelectMonth(int newMonth) {
            // If header is clickable the calendars are overlayed, switch back to day picker when month is clicked
            if (OnHeaderSelect.HasDelegate) {

                OnHeaderSelect.InvokeAsync(true);
            }
            OnNavigateDate.InvokeAsync(new NavigatedDateResult() { Date = NavigatedDate.AddMonths(-1 * (NavigatedDate.Month - newMonth)), FocusOnNavigatedDay = true });
        }


    }
}
