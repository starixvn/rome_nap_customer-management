namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using ProblemAccount;

    public class RemoveReduceBLLicenseeSiteCriteria : NonQueryWithoutRoleIdCriteriaBase
    {
        public RemoveReduceBLLicenseeSiteCriteria(string customerList, bool isConfirmed, int userId, string ip)
            : base(customerList, userId, ip)
        {
            IsConfirmed = isConfirmed;
        }

        public bool IsConfirmed { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeRemoveReduceBetLimit";
    }
}
