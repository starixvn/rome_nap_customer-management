namespace Fanex.Nap.CMS.WinLimitControl.Services.PlayerSetting
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class PlayerSettingService : IPlayerSettingService
    {
        private readonly ISettingService _settingService;
        private readonly IPlayerSettingRepository _playerSettingRepository;

        public PlayerSettingService(ISettingService settingService = null, IPlayerSettingRepository playerSettingRepository = null)
        {
            _settingService = settingService ?? new SettingService();
            _playerSettingRepository = playerSettingRepository ?? new PlayerSettingRepository();
        }

        public IList<dynamic> CustomerAutoSuggestion(ICriteria criteria)
        {
            return _playerSettingRepository.CustomerAutoSuggestion(criteria);
        }
        
        public IList<PlayerWinLimit> GetPlayerSettings(GetPlayerWinLimitCriteria criteria)
        {
            var playerWinLimits = new List<PlayerWinLimit>();

            var winLimits = _playerSettingRepository.GetPlayerSettings(criteria);
            if (winLimits.Count == 0)
            {
                return playerWinLimits;
            }

            string currencyName;
            BaseSetting player = null;
            IList<WinLimitEntity> winLimitsEntity;
            var currencyHash = _settingService.GetCurrencies().ToDictionary(s => s.Id, d => d.Name);
            var gameHash = _settingService.GetGames().ToDictionary(s => s.Id, d => d.Name);

            var winLimitsGroup = winLimits.GroupBy(w => new { PlayerId = w.Player.Id, CurrencyId = w.Currency.Id });
            foreach (var group in winLimitsGroup)
            {
                currencyName = currencyHash.ContainsKey(group.Key.CurrencyId) ? currencyHash[group.Key.CurrencyId] : group.Key.CurrencyId.ToString();
                winLimitsEntity = new List<WinLimitEntity>();
                foreach (var winlimit in group)
                {
                    player = winlimit.Player;
                    winlimit.Game.Name = gameHash.ContainsKey(winlimit.Game.Id) ? gameHash[winlimit.Game.Id] : winlimit.Game.Id.ToString();
                    winLimitsEntity.Add(winlimit);
                }

                playerWinLimits.Add(new PlayerWinLimit { Player = player, Currency = currencyName, WinLimits = winLimitsEntity });
            }

            return playerWinLimits;
        }

        public IList<UndefinedPlayerWinLimit> GetUndefinedGameByPlayer(GetUndefinedGameByPlayerCriteria criteria)
        {
            return _playerSettingRepository.GetUndefinedGameByPlayer(criteria);
        }

        public Result AddPlayerWinLimit(AddPlayerWinLimitCriteria criteria)
        {
            Result result = _playerSettingRepository.AddPlayerWinLimit(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = WinLimitControlLabels.AddPlayerWinLimitSuccess;
            }

            return result;
        }

        public Result UpdatePlayerWinLimit(UpdatePlayerWinLimitCriteria criteria)
        {
            var result = _playerSettingRepository.UpdatePlayerWinLimit(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = WinLimitControlLabels.AddPlayerWinLimitSuccess;
            }

            return result;
        }

        public Result DeletePlayerWinLimit(ICriteria criteria)
        {
            return _playerSettingRepository.DeletePlayerWinLimit(criteria);
        }
    }
}