using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class CalendarMonthBase : FabricComponentBase
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

        private bool focusOnUpdate;

        protected override Task OnInitializedAsync()
        {
            for (var i=0; i< ShortMonthNames.Length; i++)
            {
                SelectMonthCallbacks.Add(() => OnSelectMonth(i + 1));
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

        protected Task OnSelectPrevYear()
        {
            return Task.CompletedTask;
        }

        protected Task OnSelectNextYear()
        {
            return Task.CompletedTask;
        }

        private void OnSelectMonth(int newMonth) {
            // If header is clickable the calendars are overlayed, switch back to day picker when month is clicked
            if (OnHeaderSelect.HasDelegate) {

                OnHeaderSelect.InvokeAsync(true);
            }
            OnNavigateDate.InvokeAsync(new NavigatedDateResult() { Date = NavigatedDate.AddMonths(NavigatedDate.Month - newMonth), FocusOnNavigatedDay = true });
        }

    }
}
