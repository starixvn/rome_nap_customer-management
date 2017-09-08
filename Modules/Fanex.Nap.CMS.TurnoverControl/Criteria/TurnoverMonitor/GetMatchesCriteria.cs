using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetMatchesCriteria : CriteriaBase
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_TOControl_MatchSelect";
        }

        public override bool IsValid()
        {
            return LeagueId > 0;
        }
    }
}
