namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateTicketStatusCriteria : CriteriaBase
    {
        public int TicketId { get; set; }

        public string Status { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }
}
