namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class InactivateCriteria : NonQueryCriteriaBase
    {
        public InactivateCriteria(
            string custIds,
            byte roleId,
            int userId,
            string ip) : base(custIds, roleId, userId, ip)
        {
        }
    }
}
