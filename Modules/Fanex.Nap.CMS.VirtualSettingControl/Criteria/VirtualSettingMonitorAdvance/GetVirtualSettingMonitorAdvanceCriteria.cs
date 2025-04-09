namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance
{
    using Fanex.Data.Repository;
    using System;

    public class GetVirtualSettingMonitorAdvanceCriteria : CriteriaBase
    {
        public int SportId { get; set; }

        public int Rate { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }

        public DateTime ReportFromDate { get; set; }

        public DateTime ReportToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_VirtualRateMonitoringAdvanced";
        }

        public override bool IsValid()
            => true;
    }
}
