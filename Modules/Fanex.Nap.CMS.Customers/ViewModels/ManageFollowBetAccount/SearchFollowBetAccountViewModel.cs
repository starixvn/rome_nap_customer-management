namespace Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount
{
    using System.Web.Mvc;

    public class SearchFollowBetAccountViewModel
    {
        [AllowHtml]
        public string Username { get; set; }

        public string SiteName { get; set; }

        public byte CurrencyId { get; set; }

        public int DummyAgentId { get; set; }

        public bool IsActive { get; set; }

        public bool IsFollow { get; set; }

        public string CurrencyName { get; set; }

        public string DummyAgentName { get; set; }
    }
}
