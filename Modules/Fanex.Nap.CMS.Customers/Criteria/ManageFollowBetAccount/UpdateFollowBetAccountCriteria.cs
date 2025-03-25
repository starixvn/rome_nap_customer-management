namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class UpdateFollowBetAccountCriteria : CommandBaseCriteria
    {
        public UpdateFollowBetAccountCriteria(string username, int dummyAgentId, bool isActive, int userId, string ip)
        {
            Username = username;
            DummyAgentId = dummyAgentId;
            IsActive = isActive;
            UserId = userId;
            IP = ip;
        }

        public string Username { get; }

        public int DummyAgentId { get; }

        public bool IsActive { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountUpd";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && DummyAgentId > 0
            && UserId > 0;
    }
}
