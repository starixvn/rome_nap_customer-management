namespace Fanex.Nap.CMS.Customers.Criteria.CreditPayout
{
    using Fanex.Data.Repository;

    public class SelPayoutPromotionCriteria : CriteriaBase
    {
        public int TypeId { get; set; }
        public int MatchId { get; set; }
        public string Username { get; set; }
        public string VerificationCode { get; set; }
        public string Status { get; set; }
        public bool IsExpired { get; set; }
        public bool IsTest { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalItems { get; set; }

        public override string GetSettingKey()
            => "NAP_PayoutPromotion_Sel";
    }
}
