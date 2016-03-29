namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Dashboard info.
    /// </summary>
    public class GetDashboardInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int MonthToCheck { get; set; }

        public DateTime ReportToDate { get; set; }

        /// <summary>
        /// Gets Setting Key.
        /// </summary>
        /// <returns>Setting key of the criteria.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_GetCustomerDashboardInfo";
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
