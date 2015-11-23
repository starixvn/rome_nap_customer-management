namespace Fanex.Nap.CMS.WinLimitControl.Services.CurrencySetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class CurrencySettingService : ICurrencySettingService
    {
        private readonly ICurrencySettingRepository _currencySettingRepository;

        public CurrencySettingService(ICurrencySettingRepository currencySettingRepository = null)
        {
            _currencySettingRepository = currencySettingRepository ?? new CurrencySettingRepository();
        }

        public IList<WinLimitEntity> GetCurrencySettings(GetCurrencyWinLimitCriteria criteria)
        {
            var currencySettings = _currencySettingRepository.GetCurrencySettings(criteria);
            return SortCurrencySettings(currencySettings, criteria.SortColumnName ?? "colCurrency", criteria.SortDirection ?? "asc");
        }

        public Result AddCurrencySetting(AddCurrencyWinLimitCriteria criteria)
        {
            var result = _currencySettingRepository.AddCurrencySetting(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = string.Format(
                    WinLimitControlLabels.EditGlobalWinLimitSuccesss,
                    criteria.WinLimit.Currency.Name,
                    criteria.WinLimit.Game.Name);
            }

            return result;
        }

        public Result UpdateCurrencySetting(UpdateCurrencyWinLimitCriteria criteria)
        {
            var result = _currencySettingRepository.UpdateCurrencySetting(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                UpdateRelatedPlayerWinLimitCriteria updateRelatedPlayerCriteria = new UpdateRelatedPlayerWinLimitCriteria()
                {
                    WinLimit = criteria.WinLimit,
                    UserId = criteria.UserId,
                    IP = criteria.IP,
                };

                result = _currencySettingRepository.UpdateRelatedPlayerSetting(updateRelatedPlayerCriteria);
                if (result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = string.Format(
                        WinLimitControlLabels.EditGlobalWinLimitSuccesss,
                        criteria.WinLimit.Currency.Name,
                        criteria.WinLimit.Game.Name);
                }
            }

            return result;
        }

        public IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria)
        {
            return _currencySettingRepository.GetGreaterPlayerWinLimits(criteria);
        }

        private IList<WinLimitEntity> SortCurrencySettings(IList<WinLimitEntity> sources, string column, string direction)
        {
            IOrderedEnumerable<WinLimitEntity> destinations;
            if (column.Equals("colCurrency", StringComparison.OrdinalIgnoreCase))
            {
                if (direction.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    destinations = sources.OrderBy(s => s.Currency.Name);    
                }
                else
                {
                    destinations = sources.OrderByDescending(s => s.Currency.Name);    
                }
            }
            else
            {
                if (direction.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    destinations = sources.OrderBy(s => s.MaxWinLimit);
                }
                else
                {
                    destinations = sources.OrderByDescending(s => s.MaxWinLimit);
                }
            }

            return destinations.ToList();
        }
    }
}
