namespace Fanex.NAP.CMS.BetLimitTypeControl.Models
{
    public class SearchModel
    {
        public int BetLimitTypeId { get; set; }

        public string BetLimitType { get; set; }

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public int LiveIndicator { get; set; }

        public bool RestrictVIP { get; set; }

        public string ProductList { get; set; }

        public string SportTypeList { get; set; }

        public string BetTypeList { get; set; }

        public string LeagueIdList { get; set; }

        public bool IsAllProduct { get; set; }

        public bool IsAllSportType { get; set; }

        public bool IsAllBetType { get; set; }

        public bool IsAllLeague { get; set; }
    }
}
