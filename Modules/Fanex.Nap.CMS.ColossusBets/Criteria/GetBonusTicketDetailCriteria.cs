namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetBonusTicketDetailCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_BonusTicket_SelDetail";
        }
        public override bool IsValid()
            => true;
    }
}
