using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline
{
    public class AddNewLimitCriteria : NonQueryCommand
    {
        public AddNewLimitCriteria(string usernames, int userId, string ip)
        {
            CustomerList = usernames;
            UserId = userId;
            IP = ip;
        }

        public string CustomerList { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int CntMember { get; set; }

        public override string GetSettingKey()
            => "Nap_ProblemAccountLimitDownlineIns";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustomerList) && UserId > 0;
    }
}
