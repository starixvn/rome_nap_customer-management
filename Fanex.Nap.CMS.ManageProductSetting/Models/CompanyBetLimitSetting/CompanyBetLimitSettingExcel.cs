namespace Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting
{
    public class CompanyBetLimitSettingExcel
    {
        public string SportType { get; set; }
        public int SportTypeId { get; set; }
        public string CreditCurrencyGroupId { get; set; }
        public string CreditCurrencyGroup { get; set; }
        public string DepositCurrencyGroupId { get; set; }
        public string DepositCurrencyGroup { get; set; }
        public int SiteId { get; set; }
        public string Site { get; set; }
        public string GameGroup { get; set; }
        public decimal? MinBet { get; set; }
        public decimal? MaxBet { get; set; }
        public string Remark { get; set; }
        public decimal? CurMinBet { get; set; }
        public decimal? CurMaxBet { get; set; }
        public string CurRemark { get; set; }
    }
}