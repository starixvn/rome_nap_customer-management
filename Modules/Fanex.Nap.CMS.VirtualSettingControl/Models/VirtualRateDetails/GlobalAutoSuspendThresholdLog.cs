namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Extensions;

    public class GlobalAutoSuspendThresholdLog
    {
        public GlobalAutoSuspendThresholdLog(
            int logId,
            DateTime logTime,
            string updatedBy,
            IEnumerable<GlobalAutoSuspendThresholdLogByGroup> sports)
        {
            LogId = logId;
            LogTime = logTime;
            Updatedby = updatedBy;
            Sports = sports;
        }

        public int LogId { get; }

        public DateTime LogTime { get; }

        public string LogTimeString => LogTime.ToShortFormatedDateWithHourMinute();

        public string Updatedby { get; }

        public IEnumerable<GlobalAutoSuspendThresholdLogByGroup> Sports { get; }
    }
}
