namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class AddNewFollowBetAccountCriteria : CommandBaseCriteria
    {
        public AddNewFollowBetAccountCriteria(int dummyAgentId, string custList, int userId, string ip)
        {
            DummyAgentId = dummyAgentId;
            CustList = custList;
            UserId = userId;
            IP = ip;
        }

        public int DummyAgentId { get; }

        public string CustList { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountIns";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustList)
            && DummyAgentId > 0
            && UserId > 0;
    }
}
