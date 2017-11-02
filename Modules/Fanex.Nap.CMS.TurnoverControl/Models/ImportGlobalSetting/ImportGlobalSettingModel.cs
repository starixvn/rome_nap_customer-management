using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting
{
    public class ImportGlobalSettingModel : Result
    {
        public ImportGlobalSettingModel(int code, string message)
            : base(code, message)
        {
        }

        public ImportGlobalSettingModel(
            IEnumerable<GlobalSettingItem> validSettings,
            IEnumerable<ErrorGlobalSettingItem> errorSettings)
            : this(0, null)
        {
            ValidSettings = validSettings;
            ErrorSettings = errorSettings;
        }

        public IEnumerable<GlobalSettingItem> ValidSettings { get; private set; }

        public IEnumerable<ErrorGlobalSettingItem> ErrorSettings { get; private set; }
    }
}
