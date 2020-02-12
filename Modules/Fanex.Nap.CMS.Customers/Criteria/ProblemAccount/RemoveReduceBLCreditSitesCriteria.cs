namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class RemoveReduceBLCreditSitesCriteria : RemoveReduceBLCriteria
    {
        public RemoveReduceBLCreditSitesCriteria(
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
