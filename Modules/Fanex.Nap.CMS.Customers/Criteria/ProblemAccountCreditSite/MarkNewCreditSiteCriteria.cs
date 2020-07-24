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
            string voidTime,
            decimal limitAmount,
            int userId,
            string ip) : base(usernames, roleId, reasonId, comment, userId, ip)
        {
            PreventActionId = preventActionId;
            VoidTime = voidTime;
            LimitAmount = limitAmount;
        }

        public byte PreventActionId { get; }

        public string VoidTime { get; }

        public decimal LimitAmount { get; }

        public override string GetSettingKey()
            => "Nap_ProblemAccount_AddCustomer";
    }
}
