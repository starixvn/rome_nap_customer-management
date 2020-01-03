using Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.TurnoverMonitor
{
    public class UpdateMaxTurnoverViewModel : EditGlobalSettingViewModel
    {
        public int MatchId { get; set; }

        public override bool IsValid()
            => base.IsValid() && MatchId > 0;
    }
}
