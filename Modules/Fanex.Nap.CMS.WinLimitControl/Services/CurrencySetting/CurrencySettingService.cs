namespace Fanex.Nap.CMS.WinLimitControl.Services.CurrencySetting
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class CurrencySettingService : ICurrencySettingService
    {
        private const int AlreadyExistingGlobalSettingCode = 1;

        private readonly ISettingService _settingService;
        private readonly ICurrencySettingRepository _currencySettingRepository;

        public CurrencySettingService(ISettingService settingService = null, ICurrencySettingRepository currencySettingRepository = null)
        {
            _settingService = settingService ?? new SettingService();
            _currencySettingRepository = currencySettingRepository ?? new CurrencySettingRepository();
        }

        public IList<WinLimitEntity> GetCurrencySettings(ICriteria criteria)
        {
            IList<WinLimitEntity> currencySettings = _currencySettingRepository.GetCurrencySettings(criteria);
            if (currencySettings.Count == 0)
            {
                return currencySettings;
            }

            var gameHash = _settingService.GetGames().ToDictionary(s => s.Id, d => d.Name);
            var currencyHash = _settingService.GetCurrencies().ToDictionary(s => s.Id, d => d.Name);
            foreach (var setting in currencySettings)
            {
                setting.Game.Name = gameHash[setting.Game.Id];
                setting.Currency.Name = gameHash[setting.Currency.Id];
            }

            return currencySettings;
        }

        public IList<BaseSetting> GetExcludedCurrencies()
        {
            var criteria = new GetExcludedCurrencyCriteria();
            return _currencySettingRepository.GetExcludedCurrencies(criteria);
        }

        public IList<BaseSetting> GetExcludedGames()
        {
            var criteria = new GetExcludedGameCriteria();
            return _currencySettingRepository.GetExcludedGames(criteria);
        }

        public IList<BaseSetting> GetUnsetCurrencies(int gameId)
        {
            var criteria = new GetUnsetCurrencyByGameCriteria(gameId);
            return _currencySettingRepository.GetUnsetCurrencies(criteria);
        }

        public IList<BaseSetting> GetUnsetGames(int currencyId)
        {
            var criteria = new GetUnsetGameByCurrencyCriteria(currencyId);
            return _currencySettingRepository.GetUnsetGames(criteria);
        }

        public IList<Result> AddCurrency(BaseSetting currency, IList<AddCurrencyWinLimitCriteria> criterias)
        {
            Result result;
            bool allSuccess = true, success = false;
            string failedGames = string.Empty;

            foreach (AddCurrencyWinLimitCriteria criteria in criterias)
            {
                criteria.CurrencyId = currency.Id;
                result = _currencySettingRepository.AddGlobalWinLimit(criteria);
                if (result.Code == ErrorCodeConstants.NoneError)
                {
                    success = true;
                }
                else
                {
                    allSuccess = false;
                    failedGames += criteria.GameName + ", ";
                }
            }

            var results = new List<Result>();
            if (allSuccess || success)
            {
                results.Add(new Result(ErrorCodeConstants.NoneError, string.Format(WinLimitControlLabels.AddCurrencySuccess, currency.Name)));
            }

            if (failedGames.Length != 0)
            {
                failedGames = failedGames.TrimEnd(new char[] { ',', ' ' });
                results.Add(new Result(AlreadyExistingGlobalSettingCode, string.Format(WinLimitControlLabels.AddCurrencyExistingError, failedGames)));    
            }

            return results;
        }

        public IList<Result> AddGame(BaseSetting game, IList<AddCurrencyWinLimitCriteria> criterias)
        {
            Result result;
            bool allSuccess = true, success = false;
            string failedCurrencies = string.Empty;

            foreach (AddCurrencyWinLimitCriteria criteria in criterias)
            {
                criteria.GameId = game.Id;
                result = _currencySettingRepository.AddGlobalWinLimit(criteria);
                if (result.Code == ErrorCodeConstants.NoneError)
                {
                    success = true;
                }
                else
                {
                    allSuccess = false;
                    failedCurrencies += criteria.CurrencyName + ", ";
                }
            }

            var results = new List<Result>();
            if (allSuccess || success)
            {
                results.Add(new Result(ErrorCodeConstants.NoneError, string.Format(WinLimitControlLabels.AddGameSuccess, game.Name)));
            }

            if (failedCurrencies.Length != 0)
            {
                failedCurrencies = failedCurrencies.TrimEnd(new char[] { ',', ' ' });
                results.Add(new Result(AlreadyExistingGlobalSettingCode, string.Format(WinLimitControlLabels.AddGameExistingError, failedCurrencies)));
            }

            return results;
        }

        public IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria)
        {
            return _currencySettingRepository.GetGreaterPlayerWinLimits(criteria);
        }

        public Result UpdateGlobalWinLimit(UpdateCurrencyWinLimitCriteria criteria)
        {
            var result = _currencySettingRepository.UpdateGlobalWinLimit(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = string.Format(
                    WinLimitControlLabels.EditGlobalWinLimitSuccesss,
                    criteria.CurrencyName,
                    criteria.GameName);
            }

            return result;
        }
    }
}
