namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class UnmarkCriteria : InactivateCriteria
    {
        public UnmarkCriteria(
            bool? abnormalBet,
            bool? unmarkByOPS,
            string custIds,
            byte roleId,
            int userId,
            string ip) : base(custIds, roleId, userId, ip)
        {
            AbnormalBet = abnormalBet;
            UnmarkByOPS = unmarkByOPS;
        }

        public bool? AbnormalBet { get; }

        public bool? UnmarkByOPS { get; }
    }
}
