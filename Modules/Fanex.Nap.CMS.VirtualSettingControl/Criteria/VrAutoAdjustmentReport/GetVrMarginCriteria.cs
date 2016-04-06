namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport
{
    using System;
    using Data.Repository;

    public class GetVrMarginCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}