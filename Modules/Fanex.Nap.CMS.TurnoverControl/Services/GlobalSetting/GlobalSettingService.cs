using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    public class GlobalSettingService : ServiceBase, IGlobalSettingService
    {
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.Currency";
        private const string ProfileCacheKey = "Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.Profile-{0}";
        private const string GlobalSettingSessionKey = "Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.List-{0}";

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = CacheManager.Get<List<IdNameModel>>(CurrencyCacheKey);

            if (currencies == null)
            {
                currencies = DynamicRepository.Fetch<CurrencyModel>(new CurrencyListCriteria())
                    .Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency })
                    .ToList();

                if (currencies.Count > 0)
                {
                    currencies.Insert(0, new IdNameModel { Id = 0, Name = Infrastructure.Resources.Labels.All });
                    CacheManager.Set(CurrencyCacheKey, currencies);
                }
            }

            return currencies;
        }

        public GlobalSettingModel GetList(
            string username,
            short sportType,
            int exchangeId,
            string sortField,
            string sortDirection,
            int page,
            int size,
            bool cache)
        {
            var sessionKey = GetGlobalSettingSessionKey(sportType);
            var globalSettings = GetList(sessionKey, username, sportType, exchangeId, cache);
            var totalItems = globalSettings.Count();

            if (totalItems > 0)
            {
                SessionManager.Set(sessionKey, globalSettings);

                globalSettings = OrderBy(globalSettings, sortField, sortDirection);

                if (size > 0)
                {
                    globalSettings = globalSettings.Skip(page * size).Take(size);
                }
            }

            return new GlobalSettingModel(totalItems, globalSettings);
        }

        private IEnumerable<GlobalSettingCustomer> GetList(
            string sessionKey,
            string username,
            short sportType,
            int exchangeId,
            bool cache)
        {
            IEnumerable<GlobalSettingCustomer> globalSettings = !cache
                ? null
                : SessionManager.Get<List<GlobalSettingCustomer>>(sessionKey);

            if (globalSettings == null)
            {
                var rawData = DynamicRepository.Fetch<GlobalSettingItem>(new GlobalSettingListCriteria(username, sportType, exchangeId));

                globalSettings = rawData
                    .GroupBy(g => new { g.CustId, g.Username, g.Level, g.IsLock, g.Currency })
                    .Select(grp => new GlobalSettingCustomer(
                        grp.Key.CustId,
                        grp.Key.Username,
                        grp.Key.Level,
                        grp.Key.IsLock,
                        grp.Key.Currency,
                        grp.Select(g => new MaxTurnoverItem(g.ProfileId, g.MaxTurnover)).ToList()))
                    .ToList();
            }

            return globalSettings;
        }

        private static IEnumerable<GlobalSettingCustomer> OrderBy(
            IEnumerable<GlobalSettingCustomer> source,
            string propertyName,
            string direction)
        {
            if ("Username" == propertyName || "Level" == propertyName || "Currency" == propertyName)
            {
                var propertyInfo = typeof(GlobalSettingCustomer).GetProperty(propertyName);

                return string.Equals(direction, SortDirectionConstants.Asc, StringComparison.OrdinalIgnoreCase)
                    ? source.OrderBy(s => propertyInfo.GetValue(s, null))
                    : source.OrderByDescending(s => propertyInfo.GetValue(s, null));
            }

            if (propertyName?.StartsWith("Profile") == true)
            {
                var profileId = int.Parse(propertyName.Replace("Profile", string.Empty));

                return string.Equals(direction, SortDirectionConstants.Asc, StringComparison.OrdinalIgnoreCase)
                    ? source.OrderBy(s => s.MaxTurnovers.FirstOrDefault(m => m.ProfileId == profileId).MaxTurnover)
                    : source.OrderByDescending(s => s.MaxTurnovers.FirstOrDefault(m => m.ProfileId == profileId).MaxTurnover);
            }

            return source;
        }

        public IEnumerable<IdNameModel> GetProfiles(short sportType)
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

        public Result Add(string username, MaxTurnoverItem[] maxTurnovers, bool isGroupHeadline, short sportType, int userId, string ip)
        {
            var result = DoAction(
                new AddGlobalSettingCriteria(username, maxTurnovers, isGroupHeadline, sportType, userId, ip));

            if (result.IsSuccess)
            {
                RemoveGlobalSettingSessionValue(sportType);
                result.Message = string.Format(Labels.GlobalSetting_AddMaxTurnover_Success, username);
            }

            return result;
        }

        public Result Edit(UpdateMaxTurnoverParam param, int userId, string ip)
        {
            var result = DoAction(new EditGlobalSettingCriteria(param, userId, ip));

            if (result.IsSuccess)
            {
                RemoveGlobalSettingSessionValue(param.SportType);
                result.Message = Labels.GlobalSetting_EditMaxTurnover_Success;
            }

            return result;
        }

        public Result Delete(int custId, bool isGroupHeadline, short sportType, int userId, string ip)
        {
            var result = DoAction(
                new DeleteGlobalSettingCriteria(custId, isGroupHeadline, sportType, userId, ip));

            if (result.IsSuccess)
            {
                RemoveGlobalSettingSessionValue(sportType);
                result.Message = Labels.GlobalSetting_DeleteMaxTurnover_Success;
            }

            return result;
        }

        public Result Lock(LockSettingParam param, int userId, string ip)
        {
            var result = DoAction(new LockGlobalSettingCriteria(param, userId, ip));

            if (result.IsSuccess)
            {
                RemoveGlobalSettingSessionValue(param.SportType);
            }

            return result;
        }

        private static string GetProfileCacheKey(short sportType)
            => string.Format(ProfileCacheKey, sportType);

        private static void RemoveGlobalSettingSessionValue(short sportType)
            => SessionManager.Set(GetGlobalSettingSessionKey(sportType), null);

        private static string GetGlobalSettingSessionKey(short sportType)
            => string.Format(GlobalSettingSessionKey, sportType);
    }
}
