namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using System;
    using Data.Repository;

    public class GetCustomerMarginByMonthCriteria : CriteriaBase
    {
        public string CustIds { get; set; }

        public int MonthToCheck { get; set; }

        public DateTime ReportToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_CustomerMarginHistory";
        }
    }
}