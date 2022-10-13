namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Data.Repository;

    public class RemoveFollowBetAccountCriteria : CriteriaBase
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

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountDel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && DummyAgentId > 0
            && UserId > 0;
    }
}
