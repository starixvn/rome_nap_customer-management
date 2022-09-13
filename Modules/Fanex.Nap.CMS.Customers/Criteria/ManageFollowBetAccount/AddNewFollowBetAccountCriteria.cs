namespace Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount
{
    using Fanex.Data.Repository;

    public class AddNewFollowBetAccountCriteria : CriteriaBase
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

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountIns";
    }
}
