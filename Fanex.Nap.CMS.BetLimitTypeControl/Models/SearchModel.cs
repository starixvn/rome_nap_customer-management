namespace Fanex.NAP.CMS.BetLimitTypeControl.Models
{
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Newtonsoft.Json;
    using System;

    public class SearchModel
    {
        public int BetLimitTypeId { get; set; }

        public string BetLimitType { get; set; }

        [JsonConverter(typeof(TimeSpanFormatConverter), @"hh\:mm")]
        public TimeSpan? BeginTime { get; set; }

        [JsonConverter(typeof(TimeSpanFormatConverter), @"hh\:mm")]
        public TimeSpan? EndTime { get; set; }

        public int LiveIndicator { get; set; }

        public bool RestrictVIP { get; set; }

        public string ProductList { get; set; }

        public string SportTypeList { get; set; }

        public string BetTypeList { get; set; }

        public string LeagueGroupList { get; set; }

        public string ExcludedLeagueGroupList { get; set; }

        public string LeagueList { get; set; }

        public bool IsAllProduct { get; set; }

        public bool IsAllSportType { get; set; }

        public bool IsAllBetType { get; set; }

        public bool IsAllLeague { get; set; }

        public bool IsAllLeagueGroup
            => string.IsNullOrWhiteSpace(LeagueGroupList) && string.IsNullOrWhiteSpace(ExcludedLeagueGroupList);
    }
}
