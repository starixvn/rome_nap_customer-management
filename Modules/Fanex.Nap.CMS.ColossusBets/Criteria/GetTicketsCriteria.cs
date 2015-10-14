namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "username")]
        public string Member { get; set; }

        public long BetId { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int Status { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_PayoutList";
        }
    }
}
