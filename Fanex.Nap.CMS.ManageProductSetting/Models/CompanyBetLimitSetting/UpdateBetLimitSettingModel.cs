namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    public class UpdateBetLimitSettingModel
    {
        public string SportType { get; set; }
        public string CreditGroupID { get; set; }
        public string LicenseeGroupID { get; set; }
        public string SiteID { get; set; }
        public string GameGroupID { get; set; }
        public string NewMinBet { get; set; }
        public string NewMaxBet { get; set; }
        public string NewRemark { get; set; }
        public string CurMinBet { get; set; }
        public string CurMaxBet { get; set; }
        public string CurRemark { get; set; }
    }
}