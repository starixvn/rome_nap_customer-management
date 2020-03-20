namespace Fanex.Nap.CMS.VirtualSettingControl.Models.UploadVirtualRate
{
    using System.Collections.Generic;

    public class ExcelErrorModel
    {
        public ExcelErrorModel(
            IEnumerable<ExcelErrorDTO> excelErrors,
            int validSetting,
            int totalRows,
            string title)
        {
            ExcelErrors = excelErrors;
            ValidSetting = validSetting;
            TotalRows = totalRows;
            Title = title;
        }

        public IEnumerable<ExcelErrorDTO> ExcelErrors { get; }

        public int ValidSetting { get; }

        public int TotalRows { get; set; }

        public string Title { get; }
    }
}
