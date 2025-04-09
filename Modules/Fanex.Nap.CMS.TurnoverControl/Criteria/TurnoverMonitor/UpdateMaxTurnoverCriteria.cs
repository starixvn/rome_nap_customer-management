using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class UpdateMaxTurnoverCriteria : EditGlobalSettingCriteria
    {
        public int MatchId { get; set; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverUpdate";
    }
}
