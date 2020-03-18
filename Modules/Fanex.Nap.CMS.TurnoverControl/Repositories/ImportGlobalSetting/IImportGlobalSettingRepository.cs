using System;
using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.ImportGlobalSetting
{
    public interface IImportGlobalSettingRepository
    {
        Tuple<IEnumerable<GlobalSettingItem>, IEnumerable<ErrorGlobalSettingExcel>> ValidateGlobalSettingExcels(ICriteria criteria);

        Result ImportGlobalSettings(ICriteria criteria);
    }
}
