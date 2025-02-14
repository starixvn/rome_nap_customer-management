namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using Fanex.Data.Repository;

    public class GetPreventiveActionLicenseeSiteCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseePreventiveActionSel";

        public override bool IsValid()
         => true;
    }
}
