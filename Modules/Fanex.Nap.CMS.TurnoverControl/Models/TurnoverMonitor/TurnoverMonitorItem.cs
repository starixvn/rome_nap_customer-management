using System;

namespace Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor
{
    public class TurnoverMonitorItem
    {
        public short SportType { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public bool IsNeutral { get; set; }

        public DateTime EventDate { get; set; }

        public string EventStatus { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal? HouseTurnover { get; set; }

        public decimal? MaxTurnover { get; set; }

        public decimal? PercentageUsed { get; set; }

        public decimal TotalMemberTurnover { get; set; }

        public decimal TotalHouseTurnover { get; set; }

        public int ProfileId { get; set; }

        public string GroupName { get; set; }
    }
}
