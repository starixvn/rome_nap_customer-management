using Fanex.Data.Repository;

namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using System;
    using Fanex.NAP.CMS.Criteria.Common;

    public class GetVrAdjustmentLogCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Report_CustMngt_GetVrAdjustmentLog";
        }
    }
}