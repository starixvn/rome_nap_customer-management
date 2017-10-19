namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance
{
    using Fanex.Data.Repository;
    using System;

    public class GetCustomerHistoryVirtualSettingMonitorAdvanceCriteria : CriteriaBase
    {
        public int SportId { get; set; }

        public string CustIds { get; set; }

        public DateTime ReportFromDate { get; set; }

        public DateTime ReportToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_CustomerMarginHistoryAdvanced";
        }
    }
}
