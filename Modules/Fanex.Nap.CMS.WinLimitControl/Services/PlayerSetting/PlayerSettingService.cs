namespace Fanex.Nap.CMS.WinLimitControl.Services.PlayerSetting
{
    using System;
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

            var winLimitsGroup = SortPlayerSettings(winLimits, criteria.SortColumnName, criteria.SortDirection)
                .GroupBy(w => new { PlayerId = w.Player.Id, CurrencyId = w.Currency.Id });
            foreach (var group in winLimitsGroup)
            {
                currencyName = currencyHash[group.Key.CurrencyId];
                winLimitsEntity = new List<WinLimitEntity>();
                foreach (var winlimit in group)
                {
                    player = winlimit.Player;
                    winlimit.Game.Name = gameHash[winlimit.Game.Id];
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

        public Result AddPlayerWinLimit(IList<AddPlayerWinLimitCriteria> criterias)
        {
            Result dbresult = null,
                returnResult = null;
            foreach (AddPlayerWinLimitCriteria criteria in criterias)
            {
                dbresult = _playerSettingRepository.AddPlayerWinLimit(criteria);
                if (dbresult.Code == ErrorCodeConstants.NoneError)
                {
                    dbresult.Message = WinLimitControlLabels.AddPlayerWinLimitSuccess;
                }
                else
                {
                    returnResult = dbresult;
                }
            }

            return returnResult ?? dbresult;
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

        private IOrderedEnumerable<PlayerWinLimitEntity> SortPlayerSettings(IList<PlayerWinLimitEntity> sources, string column, string direction)
        {
            IOrderedEnumerable<PlayerWinLimitEntity> destinations;
            if (column.Equals("colUsername", StringComparison.OrdinalIgnoreCase))
            {
                if (direction.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    destinations = sources.OrderBy(s => s.Player.Name);
                }
                else
                {
                    destinations = sources.OrderByDescending(s => s.Player.Name);
                }
            }
            else if (column.Equals("colPlayerWinLimit", StringComparison.OrdinalIgnoreCase))
            {
                if (direction.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    destinations = sources.OrderBy(s => s.PlayerWinLimit);
                }
                else
                {
                    destinations = sources.OrderByDescending(s => s.PlayerWinLimit);
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

            return destinations;
        }
    }
}