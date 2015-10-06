namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketNotesCriteria : CriteriaBase
    {
        public int TicketId { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }
}
