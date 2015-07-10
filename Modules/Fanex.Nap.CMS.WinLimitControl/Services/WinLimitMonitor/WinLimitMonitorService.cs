namespace Fanex.Nap.CMS.WinLimitControl.Services.WinLimitMonitor
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.FPSClient.Services;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Services;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Resources;
    using Fanex.Oneyed.BussinessRule.Contracts;

    public class WinLimitMonitorService : IWinLimitMonitorService
    {
        private readonly ISettingService _settingService;
        private readonly IFPSClientService _fpsClient;
        private readonly IWinLimitMonitorRepository _winLimitMonitorRepository;

        public WinLimitMonitorService(
            ISettingService settingService = null, 
            IWinLimitMonitorRepository winLimitMonitorRepository = null,
            IFPSClientService fpsClient = null)
        {
            _settingService = settingService ?? new SettingService();
            _winLimitMonitorRepository = winLimitMonitorRepository ?? new WinLimitMonitorRepository();
            _fpsClient = fpsClient ?? new FPSClientService();
        }

        public PlayerWinLimitMonitorView GetWinLimitMonitor(ICriteria criteria)
        {
            var winLimits = _winLimitMonitorRepository.GetWinLimitMonitor(criteria);
            var playerWinLimitMonitors = new PlayerWinLimitMonitorView
            {
                PlayerWinLimitMonitorModels = new List<PlayerWinLimitMonitorModel>(),
                TotalPlayers = winLimits.TotalPlayers
            };

            if (winLimits.WinLimitMonitorEntities.Count == 0)
            {
                return playerWinLimitMonitors;
            }

            var currencyHash = _settingService.GetCurrencies().ToDictionary(s => s.Id, d => d.Name);
            var gameHash = _settingService.GetGames().ToDictionary(s => s.Id, d => d.Name);
            var winLimitsGroup = winLimits.WinLimitMonitorEntities.GroupBy(w => new { PlayerId = w.PlayerInfo.Id, CurrencyId = w.Currency.Id });
            foreach (var group in winLimitsGroup)
            {
                var playerWinLimitMonitorModel = new PlayerWinLimitMonitorModel
                {
                    GameWinLimitMonitors = new List<GameWinLimitMonitorModel>()
                };

                foreach (var winlimit in group)
                {
                    playerWinLimitMonitorModel.PlayerInfo = playerWinLimitMonitorModel.PlayerInfo ?? winlimit.PlayerInfo;
                    playerWinLimitMonitorModel.Currency = playerWinLimitMonitorModel.Currency ?? winlimit.Currency;

                    var gameWinLimitMonitorModel = GameWinLimitMonitorModel.FromWinLimitMonitorEntity(winlimit);
                    gameWinLimitMonitorModel.Game.Name = gameHash[winlimit.Game.Id];
                    playerWinLimitMonitorModel.GameWinLimitMonitors.Add(gameWinLimitMonitorModel);
                }

                playerWinLimitMonitorModel.Currency.Name = currencyHash[group.Key.CurrencyId];
                playerWinLimitMonitors.PlayerWinLimitMonitorModels.Add(playerWinLimitMonitorModel);
            }

            return playerWinLimitMonitors;
        }

        public Result UnblockAccounts(UnblockAccountsCriteria criteria)
        {
            ResponseCode fpsResult;
            WinLimitMonitorEntity monitorEntity;
            IList<WinLimitMonitorEntity> monitorEntities = criteria.WinLimitMonitor;

            // 1. Call FPS to unblock first.
            // If NOT success: remove from original criteria.
            int unblockLength = monitorEntities.Count - 1;
            for (int i = unblockLength; i >= 0; i--)
            {
                monitorEntity = monitorEntities[i];
                fpsResult = _fpsClient.WinLimitControlUnblock(monitorEntity.PlayerInfo.Id, monitorEntity.PlayerInfo.Name, monitorEntity.Game.Name);
                if (fpsResult != ResponseCode.Allow)
                {
                    monitorEntities.RemoveAt(i);    
                }
            }

            // 2. Batch unblock to DB.
            var dbresult = _winLimitMonitorRepository.UnblockAccounts(criteria);
            if (dbresult.Code == 0)
            {
                dbresult.Message = 
                    dbresult.Code == 0 ? WinLimitControlLabels.WinLimitMonitor_UnblockAccountsSuccess : WinLimitControlLabels.InternalServerError;
            }

            return dbresult;
        }
    }
}
