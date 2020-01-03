using Fanex.Nap.CMS.TurnoverControl.Constants;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting
{
    public class DeleteGlobalSettingViewModel
    {
        public int CustId { get; set; }

        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public bool IsValid()
            => CustId > 0 && SportTypeConst.IsValidSportType(SportType);
    }
}
