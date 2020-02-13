namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountLicenseeSite
{
    using System;
    using ProblemAccount;

    [Serializable]
    public class SearchLicenseeSiteViewModel : SearchBaseViewModel
    {
        public byte Acknowledge { get; set; }

        public byte VIP { get; set; }

        public byte PreventiveActionId { get; set; }

        public string Member { get; set; }

        public string Licensee { get; set; }
    }
}
