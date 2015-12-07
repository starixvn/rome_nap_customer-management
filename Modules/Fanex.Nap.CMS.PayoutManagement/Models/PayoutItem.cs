namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class PayoutItem
    {
        public int Id { get; set; }

        public IdNameModel Product { get; set; }

        public IdNameModel Member { get; set; }

        public string Site { get; set; }

        public DateTime Date { get; set; }

        public long RefNo { get; set; }

        public IdNameModel Currency { get; set; }

        public decimal Amount { get; set; }

        public IdNameModel Status { get; set; }

        public bool HasNotes { get; set; }

        public string DateDisplay
        {
            get
            {
                return Date.ToShortFormatedDateWithSlash();
            }
        }
    }
}
