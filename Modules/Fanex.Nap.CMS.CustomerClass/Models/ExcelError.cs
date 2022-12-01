namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class ExcelError
    {
        public ExcelError(string reason)
            : this(null, null, null, reason)
        {
        }

        public ExcelError(string cell, string columnName, string value, string reason)
        {
            Cell = cell;
            ColumnName = columnName;
            Value = value;
            Reason = reason;
        }

        public string Cell { get; }

        public string ColumnName { get; }

        public string Value { get; }

        public string Reason { get; }
    }
}
