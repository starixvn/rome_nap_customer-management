namespace Fanex.Nap.CMS.Customers.Criteria.CreditPayout
{
    using Fanex.Data.Repository;
    
    public class GetMatchCiretia : CriteriaBase
    {
        public int TypeId { get; set; }

        public int MatchId { get; set; }

        public override string GetSettingKey()
           => "NAP_PayoutPromotion_MatchSel";
    }
}
