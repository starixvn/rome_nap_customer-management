namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using System;
    using Data.Repository;

    public class GetCustomerMarginByMonthCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_GetCustomerMarginByMonth";
        }
    }
}