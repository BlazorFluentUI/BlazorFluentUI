using System;

namespace FluentUI
{
    public class DateTimeFormatter
    {
        public Func<DateTime, string> FormatMonthDayYear { get; set; }
        public Func<DateTime, string> FormatMonthYear { get; set; }
        public Func<DateTime, string> FormatDay { get; set; }
        public Func<DateTime, string> FormatYear { get; set; }

        public DateTimeFormatter()
        {
            FormatMonthDayYear = GetDefaultMonthDayYear;
            FormatMonthYear = (dateTime) => dateTime.ToString("Y");
            FormatDay = (dateTime) => dateTime.Day.ToString();
            FormatYear = (dateTime) => dateTime.Year.ToString();
        }


        private string GetDefaultMonthDayYear(DateTime dateTimeOffset)
        {
            return dateTimeOffset.ToString("D").Replace(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetDayName(DateTime.Now.DayOfWeek), "").TrimStart(", ".ToCharArray()).TrimEnd(", ".ToCharArray());
        }
    }
}
