namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get general info.
    /// </summary>
    public class GetSummaryInfoCriteria : CriteriaBase
    {
        public int ReportType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "TGRS_Rpt_SummaryGeneralInfo_Detail";
        }

        /// <summary>
        /// Checks validation of the criteria.
        /// </summary>
        /// <returns>Whether or not the criteria is valid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
