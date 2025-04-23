namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    public class InsertCurrencyGroupCriteria : ResultQueryCommand
    {
        public string CurrencyGroup { get; set; }
        public int UserID { get; set; }
        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_CurrencyGroup_Ins";
    }
}