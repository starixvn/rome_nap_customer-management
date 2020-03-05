namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using ProblemAccount;

    public class UpdateInformingStatusLicenseeSiteCriteria : NonQueryWithoutRoleIdCriteriaBase
    {
        public UpdateInformingStatusLicenseeSiteCriteria(string customerList, byte informedType, int userId, string ip)
            : base(customerList, userId, ip)
        {
            InformedType = informedType;
        }

        public byte InformedType { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeInformStatusUpd";
    }
}
