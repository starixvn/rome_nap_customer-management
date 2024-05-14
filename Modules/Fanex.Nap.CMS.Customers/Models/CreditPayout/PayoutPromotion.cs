namespace Fanex.Nap.CMS.Customers.Models.CreditPayout
{
    using System;

    public class PayoutPromotion
    {
        public int DrawID { get; set; }
        public int CustID { get; set; }
        public string Username { get; set; }
        public string Site { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public int  MatchID { get; set; }
        public string Match { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string ExpiredDate { get; set; }
        public string PaymentDate { get; set; }
        public bool IsExpired { get; set; }
        public string LastModifiedUser { get; set; }
        public string LastModifiedDate { get; set; }
        public string Remark { get; set; }
    }
}
