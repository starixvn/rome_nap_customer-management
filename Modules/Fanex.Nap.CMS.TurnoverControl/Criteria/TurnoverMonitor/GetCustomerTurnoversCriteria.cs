using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetCustomerTurnoversCriteria : CriteriaBase
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int SportType { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string UserName { get; set; }

        public bool IsExcludeCompletedEvent { get; set; }

        [SpParam(SqlParamName = "usedpercent")]
        public int PercentageUsed { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_TurnoverMonitor_SMATurnoverList";
        }
    }
}
