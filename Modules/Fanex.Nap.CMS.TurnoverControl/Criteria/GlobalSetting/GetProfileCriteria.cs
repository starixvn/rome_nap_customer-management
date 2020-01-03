using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting
{
    public sealed class GetProfileCriteria : CriteriaBase
    {
        public GetProfileCriteria(short sportType)
        {
            SportType = sportType;
        }

        public short SportType { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_ProfileSelect";
    }
}
