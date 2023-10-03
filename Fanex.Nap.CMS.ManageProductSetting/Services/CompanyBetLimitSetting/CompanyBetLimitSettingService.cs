namespace Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.ManageProductSetting.Repositories.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using System.Linq;
    using Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting;

    public interface ICompanyBetLimitSettingService
    {
        IEnumerable<IdNameModel> GetCurrencies(bool isCredit);
        IEnumerable<IdNameModel> GetProducts(bool isCredit);
        IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(bool isCredit);
    }

    public class CompanyBetLimitSettingService : ICompanyBetLimitSettingService
    {
        private const string CurrencyCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Currency";

        private const string ProductCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Product";

        private readonly ICompanyBetLimitRepository _repository;

        public CompanyBetLimitSettingService(ICompanyBetLimitRepository repository = null)
        {
            _repository = repository ?? new CompanyBetLimitRepository();
        }

        public IEnumerable<IdNameModel> GetCurrencies(bool isCredit)
        {
            var cacheKey = CurrencyCacheKey + isCredit;
            var currencies = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (currencies != null) return currencies;
            currencies = _repository
                .GetCurrencies(new GetCurrenciesCriteria { IsCredit = isCredit })
                .ToList();
            
            CacheManager.Set(cacheKey, currencies, new TimeSpan(0, 0, 5, 0));

            return currencies;
        }


        public IEnumerable<IdNameModel> GetProducts(bool isCredit)
        {
            var cacheKey = ProductCacheKey + isCredit;
            var products = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (products != null) return products;
            products = _repository
                .GetProducts(new GetProductsCriteria { IsCredit = isCredit })
                .ToList();
            
            CacheManager.Set(cacheKey, products, new TimeSpan(0, 0, 5, 0));

            return products;
        }

        public IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(bool isCredit)
        {
            throw new NotImplementedException();
        }
    }
}