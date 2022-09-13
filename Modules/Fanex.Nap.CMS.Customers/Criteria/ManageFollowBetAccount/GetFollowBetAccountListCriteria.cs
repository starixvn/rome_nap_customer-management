namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Data.Repository;

    public class GetFollowBetAccountListCriteria : CriteriaBase
    {
        public GetFollowBetAccountListCriteria(string username, byte currency, string site, int dummyAgentId, bool isActive)
        {
            Username = username;
            Currency = currency;
            Site = site;
            DummyAgentId = dummyAgentId;
            IsActive = isActive;
        }

        public string Username { get; }

        public byte Currency { get; }

        public string Site { get; }

        public int DummyAgentId { get; }

        public bool IsActive { get; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountListSel";
    }
}
