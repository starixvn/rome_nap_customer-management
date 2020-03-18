using System.Collections.Generic;
using System.Linq;

namespace Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting
{
    public class GlobalSettingModel
    {
        public GlobalSettingModel()
            : this(0, Enumerable.Empty<GlobalSettingCustomer>())
        {
        }

        public GlobalSettingModel(
            int totalItems,
            IEnumerable<GlobalSettingCustomer> globalSettings)
        {
            TotalItems = totalItems;
            GlobalSettings = globalSettings;
        }

        public int TotalItems { get; }

        public IEnumerable<GlobalSettingCustomer> GlobalSettings { get; }
    }
}
