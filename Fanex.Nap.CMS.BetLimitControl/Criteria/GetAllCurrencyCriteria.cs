namespace Fanex.NAP.CMS.BetLimitControl.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "NAP_BetRestriction_SelAllCurrency";

        public override bool IsValid() => true;
    }
}
