namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketNotesCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "PayoutTransId")]
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_SelDetail";
        }
    }
}
