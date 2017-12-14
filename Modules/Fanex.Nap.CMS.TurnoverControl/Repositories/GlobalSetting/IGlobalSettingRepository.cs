using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting
{
    public interface IGlobalSettingRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);

        IEnumerable<GlobalSettingItem> GetList(ICriteria criteria);

        Result Add(ICriteria criteria);

        Result Edit(ICriteria criteria);

        Result Delete(ICriteria criteria);
    }
}
