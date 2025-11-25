namespace Fanex.Nap.CMS.TurnoverControl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.TurnoverControl.Constants;
    using Fanex.Nap.CMS.TurnoverControl.Criteria;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;

    public class TurnoverBaseService : ServiceBase
    {
        private const string ProfileCacheKey = "Fanex.Nap.CMS.TurnoverControl.Services.BaseService.Profile-{0}";
        private const string LeagueGroupCacheKey = "Fanex.Nap.CMS.TurnoverControl.Services.BaseService.LeagueGroup-{0}";
        private const int CachedDefaultInMinutes = 60;

        public IEnumerable<IdNameModel> GetTOProfiles(short sportType)
        {
            var cacheKey = GetProfileCacheKey(sportType);
            var profiles = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (profiles == null)
            {
                profiles = DynamicRepository.Fetch<IdNameModel>(new GetProfileCriteria(sportType))
                    .ToList();
                CacheManager.Set(cacheKey, profiles);
            }

            return profiles;
        }


        public IEnumerable<Models.LeagueGroup> GetLeagueGroups(short sportType)
        {
            var cachedKey = GetLeagueGroupCacheKey(sportType);
            var leagues = CacheManager.Get<IEnumerable<Models.LeagueGroup>>(cachedKey);

            if (leagues == null)
            {
                var criteria = new GetLeagueGroupCriteria
                {
                    SportType = sportType,
                };

                leagues = DynamicRepository.Fetch<Models.LeagueGroup>(criteria);
                CacheManager.Set(cachedKey, leagues, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return leagues;
        }

        public IEnumerable<IdNameModel> GetMarkets()
        {
            var markets = new List<IdNameModel>
            {
                new IdNameModel { Id = MarketConst.Malay, Name = "Malay" },
                new IdNameModel { Id = MarketConst._789y, Name = "789Y" },
                new IdNameModel { Id = MarketConst.Others, Name = "Others" },
            };

            return markets;
        }

        private static string GetProfileCacheKey(short sportType)
            => string.Format(ProfileCacheKey, sportType);

        private static string GetLeagueGroupCacheKey(short sportType)
            => string.Format(LeagueGroupCacheKey, sportType);
    }
}
