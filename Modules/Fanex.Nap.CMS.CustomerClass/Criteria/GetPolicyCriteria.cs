namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetPolicyCriteria : QueryCriteriaBase
    {
        public GetPolicyCriteria(int policyId, string policyTitle, string leagueGroups)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
            LeagueGroups = leagueGroups;
        }

        public int PolicyId { get; }
        public string PolicyTitle { get; }
        public string LeagueGroups { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySel";
    }
}
