namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;

    public class VirtualRateDetailsByGroup
    {
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

        public IEnumerable<VirtualRateDetailsByGroup> Childs { get; set; }

        public void AssignData(VirtualRateDTO dto, int minRange, int maxRange)
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
        }
    }
}
