namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketDetailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int MemberId { get; set; }

        [SpParam(SqlParamName = "refno")]
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetListCB_Details";
        }
    }
}
