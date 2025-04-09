namespace Fanex.Nap.CMS.Customers.Criteria.CreditPayout
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class UpdPayoutPromotionCriteria : NonQueryCommand
    {
        [SpParam(Ignored = true)]
        public List<IdUsername> UsernameList { get; set; }
        public string CustIDList { get; set; }
        public byte Status { get; set; }
        public byte TypeID { get; set; }
        public string VerificationCode { get; set; }
        public string PaymentDate { get; set; }
        public string Remark { get; set; }
        public string IP { get; set; }
        public int UserId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_PayoutPromotion_Upd";

        public override bool IsValid()
            => true;
    }

    public class IdUsername
    {
        public int CustId { get; set; }
        public string Username { get; set; }
    }
}
