namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System.Collections.Generic;

    public class SpreadSettingExcel
    {
        public SpreadSettingExcel()
        {
            SpreadValids = new List<SpreadSettingExcelValidModel>();
            SpreadInvalids = new List<SpreadSettingExcelInvalidModel>();
        }

        public List<SpreadSettingExcelValidModel> SpreadValids { get; set; }
        public List<SpreadSettingExcelInvalidModel> SpreadInvalids { get; set; }
    }

    public class SpreadSettingExcelValidModel
    {
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public int SportType { get; set; }
        public int Site { get; set; }
        public int Currency { get; set; }
        public int Platform { get; set; }
        public decimal Spread { get; set; }
    }

    public class SpreadSettingExcelInvalidModel
    {
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public string SportType { get; set; }
        public string Site { get; set; }
        public string Currency { get; set; }
        public string Platform { get; set; }
        public string Spread { get; set; }

        public int ID { get; set; }
        public string Reason { get; set; }
    }
}
