namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
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
