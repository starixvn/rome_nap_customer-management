namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;

    public class GlobalAutoSuspendThresholdLogByGroup
    {
        private const int OneHunded = 100;

        public GlobalAutoSuspendThresholdLogByGroup(
            GlobalAutoSuspendThresholdLogDTO log,
            int groupLevel,
            int groupId,
            string groupName,
            IEnumerable<GlobalAutoSuspendThresholdLogByGroup> childs)
        {
            GroupLevel = groupLevel;
            GroupId = groupId;
            GroupName = groupName;
            NonliveAutoSuspendThreshold = (log.VRAutoSuspendThresholdNonLive_New - 1) * OneHunded;
            LiveAutoSuspendThreshold = (log.VRAutoSuspendThresholdLive_New - 1) * OneHunded;
            NonliveAutoSuspendThresholdOld = (log.VRAutoSuspendThresholdNonLive_Old - 1) * OneHunded;
            LiveAutoSuspendThresholdOld = (log.VRAutoSuspendThresholdLive_Old - 1) * OneHunded;
            Childs = childs;
        }

        public int GroupLevel { get; }

        public int GroupId { get; }

        public string GroupName { get; }

        public decimal NonliveAutoSuspendThreshold { get; }

        public decimal LiveAutoSuspendThreshold { get; }

        public decimal NonliveAutoSuspendThresholdOld { get; }

        public decimal LiveAutoSuspendThresholdOld { get; }

        public IEnumerable<GlobalAutoSuspendThresholdLogByGroup> Childs { get; }
    }
}
