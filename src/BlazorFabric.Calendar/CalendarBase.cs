using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class CalendarBase : FabricComponentBase
    {
        [Parameter] public bool AllFocusable { get; set; }
        [Parameter] public bool AutoNavigateOnSelection { get; set; } = true;
        [Parameter] public DateRangeType DateRangeType { get; set; }
        [Parameter] public DateTimeFormatter DateTimeFormatter { get; set; } = new DateTimeFormatter();
        [Parameter] public DayOfWeek FirstDayOfWeek { get; set; }
        [Parameter] public FirstWeekOfYear FirstWeekOfYear { get; set; }
        [Parameter] public bool HighlightCurrentMonth { get; set; }
        [Parameter] public bool HighlightSelectedMonth { get; set; }
        [Parameter] public bool IsDayPickerVisible { get; set; } = true;
        [Parameter] public bool IsMonthPickerVisible { get; set; } = true;
        [Parameter] public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        [Parameter] public DateTime MinDate { get; set; } = DateTime.MinValue;
        [Parameter] public EventCallback OnDismiss { get; set; }
        [Parameter] public EventCallback<SelectedDateResult> OnSelectDate { get; set; }
        [Parameter] public List<DateTime> RestrictedDates { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; }
        [Parameter] public bool ShowMonthPickerAsOverlay { get; set; }
        [Parameter] public bool ShowSixWeeksByDefault { get; set; }
        [Parameter] public bool ShowWeekNumbers { get; set; }
        [Parameter] public DateTime Today { get; set; } = DateTime.Now.Date;
        [Parameter] public DateTime? Value { get; set; }
        [Parameter] public List<DayOfWeek> WorkWeekDays { get; set; } = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        [Parameter] public bool YearPickerHidden { get; set; }

        protected DateTime CurrentDate;

        protected DateTime SelectedDate;
        protected DateTime NavigatedDayDate;
        protected DateTime NavigatedMonthDate;

        protected bool IsDayPickerVisibleInternal;
        protected bool IsMonthPickerVisibleInternal;

        private bool isLoaded = false;
        private bool focusOnUpdate = false;

        protected override Task OnParametersSetAsync()
        {
            if (!isLoaded)
            {
                if (Value.HasValue)
                {
                    CurrentDate = Value.Value;
                }
                else
                {
                    CurrentDate = Today;
                }

                SelectedDate = CurrentDate;
                NavigatedDayDate = CurrentDate;
                NavigatedMonthDate = CurrentDate;

                IsMonthPickerVisibleInternal = ShowMonthPickerAsOverlay ? false : IsMonthPickerVisible;
                IsDayPickerVisibleInternal = ShowMonthPickerAsOverlay ? true : IsDayPickerVisible;

                isLoaded = true;
            }

            return base.OnParametersSetAsync();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            bool valuesDifferent = false;

            DateTime? nextValue = null;
            parameters.TryGetValue("Value", out nextValue);
            if (nextValue.HasValue && !Value.HasValue)
            {
                valuesDifferent = true;
            }
            else if (nextValue.HasValue && Value.HasValue && DateTime.Compare(nextValue.Value.Date, Value.Value.Date) != 0)
            {
                valuesDifferent = true;
            }

            if (valuesDifferent)
            {
                NavigatedDayDate = nextValue.Value;
                NavigatedMonthDate = nextValue.Value;
            }
            SelectedDate = nextValue.HasValue ? nextValue.Value : Today;

            return base.SetParametersAsync(parameters);
        }

        protected Task OnDatePickerPopupKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            return Task.CompletedTask;
        }

        protected Task OnSelectDateInternal(SelectedDateResult result)
        {
            SelectedDate = result.Date;
            return OnSelectDate.InvokeAsync(result);
        }

        protected Task OnNavigateDayDate(NavigatedDateResult result)
        {
            NavigateDayPickerDay(result.Date);
            focusOnUpdate = result.FocusOnNavigatedDay;
            return Task.CompletedTask;
        }

        protected Task OnNavigateMonthDate(NavigatedDateResult result)
        {
            if (!result.FocusOnNavigatedDay)
            {
                NavigateMonthPickerDay(result.Date);
                focusOnUpdate = result.FocusOnNavigatedDay;
            }
            var monthPickerOnly = !ShowMonthPickerAsOverlay && !IsDayPickerVisible;

            if (monthPickerOnly)
                OnSelectDateInternal(new SelectedDateResult() { Date = result.Date });

            NavigateDayPickerDay(result.Date);

            return Task.CompletedTask;
        }

        protected Task OnHeaderSelect(bool focus)
        {
            IsDayPickerVisibleInternal = !IsDayPickerVisibleInternal;
            IsMonthPickerVisibleInternal = !IsMonthPickerVisibleInternal;

            if (focus) {
                focusOnUpdate = true;
            }
            return Task.CompletedTask;
        }

        private void NavigateDayPickerDay(DateTime date)
        {
            NavigatedDayDate = date;
            NavigatedMonthDate = date;
        }

        private void NavigateMonthPickerDay(DateTime date)
        {
            NavigatedMonthDate = date;
        }
    }
}
