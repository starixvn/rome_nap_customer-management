namespace Fanex.Nap.CMS.VirtualSettingControl.Models.UploadVirtualRate
{
    using System.Collections.Generic;

    public class ExcelErrorModel
    {
        public int ValidSetting { get; set; }

        public List<ExcelErrorDTO> ExcelErrors { get; set; }
    }
}
