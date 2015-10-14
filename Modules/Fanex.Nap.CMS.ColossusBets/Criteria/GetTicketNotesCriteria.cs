namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketNotesCriteria : CriteriaBase
    {
        public long BetId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_PayoutDetail";
        }
    }
}
