namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using Fanex.Data.Repository;

    public class GetCurrenciesCreditSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_ProblemAccount_CurrencySel";

        public override bool IsValid()
         => true;
    }
}
