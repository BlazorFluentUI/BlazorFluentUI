using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class CalenderTime : FluentUIComponentBase
    {
        [Parameter] public DateTimeFormatter DateTimeFormatter { get; set; }
        [Parameter] public DateTime SelectedDate { get; set; }
        [Parameter] public DateTime NavigatedDate { get; set; }
        [Parameter] public EventCallback<NavigatedDateResult> OnNavigateDate { get; set; }

        protected List<Action> SelectMonthCallbacks = new List<Action>();

        protected override Task OnInitializedAsync()
        {
            for (var i = 0; i < 24; i++)
            {
                var index = i;
                SelectMonthCallbacks.Add(() => OnSelectMonth(index));
            }

            return base.OnInitializedAsync();
        }

        private void OnSelectMonth(int newTime)
        {
           // SelectedDate = SelectedDate.Date.AddHours(newTime);
            OnNavigateDate.InvokeAsync(new NavigatedDateResult() { Date = SelectedDate.Date.AddHours(newTime), FocusOnNavigatedDay = true });
        }
    }


}
