namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class MarkNewCreditSitesCriteria : MarkNewCriteria
    {
        public MarkNewCreditSitesCriteria(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip) : base(usernames, roleId, reasonId, comment, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "Nap_ProblemAccount_AddCustomer";
    }
}
