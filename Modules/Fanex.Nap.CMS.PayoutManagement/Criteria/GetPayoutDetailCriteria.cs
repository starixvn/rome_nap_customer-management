namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetPayoutDetailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "PayoutTransId")]
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_SelDetail";
        }

        public override bool IsValid()
            => true;
    }
}
