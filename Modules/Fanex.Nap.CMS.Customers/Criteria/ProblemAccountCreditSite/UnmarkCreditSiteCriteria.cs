namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class UnmarkCreditSiteCriteria : UnmarkCriteria
    {
        public UnmarkCreditSiteCriteria(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip) : base(usernames, roleId, excludeAbnormalMember, excludeDirectMark, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_RemoveCustomer";
    }
}
