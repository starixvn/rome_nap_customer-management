namespace Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

    public interface IGlobalSettingRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(CriteriaBase criteria);

        IEnumerable<GlobalSettingItem> GetList(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetProfiles(CriteriaBase criteria);
    }
}
