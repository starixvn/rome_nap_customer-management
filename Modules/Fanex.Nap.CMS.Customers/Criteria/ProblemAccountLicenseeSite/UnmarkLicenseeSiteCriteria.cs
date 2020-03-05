namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using ProblemAccount;

    public class UnmarkLicenseeSiteCriteria : NonQueryWithoutRoleIdCriteriaBase
    {
        public UnmarkLicenseeSiteCriteria(string customerList, int userId, string ip)
            : base(customerList, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeRemoveCustomer";
    }
}
