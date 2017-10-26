using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting
{
    public class ErrorGlobalSettingItem : GlobalSettingItem
    {
        public string MaxTurnoverText { get; set; }

        public string Reason { get; set; }
    }
}
