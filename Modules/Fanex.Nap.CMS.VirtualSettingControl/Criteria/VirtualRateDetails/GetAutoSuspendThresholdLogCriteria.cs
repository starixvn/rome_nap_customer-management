namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using System;
    using Fanex.Data.Repository;

    public class GetAutoSuspendThresholdLogCriteria : CriteriaBase
    {
        public GetAutoSuspendThresholdLogCriteria(
            int sportType,
            DateTime fromDate,
            DateTime toDate)
        {
            SportType = sportType;
            StartLogTime = fromDate;
            EndLogTime = toDate;
        }

        public int SportType { get; }

        public DateTime StartLogTime { get; }

        public DateTime EndLogTime { get; }

        public override string GetSettingKey()
            => "Acc_VR_ViewLog_AutoSuspendThreshold";

        public override bool IsValid()
            => true;
    }
}
