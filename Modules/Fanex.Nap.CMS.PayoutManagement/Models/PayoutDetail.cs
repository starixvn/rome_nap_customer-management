namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System.Collections.Generic;

    public class PayoutDetail
    {
        public int Id { get; set; }

        public long RefNo { get; set; }

        public string TransId3rd { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public string Site { get; set; }

        public string SecurityCode { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public IList<PayoutNote> PayoutNotes { get; set; }
    }
}
