namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Data.Repository;

    public class GetDummyAgentCriteria : CriteriaBase
    {
        public GetDummyAgentCriteria(string master, string prefixAgent)
        {
            Master = master;
            PrefixAgent = prefixAgent; 
        }

        public string Master { get; }

        public string PrefixAgent { get; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountDummyAgentSel";
    }
}