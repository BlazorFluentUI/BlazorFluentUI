using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class Calendar : FluentUIComponentBase
    {
        [Parameter] public bool AllFocusable { get; set; } = false;
        [Parameter] public bool AutoNavigateOnSelection { get; set; } = false;
        [Parameter] public DateRangeType DateRangeType { get; set; } = DateRangeType.Day;
        [Parameter] public DateTimeFormatter DateTimeFormatter { get; set; } = new DateTimeFormatter();
        [Parameter] public DayOfWeek FirstDayOfWeek { get; set; } = DayOfWeek.Sunday;
        [Parameter] public FirstWeekOfYear FirstWeekOfYear { get; set; } = FirstWeekOfYear.FirstDay;
        [Parameter] public bool HighlightCurrentMonth { get; set; } = false;
        [Parameter] public bool HighlightSelectedMonth { get; set; } = true;
        [Parameter] public bool IsDayPickerVisible { get; set; } = true;
        [Parameter] public bool IsMonthPickerVisible { get; set; } = true;
        [Parameter] public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        [Parameter] public DateTime MinDate { get; set; } = DateTime.MinValue;
        [Parameter] public EventCallback OnDismiss { get; set; }
        [Parameter] public EventCallback<SelectedDateResult> OnSelectDate { get; set; }
        [Parameter] public List<DateTime> Range { get; set; }  // readonly value!  meant to supplement binding 
        [Parameter] public EventCallback<List<DateTime>> RangeChanged { get; set; }  // complement to Range readonly value!  meant to supplement binding 
        [Parameter] public List<DateTime> RestrictedDates { get; set; }
        [Parameter] public bool SelectDateOnClick { get; set; } = false;
        [Parameter] public bool ShowCloseButton { get; set; } = false;
        [Parameter] public bool ShowGoToToday { get; set; } = true;
        [Parameter] public bool ShowMonthPickerAsOverlay { get; set; } = false;
        [Parameter] public bool ShowSixWeeksByDefault { get; set; } = false;
        [Parameter] public bool ShowWeekNumbers { get; set; } = false;
        [Parameter] public DateTime Today { get; set; } = DateTimeOffset.Now.Date;
        [Parameter] public DateTime? Value { get; set; } = DateTime.MinValue;
        [Parameter] public EventCallback<DateTime?> ValueChanged { get; set; }
        [Parameter] public List<DayOfWeek> WorkWeekDays { get; set; } = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        [Parameter] public bool YearPickerHidden { get; set; } = false;
        [Parameter] public bool IsTimePickerVisible { get; set; }

        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;

        private FieldIdentifier FieldIdentifier;

        [Parameter]
        public Expression<Func<DateTime?>>? ValueExpression { get; set; }


        protected DateTime CurrentDate;

        protected DateTime? SelectedDate;
        protected DateTime NavigatedDayDate;
        protected DateTime NavigatedMonthDate;

        protected bool IsDayPickerVisibleInternal;
        protected bool IsMonthPickerVisibleInternal;


        protected bool GoTodayEnabled;
        protected string GoToTodayString = "Go to today";  // needs to be localized!

        private bool isLoaded = false;
        private bool focusOnUpdate = false;

        
        protected override Task OnParametersSetAsync()
        {
            if (CascadedEditContext != null && ValueExpression != null)
            {
                //if (ValueExpression == null)
                //{
                //    throw new InvalidOperationException($"{GetType()} requires a value for the 'ValueExpression' " +
                //        $"parameter. Normally this is provided automatically when using 'bind-Value'.");
                //}
                FieldIdentifier = FieldIdentifier.Create<DateTime?>(ValueExpression);

                CascadedEditContext?.NotifyFieldChanged(FieldIdentifier);

                CascadedEditContext.OnValidationStateChanged += CascadedEditContext_OnValidationStateChanged;
            }

            if (!isLoaded)
            {
                if (Value != null)
                {
                    CurrentDate = (DateTime)Value;
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

                GoTodayEnabled = ShowGoToToday;
                if (GoTodayEnabled)
                {
                    GoTodayEnabled = NavigatedDayDate.Year != Today.Year ||
                        NavigatedDayDate.Month != Today.Month ||
                        NavigatedMonthDate.Year != Today.Year ||
                        NavigatedMonthDate.Month != Today.Month;
                }

                isLoaded = true;
            }

            return base.OnParametersSetAsync();
        }

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            InvokeAsync(() => StateHasChanged());  //invokeasync required for serverside
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            bool valuesDifferent = false;

            DateTime? nextValue;
            parameters.TryGetValue("Value", out nextValue);
            if (nextValue != null && Value == null)
            {
                valuesDifferent = true;
            }
            else if (nextValue != null && Value != null && DateTime.Compare(((DateTime)nextValue).Date, ((DateTime)Value).Date) != 0)
            {
                valuesDifferent = true;
            }

            if (valuesDifferent)
            {
                NavigatedDayDate = (DateTime)nextValue;
                NavigatedMonthDate = (DateTime)nextValue;
            }
            SelectedDate = nextValue != DateTime.MinValue ? nextValue : Today;

            return base.SetParametersAsync(parameters);
        }

        protected async void OnGotoToday(MouseEventArgs args) {
            
            if (SelectDateOnClick) {
                // When using Defaultprops, TypeScript doesn't know that React is going to inject defaults
                // so we use exclamation mark as a hint to the type checker (see link below)
                // https://decembersoft.com/posts/error-ts2532-optional-react-component-props-in-typescript/
                var dates = DateUtilities.GetDateRangeArray(Today, DateRangeType, FirstDayOfWeek, WorkWeekDays);

                await OnSelectDateInternal(new SelectedDateResult() { Date = Today, SelectedDateRange = dates });
            }

            NavigateDayPickerDay(Today);
            GoTodayEnabled = false;
            focusOnUpdate = true;
        }

    protected Task OnDatePickerPopupKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            return Task.CompletedTask;
        }

        protected async Task OnSelectDateInternal(SelectedDateResult result)
        {
            SelectedDate = result.Date;
            CascadedEditContext?.NotifyFieldChanged(FieldIdentifier);
            await OnSelectDate.InvokeAsync(result);
            await ValueChanged.InvokeAsync(result.Date);
            await RangeChanged.InvokeAsync(result.SelectedDateRange);
        }

        protected Task OnNavigateDayDate(NavigatedDateResult result)
        {
            NavigateDayPickerDay(result.Date);
            focusOnUpdate = result.FocusOnNavigatedDay;

            GoTodayEnabled = NavigatedDayDate.Year != Today.Year ||
               NavigatedDayDate.Month != Today.Month ||
               NavigatedMonthDate.Year != Today.Year ||
               NavigatedMonthDate.Month != Today.Month;


            return Task.CompletedTask;
        }

       
        protected async Task OnNavigateMonthDate(NavigatedDateResult result)
        {
            if (!result.FocusOnNavigatedDay)
            {
                NavigateMonthPickerDay(result.Date);
                focusOnUpdate = result.FocusOnNavigatedDay;
            }
            var monthPickerOnly = !ShowMonthPickerAsOverlay && !IsDayPickerVisible;

            if (monthPickerOnly)
                await OnSelectDateInternal(new SelectedDateResult() { Date = result.Date });

            NavigateDayPickerDay(result.Date);


            GoTodayEnabled = NavigatedDayDate.Year != Today.Year ||
                NavigatedDayDate.Month != Today.Month ||
                NavigatedMonthDate.Year != Today.Year ||
                NavigatedMonthDate.Month != Today.Month;

            //StateHasChanged();

            //return Task.CompletedTask;
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
