namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System;
    using Fanex.Data.Repository;

    public class GetVirtualRateLogsCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int SportType { get; set; }

        [SpParam(SqlParamName = "IncludingUpline")]
        public bool IncludeUpline { get; set; }

        [SpParam(SqlParamName = "StartLogTime")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "EndLogTime")]
        public DateTime ToDate { get; set; }

        public int ErrCode { get; set; } = 0;

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey()
            => "Acc_CustVirtualMultiplier_ViewLog";
    }
}
