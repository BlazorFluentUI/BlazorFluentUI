using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class SelectedDateResult
    {
        public DateTime Date { get; set; }
        public List<DateTime> SelectedDateRange { get; set; }
    }
}
