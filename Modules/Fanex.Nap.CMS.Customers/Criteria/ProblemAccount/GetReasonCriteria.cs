namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    using Data.Repository;

    public class GetReasonCriteria : CriteriaBase
    {
        public GetReasonCriteria(byte mode)
        {
            Mode = mode;
        }

        public byte Mode { get; }

        public override string GetSettingKey()
            => "CS_ProblemAccount_ReasonSel";
    }
}
