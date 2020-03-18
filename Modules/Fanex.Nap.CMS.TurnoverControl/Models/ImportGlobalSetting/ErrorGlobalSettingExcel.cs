namespace Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting
{
    public class ErrorGlobalSettingExcel
    {
        public ErrorGlobalSettingExcel(
            int excelRow,
            string username,
            string field,
            string value,
            string reason)
        {
            ExcelRow = excelRow;
            Username = username;
            Field = field;
            Value = value;
            Reason = reason;
        }

        public int ExcelRow { get; }

        public string Username { get; }

        public string Field { get; }

        public string Value { get; }

        public string Reason { get; }
    }
}
