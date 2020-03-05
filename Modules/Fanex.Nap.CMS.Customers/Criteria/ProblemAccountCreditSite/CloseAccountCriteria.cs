namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class CloseAccountCriteria : InactivateCriteria
    {
        public CloseAccountCriteria(
            string usernames,
            byte roleId,
            bool confirmed,
            int userId,
            string ip) : base(usernames, roleId, confirmed, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_ClosedAccount";
    }
}
