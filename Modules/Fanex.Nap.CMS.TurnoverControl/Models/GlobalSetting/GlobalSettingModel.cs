using System.Collections.Generic;
using System.Linq;

namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    public class GlobalSettingModel
    {
        public GlobalSettingModel()
            : this(0, Enumerable.Empty<GlobalSettingItem>())
        {
        }

        public GlobalSettingModel(
            int totalItems,
            IEnumerable<GlobalSettingItem> globalSettings)
        {
            TotalItems = totalItems;
            GlobalSettings = globalSettings;
        }

        public int TotalItems { get; private set; }

        public IEnumerable<GlobalSettingItem> GlobalSettings { get; private set; }
    }
}
