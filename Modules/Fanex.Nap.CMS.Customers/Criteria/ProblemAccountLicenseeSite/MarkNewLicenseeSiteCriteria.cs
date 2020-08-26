namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using ProblemAccount;

    public class MarkNewLicenseeSiteCriteria : NonQueryWithoutRoleIdCriteriaBase
    {
        public MarkNewLicenseeSiteCriteria(
            string usernames,
            byte preventActionId,
            byte reasonId,
            string comment,
            string voidTime,
            decimal limitAmount,
            int userId,
            string ip) : base(usernames, userId, ip)
        {
            ActionId = preventActionId;
            ReasonId = reasonId;
            Comment = comment;
            VoidTime = voidTime;
            LimitAmount = limitAmount;
        }

        public byte ActionId { get; }

        public byte ReasonId { get; }

        public string Comment { get; }

        public string VoidTime { get; }

        public decimal LimitAmount { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAddCustomer";
    }
}
