using Fanex.Nap.CMS.TurnoverControl.Constants;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting
{
    public class EditGlobalSettingViewModel
    {
        public int CustId { get; set; }

        public int ProfileId { get; set; }

        public decimal MaxTurnover { get; set; }

        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public virtual bool IsValid()
            => CustId > 0
            && ProfileId > 0
            && 0 <= MaxTurnover && MaxTurnover <= int.MaxValue
            && SportTypeConst.IsValidSportType(SportType);
    }
}
