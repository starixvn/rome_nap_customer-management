namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public class PayoutNote
    {
        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string Note { get; set; }

        public string Handler { get; set; }

        public string DateDisplay
        {
            get
            {
                return Date.ToLongFormatedDateWithSlash();
            }
        }
    }
}
