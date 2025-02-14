namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class NonQueryWithoutRoleIdCriteriaBase : CommandBaseCriteria
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

        public override string GetSettingKey()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsValid()
            => UserId > 0
            && !string.IsNullOrWhiteSpace(CustomerList);
    }
}
