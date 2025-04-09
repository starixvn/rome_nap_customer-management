namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCustomerTurnoversCriteria : CriteriaBase
    {
        public GetCustomerTurnoversCriteria(
            string fromDate,
            string toDate,
            short sportType,
            int leagueId,
            int matchId,
            string username,
            bool isExcludeCompletedEvent,
            int percentageUsed,
            DateTime moveBetsDay)
        {
            FromDate = fromDate;
            ToDate = toDate;
            SportType = sportType;
            LeagueId = leagueId;
            MatchId = matchId;
            UserName = username ?? string.Empty;
            IsExcludeCompletedEvent = isExcludeCompletedEvent;
            PercentageUsed = percentageUsed;
            MoveBetsDay = moveBetsDay;
        }

        public string FromDate { get; }

        public string ToDate { get; }

        public short SportType { get; }

        public int LeagueId { get; }

        public int MatchId { get; }

        public string UserName { get; }

        public bool IsExcludeCompletedEvent { get; }

        [SpParam(SqlParamName = "usedpercent")]
        public int PercentageUsed { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverSelect";

        public override bool IsValid()
            => true;
    }
}
