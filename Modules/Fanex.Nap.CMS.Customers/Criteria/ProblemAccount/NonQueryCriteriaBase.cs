namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using Data.Repository;
    using Infrastructure.Constants;

    public class NonQueryCriteriaBase : CriteriaBase
    {
        protected NonQueryCriteriaBase(string customerList, byte roleId, int userId, string ip)
        {
            CustomerList = customerList;
            RoleId = roleId;
            UserId = userId;
            IP = ip;
        }

        public string CustomerList { get; }

        public byte RoleId { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => UserId > 0
            && !string.IsNullOrWhiteSpace(CustomerList)
            && RoleIdConstants.Member <= RoleId && RoleId <= RoleIdConstants.Super;
    }
}
