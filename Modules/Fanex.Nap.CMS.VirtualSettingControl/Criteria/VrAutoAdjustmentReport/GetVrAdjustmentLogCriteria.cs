namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using System;
    using Fanex.NAP.CMS.Criteria.Common;

    public class GetVrAdjustmentLogCriteria : PagingCriteria
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public short PageIndex { get; set; }

        public short PageSize { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_GetVrAdjustmentLog";
        }
    }
}