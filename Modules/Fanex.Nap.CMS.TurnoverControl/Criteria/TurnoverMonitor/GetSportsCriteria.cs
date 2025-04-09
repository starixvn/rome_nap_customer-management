namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    using Fanex.Data.Repository;

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

        public override bool IsValid()
            => true;
    }
}
