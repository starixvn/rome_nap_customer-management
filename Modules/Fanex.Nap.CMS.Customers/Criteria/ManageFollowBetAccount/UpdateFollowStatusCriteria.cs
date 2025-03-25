namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class UpdateFollowStatusCriteria : CommandBaseCriteria
    {
        public UpdateFollowStatusCriteria(string username, int dummyAgentId, bool isFollow, int userId, string ip)
        {
            Username = username;
            DummyAgentId = dummyAgentId;
            IsFollow = isFollow;
            UserId = userId;
            IP = ip;
        }

        public string Username { get; }

        public int DummyAgentId { get; }

        public bool IsFollow { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountUpdFollowStatus";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && DummyAgentId > 0
            && UserId > 0;
    }
}
