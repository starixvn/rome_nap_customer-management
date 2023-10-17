namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class SettingService : ISettingService
    {
        private const byte AllCurrency = 0;
        private const byte CreditCurrency = 1;
        private const string CachedSitesMarketCode = "Fanex.Nap.CMS.Infrastructure.SitesMarketCode";
        private const int CachedDefaultInMinutes = 30;
        private const string CachedCountries = "Fanex.Nap.CMS.Infrastructure.Countries";

        private readonly ISettingRepository settingRepository;

        public SettingService() : this(new SettingRepository())
        {
        }

        internal SettingService(ISettingRepository settingRepository)
            => this.settingRepository = settingRepository;

        public IEnumerable<IdNameModel> GetAllCurrencies()
            => settingRepository.GetCurrencies(new GetCurrencyCriteria(AllCurrency));

        public IEnumerable<IdNameModel> GetCreditCurrencies()
            => settingRepository.GetCurrencies(new GetCurrencyCriteria(CreditCurrency));

        /// <summary>
        /// UserId to check Special_Site permisson.
        /// UserId = 0 means get ALL sites.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SiteGroup> GetAllSites(int userId = 0)
            => settingRepository.GetAllSites(new GetAllSiteCriteria(userId));

        public IEnumerable<IdNameModel> GetCreditSites()
            => settingRepository.GetCreditSites(new GetCreditSiteCriteria());

        public IEnumerable<SiteGroup> GetSitesMarketCode()
        {
            var sites = CacheManager.Get<IEnumerable<SiteGroup>>(CachedSitesMarketCode);

            if (sites == null)
            {
                sites = settingRepository.GetSitesMarketCode(new GetSiteMarketCodeCriteria());

                CacheManager.Set(CachedSitesMarketCode, sites, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return sites;
        }

        public IEnumerable<Country> GetCountries()
        {
            var countries = CacheManager.Get<IEnumerable<Country>>(CachedCountries);

            if (countries == null)
            {
                countries = settingRepository.GetCountries(new GetCountriesCriteria());

                CacheManager.Set(CachedCountries, countries, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return countries;
        }
    }
}
