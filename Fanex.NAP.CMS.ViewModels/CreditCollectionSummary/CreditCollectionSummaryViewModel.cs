namespace Fanex.NAP.CMS.ViewModels.CreditCollectionSummary
{
    using System;
    using Core.Helpers;

    public static class CreditCollectionSummaryViewModel
    {
        public static DateTime CurrentDate => DateTime.Today;

        public static string InitFromDateMonthlyMode
            => CurrentDate.FirstDayOfMonth().ToString("MMM dd, yyyy");

        public static string InitToDateMonthlyMode
            => CurrentDate.LastDayOfMonth().ToString("MMM dd, yyyy");

        public static string InitFromDateWeeklyMode
        {
            get
            {
                var isWeekend = IsWeekend();

                var result = CurrentDate.StartDayOfWeek(startDateOfWeek: DayOfWeek.Saturday);

                return (isWeekend ? result.AddDays(-7) : result.AddDays(-14)).ToString("MMM dd, yyyy");
            }
        }

        public static string InitToDateWeeklyMode
        {
            get
            {
                var isWeekend = IsWeekend();

                var result = CurrentDate.StartDayOfWeek(startDateOfWeek: DayOfWeek.Saturday);

                return (isWeekend ? result.AddDays(6) : result.AddDays(-1)).ToString("MMM dd, yyyy");
            }
        }

        public static string MinFromDateWeekly
        {
            get
            {
                var result = CurrentDate.StartDayOfWeek(startDateOfWeek: DayOfWeek.Saturday);

                return result.AddDays(-53 * 7).ToString("MMM dd, yyyy");
            }
        }

        public static string MaxFromDateWeekly
        {
            get
            {
                var isWeekend = IsWeekend();

                var result = CurrentDate.StartDayOfWeek(startDateOfWeek: DayOfWeek.Saturday);

                return (isWeekend ? result.AddDays(-7) : result.AddDays(-14)).ToString("MMM dd, yyyy");
            }
        }

        public static string MinToDateWeekly
        {
            get
            {
                var result = CurrentDate.StartDayOfWeek(startDateOfWeek: DayOfWeek.Saturday);

                return result.AddDays(-52 * 7 + 6).ToString("MMM dd, yyyy");
            }
        }

        public static string MaxToDateWeekly
        {
            get
            {
                var isWeekend = IsWeekend();

                var result = CurrentDate.StartDayOfWeek(startDateOfWeek: DayOfWeek.Saturday);

                return (isWeekend ? result.AddDays(6) : result.AddDays(-1)).ToString("MMM dd, yyyy");
            }
        }

        public static bool IsWeekend()
            => CurrentDate.DayOfWeek == DayOfWeek.Saturday ||
           CurrentDate.DayOfWeek == DayOfWeek.Sunday;
    }
}
