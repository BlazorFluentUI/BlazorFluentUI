using System;

namespace FluentUI
{
    public class DayInfo
    {
        public string Key { get; set; }
        public string Date { get; set; }
        public DateTime OriginalDate { get; set; }
        public bool IsInMonth { get; set; }
        public bool IsToday { get; set; }
        public bool IsSelected { get; set; }
        public bool IsInBounds { get; set; }
        public Action OnSelected { get; set; }
        public int WeekIndex { get; set; }
    }
}
