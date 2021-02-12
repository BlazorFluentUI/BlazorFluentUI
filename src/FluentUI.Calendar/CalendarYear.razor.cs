using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class CalendarYear : FluentUIComponentBase
    {
        [Parameter] public DateTimeFormatter DateTimeFormatter { get; set; }
        [Parameter] public int MaxYear { get; set; }
        [Parameter] public int MinYear { get; set; }
        [Parameter] public int NavigatedYear { get; set; } = 0;
        [Parameter] public EventCallback<bool> OnHeaderSelect { get; set; }
        [Parameter] public EventCallback<NavigatedDateResult> OnNavigateDate { get; set; }
        [Parameter] public EventCallback<int> OnSelectYear { get; set; }
        [Parameter] public int SelectedYear { get; set; } = 0;

        protected string RangeAriaLabel = ""; 
        protected string PrevYearRangeAriaLabel = "Previous year range"; //localize
        protected string NextYearRangeAriaLabel = "Next year range"; //localize

        protected int FromYear;
        //protected int ToYear;

        protected override Task OnParametersSetAsync()
        {
            var rangeYear = SelectedYear != 0 ? SelectedYear : (NavigatedYear != 0 ? NavigatedYear : (DateTime.Now.Year));
            FromYear = rangeYear / 10 * 10;

            RangeAriaLabel = $"{DateTimeFormatter.FormatYear(new DateTime(FromYear,1,1))} - {DateTimeFormatter.FormatYear(new DateTime(FromYear + 12 -1, 1, 1))}";

            return base.OnParametersSetAsync();
        }

        protected Task OnSelectPrevDecade()
        {
            FromYear -= 12;
            return Task.CompletedTask;
        }

        protected Task OnSelectNextDecade()
        {
            FromYear += 12;
            return Task.CompletedTask;
        }

        private async Task OnHeaderKeyDownInternal(KeyboardEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.Key == "Enter" || keyboardEventArgs.Key == " ")
            {
                await OnHeaderSelect.InvokeAsync(true);
            }
        }
    }
}
