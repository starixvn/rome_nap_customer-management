using Fanex.Nap.CMS.TurnoverControl.Constants;

namespace Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting
{
    public class GetListGlobalSettingViewModel
    {
        public string Username { get; set; }

        public short SportType { get; set; }

        public int ExchangeId { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public bool Cache { get; set; }

        public bool IsValid()
            => SportTypeConst.IsValidSportType(SportType)
            && ExchangeId >= 0
            && !string.IsNullOrWhiteSpace(SortField)
            && !string.IsNullOrWhiteSpace(SortDirection);
    }
}
