namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using Fanex.Data.Repository;

    public class GetPreventiveActionCreditSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_ProblemAccount_ActionSel";

        public override bool IsValid()
         => true;
    }
}
