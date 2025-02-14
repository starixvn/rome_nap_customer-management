namespace Fanex.Nap.CMS.Settings.Repositories
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Settings.Models;

    public class SettingRepository : ISettingRepository
    {
        public IEnumerable<BaseSetting> GetSettings(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<BaseSetting>(criteria);
            }
        }
    }
}
