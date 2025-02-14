namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Data.Repository;

    public class GetBonusTicketCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_BonusTicket_SelList";
        }

        public override bool IsValid()
            => true;
    }
}
