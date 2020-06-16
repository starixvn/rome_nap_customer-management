namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using Fanex.Data.Repository;

    public class GetBetTypeVirtualRateMappingCriteria : CriteriaBase
    {
        public GetBetTypeVirtualRateMappingCriteria(int sportType)
        {
            SportType = sportType;
        }

        public int SportType { get; }

        public override string GetSettingKey()
            => "Acc_VR_BettypeVirtualRateMapping_Sel";

        public override bool IsValid()
            => SportType > 0;
    }
}
