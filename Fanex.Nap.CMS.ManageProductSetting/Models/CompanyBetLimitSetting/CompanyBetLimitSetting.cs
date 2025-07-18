namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    public class CompanyBetLimitSetting
    {
        public int SportType { get; set; }
        public string SportName { get; set; }
        public int CurrencyGroupID { get; set; }
        public int ExchangeID { get; set; }
        public string Currency { get; set; }
        public int SiteID { get; set; }
        public string Site { get; set; }
        public int GameGroupID { get; set; }
        public string GameGroup { get; set; }
        public decimal? MinBet { get; set; }
        public decimal? MaxBet { get; set; }
        public string Remark { get; set; }
    }
}