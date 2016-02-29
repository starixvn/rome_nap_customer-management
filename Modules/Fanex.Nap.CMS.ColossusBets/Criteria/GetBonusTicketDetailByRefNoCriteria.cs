namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using Fanex.Data.Repository;

    public class GetBonusTicketDetailByRefNoCriteria : CriteriaBase
    {
        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_BonusTicket_SelRefInfo";
        }
    }
}
