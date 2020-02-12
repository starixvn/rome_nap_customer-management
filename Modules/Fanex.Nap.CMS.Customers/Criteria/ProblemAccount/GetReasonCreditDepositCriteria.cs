namespace Fanex.Nap.CMS.Customers.Criteria.ProblemAccount
{
    public class GetReasonCreditDepositCriteria : GetReasonCriteria
    {
        public GetReasonCreditDepositCriteria(byte mode, bool isLicensee)
            : base(mode)
        {
            IsLicensee = isLicensee;
        }

        public bool IsLicensee { get; }

        public override string GetSettingKey()
            => "NAP_ProblemAccount_ReasonSel";
    }
}
