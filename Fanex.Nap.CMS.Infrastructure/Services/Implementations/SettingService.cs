namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
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

        private readonly IDynamicRepository dynamicRepository;

        public SettingService() : this(new DynamicRepository())
        {
        }

        internal SettingService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository; 
        }

        public IEnumerable<CurrencyModel> GetAllCurrencies()
        {
            var allCurrencies = CacheManager.Get<IEnumerable<CurrencyModel>>(CachedAllCurrencies);

            if (allCurrencies == null)
            {
                allCurrencies = dynamicRepository.Fetch<CurrencyModel>(new GetCurrencyCriteria(AllCurrency)).OrderBy(cur => cur.Currency);

                CacheManager.Set(CachedAllCurrencies, allCurrencies, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return allCurrencies;
        }

        public IEnumerable<CurrencyModel> GetCreditCurrencies()
            => dynamicRepository.Fetch<CurrencyModel>(new GetCurrencyCriteria(CreditCurrency)).OrderBy(cur => cur.Currency);

        /// <summary>
        /// UserId to check Special_Site permisson.
        /// UserId = 0 means get ALL sites.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<SiteGroup> GetAllSites(int? userId = null)
            => dynamicRepository.Fetch<SiteGroup>(new GetAllSiteCriteria(userId)).OrderBy(site => site.Name);

        public IEnumerable<IdNameModel> GetCreditSites()
            => dynamicRepository.Fetch<SiteGroup>(new GetCreditSiteCriteria()).OrderBy(site => site.Name);
        
        public IEnumerable<IdNameModel> GetNonsportsProducts()
        {
            var products = CacheManager.Get<IEnumerable<IdNameModel>>(CachedNonsportsProducts);

            if (products == null)
            {
                products = dynamicRepository.Fetch<IdNameModel>(new GetNonsportsProductCriteria());

                CacheManager.Set(CachedNonsportsProducts, products, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return products;
        }

        public IEnumerable<SiteGroup> GetSitesMarketCode()
        {
            var sites = CacheManager.Get<IEnumerable<SiteGroup>>(CachedSitesMarketCode);

            if (sites == null)
            {
                sites = dynamicRepository.Fetch<SiteGroup>(new GetSiteMarketCodeCriteria());

                CacheManager.Set(CachedSitesMarketCode, sites, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return sites;
        }

        public IEnumerable<Country> GetCountries()
        {
            var countries = CacheManager.Get<IEnumerable<Country>>(CachedCountries);

            if (countries == null)
            {
                countries = dynamicRepository.Fetch<Country>(new GetCountriesCriteria());

                CacheManager.Set(CachedCountries, countries, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return countries;
        }
    }
}
