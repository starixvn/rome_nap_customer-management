namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails
{
    using Fanex.Data.Repository;

    public class GetAutoSuspendThresholdCriteria : CriteriaBase
    {
        public GetAutoSuspendThresholdCriteria(int sportType)
        {
            SportType = sportType;
        }

        public int SportType { get; }

        public override string GetSettingKey()
            => "Acc_VR_AutoSuspendThreshold_Sel";

        public override bool IsValid()
            => true;
    }
}
