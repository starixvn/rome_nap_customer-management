namespace Fanex.Nap.CMS.Customers.Criteria.CreditPayout
{
    using Fanex.Data.Repository;

    public class VerifySelPayoutPromotionCriteria : CriteriaBase
    {
        public byte TypeID { get; set; }
        public int DrawID { get; set; }
        public string Username { get; set; }
        public string VerificationCode { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_PayoutPromotion_VerifySel";

        public override bool IsValid()
            => true;
    }
}
