namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    using Fanex.Data.Repository;

    public sealed class GetProfileCriteria : CriteriaBase
    {
        public GetProfileCriteria(short sportType)
        {
            SportType = sportType;
        }

        public short SportType { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_ProfileSelect";

        public override bool IsValid()
            => true;
    }
}
