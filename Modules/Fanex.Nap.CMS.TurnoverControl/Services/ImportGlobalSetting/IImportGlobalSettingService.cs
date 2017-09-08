using System.IO;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Criteria;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting
{
    public interface IImportGlobalSettingService
    {
        ImportGlobalSettingModel ReadExcelStream(SportType sportType, Stream stream, string fileExtension);

        Result ImportGlobalSettings(NonQueryCriteria criteria);
    }
}
