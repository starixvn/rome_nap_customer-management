namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using Fanex.Data.Repository;

    public class NonQueryWithoutRoleIdCriteriaBase : CriteriaBase
    {
        protected NonQueryWithoutRoleIdCriteriaBase(string customerList, int userId, string ip)
        {
            CustomerList = customerList;
            UserId = userId;
            IP = ip;
        }

        public string CustomerList { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => UserId > 0
            && !string.IsNullOrWhiteSpace(CustomerList);
    }
}
