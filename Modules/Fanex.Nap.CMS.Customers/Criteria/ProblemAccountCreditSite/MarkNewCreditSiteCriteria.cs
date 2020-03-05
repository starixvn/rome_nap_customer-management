namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class MarkNewCreditSiteCriteria : MarkNewCriteria
    {
        public MarkNewCreditSiteCriteria(
            string usernames,
            byte roleId,
            byte preventActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip) : base(usernames, roleId, reasonId, comment, userId, ip)
        {
            PreventActionId = preventActionId;
        }

        public byte PreventActionId { get; }

        public override string GetSettingKey()
            => "Nap_ProblemAccount_AddCustomer";
    }
}
