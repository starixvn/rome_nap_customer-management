namespace Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.ManageProductSetting.Repositories.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using System.Linq;
    using Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;

    public interface ICompanyBetLimitSettingService
    {
        IEnumerable<IdNameModel> GetCurrencies(bool isCredit);
        IEnumerable<IdNameModel> GetProducts(bool isCredit);
        IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(bool isCredit, string productIds, string currencies);
        IEnumerable<CompanyBetLimitSettingGroups> GetBetLimitSettingGroups(IEnumerable<CompanyBetLimitSetting> betLimitSetting);
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
            currencies = _repository.GetCurrencies(new GetCurrenciesCriteria { IsCredit = isCredit }).ToList();

            CacheManager.Set(cacheKey, currencies, new TimeSpan(0, 0, 5, 0));

            return currencies;
        }
        
        public IEnumerable<IdNameModel> GetProducts(bool isCredit)
        {
            var cacheKey = ProductCacheKey + isCredit;
            var products = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (products != null) return products;
            products = _repository.GetProducts(new GetProductsCriteria { IsCredit = isCredit }).ToList();

            CacheManager.Set(cacheKey, products, new TimeSpan(0, 0, 5, 0));

            return products;
        }

        public IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(bool isCredit, string productIds,
            string currencies)
        {
            if (string.IsNullOrWhiteSpace(productIds) || string.IsNullOrWhiteSpace(currencies))
            {
                return new List<CompanyBetLimitSetting> { };
            }

            var result = _repository.GetBetLimitSettings(new GetBetLimitSettingCriteria
            {
                IsCredit = isCredit,
                PMutilProductID = productIds,
                PMutilCurrency = currencies
            });
            
            return result;
        }

        public IEnumerable<CompanyBetLimitSettingGroups> GetBetLimitSettingGroups(
            IEnumerable<CompanyBetLimitSetting> data)
        {
            return data
                .OrderBy(entry => entry.SportType)
                .GroupBy(entry1 => entry1.SportType)
                .Select(group1 => new CompanyBetLimitSettingGroups()
                {
                    SportType = group1.Key,
                    SportName = group1.First().SportName,
                    CurrencyGroups = group1.GroupBy(entry2 => entry2.CurrencyGroupID)
                        .Select(group2 => new CurrencyGroup
                        {
                            CurrencyGroupID = group2.Key,
                            Currency = string.Join(", ", group2.Select(e => e.Currency).Distinct()),
                            GameGroups = group2.GroupBy(entry3 => entry3.GameGroup)
                                .Select(group3 => new GameGroup
                                {
                                    GameGroupName = group3.Key,
                                    Entries = group3.ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();
        }
    }
}