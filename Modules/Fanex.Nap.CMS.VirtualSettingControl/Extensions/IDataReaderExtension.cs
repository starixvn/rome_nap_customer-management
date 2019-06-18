namespace Fanex.Nap.CMS.VirtualSettingControl.Extensions
{
    using System.Data;
    using Fanex.Common;

    public static class IDataReaderExtension
    {
        public static int ToInt(this IDataReader reader, string columnName, int defaultValue)
        => reader[columnName] == System.DBNull.Value ? defaultValue : Converter.ToInt(reader[columnName]);

        public static decimal ToDecimal(this IDataReader reader, string columnName, decimal defaultValue)
        => reader[columnName] == System.DBNull.Value ? defaultValue : Converter.ToDecimal(reader[columnName]);
    }
}
