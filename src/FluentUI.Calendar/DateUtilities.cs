using System;
using System.Collections.Generic;

namespace FluentUI
{
    public static class DateUtilities
    {
        public static List<DateTime> GetDateRangeArray(DateTime date, DateRangeType dateRangeType, DayOfWeek firstDayOfWeek, List<DayOfWeek> workWeekDays, int daysToSelectInDayView = 1)
        {
            var datesArray = new List<DateTime>();
            DateTime startDate;
            DateTime endDate;

            if (workWeekDays == null)
            {
                workWeekDays = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            }

            daysToSelectInDayView = Math.Max(daysToSelectInDayView, 1);

            switch (dateRangeType)
            {
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

        public static DateTime GetStartDateOfWeek(DateTime date, DayOfWeek firstDayOfWeek)
        {
            var daysOffset = firstDayOfWeek - date.DayOfWeek;
            if (daysOffset > 0)
            {
                // If first day of week is > date, go 1 week back, to ensure resulting date is in the past.
                daysOffset -= 7;
            }
            return date.AddDays(daysOffset);
        }


        public static List<int> GetWeekNumbersInMonth(int weeksInMonth, DayOfWeek firstDayOfWeek, FirstWeekOfYear firstWeekOfYear, DateTime navigatedDate)
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

        public static int AdjustWeekDay(DayOfWeek firstDayOfWeek, DayOfWeek dateWeekDay)
        {
            return firstDayOfWeek != DayOfWeek.Sunday && dateWeekDay < firstDayOfWeek ? (int)dateWeekDay + 7 : (int)dateWeekDay;
        }

        public static int GetWeekNumber(DateTime date, DayOfWeek firstDayOfWeek, FirstWeekOfYear firstWeekOfYear)
        {
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

        public static int GetFirstDayWeekOfYear(DateTime date, DayOfWeek firstDayOfWeek)
        {
            var num = date.DayOfYear - 1;
            var num2 = date.DayOfWeek - (num % 7);
            var num3 = ((int)num2 - (int)firstDayOfWeek + 2 * 7) % 7;

            return (num + num3) / 7 + 1;
        }

        public static int GetWeekOfYearFullDays(DateTime date, DayOfWeek firstDayOfWeek, int numberOfFullDays)
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
