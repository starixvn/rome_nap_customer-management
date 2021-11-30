namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;

    public class VirtualRateDetailsByGroup
    {
        public VirtualRateDetailsByGroup()
        {
        }

        public VirtualRateDetailsByGroup(VirtualRateDTO dto, int minRange, int maxRange, int groupLevel)
        {
            LevelId = dto.Level;
            NonliveId = dto.NonliveId;
            NonLiveValue = (dto.NonliveValue - 1) * 100;
            LiveId = dto.LiveId;
            LiveValue = (dto.LiveValue - 1) * 100;
            MinRate = minRange;
            MaxRate = maxRange;

            LiveLog = dto.LiveLog;
            NonliveLog = dto.NonliveLog;
            GroupLevel = groupLevel;

            DisableNonliveAutoSuspend = dto.DisableNonliveAutoSuspend;
            DisableLiveAutoSuspend = dto.DisableLiveAutoSuspend;
            NonliveAutoSuspend = dto.NonliveAutoSuspend;
            LiveAutoSuspend = dto.LiveAutoSuspend;
            NonliveAutoSuspendThreshold = (dto.NonliveAutoSuspendThreshold - 1) * 100;
            LiveAutoSuspendThreshold = (dto.LiveAutoSuspendThreshold - 1) * 100;

            IsOverrideASLive = dto.IsOverrideASLive;
            IsOverrideASNonlive = dto.IsOverrideASNonlive;
        }

        public VirtualRateDetailsByGroup(LogDTO dto, int groupLevel)
        {
            LevelId = dto.Level;
            GroupLevel = groupLevel;

            LiveLog = dto.LiveLog;
            NonliveLog = dto.NonliveLog;

            NewNonliveAutoSuspend = dto.NewNonliveAutoSuspend;
            NewLiveAutoSuspend = dto.NewLiveAutoSuspend;
            NonliveAutoSuspend = dto.NonliveAutoSuspend;
            LiveAutoSuspend = dto.LiveAutoSuspend;
        }

        public int GroupLevel { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int LevelId { get; set; }

        public int NonliveId { get; set; }

        public decimal NonLiveValue { get; set; }

        public int LiveId { get; set; }

        public decimal LiveValue { get; set; }

        public decimal MinRate { get; set; }

        public decimal MaxRate { get; set; }

        public bool HasRepetition { get; set; }

        public string LiveLog { get; set; }

        public string NonliveLog { get; set; }

        public string NewLiveLog { get; set; }

        public string NewNonliveLog { get; set; }

        public bool NonliveAutoSuspend { get; set; }

        public bool LiveAutoSuspend { get; set; }

        public bool DisableNonliveAutoSuspend { get; set; }

        public bool DisableLiveAutoSuspend { get; set; }

        public bool? NewNonliveAutoSuspend { get; set; }

        public bool? NewLiveAutoSuspend { get; set; }

        public decimal NonliveAutoSuspendThreshold { get; set; }

        public decimal LiveAutoSuspendThreshold { get; set; }

        public bool IsOverrideASLive { get; set; }

        public bool IsOverrideASNonlive { get; set; }

        public IEnumerable<VirtualRateDetailsByGroup> Childs { get; set; }
    }
}
