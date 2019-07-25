namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    public class VirtualRateDTO
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public int LeagueGroupId { get; set; }

        public string LeagueGroupName { get; set; }

        public int BetTypeGroupId { get; set; }

        public string BetTypeGroupName { get; set; }

        public int BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public int Level { get; set; }

        public int NonliveId { get; set; }

        public decimal NonliveValue { get; set; }

        public int LiveId { get; set; }

        public decimal LiveValue { get; set; }

        public string NonliveLog { get; set; }

        public string LiveLog { get; set; }
    }
}
