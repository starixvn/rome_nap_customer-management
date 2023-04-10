namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    public class SettingService : ISettingService
    {
        private const byte AllCurrency = 0;
        private const byte CreditCurrency = 1;

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
    }
}
