namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.Data.Repository;
    public class GetCurrencyGroupCriteria : CriteriaBase
    {
        public bool IsCredit { get; set; }

        public override string GetSettingKey()
            => "NAP_CurrencyGroup_Sel";

        public override bool IsValid()
            => true;
    }
}