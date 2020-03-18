using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models;

namespace Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor
{
    public class UpdateMaxTurnoverCriteria : EditGlobalSettingCriteria
    {
        public UpdateMaxTurnoverCriteria(UpdateMaxTurnoverParam param, int userId, string ip)
            : base(param, userId, ip)
        {
            MatchId = param.MatchId;
        }

        public int MatchId { get; }

        public override string GetSettingKey()
            => "NAP_TOControl_MatchTurnoverUpdate";
    }
}
