namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class GetAllCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SelAllCurrency";
        }

        public override bool IsValid()
            => true;
    }
}
