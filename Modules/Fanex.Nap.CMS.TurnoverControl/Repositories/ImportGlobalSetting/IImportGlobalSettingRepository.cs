using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.ImportGlobalSetting
{
    public interface IImportGlobalSettingRepository
    {
        ImportGlobalSettingModel ValidateGlobalSettingExcels(ICriteria criteria);

        Result ImportGlobalSettings(ICriteria criteria);
    }
}
