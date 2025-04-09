namespace Fanex.Nap.CMS.Customers.Models.CreditPayout
{
    using System;
    using Fanex.Nap.CMS.Customers.Services.CreditPayout;

    public class PayoutPromotion
    {
        public int DrawID { get; set; }
        public int CustID { get; set; }
        public string Username { get; set; }
        public string Site { get; set; }
        public double Amount { get; set; }
        public string AmountString => CreditPayoutHelper.ExtractDoubleFromDB(Amount);
        public string Currency { get; set; }
        public int  MatchID { get; set; }
        public string Match { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString => CreditPayoutHelper.ExtractDateFromDB(CreatedDate);
        public DateTime? ExpiredDate { get; set; }
        public string ExpiredDateString => CreditPayoutHelper.ExtractDateFromDB(ExpiredDate);
        public DateTime? PaymentDate { get; set; }
        public string PaymentDateString => CreditPayoutHelper.ExtractDateFromDB(PaymentDate);
        public bool IsExpired => CreditPayoutHelper.IsExpired(ExpiredDate);
        public string LastModifiedUser { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedDateString => CreditPayoutHelper.ExtractDateFromDB(LastModifiedDate);
        public string Remark { get; set; }
    }
}
