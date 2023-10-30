namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private const string CachedAllCurrencies = "Fanex.Nap.CMS.Infrastructure.All.Currencies";
        private const string CachedNonsportsProducts = "Fanex.Nap.CMS.Infrastructure.NonsportsProducts";

        private readonly ISettingRepository settingRepository;

        public SettingService() : this(new SettingRepository())
        {
        }

        internal SettingService(ISettingRepository settingRepository)
            => this.settingRepository = settingRepository;

        public IEnumerable<IdNameModel> GetAllCurrencies()
        {
            var allCurrencies = CacheManager.Get<IEnumerable<IdNameModel>>(CachedAllCurrencies);

            if (allCurrencies == null)
            {
                allCurrencies = settingRepository.GetCurrencies(new GetCurrencyCriteria(AllCurrency)).OrderBy(cur => cur.Name);

                CacheManager.Set(CachedAllCurrencies, allCurrencies, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return allCurrencies;
        }

        public IEnumerable<IdNameModel> GetCreditCurrencies()
            => settingRepository.GetCurrencies(new GetCurrencyCriteria(CreditCurrency)).OrderBy(cur => cur.Name);

        /// <summary>
        /// UserId to check Special_Site permisson.
        /// UserId = 0 means get ALL sites.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SiteGroup> GetAllSites(int userId = 0)
            => settingRepository.GetAllSites(new GetAllSiteCriteria(userId)).OrderBy(site => site.Name);

        public IEnumerable<IdNameModel> GetCreditSites()
            => settingRepository.GetCreditSites(new GetCreditSiteCriteria()).OrderBy(site => site.Name);
        
        public IEnumerable<IdNameModel> GetNonsportsProducts()
        {
            var products = CacheManager.Get<IEnumerable<IdNameModel>>(CachedNonsportsProducts);

            if (products == null)
            {
                products = settingRepository.GetNonsportsProducts(new GetNonsportsProductCriteria());

                CacheManager.Set(CachedNonsportsProducts, products, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return products;
        }

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
