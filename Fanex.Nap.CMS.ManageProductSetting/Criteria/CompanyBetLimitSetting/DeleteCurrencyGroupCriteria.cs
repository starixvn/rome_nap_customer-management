namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    public class DeleteCurrencyGroupCriteria : ResultQueryCommand
    {
        public int CurrencyGroupID { get; set; }
        public bool IsCredit { get; set; }
        public int UserID { get; set; }
        public string IP { get; set; }

        public override string GetSettingKey()
            => "NAP_CurrencyGroup_Del";

        public override bool IsValid()
            => true;
    }
}