namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class SaveAssociatedAccountsCriteria
    {
        public VirtualRateDetailsReport Rates { get; set; }

        public string Accounts { get; set; }

        public int SportType { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public bool IsMember { get; set; }

        public bool IsManualUpdate { get; set; }
    }
}
