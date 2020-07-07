using Fanex.Nap.CMS.TurnoverControl.Constants;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting
{
    public class LockGlobalSettingViewModel
    {
        public int CustId { get; set; }

        public bool IsLock { get; set; }

        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public virtual bool IsValid()
            => CustId > 0
            && SportTypeConst.IsValidSportType(SportType);
    }
}
