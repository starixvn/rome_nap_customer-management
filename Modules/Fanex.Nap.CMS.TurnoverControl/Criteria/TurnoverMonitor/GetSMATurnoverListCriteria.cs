// --------------------------------------------------------------------------
// <copyright file="GetSMATurnoverListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get SMA Turnover List Criteria.
    /// </summary>
    public class GetSMATurnoverListCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "fromdate")]
        public string FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public string ToDate { get; set; }

        [SpParam(SqlParamName = "sporttype")]
        public int SportId { get; set; }

        [SpParam(SqlParamName = "leagueid")]
        public int LeagueId { get; set; }

        [SpParam(SqlParamName = "usedpercent")]
        public int PercentageUsed { get; set; }

        [SpParam(SqlParamName = "movebetsday")]
        public DateTime MoveBetsDay
        {
            get
            {
                return AppSession.BalanceUpDate.MoveBetsDay;
            }
        }

        public override string GetSettingKey()
        {
            return "TurnoverControl_TurnoverMonitor_SMATurnoverList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
