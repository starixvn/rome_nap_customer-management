namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class RemoveReduceBLCreditSiteCriteria : RemoveReduceBLCriteria
    {
        public RemoveReduceBLCreditSiteCriteria(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip) : base(usernames, roleId, isAllDownlines, confirmed, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_RemoveReduceBetLimit";
    }
}
