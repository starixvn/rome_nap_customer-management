namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBetTicketCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "PayoutTransId")]
        public int Id { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int Member { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_SelBetTicket";
        }

        public override bool IsValid()
            => true;
    }
}
