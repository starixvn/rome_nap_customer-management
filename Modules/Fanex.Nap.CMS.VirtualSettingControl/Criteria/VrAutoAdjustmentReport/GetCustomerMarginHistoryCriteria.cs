namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using Fanex.Data.Repository;
    using System;

    public class GetCustomerMarginHistoryCriteria : CriteriaBase
    {
        public DateTime AdjustDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_GetCustomerMarginHistoryByDate";
        }
    }
}