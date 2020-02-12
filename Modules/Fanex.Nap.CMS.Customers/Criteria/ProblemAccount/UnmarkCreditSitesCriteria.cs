namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class UnmarkCreditSitesCriteria : UnmarkCriteria
    {
        public UnmarkCreditSitesCriteria(
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
