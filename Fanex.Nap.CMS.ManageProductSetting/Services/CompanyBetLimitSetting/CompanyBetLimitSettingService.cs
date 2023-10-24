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
    using Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting;

    public interface ICompanyBetLimitSettingService
    {
        IEnumerable<IdNameModel> GetCurrencies(bool isCredit);
        
        IEnumerable<IdNameModel> GetProducts(bool isCredit);
        
        IEnumerable<IdNameModel> GetSites(bool isCredit);

        IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(GetCompanyBetLimitSettingViewModels searchParams);

        IEnumerable<CompanyBetLimitSettingGroups> GetBetLimitSettingGroups(IEnumerable<CompanyBetLimitSetting> betLimitSetting);
    }

    public class CompanyBetLimitSettingService : ICompanyBetLimitSettingService
    {
        private const string CurrencyCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Currency";

        private const string ProductCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Product";

        private const string SiteCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Site";

        private readonly ICompanyBetLimitRepository _repository;

        public CompanyBetLimitSettingService(ICompanyBetLimitRepository repository = null)
        {
            _repository = repository ?? new CompanyBetLimitRepository();
        }

        public IEnumerable<IdNameModel> GetCachedData(string cacheKey, Func<IEnumerable<IdNameModel>> getRepositoryData)
        {
            var data = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (data != null) return data;
            data = getRepositoryData().ToList();

            CacheManager.Set(cacheKey, data, new TimeSpan(0, 0, 0, 5));

            return data;
        }

        public IEnumerable<IdNameModel> GetCurrencies(bool isCredit) =>
            GetCachedData(CurrencyCacheKey + isCredit,
                () => _repository.GetCurrencies(new GetCurrenciesCriteria { IsCredit = isCredit }));

        public IEnumerable<IdNameModel> GetProducts(bool isCredit) => GetCachedData(
            ProductCacheKey + isCredit, () => _repository.GetProducts(new GetProductsCriteria { IsCredit = isCredit }));

        public IEnumerable<IdNameModel> GetSites(bool isCredit) => GetCachedData(SiteCacheKey + isCredit,
            () => _repository.GetSites(new GetSiteCriteria { IsCredit = isCredit }));

        public IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(GetCompanyBetLimitSettingViewModels searchParams)
        {
            if (string.IsNullOrWhiteSpace(searchParams.ProductIds) ||
                string.IsNullOrWhiteSpace(searchParams.Currencies)||
                string.IsNullOrWhiteSpace(searchParams.Sites))
            {
                return new List<CompanyBetLimitSetting>();
            }

            var result = _repository.GetBetLimitSettings(new GetBetLimitSettingCriteria
            {
                IsCredit = searchParams.IsCredit,
                PMutilProductID = searchParams.ProductIds,
                PMutilCurrency = searchParams.Currencies,
                PMutilSite = searchParams.Sites,
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
                            SiteGroups = group2.GroupBy(entry3 => entry3.Site)
                                .Select(group3 => new Models.CompanyBetLimitSetting.SiteGroup
                                {
                                    SiteGroupName = group3.Key,
                                    GameGroups = group3.GroupBy(entry4 => entry4.GameGroup)
                                        .Select(group4 => new GameGroup
                                        {
                                            GameGroupName = group4.Key,
                                            Entries = group3.ToList()
                                        }).ToList()
                                }).ToList()
                        })
                        .ToList()
                })
                .ToList();
        }
    }
}