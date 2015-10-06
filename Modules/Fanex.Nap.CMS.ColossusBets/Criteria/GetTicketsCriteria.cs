namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetTicketsCriteria : CriteriaBase
    {
        public string Member { get; set; }

        public string BetId { get; set; }

        public int Status { get; set; }

        [SpParam(Ignored = true)]
        public bool ShowSecurityCode { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }
    }
}
