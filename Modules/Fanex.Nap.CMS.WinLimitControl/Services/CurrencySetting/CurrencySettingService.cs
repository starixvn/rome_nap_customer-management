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
        private readonly ICurrencySettingRepository _globalSettingRepository;

        public CurrencySettingService(ISettingService settingService = null, ICurrencySettingRepository globalSettingRepository = null)
        {
            _settingService = settingService ?? new SettingService();
            _globalSettingRepository = globalSettingRepository ?? new CurrencySettingRepository();
        }

        public IList<CurrencyWinLimit> GetGlobalWinLimits(ICriteria criteria)
        {
            IList<CurrencyWinLimit> globalWinLimits = new List<CurrencyWinLimit>();

            var winLimits = _globalSettingRepository.GetGlobalWinLimits(criteria);
            if (winLimits.Count == 0)
            {
                return globalWinLimits;
            }

            string currencyName;
            IList<WinLimitEntity> winLimitsEntity;
            var currencyHash = _settingService.GetCurrencies().ToDictionary(s => s.Id, d => d.Name);
            var gameHash = _settingService.GetGames().ToDictionary(s => s.Id, d => d.Name);

            var winLimitsGroup = winLimits.GroupBy(w => w.Currency.Id);
            foreach (var group in winLimitsGroup)
            {
                currencyName = currencyHash[group.Key];
                winLimitsEntity = new List<WinLimitEntity>();
                foreach (var winlimit in group)
                {
                    winlimit.Game.Name = gameHash[winlimit.Game.Id];
                    winLimitsEntity.Add(winlimit);
                }

                globalWinLimits.Add(new CurrencyWinLimit { Currency = currencyName, WinLimits = winLimitsEntity });
            }

            return globalWinLimits;
        }

        public IList<BaseSetting> GetExcludedCurrencies()
        {
            var criteria = new GetExcludedCurrencyCriteria();
            return _globalSettingRepository.GetExcludedCurrencies(criteria);
        }

        public IList<BaseSetting> GetExcludedGames()
        {
            var criteria = new GetExcludedGameCriteria();
            return _globalSettingRepository.GetExcludedGames(criteria);
        }

        public IList<BaseSetting> GetUnsetCurrencies(int gameId)
        {
            var criteria = new GetUnsetCurrencyByGameCriteria(gameId);
            return _globalSettingRepository.GetUnsetCurrencies(criteria);
        }

        public IList<BaseSetting> GetUnsetGames(int currencyId)
        {
            var criteria = new GetUnsetGameByCurrencyCriteria(currencyId);
            return _globalSettingRepository.GetUnsetGames(criteria);
        }

        public IList<Result> AddCurrency(BaseSetting currency, IList<AddCurrencyWinLimitCriteria> criterias)
        {
            Result result;
            bool allSuccess = true, success = false;
            string failedGames = string.Empty;

            foreach (AddCurrencyWinLimitCriteria criteria in criterias)
            {
                criteria.CurrencyId = currency.Id;
                result = _globalSettingRepository.AddGlobalWinLimit(criteria);
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
                result = _globalSettingRepository.AddGlobalWinLimit(criteria);
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
            return _globalSettingRepository.GetGreaterPlayerWinLimits(criteria);
        }

        public Result UpdateGlobalWinLimit(UpdateCurrencyWinLimitCriteria criteria)
        {
            var result = _globalSettingRepository.UpdateGlobalWinLimit(criteria);
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
