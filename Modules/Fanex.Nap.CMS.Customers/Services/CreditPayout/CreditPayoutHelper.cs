namespace Fanex.Nap.CMS.Customers.Services.CreditPayout
{
    using System;

    internal static class CreditPayoutHelper
    {
        private const int OffsetGMT8GMTMinus4 = 12;
        private const string FormatDate = "MM/dd/yyyy";
        private const string FormatDateTime = "MM/dd/yyyy HH:mm:ss";

        internal static bool IsExpired(object expiredDate)
        {
            if (DBNull.Value == expiredDate)
            {
                return false;
            }

            var timeNow = DateTime.Now.AddHours(OffsetGMT8GMTMinus4);

            return Convert.ToDateTime(expiredDate).Date < timeNow.Date;
        }

        internal static string ExtractDateFromDB(object column)
        {
            if (DBNull.Value == column)
            {
                return string.Empty;
            }

            return Convert.ToDateTime(column).ToString(FormatDate);
        }

        internal static string ExtractDateTimeFromDB(object column)
        {
            if (DBNull.Value == column)
            {
                return string.Empty;
            }

            return Convert.ToDateTime(column).ToString(FormatDateTime);
        }

        internal static string ExtractDoubleFromDB(object column)
        {
            return Convert.ToDouble(column).ToString("0,0");
        }
    }
}
