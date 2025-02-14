namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using System;

    public class GetCustomerMarginHistoryCriteria : CriteriaBase
    {
        public string CustIds { get; set; }

        public int MonthToCheck { get; set; }

        public int SportId { get; set; }

        public DateTime ReportToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_CustomerMarginHistory";
        }

        public override bool IsValid()
            => true;
    }
}
