namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using ProblemAccountCreditSite;

    public class GetReasonLicenseeSiteCriteria : GetReasonCreditSiteCriteria
    {
        public GetReasonLicenseeSiteCriteria(byte mode)
            : base(mode)
        {
        }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeReasonSel";
    }
}
