namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;

    public class VirtualRateThresholdByGroup
    {
        private const int OneHunded = 100;

        public VirtualRateThresholdByGroup()
        {
        }

        public VirtualRateThresholdByGroup(VirtualRateDTO dto, int minRange, int maxRange, int groupLevel)
        {
            LevelId = dto.Level;
            MinRate = minRange;
            MaxRate = maxRange;

            GroupLevel = groupLevel;

            NonliveAutoSuspendThreshold = (dto.NonliveAutoSuspendThreshold - 1) * OneHunded;
            LiveAutoSuspendThreshold = (dto.LiveAutoSuspendThreshold - 1) * OneHunded;
        }

        public int GroupLevel { get; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int LevelId { get; set; }

        public decimal MinRate { get; }

        public decimal MaxRate { get; }

        public bool HasRepetition { get; set; }

        public decimal NonliveAutoSuspendThreshold { get; set; }

        public decimal LiveAutoSuspendThreshold { get; set; }

        public IEnumerable<VirtualRateThresholdByGroup> Childs { get; set; }
    }
}
