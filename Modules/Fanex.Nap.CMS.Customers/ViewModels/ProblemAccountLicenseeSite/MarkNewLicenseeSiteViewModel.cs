namespace Fanex.Nap.CMS.Customers.ViewModels.ProblemAccountLicenseeSite
{
    using System.Web.Mvc;

    public class MarkNewLicenseeSiteViewModel
    {
        public string Usernames { get; set; }

        public byte PreventiveAction { get; set; }

        public byte Reason { get; set; }

        public string VoidTime { get; set; }

        public decimal LimitAmount { get; set; }

        public bool UpdateTime { get; set; }

        public bool UpdateLimitAmount { get; set; }

        [AllowHtml]
        public string Comment { get; set; }

        public virtual bool IsInvalid()
            => string.IsNullOrWhiteSpace(Usernames)
            || PreventiveAction <= 0
            || Reason <= 0;
    }
}
