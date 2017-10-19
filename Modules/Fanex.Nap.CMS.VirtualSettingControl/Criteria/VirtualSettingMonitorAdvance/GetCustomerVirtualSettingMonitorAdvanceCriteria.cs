namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance
{
    using Fanex.Data.Repository;
    using System;

    public class GetCustomerVirtualSettingMonitorAdvanceCriteria : CriteriaBase
    {
        public int SportId { get; set; }

        public int Rate { get; set; }

        public int RoleId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_VirtualRateMonitoringAdvanced";
        }
    }
}
