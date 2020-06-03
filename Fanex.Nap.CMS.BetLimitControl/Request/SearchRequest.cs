namespace Fanex.NAP.CMS.BetLimitControl.Request
{
    public class SearchRequest
    {
        public int? LevelId { get; set; }

        public int? Currency { get; set; }

        public int? SiteId { get; set; }

        public int? BetLimitTypeID { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}