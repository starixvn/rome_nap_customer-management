namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using Infrastructure.Extensions;

    public class GlobalAutoSuspendThresholdLogDTO
    {
        public GlobalAutoSuspendThresholdLogDTO(
            int logId,
            DateTime logTime,
            int sportType,
            string sportName,
            int leagueGroupId,
            string leagueGroup,
            int betTypeGroupId,
            string betTypeGroup,
            int betType,
            string betTypeName,
            decimal vrAutoSuspendThresholdLive_New,
            decimal vrAutoSuspendThresholdNonLive_New,
            decimal vrAutoSuspendThresholdLive_Old,
            decimal vrAutoSuspendThresholdNonLive_Old,
            bool isNew,
            string updatedby)
        {
            LogId = logId;
            LogTime = logTime;
            SportType = sportType;
            SportName = sportName;
            LeagueGroupId = leagueGroupId;
            LeagueGroup = leagueGroup;
            BetTypeGroupId = betTypeGroupId;
            BetTypeGroup = betTypeGroup;
            BetType = betType;
            BetTypeName = betTypeName;
            VRAutoSuspendThresholdLive_New = vrAutoSuspendThresholdLive_New;
            VRAutoSuspendThresholdNonLive_New = vrAutoSuspendThresholdNonLive_New;
            VRAutoSuspendThresholdLive_Old = vrAutoSuspendThresholdLive_Old;
            VRAutoSuspendThresholdNonLive_Old = vrAutoSuspendThresholdNonLive_Old;
            IsNew = isNew;
            Updatedby = updatedby;
        }

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
