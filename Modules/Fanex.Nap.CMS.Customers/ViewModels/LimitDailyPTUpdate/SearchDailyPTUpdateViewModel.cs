namespace Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate
{
    using System.Web.Mvc;

    public class SearchDailyPTUpdateViewModel
    {
        [AllowHtml]
        public string Username { get; set; }

        public string SiteNames { get; set; }

        public string CurrencyIds { get; set; }

        public string ProductId { get; set; }

        public byte TypeId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
