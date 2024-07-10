namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Web;

    internal interface IFilterService : ISettingService
    {
        IEnumerable<short> GetCustomerClasses();

        IEnumerable<LeagueGroup> GetLeagueGroups(int sportType = 0);

        IEnumerable<LeagueGroup> GetLeagueGroups(IEnumerable<int> sportTypes);

        IEnumerable<IdNameModel> GetSports();
    }

    internal class FilterService : SettingService, IFilterService
    {
        private readonly IFilterDataAccess dataAccess;

        private const string CachedSportTypes = "CCM-SportTypes";
        private const string CachedLeagues = "CCM-Leagues-{0}";
        private const int CachedDefaultInMinutes = 60;

        public FilterService(IFilterDataAccess dataAccess = null)
            => this.dataAccess = dataAccess ?? new FilterDataAccess();

        public IEnumerable<short> GetCustomerClasses()
            => dataAccess.GetCustomerClasses(new GetCustomerClassCriteria());

        public IEnumerable<LeagueGroup> GetLeagueGroups(int sportType = 0)
        {
            var cachedKey = string.Format(CachedLeagues, sportType);
            var leagues = CacheManager.Get<IEnumerable<LeagueGroup>>(cachedKey);

            if (leagues == null)
            {
                leagues = dataAccess.GetLeagueGroups(new GetLeagueGroupCriteria(sportType));
                CacheManager.Set(cachedKey, leagues, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return leagues;
        }

        public IEnumerable<LeagueGroup> GetLeagueGroups(IEnumerable<int> sportTypes)
        {
            var leagueGroups = new List<LeagueGroup>();
            foreach (var sportType in sportTypes)
            {
                leagueGroups.AddRange(GetLeagueGroups(sportType));
            }

            return leagueGroups.Distinct();
        }

        public IEnumerable<IdNameModel> GetSports()
        {
            var sports = CacheManager.Get<IEnumerable<IdNameModel>>(CachedSportTypes);

            if (sports == null)
            {
                sports = dataAccess.GetSports(new GetSportsCriteria());
                CacheManager.Set(CachedSportTypes, sports, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return sports;
        }
    }
}
