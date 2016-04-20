namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using System;

    public class GetVirtualSettingMonitorCriteria : CriteriaBase
    {
        public int SportId { get; set; }

        public int Rate { get; set; }

        public int TopRows { get; set; }

        public int MonthToCheck { get; set; }

        public int MonthToSum { get; set; }

        public DateTime ReportToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_VirtualRateMonitoring"; 
        }
    }
}
