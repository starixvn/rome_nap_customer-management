namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketDetailCriteria : CriteriaBase
    {
        public long BetId { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_TicketDetail";
        }
    }
}
