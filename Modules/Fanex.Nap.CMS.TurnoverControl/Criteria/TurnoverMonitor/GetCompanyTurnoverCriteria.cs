// --------------------------------------------------------------------------
// <copyright file="GetCompanyTurnoverCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Company Turnover Criteria.
    /// </summary>
    public class GetCompanyTurnoverCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustId { get; set; }

        [SpParam(SqlParamName = "matchid")]
        public int MatchId { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_TurnoverMonitor_CompanyTurnover";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
