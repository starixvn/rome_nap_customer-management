namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Data.Repository;

    public class GetDummyAgentCriteria : CriteriaBase
    {
        public GetDummyAgentCriteria()
        {
        }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountDummyAgentSel";
    }
}