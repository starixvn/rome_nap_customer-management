namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using Infrastructure.Extensions;

    public class GlobalAutoSuspendThresholdLogDTO
    {
        public int LogId { get; set; }

        public DateTime LogTime { get; set; }

        public int SportType { get; set; }

        public string SportName { get; set; }

        public int LeagueGroupId { get; set; }

        public string LeagueGroup { get; set; }

        public int BetTypeGroupId { get; set; }

        public string BetTypeGroup { get; set; }

        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public decimal VRAutoSuspendThresholdLive_New { get; set; }

        public decimal VRAutoSuspendThresholdNonLive_New { get; set; }

        public decimal VRAutoSuspendThresholdLive_Old { get; set; }

        public decimal VRAutoSuspendThresholdNonLive_Old { get; set; }

        public bool IsNew { get; set; }

        public string Updatedby { get; set; }

        public string LogTimeString => LogTime.ToShortFormatedDateWithHourMinute();
    }
}
