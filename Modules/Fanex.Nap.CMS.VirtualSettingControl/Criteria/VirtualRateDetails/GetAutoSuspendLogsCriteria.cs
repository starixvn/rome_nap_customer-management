namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetAutoSuspendLogsCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int SportType { get; set; }

        [SpParam(SqlParamName = "StartLogTime")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "EndLogTime")]
        public DateTime ToDate { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "Acc_CustVirtualMultiplier_ViewLog_AutoSuspend";

        public override bool IsValid()
            => true;
    }
}
