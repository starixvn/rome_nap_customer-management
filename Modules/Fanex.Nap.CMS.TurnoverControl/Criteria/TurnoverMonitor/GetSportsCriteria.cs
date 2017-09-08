using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class GetSportsCriteria : CriteriaBase
    {
        public byte Mode { get; set; }

        public override string GetSettingKey()
        {
            return "TurnoverControl_TurnoverMonitor_SportList";
        }
    }
}
