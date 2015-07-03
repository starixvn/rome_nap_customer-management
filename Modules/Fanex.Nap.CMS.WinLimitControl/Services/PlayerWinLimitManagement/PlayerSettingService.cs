namespace Fanex.Nap.CMS.WinLimitControl.Services.PlayerWinLimitManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement;
    using Fanex.Nap.CMS.WinLimitControl.Models.GlobalSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerWinLimitManagement;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class PlayerSettingService : IPlayerSettingService
    {
        private const int InvalidUsernameErrorCode = 3;
        private const int UnsetGlobalWinLimitErrorCode = 4;

        private readonly ISettingService _settingService;
        private readonly IPlayerSettingRepository _playerSettingRepository;

        public PlayerSettingService(ISettingService settingService = null, IPlayerSettingRepository playerSettingRepository = null)
        {
            _settingService = settingService ?? new SettingService();
            _playerSettingRepository = playerSettingRepository ?? new PlayerSettingRepository();
        }

        public IList<PlayerWinLimit> GetPlayerWinLimits(ICriteria criteria)
        {
            var playerWinLimits = new List<PlayerWinLimit>();

            var winLimits = _playerSettingRepository.GetPlayerWinLimits(criteria);
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

        public IList<GameWinLimit> GetPlayerWinLimitsForUpdate(GetPlayerWinLimitForUpdateCriteria criteria)
        {
            var winlimits = _playerSettingRepository.GetPlayerWinLimitsForUpdate(criteria);
            if (winlimits.Count > 0)
            {
                var gameHash = _settingService.GetGames().ToDictionary(s => s.Id, d => d.Name);
                foreach (var winlimit in winlimits)
                {
                    winlimit.Name = gameHash[winlimit.Id];    
                }
            }

            return winlimits;
        }

        public AddOrUpdatePlayerWinLimit GetUnsetGameByPlayer(GetUnsetGameByPlayerCriteria criteria)
        {
            var unsetGames = _playerSettingRepository.GetUnsetGameByPlayer(criteria);
            if (unsetGames.DbResult.Code == ErrorCodeConstants.NoneError)
            {
                unsetGames.Player.Name = criteria.Username;

                var gameHash = _settingService.GetGames().ToDictionary(s => s.Id, d => d.Name);
                foreach (var game in unsetGames.WinLimits)
                {
                    game.Name = gameHash[game.Id];
                    game.PlayerWinLimit = game.GlobalWinLimit;
                }    
            }
            else if (unsetGames.DbResult.Code == InvalidUsernameErrorCode)
            {
                unsetGames.DbResult.Message = WinLimitControlLabels.InvalidUsername;
            }
            else if (unsetGames.DbResult.Code == UnsetGlobalWinLimitErrorCode)
            {
                unsetGames.DbResult.Message = string.Format(
                    WinLimitControlLabels.UnsetGlobalWinLimitError,
                    unsetGames.Currency,
                    "../CurrencySetting/Index");
            }
            
            return unsetGames;
        }

        public Result AddPlayerWinLimit(IList<AddPlayerWinLimitCriteria> criterias)
        {
            Result dbresult = null, returnResult = null;
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

        public Result UpdatePlayerWinLimit(IList<UpdatePlayerWinLimitCriteria> criterias)
        {
            Result dbresult = null, returnResult = null;
            foreach (UpdatePlayerWinLimitCriteria criteria in criterias)
            {
                dbresult = _playerSettingRepository.UpdatePlayerWinLimit(criteria);
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

        public Result DeletePlayerWinLimit(ICriteria criteria)
        {
            return _playerSettingRepository.DeletePlayerWinLimit(criteria);
        }
    }
}