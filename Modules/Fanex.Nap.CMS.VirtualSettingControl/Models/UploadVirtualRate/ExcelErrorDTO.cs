namespace Fanex.Nap.CMS.VirtualSettingControl.Models.UploadVirtualRate
{
    public class ExcelErrorDTO
    {
        public ExcelErrorDTO(string reason)
            : this(null, null, null, null, reason)
        {
        }

        public ExcelErrorDTO(string cellId, string username, string value, string reason)
            : this(cellId, username, value, null, reason)
        {
        }

        public ExcelErrorDTO(string cellId, string username, string value, string rule, string reason)
        {
            CellId = cellId;
            Username = username;
            Value = value;
            Rule = rule;
            Reason = reason;
        }

        public string CellId { get; }

        public string Username { get; }

        public string Value { get; }

        public string Rule { get; }

        public string Reason { get; }
    }
}
