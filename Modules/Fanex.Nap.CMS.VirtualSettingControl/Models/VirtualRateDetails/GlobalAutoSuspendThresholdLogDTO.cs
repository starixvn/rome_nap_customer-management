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

        public int LogId { get; }

        public DateTime LogTime { get; }

        public int SportType { get; }

        public string SportName { get; }

        public int LeagueGroupId { get; }

        public string LeagueGroup { get; }

        public int BetTypeGroupId { get; }

        public string BetTypeGroup { get; }

        public int BetType { get; }

        public string BetTypeName { get; }

        public decimal VRAutoSuspendThresholdLive_New { get; }

        public decimal VRAutoSuspendThresholdNonLive_New { get; }

        public decimal VRAutoSuspendThresholdLive_Old { get; }

        public decimal VRAutoSuspendThresholdNonLive_Old { get; }

        public bool IsNew { get; }

        public string Updatedby { get; }

        public string LogTimeString => LogTime.ToShortFormatedDateWithHourMinute();
    }
}
