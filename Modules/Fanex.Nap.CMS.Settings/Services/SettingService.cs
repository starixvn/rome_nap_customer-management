namespace Fanex.Nap.CMS.Settings.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.Settings.Criteria;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.Settings.Repositories;

    public class SettingService : ISettingService
    {
        private const int CacheDurationInMinutes = 60 * 60 * 1; // 1 day.
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.Settings.Currency";
        private const string ProductCacheKey = "Fanex.Nap.CMS.Settings.Product";

        private readonly ISettingRepository _settingRepository;

        public SettingService(ISettingRepository settingRepository = null)
        {
            _settingRepository = settingRepository ?? new SettingRepository();
        }

        public IList<BaseSetting> GetCurrencies()
        {
            ICriteria criteria = new GetCurrencyCriteria();
            return GetSettingsFromCacheOrDatabase(CurrencyCacheKey, criteria);
        }

        public IList<BaseSetting> GetGames()
        {
            ICriteria criteria = new GetGameCriteria();
            return GetSettingsFromCacheOrDatabase(ProductCacheKey, criteria);
        }

        private IList<BaseSetting> GetSettingsFromCacheOrDatabase(string cacheKey, ICriteria criteria)
        {
            var settings = CacheManager.Get<IList<BaseSetting>>(cacheKey);
            if (settings == null)
            {
                settings = _settingRepository.GetSettings(criteria).ToList();
                if (settings.Count != 0)
                {
                    var cacheExpiration = DateTime.Now.AddMinutes(CacheDurationInMinutes);
                    CacheManager.Set<IList<BaseSetting>>(cacheKey, settings, cacheExpiration);    
                }
            }

            return settings;
        }
    }
}
