namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using ProblemAccount;

    public class UpdateCreditSiteCriteria : NonQueryCriteriaBase
    {
        public UpdateCreditSiteCriteria(
            string usernames,
            byte roleId,
            byte preventActionId,
            byte reasonId,
            string comment,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            PreventActionId = preventActionId;
            ReasonId = reasonId;
            Comment = comment;
        }

        public byte PreventActionId { get; }

        public byte ReasonId { get; }

        public string Comment { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_UpdateCustomer";
    }
}