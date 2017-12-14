using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;

namespace Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting
{
    public class GlobalSettingService : IGlobalSettingService
    {
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.Currency";

        private const string SoccerSessionKey = "Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.Soccer";
        private const string OtherSportsSessionKey = "Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting.OtherSports";

        private readonly IGlobalSettingRepository _globalSettingRepository;

        public GlobalSettingService(IGlobalSettingRepository globalSettingRepository = null)
        {
            _globalSettingRepository = globalSettingRepository ?? new GlobalSettingRepository();
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = CacheManager.Get<List<IdNameModel>>(CurrencyCacheKey);

            if (currencies == null)
            {
                var rawdata = _globalSettingRepository.GetCurrencies(new CurrencyListCriteria());

                if (rawdata.Any())
                {
                    currencies = new List<IdNameModel>
                    {
                        new IdNameModel { Id = 0, Name = Infrastructure.Resources.Labels.All }
                    };

                    currencies.AddRange(rawdata);
                    CacheManager.Set<List<IdNameModel>>(CurrencyCacheKey, currencies);
                }
            }

            return currencies;
        }

        public GlobalSettingModel GetList(GlobalSettingListCriteria criteria)
        {
            string sessionKey = GetSessionKey((SportType)criteria.SportType);

            IEnumerable<GlobalSettingItem> globalSettings = !criteria.Cache ?
                null :
                (List<GlobalSettingItem>)SessionManager.GetSession(HttpContext.Current.Session, sessionKey);

            if (globalSettings == null)
            {
                globalSettings = _globalSettingRepository.GetList(criteria);
                SessionManager.AddSession(HttpContext.Current.Session, sessionKey, globalSettings);
            }

            int totalItems = globalSettings.Count();

            if (totalItems == 0)
            {
                return new GlobalSettingModel();
            }

            globalSettings = OrderBy(globalSettings, criteria.SortField, criteria.SortDirection);

            if (criteria.Size > 0)
            {
                globalSettings = globalSettings.Skip(criteria.Page * criteria.Size).Take(criteria.Size);
            }
                
            return new GlobalSettingModel(totalItems, globalSettings);
        }

        public Result Add(AddGlobalSettingCriteria criteria)
        {
            var result = _globalSettingRepository.Add(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                RemoveSessionKey((SportType)criteria.SportType);
                result.Message = string.Format(Labels.GlobalSetting_AddMaxTurnover_Success, criteria.UserName);
            }

            return result;
        }

        public Result Edit(EditGlobalSettingCriteria criteria)
        {
            var result = _globalSettingRepository.Edit(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                RemoveSessionKey((SportType)criteria.SportType);
                result.Message = Labels.GlobalSetting_EditMaxTurnover_Success;
            }

            return result;
        }

        public Result Delete(DeleteGlobalSettingCriteria criteria)
        {
            var result = _globalSettingRepository.Delete(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                RemoveSessionKey((SportType)criteria.SportType);
                result.Message = Labels.GlobalSetting_DeleteMaxTurnover_Success;
            }

            return result;
        }

        private static string GetSessionKey(SportType sportType)
        {
            return sportType == SportType.Others ?
                OtherSportsSessionKey :
                SoccerSessionKey;
        }

        private static void RemoveSessionKey(SportType sportType)
        {
            SessionManager.AddSession(
                HttpContext.Current.Session,
                sportType == SportType.Others ? OtherSportsSessionKey : SoccerSessionKey,
                null);
        }

        private static IEnumerable<GlobalSettingItem> OrderBy(
            IEnumerable<GlobalSettingItem> source,
            string propertyName,
            string direction)
        {
            if (propertyName.Equals("UserName", StringComparison.OrdinalIgnoreCase) ||
                propertyName.Equals("Level", StringComparison.OrdinalIgnoreCase) ||
                propertyName.Equals("Currency", StringComparison.OrdinalIgnoreCase) ||
                propertyName.Equals("MaxTurnover", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(direction, SortDirectionConstants.Asc, StringComparison.OrdinalIgnoreCase) ?
                    source.OrderBy(s => s.GetType().GetProperty(propertyName).GetValue(s, null)) :
                    source.OrderByDescending(s => s.GetType().GetProperty(propertyName).GetValue(s, null));
            }

            return source;
        }
    }
}
