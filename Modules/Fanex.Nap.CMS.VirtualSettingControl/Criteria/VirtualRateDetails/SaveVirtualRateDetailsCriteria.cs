namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class SaveVirtualRateDetailsCriteria : CommandBaseCriteria
    {
        public int CustId { get; set; }

        public int SportType { get; set; }

        public bool IsManualUpdate { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public VirtualRateDetailsReport Rates { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGlobalSetting { get; set; }

        [SpParam(Ignored = true)]
        public bool IsMember { get; set; }

        [SpParam(Ignored = true)]
        public bool IsMemberCompactMode { get; set; }

        public string VirtualMultiplier { get; set; }

        public override string GetSettingKey()
            => "Acc_CustVirtualMultiplier_Upd";

        public override bool IsValid()
            => true;
    }
}
