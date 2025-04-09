namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class RemoveFollowBetAccountCriteria : CommandBaseCriteria
    {
        public RemoveFollowBetAccountCriteria(string username, int dummyAgentId, int userId, string ip)
        {
            Username = username;
            DummyAgentId = dummyAgentId;
            UserId = userId;
            IP = ip;
        }

        public string Username { get; }

        public int DummyAgentId { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountDel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && DummyAgentId > 0
            && UserId > 0;
    }
}
