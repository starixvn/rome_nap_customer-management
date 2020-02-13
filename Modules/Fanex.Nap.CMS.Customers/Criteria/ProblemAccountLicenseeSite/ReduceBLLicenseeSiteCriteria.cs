namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using ProblemAccount;

    public class ReduceBLLicenseeSiteCriteria : NonQueryWithoutRoleIdCriteriaBase
    {
        public ReduceBLLicenseeSiteCriteria(
            string customerList,
            decimal maxPerMatch,
            decimal maxBet,
            int userId,
            string ip)
            : base(customerList, userId, ip)
        {
            MaxPerMatch = maxPerMatch;
            MaxBet = maxBet;
        }

        public decimal MaxPerMatch { get; }

        public decimal MaxBet { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeReduceBetLimit";
    }
}
