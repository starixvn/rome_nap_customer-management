namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class RemoveReduceBLCriteria : UnmarkCriteria
    {
        public RemoveReduceBLCriteria(
            bool? abnormalBet,
            bool? unmarkByOPS,
            string custIds,
            byte roleId,
            int userId,
            string ip) : base(abnormalBet, unmarkByOPS, custIds, roleId, userId, ip)
        {
        }
    }
}
