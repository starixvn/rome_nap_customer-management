namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccountCreditSite
{
    using Fanex.Data.Repository;

    public class GetReasonCreditSiteCriteria : CriteriaBase
    {
        public GetReasonCreditSiteCriteria(byte mode)
        {
            Mode = mode;
        }

        public byte Mode { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_ReasonSel";

        public override bool IsValid()
         => true;
    }
}
