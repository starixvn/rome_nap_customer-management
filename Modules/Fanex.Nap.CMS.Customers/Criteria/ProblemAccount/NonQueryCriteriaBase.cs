namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using Infrastructure.Constants;

    public class NonQueryCriteriaBase : NonQueryWithoutRoleIdCriteriaBase
    {
        protected NonQueryCriteriaBase(string customerList, byte roleId, int userId, string ip)
            : base(customerList, userId, ip)
        {
            RoleId = roleId;
        }

        public byte RoleId { get; }

        public override bool IsValid()
            => base.IsValid()
            && RoleIdConstants.Member <= RoleId && RoleId <= RoleIdConstants.Super;
    }
}