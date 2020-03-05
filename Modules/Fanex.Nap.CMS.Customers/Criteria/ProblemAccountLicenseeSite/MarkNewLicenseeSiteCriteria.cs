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
            int userId,
            string ip) : base(usernames, userId, ip)
        {
            ActionId = preventActionId;
            ReasonId = reasonId;
            Comment = comment;
        }

        public byte ActionId { get; }

        public byte ReasonId { get; }

        public string Comment { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAddCustomer";
    }
}
