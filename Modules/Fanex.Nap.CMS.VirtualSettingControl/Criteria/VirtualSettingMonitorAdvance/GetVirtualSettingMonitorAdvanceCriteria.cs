namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance
{
    using Fanex.Data.Repository;
    using System;

    public class GetVirtualSettingMonitorAdvanceCriteria : CriteriaBase
    {
        public int SportId { get; set; }

        public int Rate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Depth { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_VirtualSettingMonitorAdvance";
        }
    }
}
