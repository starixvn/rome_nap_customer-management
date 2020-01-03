using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetSportsCriteria : CriteriaBase
    {
        public GetSportsCriteria(byte mode, short sportType)
        {
            Mode = mode;
            SportType = sportType;
        }

        public byte Mode { get; }

        public short SportType { get; }

        public override string GetSettingKey()
            => "TurnoverControl_TurnoverMonitor_SportList";
    }
}
