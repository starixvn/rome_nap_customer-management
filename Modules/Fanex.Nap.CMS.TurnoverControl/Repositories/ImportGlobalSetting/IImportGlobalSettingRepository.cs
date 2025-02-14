using System;
using System.Collections.Generic;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Infrastructure.Criteria;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.ImportGlobalSetting
{
    public interface IImportGlobalSettingRepository
    {
        Tuple<IEnumerable<GlobalSettingItem>, IEnumerable<ErrorGlobalSettingExcel>> ValidateGlobalSettingExcels(CriteriaBase criteria);

        Result ImportGlobalSettings(CommandBaseCriteria criteria);
    }
}
