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
            string voidTime,
            decimal limitAmount,
            bool updateTime,
            bool updateLimitAmount,
            int userId,
            string ip) : base(usernames, roleId, userId, ip)
        {
            PreventActionId = preventActionId;
            ReasonId = reasonId;
            Comment = comment;
            VoidTime = voidTime;
            LimitAmount = limitAmount;
            UpdateTime = updateTime;
            UpdateLimitAmount = updateLimitAmount;
        }

        public byte PreventActionId { get; }

        public byte ReasonId { get; }

        public string Comment { get; }

        public string VoidTime { get; }

        public decimal LimitAmount { get; }

        public bool UpdateTime { get; }

        public bool UpdateLimitAmount { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_UpdateCustomer";
    }
}