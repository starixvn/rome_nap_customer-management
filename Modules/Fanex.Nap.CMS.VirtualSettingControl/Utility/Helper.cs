using System.Collections.Generic;

namespace Fanex.Nap.CMS.VirtualSettingControl.Utility
{
    using System;

    public static class Helper
    {
        public static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan";

                case 2:
                    return "Feb";

                case 3:
                    return "Mar";

                case 4:
                    return "Apr";

                case 5:
                    return "May";

                case 6:
                    return "Jun";

                case 7:
                    return "Jul";

                case 8:
                    return "Aug";

                case 9:
                    return "Sep";

                case 10:
                    return "Oct";

                case 11:
                    return "Nov";

                case 12:
                    return "Dec";

                default:
                    throw new Exception(month + " is not a valid month");
            }
        }

        public static string[] GetMonthListName(int[] months)
        {
            var monthListName = new string[months.Length];

            for (var i = 0; i < months.Length; i++)
            {
                monthListName[i] = GetMonthName(months[i]);
            }

            return monthListName;
        }

        public static int[] GenerateMonthList(DateTime currentMonth, int monthRange)
        {
            var result = new int[monthRange];

            for (var i = 1; i <= monthRange; i++)
            {
                result[monthRange - i] = currentMonth.Month;
                currentMonth = currentMonth.AddMonths(-1);
            }

            return result;
        }

        public static IEnumerable<string> GetDate(DateTime currentTime, int monthRange, string format)
        {
            for (int i = monthRange; i > 0; --i)
            {
                yield return currentTime.AddMonths(-i).ToString(format);
            }
        }
    }
}