using Fanex.Nap.CMS.TurnoverControl.Constants;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting
{
    public class AddGlobalSettingViewModel
    {
        public string Username { get; set; }

        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public MaxTurnoverItem[] MaxTurnovers { get; set; }

        public bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && SportTypeConst.IsValidSportType(SportType)
            && MaxTurnovers != null && MaxTurnovers.Length > 0;
    }
}
