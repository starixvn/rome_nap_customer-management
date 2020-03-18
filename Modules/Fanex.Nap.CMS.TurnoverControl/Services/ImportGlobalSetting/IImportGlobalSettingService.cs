using System.Collections.Generic;
using System.IO;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting
{
    public interface IImportGlobalSettingService
    {
        ImportGlobalSettingModel ReadExcelStream(short sportType, Stream stream, string fileExtension);

        Result ImportGlobalSettings(short sportType, IEnumerable<GlobalSettingCustomer> globalSettings, int userId, string ip);
    }
}
