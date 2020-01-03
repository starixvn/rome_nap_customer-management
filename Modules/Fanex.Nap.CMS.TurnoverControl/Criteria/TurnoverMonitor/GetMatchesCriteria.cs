using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetMatchesCriteria : CriteriaBase
    {
        public GetMatchesCriteria(string fromDate, string toDate, int leagueId)
        {
            FromDate = fromDate;
            ToDate = toDate;
            LeagueId = leagueId;
        }

        public string FromDate { get; }

        public string ToDate { get; }

        public int LeagueId { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchSelect";

        public override bool IsValid()
            => LeagueId > 0;
    }
}
