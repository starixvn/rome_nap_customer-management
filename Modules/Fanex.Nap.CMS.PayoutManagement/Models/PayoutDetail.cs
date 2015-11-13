namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class PayoutDetail
    {
        public long RefNo { get; set; }

        public string TransId3rd { get; set; }

        public string Site { get; set; }

        public string SecurityCode { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public IList<PayoutNote> PayoutNotes { get; set; }
    }
}
