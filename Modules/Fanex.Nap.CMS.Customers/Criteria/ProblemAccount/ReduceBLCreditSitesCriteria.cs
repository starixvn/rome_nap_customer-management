namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class ReduceBLCreditSitesCriteria : ReduceBLCriteria
    {
        public ReduceBLCreditSitesCriteria(
            string usernames,
            byte roleId,
            int maxPerMatch,
            int maxBet,
            int userId,
            string ip) : base(usernames, roleId, maxPerMatch, maxBet, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_ReduceBetLimit";
    }
}
