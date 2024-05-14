namespace Fanex.Nap.CMS.Customers.Models.CreditPayout
{
    using System.Collections.Generic;
    
    public class SearchPayoutResponse
    {
        public IEnumerable<PayoutPromotion> Payouts { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
