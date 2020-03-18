using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetLeaguesCriteria : CriteriaBase
    {
        public GetLeaguesCriteria(
            string fromDate,
            string toDate,
            short sportType,
            DateTime moveBetsDay)
        {
            FromDate = fromDate;
            ToDate = toDate;
            SportType = sportType;
            MoveBetsDay = moveBetsDay;
        }

        public string FromDate { get; }

        public string ToDate { get; }

        public short SportType { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey()
            => "TurnoverControl_TurnoverMonitor_LeagueList";
    }
}
