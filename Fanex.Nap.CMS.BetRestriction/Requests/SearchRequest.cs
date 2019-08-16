namespace Fanex.Nap.CMS.BetRestriction.Requests
{
    public class SearchRequest
    {
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string Status { get; set; }

        public int SiteId { get; set; }

        public int CurrencyId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
