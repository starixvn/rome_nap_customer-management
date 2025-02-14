namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite
{
    using Fanex.Data.Repository;

    public class GetLicenseeAcknowledgementCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "NAP_ProblemAccount_LicenseeAcknowledgementSel";

        public override bool IsValid()
         => true;
    }
}
