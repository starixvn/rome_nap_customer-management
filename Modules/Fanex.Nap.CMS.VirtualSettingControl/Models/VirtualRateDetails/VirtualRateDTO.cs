namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    public class VirtualRateDTO
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public int LeagueGroupId { get; set; } = -1;

        public string LeagueGroupName { get; set; }

        public int BetTypeGroupId { get; set; } = -1;

        public string BetTypeGroupName { get; set; }

        public int BetTypeId { get; set; } = -1;

        public string BetTypeName { get; set; }

        public int Level { get; set; }

        public int NonliveId { get; set; }

        public decimal NonliveValue { get; set; } = -1;

        public int LiveId { get; set; }

        public decimal LiveValue { get; set; } = -1;

        public string NonliveLog { get; set; }

        public string Nonlive { get; set; }

        public string LiveLog { get; set; }

        public string Live { get; set; }

        public bool DisableNonliveAutoSuspend { get; set; }

        public bool DisableLiveAutoSuspend { get; set; }

        public bool NewNonliveAutoSuspend { get; set; }

        public bool NewLiveAutoSuspend { get; set; }

        public bool NonliveAutoSuspend { get; set; }

        public bool LiveAutoSuspend { get; set; }

        public decimal NonliveAutoSuspendThreshold { get; set; } = -1;

        public decimal LiveAutoSuspendThreshold { get; set; } = -1;

        public bool IsOverrideASLive { get; set; }

        public bool IsOverrideASNonlive { get; set; }

        public bool IsOverrideASLiveNew { get; set; }

        public bool IsOverrideASNonliveNew { get; set; }
    }
}
