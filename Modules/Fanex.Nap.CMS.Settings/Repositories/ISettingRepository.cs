namespace Fanex.Nap.CMS.Settings.Repositories
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Settings.Models;

    public interface ISettingRepository
    {
        IEnumerable<BaseSetting> GetSettings(ICriteria criteria);
    }
}
