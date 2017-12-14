using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    public interface IGlobalSettingService
    {
        IEnumerable<IdNameModel> GetCurrencies();

        GlobalSettingModel GetList(GlobalSettingListCriteria criteria);

        Result Add(AddGlobalSettingCriteria criteria);

        Result Edit(EditGlobalSettingCriteria criteria);

        Result Delete(DeleteGlobalSettingCriteria criteria);
    }
}
